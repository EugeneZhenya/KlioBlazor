using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace KlioWeb.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly ApplicationDbContext context;

        public MoviesRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<HomePageDTO> GetHomePageDTO()
        {
            var limitPopular = 9;
            var limitPartition = 12;
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            Random rnd = new Random();
            int randomID = rnd.Next(1, context.Movies.Count());

            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .Include(x => x.MovieInfos)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var lastCountries = movieLast.MoviesCountries.Select(x => x.Country).ToList();


            var movieRecomend = await context.Movies
               .Include(x => x.Partition).ThenInclude(x => x.Category)
               .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
               .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
               .Include(x => x.MovieInfos)
               .FirstOrDefaultAsync(x => x.Id == randomID);
            movieRecomend.MoviesGenres = movieRecomend.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieRecomend.MoviesCountries = movieRecomend.MoviesCountries.OrderBy(x => x.Order).ToList();
            var recCountries = movieRecomend.MoviesCountries.Select(x => x.Country).ToList();

            var allMoviesPopular = await context.Movies
                .OrderByDescending(x => x.ViewCounter)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .ToListAsync();

            var allPartitionsPopular = allMoviesPopular.GroupBy(x => x.Partition).Take(limitPartition).ToList();

            List<Partition> partitionsPopular = new List<Partition>();
            foreach (var part in allPartitionsPopular)
            {
                var newPart = context.Partitions
                    .Include(x => x.Category)
                    .Include(x => x.Movies)
                    .FirstOrDefault(x => x.Id == part.Key.Id);
                partitionsPopular.Add(newPart);
            }

            var moviesPopular = allMoviesPopular.Take(limitPopular).ToList();

            foreach (var film in moviesPopular)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var allLastMovies = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition)
                .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new HomePageDTO();
            response.LastMovie = movieLast;
            response.MoviesPopular = moviesPopular;
            response.LastMovieCountries = lastCountries;
            response.PartitionsPopular = partitionsPopular;
            response.RecomendMovie = movieRecomend;
            response.RecomendMovieCountries = recCountries;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<DetailsMovieDTO> GetDetailsMovieDTO(int id)
        {
            var movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Person)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .Include(x => x.MoviesCreators).ThenInclude(x => x.Creator)
                .Include(x => x.MoviesKeywords).ThenInclude(x => x.Keyword)
                .FirstOrDefaultAsync();

            if (movie == null) { return null; }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();
            movie.MoviesGenres = movie.MoviesGenres.OrderBy(x => x.Order).ToList();
            movie.MoviesCreators = movie.MoviesCreators.OrderBy(x => x.Order).ToList();
            movie.MoviesCountries = movie.MoviesCountries.OrderBy(x => x.Order).ToList();
            movie.MoviesKeywords = movie.MoviesKeywords.OrderBy(x => x.Order).ToList();

            var model = new DetailsMovieDTO();
            model.Movie = movie;
            model.MovieInfos = await context.MovieInfos.Where(x => x.MovieId == id).ToListAsync();
            model.Genres = movie.MoviesGenres.Select(x => x.Genre).ToList();
            model.Creators = movie.MoviesCreators.Select(x => x.Creator).ToList();
            model.Countries = movie.MoviesCountries.Select(x => x.Country).ToList();
            model.Keywords = movie.MoviesKeywords.Select(x => x.Keyword).ToList();
            model.Staff = movie.MoviesActors
                .Where(x => x.IsActor == false && x.IsTranslator == false).Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    HasPicture = x.Person.HasPicture,
                    IsFemale = x.Person.IsFemale,
                    Character = x.Character,
                    IsActor = x.IsActor,
                    IsTranslator = x.IsTranslator,
                    Id = x.PersonId

                }).ToList();
            model.Actors = movie.MoviesActors
                .Where(x => x.IsActor == true && x.IsTranslator == false).Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    HasPicture = x.Person.HasPicture,
                    IsFemale = x.Person.IsFemale,
                    Character = x.Character,
                    IsActor = x.IsActor,
                    IsTranslator = x.IsTranslator,
                    Id = x.PersonId

                }).ToList();
            model.TranslateStaff = movie.MoviesActors
                .Where(x => x.IsActor == false && x.IsTranslator == true).Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    HasPicture = x.Person.HasPicture,
                    IsFemale = x.Person.IsFemale,
                    Character = x.Character,
                    IsActor = x.IsActor,
                    IsTranslator = x.IsTranslator,
                    Id = x.PersonId

                }).ToList();
            model.TranslateActors = movie.MoviesActors
                .Where(x => x.IsActor == true && x.IsTranslator == true).Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    HasPicture = x.Person.HasPicture,
                    IsFemale = x.Person.IsFemale,
                    Character = x.Character,
                    IsActor = x.IsActor,
                    IsTranslator = x.IsTranslator,
                    Id = x.PersonId

                }).ToList();

            return model;
        }

        public async Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMoviesDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(filterMoviesDTO.Title));
            }

            if (filterMoviesDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable.Where(x => x.MoviesGenres.Select(y => y.GenreId).Contains(filterMoviesDTO.GenreId));
            }

            moviesQueryable = moviesQueryable.OrderByDescending(x => x.PublicDate);

            double count = await moviesQueryable.CountAsync();
            double totalAmountPages = Math.Ceiling(count / filterMoviesDTO.RecordsPerPage);

            var movies = await moviesQueryable
                .Paginate(filterMoviesDTO.Pagination)
                .Include(x => x.Partition)
                .ToListAsync();

            foreach (var film in movies)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            return new PaginatedResponse<List<Movie>>() { Response = movies, TotalAmountPages = (int)totalAmountPages };
        }
    }
}

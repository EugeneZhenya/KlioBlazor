using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.SSR.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.SSR.Repository
{
    public class MoviesRepository : IMoviesRepository
    {
        private ApplicationDbContext context { get; set; }
        private IFileStorageService fileStorageService;
        private string containerName = "Movies";

        public MoviesRepository(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        public async Task<HomePageDTO> GetHomePageDTO()
        {
            var limitPopular = 6;
            var limitPartition = 3;
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
                .Include(x => x.Partition)
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

        public async Task<List<Movie>> GetMoviesFiltered(FilterMoviesDTO filterMovieDTO)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterMovieDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(x => x.Title.Contains(filterMovieDTO.Title));
            }

            if (filterMovieDTO.GenreId != 0)
            {
                moviesQueryable = moviesQueryable.Where(x => x.MoviesGenres.Select(y => y.GenreId).Contains(filterMovieDTO.GenreId));
            }

            moviesQueryable = moviesQueryable.OrderByDescending(x => x.PublicDate);

            var movies = await moviesQueryable
                .Paginate(filterMovieDTO.Pagination)
                .Include(x => x.Partition)
                .ToListAsync();

            foreach (var film in movies)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            return movies;
        }

        public async Task<MovieUpdateDTO> GetMovieForUpdate(int id)
        {
            var movieActionResult = await GetDetailsMovieDTO(id);
            if (movieActionResult == null) { return null; }

            var movieDetailDTO = movieActionResult;
            var selectedGenresIds = movieDetailDTO.Genres.Select(x => x.Id).ToList();
            var notSelectedGenres = await context.Genres
                .Where(x => !selectedGenresIds.Contains(x.Id))
                .ToListAsync();
            var allPartitions = context.Partitions.ToList();
            var allCategories = context.Categories.ToList();
            var selectedCountriesIds = movieDetailDTO.Countries.Select(x => x.Id).ToList();
            var notSelectedCountries = await context.Countries
                .Where(x => !selectedCountriesIds.Contains(x.Id))
                .ToListAsync();

            var model = new MovieUpdateDTO();
            model.Movie = movieDetailDTO.Movie;
            model.SelectedGenres = movieDetailDTO.Genres;
            model.NotSelectedGenres = notSelectedGenres;
            model.Actors = movieDetailDTO.Staff;
            model.Actors.AddRange(movieDetailDTO.Actors);
            model.Actors.AddRange(movieDetailDTO.TranslateStaff);
            model.Actors.AddRange(movieDetailDTO.TranslateActors);
            model.AllPartitions = allPartitions;
            model.AllCategories = allCategories;
            model.MovieInfos = movieDetailDTO.Movie.MovieInfos.ToList();
            model.SelectedCountries = movieDetailDTO.Countries;
            model.NotSelectedCountries = notSelectedCountries;
            model.Keywords = movieDetailDTO.Keywords;
            model.Creators = movieDetailDTO.Creators;
            return model;
        }

        public async Task<int> CreateMovie(Movie movie)
        {
            movie.PublicDate = DateTime.Now;

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            if (movie.MoviesKeywords != null)
            {
                for (int i = 0; i < movie.MoviesKeywords.Count; i++)
                {
                    movie.MoviesKeywords[i].Order = i + 1;
                }
            }

            if (movie.MoviesCreators != null)
            {
                for (int i = 0; i < movie.MoviesCreators.Count; i++)
                {
                    movie.MoviesCreators[i].Order = i + 1;
                }
            }

            if (movie.MoviesCountries != null)
            {
                for (int i = 0; i < movie.MoviesCountries.Count; i++)
                {
                    movie.MoviesCountries[i].Order = i + 1;
                }
            }

            if (movie.MoviesGenres != null)
            {
                for (int i = 0; i < movie.MoviesGenres.Count; i++)
                {
                    movie.MoviesGenres[i].Order = i + 1;
                }
            }

            context.Add(movie);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var cover = Convert.FromBase64String(movie.Poster);
                var filePath = await fileStorageService.SaveFile(cover, "cover.jpg", $"{containerName}/{movie.Id}");
            }
            if (!string.IsNullOrWhiteSpace(movie.Background))
            {
                var background = Convert.FromBase64String(movie.Background);
                var filePath = await fileStorageService.SaveFile(background, "background.jpg", $"{containerName}/{movie.Id}");
            }

            return movie.Id;
        }

        public async Task UpdateMovie(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var cover = Convert.FromBase64String(movie.Poster);
                var filePath = await fileStorageService.SaveFile(cover, "cover.jpg", $"{containerName}/{movie.Id}");
            }
            if (!string.IsNullOrWhiteSpace(movie.Background))
            {
                var background = Convert.FromBase64String(movie.Background);
                var filePath = await fileStorageService.SaveFile(background, "background.jpg", $"{containerName}/{movie.Id}");
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"delete from MovieInfos where MovieId = {movie.Id}; delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesCountries where MovieId = {movie.Id}; delete from MoviesCreators where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}; delete from MoviesKeywords where MovieId = {movie.Id};");

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            if (movie.MoviesKeywords != null)
            {
                for (int i = 0; i < movie.MoviesKeywords.Count; i++)
                {
                    movie.MoviesKeywords[i].Order = i + 1;
                }
            }

            if (movie.MoviesCreators != null)
            {
                for (int i = 0; i < movie.MoviesCreators.Count; i++)
                {
                    movie.MoviesCreators[i].Order = i + 1;
                }
            }

            if (movie.MoviesCountries != null)
            {
                for (int i = 0; i < movie.MoviesCountries.Count; i++)
                {
                    movie.MoviesCountries[i].Order = i + 1;
                }
            }

            if (movie.MoviesGenres != null)
            {
                for (int i = 0; i < movie.MoviesGenres.Count; i++)
                {
                    movie.MoviesGenres[i].Order = i + 1;
                }
            }

            if (movie.MovieInfos != null)
            {
                foreach (var movieInfo in movie.MovieInfos)
                {
                    var addInfo = new MovieInfo { MovieId = movie.Id, Info = movieInfo.Info, Remark = movieInfo.Remark, Text = movieInfo.Text };
                    context.Add(addInfo);
                }
                movie.MovieInfos = null;
            }

            context.Attach(movie).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteMovie(int Id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == Id);
            if (movie == null)
            {
                return;
            }

            context.Remove(movie);
            await context.SaveChangesAsync();
        }
    }
}

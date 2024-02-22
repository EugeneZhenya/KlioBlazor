using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext context;

        public GenreRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Genre>> GetAllGenres()
        {
            return await context.Genres.ToListAsync();
        }

        public async Task<IndexGenresDTO> GetIndexGenresDTO()
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            var allGenres = await context.Genres
                .Include(x => x.MoviesGenres)
                .ToListAsync();

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new IndexGenresDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllGenres = allGenres;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<Genre> GetGenre(int Id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);
            if (genre == null) { return null; }
            return genre;
        }

        public async Task<DetailsGenreDTO> GetDetailsGenreDTO(FilterMoviesDTO filterMoviesDTO)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();
            moviesQueryable = moviesQueryable.OrderBy(x => x.ReleaseDate);

            var genre = await context.Genres
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == filterMoviesDTO.GenreId);

            if (genre == null) { return null; }

            var allMovieInGenre = await context.MoviesGenres.OrderByDescending(x => x.MovieId).Where(x => x.GenreId == filterMoviesDTO.GenreId).ToListAsync();
            var listOfIds = from n in allMovieInGenre where n.GenreId == filterMoviesDTO.GenreId select n.MovieId;
            int lastMovieId = allMovieInGenre[0].MovieId;

            var lsstMovie = await context.Movies
                .Where(x => x.Id == lastMovieId)
                .FirstOrDefaultAsync();

            double count = await (from m in moviesQueryable where listOfIds.Contains(m.Id) select m).CountAsync();
            var allMovies = await (from m in moviesQueryable where listOfIds.Contains(m.Id) select m)
                .Paginate(filterMoviesDTO.Pagination)
                .Include(x => x.Partition)
                .ToListAsync();

            foreach (var film in allMovies)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            double totalAmountPages = Math.Ceiling(count / filterMoviesDTO.RecordsPerPage);

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            var genreMovies = new PaginatedResponse<List<Movie>>() { Response = allMovies, TotalAmountPages = (int)totalAmountPages, TotalRecords = (int)count };

            var model = new DetailsGenreDTO();
            model.Genre = genre;
            model.LastMovie = lsstMovie;
            model.GenreMovies = genreMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}

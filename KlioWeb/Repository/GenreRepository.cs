using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
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

        public async Task<DetailsGenreDTO> GetDetailsGenreDTO(int Id)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var genre = await context.Genres
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (genre == null) { return null; }

            var allMovieInGenre = await context.MoviesGenres.OrderByDescending(x => x.MovieId).Where(x => x.GenreId == Id).ToListAsync();
            var listOfIds = from n in allMovieInGenre where n.GenreId == Id select n.MovieId;
            int lastMovieId = allMovieInGenre[0].MovieId;

            var lsstMovie = await context.Movies
                .Where(x => x.Id == lastMovieId)
                .FirstOrDefaultAsync();

            var allMovies = await (from m in context.Movies where listOfIds.Contains(m.Id) select m)
                .Include(x => x.Partition)
                .OrderBy(x => x.ReleaseDate)
                .ToListAsync();

            foreach (var film in allMovies)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            var model = new DetailsGenreDTO();
            model.Genre = genre;
            model.LastMovie = lsstMovie;
            model.GenreMovies = allMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}

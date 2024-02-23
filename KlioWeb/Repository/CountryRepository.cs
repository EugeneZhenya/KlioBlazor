using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationDbContext context;

        public CountryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<IndexCountriesDTO> GetIndexCountriesDTO()
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

            var allCountries = await context.Countries
                .Include(x => x.MoviesCountries)
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

            var response = new IndexCountriesDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCountries = allCountries;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<Country> GetCountry(int Id)
        {
            var country = await context.Countries.FirstOrDefaultAsync(x => x.Id == Id);
            if (country == null) { return null; }
            return country;
        }

        public async Task<DetailsCountryDTO> GetDetailsCountryDTO(FilterMoviesDTO filterMoviesDTO)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();
            moviesQueryable = moviesQueryable.OrderBy(x => x.ReleaseDate);

            var country = await context.Countries
                .Include(x => x.MoviesCountries)
                .FirstOrDefaultAsync(x => x.Id == filterMoviesDTO.CountryId);

            if (country == null) { return null; }

            var allMovieOfCountry = await context.MoviesCountries.OrderByDescending(x => x.MovieId).Where(x => x.CountryId == filterMoviesDTO.CountryId).ToListAsync();
            var listOfIds = from n in allMovieOfCountry where n.CountryId == filterMoviesDTO.CountryId select n.MovieId;

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

            var countryMovies = new PaginatedResponse<List<Movie>>() { Response = allMovies, TotalAmountPages = (int)totalAmountPages, TotalRecords = (int)count };

            var model = new DetailsCountryDTO();
            model.Country = country;
            model.CountryMoviesPage = countryMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}

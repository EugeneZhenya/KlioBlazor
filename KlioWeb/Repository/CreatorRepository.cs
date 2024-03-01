using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class CreatorRepository : ICreatorRepository
    {
        private readonly ApplicationDbContext context;

        public CreatorRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Creator>> GetAllCreators()
        {
            return await context.Creators.ToListAsync();
        }

        public async Task<IndexCreatorsDTO> GetIndexCreatorsDTO()
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

            var allCreators = await context.Creators
                .Include(x => x.MoviesCreators)
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

            var response = new IndexCreatorsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCreators = allCreators;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<Creator> GetCreator(int Id)
        {
            var creator = await context.Creators.FirstOrDefaultAsync(x => x.Id == Id);
            if (creator == null) { return null; }
            return creator;
        }

        public async Task<List<Creator>> GetCreatorByTitle(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Creator>(); }
            return await context.Creators.Where(x => x.Title.Contains(searchText)).Take(5).ToListAsync();
        }

        public async Task<DetailsCreatorDTO> GetDetailsCreatorDTO(FilterMoviesDTO filterMoviesDTO)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();
            moviesQueryable = moviesQueryable.OrderBy(x => x.ReleaseDate);

            var creator = await context.Creators
                .Include(x => x.MoviesCreators)
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == filterMoviesDTO.CreatorId);

            if (creator == null) { return null; }

            var allMovieOfCreator = await context.MoviesCreators.OrderByDescending(x => x.MovieId).Where(x => x.CreatorID == filterMoviesDTO.CreatorId).ToListAsync();
            var listOfIds = from n in allMovieOfCreator where n.CreatorID == filterMoviesDTO.CreatorId select n.MovieId;
            int lastMovieId = allMovieOfCreator[0].MovieId;

            var lsstMovie = await context.Movies
               .Where(x => x.Id == lastMovieId)
               .FirstOrDefaultAsync();

            double count = await (from m in moviesQueryable where listOfIds.Contains(m.Id) select m).CountAsync();
            var allMovies = await (from m in context.Movies where listOfIds.Contains(m.Id) select m)
                .Paginate(filterMoviesDTO.Pagination)
                .Include(x => x.Partition)
                .OrderBy(x => x.ReleaseDate)
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

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var creatorMovies = new PaginatedResponse<List<Movie>>() { Response = allMovies, TotalAmountPages = (int)totalAmountPages, TotalRecords = (int)count };

            var model = new DetailsCreatorDTO();
            model.Creator = creator;
            model.LastMovie = lsstMovie;
            model.CreatorMoviesPage = creatorMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}

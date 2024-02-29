using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class KeywordRepository : IKeywordRepository
    {
        private readonly ApplicationDbContext context;

        public KeywordRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Keyword>> GetAllKeywords()
        {
            return await context.Keywords.ToListAsync();
        }

        public async Task<List<Keyword>> GetKeywordByWord(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Keyword>(); }
            return await context.Keywords.Where(x => x.Word.Contains(searchText)).Take(5).ToListAsync();
        }

        public async Task<IndexKeywordsDTO> GetIndexKeywordsDTO()
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .Include(x => x.MovieInfos)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            var allKeywords = await context.Keywords.ToListAsync();

            var allLastMovies = await context.Movies
                   .OrderByDescending(x => x.PublicDate)
                   .Include(x => x.Partition).ThenInclude(x => x.Category)
                   .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new IndexKeywordsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllKeywords = allKeywords;
            response.LastAdded = moviesLast;

            return response;
        }

        public async Task<Keyword> GetKeyword(int Id)
        {
            var keyword = await context.Keywords.FirstOrDefaultAsync(x => x.Id == Id);
            if (keyword == null) { return null; }
            return keyword;
        }

        public async Task<DetailsKeywordDTO> GetDetailsKeywordDTO(FilterMoviesDTO filterMoviesDTO)
        {
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);
            var moviesQueryable = context.Movies.AsQueryable();
            moviesQueryable = moviesQueryable.OrderBy(x => x.ReleaseDate);

            var keyword = await context.Keywords
                .Include(x => x.MoviesKeywords)
                .FirstOrDefaultAsync(x => x.Id == filterMoviesDTO.KeywordId);

            if (keyword == null) { return null; }

            var allMovieByKeyword = await context.MoviesKeywords.OrderByDescending(x => x.MovieId).Where(x => x.KeywordId == filterMoviesDTO.KeywordId).ToListAsync();
            var listOfIds = from n in allMovieByKeyword where n.KeywordId == filterMoviesDTO.KeywordId select n.MovieId;
            int lastMovieId = allMovieByKeyword[0].MovieId;

            var lsstMovie = await context.Movies
                .Where(x => x.Id == lastMovieId)
                .FirstOrDefaultAsync();

            double count = await (from m in moviesQueryable where listOfIds.Contains(m.Id) select m).CountAsync();
            var allMovies = await (from m in context.Movies where listOfIds.Contains(m.Id) select m)
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

            var keywordMovies = new PaginatedResponse<List<Movie>>() { Response = allMovies, TotalAmountPages = (int)totalAmountPages, TotalRecords = (int)count };

            var model = new DetailsKeywordDTO();
            model.Keyword = keyword;
            model.LastMovie = lsstMovie;
            model.KeywordMoviesPage = keywordMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}

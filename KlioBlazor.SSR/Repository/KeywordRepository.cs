using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioBlazor.SSR.Repository
{
    public class KeywordRepository : IKeywordRepository
    {
        private ApplicationDbContext context { get; set; }

        public KeywordRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Keyword>> GetAllKeywords()
        {
            return await context.Keywords.ToListAsync();
        }

        public async Task<List<Keyword>> GetKeywordByWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) { return new List<Keyword>(); }
            return await context.Keywords.Where(x => x.Word.Contains(word)).Take(5).ToListAsync();
        }

        public async Task<IndexKeywordsDTO> GetIndexKeywordsDTO()
        {
            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            var allKeywords = await context.Keywords.ToListAsync();

            var response = new IndexKeywordsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllKeywords = allKeywords;

            return response;
        }

        public async Task<Keyword> GetKeyword(int Id)
        {
            var keyword = await context.Keywords.FirstOrDefaultAsync(x => x.Id == Id);
            if (keyword == null) { return null; }
            return keyword;
        }

        public async Task CreateKeyword(Keyword keyword)
        {
            context.Add(keyword);
            await context.SaveChangesAsync();
        }

        public async Task UpdateKeyword(Keyword keyword)
        {
            context.Attach(keyword).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteKeyword(int Id)
        {
            var keyword = await context.Keywords.FirstOrDefaultAsync(x => x.Id == Id);
            if (keyword == null)
            {
                return;
            }

            context.Remove(keyword);
            await context.SaveChangesAsync();
        }
    }
}

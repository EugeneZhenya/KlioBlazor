using KlioBlazor.Data;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeywordsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public KeywordsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Keyword>>> GetAllKeywords()
        {
            return await context.Keywords.ToListAsync();
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Keyword>>> FilterByWord(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Keyword>(); }
            return await context.Keywords.Where(x => x.Word.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IndexKeywordsDTO>> Get()
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

        [HttpGet("{id}")]
        public async Task<ActionResult<Keyword>> Get(int id)
        {
            var keyword = await context.Keywords.FirstOrDefaultAsync(x => x.Id == id);
            if (keyword == null) { return NotFound(); }
            return keyword;
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<DetailsKeywordDTO>> GetKeywordDetails(int id)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var keyword = await context.Keywords
                .Include(x => x.MoviesKeywords)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (keyword == null) { return NotFound(); }

            var allMovieByKeyword = await context.MoviesKeywords.OrderByDescending(x => x.MovieId).Where(x => x.KeywordId == id).ToListAsync();
            var listOfIds = from n in allMovieByKeyword where n.KeywordId == id select n.MovieId;
            int lastMovieId = allMovieByKeyword[0].MovieId;

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

            var model = new DetailsKeywordDTO();
            model.Keyword = keyword;
            model.LastMovie = lsstMovie;
            model.KeywordMovies = allMovies;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Keyword keyword)
        {
            context.Add(keyword);
            await context.SaveChangesAsync();
            return keyword.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Keyword keyword)
        {
            context.Attach(keyword).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var keyword = await context.Keywords.FirstOrDefaultAsync(x => x.Id == id);
            if (keyword == null)
            {
                return NotFound();
            }

            context.Remove(keyword);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

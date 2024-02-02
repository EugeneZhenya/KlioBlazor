using KlioBlazor.Data;
using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreatorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private string containerName = "creators";

        public CreatorsController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Creator>>> GetAllCreators()
        {
            return await context.Creators.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IndexCreatorsDTO>> Get()
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

            var allCreators = await context.Creators
                .Include(x => x.MoviesCreators)
                .ToListAsync();

            var response = new IndexCreatorsDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCreators = allCreators;

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Creator>> Get(int id)
        {
            var creator = await context.Creators.FirstOrDefaultAsync(x => x.Id == id);
            if (creator == null) { return NotFound(); }
            return creator;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Creator>>> FilterByTitle(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Creator>(); }
            return await context.Creators.Where(x => x.Title.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<DetailsCreatorDTO>> GetCreatorDetails(int id)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var creator = await context.Creators
                .Include(x => x.MoviesCreators)
                .Include(x => x.Country)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (creator == null) { return NotFound(); }

            var allMovieOfCreator = await context.MoviesCreators.OrderByDescending(x => x.MovieId).Where(x => x.CreatorID == id).ToListAsync();
            var listOfIds = from n in allMovieOfCreator where n.CreatorID == id select n.MovieId;
            int lastMovieId = allMovieOfCreator[0].MovieId;

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

            var model = new DetailsCreatorDTO();
            model.Creator = creator;
            model.LastMovie = lsstMovie;
            model.CreatorMovies = allMovies;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Creator creator)
        {
            context.Add(creator);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(creator.Logo))
            {
                var creatorLogo = Convert.FromBase64String(creator.Logo);
                var filePath = await fileStorageService.SaveFile(creatorLogo, creator.LogoUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return creator.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Creator creator)
        {
            if (!string.IsNullOrWhiteSpace(creator.Logo))
            {
                var creatorLogo = Convert.FromBase64String(creator.Logo);
                var filePath = await fileStorageService.SaveFile(creatorLogo, creator.LogoUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            context.Attach(creator).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var creator = await context.Creators.FirstOrDefaultAsync(x => x.Id == id);
            if (creator == null)
            {
                return NotFound();
            }

            context.Remove(creator);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

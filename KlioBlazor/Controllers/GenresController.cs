using KlioBlazor.Components.Shared;
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
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private string containerName = "genres";

        public GenresController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Genre>>> GetAllGenres()
        {
            return await context.Genres.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IndexGenresDTO>> Get()
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

            var allGenres = await context.Genres
                .Include(x => x.MoviesGenres)
                .ToListAsync();

            var response = new IndexGenresDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllGenres = allGenres;

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null) { return NotFound(); }
            return genre;
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<DetailsGenreDTO>> GetGenreDetails(int id)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var genre = await context.Genres
                .Include(x => x.MoviesGenres)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (genre == null) { return NotFound(); }

            var allMovieInGenre = await context.MoviesGenres.OrderByDescending(x => x.MovieId).Where(x => x.GenreId == id).ToListAsync();
            var listOfIds = from n in allMovieInGenre where n.GenreId == id select n.MovieId;
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

            var model = new DetailsGenreDTO();
            model.Genre = genre;
            model.LastMovie = lsstMovie;
            model.GenreMovies = allMovies;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(genre.Icon))
            {
                var genreIcon = Convert.FromBase64String(genre.Icon);
                var filePath = await fileStorageService.SaveFile(genreIcon, genre.IconUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return genre.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Genre genre)
        {
            if (!string.IsNullOrWhiteSpace(genre.Icon))
            {
                var genreIcon = Convert.FromBase64String(genre.Icon);
                var filePath = await fileStorageService.SaveFile(genreIcon, genre.IconUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            context.Attach(genre).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            context.Remove(genre);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

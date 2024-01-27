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
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private string containerName = "countries";

        public CountriesController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<Country>>> GetAllCountries()
        {
            return await context.Countries.ToListAsync();
        }

        [HttpGet]
        public async Task<ActionResult<IndexCountriesDTO>> Get()
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

            var allCountries = await context.Countries
                .Include(x => x.MoviesCountries)
                .ToListAsync();

            var response = new IndexCountriesDTO();
            response.LastMovie = movieLast;
            response.LastMovieCountries = Countries;
            response.AllCountries = allCountries;

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> Get(int id)
        {
            var country = await context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null) { return NotFound(); }
            return country;
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<DetailsCountryDTO>> GetCountryDetails(int id)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var country = await context.Countries
                .Include(x => x.MoviesCountries)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (country == null) { return NotFound(); }

            var allMovieOfCountry = await context.MoviesCountries.OrderByDescending(x => x.MovieId).Where(x => x.CountryId == id).ToListAsync();
            var listOfIds = from n in allMovieOfCountry where n.CountryId == id select n.MovieId;

            var allMovies = await (from m in context.Movies where listOfIds.Contains(m.Id) select m)
                .Include(x => x.Partition)
                .OrderBy(x => x.ReleaseDate)
                .ToListAsync();


            foreach (var film in allMovies)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var model = new DetailsCountryDTO();
            model.Country = country;
            model.CountryMovies = allMovies;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Country country)
        {
            context.Add(country);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(country.Flag))
            {
                var countryFlag = Convert.FromBase64String(country.Flag);
                var filePath = await fileStorageService.SaveFile(countryFlag, country.FlagUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!string.IsNullOrWhiteSpace(country.Emblem))
            {
                var countryEmblem = Convert.FromBase64String(country.Emblem);
                var filePath = await fileStorageService.SaveFile(countryEmblem, country.EmblemUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!string.IsNullOrWhiteSpace(country.Background))
            {
                var countryBackground = Convert.FromBase64String(country.Background);
                var filePath = await fileStorageService.SaveFile(countryBackground, country.BackgroundUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return country.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Country country)
        {
            if (!string.IsNullOrWhiteSpace(country.Flag))
            {
                var countryFlag = Convert.FromBase64String(country.Flag);
                var filePath = await fileStorageService.SaveFile(countryFlag, country.FlagUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!string.IsNullOrWhiteSpace(country.Emblem))
            {
                var countryEmblem = Convert.FromBase64String(country.Emblem);
                var filePath = await fileStorageService.SaveFile(countryEmblem, country.EmblemUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!string.IsNullOrWhiteSpace(country.Background))
            {
                var countryBackground = Convert.FromBase64String(country.Background);
                var filePath = await fileStorageService.SaveFile(countryBackground, country.BackgroundUrl, containerName);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            context.Attach(country).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var country = await context.Countries.FirstOrDefaultAsync(x => x.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            context.Remove(country);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

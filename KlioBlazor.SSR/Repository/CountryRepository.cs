using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.SSR.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioBlazor.SSR.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private ApplicationDbContext context { get; set; }
        private IFileStorageService fileStorageService;
        private string containerName = "countries";

        public CountryRepository(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await context.Countries.ToListAsync();
        }

        public async Task<IndexCountriesDTO> GetIndexCountriesDTO()
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

        public async Task<Country> GetCountry(int Id)
        {
            var country = await context.Countries.FirstOrDefaultAsync(x => x.Id == Id);
            if (country == null) { return null; }
            return country;
        }

        public async Task CreateCountry(Country country)
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
        }

        public async Task UpdateCountry(Country country)
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
        }

        public async Task DeleteCountry(int Id)
        {
            var country = await context.Countries.FirstOrDefaultAsync(x => x.Id == Id);
            if (country == null)
            {
                return;
            }

            context.Remove(country);
            await context.SaveChangesAsync();
        }
    }
}

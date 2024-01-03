using KlioBlazor.Data;
using KlioBlazor.Helpers;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        public CountriesController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Country country)
        {
            context.Add(country);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(country.Flag))
            {
                var countryFlag = Convert.FromBase64String(country.Flag);
                var filePath = await fileStorageService.SaveFile(countryFlag, country.FlagUrl, "countries");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!string.IsNullOrWhiteSpace(country.Emblem))
            {
                var countryEmblem = Convert.FromBase64String(country.Emblem);
                var filePath = await fileStorageService.SaveFile(countryEmblem, country.EmblemUrl, "countries");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (!string.IsNullOrWhiteSpace(country.Background))
            {
                var countryBackground = Convert.FromBase64String(country.Background);
                var filePath = await fileStorageService.SaveFile(countryBackground, country.BackgroundUrl, "countries");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return country.Id;
        }
    }
}

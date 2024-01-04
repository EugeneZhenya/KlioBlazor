using KlioBlazor.Data;
using KlioBlazor.Helpers;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        public GenresController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(genre.Icon))
            {
                var genreIcon = Convert.FromBase64String(genre.Icon);
                var filePath = await fileStorageService.SaveFile(genreIcon, genre.IconUrl, "genres");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return genre.Id;
        }
    }
}

using KlioBlazor.Data;
using KlioBlazor.Helpers;
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

        public CreatorsController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Creator>>> Get()
        {
            return await context.Creators.ToListAsync();
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Creator>>> FilterByTitle(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Creator>(); }
            return await context.Creators.Where(x => x.Title.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Creator creator)
        {
            context.Add(creator);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(creator.Logo))
            {
                var creatorLogo = Convert.FromBase64String(creator.Logo);
                var filePath = await fileStorageService.SaveFile(creatorLogo, creator.LogoUrl, "creators");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(filePath);
                Console.ForegroundColor = ConsoleColor.White;
            }

            return creator.Id;
        }
    }
}

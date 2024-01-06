using KlioBlazor.Data;
using KlioBlazor.Helpers;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        public MoviesController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            context.Add(movie);
            await context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var cover = Convert.FromBase64String(movie.Poster);
                var filePath = await fileStorageService.SaveFile(cover, "cover.jpg", "Movies/" + movie.Id.ToString());
            }
            if (!string.IsNullOrWhiteSpace(movie.Background))
            {
                var background = Convert.FromBase64String(movie.Background);
                var filePath = await fileStorageService.SaveFile(background, "background.jpg", "Movies/" + movie.Id.ToString());
            }

            return movie.Id;
        }
    }
}

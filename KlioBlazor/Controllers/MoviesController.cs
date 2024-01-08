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
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;

        public MoviesController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limit = 6;

            var movieLast = await context.Movies.OrderByDescending(x => x.PublicDate).FirstOrDefaultAsync();
            var moviesPopular = await context.Movies.OrderByDescending(x => x.ViewCounter).Take(limit).ToListAsync();

            var response = new HomePageDTO();
            response.LastMovie = movieLast;
            response.MoviesPopular = moviesPopular;

            return response;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Movie movie)
        {
            movie.PublicDate = DateTime.Now;

            if (movie.MoviesActors != null)
            {
                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            if (movie.MoviesKeywords != null)
            {
                for (int i = 0; i < movie.MoviesKeywords.Count; i++)
                {
                    movie.MoviesKeywords[i].Order = i + 1;
                }
            }

            if (movie.MoviesCreators != null)
            {
                for (int i = 0; i < movie.MoviesCreators.Count; i++)
                {
                    movie.MoviesCreators[i].Order = i + 1;
                }
            }

            if (movie.MoviesCountries != null)
            {
                for (int i = 0; i < movie.MoviesCountries.Count; i++)
                {
                    movie.MoviesCountries[i].Order = i + 1;
                }
            }

            if (movie.MoviesGenres != null)
            {
                for (int i = 0; i < movie.MoviesGenres.Count; i++)
                {
                    movie.MoviesGenres[i].Order = i + 1;
                }
            }

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

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
        private string containerName = "Movies";

        public MoviesController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<HomePageDTO>> Get()
        {
            var limit = 6;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var movieLast = await context.Movies
                .OrderByDescending(x => x.PublicDate)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .FirstOrDefaultAsync();

            movieLast.MoviesGenres = movieLast.MoviesGenres.OrderBy(x => x.Order).ToList();
            movieLast.MoviesCountries = movieLast.MoviesCountries.OrderBy(x => x.Order).ToList();
            var Countries = movieLast.MoviesCountries.Select(x => x.Country).ToList();

            var moviesPopular = await context.Movies
                .OrderByDescending(x => x.ViewCounter)
                .Include(x => x.Partition)
                .Take(limit)
                .ToListAsync();

            foreach (var film in moviesPopular)
            {
                film.Rating = Math.Truncate((double)film.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var response = new HomePageDTO();
            response.LastMovie = movieLast;
            response.MoviesPopular = moviesPopular;
            response.LastMovieCountries = Countries;

            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetailsMovieDTO>> Get(int id)
        {
            var movie = await context.Movies.Where(x => x.Id == id)
                .Include(x => x.Partition).ThenInclude(x => x.Category)
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Person)
                .Include(x => x.MoviesCountries).ThenInclude(x => x.Country)
                .Include(x => x.MoviesCreators).ThenInclude(x => x.Creator)
                .Include(x => x.MoviesKeywords).ThenInclude(x => x.Keyword)
                .Include(x => x.MovieInfos)
                .FirstOrDefaultAsync();

            if (movie == null) { return NotFound(); }

            movie.MoviesActors = movie.MoviesActors.OrderBy(x => x.Order).ToList();
            movie.MoviesGenres = movie.MoviesGenres.OrderBy(x => x.Order).ToList();
            movie.MoviesCreators = movie.MoviesCreators.OrderBy(x => x.Order).ToList();
            movie.MoviesCountries = movie.MoviesCountries.OrderBy(x => x.Order).ToList();
            movie.MoviesKeywords = movie.MoviesKeywords.OrderBy(x => x.Order).ToList();

            var model = new DetailsMovieDTO();
            model.Movie = movie;
            model.Genres = movie.MoviesGenres.Select(x => x.Genre).ToList();
            model.Creators = movie.MoviesCreators.Select(x => x.Creator).ToList();
            model.Countries = movie.MoviesCountries.Select(x => x.Country).ToList();
            model.Keywords = movie.MoviesKeywords.Select(x => x.Keyword).ToList();
            model.Actors = movie.MoviesActors.Select(x =>
                new Person
                {
                    Name = x.Person.Name,
                    HasPicture = x.Person.HasPicture,
                    IsFemale = x.Person.IsFemale,
                    Character = x.Character,
                    IsActor = x.IsActor,
                    IsTranslator = x.IsTranslator,
                    Id = x.PersonId

                }).ToList();


            return model;
        }

        [HttpGet("update/{id}")]
        public async Task<ActionResult<MovieUpdateDTO>> PutGet(int id)
        {
            var movieActionResult = await Get(id);
            if (movieActionResult.Result is NotFoundResult) { return NotFound(); }

            var movieDetailDTO = movieActionResult.Value;
            var selectedGenresIds = movieDetailDTO.Genres.Select(x => x.Id).ToList();
            var notSelectedGenres = await context.Genres
                .Where(x => !selectedGenresIds.Contains(x.Id))
                .ToListAsync();
            var allPartitions = context.Partitions.ToList();
            var allCategories = context.Categories.ToList();
            var selectedCountriesIds = movieDetailDTO.Countries.Select(x => x.Id).ToList();
            var notSelectedCountries = await context.Countries
                .Where(x => !selectedCountriesIds.Contains(x.Id))
                .ToListAsync();

            var model = new MovieUpdateDTO();
            model.Movie = movieDetailDTO.Movie;
            model.SelectedGenres = movieDetailDTO.Genres;
            model.NotSelectedGenres = notSelectedGenres;
            model.Actors = movieDetailDTO.Actors;
            model.AllPartitions = allPartitions;
            model.AllCategories = allCategories;
            model.MovieInfos = movieDetailDTO.Movie.MovieInfos.ToList();
            model.SelectedCountries = movieDetailDTO.Countries;
            model.NotSelectedCountries = notSelectedCountries;
            model.Keywords = movieDetailDTO.Keywords;
            model.Creators = movieDetailDTO.Creators;
            return model;
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
                var filePath = await fileStorageService.SaveFile(cover, "cover.jpg", $"{containerName}/{movie.Id}");
            }
            if (!string.IsNullOrWhiteSpace(movie.Background))
            {
                var background = Convert.FromBase64String(movie.Background);
                var filePath = await fileStorageService.SaveFile(background, "background.jpg", $"{containerName}/{movie.Id}");
            }

            return movie.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Movie movie)
        {
            if (!string.IsNullOrWhiteSpace(movie.Poster))
            {
                var cover = Convert.FromBase64String(movie.Poster);
                var filePath = await fileStorageService.SaveFile(cover, "cover.jpg", $"{containerName}/{movie.Id}");
            }
            if (!string.IsNullOrWhiteSpace(movie.Background))
            {
                var background = Convert.FromBase64String(movie.Background);
                var filePath = await fileStorageService.SaveFile(background, "background.jpg", $"{containerName}/{movie.Id}");
            }

            await context.Database.ExecuteSqlInterpolatedAsync($"delete from MovieInfos where MovieId = {movie.Id}; delete from MoviesActors where MovieId = {movie.Id}; delete from MoviesCountries where MovieId = {movie.Id}; delete from MoviesCreators where MovieId = {movie.Id}; delete from MoviesGenres where MovieId = {movie.Id}; delete from MoviesKeywords where MovieId = {movie.Id};");

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

            if (movie.MovieInfos != null)
            {
                foreach (var movieInfo in movie.MovieInfos)
                {
                    var addInfo = new MovieInfo { MovieId = movie.Id, Info = movieInfo.Info, Remark = movieInfo.Remark, Text = movieInfo.Text };
                    context.Add(addInfo);
                }
                movie.MovieInfos = null;
            }

            context.Attach(movie).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            context.Remove(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

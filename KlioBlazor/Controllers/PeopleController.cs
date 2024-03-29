using KlioBlazor.Data;
using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace KlioBlazor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IFileStorageService fileStorageService;
        private string containerName = "people";

        public PeopleController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.People.AsQueryable();
            await HttpContext.InsertPaginationParametersInResponse(queryable, paginationDTO.RecordsPerPage);
            return await queryable.OrderBy(q => q.Name).Paginate(paginationDTO).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) { return NotFound(); }
            return person;
        }

        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<List<Person>>> FilterByName(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText)) { return new List<Person>(); }
            return await context.People.Where(x => x.Name.Contains(searchText) || x.Biography.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<DetailsPersonDTO>> GetPersonDetails(int id)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var person = await context.People
                .Include(x => x.MoviesActors)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (person == null) { return NotFound(); }

            var allMovieByPerson = await context.MoviesActors.OrderByDescending(x => x.MovieId).Where(x => x.PersonId == id).ToListAsync();
            var listOfIds = from n in allMovieByPerson where n.PersonId == id select n.MovieId;
            int lastMovieId = allMovieByPerson[0].MovieId;

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

            var model = new DetailsPersonDTO();
            model.Person = person;
            model.LastMovie = lsstMovie;
            model.PersonMovies = allMovies;

            return model;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            context.Add(person);
            await context.SaveChangesAsync();

            if (person.HasPicture)
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                var filePath = await fileStorageService.SaveFile(personPicture, person.PictureUrl, containerName);
            }

            return person.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                var filePath = await fileStorageService.SaveFile(personPicture, person.PictureUrl, containerName);
                person.HasPicture = true;
            }

            context.Attach(person).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null)
            {
                return NotFound();
            }

            context.Remove(person);
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

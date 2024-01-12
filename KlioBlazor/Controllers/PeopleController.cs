using KlioBlazor.Data;
using KlioBlazor.Helpers;
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

        public PeopleController(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> Get()
        {
            return await context.People.ToListAsync();
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
            return await context.People.Where(x => x.Name.Contains(searchText)).Take(5).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                var filePath = await fileStorageService.SaveFile(personPicture, person.PictureUrl, "people");
                person.HasPicture = true;
            }

            context.Add(person);
            await context.SaveChangesAsync();

            return person.Id;
        }

        [HttpPut]
        public async Task<ActionResult> Put(Person person)
        {
            context.Attach(person).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}

using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.SSR.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace KlioBlazor.SSR.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private ApplicationDbContext context { get; set; }
        private IFileStorageService fileStorageService;
        private string containerName = "people";

        public PersonRepository(ApplicationDbContext context, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.fileStorageService = fileStorageService;
        }

        public async Task<List<Person>> GetPeople(PaginationDTO paginationDTO)
        {
            var queryable = context.People.AsQueryable();
            return await queryable.OrderBy(q => q.Name).Paginate(paginationDTO).ToListAsync();
        }

        public async Task<List<Person>> GetPeopleByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return new List<Person>(); }
            return await context.People.Where(x => x.Name.Contains(name)).Take(5).ToListAsync();
        }

        public async Task<Person> GetPersonById(int id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == id);
            if (person == null) { return null; }
            return person;
        }

        public async Task CreatePerson(Person person)
        {
            context.Add(person);
            await context.SaveChangesAsync();

            if (person.HasPicture)
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                var filePath = await fileStorageService.SaveFile(personPicture, person.PictureUrl, containerName);
            }
        }

        public async Task UpdatePerson(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Picture))
            {
                var personPicture = Convert.FromBase64String(person.Picture);
                var filePath = await fileStorageService.SaveFile(personPicture, person.PictureUrl, containerName);
                person.HasPicture = true;
            }

            context.Attach(person).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeletePerson(int Id)
        {
            var person = await context.People.FirstOrDefaultAsync(x => x.Id == Id);
            if (person == null)
            {
                return;
            }

            context.Remove(person);
            await context.SaveChangesAsync();
        }
    }
}

using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Data;
using KlioWeb.Helpers;
using Microsoft.EntityFrameworkCore;
using System;

namespace KlioWeb.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext context;

        public PersonRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO paginationDTO)
        {
            var queryable = context.People.OrderBy(q => q.Name).AsQueryable();
            double count = await queryable.CountAsync();
            double totalAmountPages = Math.Ceiling(count / paginationDTO.RecordsPerPage);

            var allPeople = await queryable.Paginate(paginationDTO).ToListAsync();
            return new PaginatedResponse<List<Person>>() { Response = allPeople, TotalAmountPages = (int)totalAmountPages, TotalRecords = (int)count };
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

        public async Task<DetailsPersonDTO> GetDetailsPersonDTO(int Id)
        {
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var person = await context.People
                .Include(x => x.MoviesActors)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (person == null) { return null; }

            var allMovieByPerson = await context.MoviesActors.OrderByDescending(x => x.MovieId).Where(x => x.PersonId == Id).ToListAsync();
            var listOfIds = from n in allMovieByPerson where n.PersonId == Id select n.MovieId;
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
    }
}

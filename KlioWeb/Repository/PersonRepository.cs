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

        public async Task<IndexPersonDTO> GetIndexPeople(IndexPersonDTO indexPersonDTO)
        {
            var DayofToday = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            // var DayofToday = new DateTime(DateTime.Today.Year, 2, 28);

            var queryable = context.People.OrderBy(q => q.Name).AsQueryable();
            double count = await queryable.CountAsync();
            double totalAmountPages = Math.Ceiling(count / indexPersonDTO.RecordsPerPage);

            var allPeople = await queryable.Paginate(indexPersonDTO.Pagination).ToListAsync();

            var Jubilyars = await context.People
                .Where(x => x.DateOfBirthExact == true)
                .OrderBy(x => x.DateOfBirth)
                .Where(x => x.DateOfBirth.HasValue)
                .Where(x => x.DateOfBirth.Value.Day == DayofToday.Day && x.DateOfBirth.Value.Month == DayofToday.Month)
                .ToListAsync();

            var Memories = await context.People
                .Where(x => x.DateOfDeathExact == true)
                .OrderBy(x => x.DateOfDeath)
                .Where(x => x.DateOfDeath.HasValue)
                .Where(x => x.DateOfDeath.Value.Day == DayofToday.Day && x.DateOfDeath.Value.Month == DayofToday.Month)
                .ToListAsync();

            return new IndexPersonDTO()
            {
                PeopleList = allPeople,
                TotalAmountPages = (int)totalAmountPages,
                TotalRecords = (int)count,
                Jubilees = Jubilyars,
                Memorials = Memories
            };
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
            int lastMovieId = 0;
            var limitLasts = 3;
            double maxViews = (double)context.Movies.Max(p => p.ViewCounter);

            var person = await context.People
                .Include(x => x.MoviesActors)
                .FirstOrDefaultAsync(x => x.Id == Id);

            if (person == null) { return null; }

            var allMovieByPerson = await context.MoviesActors.OrderByDescending(x => x.MovieId).Where(x => x.PersonId == Id).ToListAsync();
            var listOfIds = from n in allMovieByPerson where n.PersonId == Id select n.MovieId;
            if (allMovieByPerson.Count > 0)
            {
                lastMovieId = allMovieByPerson[0].MovieId;
            }

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

            var allLastMovies = await context.Movies
                    .OrderByDescending(x => x.PublicDate)
                    .Include(x => x.Partition).ThenInclude(x => x.Category)
                    .ToListAsync();

            var moviesLast = allLastMovies.Take(limitLasts).ToList();

            foreach (var movie in moviesLast)
            {
                movie.Rating = Math.Truncate((double)movie.ViewCounter / (double)maxViews * 10000) / 100;
            }

            var model = new DetailsPersonDTO();
            model.Person = person;
            model.LastMovie = lsstMovie;
            model.PersonMovies = allMovies;
            model.LastAdded = moviesLast;

            return model;
        }
    }
}

using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/people";

        public PersonRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO paginationDTO)
        {
            return await httpService.GetHelper<List<Person>>(url, paginationDTO);
        }

        public async Task<List<Person>> GetPeopleByName(string name)
        {
            return await httpService.GetHelper<List<Person>>($"{url}/search/{name}");
        }

        public async Task<Person> GetPersonById(int id)
        {
            return await httpService.GetHelper<Person>($"{url}/{id}");
        }

        public async Task<DetailsPersonDTO> GetDetailsPersonDTO(int Id)
        {
            return await httpService.GetHelper<DetailsPersonDTO>($"{url}/details/{Id}");
        }

        public async Task CreatePerson(Person person)
        {
            var response = await httpService.Post(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task UpdatePerson(Person person)
        {
            var response = await httpService.Put(url, person);
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }

        public async Task DeletePerson(int Id)
        {
            var response = await httpService.Delete($"{url}/{Id}");
            if (!response.Success)
            {
                throw new ApplicationException(await response.GetBody());
            }
        }
    }
}

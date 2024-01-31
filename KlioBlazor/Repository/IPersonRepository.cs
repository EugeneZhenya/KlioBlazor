using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IPersonRepository
    {
        Task CreatePerson(Person person);
        Task DeletePerson(int Id);
        Task<DetailsPersonDTO> GetDetailsPersonDTO(int Id);
        Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO paginationDTO);
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPersonById(int id);
        Task UpdatePerson(Person person);
    }
}

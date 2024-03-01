using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface IPersonRepository
    {
        Task<DetailsPersonDTO> GetDetailsPersonDTO(int Id);
        Task<IndexPersonDTO> GetIndexPeople(IndexPersonDTO indexPersonDTO);
        Task<PaginatedResponse<List<Person>>> GetPeople(PaginationDTO paginationDTO);
        Task<List<Person>> GetPeopleByName(string name);
        Task<Person> GetPersonById(int id);
    }
}

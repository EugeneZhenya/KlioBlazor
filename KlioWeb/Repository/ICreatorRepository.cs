using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface ICreatorRepository
    {
        Task<List<Creator>> GetAllCreators();
        Task<Creator> GetCreator(int Id);
        Task<List<Creator>> GetCreatorByTitle(string searchText);
        Task<DetailsCreatorDTO> GetDetailsCreatorDTO(FilterMoviesDTO filterMoviesDTO);
        Task<IndexCreatorsDTO> GetIndexCreatorsDTO();
    }
}

using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.SSR.Repository
{
    public interface ICreatorRepository
    {
        Task CreateCreator(Creator creator);
        Task DeleteCreator(int Id);
        Task<List<Creator>> GetAllCreators();
        Task<Creator> GetCreator(int Id);
        Task<List<Creator>> GetCreatorByTitle(string name);
        Task<IndexCreatorsDTO> GetIndexCreatorsDTO();
        Task UpdateCreator(Creator creator);
    }
}

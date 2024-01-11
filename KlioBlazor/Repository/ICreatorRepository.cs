using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface ICreatorRepository
    {
        Task<List<Creator>> GetCreatorByTitle(string name);
        Task<List<Creator>> GetAllCreators();
        Task<IndexCreatorsDTO> GetIndexCreatorsDTO();
        Task<Creator> GetCreator(int Id);
        Task UpdateCreator(Creator creator);
        Task CreateCreator(Creator creator);
    }
}

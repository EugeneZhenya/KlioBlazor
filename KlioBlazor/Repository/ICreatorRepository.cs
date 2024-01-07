using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface ICreatorRepository
    {
        Task CreateCreator(Creator creator);
        Task<List<Creator>> GetCreators();
    }
}

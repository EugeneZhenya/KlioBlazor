using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IKeywordRepository
    {
        Task CreateKeyword(Keyword keyword);
        Task<List<Keyword>> GetKeywords();
    }
}

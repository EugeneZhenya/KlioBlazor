using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IKeywordRepository
    {
        Task CreateKeyword(Keyword keyword);
        Task<List<Keyword>> GetKeywordByWord(string word);
        Task<List<Keyword>> GetKeywords();
    }
}

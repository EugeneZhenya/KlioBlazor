using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IKeywordRepository
    {
        Task CreateKeyword(Keyword keyword);
        Task<List<Keyword>> GetAllKeywords();
        Task<IndexKeywordsDTO> GetIndexKeywordsDTO();
        Task<Keyword> GetKeyword(int Id);
        Task<List<Keyword>> GetKeywordByWord(string word);
        Task UpdateKeyword(Keyword keyword);
    }
}

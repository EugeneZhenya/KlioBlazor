using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IKeywordRepository
    {
        Task CreateKeyword(Keyword keyword);
        Task DeleteKeyword(int Id);
        Task<List<Keyword>> GetAllKeywords();
        Task<DetailsKeywordDTO> GetDetailsKeywordDTO(int Id);
        Task<IndexKeywordsDTO> GetIndexKeywordsDTO();
        Task<Keyword> GetKeyword(int Id);
        Task<List<Keyword>> GetKeywordByWord(string word);
        Task UpdateKeyword(Keyword keyword);
    }
}

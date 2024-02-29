using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface IKeywordRepository
    {
        Task<List<Keyword>> GetAllKeywords();
        Task<DetailsKeywordDTO> GetDetailsKeywordDTO(FilterMoviesDTO filterMoviesDTO);
        Task<IndexKeywordsDTO> GetIndexKeywordsDTO();
        Task<Keyword> GetKeyword(int Id);
        Task<List<Keyword>> GetKeywordByWord(string searchText);
    }
}

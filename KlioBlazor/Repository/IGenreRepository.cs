using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.Repository
{
    public interface IGenreRepository
    {
        Task CreateGenre(Genre genre);
        Task<List<Genre>> GetAllGenres();
        Task<Genre> GetGenre(int Id);
        Task<IndexGenresDTO> GetIndexGenresDTO();
        Task UpdateGenre(Genre genre);
    }
}

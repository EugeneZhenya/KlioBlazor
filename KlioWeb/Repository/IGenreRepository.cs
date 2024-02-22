using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface IGenreRepository
    {
        Task<List<Genre>> GetAllGenres();
        Task<DetailsGenreDTO> GetDetailsGenreDTO(FilterMoviesDTO filterMoviesDTO);
        Task<Genre> GetGenre(int Id);
        Task<IndexGenresDTO> GetIndexGenresDTO();
    }
}

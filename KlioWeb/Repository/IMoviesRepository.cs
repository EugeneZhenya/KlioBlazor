using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface IMoviesRepository
    {
        Task<DetailsMovieDTO> GetDetailsMovieDTO(int id);
        Task<HomePageDTO> GetHomePageDTO();
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO);
    }
}

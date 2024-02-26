using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioWeb.Repository
{
    public interface IMoviesRepository
    {
        Task<DetailsMovieDTO> GetDetailsMovieDTO(int id);
        Task<DetailsYearDTO> GetDetailsYearDTO(FilterMoviesDTO filterMoviesDTO);
        Task<HomePageDTO> GetHomePageDTO();
        Task<IndexYearsDTO> GetIndexYearsDTO();
        Task<Movie> GetMovieById(int Id);
        Task<PaginatedResponse<List<Movie>>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO);
    }
}

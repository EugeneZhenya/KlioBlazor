using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;

namespace KlioBlazor.SSR.Repository
{
    public interface IMoviesRepository
    {
        Task<int> CreateMovie(Movie movie);
        Task DeleteMovie(int Id);
        Task<DetailsMovieDTO> GetDetailsMovieDTO(int id);
        Task<HomePageDTO> GetHomePageDTO();
        Task<MovieUpdateDTO> GetMovieForUpdate(int id);
        Task<List<Movie>> GetMoviesFiltered(FilterMoviesDTO filterMoviesDTO);
        Task UpdateMovie(Movie movie);
    }
}

using KlioBlazor.Shared.DTOs;

namespace KlioWeb.Repository
{
    public interface IAgeLimitsRepository
    {
        Task<DetailsAgeLimitsDTO> GetDetailsAgeLimitsDTO(FilterMoviesDTO filterMoviesDTO);
        Task<IndexAgeLimitsDTO> GetIndexAgeLimitsDTO();
    }
}

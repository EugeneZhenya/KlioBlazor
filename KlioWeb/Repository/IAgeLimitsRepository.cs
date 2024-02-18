using KlioBlazor.Shared.DTOs;

namespace KlioWeb.Repository
{
    public interface IAgeLimitsRepository
    {
        Task<DetailsAgeLimitsDTO> GetDetailsAgeLimitsDTO(int age);
        Task<IndexAgeLimitsDTO> GetIndexAgeLimitsDTO();
    }
}

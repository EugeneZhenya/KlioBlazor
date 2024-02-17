using KlioBlazor.Shared.DTOs;

namespace KlioWeb.Repository
{
    public interface IAgeLimitsRepository
    {
        Task<IndexAgeLimitsDTO> GetIndexAgeLimitsDTO();
    }
}

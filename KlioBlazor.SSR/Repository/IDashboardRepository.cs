using KlioBlazor.Shared.DTOs;

namespace KlioBlazor.SSR.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardDTO> GetDashboardDTO();
    }
}

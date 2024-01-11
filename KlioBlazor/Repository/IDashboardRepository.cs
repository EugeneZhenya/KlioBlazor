using KlioBlazor.Shared.DTOs;

namespace KlioBlazor.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardDTO> GetDashboardDTO();
    }
}

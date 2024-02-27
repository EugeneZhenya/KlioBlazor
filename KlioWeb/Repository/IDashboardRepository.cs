using KlioBlazor.Shared.DTOs;

namespace KlioWeb.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardDTO> GetDashboardDTO();
    }
}

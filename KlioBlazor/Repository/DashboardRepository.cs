using KlioBlazor.Helpers;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;

namespace KlioBlazor.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private NavigationManager navigationManager { get; set; }
        private readonly IHttpService httpService;
        private string url = "api/dashboard";

        public DashboardRepository(IHttpService httpService, NavigationManager navigationManager)
        {
            this.httpService = httpService;
            this.navigationManager = navigationManager;
            this.url = this.navigationManager.BaseUri + this.url;
        }

        public async Task<DashboardDTO> GetDashboardDTO()
        {
            return await httpService.GetHelper<DashboardDTO>(url);
        }
    }
}

using KlioBlazor.Shared.DTOs;
using KlioWeb.Pages.Movies;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.General
{
    public class DashboardModel : PageModel
    {
        public BreadcrumbArea BreadcrumbArea;
        public DashboardDTO DashboardInfo;

        private readonly ILogger<DashboardModel> _logger;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardModel(ILogger<DashboardModel> logger, IDashboardRepository dashboardRepository)
        {
            _logger = logger;
            _dashboardRepository = dashboardRepository;
        }

        public async Task OnGetAsync()
        {
            BreadcrumbArea = new BreadcrumbArea()
            {
                TitleContent = "<h2 class=\"title\">Адміністративна <span>панель</span></h2>",
                ChildContent = "<li class=\"breadcrumb-item\"><a href=\"\">Домівка</a></li><li class=\"breadcrumb-item active\" aria-current=\"page\">Адміністративна панель</li>"
            };

            DashboardInfo = await _dashboardRepository.GetDashboardDTO();
        }
    }
}

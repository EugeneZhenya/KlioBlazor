using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Movies;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.People
{
    public class IndexPeopleModel : PageModel
    {
        public AppState AppState = new AppState();
        public List<Person> People;
        [BindProperty]
        public PaginationDTO PaginationDTO { get; set; } = new PaginationDTO() { RecordsPerPage = 14 };
        public int TotalAmountPages;

        private readonly ILogger<IndexPeopleModel> _logger;
        private readonly IPersonRepository _personRepository;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }

        public BreadcrumbArea BreadcrumbArea;

        public IndexPeopleModel(ILogger<IndexPeopleModel> logger, IPersonRepository personRepository)
        {
            _logger = logger;
            _personRepository = personRepository;
        }

        public async Task OnGetAsync()
        {
            BreadcrumbArea = new BreadcrumbArea()
            {
                TitleContent = "<h2 class=\"title\">Список <span>осіб</span></h2>",
                ChildContent = "<li class=\"breadcrumb-item\"><a href=\"\">Домівка</a></li><li class=\"breadcrumb-item active\" aria-current=\"page\">Особи</li>"
            };

            if (!string.IsNullOrEmpty(Request.Query["page"]))
            {
                PaginationDTO.Page = int.Parse(Request.Query["page"]);
            }

            var paginatedResponse = await _personRepository.GetPeople(PaginationDTO);
            People = paginatedResponse.Response;
            TotalAmountPages = paginatedResponse.TotalAmountPages;
            TotalRecords = paginatedResponse.TotalRecords;
        }
    }
}

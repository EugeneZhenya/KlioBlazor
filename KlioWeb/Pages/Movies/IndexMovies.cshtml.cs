using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Movies
{
    public class IndexMoviesModel : PageModel
    {
        [BindProperty]
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public BreadcrumbArea BreadcrumbArea;
        public List<Movie> Movies;
        public int TotalAmountPages;

        private readonly ILogger<IndexMoviesModel> _logger;
        private readonly IMoviesRepository _moviesRepository;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }

        public MoviesArea FiltredMovies = new MoviesArea();

        public IndexMoviesModel(ILogger<IndexMoviesModel> logger, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
        }

        public async Task OnGetAsync()
        {
            BreadcrumbArea = new BreadcrumbArea()
            {
                TitleContent = "<h2 class=\"title\">Список <span>відео</span></h2>",
                ChildContent = "<li class=\"breadcrumb-item\"><a href=\"\">Домівка</a></li><li class=\"breadcrumb-item active\" aria-current=\"page\">Відео</li>"
            };

            if (!string.IsNullOrEmpty(Request.Query["page"]))
            {
                FilterMoviesDTO.Page = int.Parse(Request.Query["page"]);
            }
                
            await LoadMovies();
        }

        private async Task LoadMovies()
        {
            Movies = null;
            var pagenatedResponse = await _moviesRepository.GetMoviesFiltered(FilterMoviesDTO);
            Movies = pagenatedResponse.Response;
            TotalAmountPages = pagenatedResponse.TotalAmountPages;
            TotalRecords = pagenatedResponse.TotalRecords;

            FiltredMovies = new MoviesArea() { Movies = Movies, Title = "Список відео", Subtitle = "Усі відео нашого архіву", NoRecords = "Нічого не знайдено", CarouselClass = "bottom-carousel" };
        }
    }
}

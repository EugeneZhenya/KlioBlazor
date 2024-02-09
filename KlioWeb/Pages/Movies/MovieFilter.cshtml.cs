using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Web;

namespace KlioWeb.Pages.Movies
{
    public class MovieFilterModel : PageModel
    {
        [BindProperty]
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public BreadcrumbArea BreadcrumbArea;
        public List<Genre> Genres = new List<Genre>();
        public List<Movie> Movies;
        public int TotalAmountPages;
        public string TitleDisplay = "";

        private readonly ILogger<IndexModel> _logger;
        private readonly IGenreRepository _genreRepository;
        private readonly IMoviesRepository _moviesRepository;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }

        public MoviesArea FiltredMovies = new MoviesArea();

        public MovieFilterModel(ILogger<IndexModel> logger, IGenreRepository genreRepository, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _genreRepository = genreRepository;
            _moviesRepository = moviesRepository;
        }

        public async Task OnGetAsync()
        {

            BreadcrumbArea = new BreadcrumbArea() {
                TitleContent = "<h2 class=\"title\">Пошук <span>фільмів</span></h2>",
                ChildContent = "<li class=\"breadcrumb-item\"><a href=\"\">Домівка</a></li><li class=\"breadcrumb-item active\" aria-current=\"page\">Пошук</li>"
            };

            Genres = await _genreRepository.GetAllGenres();
            Genres = Genres.OrderBy(x => x.Name).ToList();

            if (!string.IsNullOrEmpty(Request.Query["genreId"]))
            {
                FilterMoviesDTO.GenreId = int.Parse(Request.Query["genreId"]);
            }

            if (!string.IsNullOrEmpty(Request.Query["title"]))
            {
                FilterMoviesDTO.Title = HttpUtility.UrlDecode(Request.Query["title"]);
            }

            if (!string.IsNullOrEmpty(Request.Query["page"]))
            {
                FilterMoviesDTO.Page = int.Parse(Request.Query["page"]);
            }
            await LoadMovies();
        }

        public IActionResult OnPost()
        {
            BreadcrumbArea = new BreadcrumbArea()
            {
                TitleContent = "<h2 class=\"title\">Пошук <span>фільмів</span></h2>",
                ChildContent = "<li class=\"breadcrumb-item\"><a href=\"\">Домівка</a></li><li class=\"breadcrumb-item active\" aria-current=\"page\">Пошук</li>"
            };

            if (FilterMoviesDTO.GenreId > 0)
            {
                return Redirect(Uri.EscapeUriString("/movies/search?title=" + FilterMoviesDTO.Title + "&genreId=" + FilterMoviesDTO.GenreId).ToString());
            }

            return Redirect(Uri.EscapeUriString("/movies/search?title=" + FilterMoviesDTO.Title).ToString());
        }

        private async Task LoadMovies()
        {
            TitleDisplay = FilterMoviesDTO.Title;
            Movies = null;
            var pagenatedResponse = await _moviesRepository.GetMoviesFiltered(FilterMoviesDTO);
            Movies = pagenatedResponse.Response;
            TotalAmountPages = pagenatedResponse.TotalAmountPages;
            TotalRecords = pagenatedResponse.TotalRecords;

            FiltredMovies = new MoviesArea() { Movies = Movies, Title = TitleDisplay, Subtitle = "Результати пошуку", NoRecords = "Нічого не знайдено", CarouselClass = "bottom-carousel" };
        }
    }
}

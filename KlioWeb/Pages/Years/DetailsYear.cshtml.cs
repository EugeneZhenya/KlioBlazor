using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.Shared.Helpers;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Years
{
    public class DetailsYearModel : PageModel
    {
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int Year { get; set; }
        public DetailsYearDTO model;

        private readonly ILogger<DetailsYearModel> _logger;
        private readonly IMoviesRepository _moviesRepository;
        public MoviesArea YearMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }
        public List<Movie> MoviesOfYear;
        public Movie LastMovie;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }
        public int TotalAmountPages;

        public DetailsYearModel(ILogger<DetailsYearModel> logger, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Request.Query["page"]))
            {
                FilterMoviesDTO.Page = int.Parse(Request.Query["page"]);
            }
            await LoadMovies();
        }

        private async Task LoadMovies()
        {
            model = null;
            FilterMoviesDTO.Year = Year;
            model = await _moviesRepository.GetDetailsYearDTO(FilterMoviesDTO);

            MoviesOfYear = model.YearMovies.Response;
            LastAdded = model.LastAdded;
            LastMovie = model.LastMovie;

            TotalAmountPages = model.YearMovies.TotalAmountPages;
            TotalRecords = model.YearMovies.TotalRecords;

            YearMovies = new MoviesArea() { Movies = MoviesOfYear, Title = "Фільми " + Year + "-го року", Subtitle = "Фільми за роком виходу", ShowCategoryName = true };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

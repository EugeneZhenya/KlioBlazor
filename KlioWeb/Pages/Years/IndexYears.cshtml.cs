using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Years
{
    public class IndexYearsModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;

        private readonly ILogger<IndexYearsModel> _logger;
        private readonly IMoviesRepository _moviesRepository;

        public BannerArea BannerArea;
        public List<Country> LastMovieCountries;
        public List<DateTime> ListOfYears;
        public List<Movie> LastAdded { get; set; }
        public MoviesArea MoviesArea = new MoviesArea();

        public IndexYearsModel(ILogger<IndexYearsModel> logger, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _moviesRepository.GetIndexYearsDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            ListOfYears = response.AllYears;
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

using KlioBlazor.Shared.Entities;
using KlioBlazor.Shared.Enums;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.AgeLimits
{
    public class IndexAgeLimitsModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;

        private readonly ILogger<IndexModel> _logger;
        private readonly IAgeLimitsRepository _ageLimitsRepository;

        public BannerArea BannerArea;
        public List<Country> LastMovieCountries;
        public List<AgeCategory> AgeCategories;
        public List<int> AgeCounters;
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        public IndexAgeLimitsModel(ILogger<IndexModel> logger, IAgeLimitsRepository ageLimitsRepository)
        {
            _logger = logger;
            _ageLimitsRepository = ageLimitsRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _ageLimitsRepository.GetIndexAgeLimitsDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            AgeCategories = response.AllAges;
            AgeCounters = response.AgeCounters;
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

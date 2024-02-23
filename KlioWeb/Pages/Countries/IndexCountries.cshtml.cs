using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Genres;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Countries
{
    public class IndexCountriesModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;
        public List<Country> LastMovieCountries;
        public List<Country> Countries;
        public List<Movie> LastAdded { get; set; }

        private readonly ILogger<IndexCountriesModel> _logger;
        private readonly ICountryRepository _countryRepository;

        public BannerArea BannerArea;
        public MoviesArea MoviesArea = new MoviesArea();

        public IndexCountriesModel(ILogger<IndexCountriesModel> logger, ICountryRepository countryRepository)
        {
            _logger = logger;
            _countryRepository = countryRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _countryRepository.GetIndexCountriesDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            Countries = response.AllCountries.OrderBy(x => x.Name).ToList();
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

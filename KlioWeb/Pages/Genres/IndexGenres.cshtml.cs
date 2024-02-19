using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Genres
{
    public class IndexGenresModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;

        private readonly ILogger<IndexModel> _logger;
        private readonly IGenreRepository _genreRepository;

        public BannerArea BannerArea;
        public List<Country> LastMovieCountries;
        public List<Genre> Genres;
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        public IndexGenresModel(ILogger<IndexModel> logger, IGenreRepository genreRepository)
        {
            _logger = logger;
            _genreRepository = genreRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _genreRepository.GetIndexGenresDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            Genres = response.AllGenres.OrderBy(x => x.Name).ToList();
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

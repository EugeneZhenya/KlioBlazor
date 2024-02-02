using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages
{
    public class IndexModel : PageModel
    {
        public AppState appState = new AppState();
        private Movie lastMovie = new Movie();
        private List<Country> lastMovieCountries;
        private List<Partition> partitionsPopular;
        private List<Movie> moviesPopular;
        public Movie RecommendMovie;
        public List<Country> RecommendMovieCountries;

        public BannerArea BannerArea;
        public PartitionsArea PartitionsArea = new PartitionsArea();
        public MoviesArea PopularMovies = new MoviesArea();

        private readonly ILogger<IndexModel> _logger;
        private readonly IMoviesRepository _moviesRepository;

        public IndexModel(ILogger<IndexModel> logger, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _moviesRepository.GetHomePageDTO();
            lastMovie = response.LastMovie;
            lastMovieCountries = response.LastMovieCountries;
            partitionsPopular = response.PartitionsPopular;
            moviesPopular = response.MoviesPopular;
            RecommendMovie = response.RecomendMovie;
            RecommendMovieCountries = response.RecomendMovieCountries;

            BannerArea = new BannerArea() { ShowMovie = lastMovie, ShowMovieCountries = lastMovieCountries };
            PartitionsArea = new PartitionsArea() { Partitions = partitionsPopular, Title = "Найпопулярніші розділи", Subtitle = "Дивіться зараз" };
            PopularMovies = new MoviesArea() { Movies = moviesPopular, Title = "Найпопулярніші фільми", Subtitle = "Дивіться зараз", CenterHeader = true, UseFilter = true };
        }
    }
}

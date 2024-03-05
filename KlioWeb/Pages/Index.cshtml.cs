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
        private List<Movie> lastAdded;
        public List<Movie> todayMovies;
        public Movie RecommendMovie;
        public List<Country> RecommendMovieCountries;

        public BannerArea BannerArea;
        public PartitionsArea PartitionsArea = new PartitionsArea();
        public MoviesArea PopularMovies = new MoviesArea();
        public MoviesArea LastAdded = new MoviesArea();
        public MoviesArea TodayMovies = new MoviesArea();

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
            lastAdded = response.LastAdded;
            todayMovies = response.TodaysFilms;

            BannerArea = new BannerArea() { ShowMovie = lastMovie, ShowMovieCountries = lastMovieCountries };
            PartitionsArea = new PartitionsArea() { Partitions = partitionsPopular, Title = "Найпопулярніші розділи", Subtitle = "Дивіться зараз" };
            PopularMovies = new MoviesArea() { Movies = moviesPopular, Title = "Найпопулярніші фільми", Subtitle = "Дивіться зараз", CenterHeader = true, UseFilter = true };
            LastAdded = new MoviesArea() { Movies = lastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass= "bottom-carousel" };
            TodayMovies = new MoviesArea() { Movies = todayMovies, Title = "Фільми цього дня", Subtitle = "Фільми, що вийшли у цей день", ShowCategoryName = true, WatchAll = false, CarouselClass = "bottom-carousel" };
        }
    }
}

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

        public BannerArea BannerArea;
        public PartitionsArea PartitionsArea = new PartitionsArea();

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

            BannerArea = new BannerArea() { ShowMovie = lastMovie, ShowMovieCountries = lastMovieCountries };
            PartitionsArea = new PartitionsArea() { Partitions = partitionsPopular, Title = "Найпопулярніші розділи", Subtitle = "Дивіться зараз" };
        }
    }
}

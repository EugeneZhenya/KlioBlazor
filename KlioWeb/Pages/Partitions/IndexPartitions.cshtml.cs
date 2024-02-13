using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Partitions
{
    public class IndexPartitionsModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;

        private readonly ILogger<IndexModel> _logger;
        private readonly IPartitionRepository _partitionRepository;

        public BannerArea BannerArea;
        public List<Country> LastMovieCountries;
        public List<Partition> Partitions;
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        public IndexPartitionsModel(ILogger<IndexModel> logger, IPartitionRepository partitionRepository)
        {
            _logger = logger;
            _partitionRepository = partitionRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _partitionRepository.GetIndexPartitionsDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            Partitions = response.AllPartitions;
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

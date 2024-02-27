using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Countries;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Creators
{
    public class IndexCreatorsModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;
        public List<Country> LastMovieCountries;
        public List<Creator> Creators;
        public List<Movie> LastAdded { get; set; }

        private readonly ILogger<IndexCreatorsModel> _logger;
        private readonly ICreatorRepository _creatorRepository;

        public BannerArea BannerArea;
        public MoviesArea MoviesArea = new MoviesArea();

        public IndexCreatorsModel(ILogger<IndexCreatorsModel> logger, ICreatorRepository creatorRepository)
        {
            _logger = logger;
            _creatorRepository = creatorRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _creatorRepository.GetIndexCreatorsDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            Creators = response.AllCreators.OrderBy(x => x.Title).ToList();
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

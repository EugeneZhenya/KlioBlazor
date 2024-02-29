using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Genres;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Keywords
{
    public class IndexKeywordsModel : PageModel
    {
        public AppState AppState = new AppState();
        public Movie LastMovie;
        public List<Country> LastMovieCountries;
        public List<Keyword> Keywords;
        public List<Movie> LastAdded { get; set; }

        private readonly ILogger<IndexKeywordsModel> _logger;
        private readonly IKeywordRepository _keywordRepository;

        public BannerArea BannerArea;
        public MoviesArea MoviesArea = new MoviesArea();

        public IndexKeywordsModel(ILogger<IndexKeywordsModel> logger, IKeywordRepository keywordRepository)
        {
            _logger = logger;
            _keywordRepository = keywordRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _keywordRepository.GetIndexKeywordsDTO();
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            Keywords = response.AllKeywords.OrderBy(x => x.Word).ToList();
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Genres
{
    public class DetailsGenreModel : PageModel
    {
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int GenreId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? GenreName { get; set; }

        public DetailsGenreDTO model;
        public List<Movie> MoviesOfGenre;
        public string NameOfGenre;

        private readonly ILogger<IndexModel> _logger;
        private readonly IGenreRepository _genreRepository;
        public MoviesArea GenreMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        public DetailsGenreModel(ILogger<IndexModel> logger, IGenreRepository genreRepository)
        {
            _logger = logger;
            _genreRepository = genreRepository;
        }

        public async Task OnGetAsync()
        {
            model = await _genreRepository.GetDetailsGenreDTO(GenreId);

            MoviesOfGenre = model.GenreMovies;
            NameOfGenre = model.Genre.Name;
            LastAdded = model.LastAdded;

            GenreMovies = new MoviesArea() { Movies = MoviesOfGenre, Title = NameOfGenre, Subtitle = "Фільми жанру", ShowCategoryName = false, WatchAll = false };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

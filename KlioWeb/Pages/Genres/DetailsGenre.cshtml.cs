using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.Shared.Helpers;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Genres
{
    public class DetailsGenreModel : PageModel
    {
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int GenreId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? GenreName { get; set; }

        public DetailsGenreDTO model;
        public List<Movie> MoviesOfGenre;
        public string NameOfGenre;

        private readonly ILogger<DetailsGenreModel> _logger;
        private readonly IGenreRepository _genreRepository;
        public MoviesArea GenreMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }
        public int TotalAmountPages;

        public DetailsGenreModel(ILogger<DetailsGenreModel> logger, IGenreRepository genreRepository)
        {
            _logger = logger;
            _genreRepository = genreRepository;
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Request.Query["page"]))
            {
                FilterMoviesDTO.Page = int.Parse(Request.Query["page"]);
            }
            await LoadMovies();
        }

        private async Task LoadMovies()
        {
            model = null;
            FilterMoviesDTO.GenreId = GenreId;
            model = await _genreRepository.GetDetailsGenreDTO(FilterMoviesDTO);

            MoviesOfGenre = model.GenreMovies.Response;
            NameOfGenre = model.Genre.Name;
            LastAdded = model.LastAdded;

            TotalAmountPages = model.GenreMovies.TotalAmountPages;
            TotalRecords = model.GenreMovies.TotalRecords;

            GenreMovies = new MoviesArea() { Movies = MoviesOfGenre, Title = NameOfGenre, Subtitle = "Фільми жанру", ShowCategoryName = true, WatchAll = false };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

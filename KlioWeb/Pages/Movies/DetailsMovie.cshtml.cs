using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Movies
{
    public class DetailsMovieModel : PageModel
    {
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? MovieName { get; set; }

        public DetailsMovieDTO model;
        public List<Partition> Partitions;
        public MoviesArea OtherMovies = new MoviesArea();

        private readonly ILogger<DetailsMovieModel> _logger;
        private readonly IMoviesRepository _moviesRepository;
        public DetailsMovieModel(ILogger<DetailsMovieModel> logger, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
        }

        public async Task OnGetAsync()
        {
            model = await _moviesRepository.GetDetailsMovieDTO(MovieId);
            Partitions = model.Partitions.OrderByDescending(x => x.Movies.Count).ToList();
            OtherMovies = new MoviesArea() { Movies = model.OtherMovies, Title = model.Movie.Partition.Name, Subtitle = "Інші відео розділу", CenterHeader = false, CarouselClass = "another-carousel" };

            await _moviesRepository.IncrementViewCounter(MovieId);
        }
    }
}

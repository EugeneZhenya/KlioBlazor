using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Countries;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Creators
{
    public class DetailsCreatorModel : PageModel
    {
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int CreatorId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? CreatorName { get; set; }

        public DetailsCreatorDTO model;
        public List<Movie> MoviesOfCreator;
        public string NameOfCreator;

        private readonly ILogger<DetailsCreatorModel> _logger;
        private readonly ICreatorRepository _creatorRepository;
        public MoviesArea CreatorMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }
        public int TotalAmountPages;

        public DetailsCreatorModel(ILogger<DetailsCreatorModel> logger, ICreatorRepository creatorRepository)
        {
            _logger = logger;
            _creatorRepository = creatorRepository;
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
            FilterMoviesDTO.CreatorId = CreatorId;
            model = await _creatorRepository.GetDetailsCreatorDTO(FilterMoviesDTO);

            MoviesOfCreator = model.CreatorMoviesPage.Response;
            NameOfCreator = model.Creator.Title;
            LastAdded = model.LastAdded;

            TotalAmountPages = model.CreatorMoviesPage.TotalAmountPages;
            TotalRecords = model.CreatorMoviesPage.TotalRecords;

            CreatorMovies = new MoviesArea() { Movies = MoviesOfCreator, Title = NameOfCreator, Subtitle = "Фільми виробника (кіностудії)", ShowCategoryName = true, WatchAll = false };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

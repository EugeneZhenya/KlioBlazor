using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioBlazor.Shared.Enums;
using KlioBlazor.Shared.Helpers;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Concurrent;

namespace KlioWeb.Pages.AgeLimits
{
    public class DetailsAgeLimitModel : PageModel
    {
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public string LimitName { get; set; }
        public DetailsAgeLimitsDTO model;

        private readonly ILogger<DetailsAgeLimitModel> _logger;
        private readonly IAgeLimitsRepository _ageLimitsRepository;
        public MoviesArea AgeLimitsMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }
        public List<Movie> MoviesOfAgeLimit;
        public Movie LastMovie;

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }
        public int TotalAmountPages;

        public DetailsAgeLimitModel(ILogger<DetailsAgeLimitModel> logger, IAgeLimitsRepository ageLimitsRepository)
        {
            _logger = logger;
            _ageLimitsRepository = ageLimitsRepository;
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
            AgeCategory ageEnum = (AgeCategory)Enum.Parse(typeof(AgeCategory), LimitName);
            FilterMoviesDTO.Age = (int)ageEnum;
            model = await _ageLimitsRepository.GetDetailsAgeLimitsDTO(FilterMoviesDTO);

            MoviesOfAgeLimit = model.AgesMovies.Response;
            LastAdded = model.LastAdded;
            LastMovie = model.LastMovie;

            TotalAmountPages = model.AgesMovies.TotalAmountPages;
            TotalRecords = model.AgesMovies.TotalRecords;

            AgeLimitsMovies = new MoviesArea() { Movies = MoviesOfAgeLimit, Title = EnumHelper<AgeCategory>.GetDisplayValue(ageEnum), Subtitle = "Вікові обмеження", ShowCategoryName = true };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

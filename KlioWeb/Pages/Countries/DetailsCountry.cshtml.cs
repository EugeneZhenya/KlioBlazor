using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Genres;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Countries
{
    public class DetailsCountryModel : PageModel
    {
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int CountryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? CountryName { get; set; }

        public DetailsCountryDTO model;
        public List<Movie> MoviesOfCountry;
        public string NameOfCountry;

        private readonly ILogger<DetailsCountryModel> _logger;
        private readonly ICountryRepository _countryRepository;
        public MoviesArea CountryMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }
        public int TotalAmountPages;

        public DetailsCountryModel(ILogger<DetailsCountryModel> logger, ICountryRepository countryRepository)
        {
            _logger = logger;
            _countryRepository = countryRepository;
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
            FilterMoviesDTO.CountryId = CountryId;
            model = await _countryRepository.GetDetailsCountryDTO(FilterMoviesDTO);

            MoviesOfCountry = model.CountryMoviesPage.Response;
            NameOfCountry = model.Country.Name;
            LastAdded = model.LastAdded;

            TotalAmountPages = model.CountryMoviesPage.TotalAmountPages;
            TotalRecords = model.CountryMoviesPage.TotalRecords;

            CountryMovies = new MoviesArea() { Movies = MoviesOfCountry, Title = NameOfCountry, Subtitle = "Фільми країни", ShowCategoryName = true, WatchAll = false };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

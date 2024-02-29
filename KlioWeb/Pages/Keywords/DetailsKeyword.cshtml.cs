using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Genres;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace KlioWeb.Pages.Keywords
{
    public class DetailsKeywordModel : PageModel
    {
        public FilterMoviesDTO FilterMoviesDTO { get; set; } = new FilterMoviesDTO();
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int KeywordId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? KeywordName { get; set; }

        public DetailsKeywordDTO model;
        public List<Movie> MoviesByKeyword;
        public string NameOfKeyword;
        public string ImageUrl = string.Empty;

        private readonly ILogger<DetailsKeywordModel> _logger;
        private readonly IKeywordRepository _keywordRepository;
        public MoviesArea KeywordMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 15;
        [BindProperty(SupportsGet = true)]
        public int TotalRecords { get; set; }
        public int TotalAmountPages;

        public DetailsKeywordModel(ILogger<DetailsKeywordModel> logger, IKeywordRepository keywordRepository)
        {
            _logger = logger;
            _keywordRepository = keywordRepository;
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
            FilterMoviesDTO.KeywordId = KeywordId;
            model = await _keywordRepository.GetDetailsKeywordDTO(FilterMoviesDTO);

            MoviesByKeyword = model.KeywordMoviesPage.Response;
            NameOfKeyword = model.Keyword.Word;
            LastAdded = model.LastAdded;
            await GetImageUrl();

            TotalAmountPages = model.KeywordMoviesPage.TotalAmountPages;
            TotalRecords = model.KeywordMoviesPage.TotalRecords;

            KeywordMovies = new MoviesArea() { Movies = MoviesByKeyword, Title = NameOfKeyword, Subtitle = "Фільми за ключовим словом", ShowCategoryName = true, WatchAll = false };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }

        private async Task GetImageUrl()
        {
            string API_KEY = "42108270-3502561b495d630d6052e1bcc";
            string baseURL = "https://pixabay.com/api/";
            string searchTerm = model.Keyword.Equivalent;
            string URL = $"{baseURL}?key={API_KEY}&q={Uri.EscapeDataString(searchTerm)}";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(URL);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic data = JsonConvert.DeserializeObject(content);

                    int totalHits = data["totalHits"].ToObject<int>();
                    if (totalHits > 0)
                    {
                        ImageUrl = data["hits"][0]["largeImageURL"];
                    }
                    else
                    {
                        ImageUrl = string.Empty;
                    }
                }
            }
        }
    }
}

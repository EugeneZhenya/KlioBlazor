using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Categories
{
    public class IndexCategoriesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? CatName { get; set; }
        public Movie LastMovie;
        public List<Country> LastMovieCountries;
        public List<Category> Categories;
        public List<Partition> Partitions;
        public List<Movie> LastAdded { get; set; }

        private readonly ILogger<IndexModel> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public BannerArea BannerArea;
        public PartitionsArea PartitionsArea = new PartitionsArea();
        public MoviesArea MoviesArea = new MoviesArea();

        public IndexCategoriesModel(ILogger<IndexModel> logger, ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task OnGetAsync()
        {
            var response = await _categoryRepository.GetCategoryByName(CatName);
            LastMovie = response.LastMovie;
            LastMovieCountries = response.LastMovieCountries;
            Categories = response.AllCategories;
            Partitions = response.CategoryPartitions;
            LastAdded = response.LastAdded;

            BannerArea = new BannerArea() { ShowMovie = LastMovie, ShowMovieCountries = LastMovieCountries };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };

            if (LastMovie != null)
            {
                PartitionsArea = new PartitionsArea() { Partitions = Partitions, Title = LastMovie.Partition.Category.Name, Subtitle = "Розділи категорії", ShowCategoryName = false, UseOwlCarousel = false };
            }
            else
            {
                PartitionsArea = new PartitionsArea();
            }
        }
    }
}

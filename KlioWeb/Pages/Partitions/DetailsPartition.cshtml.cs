using Azure;
using KlioBlazor.Shared.DTOs;
using KlioBlazor.Shared.Entities;
using KlioWeb.Pages.Shared;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Partitions
{
    public class DetailsPartitionModel : PageModel
    {
        public AppState AppState = new AppState();
        [BindProperty(SupportsGet = true)]
        public int PartitionId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? PartitionName { get; set; }
        public DetailsPartitionDTO model;
        public List<Movie> MoviesOfPartition;
        public string NameOfPartition;
        public string NameOfCategory;

        private readonly ILogger<DetailsPartitionModel> _logger;
        private readonly IPartitionRepository _partitionRepository;
        public MoviesArea PartitionMovies = new MoviesArea();
        public MoviesArea MoviesArea = new MoviesArea();
        public List<Movie> LastAdded { get; set; }

        public DetailsPartitionModel(ILogger<DetailsPartitionModel> logger, IPartitionRepository partitionRepository)
        {
            _logger = logger;
            _partitionRepository = partitionRepository;
        }

        public async Task OnGetAsync()
        {
            model = await _partitionRepository.GetDetailsPartitionDTO(PartitionId);

            if (model.LastMovie == null)
            {
                model.LastMovie = new Movie() { Id = 0, PatitionId = model.Partition.Id };
            }

            MoviesOfPartition = model.PartitionMovies;
            NameOfPartition = model.Partition.Name;
            NameOfCategory = model.Partition.Category.Name;
            LastAdded = model.LastAdded;

            PartitionMovies = new MoviesArea() { Movies = MoviesOfPartition, Title = NameOfPartition, Subtitle = NameOfCategory, ShowCategoryName = false, WatchAll = true };
            MoviesArea = new MoviesArea() { Movies = LastAdded, Title = "Останні додані", Subtitle = "Не проґавте", CenterHeader = true, CarouselClass = "bottom-carousel" };
        }
    }
}

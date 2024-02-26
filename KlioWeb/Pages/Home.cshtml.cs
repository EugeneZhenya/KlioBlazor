using KlioBlazor.Shared.Helpers;
using KlioWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages
{
    public class HomeModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Param1 { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Param2 { get; set; }

        private readonly ILogger<HomeModel> _logger;
        private readonly IPartitionRepository _partitionRepository;
        private readonly IMoviesRepository _moviesRepository;

        public HomeModel(ILogger<HomeModel> logger, IPartitionRepository partitionRepository, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _partitionRepository = partitionRepository;
            _moviesRepository = moviesRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(Param1))
            {
                switch (Param1.ToLower())
                {
                    case "serie":
                        string partName;
                        int partId;
                        try
                        {
                            partId = Int32.Parse(Param2);
                            var partInfo = await _partitionRepository.GetPartition(partId);
                            partName = partInfo.Name;
                            partName = StringUtilities.Translit(partName);
                        }
                        catch (Exception)
                        {
                            return RedirectToPage("Partitions/DetailsPartition", new { PartitionId = Param2 });
                        }
                        return RedirectToPage("Partitions/DetailsPartition", new { PartitionId = Param2, PartitionName = partName });
                    case "view":
                        string movieName;
                        int movieId;
                        try
                        {
                            movieId = Int32.Parse(Param2);
                            var movieInfo = await _moviesRepository.GetMovieById(movieId);
                            movieName = movieInfo.Title;
                            movieName = StringUtilities.Translit(movieName);
                        }
                        catch (Exception)
                        {
                            return RedirectToPage("Movies/DetailsMovie", new { MovieId = Param2 });
                        }
                        return RedirectToPage("Movies/DetailsMovie", new { MovieId = movieId, MovieName = movieName });
                    default:
                        return RedirectToPage("Categories/IndexCategories", new { CatName = Param1 });
                }
            }

            return RedirectToPage("Index");
        }
    }
}

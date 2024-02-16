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

        private readonly ILogger<IndexModel> _logger;
        private readonly IPartitionRepository _partitionRepository;

        public HomeModel(ILogger<IndexModel> logger, IPartitionRepository partitionRepository)
        {
            _logger = logger;
            _partitionRepository = partitionRepository;
        }

        public async Task<IActionResult> OnGet()
        {
            if (!string.IsNullOrEmpty(Param1))
            {
                switch (Param1)
                {
                    case "Serie":
                        string partName;
                        try
                        {
                            int partId = Int32.Parse(Param2);
                            var partInfo = await _partitionRepository.GetPartition(partId);
                            partName = partInfo.Name;
                            partName = StringUtilities.Translit(partName);
                        }
                        catch (Exception)
                        {
                            return RedirectToPage("Partitions/DetailsPartition", new { PartitionId = Param2 });
                        }
                        return RedirectToPage("Partitions/DetailsPartition", new { PartitionId = Param2, PartitionName = partName });
                    default:
                        return RedirectToPage("Categories/IndexCategories", new { CatName = Param1 });
                }
            }

            return RedirectToPage("Index");
        }
    }
}

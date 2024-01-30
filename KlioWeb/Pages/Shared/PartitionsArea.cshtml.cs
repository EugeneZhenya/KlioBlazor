using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Shared
{
    public class PartitionsArea : PageModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public String NoRecords { get; set; } = "Розділ не має записів.";
        public List<Partition> Partitions { get; set; }
        public bool ShowCategoryName { get; set; } = true;

        public void OnGet()
        {
        }
    }
}

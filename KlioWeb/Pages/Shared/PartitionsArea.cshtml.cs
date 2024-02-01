using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Shared
{
    public class PartitionsArea : PageModel
    {
        public AppState appState = new AppState();
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string NoRecords { get; set; } = "Розділ не має записів.";
        public List<Partition> Partitions { get; set; }
        public bool ShowCategoryName { get; set; } = true;
    }
}

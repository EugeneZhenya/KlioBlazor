using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Shared
{
    public class MoviesArea : PageModel
    {
        public AppState appState = new AppState();
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public String NoRecords { get; set; } = "Розділ не має записів.";
        public List<Movie> Movies { get; set; }
        public bool CenterHeader { get; set; } = false;
        public bool ShowCategoryName { get; set; } = true;
        public bool UseFilter { get; set; } = false;
        public string CarouselClass { get; set; }
    }
}

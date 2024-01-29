using KlioBlazor.Shared.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Shared
{
    public class BannerArea : PageModel
    {
        public AppState appState = new AppState();
        public Movie ShowMovie { get; set; }
        public List<Country> ShowMovieCountries { get; set; }
        public string OriginalTitle = string.Empty;

        public BannerArea()
        {
        }

        public void OnGet()
        {
            
        }
    }
}

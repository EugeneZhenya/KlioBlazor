using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Categories
{
    public class IndexCategoriesModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string CatName { get; set; }

        public void OnGet()
        {
        }
    }
}

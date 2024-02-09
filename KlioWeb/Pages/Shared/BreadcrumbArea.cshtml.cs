using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KlioWeb.Pages.Shared
{
    public class BreadcrumbArea : PageModel
    {
        public string TitleContent { get; set; }
        public string ChildContent { get; set; }
    }
}

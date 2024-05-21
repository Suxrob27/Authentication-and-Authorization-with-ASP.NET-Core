using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie_Realization.Pages
{
    [Authorize(Policy ="AdminOnly")]
    public class UserControloModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

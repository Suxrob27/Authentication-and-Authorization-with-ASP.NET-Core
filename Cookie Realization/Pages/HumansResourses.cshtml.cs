using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Cookie_Realization.Pages
{
    [Authorize(Policy = "MustBelongToHRMeneger")]
    public class Index1Model : PageModel
    {
        public void OnGet()
        {
        }
    }
}

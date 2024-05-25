using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Cookie_Realization.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credentials Credential { get; set; }
        public void OnGet()
        {

        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return null;

            if (Credential.Name == "admin" && Credential.Password == "password")
            {
                //Creating Security Claim
                var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Email,"admin@mywebsite.com"),
                    new Claim("Department","HR"),
                    new Claim("Admin", "true"),
                    new Claim("EmploymentDate","2023-05-01")
                   
                };
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Credential.remember_Me 
                };
                var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal,authProperties);
                return RedirectToAction("/Index");

            }
            return Page();
        }
        public class Credentials
        {
            [Required]
            [Display(Name = "User Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember ME")]
            public bool remember_Me { get; set; }   
        }
    }
}

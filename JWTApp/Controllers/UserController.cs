using JWTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Xml.Linq;

namespace JWTApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("Public")]
        public IActionResult Public()
        {
            return Ok("Hi There , You are on public property");
        }
        [HttpGet("Admins")]
        [Authorize]
        public IActionResult AdminsEndPoint()
        {
            var currentUser = GetCurrentUser();
  
            return Ok($"{currentUser.Name}, you an {currentUser.Role}" ); 
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
            {
                var uesrClaims = identity.Claims;

                return new UserModel
                {
                    Name = uesrClaims.FirstOrDefault(o =>o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Email = uesrClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = uesrClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value,
                };
            }
            return null;
        }
    }
}

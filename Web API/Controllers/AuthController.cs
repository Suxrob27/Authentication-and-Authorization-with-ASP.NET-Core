using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
          _config = configuration;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody]Credential credentials)
        {
            if(credentials.UserName != null && credentials.Password != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, credentials.UserName),
                    new Claim(ClaimTypes.Email,"admin@mywebsite.com"),
                    new Claim("Department","HR"),
                    new Claim("Admin", "true"),
                    new Claim("EmploymentDate","2023-05-01")
                };

                return Ok(new
                {
                    access_token = CreateToken(credentials, claims, DateTime.UtcNow.AddMinutes(60))
                }) ;
            }
            ModelState.AddModelError("Unauthorized","You are Not Authorized To Access the EndPoint");
            return Unauthorized(ModelState);  
        }

        private string CreateToken(Credential credential, IEnumerable<Claim> claims, DateTime expiresAt)
        {
            var securityKey = Encoding.UTF8.GetBytes(_config["JwtConfig:SignKey"]??"");
            var jwt = new JwtSecurityToken(
                 claims: claims,
                 notBefore: DateTime.UtcNow,
                 expires: expiresAt,
                 signingCredentials: new SigningCredentials(
                     new SymmetricSecurityKey(securityKey),
                     SecurityAlgorithms.HmacSha256Signature)

                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }


    public class Credential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
       
using JWTApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.CodeDom.Compiler;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ValuesController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userlogin)
        {
            var user = Authenticate(userlogin);

            if(user != null) 
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("User Not Found");
        }

        private object Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
              new Claim(ClaimTypes.NameIdentifier,user.Name),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, user.Role),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires:DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(UserLogin userlogin)
        {
            var currentUser = UserConstant.Users.FirstOrDefault(o => o.Name.ToLower() == userlogin.Name.ToLower() && o.Password == userlogin.Password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}

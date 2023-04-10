using AuthorizeApi.Model;
using AuthorizeApi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthorizeApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<String> getvalues()
        {
            return new String[] { "user1", "user2" };
        }

       

       

        [AllowAnonymous]
        [HttpPost, Route("createtoken")]
        public IActionResult CreateToken([FromBody] Cred login)
        {
            
            IActionResult response = Unauthorized();
            var user = Authenticate(login);
            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }

        private string BuildToken(Agent user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Agent"),
                new Claim("UserName", user.Name.ToString())
            };


            var token = new JwtSecurityToken(_config["Jwt:Issuer"],

             _config["Jwt:Issuer"],

             claims: claims,

             expires: DateTime.Now.AddMinutes(30),

             signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        private Agent Authenticate(Cred login)

        {
            Agent user = null;
            if (login.Email == "admin" && login.Password == "admin")
            {
                user = new Agent { Name = "Aman Chaudhary", Email = "aman@domain.com" };
            }
            return user;

        }



    }
}

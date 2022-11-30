using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Propeller.Models.Requests;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Propeller.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
  

        private class PropellerUser
        {
            public string UserName { get; set; }
            public int Role { get; set; }

        }

        private IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthRequest request)
        {
            var user = ValidateCreds("", "");

            if (user == null)
            {
                return Unauthorized();
            }

            // TODO, null check
            var s = _configuration["Authentication:Secret"];

            if (string.IsNullOrEmpty(s))
            {
                return StatusCode(500, "Error ocurred.");
            }

            var secKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(s));
            var signCreds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("sub", user.UserName),
                new Claim("profile", user.Role.ToString())
            };

            var jwt = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(24),
                signCreds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(token);

        }

        private PropellerUser ValidateCreds(string uid, string pwd)
        {
            return new PropellerUser
            {
                UserName = "yami.soki@gmail.com",
                Role = 99,
            };
        }

    }
}

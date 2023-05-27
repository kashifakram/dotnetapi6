using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityAPI.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // if we are not going to use a class outsite of another class then it should be defined within parent class
        public class AuthenticationRequestBody 
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        private class CityInfoUser
        {
            internal int UserId { get; init; }
            private string UserName { get; set; }
            internal string FirstName { get; set; }
            internal string LastName { get; set; }
            internal string City { get; set; }

            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            // Step 1: validate credentials
            var user = ValidateUserCredentials(authenticationRequestBody.UserName, authenticationRequestBody.Password);

            if (user == null) return Unauthorized();

            // Step 2: create token
            var securityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

            var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenClaims = new List<Claim>
            {
                new("sub", user.UserId.ToString()),
                new("given_name", user.FirstName),
                new("last_name", user.LastName),
                new("city", user.City)
            };

            var jwtSecurityToken = new JwtSecurityToken(_configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"], tokenClaims, DateTime.UtcNow, DateTime.UtcNow.AddHours(1),
                signingCreds);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private static CityInfoUser? ValidateUserCredentials(string? userName, string? password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)) return null;

            return new CityInfoUser(1, "kashifakram", "Kashif", "Akram", "Faisalabad");
        }
    }
}

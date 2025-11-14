using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lection1114.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly string _secretKey = "12345678123456781234567812345678";

        //optional
        private static readonly string _issuer = "myapp"; // ne nado v api
        private static readonly string _audience = "myapp-users";
        [AllowAnonymous]
        [HttpGet("token")]
        public ActionResult<string> GenerateToken()
        {
            return GenerateToken(1, "user1");
        }

        public static string GenerateToken(int id, string login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //var claims = new Claim[]
            //{
            //    new(ClaimTypes.NameIdentifier, id.ToString()),
            //    new (ClaimTypes.Name, login),
            //    new (ClaimTypes.Role, "guest"), // neobyazatelno
            //    new (ClaimTypes.Role, "customer"), // neobyazatelno
            //};

            var claims = new Claim[]
            {
                new ("id", id.ToString()),
                new ("name", login),
                new ("role", "guest"), // neobyazatelno
                new ("role", "customer"), // neobyazatelno
            };


            var token = new JwtSecurityToken(signingCredentials: credentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
        //optional
        issuer: _issuer,
            audience: _audience);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // GET: api/<ValuesController>
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ValuesController>/5
        [Authorize]
        [HttpGet("{id:int}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [Authorize(Roles = "Admin,Manager")] // admin or manager
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Manager")] // admin and manager
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

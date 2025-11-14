using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Lection1113
{
    public class AuthService
    {
        // mojno proverit token na jwt.io

        private readonly string _secretKey = "12345678123456781234567812345678"; // nado 32 symvola

        //optional
        private readonly string _issuer = "myapp"; // kto izdatel / kto vidaet
        private readonly string _audience = "myapp-users"; // kto potrebytel / kto polzyetsa

        public string GenerateToken(int id, string login)
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
                expires: DateTime.UtcNow.AddMinutes(15));
            //optional
            //issuer: _issuer,
            //audience: _audience);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumbers = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomNumbers);

            return Convert.ToBase64String(randomNumbers);
        }

        public bool IsValidToken(string token)
        {
            try
            {
                var tokenParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,

                };

                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoWithDatabase.Services.Interfaces;

namespace TodoWithDatabase.Services
{
    public class TokenService : ITokenService
    {
        public string GenerateToken(string userName, string role, bool testing)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes("");

            if (testing)
            {
                key = Encoding.ASCII.GetBytes("secretTestingKey");
            }
            else
            {
                key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("TODOTOKENSECRET"));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key)
                , SecurityAlgorithms.HmacSha256Signature)
            };

            string token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

            return token;
        }
    }
}

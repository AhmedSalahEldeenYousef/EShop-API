using Eshop.Core.Entities.Auth;
using Eshop.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eshop.Infrastructure.Repositories.Service
{
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration _configuration;
        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAndGenrateToken(AppUser user)
        {
            // Implementation for generating token goes here
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            };
            var Security = _configuration["Token:Secret"];
            var Key = Encoding.ASCII.GetBytes(Security);// new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(security));
            SigningCredentials signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha256);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                Issuer = _configuration["Token:Issuer"],
                SigningCredentials = signingCredentials,
                NotBefore = DateTime.UtcNow
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenCreated = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(tokenCreated);
        }
    }
}

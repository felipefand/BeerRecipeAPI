using BeerRecipeAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeerRecipeAPI.Auth
{
    public class GenerateToken
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public GenerateToken()
        {
            _tokenConfiguration = new TokenConfiguration();
        }
        public GenerateToken(TokenConfiguration tokenConfiguration)
        {
            _tokenConfiguration = tokenConfiguration;
        }

        public string GenerateJwt(User user)
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Username));
            claims.Add(new Claim(ClaimTypes.Role, user.Role));

            return Generate(claims);
        }

        public string GenerateJwt()
        {
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, "usuario"));
            claims.Add(new Claim(ClaimTypes.Role, "User"));

            return Generate(claims);
        }

        private string Generate(List<Claim> claims)
        {
            claims.Add(new Claim("sub", _tokenConfiguration.Subject));
            claims.Add(new Claim("module", _tokenConfiguration.Module));

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenConfiguration.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtToken = new JwtSecurityToken(
                issuer: _tokenConfiguration.Issuer,
                expires: DateTime.UtcNow.AddHours(_tokenConfiguration.ExpirationTimeInHours),
                audience: _tokenConfiguration.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            return tokenHandler.WriteToken(jwtToken);
        }
    }
}

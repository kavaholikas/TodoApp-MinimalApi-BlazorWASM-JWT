using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoJWT.Models;

namespace TodoJWT.API.Services
{
    public class JwtGenerator
    {
        public string GenerateJWT(User user, IConfiguration configuration)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("ID", user.UserID.ToString()),
                new Claim(ClaimTypes.Role, "user")
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "admin"));
            }

            JwtSecurityToken token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

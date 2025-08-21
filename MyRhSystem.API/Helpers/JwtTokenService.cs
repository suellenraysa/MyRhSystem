using Microsoft.IdentityModel.Tokens;
using MyRhSystem.Domain.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyRhSystem.API.Helpers;

public static class JwtTokenService
{
    public static string IssueToken(IConfiguration config, User user)
    {
        var section = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(section["Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("sub", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var expires = DateTime.UtcNow.AddMinutes(double.Parse(section["ExpireMinutes"]));

        var token = new JwtSecurityToken(
            issuer: section["Issuer"],
            audience: section["Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

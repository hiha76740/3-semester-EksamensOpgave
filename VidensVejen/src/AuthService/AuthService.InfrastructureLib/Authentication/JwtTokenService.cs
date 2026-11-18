using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.ApplicationLib.Authentication;
using AuthService.DomainLib.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VidensVejen.ContractsLib.Authentication;

namespace AuthService.InfrastructureLib.Authentication;

/// <summary>
/// 
/// </summary>
/// <param name="configuration"></param>
public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    string ITokenService.CreateToken(AuthUser authUser)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, authUser.Id.Value.ToString()),
            new(ClaimTypes.Name, authUser.Email.Value),
        };

        foreach (var permission in authUser.Permissions)
        {
            claims.Add(new Claim(CustomClaimTypes.Permission, permission.Name));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));

        // 512bits 64bytes = 64 char
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Jwt:Issuer"),
            audience: configuration.GetValue<string>("Jwt:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    ClaimsPrincipal ITokenService.GetPrincipalFromExpiredToken(string expiredAccessToken)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = configuration["Jwt:Audience"],
            ValidateLifetime = false,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(expiredAccessToken, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken || jwtSecurityToken.Header.Alg
            .Equals(SecurityAlgorithms.HmacSha512, StringComparison.OrdinalIgnoreCase) == false)
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
}

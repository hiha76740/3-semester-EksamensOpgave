using AuthService.ApplicationLib.Authentication;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.ValueObjects;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;

namespace AuthService.ApplicationLib.Handlers.Authentication;

/// <summary>
/// 
/// </summary>
/// <param name="authUserRepo"></param>
/// <param name="tokenService"></param>
/// <param name="refreshTokenService"></param>
public class RefreshTokenHandler(
    IAuthUserRepository authUserRepo,
    ITokenService tokenService,
    IRefreshTokenService refreshTokenService
    ) : IRefreshTokenHandler
{
    async Task<TokenDto?> IRefreshTokenHandler.HandleAsync(RefreshTokenCommand command)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(command.ExpiredAccessToken);

        if (principal.Identity == null || principal.Identity.Name == null)
            throw new InvalidOperationException("The principal must contain an identity with a name.");

        var email = new Email(principal.Identity.Name);

        var authUser = await authUserRepo.GetAuthUserByEmailAsync(email);

        if (
            authUser == null
            || command.RefreshToken == null
            || authUser.RefreshToken != command.RefreshToken
            || authUser.RefreshTokenExpiryTime <= DateTime.UtcNow
            )
        {
            return null;
        }

        var token = tokenService.CreateToken(authUser);
        var refreshToken = refreshTokenService.GenerateRefreshToken();

        authUser.SetRefreshToken(refreshToken, false);

        await authUserRepo.SaveAsync();

        var dto = new TokenDto(token, refreshToken);

        return dto;
    }
}

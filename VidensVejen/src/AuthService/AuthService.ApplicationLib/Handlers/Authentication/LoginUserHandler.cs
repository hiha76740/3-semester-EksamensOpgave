using AuthService.ApplicationLib.Authentication;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Enums;
using AuthService.DomainLib.ValueObjects;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Handlers.Authentication;

/// <summary>
/// 
/// </summary>
/// <param name="authUserRepo"></param>
/// <param name="passwordHashService"></param>
/// <param name="tokenService"></param>
/// <param name="refreshTokenService"></param>
public class LoginUserHandler(
    IAuthUserRepository authUserRepo,
    IPasswordHashService passwordHashService,
    ITokenService tokenService,
    IRefreshTokenService refreshTokenService
    ) : ILoginUserHandler
{
    async Task<TokenDto?> ILoginUserHandler.HandleAsync(LoginUserCommand command)
    {
        try
        {
            var email = new Email(command.Email);

            var authUser = await authUserRepo.GetAuthUserByEmailAsync(email);

            if (authUser == null)
                return null;

            if (passwordHashService.Verify(command.Password, authUser.PasswordHash) == false)
                return null;

            if (authUser.Status != AuthUserStatus.Active)
                throw new UserNotActivatedException("Unable to log in, user not activated");

            var token = tokenService.CreateToken(authUser);

            var refreshToken = refreshTokenService.GenerateRefreshToken();

            authUser.SetRefreshToken(refreshToken, true);

            await authUserRepo.SaveAsync();

            var dto = new TokenDto(token, refreshToken);

            return dto;
        }
        catch (UserNotActivatedException)
        {
            throw;
        }
        catch (Exception)
        {

            throw;
        }
    }
}

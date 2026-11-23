using AuthService.ApplicationLib.Authentication;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Handlers.Users;

/// <summary>
/// 
/// </summary>
public class UpdateUserPasswordHandler(
    IAuthUserRepository authUserRepo,
    IPasswordHashService passwordHashService,
    ILogger<UpdateUserPasswordHandler> logger
    ) : IUpdateUserPasswordHandler
{
    async Task IUpdateUserPasswordHandler.HandleAsync(UpdateAuthUserPasswordCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning update of password for user {UserId}", command.Id);

            var authUserId = new AuthUserId(command.Id);

            var authUser = await authUserRepo.GetAuthUserByIdAsync(authUserId)
                ?? throw new NotFoundException("User was not found");

            if (passwordHashService.Verify(command.OldPassword, authUser.PasswordHash) == false)
                throw new InvalidOperationException("Old password was incorrect");

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Updating auth user password");

            var hashedPassword = passwordHashService.Hash(command.Password);

            authUser.ChangePassword(hashedPassword);

            await authUserRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Password updated for user {UserId} succesfully", authUser.Id.Value);
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            throw;
        }
    }
}

using Microsoft.Extensions.Logging;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using UserService.FacadeLib.Commands.DTOs;
using UserService.FacadeLib.Commands.Interfaces;
using UserService.FacadeLib.Queries.DTOs;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class UpdateUserHandler(
    IUserRepository userRepo,
    ILogger<UpdateUserHandler> logger
    ) : IUpdateUserHandler
{
    async Task<UserDto> IUpdateUserHandler.HandleAsync(UpdateUserCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to update user {UserId}", command.Id);

            var id = new UserId(command.Id);

            var user = await userRepo.GetUserByIdAsync(id)
                ?? throw new NotFoundException("User not found");

            var changeMade = false;


            if (command.Firstname.Equals(user.Firstname, StringComparison.OrdinalIgnoreCase) == false)
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Updating firstname {Firstname} for user {UserId}", command.Firstname, command.Id);

                user.ChangeFirstname(command.Firstname);
                changeMade = true;
            }

            if (command.Surname.Equals(user.Surname, StringComparison.OrdinalIgnoreCase) == false)
            {
                if (logger.IsEnabled(LogLevel.Information))
                    logger.LogInformation("Updating surname {Surname} for user {UserId}", command.Surname, command.Id);

                user.ChangeSurname(command.Surname);
                changeMade = true;
            }

            if (changeMade == true)
                await userRepo.SaveAsync();


            return new UserDto(
                user.Id.Value,
                user.Firstname,
                user.Surname,
                user.EmailAddress.Value
                );

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

using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using AuthService.FacadeLib.Queries.DTOs;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Handlers.Users;

/// <summary>
/// 
/// </summary>
/// <param name="authUserRepo"></param>
/// <param name="permissionRepo"></param>
/// <param name="logger"></param>
public class UpdateUserPermissionsHandler(
    IAuthUserRepository authUserRepo,
    IPermissionRepository permissionRepo,
    ILogger<UpdateUserPermissionsHandler> logger
    ) : IUpdateUserPermissionsHandler
{
    async Task<AuthUserDto> IUpdateUserPermissionsHandler.HandleAsync(UpdateAuthUserPermissionsCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning update of permissions for user {UserId}", command.Id);

            var authUserId = new AuthUserId(command.Id);

            var authUser = await authUserRepo.GetAuthUserByIdAsync(authUserId)
                ?? throw new NotFoundException("User was not found");

            var accessPermissionList = new List<AccessPermission>();

            foreach (var permission in command.PermissionsIds)
            {
                var id = new AccessPermissionId(permission);
                var accessPermission = await permissionRepo.GetPermissionByIdAsync(id)
                    ?? throw new NotFoundException($"Permission Id {permission} was not found");

                accessPermissionList.Add(accessPermission);
            }

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Updating permissions for user {UserId}", authUser.Id.Value);

            foreach (var permission in accessPermissionList)
            {
                if (authUser.Permissions.Contains(permission) == false)
                    authUser.AddPermission(permission);
            }

            foreach (var permission in authUser.Permissions.ToList())
            {
                if (accessPermissionList.Contains(permission) == false)
                    authUser.RemovePermission(permission);
            }

            authUserRepo.SetOriginalRowVersion(authUser, command.RowVersion);

            await authUserRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Permissions for user {UserId} updated successfully", authUser.Id.Value);

            return new AuthUserDto(
                authUser.Id.Value,
                authUser.Email.Value,
                authUser.Permissions
                .Select(
                    p => new AccessPermissionDto(
                        p.Id.Value,
                        p.Name))
                .ToList(),
                authUser.RowVersion
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

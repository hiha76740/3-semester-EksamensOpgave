using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.AuthService;
using VidensVejen.ContractsLib.Responses.AuthService;

namespace AuthService.Api.Mappers;

internal static class UsersMapper
{
    internal static RegisterUserCommand AsRegisterCommand(this RegisterUserRequest request)
    {
        var output = new RegisterUserCommand(
            request.Firstname,
            request.Surname,
            request.Email,
            request.Password,
            request.PermissionsIds
            );

        return output;
    }

    internal static AuthUserResponse AsAuthUserResponse(this AuthUserDto dto)
    {
        var output = new AuthUserResponse(
            dto.Id,
            dto.Email,
            dto.Permissions.Select(p => new PermissionResponse(p.Id, p.Name)).ToList()
            );

        return output;
    }

    internal static UpdateAuthUserPermissionsCommand AsUpdatePermissionsCommand(this UpdateAuthUserPermissionsRequest request, Guid id)
    {
        var output = new UpdateAuthUserPermissionsCommand(
            id,
            request.PermissionsIds
            );

        return output;
    }

    internal static UpdateAuthUserEmailCommand AsEmailCommand(this UpdateAuthUserEmailRequest request, Guid id)
    {
        var output = new UpdateAuthUserEmailCommand(
            id,
            request.Email
            );

        return output;
    }

    internal static UserEmailResponse AsEmailResponse(this AuthUserEmailDto dto)
    {
        var output = new UserEmailResponse(
            dto.Email
            );

        return output;
    }

    internal static UpdateAuthUserPasswordCommand AsPasswordCommand(this UpdateAuthUserPasswordRequest request, Guid id)
    {
        var output = new UpdateAuthUserPasswordCommand(
            id,
            request.Password,
            request.OldPassword
            );

        return output;
    }
}

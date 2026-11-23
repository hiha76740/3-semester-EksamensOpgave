using UserService.FacadeLib.Commands.DTOs;
using UserService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Events;
using VidensVejen.ContractsLib.Requests.UserService;
using VidensVejen.ContractsLib.Responses.UserService;

namespace UserService.Api.Mappers;

internal static class UserMapper
{
    internal static UserResponse AsResponse(this UserDto dto)
    {
        var output = new UserResponse(
            dto.Id,
            dto.Firstname,
            dto.Surname,
            dto.Email,
            dto.RowVersion
            );

        return output;
    }

    internal static CreateUserCommand AsCreateCommand(this UserRegisteredIntegrationEvent integrationEvent)
    {
        var output = new CreateUserCommand(
            integrationEvent.UserId,
            integrationEvent.Firstname,
            integrationEvent.Surname,
            integrationEvent.Email
            );

        return output;
    }

    internal static UpdateUserCommand AsUpdateCommand(this UpdateUserRequest request, Guid id)
    {
        var output = new UpdateUserCommand(
            id,
            request.Firstname,
            request.Surname,
            request.RowVersion
            );

        return output;
    }
}

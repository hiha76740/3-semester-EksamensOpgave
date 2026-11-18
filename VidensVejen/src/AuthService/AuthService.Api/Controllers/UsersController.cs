using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AuthService.Api.Mappers;
using AuthService.FacadeLib.Commands.Interfaces;
using AuthService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.BuildingBlocksLib.Helpers;
using VidensVejen.ContractsLib.Requests.AuthService;
using VidensVejen.ContractsLib.Responses.AuthService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.Api.Controllers;

[Route("/v1/auth/users")]
[ApiController]
public class UsersController(
    IRegisterUserHandler register,
    IAuthUsersQueries queries,
    IUpdateUserPermissionsHandler updatePermissions,
    IDeleteUserHandler delete,
    IUpdateUserEmailHandler updateUserEmail,
    IUpdateUserPasswordHandler updateUserPassword
    ) : ControllerBase
{
    [HttpPost("register")]
    [EndpointSummary("This endpoint will register a user")]
    [EndpointDescription("Register a user when all required info is given")]
    [ProducesResponseType(StatusCodes.Status201Created, Description = "User was registered succesfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error doing registration of user")]
    public async Task<ActionResult> Register(
        [Description("The values need to create a user")] RegisterUserRequest request)
    {
        try
        {
            await register.HandleAsync(request.AsRegisterCommand());

            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a user")]
    [EndpointDescription("Gets the details of a single instance of a user")]
    [ProducesResponseType<AuthUserResponse>(StatusCodes.Status200OK, "application/json", Description = "Returns a single user")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the requested user")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User was not found")]
    public async Task<ActionResult<AuthUserResponse>> GetUser(
        [Required][Description("A unique identifier for a user")] Guid id)
    {
        try
        {
            var user = await queries.GetByIdAsync(id);

            if (user == null)
                return NotFound("No user was found with the specified id");

            return Ok(user.AsAuthUserResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/permissions")]
    [EndpointSummary("Updates user permissions")]
    [EndpointDescription("Updates the permissions of a single instance of a user")]
    [ProducesResponseType<AuthUserResponse>(StatusCodes.Status200OK, "application/json", Description = "Returns the updated user")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to update permissions")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User was not found")]
    public async Task<ActionResult<AuthUserResponse>> UpdateUserPermissions(
        [Required][Description("A unique identifier for a user")] Guid id,
        [Description("The object needed to update user permissions")] UpdateAuthUserPermissionsRequest request)
    {
        try
        {
            var user = await updatePermissions.HandleAsync(request.AsUpdatePermissionsCommand(id));

            return Ok(user.AsAuthUserResponse());
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Deletes a user")]
    [EndpointDescription("Deletes a single instance of a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "User was deleted successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the requested user")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User was not found")]
    public async Task<ActionResult> DeleteUser(
        [Required][Description("A unique identifier for a user")] Guid id)
    {
        try
        {
            await delete.HandleAsync(id);

            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/email")]
    [EndpointSummary("Updates email")]
    [EndpointDescription("Updates the email of a single instance of a user")]
    [ProducesResponseType<UserEmailResponse>(StatusCodes.Status200OK, "application/json", Description = "Returns the updated email")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to update email")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User was not found")]
    public async Task<ActionResult<UserEmailResponse>> UpdateEmail(
        [Required][Description("A unique identifier for a user")] Guid id,
        [Required][Description("The value of the new email")] UpdateAuthUserEmailRequest request)
    {
        try
        {
            var email = await updateUserEmail.HandleAsync(request.AsEmailCommand(id));

            return Ok(email.AsEmailResponse());
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}/password")]
    [EndpointSummary("Updates password")]
    [EndpointDescription("Updates the password of a single instance of a user")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Password was updated successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to update password")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User was not found")]
    public async Task<ActionResult> UpdatePassword(
        [Required][Description("A unique identifier for a user")] Guid id,
        [Required][Description("The value of the new password")] UpdateAuthUserPasswordRequest request)
    {
        try
        {
            await updateUserPassword.HandleAsync(request.AsPasswordCommand(id));

            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("Me")]
    [EndpointSummary("This endpoint will get current user information")]
    [EndpointDescription("Get current user information, like username, firstname and lastname")]
    [ProducesResponseType<AuthUserResponse>(StatusCodes.Status200OK, "application/json", Description = "Returns info of current user")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while getting current user info")]
    public async Task<ActionResult<AuthUserResponse>> GetCurrentUserInfo()
    {
        try
        {
            var userId = UserHelper.GetCurrentUserId(HttpContext.User);

            if (userId == null)
                return BadRequest("User id not valid");

            var dto = await queries.GetCurrentUserInfo(userId.Value);

            if (dto == null)
                return NotFound("user not found");

            return Ok(dto.AsAuthUserResponse());

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

using System.ComponentModel;
using AuthService.Api.Mappers;
using AuthService.Api.Services;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.AuthService;

namespace AuthService.Api.Controllers;

[Route("/v1/auth")]
[ApiController]
public class AuthController(
    ILoginUserHandler login,
    IRefreshTokenHandler refresh,
    ICookieService cookieService
    ) : ControllerBase
{

    [HttpPost("login")]
    [EndpointSummary("This endpoint will log in a user")]
    [EndpointDescription("Logs in a user when username and password is correct")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "User was logged in succesfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error doing log in")]
    public async Task<ActionResult> Login(
        [Description("The values need to login a user")] LoginUserRequest request)
    {
        try
        {
            var token = await login.HandleAsync(request.AsLoginCommand());

            if (token == null)
                return BadRequest("Invalid username or password");

            cookieService.SetTokenInsideCookie(token, HttpContext);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("logout")]
    [Authorize]
    [EndpointSummary("Logout the current logged in user")]
    [EndpointDescription("Log out the current user from the system")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "User logged out successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to logout current user")]
    public ActionResult Logout()
    {
        try
        {
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return NoContent();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("refresh-token")]
    [Authorize]
    [EndpointSummary("This endpoint will create a new refresh token")]
    [EndpointDescription("Creates a new refresh token when all required info is given")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Refresh token was created succesfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error doing creation of refresh token")]
    public async Task<ActionResult> Refresh()
    {
        try
        {
            HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            HttpContext.Request.Cookies.TryGetValue("accessToken", out var expiredAccessToken);

            if (refreshToken == null || expiredAccessToken == null)
                return BadRequest("Invalid request");

            var request = new RefreshTokenRequest(expiredAccessToken, refreshToken);

            var result = await refresh.HandleAsync(request.AsRefreshCommand());

            if (result == null || result.AccessToken == null || result.RefreshToken == null)
                return Unauthorized("Invalid refresh token");

            cookieService.SetTokenInsideCookie(result, HttpContext);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

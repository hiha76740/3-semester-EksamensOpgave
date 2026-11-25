using AuthService.Api.Mappers;
using AuthService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Responses.AuthService;

namespace AuthService.Api.Controllers;

[Route("/v1/permissions")]
[ApiController]
public class PermissionsController(
    IPermissionsQueries queries
    ) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all permissions")]
    [EndpointDescription("Gets a list of all permissions")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Returns a list of all permissions")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while getting all permissions")]

    public async Task<ActionResult<IReadOnlyList<PermissionResponse>>> GetAllPermissions()
    {
        try
        {
            var list = await queries.GetAllPermissionsAsync();

            var response = new List<PermissionResponse>();

            foreach (var item in list)
            {
                response.Add(item.AsResponse());
            }

            return Ok(response);

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

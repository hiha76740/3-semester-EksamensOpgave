using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExhibitionService.Api.Mappers;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.ExhibitionService;
using VidensVejen.ContractsLib.Responses.ExhibitionService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.Api.Controllers;

[Route("api/exhibitions")]
[ApiController]
public class ExhibitionsController(
    ICreateExhibitionHandler create,
    IUpdateExhibitionHandler update,
    IDeleteExbitionHandler delete,
    IExhibitionQueries queries
    ) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a single instance of a exhibition")]
    [EndpointDescription("Gets the details of a exhibition")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Returns a single exhibition")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the requested exhibition")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Exhibition was not found")]
    public async Task<ActionResult<ExhibitionResponse>> GetExhibition(Guid id)
    {
        try
        {
            var dto = await queries.GetExhibitionByIdAsync(id);

            if (dto == null)
                return NotFound("Exhibition was not found");

            return Ok(dto.AsResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update a single instance of a exhibition")]
    [EndpointDescription("Updates the details of a exhibition")]
    [ProducesResponseType<ExhibitionResponse>(StatusCodes.Status200OK, "application/json", Description = "Returns the updated instance of the requested exhibition")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to update the requested exhibition")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Exhibition was not found")]
    public async Task<ActionResult<ExhibitionResponse>> UpdateExhibition(
        [Required][Description("A unique identifier for a exhibition")] Guid id,
        [Required][Description("The object used for updating Exhibition, Subject and Topics")] UpdateExhibitionRequest request)
    {
        try
        {
            var dto = await update.HandleAsync(request.AsUpdateCommand(id));

            return Ok(dto.AsResponse());
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
    [EndpointSummary("Delete a exhibition")]
    [EndpointDescription("Deletes a single instance of the requested exhibition")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Deletion of requested exhibition completed successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the requested exhibition")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Exhibition was not found")]
    public async Task<ActionResult> DeleteExhibition(
        [Required][Description("A unique identifier for a exhibition")] Guid id)
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

    [HttpPost]
    [EndpointSummary("Create a exhibition")]
    [EndpointDescription("Creates a new instance of exhibition")]
    [ProducesResponseType(StatusCodes.Status201Created, Description = "Exhibition created successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create exhibition")]
    public async Task<ActionResult> CreateExhibition(
         [Required][Description("Object used to create a exhibition")] CreateExhibitionRequest request)
    {
        try
        {
            await create.HandleAsync(request.AsCreateCommand());

            return Created();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("names")]
    [EndpointSummary("Get all names of exhibitions")]
    [EndpointDescription("Gets the name of all exhibitions")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Returns a list of exhibition names")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get all exhibition names")]
    public async Task<ActionResult<IReadOnlyList<ExhibitionNameResponse>>> GetAllExhibitionNames()
    {
        try
        {
            var list = await queries.GetAllExhibitionNamesAsync();

            var response = new List<ExhibitionNameResponse>();

            foreach (var exhibitionName in list)
            {
                response.Add(exhibitionName.AsNameResponse());
            }

            return response;
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("inuse")]
    [EndpointSummary("Check if image is in use")]
    [EndpointDescription("Checks if the requested image is in use by any exhibition ")]
    [ProducesResponseType<bool>(StatusCodes.Status200OK, "application/json", Description = "Returns true if image is in use")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while checking requested image")]
    public async Task<ActionResult<bool>> CheckIfInUse(
        [Required][Description("Unique identifier for a image")] Guid id)
    {
        try
        {
            var inUse = await queries.CheckIfInUseAsync(id);

            return Ok(inUse);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }
}

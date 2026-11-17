using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
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
    ICreateExhibition create,
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
    [RequestSizeLimit(6 * 1024 * 1024)]
    [EndpointSummary("Create a exhibition")]
    [EndpointDescription("Creates a new instance of exhibition")]
    [ProducesResponseType(StatusCodes.Status201Created, Description = "Quiz created successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create exhibition")]
    public async Task<ActionResult> CreateExhibition(
         [Required][FromForm][Description("String of data used to create a exhibition")] string data,
         [FromForm][Description("Image file for theme")] IFormFile themeFile)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            var options = jsonOptions;
            var request = JsonSerializer.Deserialize<CreateExhibitionRequest>(data, options);

            if (request == null)
                return BadRequest("Could not deserialize data");

            const long MaxFileSize = 5 * 1024 * 1024;

            if (themeFile.Length == 0)
                return BadRequest("Please choose a image file");

            if (themeFile.Length > MaxFileSize)
                return BadRequest($"Size limit of image is 5 MB.");

            var extension = Path.GetExtension(themeFile.FileName)
                .ToLowerInvariant();

            if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png")
            {
                return BadRequest("Only jpg- and png-image files are allowed");
            }

            var imageStream = themeFile.OpenReadStream();


            await create.HandleAsync(request.AsCreateCommand(imageStream, extension));

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
}

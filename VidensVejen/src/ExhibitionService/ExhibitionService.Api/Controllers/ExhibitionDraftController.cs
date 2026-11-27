using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExhibitionService.Api.Mappers;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Responses.ExhibitionService;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;
using VidensVejen.SharedKernelLib.Exceptions;


namespace ExhibitionService.Api.Controllers;

[Route("v1/exhibitions-drafts")]
[ApiController]
public class ExhibitionDraftController(
    ICreateExhibitionDraftHandler create,
    ICreateExhibitionHandler publish,
    IUpdateExhibitionDraftHandler update,
    IDeleteExhibitionDraftHandler delete,
    IExhibitionDraftQueries queries
    ) : ControllerBase
{

    [HttpGet]
    [EndpointSummary("Get exhibition drafts")]
    [EndpointDescription("Gets all exhibition drafts")]
    [ProducesResponseType<IReadOnlyList<ExhibitionDraftResponse>>(StatusCodes.Status200OK, "Application/json", Description = "Returns all exhibition drafts")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get all exhibition drafts")]
    public async Task<ActionResult<IReadOnlyList<ExhibitionDraftResponse>>> GetExhibitionDrafts()
    {
        var list = await queries.GetAllExhibitionDrafts();

        var response = new List<ExhibitionDraftResponse>();

        foreach (var dto in list)
        {
            response.Add(dto.AsResponse());
        }

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a single instance of an exhibition draft")]
    [EndpointDescription("Gets the details of an exhibition draft")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Returns a single exhibition draft")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the requested exhibition draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Exhibition draft was not found")]
    public async Task<ActionResult<ExhibitionDraftResponse>> GetExhibitionDraft(Guid id)
    {
        try
        {
            var dto = await queries.GetExhibitionDraftByIdAsync(id);

            if (dto == null)
                return NotFound("Exhibition draft was not found");

            return Ok(dto.AsResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [EndpointSummary("Create an exhibition draft")]
    [EndpointDescription("Creates a new instance of ExhibitionDraft")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Exhibition draft created successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create exhibition draft")]
    public async Task<ActionResult<Guid>> CreateExhibitionDraft()
    {
        try
        {
            var id = await create.HandleAsync();

            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("from-exhibition/{exhibitionId:guid}")]
    [EndpointSummary("Create an edit draft for a published exhibition")]
    [ProducesResponseType<Guid>(StatusCodes.Status200OK, Description = "Edit draft for exhibition created successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create edit draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Original exhibition not found")]
    public async Task<ActionResult<Guid>> CreateRevision(
        [Required][Description("A unique identifier for a exhibition")] Guid exhibitionId,
        [Required][Description("Object used to create a revision for a  exhibition")] UpdateExhibitionDraftRequest request)
    {
        try
        {
            var id = await create.CreateRevisionAsync(request.AsUpdateDraftCommand(exhibitionId));

            return Ok(id);
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

    [HttpPost("{id:guid}/publish-revision")]
    [EndpointSummary("Publish a revision of an existing exhibition")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PublishRevision(
        [Required][Description("A unique identifier for an exhibition draft")] Guid id,
        [Required][Description("Object used to publish a exhibition")] CreateExhibitionRequest request)
    {
        try
        {
            await publish.PublishRevisionAsync(id, request.AsCreateCommand());
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update exhibition draft")]
    [EndpointDescription("Saves the exhibition draft. Every field may be empty until the draft is published")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Exhibition draft was saved successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to save the Exhibition draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Exhibition draft not found")]
    public async Task<ActionResult> UpdateExhibitionDraft(
    [Required][Description("A unique identifier for an exhibition draft")] Guid id,
    [Required][Description("Object used to save as exhibition draft")] UpdateExhibitionDraftRequest request)
    {
        try
        {
            await update.HandleAsync(request.AsUpdateDraftCommand(id));

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

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Delete an exhibition draft")]
    [EndpointDescription("Delete a single exhibition draft")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Exhibition draft was successfully deleted")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the exhibition draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Exhibition draft was not found")]
    public async Task<ActionResult> DeleteExhibitionDraft(
    [Required][Description("A unique identifier for an exhibition draft")] Guid id)
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


}

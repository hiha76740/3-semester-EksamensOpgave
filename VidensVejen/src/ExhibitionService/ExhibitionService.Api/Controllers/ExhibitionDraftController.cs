using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExhibitionService.Api.Mappers;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Responses.ExhibitionService;
using VidensVejen.SharedKernelLib.Exceptions;


namespace ExhibitionService.Api.Controllers;

[Route("v1/exhibitions-drafts")]
[ApiController]
public class ExhibitionDraftController : ControllerBase
{
    public class ExhibitionDraftsController(
    ICreateExhibitionDraftHandler create,
    IUpdateExhibitionDraftHandler update,
    IDeleteExhibitionDraftHandler delete,
    IExhibitionDraftQueries queries) : ControllerBase
    {
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
        [Required][Description("A unique identifier for an exhibition draft")] Guid id,
        [Required][Description("The object used to delete an exhibition draft")] DeleteExhibitionDraftRequest request)
        {
            try
            {
                await delete.HandleAsync(request.AsDeleteDraftCommand(id));

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
}

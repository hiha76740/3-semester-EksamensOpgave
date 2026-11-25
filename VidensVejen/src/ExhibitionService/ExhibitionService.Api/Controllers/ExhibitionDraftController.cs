using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ExhibitionService.Api.Controllers;

[Route("v1/exhibitions-drafts")]
[ApiController]
public class ExhibitionDraftController : ControllerBase
{
    public class ExhibitionDraftsController(
    ICreateExhibitionDraftHandler create) : ControllerBase
    {
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
    }
}

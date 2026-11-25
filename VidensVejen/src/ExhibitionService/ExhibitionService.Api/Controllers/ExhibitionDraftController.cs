using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace ExhibitionService.Api.Controllers;

[Route("v1/exhibitions-draft")]
[ApiController]
public class ExhibitionDraftController : ControllerBase
{
    public class ExhibitionDraftsController(
    ICreateExhibitionDraftHandler create) : ControllerBase
    {
        [HttpPost]
        [EndpointSummary("Create an exhibition draft")]
        [EndpointDescription("Creates a new instance of ExhibitionDraft")]
        [ProducesResponseType(StatusCodes.Status201Created, Description = "Exhibition draft created successfully")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create exhibition draft")]
        public async Task<ActionResult> CreateExhibitionDraft()
        {
            try
            {
                await create.HandleAsync();

                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

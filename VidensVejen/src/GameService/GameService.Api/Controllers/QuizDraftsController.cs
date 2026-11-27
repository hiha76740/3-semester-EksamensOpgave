using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameService.Api.Mappers.Quizzes;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.Api.Controllers;

[Route("/v1/games/quiz-drafts")]
[ApiController]
public class QuizDraftsController(
    ICreateQuizDraftHandler create,
    IUpdateQuizDraftHandler update,
    IDeleteDraftHandler delete,
    IQuizDraftQueries queries
    ) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create a quiz draft")]
    [EndpointDescription("Creates a new quiz draft. Every field may be empty until the draft is published")]
    [ProducesResponseType<QuizDraftIdResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the id of the created quiz draft")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create the quiz draft")]
    public async Task<ActionResult<QuizDraftIdResponse>> CreateQuizDraft()
    {
        try
        {
            var id = await create.HandleAsync();

            var response = new QuizDraftIdResponse(id);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a quiz draft")]
    [EndpointDescription("Gets a single quiz draft so it can be opened and edited again")]
    [ProducesResponseType<QuizDraftResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the requested quiz draft")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the quiz draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz draft was not found")]
    public async Task<ActionResult<QuizDraftResponse>> GetQuizDraft(
        [Required][Description("A unique identifier for a quiz draft")] Guid id)
    {
        try
        {
            var dto = await queries.GetQuizDraftByIdAsync(id);

            if (dto == null)
                return NotFound("Quiz draft was not found");

            return Ok(dto.AsResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [EndpointSummary("Get quiz drafts")]
    [EndpointDescription("Gets all quiz drafts")]
    [ProducesResponseType<List<QuizDraftResponse>>(StatusCodes.Status200OK, "Application/json", Description = "Returns all quiz drafts")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get all quiz drafts")]
    public async Task<ActionResult<List<QuizDraftResponse>>> GetQuizDrafts()
    {
        try
        {
            var list = await queries.GetQuizDraftsAsync();

            var response = new List<QuizDraftResponse>();

            foreach (var dto in list)
            {
                response.Add(dto.AsResponse());
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update a quiz draft")]
    [EndpointDescription("Saves the quiz draft. Every field may be empty until the draft is published")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Quiz draft was saved successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to save the quiz draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz draft was not found")]
    public async Task<ActionResult> UpdateQuizDraft(
        [Required][Description("A unique identifier for a quiz draft")] Guid id,
        [Required][Description("Object used to save a quiz draft")] UpdateQuizDraftRequest request)
    {
        try
        {
            await update.HandleAsync(request.AsUpdateCommand(id));

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
    [EndpointSummary("Delete a quiz draft")]
    [EndpointDescription("Delete a single quiz draft so it can be opened and edited again")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Draft was deleted successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the quiz draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz draft was not found")]
    public async Task<ActionResult<QuizDraftResponse>> DeleteQuizDraft(
        [Required][Description("A unique identifier for a quiz draft")] Guid id,
        [Required][Description("The object used to delete a quiz draft")] DeleteQuizDraftRequest request)
    {
        try
        {
            await delete.HandleAsync(request.AsDeleteCommand(id));

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

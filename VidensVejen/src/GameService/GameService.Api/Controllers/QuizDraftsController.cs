using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameService.Api.Mappers.Quizzes;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.Api.Controllers;

[Route("/v1/games/quiz-drafts")]
[ApiController]
public class QuizDraftsController(
    ICreateQuizDraftHandler create,
    IPublishQuizHandler publish
    ) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create a quiz draft")]
    [EndpointDescription("Creates a new quiz draft. Every field may be empty until the draft is published")]
    [ProducesResponseType<QuizDraftIdResponse>(StatusCodes.Status201Created, "Application/json", Description = "Returns the id of the created quiz draft")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create the quiz draft")]
    public async Task<ActionResult<QuizDraftIdResponse>> CreateQuizDraft(
        [Required][Description("Object used to create a quiz draft")] CreateQuizRequest request)
    {
        try
        {
            var id = await create.HandleAsync(request.AsCreateCommand());

            var response = new QuizDraftIdResponse(id);

            return Created($"/v1/games/quiz-drafts/{id}", response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/publish")]
    [EndpointSummary("Publish a quiz draft")]
    [EndpointDescription("Turns a quiz draft into a quiz that can be played. The draft is deleted in the same transaction")]
    [ProducesResponseType<QuizIdResponse>(StatusCodes.Status201Created, "Application/json", Description = "Returns the id of the published quiz")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The draft does not meet the rules for a quiz")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz draft was not found")]
    public async Task<ActionResult<QuizIdResponse>> PublishQuizDraft(
        [Required][Description("A unique identifier for a quiz draft")] Guid id)
    {
        try
        {
            var quizId = await publish.HandleAsync(id);

            var response = new QuizIdResponse(quizId);

            return Created($"/v1/games/quizzes/{quizId}", response);
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

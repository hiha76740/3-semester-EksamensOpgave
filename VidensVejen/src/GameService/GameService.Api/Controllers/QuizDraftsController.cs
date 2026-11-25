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
    IUpdateQuizDraftHandler update
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

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update a quiz draft")]
    [EndpointDescription("Saves the quiz draft. Every field may be empty until the draft is published")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Quiz draft was saved successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to save the quiz draft")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz draft was not found")]
    public async Task<ActionResult> UpdateQuizDraft(
        [Required][Description("A unique identifier for a quiz draft")] Guid id,
        [Required][Description("Object used to save a quiz draft")] CreateQuizRequest request)
    {
        try
        {
            await update.HandleAsync(id, request.AsCreateCommand());

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

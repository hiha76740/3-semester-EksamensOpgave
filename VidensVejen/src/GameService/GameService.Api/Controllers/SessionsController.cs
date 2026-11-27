using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameService.Api.Mappers.Sessions;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.GameService.Sessions;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.Api.Controllers;

[Route("/v1/games/sessions")]
[ApiController]
public class SessionsController(
    IStartSessionHandler start,
    IAnswerQuestionHandler answer,
    IFinishSessionHandler finish
    ) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Start a session")]
    [EndpointDescription("Starts a new game session on a published quiz. Returns the quiz without the correct answers")]
    [ProducesResponseType<StartSessionResponse>(StatusCodes.Status201Created, "Application/json", Description = "Returns the session id and the quiz to play")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The request is invalid")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz was not found")]
    public async Task<ActionResult<StartSessionResponse>> StartSession(
        [Required][Description("Object used to start a session")] StartSessionRequest request)
    {
        try
        {
            var dto = await start.HandleAsync(request.QuizId);

            var response = dto.AsResponse();

            return Created($"/v1/games/sessions/{response.SessionId}", response);
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

    [HttpPost("{id:guid}/answers")]
    [EndpointSummary("Answer a question")]
    [EndpointDescription("Answers a question in the session. The answer is validated in GameService and is final")]
    [ProducesResponseType<AnswerResultResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns whether the answer was correct, the feedback and the correct answer")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The question is already answered or the choice does not exist")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Session or question was not found")]
    public async Task<ActionResult<AnswerResultResponse>> AnswerQuestion(
        [Required][Description("A unique identifier for the session")] Guid id,
        [Required][Description("Object used to answer a question")] AnswerQuestionRequest request)
    {
        try
        {
            var dto = await answer.HandleAsync(request.AsCommand(id));

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

    [HttpPost("{id:guid}/finish")]
    [EndpointSummary("Finish a session")]
    [EndpointDescription("Finishes the session when every question is answered and returns the score")]
    [ProducesResponseType<SessionScoreResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the number of correct answers out of all questions and the percentage")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Not every question is answered or the session is already finished")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Session was not found")]
    public async Task<ActionResult<SessionScoreResponse>> FinishSession(
        [Required][Description("A unique identifier for the session")] Guid id)
    {
        try
        {
            var dto = await finish.HandleAsync(id);

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
}

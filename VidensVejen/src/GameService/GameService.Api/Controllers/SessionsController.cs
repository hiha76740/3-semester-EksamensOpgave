using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameService.Api.Mappers.Sessions;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.Api.Controllers;

[Route("/v1/games/sessions")]
[ApiController]
public class SessionsController(
    IStartSessionHandler start
    ) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Start a session")]
    [EndpointDescription("Starts a new game session on a published quiz. Returns the quiz without the correct answers")]
    [ProducesResponseType<StartSessionResponse>(StatusCodes.Status201Created, "Application/json", Description = "Returns the session id and the quiz to play")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The quiz is not published or the request is invalid")]
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
}

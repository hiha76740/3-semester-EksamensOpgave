using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameService.Api.Mappers.Quizzes;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.GameService.Quizzes;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.Api.Controllers;

[Route("/v1/games/quizzes")]
[ApiController]
public class QuizzesController(
    ICreateQuizHandler publish,
    IDeleteQuizHandler delete,
    IQuizQueries queries
    ) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Creates a quiz.")]
    [EndpointDescription("Creates a quiz from the request and deletes the quiz draft in the same transaction.")]
    [ProducesResponseType(StatusCodes.Status201Created, Description = "Quiz created successfully.")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The quiz does not meet the rules for publishing.")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz draft was not found.")]
    public async Task<ActionResult> PublishQuiz(
        [Required][FromQuery][Description("A unique identifier for the quiz draft that is published")] Guid quizDraftId,
        [Required][Description("Object used to publish a quiz")] CreateQuizRequest request)
    {
        try
        {
            await publish.HandleAsync(request.AsCreateCommand(quizDraftId));

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

    [HttpGet("{id:guid}")]
    [EndpointSummary("Get a quiz")]
    [EndpointDescription("Gets a single instance of quiz")]
    [ProducesResponseType<QuizResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns a single instance of the requested quiz")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the requested quiz")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz was not found")]
    public async Task<ActionResult<QuizResponse>> GetQuiz(
        [Required][Description("A unique identifier for a quiz")] Guid id)
    {
        try
        {
            var dto = await queries.GetQuizByIdAsync(id);

            if (dto == null)
                return NotFound("Quiz was not found");

            return Ok(dto.AsResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [EndpointSummary("Get quizzes for an exhibition")]
    [EndpointDescription("Gets all quizzes for an exhibition. Every quiz can be played, drafts are not included")]
    [ProducesResponseType<List<QuizResponse>>(StatusCodes.Status200OK, "Application/json", Description = "Returns the quizzes for the exhibition")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the quizzes")]
    public async Task<ActionResult<List<QuizResponse>>> GetQuizzes(
        [Required][Description("A unique identifier for the exhibition")] Guid exhibitionId)
    {
        try
        {
            var dtos = await queries.GetQuizzesByExhibitionAsync(exhibitionId);

            return Ok(dtos.Select(d => d.AsResponse()).ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("admin")]
    [EndpointSummary("Get quizzes")]
    [EndpointDescription("Gets all instances of published quizzes.")]
    [ProducesResponseType<List<QuizResponse>>(StatusCodes.Status200OK, "Application/json", Description = "Returns all published quizzes")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get all quizzes")]
    public async Task<ActionResult<List<QuizResponse>>> GetAllPublishedQuizzes()
    {
        try
        {
            var list = await queries.GetAllPublishedQuizzes();

            var response = new List<QuizResponse>();

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

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Delete a quiz")]
    [EndpointDescription("Deletes a single instance of a quiz")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Quiz was deleted successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the requested quiz")]
    public async Task<ActionResult> DeleteQuiz(
        [Required][Description("A unique identifier for a quiz")] Guid id,
        [Required][Description("The object used to delete a quiz draft")] DeleteQuizRequest request)
    {
        try
        {
            await delete.HandleAsync(request.AsDeleteCommand(id));

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

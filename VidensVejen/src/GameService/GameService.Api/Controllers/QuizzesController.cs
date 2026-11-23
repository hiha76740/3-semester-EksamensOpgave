using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GameService.Api.Mappers.Quizzes;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.Interfaces;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.Api.Controllers;

[Route("/v1/games/quizzes")]
[ApiController]
public class QuizzesController(
    ICreateQuizHandler create,
    IUpdateQuizHandler update,
    IDeleteQuizHandler delete,
    IPublishQuizHandler publish,
    IQuizQueries queries
    ) : ControllerBase
{
    [HttpPost]
    [EndpointSummary("Create a quiz")]
    [EndpointDescription("Creates a new instance of quiz")]
    [ProducesResponseType<QuizIdResponse>(StatusCodes.Status201Created, "Application/json", Description = "Returns the id of the created quiz")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to create quiz")]
    public async Task<ActionResult<QuizIdResponse>> CreateQuiz(
        [Required][Description("Object used to create a quiz")] CreateQuizRequest request)
    {
        try
        {
            var id = await create.HandleAsync(request.AsCreateCommand());

            var response = new QuizIdResponse(id);

            return CreatedAtAction(nameof(GetQuiz), new { id }, response);
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

    [HttpGet("published")]
    [EndpointSummary("Get published quizzes")]
    [EndpointDescription("Gets the published quizzes for an exhibition. Used by the kiosk, drafts are never returned")]
    [ProducesResponseType<List<QuizResponse>>(StatusCodes.Status200OK, "Application/json", Description = "Returns the published quizzes for the exhibition")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the published quizzes")]
    public async Task<ActionResult<List<QuizResponse>>> GetPublishedQuizzes(
        [Required][Description("A unique identifier for the exhibition")] Guid exhibitionId)
    {
        try
        {
            var dtos = await queries.GetPublishedQuizzesAsync(exhibitionId);

            return Ok(dtos.Select(d => d.AsResponse()).ToList());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:guid}")]
    [EndpointSummary("Update a quiz")]
    [EndpointDescription("Updates the details of the requested quiz")]
    [ProducesResponseType<QuizResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the updated quiz")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to update the requested quiz")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz was not found")]
    public async Task<ActionResult<QuizResponse>> UpdateQuiz(
        [Required][Description("A unique identifier for a quiz")] Guid id,
        [Required][Description("Object needed to update a quiz")] UpdateQuizRequest request)
    {
        try
        {
            var dto = await update.HandleAsync(request.AsUpdateCommand(id));

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

    [HttpDelete("{id:guid}")]
    [EndpointSummary("Deletes a quiz")]
    [EndpointDescription("Deletes a single instance of a quiz")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Quiz was deleted successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to delete the requested quiz")]
    public async Task<ActionResult> DeleteQuiz(
        [Required][Description("A unique identifier for a quiz")] Guid id)
    {
        try
        {
            await delete.HandleAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id:guid}/publish")]
    [EndpointSummary("Publish a quiz")]
    [EndpointDescription("Publishes a draft quiz so it can be shown in the kiosk")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "Quiz was published successfully")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "The quiz does not meet the rules for publishing")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Description = "Quiz was not found")]
    public async Task<ActionResult> PublishQuiz(
        [Required][Description("A unique identifier for a quiz")] Guid id)
    {
        try
        {
            await publish.HandleAsync(id);

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

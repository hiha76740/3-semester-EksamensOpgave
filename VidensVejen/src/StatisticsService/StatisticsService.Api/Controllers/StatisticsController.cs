using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using StatisticsService.Api.Mappers;
using StatisticsService.FacadeLib.Queries.Interfaces;
using VidensVejen.ContractsLib.Responses.StatisticsService;

namespace StatisticsService.Api.Controllers;

[Route("/v1/statistics")]
[ApiController]
public class StatisticsController(
    IGameResultQueries queries
    ) : ControllerBase
{
    [HttpGet("quizzes/{quizId:guid}/average")]
    [EndpointSummary("Get today's average for a quiz")]
    [EndpointDescription("Gets the average share of correct answers in percent for all finished games of the quiz today")]
    [ProducesResponseType<QuizAverageResponse>(StatusCodes.Status200OK, "Application/json", Description = "Returns the average percentage and the number of games today")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Description = "Error while trying to get the average")]
    public async Task<ActionResult<QuizAverageResponse>> GetTodayAverage(
        [Required][Description("A unique identifier for the quiz")] Guid quizId)
    {
        try
        {
            var dto = await queries.GetTodayAverageAsync(quizId);

            return Ok(dto.AsResponse());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

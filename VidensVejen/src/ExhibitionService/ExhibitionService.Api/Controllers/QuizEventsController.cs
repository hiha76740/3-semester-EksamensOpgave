using System.ComponentModel;
using System.Text.Json;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.ContractsLib.Events;

namespace ExhibitionService.Api.Controllers;


[ApiController]
public class QuizEventsController(
    IInboxRepository inbox
    ) : ControllerBase
{
    [Topic("pubsub", "quiz-deletion")]
    [HttpPost("events/quiz-deletion")]
    [EndpointSummary("Delete quizzes")]
    [EndpointDescription("Delete all instances of quizzes for a exhibition ")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Quizzes was deleted succesfully")]
    public async Task<ActionResult> DeleteQuizzesAsync(
        [Description("The event object used to delete all quizzes for a exhibition")] QuizDeletionIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {

        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = "quiz-deletion",
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };

        await inbox.StoreIfMissingAsync(message, cancellationToken);

        return Ok();
    }
}

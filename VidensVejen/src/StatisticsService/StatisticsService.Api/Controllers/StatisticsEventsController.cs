using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.ContractsLib.Events;

namespace StatisticsService.Api.Controllers;

[ApiController]
public class StatisticsEventsController(
    IInboxRepository inbox
    ) : ControllerBase
{
    [Topic("pubsub", "game-completed")]
    [HttpPost("events/game-completed")]
    [EndpointSummary("Store a completed game")]
    [EndpointDescription("Receives a completed game from GameService and stores it in the inbox")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Completed game stored successfully")]
    public async Task<ActionResult> GameCompletedAsync(
        [Required][Description("Object describing the completed game")] GameCompletedIntegrationEvent integrationEvent,
        CancellationToken cancellationToken
        )
    {
        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = "game-completed",
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };

        await inbox.StoreIfMissingAsync(message, cancellationToken);

        return Ok();
    }
}

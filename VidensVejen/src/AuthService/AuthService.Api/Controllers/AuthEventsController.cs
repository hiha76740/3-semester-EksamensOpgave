using System.ComponentModel;
using System.Text.Json;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.ContractsLib.Events;

namespace AuthService.Api.Controllers;


[ApiController]
public class AuthEventsController(
    IInboxRepository inbox
    ) : ControllerBase
{
    [Topic("pubsub", "user-activate")]
    [HttpPost("events/user-activate")]
    [EndpointSummary("Activate a user")]
    [EndpointDescription("Activates the requested user")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "User was activated succesfully")]
    public async Task<ActionResult> ActivateUserAsync(
        [Description("The event object used to activate a user")] UserActivateIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {

        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = "user-activate",
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };

        await inbox.StoreIfMissingAsync(message, cancellationToken);

        return Ok();
    }
}

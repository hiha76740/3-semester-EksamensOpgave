using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.ContractsLib.Events;

namespace UserService.Api.Controllers;


[ApiController]
public class UserEventsController(
    IInboxRepository inbox
    ) : ControllerBase
{
    [Topic("pubsub", "user-registered")]
    [HttpPost("events/user-registered")]
    [EndpointSummary("Create a user")]
    [EndpointDescription("Creates a new instance of a user")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "User created successfully")]
    public async Task<ActionResult> CreateUserAsync(
        [Required][Description("Object used to create a user")] UserRegisteredIntegrationEvent integrationEvent,
        CancellationToken cancellationToken
        )
    {

        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = "user-registered",
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };

        await inbox.StoreIfMissingAsync(message, cancellationToken);

        return Ok();
    }

    [Topic("pubsub", "user-deletion")]
    [HttpPost("events/user-deletion")]
    [EndpointSummary("Deletes a user")]
    [EndpointDescription("Deletes a single instance of a user")]
    [ProducesResponseType(StatusCodes.Status204NoContent, Description = "User was deleted successfully")]
    public async Task<ActionResult> DeleteUserAsync(
        [Required][Description("Object used to delete a user")] UserDeletionIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {

        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = integrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };

        await inbox.StoreIfMissingAsync(message, cancellationToken);

        return Ok();
    }

    [Topic("pubsub", "user-update-email")]
    [HttpPost("events/user-update-email")]
    [EndpointSummary("Updates email of a user")]
    [EndpointDescription("Updates the email of a single instance of a user")]
    [ProducesResponseType(StatusCodes.Status200OK, Description = "Email was updated successfully")]
    public async Task<ActionResult> UpdateUserEmailAsync(
        [Required][Description("Object used to update email for a user")] UserUpdateEmailIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {

        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = integrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };

        await inbox.StoreIfMissingAsync(message, cancellationToken);

        return Ok();
    }
}

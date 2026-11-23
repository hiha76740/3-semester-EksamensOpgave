namespace VidensVejen.ContractsLib.Events;

/// <summary>
/// 
/// </summary>
/// <param name="EventId"></param>
/// <param name="EventTopic"></param>
/// <param name="Email"></param>
/// <param name="UserId"></param>
public record UserUpdateEmailIntegrationEvent(
    Guid EventId,
    string EventTopic,
    Guid UserId,
    string Email
    )
{
    /// <summary>
    /// A unique identifier for the event.
    /// </summary>
    public Guid EventId { get; init; } = EventId;

    /// <summary>
    /// The topic of the event
    /// </summary>
    public string EventTopic { get; init; } = EventTopic;

    /// <summary>
    /// A unique identifier for the user.
    /// </summary>
    public Guid UserId { get; init; } = UserId;

    /// <summary>
    /// The email of the user
    /// </summary>
    public string Email { get; init; } = Email;
}

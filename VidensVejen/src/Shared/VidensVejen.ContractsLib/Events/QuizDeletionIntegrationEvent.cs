namespace VidensVejen.ContractsLib.Events;

/// <summary>
/// 
/// </summary>
/// <param name="EventId"></param>
/// <param name="EventTopic"></param>
/// <param name="ExhibitionId"></param>
public record QuizDeletionIntegrationEvent(
    Guid EventId,
    string EventTopic,
    Guid ExhibitionId
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
    /// A unique identifier for the exhibition.
    /// </summary>
    public Guid ExhibitionId { get; init; } = ExhibitionId;
}

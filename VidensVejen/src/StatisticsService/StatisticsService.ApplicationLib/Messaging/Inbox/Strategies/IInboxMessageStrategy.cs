namespace StatisticsService.ApplicationLib.Messaging.Inbox.Strategies;

/// <summary>
/// 
/// </summary>
public interface IInboxMessageStrategy
{
    /// <summary>
    /// 
    /// </summary>
    string EventType { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="content"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task ApplyAsync(Guid eventId, string content, CancellationToken cancellationToken);
}

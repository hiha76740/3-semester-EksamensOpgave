using Dapr.Client;
using VidensVejen.BuildingBlocksLib.Messaging.Publisher;

namespace ExhibitionService.InfrastructureLib.Messaging.Publisher;

/// <summary>
/// 
/// </summary>
/// <param name="daprClient"></param>
public class DaprOutboxEventPublisher(DaprClient daprClient) : IOutboxEventPublisher
{
    Task IOutboxEventPublisher.PublishAsync<T>(string topic, T integrationEvent)
    {
        return daprClient.PublishEventAsync(
            "pubsub",
            topic,
            integrationEvent);
    }
}

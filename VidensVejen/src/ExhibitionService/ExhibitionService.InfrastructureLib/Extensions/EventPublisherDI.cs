using ExhibitionService.InfrastructureLib.Messaging.Publisher;
using Microsoft.Extensions.DependencyInjection;
using VidensVejen.BuildingBlocksLib.Messaging.Publisher;

namespace ExhibitionService.InfrastructureLib.Extensions;


/// <summary>
/// 
/// </summary>
public static class EventPublisherDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddEventPublisherDI(this IServiceCollection services)
    {
        services.AddScoped<IOutboxEventPublisher, DaprOutboxEventPublisher>();

        return services;
    }
}

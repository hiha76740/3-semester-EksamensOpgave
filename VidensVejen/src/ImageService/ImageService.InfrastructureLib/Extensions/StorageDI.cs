using ImageService.ApplicationLib.Storages;
using ImageService.InfrastructureLib.Storages;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class StorageDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="imageDirectory"></param>
    /// <returns></returns>
    public static IServiceCollection AddStorageDI(this IServiceCollection services, string imageDirectory)
    {
        services.AddScoped<IImageStorage>(serviceProvider => new LocalImageStorage(imageDirectory));

        return services;
    }
}

using ImageService.ApplicationLib.Handlers;
using ImageService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.ApplicationLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class HandlerDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHandlerDI(this IServiceCollection services)
    {
        services.AddScoped<IUploadImageHandler, UploadImageHandler>();
        services.AddScoped<IDeleteImageHandler, DeleteImageHandler>();

        return services;
    }
}

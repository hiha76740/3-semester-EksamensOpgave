using ExhibitionService.ApplicationLib.Handlers;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ExhibitionService.ApplicationLib.Extensions;

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
        services.AddScoped<ICreateExhibitionHandler, CreateExhibitionHandler>();
        services.AddScoped<IUpdateExhibitionHandler, UpdateExhibitionHandler>();
        services.AddScoped<IDeleteExbitionHandler, DeleteExhibitionHandler>();

        return services;
    }
}

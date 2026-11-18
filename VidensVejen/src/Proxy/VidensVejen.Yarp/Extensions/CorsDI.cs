namespace VidensVejen.Yarp.Extensions;

/// <summary>
/// 
/// </summary>
public static class CorsDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="policyName"></param>
    /// <returns></returns>
    public static IServiceCollection AddVidensVejenCors(this IServiceCollection services, string policyName)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.WithOrigins(
                    "https://localhost:7237",
                    "https://localhost:7296",
                    "http://localhost:5001",
                    "http://localhost:5091")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
        });

        return services;
    }
}

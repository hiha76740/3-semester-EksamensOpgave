using Microsoft.Extensions.FileProviders;

namespace ImageService.Api.Extensions;

/// <summary>
/// 
/// </summary>
public static class ImageDirectory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static string SetThemeImageDirectory(this WebApplicationBuilder builder)
    {
        var imageDirectory = Path.Combine(builder.Environment.ContentRootPath, "Storage", "images", "themes");

        return imageDirectory;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="imageDirectory"></param>
    /// <returns></returns>
    public static WebApplication AddStaticImageFiles(this WebApplication app, string imageDirectory)
    {
        Directory.CreateDirectory(imageDirectory);
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(imageDirectory),
            RequestPath = "/images/themes"
        });

        return app;
    }
}

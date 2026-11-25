using Microsoft.AspNetCore.Components.Forms;
using VidensVejen.ContractsLib.Responses.ImageService;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>
/// 
/// </summary>
public interface IImageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="maxFileSize"></param>
    /// <returns></returns>
    Task<ImageIdResponse> UploadImage(IBrowserFile file, long maxFileSize);


    /// <summary>Gets the images available in the media library.</summary>
    /// <returns>The registered images and their display URLs.</returns>
    Task<IReadOnlyList<ImageResponse>> GetAllAsync();

}

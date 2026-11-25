using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using VidensVejen.ContractsLib.Requests.ImageService;
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

    /// <summary>Deletes an image using its current row version.</summary>
    /// <param name="id">The image identifier.</param>
    /// <param name="deletion">The row version received with the image.</param>
    /// <returns>The status code and whether the image is still in use.</returns>
    Task<(HttpStatusCode Status, bool InUse)> DeleteAsync(Guid id, DeleteImageRequest deletion);

}

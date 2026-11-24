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
}

using ImageService.FacadeLib.Queries.DTOs;

namespace ImageService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IImageQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ImageUrlComponentsDto?> GetImageUrlComponentsById(Guid id);
}

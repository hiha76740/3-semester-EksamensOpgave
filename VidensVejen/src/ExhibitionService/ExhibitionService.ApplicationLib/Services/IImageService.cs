namespace ExhibitionService.ApplicationLib.Services;

/// <summary>
/// 
/// </summary>
public interface IImageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    Task<Guid> SaveAsync(Stream content, string extension);
}

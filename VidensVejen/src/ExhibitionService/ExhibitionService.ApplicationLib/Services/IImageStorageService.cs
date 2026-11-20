namespace ExhibitionService.ApplicationLib.Services;

/// <summary>
/// 
/// </summary>
public interface IImageStorageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="storageKey"></param>
    /// <param name="content"></param>
    /// <param name="extension"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task SaveAsync(string storageKey, Stream content, string extension, CancellationToken cancellationToken);
}

namespace ImageService.ApplicationLib.Storages;

/// <summary>
/// 
/// </summary>
public interface IImageStorage
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="storageKey"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    Task DeleteAsync(string storageKey, string extension);

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

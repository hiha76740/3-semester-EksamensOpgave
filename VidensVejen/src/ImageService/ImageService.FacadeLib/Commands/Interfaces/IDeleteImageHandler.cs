namespace ImageService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDeleteImageHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task HandleAsync(Guid id);
}

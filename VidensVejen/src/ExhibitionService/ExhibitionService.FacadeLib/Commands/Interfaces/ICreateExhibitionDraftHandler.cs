namespace ExhibitionService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ICreateExhibitionDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<Guid> HandleAsync();
}

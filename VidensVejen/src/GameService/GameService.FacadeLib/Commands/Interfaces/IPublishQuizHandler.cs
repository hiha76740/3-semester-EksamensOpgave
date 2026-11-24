namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IPublishQuizHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="draftId"></param>
    /// <returns></returns>
    Task<Guid> HandleAsync(Guid draftId);
}

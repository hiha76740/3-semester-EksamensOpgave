namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ICreateQuizDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<Guid> HandleAsync();
}

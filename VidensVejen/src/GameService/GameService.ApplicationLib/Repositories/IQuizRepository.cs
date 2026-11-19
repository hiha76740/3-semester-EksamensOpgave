using GameService.DomainLib.Entities;

namespace GameService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IQuizRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    Task AddAsync(Quiz quiz);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}

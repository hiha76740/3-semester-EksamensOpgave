using UserService.DomainLib.Entities;

namespace UserService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> UserExistsAsync(UserId id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task AddAsync(User user);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<User?> GetUserByIdAsync(UserId userId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    void Delete(User user);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}

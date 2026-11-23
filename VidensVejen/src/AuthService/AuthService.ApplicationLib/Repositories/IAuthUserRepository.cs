using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;

namespace AuthService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IAuthUserRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> EmailExistAsync(Email email);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="authUser"></param>
    /// <returns></returns>
    Task RegisterAsync(AuthUser authUser);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AuthUser?> GetAuthUserByIdAsync(AuthUserId id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<AuthUser?> GetAuthUserByEmailAsync(Email email);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authUser"></param>
    void Delete(AuthUser authUser);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="authUser"></param>
    /// <param name="originalRowVersion"></param>
    /// <returns></returns>
    void SetOriginalRowVersion(AuthUser authUser, byte[] originalRowVersion);
}

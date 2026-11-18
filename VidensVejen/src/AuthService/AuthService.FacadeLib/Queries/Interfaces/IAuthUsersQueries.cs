using AuthService.FacadeLib.Queries.DTOs;

namespace AuthService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IAuthUsersQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AuthUserDto?> GetByIdAsync(Guid id);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AuthUserDto?> GetCurrentUserInfo(Guid id);
}

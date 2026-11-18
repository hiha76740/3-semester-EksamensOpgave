using VidensVejen.ContractsLib.Responses.AuthService;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>
/// 
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<AuthUserResponse> GetAuthUserAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<int> Login(string username, string password);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task LogOut();
}

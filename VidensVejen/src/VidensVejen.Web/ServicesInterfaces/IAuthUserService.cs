using VidensVejen.ContractsLib.Responses.AuthService;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>
/// 
/// </summary>
public interface IAuthUserService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<AuthUserResponse> GetAuthUserAsync();
}

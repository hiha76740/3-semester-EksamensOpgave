using System.Net;
using VidensVejen.ContractsLib.Requests.AuthService;
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

    /// <summary>Gets the permissions available for user registration.</summary>
    /// <returns>The permission options returned by AuthService.</returns>
    Task<IReadOnlyList<PermissionResponse>> GetPermissionsAsync();

    /// <summary>Registers a new user in AuthService.</summary>
    /// <param name="registration">The account details and assigned permissions.</param>
    /// <returns>The registration response status.</returns>
    Task<HttpStatusCode> RegisterAsync(RegisterUserRequest registration);

    /// <summary>Updates the email address of the specified account.</summary>
    Task<UserEmailResponse> UpdateEmailAsync(Guid id, UpdateAuthUserEmailRequest update);

    /// <summary>Changes the password after the current password has been verified.</summary>
    Task UpdatePasswordAsync(Guid id, UpdateAuthUserPasswordRequest update);
}

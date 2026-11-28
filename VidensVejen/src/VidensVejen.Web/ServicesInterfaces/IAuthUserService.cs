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

    /// <summary>Gets the selected account and its current row version.</summary>
    Task<AuthUserResponse> GetByIdAsync(Guid id);

    /// <summary>Replaces the assigned permissions using the account's current row version.</summary>
    Task<AuthUserResponse> UpdatePermissionsAsync(Guid id, UpdateAuthUserPermissionsRequest request);

    /// <summary>Resets the selected account password as an administrator.</summary>
    Task ResetPasswordAsync(Guid id, ResetUserPasswordRequest request);

    /// <summary>Deletes the selected account using its current row version.</summary>
    Task DeleteAsync(Guid id, DeleteUserRequest request);
}

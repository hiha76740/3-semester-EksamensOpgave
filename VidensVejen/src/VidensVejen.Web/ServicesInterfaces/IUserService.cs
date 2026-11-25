using VidensVejen.ContractsLib.Requests.UserService;
using VidensVejen.ContractsLib.Responses.UserService;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>Provides access to user profile data.</summary>
public interface IUserService
{
    /// <summary>Gets the profile for the specified user.</summary>
    Task<UserResponse> GetByIdAsync(Guid id);

    /// <summary>Updates the user's name using the profile row version.</summary>
    Task<UserResponse> UpdateAsync(Guid id, UpdateUserRequest update);
}

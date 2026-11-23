namespace VidensVejen.ContractsLib.Requests.AuthService;


/// <summary>
/// 
/// </summary>
/// <param name="Password"></param>
/// <param name="OldPassword"></param>
/// <example>
/// {
///     "password": "MySecretPassword123",
///     "oldPassword": "MyOldSecretPassword123"
/// }
/// </example>
public record UpdateAuthUserPasswordRequest(string Password, string OldPassword)
{
    /// <summary>
    /// The new password of the user.
    /// </summary>
    public string Password { get; init; } = Password;

    /// <summary>
    /// The old password of the user.
    /// </summary>
    public string OldPassword { get; init; } = OldPassword;
};

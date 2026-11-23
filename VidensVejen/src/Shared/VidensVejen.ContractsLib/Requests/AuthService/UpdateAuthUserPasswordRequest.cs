namespace VidensVejen.ContractsLib.Requests.AuthService;


/// <summary>
/// 
/// </summary>
/// <param name="Password"></param>
/// <param name="OldPassword"></param>
/// <param name="RowVersion"></param>
/// <example>
/// {
///     "password": "MySecretPassword123",
///     "oldPassword": "MyOldSecretPassword123",
///     "rowVersion": "AAAAAAAAB9E="
/// }
/// </example>
public record UpdateAuthUserPasswordRequest(string Password, string OldPassword, byte[] RowVersion)
{
    /// <summary>
    /// The new password of the user.
    /// </summary>
    public string Password { get; init; } = Password;

    /// <summary>
    /// The old password of the user.
    /// </summary>
    public string OldPassword { get; init; } = OldPassword;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
};

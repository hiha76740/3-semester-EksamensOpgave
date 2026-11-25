namespace VidensVejen.ContractsLib.Requests.AuthService;

/// <summary>Requests an administrative password reset.</summary>
/// <param name="Password">The new password.</param>
/// <param name="RowVersion">The AuthService account version.</param>
/// <example>
/// {
///     "password": "MyNewSecretPassword123",
///     "rowVersion": "AAAAAAAAB9E="
/// }
/// </example>
public record ResetUserPasswordRequest(string Password, byte[] RowVersion)
{
    /// <summary>
    /// The password of the user
    /// </summary>
    public string Password { get; set; } = Password;


    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}

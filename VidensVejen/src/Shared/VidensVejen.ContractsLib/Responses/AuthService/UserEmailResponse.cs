namespace VidensVejen.ContractsLib.Responses.AuthService;

/// <summary>
/// 
/// </summary>
/// <param name="Email"></param>
/// /// <param name="RowVersion"></param>
/// <example>
/// {
///     "email": "SueStorm@FantasticFour.test",
///     "rowVersion": "AAAAAAAAB9E="
/// }
/// </example>
public record UserEmailResponse(string Email, byte[] RowVersion)
{
    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; init; } = Email;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}

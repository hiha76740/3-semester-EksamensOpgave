namespace VidensVejen.ContractsLib.Responses.AuthService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Email"></param>
/// <param name="Permissions"></param>
/// <param name="RowVersion"></param>
/// <example>
/// {
///     "id": "7693612e-45b7-430c-a182-318f1fc86442",
///     "email": "SueStorm@FantasticFour.test",
///     "permissions": [
///         {
///             "id": "4837ca0d-9acb-47e0-bb7c-3f1a17bce7a5",
///             "name": "game.read"
///         },
///         {
///             "id": "e35a057d-74b0-4c4c-a064-69a38e3fbf41",
///             "name": "game.edit"
///         }
///     ],
///     "rowVersion": "AAAAAAAAB9E="
/// }
/// </example>
public record AuthUserResponse(
    Guid Id,
    string Email,
    IReadOnlyList<PermissionResponse> Permissions,
    byte[] RowVersion
    )
{
    /// <summary>
    /// A unique identifier for the auth user.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; init; } = Email;

    /// <summary>
    /// A list of the permissions the user has.
    /// </summary>
    public IReadOnlyList<PermissionResponse> Permissions { get; init; } = Permissions;


    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;

}

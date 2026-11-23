namespace VidensVejen.ContractsLib.Responses.UserService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Firstname"></param>
/// <param name="Surname"></param>
/// <param name="Email"></param>
/// <param name="RowVersion"></param>
/// <example>
/// {
///     "id": "123e4567-e89b-12d3-a456-426614174000",
///     "firstname": "Sue",
///     "surname": "Storm",
///     "email": "SueStorm@FantasticFour.test",
///     "rowVersion": "AAAAAAAAB9E="
/// }
/// </example>
public record UserResponse(
    Guid Id,
    string Firstname,
    string Surname,
    string Email,
    byte[] RowVersion
    )
{
    /// <summary>
    /// A unique identifier for the user.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The firstname of the user.
    /// </summary>
    public string Firstname { get; init; } = Firstname;

    /// <summary>
    /// The surname of the user.
    /// </summary>
    public string Surname { get; init; } = Surname;

    /// <summary>
    /// The email of the user.
    /// </summary>
    public string Email { get; init; } = Email;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}

namespace VidensVejen.ContractsLib.Requests.UserService;

/// <summary>
/// 
/// </summary>
/// <param name="Firstname"></param>
/// <param name="Surname"></param>
/// <param name="RowVersion"></param>
/// <example>
/// {
///     "firstname": "Sue",
///     "surname": "Storm",
///     "rowVersion": "AAAAAAAAB9E="
/// }
/// </example>
public record UpdateUserRequest(
    string Firstname,
    string Surname,
    byte[] RowVersion
    )
{
    /// <summary>
    /// The firstname of the user.
    /// </summary>
    public string Firstname { get; init; } = Firstname;

    /// <summary>
    /// The surname of the user.
    /// </summary>
    public string Surname { get; init; } = Surname;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}

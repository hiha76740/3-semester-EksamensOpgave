namespace VidensVejen.ContractsLib;

/// <summary>
/// Identifies the built-in administrator across AuthService and UserService.
/// </summary>
public static class BootstrapAdmin
{
    /// <summary>
    /// Gets the shared database identifier of the built-in administrator.
    /// </summary>
    public static Guid Id { get; } = new("34c244cd-dbbc-492a-b6d3-1120562fbc31");
}

namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <example>
/// {
///     "name": "Vand og hav"
/// }
/// </example>
public record ExhibitionNameResponse(Guid Id, string Name)
{
    /// <summary>
    /// A unique identifier of the Exhibition.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The name of the exhibition.
    /// </summary>
    public string Name { get; init; } = Name;
}

namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Subjects"></param>
/// <param name="ThemeUrl"></param>
/// <example>
/// {
///     "id": "123e4567-e89b-12d3-a456-426614174000",
///     "exhibitionName": "Vand og hav",
///     "themeUrl":"https://example.com/image"
///     "subjects": [
///         {
///             "id": "123e4567-e89b-12d3-a456-426614174000",
///             "title": "Affaldshierakiet",
///             "description": "Tryk på ét trin. De bedste løsniger er øverst",
///             "topics": [
///                 {
///                     "id": "123e4567-e89b-12d3-a456-426614174000",
///                     "title":"Forebyggelse",
///                     "description":"Undgå at skabe affald i første omgang",
///                     "body":"Eksempel: Tag en drikkedunk med i stedet for at købe en ny flaske",
///                     "calloutLabel":"Otto siger:",
///                     "calloutText":"Det bedste affald er det, vi slet ikke skaber",
///                     "qoute":"Otto anbefaler: måske kan en andens gamle ting blive din nye favorit"
///                 }
///             ]
///         }
///     ]
/// }
/// </example>
public record ExhibitionResponse(
    Guid Id,
    string Title,
    string ThemeUrl,
    IReadOnlyList<SubjectResponse> Subjects
    )
{
    /// <summary>
    /// A unique identifier for the exhibition.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the exhibition.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The url of the theme.
    /// </summary>
    public string ThemeUrl { get; init; } = ThemeUrl;

    /// <summary>
    /// A list of subjects the exhibition has associated.
    /// </summary>
    public IReadOnlyList<SubjectResponse> Subjects { get; init; } = Subjects;
}

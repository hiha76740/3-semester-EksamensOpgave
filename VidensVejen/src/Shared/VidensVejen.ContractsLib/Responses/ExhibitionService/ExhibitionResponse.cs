namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Subjects"></param>
/// <param name="RowVersion"></param>
/// <param name="ThemeImageId"></param>
/// <example>
/// {
///     "id": "123e4567-e89b-12d3-a456-426614174000",
///     "exhibitionName": "Vand og hav",
///     "themeImageId": "123e4567-e89b-12d3-a456-426614174000",
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
    string TitleEn,
    Guid ThemeImageId,
    IReadOnlyList<SubjectResponse> Subjects,
    byte[] RowVersion
    )
{
    /// <summary>
    /// A unique identifier for the exhibition.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the exhibition shown in Danish.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The title of the exhibition shown in English.
    /// </summary>
    public string TitleEn { get; init; } = TitleEn;

    /// <summary>
    /// The url of the theme.
    /// </summary>
    public Guid ThemeImageId { get; init; } = ThemeImageId;

    /// <summary>
    /// A list of subjects the exhibition has associated.
    /// </summary>
    public IReadOnlyList<SubjectResponse> Subjects { get; init; } = Subjects;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}

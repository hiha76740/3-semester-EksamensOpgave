namespace VidensVejen.ContractsLib.Requests.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibitionName"></param>
/// <param name="ThemeImageId"></param>
/// <param name="Subjects"></param>
/// <example>
/// {
///     "exhibitionName": "Vand og hav",
///     "themeImageId": "123e4567-e89b-12d3-a456-426614174000",
///     "subjects": [
///         {
///             "title": "Affaldshierakiet",
///             "description": "Tryk på ét trin. De bedste løsniger er øverst",
///             "topics": [
///                 {
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
public record CreateExhibitionRequest(
    string ExhibitionName,
    Guid ThemeImageId,
    List<CreateSubjectRequest> Subjects
    )
{
    /// <summary>
    /// The name of the exhibition.
    /// </summary>
    public string ExhibitionName { get; init; } = ExhibitionName;

    /// <summary>
    /// The unique identifier for the theme image.
    /// </summary>
    public Guid ThemeImageId { get; init; } = ThemeImageId;

    /// <summary>
    /// List of the subjects objects you want exhibition to have
    /// </summary>
    public List<CreateSubjectRequest> Subjects { get; init; } = Subjects;
}

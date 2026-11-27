namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>
/// 
/// </summary>
/// <param name="Name"></param>
/// <param name="NameEn"></param>
/// <param name="ThemeImageId"></param>
/// <param name="SubjectDrafts"></param>
/// <param name="RowVersion"></param>
/// <example>
/// {
///     "name": "Vand og hav",
///     "nameEn": "Water and sea"
///     "themeImageId": "123e4567-e89b-12d3-a456-426614174000",
///     "subjects": [
///         {
///             "id": "123e4567-e89b-12d3-a456-426614174000",
///             "title": "Affaldshierakiet",
///             "titleEn": "The Waste Hierarchy",
///             "slug": "affaldshierakiet",
///             "description": "Tryk på ét trin. De bedste løsniger er øverst",
///             "descriptionEn": "Click on a step. The best solutions are at the top.",
///             "topics": [
///                 {
///                     "title": "Forebyggelse",
///                     "titleEn": "Prevention",  
///                     "description": "Undgå at skabe affald i første omgang",
///                     "descriptionEn": "Avoid creating waste in the first place",
///                     "body": "Eksempel: Tag en drikkedunk med i stedet for at købe en ny flaske",
///                     "bodyEn": "Example: Bring a reusable water bottle instead of buying a new bottle.",
///                     "calloutLabel": "Otto siger:",
///                     "calloutLabelEn": "Otto says:",
///                     "calloutText": "Det bedste affald er det, vi slet ikke skaber",
///                     "calloutTextEn": "The best waste is the waste we don't create at all.",
///                     "quote": "Otto anbefaler: måske kan en andens gamle ting blive din nye favorit"
///                     "quoteEn": "Otto recommends: perhaps someone else’s old things can become your new favorite"
///                 }
///             ]
///         }
///     ]
/// }
/// </example>
public record UpdateExhibitionDraftRequest(
    string? Name,
    string? NameEn,
    Guid? ThemeImageId,
    List<UpdateSubjectDraftRequest> SubjectDrafts,
    byte[] RowVersion
    )
{
    /// <summary>
    /// The name of the exhibition draft.
    /// </summary>
    public string? Name { get; init; } = Name;

    /// <summary>
    /// The english name of the exhibition draft.
    /// </summary>
    public string? NameEn { get; init; } = NameEn;

    /// <summary>
    /// The unique identifier for the theme image.
    /// </summary>
    public Guid? ThemeImageId { get; init; } = ThemeImageId;

    /// <summary>
    /// List of the subjects objects you want the exhibition draft to have
    /// </summary>
    public List<UpdateSubjectDraftRequest> SubjectDrafts { get; init; } = SubjectDrafts;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}

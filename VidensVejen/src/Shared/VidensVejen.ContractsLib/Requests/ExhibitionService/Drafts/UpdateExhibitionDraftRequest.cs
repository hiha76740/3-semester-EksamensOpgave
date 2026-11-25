namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibitionName"></param>
/// <param name="ExhibitionNameEn"></param>
/// <param name="ThemeImageId"></param>
/// <param name="SubjectDrafts"></param>
/// <example>
/// {
///     "exhibitionName": "Vand og hav",
///     "exhibitionNameEn": "Water and sea"
///     "themeImageId": "123e4567-e89b-12d3-a456-426614174000",
///     "subjects": [
///         {
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
    string? ExhibitionName,
    string? ExhibitionNameEn,
    Guid? ThemeImageId,
    List<UpdateSubjectDraftRequest> SubjectDrafts
    )
{
    /// <summary>
    /// The name of the exhibition draft.
    /// </summary>
    public string? ExhibitionName { get; init; } = ExhibitionName;

    /// <summary>
    /// The english name of the exhibition draft.
    /// </summary>
    public string? ExhibitionNameEn { get; init; } = ExhibitionNameEn;

    /// <summary>
    /// The unique identifier for the theme image.
    /// </summary>
    public Guid? ThemeImageId { get; init; } = ThemeImageId;

    /// <summary>
    /// List of the subjects objects you want the exhibition draft to have
    /// </summary>
    public List<UpdateSubjectDraftRequest> SubjectDrafts { get; init; } = SubjectDrafts;
}

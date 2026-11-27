namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="NameEn"></param>
/// <param name="ThemeImageId"></param>
/// <param name="SubjectDrafts"></param>
/// <param name="SourceExhibitionId"></param>
///  <example>
/// {
///     "id": "123e4567-e89b-12d3-a456-426614174000",
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
public record ExhibitionDraftResponse(
    Guid Id,
    string? Name,
    string? NameEn,
    Guid? ThemeImageId,
    IReadOnlyList<SubjectDraftResponse> SubjectDrafts,
    Guid? SourceExhibitionId
    )
{
    /// <summary>
    /// A unique identifier for the exhibition draft.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The name of the exhibition.
    /// </summary>
    public string? Name { get; init; } = Name;

    /// <summary>
    /// The english name of the exhibition.
    /// </summary>
    public string? NameEn { get; init; } = NameEn;

    /// <summary>
    /// The url of the theme.
    /// </summary>
    public Guid? ThemeImageId { get; init; } = ThemeImageId;

    /// <summary>
    /// A list of subject drafts the exhibition has associated.
    /// </summary>
    public IReadOnlyList<SubjectDraftResponse> SubjectDrafts { get; init; } = SubjectDrafts;

    /// <summary>
    /// A unique identifier for the source exhibition.
    /// </summary>
    public Guid? SourceExhibitionId { get; init; } = SourceExhibitionId;
}


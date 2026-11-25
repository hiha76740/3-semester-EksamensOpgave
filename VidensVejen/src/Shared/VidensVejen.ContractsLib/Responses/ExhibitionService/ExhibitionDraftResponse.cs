namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="SubjectDrafts"></param>
/// <param name="ThemeImageId"></param>
public record ExhibitionDraftResponse(
    Guid Id,
    string? Title,
    Guid? ThemeImageId,
    IReadOnlyList<SubjectDraftResponse> SubjectDrafts
    )
{
    /// <summary>
    /// A unique identifier for the exhibition draft.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the exhibition draft.
    /// </summary>
    public string? Title { get; init; } = Title;

    /// <summary>
    /// The url of the theme.
    /// </summary>
    public Guid? ThemeImageId { get; init; } = ThemeImageId;

    /// <summary>
    /// A list of subject drafts the exhibition has associated.
    /// </summary>
    public IReadOnlyList<SubjectDraftResponse>? SubjectDrafts { get; init; } = SubjectDrafts;
}


namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="TopicDrafts"></param>
public record UpdateSubjectDraftRequest(
    string? Title,
    string? TitleEn,
    string? Slug,
    string? Description,
    string? DescriptionEn,
    List<UpdateTopicDraftRequest> TopicDrafts
    )
{
    /// <summary>
    /// The title of the subject draft.
    /// </summary>
    public string? Title { get; init; } = Title;

    /// <summary>
    /// The english title of the subject draft.
    /// </summary>
    public string? TitleEn { get; init; } = TitleEn;

    /// <summary>
    /// The slug of the subject draft.
    /// </summary>
    public string? Slug { get; init; } = Slug;

    /// <summary>
    /// The description of the subject draft.
    /// </summary>
    public string? Description { get; init; } = Description;

    /// <summary>
    /// The description of the subject draft.
    /// </summary>
    public string? DescriptionEn { get; init; } = DescriptionEn;

    /// <summary>
    /// The list of topics you want under the subject draft.
    /// </summary>
    public List<UpdateTopicDraftRequest> TopicDrafts { get; init; } = TopicDrafts;
}

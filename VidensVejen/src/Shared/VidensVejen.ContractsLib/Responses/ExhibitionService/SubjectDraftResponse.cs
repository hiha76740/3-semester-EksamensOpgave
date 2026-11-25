namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Slug"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="TopicDrafts"></param>
public record SubjectDraftResponse(
    Guid? Id,
    string? Slug,
    string? Title,
    string? Description,
    IReadOnlyList<TopicDraftResponse>? TopicDrafts
    )
{
    /// <summary>
    /// A unique identifier for the subject draft.
    /// </summary>
    public Guid? Id { get; init; } = Id;

    /// <summary>
    /// The slug of the subject draft.
    /// </summary>
    public string? Slug { get; init; } = Slug;

    /// <summary>
    /// The title of the subject draft.
    /// </summary>
    public string? Title { get; init; } = Title;

    /// <summary>
    /// The description of the subject draft.
    /// </summary>
    public string? Description { get; init; } = Description;

    /// <summary>
    /// A list of topics the subject draft has associated.
    /// </summary>
    public IReadOnlyList<TopicDraftResponse>? TopicDrafts { get; init; } = TopicDrafts;
}

namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="QuizId"></param>
/// <param name="QuizDraftId"></param>
/// <param name="IsQuiz"></param>
/// <param name="TopicDrafts"></param>
public record UpdateSubjectDraftRequest(
    Guid Id,
    string? Title,
    string? TitleEn,
    string? Slug,
    string? Description,
    string? DescriptionEn,
    Guid? QuizId,
    Guid? QuizDraftId,
    bool IsQuiz,
    List<UpdateTopicDraftRequest> TopicDrafts
    )

{
    /// <summary>Gets the identifier of the published quiz.</summary>
    public Guid? QuizId { get; init; } = QuizId;

    /// <summary>Gets the identifier of the quiz draft.</summary>
    public Guid? QuizDraftId { get; init; } = QuizDraftId;

    /// <summary>Gets whether this subject is intended to contain a quiz.</summary>
    public bool IsQuiz { get; init; } = IsQuiz;

    /// <summary>
    /// A unique identifier for the subject draft.
    /// </summary>
    public Guid Id { get; init; } = Id;

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

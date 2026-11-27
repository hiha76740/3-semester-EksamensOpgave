namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Slug"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="QuizId"></param>
/// <param name="Topics"></param>
public record SubjectResponse(
    Guid Id,
    string Slug,
    string Title,
    string TitleEn,
    string Description,
    string DescriptionEn,
    Guid? QuizId,
    IReadOnlyList<TopicResponse> Topics
    )
{

    /// <summary>
    /// A unique identifier for the subject.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The slug of the subject.
    /// </summary>
    public string Slug { get; init; } = Slug;

    /// <summary>
    /// The title of the subject.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The description of the subject.
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// Gets the identifier of the published quiz.
    /// </summary>
    public Guid? QuizId { get; init; } = QuizId;

    /// <summary>
    /// A list of topics the subject has associated.
    /// </summary>
    public IReadOnlyList<TopicResponse> Topics { get; init; } = Topics;
}

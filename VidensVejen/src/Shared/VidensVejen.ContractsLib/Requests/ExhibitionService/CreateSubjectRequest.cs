namespace VidensVejen.ContractsLib.Requests.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="Topics"></param>
public record CreateSubjectRequest(
    string Title,
    string Slug,
    string Description,
    List<CreateTopicRequest> Topics
    )
{
    /// <summary>
    /// The title of the subject.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The slug of the subject.
    /// </summary>
    public string Slug { get; init; } = Slug;

    /// <summary>
    /// The description of the subject.
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// The list of topics you want under the subject.
    /// </summary>
    public List<CreateTopicRequest> Topics { get; init; } = Topics;
}

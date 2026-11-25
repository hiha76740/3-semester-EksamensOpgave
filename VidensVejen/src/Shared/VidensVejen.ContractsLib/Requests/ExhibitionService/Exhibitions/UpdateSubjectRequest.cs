namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Topics"></param>
public record UpdateSubjectRequest(
    Guid Id,
    string Title,
    string Description,
    List<UpdateTopicRequest> Topics
    )
{
    /// <summary>
    /// A unique identifier for a subject.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the subject.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The description of the subject.
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// A list of the topics that you want to update.
    /// </summary>
    public List<UpdateTopicRequest> Topics { get; init; } = Topics;
}

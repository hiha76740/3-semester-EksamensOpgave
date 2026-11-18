using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities;

/// <summary>
///
/// </summary>
public class Quiz
{
    /// <summary>
    ///
    /// </summary>
    public QuizId Id { get; init; } = null!;

    /// <summary>
    ///
    /// </summary>
    public Guid ExhibitionId { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public string QuizLayout { get; private set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public string? EndMessage { get; private set; }

    /// <summary>
    ///
    /// </summary>
    public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

    private readonly List<Question> _questions = [];

    /// <summary>
    ///
    /// </summary>
    /// <param name="title"></param>
    /// <param name="exhibitionId"></param>
    /// <param name="quizLayout"></param>
    /// <param name="endMessage"></param>
    /// <param name="questions"></param>
    /// <returns>The created quiz.</returns>
    /// <exception cref="DomainException"></exception>
    public static Quiz Create(string title, Guid exhibitionId, string quizLayout, string? endMessage, List<Question> questions)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title could not be set, no value");

        if (exhibitionId == Guid.Empty)
            throw new DomainException("ExhibitionId could not be set, no value");

        if (string.IsNullOrWhiteSpace(quizLayout))
            throw new DomainException("Quiz layout could not be set, no value");

        if (questions == null || questions.Count == 0)
            throw new DomainException("A quiz must have at least one question");

        return new Quiz(title, exhibitionId, quizLayout, endMessage, questions);
    }

    private Quiz(string title, Guid exhibitionId, string quizLayout, string? endMessage, List<Question> questions)
    {
        Id = new QuizId(Guid.NewGuid());
        Title = title;
        ExhibitionId = exhibitionId;
        QuizLayout = quizLayout;
        EndMessage = endMessage;
        _questions = questions;
    }

    // EF Core constructor
    private Quiz() { }
}

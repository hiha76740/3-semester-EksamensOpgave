using GameService.DomainLib.Enums;
using GameService.DomainLib.ValueObjects;
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
    public ExhibitionId? ExhibitionId { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? QuizLayout { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? EndMessage { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public PublicationStatus Status { get; private set; }

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
    /// <returns></returns>
    public static Quiz Create(string? title, Guid? exhibitionId, string? quizLayout, string? endMessage, List<NewQuestion>? questions)
    {
        ExhibitionId? exhibitionKey = null;

        if (exhibitionId != null)
            exhibitionKey = new ExhibitionId(exhibitionId.Value);

        var quiz = new Quiz(title, exhibitionKey, quizLayout, endMessage, questions ?? []);

        return quiz;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void Publish()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new DomainException("Title could not be set, no value");

        if (ExhibitionId == null || ExhibitionId.Value == Guid.Empty)
            throw new DomainException("ExhibitionId could not be set, no value");

        if (string.IsNullOrWhiteSpace(QuizLayout))
            throw new DomainException("Quiz layout could not be set, no value");

        if (_questions.Count == 0)
            throw new DomainException("A quiz must have at least one question");

        foreach (var question in _questions)
        {
            question.EnsureCanBePublished();
        }

        Status = PublicationStatus.Published;
    }

    private void AddQuestion(NewQuestion newQuestion)
    {
        var question = new Question(newQuestion.Text, newQuestion.Choices, newQuestion.CorrectFeedback, newQuestion.IncorrectFeedback);
        _questions.Add(question);
    }

    private Quiz(string? title, ExhibitionId? exhibitionId, string? quizLayout, string? endMessage, List<NewQuestion> questions)
    {
        Id = new QuizId(Guid.NewGuid());
        Title = title;
        ExhibitionId = exhibitionId;
        QuizLayout = quizLayout;
        EndMessage = endMessage;
        Status = PublicationStatus.Draft;

        foreach (var question in questions)
        {
            AddQuestion(question);
        }
    }

    // EF Core constructor
    private Quiz() { }
}

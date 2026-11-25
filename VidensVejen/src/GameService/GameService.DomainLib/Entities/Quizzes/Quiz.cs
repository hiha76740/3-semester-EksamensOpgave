using GameService.DomainLib.ValueObjects;
using GameService.DomainLib.ValueObjects.Quizzes;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities.Quizzes;

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
    public ExhibitionId ExhibitionId { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? TitleEn { get; private set; }
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
    public string? EndMessageEn { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Question> Questions => _questions.AsReadOnly();

    private readonly List<Question> _questions = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="titleEn"></param>
    /// <param name="exhibitionId"></param>
    /// <param name="quizLayout"></param>
    /// <param name="endMessage"></param>
    /// <param name="endMessageEn"></param>
    /// <param name="questions"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Quiz Create(string? title, string? titleEn, Guid? exhibitionId, string? quizLayout, string? endMessage, string? endMessageEn, List<NewQuestion>? questions)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title could not be set, no value");

        if (exhibitionId == null || exhibitionId == Guid.Empty)
            throw new DomainException("ExhibitionId could not be set, no value");

        if (string.IsNullOrWhiteSpace(quizLayout))
            throw new DomainException("Quiz layout could not be set, no value");

        if (questions == null || questions.Count == 0)
            throw new DomainException("A quiz must have at least one question");

        var exhibitionKey = new ExhibitionId(exhibitionId.Value);

        var quiz = new Quiz(title, titleEn, exhibitionKey, quizLayout, endMessage, endMessageEn, questions);

        return quiz;
    }

    private void AddQuestion(NewQuestion newQuestion)
    {
        var question = new Question(newQuestion.Text, newQuestion.TextEn, newQuestion.Choices);
        _questions.Add(question);
    }

    private Quiz(string title, string? titleEn, ExhibitionId exhibitionId, string quizLayout, string? endMessage, string? endMessageEn, List<NewQuestion> questions)
    {
        Id = new QuizId(Guid.NewGuid());
        Title = title;
        TitleEn = titleEn;
        ExhibitionId = exhibitionId;
        QuizLayout = quizLayout;
        EndMessage = endMessage;
        EndMessageEn = endMessageEn;

        foreach (var question in questions)
        {
            AddQuestion(question);
        }
    }

    // EF Core constructor
    private Quiz() { }
}

using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class QuizDraft
{
    /// <summary>
    /// 
    /// </summary>
    public QuizDraftId Id { get; init; } = null!;
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
    public string? TitleEn { get; private set; }
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
    public string? EndMessageEn { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<DraftQuestion> Questions => _questions.AsReadOnly();

    private readonly List<DraftQuestion> _questions = [];

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
    public static QuizDraft Create(string? title, string? titleEn, Guid? exhibitionId, string? quizLayout, string? endMessage, string? endMessageEn, List<NewQuestion>? questions)
    {
        ExhibitionId? exhibitionKey = null;

        if (exhibitionId != null && exhibitionId != Guid.Empty)
            exhibitionKey = new ExhibitionId(exhibitionId.Value);

        var draft = new QuizDraft(title, titleEn, exhibitionKey, quizLayout, endMessage, endMessageEn, questions ?? []);

        return draft;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public Quiz Publish()
    {
        var questions = new List<NewQuestion>();

        foreach (var question in _questions)
        {
            questions.Add(question.AsNewQuestion());
        }

        return Quiz.Create(Title, TitleEn, ExhibitionId?.Value, QuizLayout, EndMessage, EndMessageEn, questions);
    }

    private void AddQuestion(NewQuestion newQuestion)
    {
        var question = new DraftQuestion(newQuestion.Text, newQuestion.TextEn, newQuestion.Choices);
        _questions.Add(question);
    }

    private QuizDraft(string? title, string? titleEn, ExhibitionId? exhibitionId, string? quizLayout, string? endMessage, string? endMessageEn, List<NewQuestion> questions)
    {
        Id = new QuizDraftId(Guid.NewGuid());
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
    private QuizDraft() { }
}

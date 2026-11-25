using GameService.DomainLib.ValueObjects;
using GameService.DomainLib.ValueObjects.Drafts;

namespace GameService.DomainLib.Entities.Drafts;

/// <summary>
/// 
/// </summary>
public class QuizDraft
{
    /// <summary>
    /// 
    /// </summary>
    public QuizDraftId Id { get; init; } = new QuizDraftId(Guid.NewGuid());
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
    public string? Instruction { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? InstructionEn { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<QuestionDraft> Questions => _questions.AsReadOnly();

    private readonly List<QuestionDraft> _questions = [];

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static QuizDraft Create()
    {
        var draft = new QuizDraft();
        return draft;
    }

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
    /// <param name="instruction"></param>
    /// <param name="instructionEn"></param>
    public void Update(
        string? title,
        string? titleEn,
        Guid? exhibitionId,
        string? quizLayout,
        string? endMessage,
        string? endMessageEn,
        List<NewQuestionDraft>? questions,
        string? instruction,
        string? instructionEn
        )
    {
        ExhibitionId = null;

        if (exhibitionId != null && exhibitionId != Guid.Empty)
            ExhibitionId = new ExhibitionId(exhibitionId.Value);

        Title = title;
        TitleEn = titleEn;
        QuizLayout = quizLayout;
        EndMessage = endMessage;
        EndMessageEn = endMessageEn;
        Instruction = instruction;
        InstructionEn = instructionEn;

        _questions.Clear();

        foreach (var question in questions ?? [])
        {
            AddQuestion(question);
        }
    }

    private void AddQuestion(NewQuestionDraft newQuestion)
    {
        var question = new QuestionDraft(newQuestion.Text, newQuestion.TextEn, newQuestion.Choices);
        _questions.Add(question);
    }

    // EF Core constructor
    private QuizDraft() { }
}

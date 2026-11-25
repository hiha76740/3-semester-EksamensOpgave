using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class GameSession
{
    /// <summary>
    /// 
    /// </summary>
    public GameSessionId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public QuizId QuizId { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public DateTime? CompletedAt { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<SessionAnswer> Answers => _answers.AsReadOnly();

    private readonly List<SessionAnswer> _answers = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    public static GameSession Start(Quiz quiz)
    {
        var session = new GameSession(quiz.Id);

        return session;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <param name="questionId"></param>
    /// <param name="choiceIndex"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public AnswerResult Answer(Quiz quiz, Guid questionId, int choiceIndex)
    {
        var question = quiz.Questions.FirstOrDefault(q => q.Id.Value == questionId)
            ?? throw new NotFoundException("Question was not found in the quiz");

        if (_answers.Any(a => a.QuestionId == questionId))
            throw new DomainException("The question has already been answered");

        if (choiceIndex < 0 || choiceIndex >= question.Choices.Count)
            throw new DomainException("Choice could not be found");

        var isCorrect = question.Choices[choiceIndex].IsCorrect;

        _answers.Add(new SessionAnswer(questionId, choiceIndex, isCorrect));

        var correctChoice = question.Choices.First(c => c.IsCorrect);

        var feedback = question.Choices[choiceIndex].Feedback;
        var feedbackEn = question.Choices[choiceIndex].FeedbackEn;

        return new AnswerResult(isCorrect, feedback, feedbackEn, correctChoice.Text, correctChoice.TextEn);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public SessionScore Finish(Quiz quiz)
    {
        if (_answers.Count != quiz.Questions.Count)
            throw new DomainException("All questions must be answered before the session can be finished");

        CompletedAt = DateTime.UtcNow;

        var correctCount = _answers.Count(a => a.IsCorrect);

        return new SessionScore(correctCount, quiz.Questions.Count);
    }

    private GameSession(QuizId quizId)
    {
        Id = new GameSessionId(Guid.NewGuid());
        QuizId = quizId;
    }

    // EF Core constructor
    private GameSession() { }
}

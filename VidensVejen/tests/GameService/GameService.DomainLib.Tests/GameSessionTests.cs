using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class GameSessionTests
{
    private static Quiz CreateValidDraft()
    {
        return Quiz.Create("Quiz", Guid.NewGuid(), "vertical", "Done",
            [new NewQuestion("Question", [new Choice("Yes", true), new Choice("No", false)], "Correct", "Incorrect")]);
    }

    [Fact]
    public void Start_GivenPublishedQuiz_CreatesSession()
    {
        // Arrange
        var quiz = CreateValidDraft();
        quiz.Publish();

        // Act
        var session = GameSession.Start(quiz);

        // Assert
        Assert.NotEqual(Guid.Empty, session.Id.Value);
        Assert.Equal(quiz.Id, session.QuizId);
    }

    [Fact]
    public void Start_GivenDraft_ThrowsDomainException()
    {
        // Arrange
        var quiz = CreateValidDraft();

        // Act & Assert
        Assert.Throws<DomainException>(() => GameSession.Start(quiz));
    }
}

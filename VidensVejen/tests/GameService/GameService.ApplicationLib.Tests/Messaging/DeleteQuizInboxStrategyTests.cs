using System.Text.Json;
using GameService.ApplicationLib.Messaging.Inbox.Strategies;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.ValueObjects;
using GameService.DomainLib.ValueObjects.Quizzes;
using Moq;
using VidensVejen.ContractsLib.Events;

namespace GameService.ApplicationLib.Tests.Messaging;

public class DeleteQuizInboxStrategyTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public async Task Apply_GivenExhibition_DeletesAllReturnedQuizzesWithoutSaving(int count)
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();
        var integrationEvent = new QuizDeletionIntegrationEvent(Guid.NewGuid(), "quiz-deletion", exhibitionId);
        var quizzes = new List<Quiz>();
        for (var i = 0; i < count; i++)
        {
            quizzes.Add(Quiz.Create("Quiz", "Quiz EN", exhibitionId, "vertical", null, null,
                [new NewQuestion("Question", "Question EN",
                    [new Choice("Yes", "Yes EN", true, null, null), new Choice("No", "No EN", false, null, null)])],
                "Instructions", "Instructions EN"));
        }
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetAllForExhibitionAsync(new ExhibitionId(exhibitionId))).ReturnsAsync(quizzes);
        IInboxMessageStrategy strategy = new DeleteQuizInboxStrategy(repository.Object);

        // Act
        var result = await strategy.ApplyAsync(integrationEvent.EventId, JsonSerializer.Serialize(integrationEvent), CancellationToken.None);

        // Assert
        Assert.Equal(typeof(QuizDeletionIntegrationEvent).FullName, strategy.EventType);
        Assert.Null(result);
        repository.Verify(r => r.GetAllForExhibitionAsync(new ExhibitionId(exhibitionId)), Times.Once);
        repository.Verify(r => r.Delete(It.IsAny<Quiz>()), Times.Exactly(count));
        foreach (var quiz in quizzes)
        {
            repository.Verify(r => r.Delete(quiz), Times.Once);
        }
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Theory]
    [InlineData("null")]
    [InlineData("invalid JSON")]
    public async Task Apply_GivenInvalidContent_ThrowsWithoutAccessingRepository(string content)
    {
        // Arrange
        var repository = new Mock<IQuizRepository>();
        IInboxMessageStrategy strategy = new DeleteQuizInboxStrategy(repository.Object);

        // Act & Assert
        if (content == "null")
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => strategy.ApplyAsync(Guid.NewGuid(), content, CancellationToken.None));
        }
        else
        {
            await Assert.ThrowsAsync<JsonException>(() => strategy.ApplyAsync(Guid.NewGuid(), content, CancellationToken.None));
        }
        repository.VerifyNoOtherCalls();
    }
}

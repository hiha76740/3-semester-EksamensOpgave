using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.Enums;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class PublishQuizTests
{
    private static Quiz CreateValidDraft()
    {
        List<Choice> choices =
        [
            new Choice("Yes", "Yes EN", true, "Correct", "Correct EN"),
            new Choice("No", "No EN", false, "Incorrect", "Incorrect EN")
        ];
        List<NewQuestion> questions = [new NewQuestion("Question", "Question EN", choices)];

        return Quiz.Create(
            "Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", questions);
    }

    [Fact]
    public async Task Handle_GivenValidDraft_PublishesAndSaves()
    {
        // Arrange
        var quiz = CreateValidDraft();
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        IPublishQuizHandler handler = new PublishQuizHandler(repository.Object);

        // Act
        await handler.HandleAsync(quiz.Id.Value);

        // Assert
        Assert.Equal(PublicationStatus.Published, quiz.Status);
        repository.Verify(r => r.GetQuizByIdAsync(It.Is<QuizId>(id => id.Value == quiz.Id.Value)), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownId_ThrowsNotFoundExceptionWithoutSaving()
    {
        // Arrange
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync((Quiz?)null);
        IPublishQuizHandler handler = new PublishQuizHandler(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenInvalidDraft_ThrowsDomainExceptionWithoutSaving()
    {
        // Arrange
        var quiz = Quiz.Create(null, "Quiz EN", null, null, null, "Done EN", null);
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        IPublishQuizHandler handler = new PublishQuizHandler(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(quiz.Id.Value));
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }
}

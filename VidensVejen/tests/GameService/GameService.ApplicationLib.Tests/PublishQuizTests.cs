using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class PublishQuizTests
{
    private static QuizDraft CreateValidDraft()
    {
        return QuizDraft.Create("Quiz", Guid.NewGuid(), "vertical", "Done",
            [new NewQuestion("Question", [new Choice("Yes", true), new Choice("No", false)], "Correct", "Incorrect")]);
    }

    [Fact]
    public async Task Handle_GivenValidDraft_AddsQuizDeletesDraftAndSavesOnce()
    {
        // Arrange
        var draft = CreateValidDraft();
        var draftRepository = new Mock<IQuizDraftRepository>();
        draftRepository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync(draft);
        var quizRepository = new Mock<IQuizRepository>();
        Quiz? savedQuiz = null;
        quizRepository.Setup(r => r.AddAsync(It.IsAny<Quiz>()))
            .Callback<Quiz>(quiz => savedQuiz = quiz);
        IPublishQuizHandler handler = new PublishQuizHandler(draftRepository.Object, quizRepository.Object);

        // Act
        var quizId = await handler.HandleAsync(draft.Id.Value);

        // Assert
        Assert.NotNull(savedQuiz);
        Assert.Equal(savedQuiz.Id.Value, quizId);
        Assert.Equal("Quiz", savedQuiz.Title);
        draftRepository.Verify(r => r.Delete(draft), Times.Once);
        draftRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownDraft_ThrowsNotFoundExceptionWithoutWriting()
    {
        // Arrange
        var draftRepository = new Mock<IQuizDraftRepository>();
        draftRepository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync((QuizDraft?)null);
        var quizRepository = new Mock<IQuizRepository>();
        IPublishQuizHandler handler = new PublishQuizHandler(draftRepository.Object, quizRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));
        draftRepository.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        draftRepository.Verify(r => r.SaveAsync(), Times.Never);
        quizRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenInvalidDraft_ThrowsDomainExceptionWithoutWriting()
    {
        // Arrange
        var draft = QuizDraft.Create(null, null, null, null, null);
        var draftRepository = new Mock<IQuizDraftRepository>();
        draftRepository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync(draft);
        var quizRepository = new Mock<IQuizRepository>();
        IPublishQuizHandler handler = new PublishQuizHandler(draftRepository.Object, quizRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(draft.Id.Value));
        draftRepository.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        draftRepository.Verify(r => r.SaveAsync(), Times.Never);
        quizRepository.VerifyNoOtherCalls();
    }
}

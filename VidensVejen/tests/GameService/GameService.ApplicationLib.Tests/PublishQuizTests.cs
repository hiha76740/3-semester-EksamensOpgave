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
        List<Choice> choices =
        [
            new Choice("Yes", "Yes EN", true, "Correct", "Correct EN"),
            new Choice("No", "No EN", false, "Incorrect", "Incorrect EN")
        ];
        List<NewQuestion> questions = [new NewQuestion("Question", "Question EN", choices)];

        return QuizDraft.Create(
            "Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", questions);
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
        Assert.Equal("Quiz EN", savedQuiz.TitleEn);
        draftRepository.Verify(r => r.GetDraftByIdAsync(It.Is<QuizDraftId>(id => id.Value == draft.Id.Value)), Times.Once);
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
        var draft = QuizDraft.Create(null, "Quiz EN", null, null, null, "Done EN", null);
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

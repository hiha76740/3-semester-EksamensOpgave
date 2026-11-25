using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class PublishQuizTests
{
    private static CreateQuizCommand CreateCommand()
    {
        List<CreateChoiceCommand> choices =
        [
            new CreateChoiceCommand("Yes", "Yes EN", true, "Correct", "Correct EN"),
            new CreateChoiceCommand("No", "No EN", false, "Incorrect", "Incorrect EN")
        ];
        List<CreateQuestionCommand> questions =
        [
            new CreateQuestionCommand("Question", "Question EN", choices)
        ];

        return new CreateQuizCommand(
            "Quiz", "Quiz EN", Guid.NewGuid(), "vertical", questions, "Done", "Done EN");
    }

    [Fact]
    public async Task Handle_GivenValidCommand_AddsQuizDeletesDraftAndSavesOnce()
    {
        // Arrange
        var draft = QuizDraft.Create(null, null, null, null, null, null, null);
        var draftRepository = new Mock<IQuizDraftRepository>();
        draftRepository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync(draft);
        var quizRepository = new Mock<IQuizRepository>();
        Quiz? savedQuiz = null;
        quizRepository.Setup(r => r.AddAsync(It.IsAny<Quiz>()))
            .Callback<Quiz>(quiz => savedQuiz = quiz);
        IPublishQuizHandler handler = new PublishQuizHandler(
            draftRepository.Object,
            quizRepository.Object,
            new Mock<ILogger<PublishQuizHandler>>().Object);
        var command = CreateCommand();

        // Act
        var quizId = await handler.HandleAsync(draft.Id.Value, command);

        // Assert
        Assert.NotNull(savedQuiz);
        Assert.Equal(savedQuiz.Id.Value, quizId);
        Assert.Equal("Quiz", savedQuiz.Title);
        Assert.Equal("Quiz EN", savedQuiz.TitleEn);
        Assert.Equal(command.ExhibitionId, savedQuiz.ExhibitionId.Value);
        var question = Assert.Single(savedQuiz.Questions);
        Assert.Equal("Question", question.Text);
        Assert.Equal(2, question.Choices.Count);
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
        IPublishQuizHandler handler = new PublishQuizHandler(
            draftRepository.Object,
            quizRepository.Object,
            new Mock<ILogger<PublishQuizHandler>>().Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid(), CreateCommand()));
        draftRepository.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        draftRepository.Verify(r => r.SaveAsync(), Times.Never);
        quizRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenInvalidCommand_ThrowsDomainExceptionWithoutWriting()
    {
        // Arrange
        var draft = QuizDraft.Create(null, null, null, null, null, null, null);
        var draftRepository = new Mock<IQuizDraftRepository>();
        draftRepository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync(draft);
        var quizRepository = new Mock<IQuizRepository>();
        IPublishQuizHandler handler = new PublishQuizHandler(
            draftRepository.Object,
            quizRepository.Object,
            new Mock<ILogger<PublishQuizHandler>>().Object);
        var command = new CreateQuizCommand("Quiz", null, null, "vertical", null, null, null);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(draft.Id.Value, command));
        draftRepository.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        draftRepository.Verify(r => r.SaveAsync(), Times.Never);
        quizRepository.VerifyNoOtherCalls();
    }
}

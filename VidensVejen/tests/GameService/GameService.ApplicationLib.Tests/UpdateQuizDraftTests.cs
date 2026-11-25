using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class UpdateQuizDraftTests
{
    [Fact]
    public async Task Handle_GivenCommand_UpdatesDraftAndSaves()
    {
        // Arrange
        var draft = QuizDraft.Create(null, null, null, null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync(draft);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object,
            new Mock<ILogger<UpdateQuizDraftHandler>>().Object);
        var command = new CreateQuizCommand("Quiz", "Quiz EN", null, null,
            [new CreateQuestionCommand("Question", null, [new CreateChoiceCommand("A", null, true, null, null)])], null, null);

        // Act
        await handler.HandleAsync(draft.Id.Value, command);

        // Assert
        Assert.Equal("Quiz", draft.Title);
        Assert.Equal("Quiz EN", draft.TitleEn);
        var question = Assert.Single(draft.Questions);
        Assert.Equal("Question", question.Text);
        repository.Verify(r => r.GetDraftByIdAsync(It.Is<QuizDraftId>(id => id.Value == draft.Id.Value)), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownDraft_ThrowsNotFoundExceptionWithoutSaving()
    {
        // Arrange
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(It.IsAny<QuizDraftId>()))
            .ReturnsAsync((QuizDraft?)null);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object,
            new Mock<ILogger<UpdateQuizDraftHandler>>().Object);
        var command = new CreateQuizCommand(null, null, null, null, null, null, null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid(), command));
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }
}

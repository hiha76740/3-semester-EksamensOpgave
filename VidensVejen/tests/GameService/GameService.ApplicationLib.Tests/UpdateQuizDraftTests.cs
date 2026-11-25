using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.ValueObjects.Drafts;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class UpdateQuizDraftTests
{
    [Fact]
    public async Task Handle_GivenCommand_MapsAllFieldsAndReplacesQuestions()
    {
        // Arrange
        var draft = QuizDraft.Create();
        draft.Update("Old", null, null, null, null, null,
            [new NewQuestionDraft("Old question", null, null)], "Instructions", "Instructions EN");
        var exhibitionId = Guid.NewGuid();
        var command = new UpdateQuizDraftCommand(draft.Id.Value, "Quiz", "Quiz EN", exhibitionId,
            "vertical", [new UpdateQuestionDraftCommand("Question", "Question EN",
                [new UpdateChoiceDraftCommand("Answer", "Answer EN", true, "Feedback", "Feedback EN")])],
            "Done", "Done EN", "Instructions", "Instructions EN");
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object, NullLogger<UpdateQuizDraftHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.Equal(command.DraftId, draft.Id.Value);
        Assert.Equal(command.Title, draft.Title);
        Assert.Equal(command.TitleEn, draft.TitleEn);
        Assert.Equal(command.Instruction, draft.Instruction);
        Assert.Equal(command.InstructionEn, draft.InstructionEn);
        Assert.Equal(exhibitionId, draft.ExhibitionId?.Value);
        Assert.Equal(command.QuizLayout, draft.QuizLayout);
        Assert.Equal(command.EndMessage, draft.EndMessage);
        Assert.Equal(command.EndMessageEn, draft.EndMessageEn);
        var question = Assert.Single(draft.Questions);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        var choice = Assert.Single(question.Choices);
        Assert.Equal("Answer", choice.Text);
        Assert.Equal("Answer EN", choice.TextEn);
        Assert.True(choice.IsCorrect);
        Assert.Equal("Feedback", choice.Feedback);
        Assert.Equal("Feedback EN", choice.FeedbackEn);
        repository.Verify(r => r.GetDraftByIdAsync(draft.Id), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyFields_ClearsExistingDraftAndSaves()
    {
        // Arrange
        var draft = QuizDraft.Create();
        draft.Update("Old", "Old EN", Guid.NewGuid(), "vertical", "Done", "Done EN",
            [new NewQuestionDraft("Old question", null, null)], "Instructions", "Instructions EN");
        var command = new UpdateQuizDraftCommand(draft.Id.Value, null, null, null, null, null, null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object, NullLogger<UpdateQuizDraftHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.Null(draft.Instruction);
        Assert.Null(draft.InstructionEn);
        Assert.Null(draft.Title);
        Assert.Null(draft.TitleEn);
        Assert.Null(draft.ExhibitionId);
        Assert.Null(draft.QuizLayout);
        Assert.Null(draft.EndMessage);
        Assert.Null(draft.EndMessageEn);
        Assert.Empty(draft.Questions);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenIncompleteQuestionWithoutChoices_SavesDraft()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = new UpdateQuizDraftCommand(draft.Id.Value, null, null, null, null,
            [new UpdateQuestionDraftCommand(null, null, null)], null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object, NullLogger<UpdateQuizDraftHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        var question = Assert.Single(draft.Questions);
        Assert.Null(question.Text);
        Assert.Empty(question.Choices);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownDraft_ThrowsWithoutSaving()
    {
        // Arrange
        var command = new UpdateQuizDraftCommand(Guid.NewGuid(), null, null, null, null, null, null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(new QuizDraftId(command.DraftId)))
            .ReturnsAsync((QuizDraft?)null);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object, NullLogger<UpdateQuizDraftHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));

        // Assert
        repository.Verify(r => r.GetDraftByIdAsync(new QuizDraftId(command.DraftId)), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = new UpdateQuizDraftCommand(draft.Id.Value, "Quiz", null, null, null, null, null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var expected = new InvalidOperationException("Save failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object, NullLogger<UpdateQuizDraftHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Handle_GivenBlankChoiceText_SavesIncompleteDraft(string? text)
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = new UpdateQuizDraftCommand(draft.Id.Value, null, null, null, null,
            [new UpdateQuestionDraftCommand(null, null,
                [new UpdateChoiceDraftCommand(text, null, false, null, null)])],
            null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IUpdateQuizDraftHandler handler = new UpdateQuizDraftHandler(
            repository.Object, NullLogger<UpdateQuizDraftHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        var question = Assert.Single(draft.Questions);
        var choice = Assert.Single(question.Choices);
        Assert.Equal(text, choice.Text);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}

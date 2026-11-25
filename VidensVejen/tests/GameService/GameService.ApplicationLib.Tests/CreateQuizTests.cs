using System.Data;
using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.Entities.Quizzes;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class CreateQuizTests
{
    private static CreateQuizCommand CreateCommand(Guid draftId)
    {
        return new CreateQuizCommand(draftId, "Quiz", "Quiz EN", Guid.NewGuid(), "vertical",
            [new CreateQuestionCommand("Question", "Question EN",
                [new CreateChoiceCommand("Yes", "Yes EN", true, "Correct", "Correct EN"),
                 new CreateChoiceCommand("No", "No EN", false, "Incorrect", "Incorrect EN")])],
            "Done", "Done EN", "Instructions", "Instructions EN");
    }

    [Fact]
    public async Task Handle_GivenValidCommand_MapsQuizDeletesDraftAndCommitsTransaction()
    {
        // Arrange
        var draft = QuizDraft.Create();
        draft.Update("Old draft title", null, null, null, null, null, null, "Instructions", "Instructions EN");
        var command = CreateCommand(draft.Id.Value);
        var drafts = new Mock<IQuizDraftRepository>(MockBehavior.Strict);
        var quizzes = new Mock<IQuizRepository>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        Quiz? savedQuiz = null;
        var sequence = new MockSequence();
        unitOfWork.InSequence(sequence).Setup(u => u.BeginTransaction(IsolationLevel.Serializable));
        drafts.InSequence(sequence).Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        quizzes.InSequence(sequence).Setup(r => r.AddAsync(It.IsAny<Quiz>()))
            .Callback<Quiz>(quiz => savedQuiz = quiz).Returns(Task.CompletedTask);
        drafts.InSequence(sequence).Setup(r => r.Delete(draft));
        unitOfWork.InSequence(sequence).Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        unitOfWork.InSequence(sequence).Setup(u => u.Commit());
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(savedQuiz);
        Assert.NotEqual(Guid.Empty, savedQuiz.Id.Value);
        Assert.Equal(command.Title, savedQuiz.Title);
        Assert.Equal(command.TitleEn, savedQuiz.TitleEn);
        Assert.Equal(command.Instruction, savedQuiz.Instruction);
        Assert.Equal(command.InstructionEn, savedQuiz.InstructionEn);
        Assert.Equal(command.ExhibitionId, savedQuiz.ExhibitionId.Value);
        Assert.Equal(command.QuizLayout, savedQuiz.QuizLayout);
        Assert.Equal(command.EndMessage, savedQuiz.EndMessage);
        Assert.Equal(command.EndMessageEn, savedQuiz.EndMessageEn);
        var question = Assert.Single(savedQuiz.Questions);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        Assert.Collection(question.Choices,
            choice =>
            {
                Assert.Equal("Yes", choice.Text);
                Assert.Equal("Yes EN", choice.TextEn);
                Assert.True(choice.IsCorrect);
                Assert.Equal("Correct", choice.Feedback);
                Assert.Equal("Correct EN", choice.FeedbackEn);
            },
            choice =>
            {
                Assert.Equal("No", choice.Text);
                Assert.Equal("No EN", choice.TextEn);
                Assert.False(choice.IsCorrect);
                Assert.Equal("Incorrect", choice.Feedback);
                Assert.Equal("Incorrect EN", choice.FeedbackEn);
            });
        unitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        drafts.Verify(r => r.GetDraftByIdAsync(draft.Id), Times.Once);
        quizzes.Verify(r => r.AddAsync(savedQuiz), Times.Once);
        drafts.Verify(r => r.Delete(draft), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        unitOfWork.Verify(u => u.Rollback(), Times.Never);
        drafts.Verify(r => r.SaveAsync(), Times.Never);
        quizzes.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownDraft_RollsBackWithoutWriting()
    {
        // Arrange
        var command = CreateCommand(Guid.NewGuid());
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(new QuizDraftId(command.DraftId))).ReturnsAsync((QuizDraft?)null);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));

        // Assert
        drafts.Verify(r => r.GetDraftByIdAsync(new QuizDraftId(command.DraftId)), Times.Once);
        drafts.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Never);
        unitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidQuiz_RollsBackAndKeepsDraft()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = CreateCommand(draft.Id.Value) with { Questions = [] };
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));

        // Assert
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Never);
        drafts.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenAddFailure_RollsBackAndRethrowsOriginalException()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = CreateCommand(draft.Id.Value);
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var expected = new InvalidOperationException("Add failed");
        quizzes.Setup(r => r.AddAsync(It.IsAny<Quiz>())).ThrowsAsync(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        drafts.Verify(r => r.Delete(draft), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenDeleteDraftFailure_RollsBackAndRethrowsOriginalException()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = CreateCommand(draft.Id.Value);
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var expected = new InvalidOperationException("DeleteDraft failed");
        drafts.Setup(r => r.Delete(draft)).Throws(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        drafts.Verify(r => r.Delete(draft), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RollsBackAndRethrowsOriginalException()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = CreateCommand(draft.Id.Value);
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var expected = new InvalidOperationException("Save failed");
        unitOfWork.Setup(u => u.SaveChangesAsync()).ThrowsAsync(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        drafts.Verify(r => r.Delete(draft), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenCommitFailure_RollsBackAndRethrowsOriginalException()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = CreateCommand(draft.Id.Value);
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var expected = new InvalidOperationException("Commit failed");
        unitOfWork.Setup(u => u.Commit()).Throws(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        drafts.Verify(r => r.Delete(draft), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_GivenMissingInstruction_RollsBackAndKeepsDraft(bool english)
    {
        // Arrange
        var draft = QuizDraft.Create();
        var command = CreateCommand(draft.Id.Value);
        command = english ? (command with { InstructionEn = "" }) : (command with { Instruction = "" });
        var drafts = new Mock<IQuizDraftRepository>();
        drafts.Setup(r => r.GetDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var quizzes = new Mock<IQuizRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        ICreateQuizHandler handler = new CreateQuizHandler(
            drafts.Object, quizzes.Object, unitOfWork.Object, NullLogger<CreateQuizHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));

        // Assert
        quizzes.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Never);
        drafts.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }
}

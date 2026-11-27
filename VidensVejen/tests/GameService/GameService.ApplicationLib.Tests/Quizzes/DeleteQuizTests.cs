using GameService.ApplicationLib.Services;
using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.ValueObjects.Quizzes;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests.Quizzes;

public class DeleteQuizTests
{
    [Fact]
    public async Task Handle_GivenExistingEntity_SetsRowVersionDeletesAndSaves()
    {
        // Arrange
        var entity = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, null,
            [new NewQuestion("Question", "Question EN",
                [new Choice("Yes", "Yes EN", true, null, null), new Choice("No", "No EN", false, null, null)])],
            "Instructions", "Instructions EN");
        var command = new DeleteQuizCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizRepository>(MockBehavior.Strict);
        var sequence = new MockSequence();
        repository.InSequence(sequence).Setup(r => r.GetQuizByIdAsync(entity.Id)).ReturnsAsync(entity);
        repository.InSequence(sequence).Setup(r => r.SetOriginalRowVersion(entity, command.RowVersion));
        repository.InSequence(sequence).Setup(r => r.Delete(entity));
        repository.InSequence(sequence).Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);
        IDeleteQuizHandler handler = new DeleteQuizHandler(repository.Object, new Mock<IExhibitionService>().Object, NullLogger<DeleteQuizHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        repository.Verify(r => r.GetQuizByIdAsync(entity.Id), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(entity, command.RowVersion), Times.Once);
        repository.Verify(r => r.Delete(entity), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownEntity_ThrowsWithoutDeletingOrSaving()
    {
        // Arrange
        var command = new DeleteQuizCommand(Guid.NewGuid(), [1, 2, 3]);
        var id = new QuizId(command.DraftId);
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(id)).ReturnsAsync((Quiz?)null);
        IDeleteQuizHandler handler = new DeleteQuizHandler(repository.Object, new Mock<IExhibitionService>().Object, NullLogger<DeleteQuizHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));

        // Assert
        repository.Verify(r => r.GetQuizByIdAsync(id), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(It.IsAny<Quiz>(), It.IsAny<byte[]>()), Times.Never);
        repository.Verify(r => r.Delete(It.IsAny<Quiz>()), Times.Never);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenRowVersionFailure_RethrowsOriginalException()
    {
        // Arrange
        var entity = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, null,
            [new NewQuestion("Question", "Question EN",
                [new Choice("Yes", "Yes EN", true, null, null), new Choice("No", "No EN", false, null, null)])],
            "Instructions", "Instructions EN");
        var command = new DeleteQuizCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(entity.Id)).ReturnsAsync(entity);
        var expected = new InvalidOperationException("RowVersion failed");
        repository.Setup(r => r.SetOriginalRowVersion(entity, command.RowVersion)).Throws(expected);
        IDeleteQuizHandler handler = new DeleteQuizHandler(repository.Object, new Mock<IExhibitionService>().Object, NullLogger<DeleteQuizHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.Delete(entity), Times.Never);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenDeleteFailure_RethrowsOriginalException()
    {
        // Arrange
        var entity = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, null,
            [new NewQuestion("Question", "Question EN",
                [new Choice("Yes", "Yes EN", true, null, null), new Choice("No", "No EN", false, null, null)])],
            "Instructions", "Instructions EN");
        var command = new DeleteQuizCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(entity.Id)).ReturnsAsync(entity);
        var expected = new InvalidOperationException("Delete failed");
        repository.Setup(r => r.Delete(entity)).Throws(expected);
        IDeleteQuizHandler handler = new DeleteQuizHandler(repository.Object, new Mock<IExhibitionService>().Object, NullLogger<DeleteQuizHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.Delete(entity), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var entity = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, null,
            [new NewQuestion("Question", "Question EN",
                [new Choice("Yes", "Yes EN", true, null, null), new Choice("No", "No EN", false, null, null)])],
            "Instructions", "Instructions EN");
        var command = new DeleteQuizCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizRepository>();
        repository.Setup(r => r.GetQuizByIdAsync(entity.Id)).ReturnsAsync(entity);
        var expected = new InvalidOperationException("Save failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IDeleteQuizHandler handler = new DeleteQuizHandler(repository.Object, new Mock<IExhibitionService>().Object, NullLogger<DeleteQuizHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.Delete(entity), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenQuizInUse_ThrowsWithoutDeletingOrSaving()
    {
        // Arrange
        var command = new DeleteQuizCommand(Guid.NewGuid(), [1, 2, 3]);
        var repository = new Mock<IQuizRepository>();
        var exhibitions = new Mock<IExhibitionService>();
        exhibitions.Setup(s => s.QuizInUseAsync(new QuizId(command.DraftId))).ReturnsAsync(true);
        IDeleteQuizHandler handler = new DeleteQuizHandler(repository.Object, exhibitions.Object, NullLogger<DeleteQuizHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        exhibitions.Verify(s => s.QuizInUseAsync(new QuizId(command.DraftId)), Times.Once);
        repository.VerifyNoOtherCalls();
    }
}

using Microsoft.Extensions.Logging;
using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.Enums;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;

namespace GameService.ApplicationLib.Tests;

public class CreateQuizTests
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
    public async Task Handle_GivenValidCommand_MapsQuizAndSaves()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizRepository>();
        Quiz? savedQuiz = null;
        repository.Setup(r => r.AddAsync(It.IsAny<Quiz>()))
            .Callback<Quiz>(quiz => savedQuiz = quiz);
        ICreateQuizHandler handler = new CreateQuizHandler(
            repository.Object,
            new Mock<ILogger<CreateQuizHandler>>().Object);

        // Act
        var id = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(savedQuiz);
        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(id, savedQuiz.Id.Value);
        Assert.Equal(PublicationStatus.Draft, savedQuiz.Status);
        Assert.Equal(command.ExhibitionId, savedQuiz.ExhibitionId?.Value);
        Assert.Equal(command.Title, savedQuiz.Title);
        Assert.Equal(command.TitleEn, savedQuiz.TitleEn);
        Assert.Equal(command.EndMessageEn, savedQuiz.EndMessageEn);
        Assert.Equal(command.QuizLayout, savedQuiz.QuizLayout);
        Assert.Equal(command.EndMessage, savedQuiz.EndMessage);
        var question = Assert.Single(savedQuiz.Questions);
        Assert.Equal(command.Questions[0].Text, question.Text);
        Assert.Equal(command.Questions[0].TextEn, question.TextEn);
        for (var index = 0; index < question.Choices.Count; index++)
        {
            var expectedChoice = command.Questions[0].Choices[index];
            var actualChoice = question.Choices[index];
            Assert.Equal(expectedChoice.TextEn, actualChoice.TextEn);
            Assert.Equal(expectedChoice.Feedback, actualChoice.Feedback);
            Assert.Equal(expectedChoice.FeedbackEn, actualChoice.FeedbackEn);
        }
        Assert.Collection(question.Choices,
            choice => { Assert.Equal("Yes", choice.Text); Assert.True(choice.IsCorrect); },
            choice => { Assert.Equal("No", choice.Text); Assert.False(choice.IsCorrect); });
        repository.Verify(r => r.AddAsync(savedQuiz), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenAddFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizRepository>();
        var expected = new InvalidOperationException("Write failed");
        repository.Setup(r => r.AddAsync(It.IsAny<Quiz>())).ThrowsAsync(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(
            repository.Object,
            new Mock<ILogger<CreateQuizHandler>>().Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizRepository>();
        var expected = new InvalidOperationException("Write failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(
            repository.Object,
            new Mock<ILogger<CreateQuizHandler>>().Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}

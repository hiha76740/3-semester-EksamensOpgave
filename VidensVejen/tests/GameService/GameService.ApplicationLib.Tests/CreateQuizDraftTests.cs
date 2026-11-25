//using GameService.ApplicationLib.Handlers;
//using GameService.ApplicationLib.Repositories;
//using GameService.DomainLib.Entities;
//using GameService.FacadeLib.Commands.DTOs;
//using GameService.FacadeLib.Commands.Interfaces;
//using Microsoft.Extensions.Logging;
//using Moq;

//namespace GameService.ApplicationLib.Tests;

//public class CreateQuizDraftTests
//{
//    private static CreateQuizCommand CreateCommand()
//    {
//        List<CreateChoiceCommand> choices =
//        [
//            new CreateChoiceCommand("Yes", "Yes EN", true, "Correct", "Correct EN"),
//            new CreateChoiceCommand("No", "No EN", false, "Incorrect", "Incorrect EN")
//        ];
//        List<CreateQuestionCommand> questions =
//        [
//            new CreateQuestionCommand("Question", "Question EN", choices)
//        ];

//        return new CreateQuizCommand(
//            "Quiz", "Quiz EN", Guid.NewGuid(), "vertical", questions, "Done", "Done EN");
//    }

//    [Fact]
//    public async Task Handle_GivenValidCommand_MapsDraftAndSaves()
//    {
//        // Arrange
//        var command = CreateCommand();
//        var repository = new Mock<IQuizDraftRepository>();
//        QuizDraft? savedDraft = null;
//        repository.Setup(r => r.AddAsync(It.IsAny<QuizDraft>()))
//            .Callback<QuizDraft>(draft => savedDraft = draft);
//        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
//            repository.Object,
//            new Mock<ILogger<CreateQuizDraftHandler>>().Object);

//        // Act
//        var id = await handler.HandleAsync(command);

//        // Assert
//        Assert.NotNull(savedDraft);
//        Assert.Equal(id, savedDraft.Id.Value);
//        Assert.Equal(command.Title, savedDraft.Title);
//        Assert.Equal(command.TitleEn, savedDraft.TitleEn);
//        Assert.Equal(command.ExhibitionId, savedDraft.ExhibitionId?.Value);
//        Assert.Equal(command.QuizLayout, savedDraft.QuizLayout);
//        Assert.Equal(command.EndMessage, savedDraft.EndMessage);
//        Assert.Equal(command.EndMessageEn, savedDraft.EndMessageEn);
//        var question = Assert.Single(savedDraft.Questions);
//        Assert.Equal("Question", question.Text);
//        Assert.Equal("Question EN", question.TextEn);
//        Assert.Collection(question.Choices,
//            choice =>
//            {
//                Assert.Equal("Yes", choice.Text);
//                Assert.Equal("Yes EN", choice.TextEn);
//                Assert.True(choice.IsCorrect);
//                Assert.Equal("Correct", choice.Feedback);
//                Assert.Equal("Correct EN", choice.FeedbackEn);
//            },
//            choice =>
//            {
//                Assert.Equal("No", choice.Text);
//                Assert.False(choice.IsCorrect);
//                Assert.Equal("Incorrect", choice.Feedback);
//            });
//        repository.Verify(r => r.AddAsync(savedDraft), Times.Once);
//        repository.Verify(r => r.SaveAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenIncompleteCommand_SavesDraft()
//    {
//        // Arrange
//        var command = new CreateQuizCommand(null, null, null, null,
//            [new CreateQuestionCommand(null, null, [new CreateChoiceCommand("A", null, true, null, null)])], null, null);
//        var repository = new Mock<IQuizDraftRepository>();
//        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
//            repository.Object,
//            new Mock<ILogger<CreateQuizDraftHandler>>().Object);

//        // Act
//        await handler.HandleAsync(command);

//        // Assert
//        repository.Verify(r => r.AddAsync(It.IsAny<QuizDraft>()), Times.Once);
//        repository.Verify(r => r.SaveAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenNoQuestions_SavesEmptyDraft()
//    {
//        // Arrange
//        var command = new CreateQuizCommand(null, null, null, null, null, null, null);
//        var repository = new Mock<IQuizDraftRepository>();
//        QuizDraft? savedDraft = null;
//        repository.Setup(r => r.AddAsync(It.IsAny<QuizDraft>()))
//            .Callback<QuizDraft>(draft => savedDraft = draft);
//        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
//            repository.Object,
//            new Mock<ILogger<CreateQuizDraftHandler>>().Object);

//        // Act
//        await handler.HandleAsync(command);

//        // Assert
//        Assert.NotNull(savedDraft);
//        Assert.Empty(savedDraft.Questions);
//        repository.Verify(r => r.SaveAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
//    {
//        // Arrange
//        var command = CreateCommand();
//        var repository = new Mock<IQuizDraftRepository>();
//        var expected = new InvalidOperationException("Write failed");
//        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
//        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
//            repository.Object,
//            new Mock<ILogger<CreateQuizDraftHandler>>().Object);

//        // Act & Assert
//        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
//        Assert.Same(expected, exception);
//        repository.Verify(r => r.AddAsync(It.IsAny<QuizDraft>()), Times.Once);
//    }
//}

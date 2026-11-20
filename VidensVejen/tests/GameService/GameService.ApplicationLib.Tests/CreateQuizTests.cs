//using GameService.ApplicationLib.Handlers;
//using GameService.ApplicationLib.Repositories;
//using GameService.DomainLib.Entities;
//using GameService.FacadeLib.Commands.DTOs;
//using GameService.FacadeLib.Commands.Interfaces;
//using Moq;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace GameService.ApplicationLib.Tests;

//public class CreateQuizTests
//{
//    private static readonly Guid s_exhibitionId = Guid.NewGuid();

//    private static CreateQuizCommand CreateCommand(bool secondChoiceCorrect)
//    {
//        return new CreateQuizCommand(
//            "Hjælp Otto",
//            s_exhibitionId,
//            "Vertikalt",
//            [
//                new CreateQuestionCommand(
//                    "🪑 En stol har fået et løst ben. Hvad er bedst?",
//                    [new CreateChoiceCommand("🔧 Reparér stolen", true), new CreateChoiceCommand("🔥 Storskrald", secondChoiceCorrect)],
//                    "God idé!",
//                    "Næsten!"),
//                new CreateQuestionCommand(
//                    "🥫 Hvad gør du med en tom dåse?",
//                    [new CreateChoiceCommand("♻ Genanvendelse", true), new CreateChoiceCommand("🗑 Restaffald", false)],
//                    null,
//                    null)
//            ],
//            "Godt hjulpet! 🦦");
//    }

//    [Fact]
//    public async Task Handle_GivenValidQuiz_AddsAndSavesQuiz()
//    {
//        // Arrange
//        var repository = new Mock<IQuizRepository>();
//        Quiz? registeredQuiz = null;

//        repository.Setup(r => r.AddAsync(It.IsAny<Quiz>()))
//            .Callback<Quiz>(quiz => registeredQuiz = quiz);
//        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);

//        // Act
//        var id = await handler.HandleAsync(CreateCommand(false));

//        // Assert
//        Assert.NotNull(registeredQuiz);
//        Assert.Equal(id, registeredQuiz.Id.Value);
//        Assert.Equal("Hjælp Otto", registeredQuiz.Title);
//        Assert.Equal(s_exhibitionId, registeredQuiz.ExhibitionId);
//        Assert.Equal(2, registeredQuiz.Questions.Count);
//        Assert.Equal(2, registeredQuiz.Questions[0].Choices.Count);
//        Assert.Equal("God idé!", registeredQuiz.Questions[0].CorrectFeedback);
//        repository.Verify(r => r.AddAsync(registeredQuiz), Times.Once);
//        repository.Verify(r => r.SaveAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenQuestionWithTwoCorrectChoices_ThrowsBeforeWriting()
//    {
//        // Arrange
//        var repository = new Mock<IQuizRepository>();
//        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);

//        // Act & Assert
//        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(CreateCommand(true)));

//        repository.VerifyNoOtherCalls();
//    }

//    [Fact]
//    public async Task Handle_GivenNoQuestions_ThrowsBeforeWriting()
//    {
//        // Arrange
//        var repository = new Mock<IQuizRepository>();
//        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);
//        var command = new CreateQuizCommand("Hjælp Otto", s_exhibitionId, "Vertikalt", [], null);

//        // Act & Assert
//        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));

//        repository.VerifyNoOtherCalls();
//    }
//}

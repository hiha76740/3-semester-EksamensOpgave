using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuizTests
{
    private static readonly Guid s_exhibitionId = Guid.NewGuid();

    private static NewQuestion CreateValidQuestion()
    {
        return new NewQuestion(
            "🪑 En stol har fået et løst ben. Hvad er bedst?",
            [new Choice("🔧 Reparér stolen", true), new Choice("🔥 Storskrald", false)],
            null,
            null);
    }

    [Fact]
    public void Create_GivenValidInput_InitializesQuiz()
    {
        // Act
        var quiz = Quiz.Create("Hjælp Otto", s_exhibitionId, "Vertikalt", "Godt hjulpet! 🦦", [CreateValidQuestion()]);

        // Assert
        Assert.NotEqual(Guid.Empty, quiz.Id.Value);
        Assert.Equal("Hjælp Otto", quiz.Title);
        Assert.Equal(s_exhibitionId, quiz.ExhibitionId);
        Assert.Equal("Vertikalt", quiz.QuizLayout);
        Assert.Equal("Godt hjulpet! 🦦", quiz.EndMessage);
        Assert.Single(quiz.Questions);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankTitle_ThrowsDomainException(string? title)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create(title!, s_exhibitionId, "Vertikalt", null, [CreateValidQuestion()]));
    }

    [Fact]
    public void Create_GivenEmptyExhibitionId_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Hjælp Otto", Guid.Empty, "Vertikalt", null, [CreateValidQuestion()]));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankLayout_ThrowsDomainException(string? layout)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Hjælp Otto", s_exhibitionId, layout!, null, [CreateValidQuestion()]));
    }

    [Fact]
    public void Create_GivenNoQuestions_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Hjælp Otto", s_exhibitionId, "Vertikalt", null, []));
    }

    [Fact]
    public void Create_GivenNullQuestions_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Hjælp Otto", s_exhibitionId, "Vertikalt", null, null!));
    }

    [Fact]
    public void Create_GivenNoEndMessage_AllowsNull()
    {
        // Act
        var quiz = Quiz.Create("Hjælp Otto", s_exhibitionId, "Vertikalt", null, [CreateValidQuestion()]);

        // Assert
        Assert.Null(quiz.EndMessage);
    }

    [Fact]
    public void Create_GivenQuestionWithoutCorrectChoice_ThrowsDomainException()
    {
        // Arrange
        var invalidQuestion = new NewQuestion(
            "Hvad er bedst?",
            [new Choice("Svar", false), new Choice("Svar", false)],
            null,
            null);

        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Hjælp Otto", s_exhibitionId, "Vertikalt", null, [invalidQuestion]));
    }

    [Fact]
    public void AddQuestion_GivenValidQuestion_AddsQuestionToQuiz()
    {
        // Arrange
        var quiz = Quiz.Create("Hjælp Otto", s_exhibitionId, "Vertikalt", null, [CreateValidQuestion()]);

        // Act
        quiz.AddQuestion(CreateValidQuestion());

        // Assert
        Assert.Equal(2, quiz.Questions.Count);
    }
}

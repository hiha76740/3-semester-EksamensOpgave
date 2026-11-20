//using GameService.DomainLib.Entities;
//using GameService.DomainLib.ValueObjects;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace GameService.DomainLib.Tests;

//public class QuestionTests
//{
//    private static List<Choice> CreateChoices(int total, int correct)
//    {
//        var choices = new List<Choice>();

//        for (var i = 0; i < total; i++)
//        {
//            choices.Add(new Choice("Svar", i < correct));
//        }

//        return choices;
//    }

//    [Theory]
//    [InlineData(2)]
//    [InlineData(4)]
//    [InlineData(6)]
//    public void Create_GivenOneCorrectChoice_InitializesQuestion(int totalChoices)
//    {
//        // Arrange
//        var choices = CreateChoices(totalChoices, 1);

//        // Act
//        var question = Question.Create("Hvad er bedst?", choices, "Godt set!", "Næsten!");

//        // Assert
//        Assert.NotEqual(Guid.Empty, question.Id.Value);
//        Assert.Equal("Hvad er bedst?", question.Text);
//        Assert.Equal(totalChoices, question.Choices.Count);
//        Assert.Equal("Godt set!", question.CorrectFeedback);
//        Assert.Equal("Næsten!", question.IncorrectFeedback);
//    }

//    [Theory]
//    [InlineData(0, 0)]
//    [InlineData(1, 1)]
//    public void Create_GivenFewerThanTwoChoices_ThrowsDomainException(int totalChoices, int correctChoices)
//    {
//        // Arrange
//        var choices = CreateChoices(totalChoices, correctChoices);

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Question.Create("Hvad er bedst?", choices, null, null));
//    }

//    [Fact]
//    public void Create_GivenNullChoices_ThrowsDomainException()
//    {
//        // Act & Assert
//        Assert.Throws<DomainException>(() => Question.Create("Hvad er bedst?", null!, null, null));
//    }

//    [Theory]
//    [InlineData(2, 0)]
//    [InlineData(2, 2)]
//    [InlineData(4, 0)]
//    [InlineData(4, 3)]
//    public void Create_GivenNotExactlyOneCorrectChoice_ThrowsDomainException(int totalChoices, int correctChoices)
//    {
//        // Arrange
//        var choices = CreateChoices(totalChoices, correctChoices);

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Question.Create("Hvad er bedst?", choices, null, null));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankText_ThrowsDomainException(string? text)
//    {
//        // Arrange
//        var choices = CreateChoices(2, 1);

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Question.Create(text!, choices, null, null));
//    }

//    [Fact]
//    public void Create_GivenNoFeedback_AllowsNull()
//    {
//        // Act
//        var question = Question.Create("Hvad er bedst?", CreateChoices(2, 1), null, null);

//        // Assert
//        Assert.Null(question.CorrectFeedback);
//        Assert.Null(question.IncorrectFeedback);
//    }

//    [Fact]
//    public void Create_GivenTextWithEmoji_KeepsEmoji()
//    {
//        // Act
//        var question = Question.Create("🪑 Stol med løst ben 🦦", CreateChoices(2, 1), null, null);

//        // Assert
//        Assert.Equal("🪑 Stol med løst ben 🦦", question.Text);
//    }
//}

using GameService.DomainLib.Entities;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class ChoiceTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_GivenValidText_InitializesChoice(bool isCorrect)
    {
        // Act
        var choice = Choice.Create("🔧 Reparér stolen", isCorrect);

        // Assert
        Assert.NotEqual(Guid.Empty, choice.Id.Value);
        Assert.Equal("🔧 Reparér stolen", choice.Text);
        Assert.Equal(isCorrect, choice.IsCorrect);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Choice.Create(text!, true));
    }
}

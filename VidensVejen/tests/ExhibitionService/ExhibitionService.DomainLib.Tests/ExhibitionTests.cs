using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.DomainLib.Tests;

public class ExhibitionTests
{
    private static List<NewSubject> CreateSubjects()
    {
        return [new NewSubject("Subject", "Description",
            [new NewTopic("Topic", "Topic description", "Body", "Label", "Callout", null)])];
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Create_GivenValidData_InitializesExhibitionAndContent(bool hasTheme)
    {
        // Arrange
        Guid? theme = null;
        if (hasTheme)
        { theme = Guid.NewGuid(); }
        var subjects = CreateSubjects();

        // Act
        var exhibition = Exhibition.Create("Exhibition", theme, subjects);

        // Assert
        Assert.NotEqual(Guid.Empty, exhibition.Id.Value);
        Assert.Equal("Exhibition", exhibition.Name);
        Assert.Equal(theme, exhibition.Theme?.Value);
        var subject = Assert.Single(exhibition.Subjects);
        Assert.NotEqual(Guid.Empty, subject.Id.Value);
        Assert.Equal("Subject", subject.Title);
        Assert.Equal("Description", subject.Description);
        var topic = Assert.Single(subject.Topics);
        Assert.Equal("Topic", topic.Title);
        Assert.Equal("Topic description", topic.Description);
        Assert.Equal("Body", topic.Body);
        Assert.Equal("Label", topic.CalloutLabel);
        Assert.Equal("Callout", topic.CalloutText);
        Assert.Null(topic.Quote);
    }

    [Fact]
    public void Create_GivenNoSubjects_ThrowsDomainException()
    {
        // Arrange
        List<NewSubject> subjects = [];

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", null, subjects));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Create_GivenMissingTopics_ThrowsDomainException(bool useNull)
    {
        // Arrange
        var subjects = CreateSubjects();
        List<NewTopic> topics = [];
        if (useNull)
        { topics = null!; }
        subjects[0] = subjects[0] with { Topics = topics };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankName_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        name = invalidValue!;

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankSubjectTitle_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0] = subjects[0] with { Title = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankSubjectDescription_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0] = subjects[0] with { Description = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankTopicTitle_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0].Topics[0] = subjects[0].Topics[0] with { Title = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankTopicDescription_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0].Topics[0] = subjects[0].Topics[0] with { Description = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankBody_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0].Topics[0] = subjects[0].Topics[0] with { Body = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankCalloutLabel_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0].Topics[0] = subjects[0].Topics[0] with { CalloutLabel = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankCalloutText_ThrowsDomainException(string? invalidValue)
    {
        // Arrange
        var name = "Exhibition";
        var subjects = CreateSubjects();
        subjects[0].Topics[0] = subjects[0].Topics[0] with { CalloutText = invalidValue! };

        // Act & Assert
        Assert.Throws<DomainException>(() => Exhibition.Create(name, null, subjects));
    }

    [Fact]
    public void Create_GivenMultipleSubjectsAndTopics_PreservesContentOrderAndQuote()
    {
        // Arrange
        var subjects = CreateSubjects();
        subjects[0].Topics.Add(new NewTopic("Second topic", "Description", "Body", "Label", "Text", "A quote"));
        subjects.Add(new NewSubject("Second subject", "Description",
            [new NewTopic("Third topic", "Description", "Body", "Label", "Text", null)]));

        // Act
        var exhibition = Exhibition.Create("Exhibition", null, subjects);

        // Assert
        Assert.Equal(2, exhibition.Subjects.Count);
        Assert.Equal("Subject", exhibition.Subjects[0].Title);
        Assert.Equal("Second subject", exhibition.Subjects[1].Title);
        Assert.Equal(2, exhibition.Subjects[0].Topics.Count);
        Assert.Equal("Topic", exhibition.Subjects[0].Topics[0].Title);
        Assert.Equal("Second topic", exhibition.Subjects[0].Topics[1].Title);
        Assert.Equal("A quote", exhibition.Subjects[0].Topics[1].Quote);
        Assert.Equal("Third topic", Assert.Single(exhibition.Subjects[1].Topics).Title);
        Assert.NotEqual(exhibition.Subjects[0].Id, exhibition.Subjects[1].Id);
    }
}

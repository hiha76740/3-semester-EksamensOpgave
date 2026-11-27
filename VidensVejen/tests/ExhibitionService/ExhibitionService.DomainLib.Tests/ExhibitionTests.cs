//using ExhibitionService.DomainLib.Entities;
//using ExhibitionService.DomainLib.ValueObjects;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace ExhibitionService.DomainLib.Tests;

//public class ExhibitionTests
//{
//    private static List<NewSubject> CreateSubjects()
//    {
//        return [new NewSubject("Subject", "Subject EN", "subject", "Description", "Description EN",
//            [new NewTopic("Topic", "Topic EN", "Topic description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Callout", "Callout EN", null, null)])];
//    }

//    [Fact]
//    public void Create_GivenValidData_InitializesExhibitionAndContent()
//    {
//        // Arrange
//        var theme = new ImageId(Guid.NewGuid());
//        var subjects = CreateSubjects();

//        // Act
//        var exhibition = Exhibition.Create("Exhibition", "Exhibition EN", theme, subjects);

//        // Assert
//        Assert.NotEqual(Guid.Empty, exhibition.Id.Value);
//        Assert.Equal("Exhibition", exhibition.Name);
//        Assert.Equal("Exhibition EN", exhibition.NameEn);
//        Assert.Equal(theme, exhibition.ThemeImageId);
//        var subject = Assert.Single(exhibition.Subjects);
//        Assert.NotEqual(Guid.Empty, subject.Id.Value);
//        Assert.Equal("Subject", subject.Title);
//        Assert.Equal("Subject EN", subject.TitleEn);
//        Assert.Equal("Description EN", subject.DescriptionEn);
//        Assert.Equal("subject", subject.Slug);
//        Assert.Equal("Description", subject.Description);
//        var topic = Assert.Single(subject.Topics);
//        Assert.Equal("Topic", topic.Title);
//        Assert.Equal("Topic description", topic.Description);
//        Assert.Equal("Body", topic.Body);
//        Assert.Equal("Label", topic.CalloutLabel);
//        Assert.Equal("Callout", topic.CalloutText);
//        Assert.Null(topic.Quote);
//        Assert.Null(topic.QuoteEn);
//        Assert.Equal("Topic EN", topic.TitleEn);
//        Assert.Equal("Description EN", topic.DescriptionEn);
//        Assert.Equal("Body EN", topic.BodyEn);
//        Assert.Equal("Label EN", topic.CalloutLabelEn);
//        Assert.Equal("Callout EN", topic.CalloutTextEn);
//    }

//    [Fact]
//    public void Create_GivenNoSubjects_ThrowsDomainException()
//    {
//        // Arrange
//        List<NewSubject> subjects = [];

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(false)]
//    [InlineData(true)]
//    public void Create_GivenMissingTopics_ThrowsDomainException(bool useNull)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        List<NewTopic> topics = [];
//        if (useNull)
//        { topics = null!; }
//        subjects[0] = subjects[0] with { Topics = topics };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankName_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        name = invalidValue!;

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankSubjectTitle_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0] = subjects[0] with { Title = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankSubjectDescription_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0] = subjects[0] with { Description = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankTopicTitle_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { Title = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankTopicDescription_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { Description = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankBody_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { Body = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankCalloutLabel_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { CalloutLabel = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankCalloutText_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var name = "Exhibition";
//        var subjects = CreateSubjects();
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { CalloutText = invalidValue! };

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create(name, "Exhibition EN", new ImageId(Guid.NewGuid()), subjects));
//    }

//    [Fact]
//    public void Create_GivenMultipleSubjectsAndTopics_PreservesContentOrderAndQuote()
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        subjects[0].Topics.Add(new NewTopic("Second topic", "Second topic EN", "Description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Text", "Text EN", "A quote", "A quote EN"));
//        subjects.Add(new NewSubject("Second subject", "Second subject EN", "second-subject", "Description", "Description EN",
//            [new NewTopic("Third topic", "Third topic EN", "Description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Text", "Text EN", null, null)]));

//        // Act
//        var exhibition = Exhibition.Create("Exhibition", "Exhibition EN", new ImageId(Guid.NewGuid()), subjects);

//        // Assert
//        Assert.Equal(2, exhibition.Subjects.Count);
//        Assert.Equal("Subject", exhibition.Subjects[0].Title);
//        Assert.Equal("Second subject", exhibition.Subjects[1].Title);
//        Assert.Equal(2, exhibition.Subjects[0].Topics.Count);
//        Assert.Equal("Topic", exhibition.Subjects[0].Topics[0].Title);
//        Assert.Equal("Second topic", exhibition.Subjects[0].Topics[1].Title);
//        Assert.Equal("A quote", exhibition.Subjects[0].Topics[1].Quote);
//        Assert.Equal("A quote EN", exhibition.Subjects[0].Topics[1].QuoteEn);
//        Assert.Equal("Third topic", Assert.Single(exhibition.Subjects[1].Topics).Title);
//        Assert.NotEqual(exhibition.Subjects[0].Id, exhibition.Subjects[1].Id);
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankNameEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = invalidValue!;
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankSubjectTitleEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0] = subjects[0] with { TitleEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankSubjectDescriptionEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0] = subjects[0] with { DescriptionEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankSubjectSlug_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0] = subjects[0] with { Slug = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankTopicTitleEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { TitleEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankTopicDescriptionEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { DescriptionEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankBodyEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { BodyEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankCalloutLabelEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { CalloutLabelEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankCalloutTextEn_ThrowsDomainException(string? invalidValue)
//    {
//        // Arrange
//        var subjects = CreateSubjects();
//        var nameEn = "Exhibition EN";
//        subjects[0].Topics[0] = subjects[0].Topics[0] with { CalloutTextEn = invalidValue! };
//        var imageId = new ImageId(Guid.NewGuid());

//        // Act & Assert
//        Assert.Throws<DomainException>(() => Exhibition.Create("Exhibition", nameEn, imageId, subjects));
//    }
//}

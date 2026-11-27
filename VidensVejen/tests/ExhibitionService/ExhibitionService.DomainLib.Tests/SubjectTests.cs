using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Tests;

public class SubjectTests
{
    [Fact]
    public void Create_GivenQuizSubjectWithoutTopics_PreservesReference()
    {
        // Arrange
        var quizId = Guid.NewGuid();

        // Act
        var exhibition = Exhibition.Create(new ExhibitionId(Guid.NewGuid()), "Name", "Name EN", new ImageId(Guid.NewGuid()),
            [new NewSubject("Quiz", "Quiz EN", "quiz", "Description", "Description EN", quizId, [])]);

        // Assert
        var subject = Assert.Single(exhibition.Subjects);
        Assert.Equal(quizId, subject.QuizId?.Value);
        Assert.Empty(subject.Topics);
        Assert.Equal(0, subject.Position);
    }

    [Fact]
    public void Create_GivenMultipleSubjects_AssignsPositionsInInputOrder()
    {
        // Arrange
        var quizId = Guid.NewGuid();
        List<NewSubject> subjects =
        [
            new NewSubject("First", "First EN", "first", "Description", "Description EN", quizId, []),
            new NewSubject("Second", "Second EN", "second", "Description", "Description EN", quizId, [])
        ];

        // Act
        var exhibition = Exhibition.Create(new ExhibitionId(Guid.NewGuid()), "Name", "Name EN", new ImageId(Guid.NewGuid()), subjects);

        // Assert
        Assert.Equal("First", exhibition.Subjects[0].Title);
        Assert.Equal(0, exhibition.Subjects[0].Position);
        Assert.Equal("Second", exhibition.Subjects[1].Title);
        Assert.Equal(1, exhibition.Subjects[1].Position);
        Assert.NotEqual(exhibition.Subjects[0].Id, exhibition.Subjects[1].Id);
    }
}

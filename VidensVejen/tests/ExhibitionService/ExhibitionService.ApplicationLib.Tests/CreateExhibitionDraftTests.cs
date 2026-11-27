using ExhibitionService.ApplicationLib.Handlers;
using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Tests;


public class CreateExhibitionDraftTests
{
    [Fact]
    public async Task Handle_CreatesEmptyDraftAndReturnsItsId()
    {
        // Arrange
        var drafts = new Mock<IExhibitionDraftRepository>();
        var exhibitions = new Mock<IExhibitionRepository>();
        ExhibitionDraft? saved = null;
        drafts.Setup(r => r.AddAsync(It.IsAny<ExhibitionDraft>()))
            .Callback<ExhibitionDraft>(draft => saved = draft).Returns(Task.CompletedTask);
        ICreateExhibitionDraftHandler handler = new CreateExhibitionDraftHandler(drafts.Object, exhibitions.Object);

        // Act
        var id = await handler.HandleAsync();

        // Assert
        Assert.NotNull(saved);
        Assert.Equal(saved.Id.Value, id);
        Assert.Null(saved.SourceExhibitionId);
        Assert.Empty(saved.SubjectDrafts);
        drafts.Verify(r => r.SaveAsync(), Times.Once);
        exhibitions.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateRevision_GivenPublishedExhibition_CopiesCommandAndLinksSource()
    {
        // Arrange
        var exhibition = ExhibitionTestData.Published();
        var command = ExhibitionTestData.DraftContent(exhibition.Id.Value);
        var drafts = new Mock<IExhibitionDraftRepository>();
        var exhibitions = new Mock<IExhibitionRepository>();
        exhibitions.Setup(r => r.GetExhibitionById(exhibition.Id)).ReturnsAsync(exhibition);
        ExhibitionDraft? saved = null;
        drafts.Setup(r => r.AddAsync(It.IsAny<ExhibitionDraft>()))
            .Callback<ExhibitionDraft>(draft => saved = draft).Returns(Task.CompletedTask);
        ICreateExhibitionDraftHandler handler = new CreateExhibitionDraftHandler(drafts.Object, exhibitions.Object);

        // Act
        var id = await handler.CreateRevisionAsync(command);

        // Assert
        Assert.NotNull(saved);
        Assert.Equal(saved.Id.Value, id);
        Assert.Equal(exhibition.Id, saved.SourceExhibitionId);
        Assert.Equal(command.ExhibtionName, saved.Name);
        Assert.Equal(command.ExhibitionNameEn, saved.NameEn);
        Assert.Equal(command.ThemeImageId, saved.ThemeImageId?.Value);
        var subject = Assert.Single(saved.SubjectDrafts);
        Assert.Equal(command.SubjectDrafts[0].QuizId, subject.QuizId?.Value);
        Assert.Equal(command.SubjectDrafts[0].QuizDraftId, subject.QuizDraftId?.Value);
        Assert.True(subject.IsQuiz);
        Assert.Equal("Quote EN", Assert.Single(subject.TopicDrafts).QuoteEn);
        drafts.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateRevision_GivenUnknownSource_ThrowsWithoutSaving()
    {
        // Arrange
        var drafts = new Mock<IExhibitionDraftRepository>();
        var exhibitions = new Mock<IExhibitionRepository>();
        ICreateExhibitionDraftHandler handler = new CreateExhibitionDraftHandler(drafts.Object, exhibitions.Object);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.CreateRevisionAsync(ExhibitionTestData.DraftContent(Guid.NewGuid())));

        // Assert
        drafts.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenAddFailure_DoesNotSave()
    {
        // Arrange
        var drafts = new Mock<IExhibitionDraftRepository>();
        var expected = new InvalidOperationException("Add failed");
        drafts.Setup(r => r.AddAsync(It.IsAny<ExhibitionDraft>())).ThrowsAsync(expected);
        ICreateExhibitionDraftHandler handler = new CreateExhibitionDraftHandler(drafts.Object, new Mock<IExhibitionRepository>().Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(handler.HandleAsync);

        // Assert
        Assert.Same(expected, actual);
        drafts.Verify(r => r.SaveAsync(), Times.Never);
    }
}

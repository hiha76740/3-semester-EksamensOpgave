//using ExhibitionService.ApplicationLib.Handlers;
//using ExhibitionService.ApplicationLib.Repositories;
//using ExhibitionService.ApplicationLib.Services;
//using ExhibitionService.DomainLib.Entities;
//using ExhibitionService.DomainLib.ValueObjects;
//using ExhibitionService.FacadeLib.Commands.Interfaces;
//using Microsoft.Extensions.Logging.Abstractions;
//using Moq;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace ExhibitionService.ApplicationLib.Tests;


//public class PublishExhibitionRevisionTests
//{
//    [Fact]
//    public async Task PublishRevision_GivenLinkedDraft_UpdatesOriginalAndDeletesDraft()
//    {
//        // Arrange
//        var original = ExhibitionTestData.Published();
//        var draft = ExhibitionDraft.CreateRevision(original.Id);
//        var command = ExhibitionTestData.Publication(Guid.NewGuid());
//        var exhibitions = new Mock<IExhibitionRepository>();
//        exhibitions.Setup(r => r.GetExhibitionById(original.Id)).ReturnsAsync(original);
//        var drafts = new Mock<IExhibitionDraftRepository>();
//        drafts.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
//        var images = new Mock<IImageService>();
//        images.Setup(s => s.ExistsAsync(new ImageId(command.ThemeImageId))).ReturnsAsync(true);
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(exhibitions.Object, drafts.Object, images.Object, NullLogger<CreateExhibitionHandler>.Instance);
//        var originalId = original.Id;

//        // Act
//        await handler.PublishRevisionAsync(draft.Id.Value, command);

//        // Assert
//        Assert.Equal(originalId, original.Id);
//        Assert.Equal(command.ExhibtionName, original.Name);
//        Assert.Equal(command.ExhibitionNameEn, original.NameEn);
//        Assert.Equal(command.ThemeImageId, original.ThemeImageId.Value);
//        Assert.Equal(command.Subjects[0].QuizId, Assert.Single(original.Subjects).QuizId?.Value);
//        drafts.Verify(r => r.Delete(draft), Times.Once);
//        exhibitions.Verify(r => r.AddAsync(It.IsAny<Exhibition>()), Times.Never);
//        exhibitions.Verify(r => r.SaveAsync(), Times.Once);
//        drafts.Verify(r => r.SaveAsync(), Times.Never);
//    }

//    [Fact]
//    public async Task PublishRevision_GivenUnknownDraft_ThrowsWithoutWriting()
//    {
//        // Arrange
//        var exhibitions = new Mock<IExhibitionRepository>();
//        var drafts = new Mock<IExhibitionDraftRepository>();
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(exhibitions.Object, drafts.Object, new Mock<IImageService>().Object, NullLogger<CreateExhibitionHandler>.Instance);

//        // Act
//        await Assert.ThrowsAsync<NotFoundException>(() => handler.PublishRevisionAsync(Guid.NewGuid(), ExhibitionTestData.Publication(Guid.NewGuid())));

//        // Assert
//        exhibitions.VerifyNoOtherCalls();
//        drafts.Verify(r => r.Delete(It.IsAny<ExhibitionDraft>()), Times.Never);
//    }

//    [Fact]
//    public async Task PublishRevision_GivenUnlinkedDraft_ThrowsWithoutWriting()
//    {
//        // Arrange
//        var exhibitions = new Mock<IExhibitionRepository>();
//        var draft = ExhibitionDraft.Create();
//        var drafts = new Mock<IExhibitionDraftRepository>();
//        drafts.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(exhibitions.Object, drafts.Object, new Mock<IImageService>().Object, NullLogger<CreateExhibitionHandler>.Instance);

//        // Act
//        await Assert.ThrowsAsync<DomainException>(() => handler.PublishRevisionAsync(draft.Id.Value, ExhibitionTestData.Publication(Guid.NewGuid())));

//        // Assert
//        exhibitions.VerifyNoOtherCalls();
//        drafts.Verify(r => r.Delete(draft), Times.Never);
//    }
//}

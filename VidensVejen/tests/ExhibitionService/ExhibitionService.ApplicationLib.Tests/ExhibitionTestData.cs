using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.ApplicationLib.Tests;

internal static class ExhibitionTestData
{
    internal static Exhibition Published()
    {
        return Exhibition.Create(new ExhibitionId(Guid.NewGuid()), "Old", "Old EN", new ImageId(Guid.NewGuid()),
            [new NewSubject("Quiz", "Quiz EN", "quiz", "Description", "Description EN", Guid.NewGuid(), [])]);
    }

    internal static CreateExhibitionCommand Publication(Guid id)
    {
        return new CreateExhibitionCommand(id, "New", "New EN", Guid.NewGuid(),
            [new CreateSubjectCommand("Quiz", "Quiz EN", "quiz", "Description", "Description EN", Guid.NewGuid(), [])], []);
    }

    internal static UpdateExhibitionDraftCommand DraftContent(Guid id)
    {
        return new UpdateExhibitionDraftCommand(id, "Draft", "Draft EN", Guid.NewGuid(),
            [new UpdateSubjectDraftCommand(Guid.NewGuid(), "Quiz", "Quiz EN", "quiz", "Description", "Description EN",
                Guid.NewGuid(), Guid.NewGuid(), true,
                [new UpdateTopicDraftCommand("Topic", "Topic EN", "Description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Text", "Text EN", "Quote", "Quote EN")])], [1, 2, 3]);
    }
}

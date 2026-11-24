using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.Interfaces;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="draftRepo"></param>
/// <param name="quizRepo"></param>
public class PublishQuizHandler(IQuizDraftRepository draftRepo, IQuizRepository quizRepo) : IPublishQuizHandler
{
    async Task<Guid> IPublishQuizHandler.HandleAsync(Guid draftId)
    {
        var draft = await draftRepo.GetDraftByIdAsync(new QuizDraftId(draftId))
            ?? throw new NotFoundException("Quiz draft was not found");

        var quiz = draft.Publish();

        await quizRepo.AddAsync(quiz);

        draftRepo.Delete(draft);

        await draftRepo.SaveAsync();

        return quiz.Id.Value;
    }
}

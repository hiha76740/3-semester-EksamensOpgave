using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.Interfaces;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="quizRepo"></param>
public class PublishQuizHandler(IQuizRepository quizRepo) : IPublishQuizHandler
{
    async Task IPublishQuizHandler.HandleAsync(Guid id)
    {
        var quiz = await quizRepo.GetQuizByIdAsync(new QuizId(id))
            ?? throw new NotFoundException("Quiz was not found");

        quiz.Publish();

        await quizRepo.SaveAsync();
    }
}

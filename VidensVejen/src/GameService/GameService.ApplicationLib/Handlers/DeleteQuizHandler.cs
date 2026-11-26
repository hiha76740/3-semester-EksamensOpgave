using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="repo"></param>
/// <param name="logger"></param>
public class DeleteQuizHandler(
    IQuizRepository repo,
    //IExhibitionService exhibitionService,
    ILogger<DeleteQuizHandler> logger
    ) : IDeleteQuizHandler
{
    async Task IDeleteQuizHandler.HandleAsync(DeleteQuizCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning deletion of draft {DraftId}", command.DraftId);

            var quizId = new QuizId(command.DraftId);

            // TODO: ud kommenteret til vi finder ud af hvordan quiz er tilknyttet til en udstilling
            //var inUse = await exhibitionService.QuizInUseAsync(quizId);

            //if (inUse == true)
            //    throw new InvalidOperationException("Deletion aborted, quiz is in use");

            var quiz = await repo.GetQuizByIdAsync(quizId)
                ?? throw new NotFoundException("Quiz was not found");

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting quiz {QuizId} from auth database", quiz.Id.Value);

            repo.SetOriginalRowVersion(quiz, command.RowVersion);


            repo.Delete(quiz);

            await repo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz {Id} was deleted from auth database successfully", quiz.Id.Value);

        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            throw;
        }
    }
}

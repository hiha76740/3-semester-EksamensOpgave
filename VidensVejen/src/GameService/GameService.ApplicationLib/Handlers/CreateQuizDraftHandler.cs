using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="draftRepo"></param>
/// <param name="logger"></param>
public class CreateQuizDraftHandler(
    IQuizDraftRepository draftRepo,
    ILogger<CreateQuizDraftHandler> logger
    ) : ICreateQuizDraftHandler
{
    async Task<Guid> ICreateQuizDraftHandler.HandleAsync()
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to create a quiz draft");

            var draft = QuizDraft.Create();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz draft created with id: {QuizDraftId}", draft.Id.Value);

            await draftRepo.AddAsync(draft);
            await draftRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz draft {Id} saved in database successfully", draft.Id.Value);

            return draft.Id.Value;
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

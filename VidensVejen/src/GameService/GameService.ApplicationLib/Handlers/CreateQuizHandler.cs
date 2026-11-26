using System.Data;
using GameService.ApplicationLib.Mappers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.Entities.Quizzes;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="draftRepo"></param>
/// <param name="quizRepo"></param>
/// <param name="unitOfWork"></param>
/// <param name="logger"></param>
public class CreateQuizHandler(
    IQuizDraftRepository draftRepo,
    IQuizRepository quizRepo,
    IUnitOfWork unitOfWork,
    ILogger<CreateQuizHandler> logger
    ) : ICreateQuizHandler
{
    async Task ICreateQuizHandler.HandleAsync(CreateQuizCommand command)
    {
        unitOfWork.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to create quiz from draft {DraftId}", command.DraftId);

            var id = new QuizDraftId(command.DraftId);

            var draft = await draftRepo.GetDraftByIdAsync(id)
                ?? throw new NotFoundException("Quiz draft was not found");

            var quiz = Quiz.Create(
                command.Title,
                command.TitleEn,
                command.ExhibitionId,
                command.QuizLayout,
                command.EndMessage,
                command.EndMessageEn,
                command.Questions.AsNewQuestions(),
                command.Instruction,
                command.InstructionEn);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz created with id: {QuizId}", quiz.Id.Value);

            await quizRepo.AddAsync(quiz);

            draftRepo.SetOriginalRowVersion(draft, command.DraftRowVersion);

            draftRepo.Delete(draft);

            await unitOfWork.SaveChangesAsync();
            unitOfWork.Commit();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz {QuizId} saved in database and draft {DraftId} deleted from database", quiz.Id.Value, command.DraftId);
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            unitOfWork.Rollback();
            throw;
        }
    }
}

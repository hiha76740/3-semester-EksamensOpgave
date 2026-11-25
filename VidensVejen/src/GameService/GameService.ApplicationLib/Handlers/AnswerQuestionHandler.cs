using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Sessions;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="sessionRepo"></param>
/// <param name="quizRepo"></param>
public class AnswerQuestionHandler(IGameSessionRepository sessionRepo, IQuizRepository quizRepo) : IAnswerQuestionHandler
{
    async Task<AnswerResultDto> IAnswerQuestionHandler.HandleAsync(AnswerQuestionCommand command)
    {
        var session = await sessionRepo.GetSessionByIdAsync(new GameSessionId(command.SessionId))
            ?? throw new NotFoundException("Session was not found");

        var quiz = await quizRepo.GetQuizByIdAsync(session.QuizId)
            ?? throw new NotFoundException("Quiz was not found");

        var result = session.Answer(quiz, command.QuestionId, command.ChoiceIndex);

        await sessionRepo.SaveAsync();

        return new AnswerResultDto(result.IsCorrect, result.Feedback, result.CorrectAnswer);
    }
}

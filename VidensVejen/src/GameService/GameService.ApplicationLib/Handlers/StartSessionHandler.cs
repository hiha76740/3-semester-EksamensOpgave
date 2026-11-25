using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.Entities.Sessions;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="quizRepo"></param>
/// <param name="sessionRepo"></param>
public class StartSessionHandler(IQuizRepository quizRepo, IGameSessionRepository sessionRepo) : IStartSessionHandler
{
    async Task<StartSessionDto> IStartSessionHandler.HandleAsync(Guid quizId)
    {
        var quiz = await quizRepo.GetQuizByIdAsync(new QuizId(quizId))
            ?? throw new NotFoundException("Quiz was not found");

        var session = GameSession.Start(quiz);

        await sessionRepo.AddAsync(session);
        await sessionRepo.SaveAsync();

        var questions = new List<PlayQuestionDto>();

        foreach (var question in quiz.Questions)
        {
            var choices = new List<PlayChoiceDto>();

            for (var i = 0; i < question.Choices.Count; i++)
            {
                choices.Add(new PlayChoiceDto(i, question.Choices[i].Text));
            }

            questions.Add(new PlayQuestionDto(question.Id.Value, question.Text, choices));
        }

        var playQuiz = new PlayQuizDto(quiz.Id.Value, quiz.Title, quiz.QuizLayout, quiz.EndMessage, questions);

        return new StartSessionDto(session.Id.Value, playQuiz);
    }
}

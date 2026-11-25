using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.Entities.Sessions;
using Microsoft.EntityFrameworkCore;

namespace GameService.InfrastructureLib.Persistence;

public class GameDbContext : DbContext
{
    public DbSet<QuizDraft> QuizDrafts { get; set; }
    public DbSet<QuestionDraft> DraftQuestions { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<GameSession> GameSessions { get; set; }

    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GameDbContext).Assembly);
    }
}

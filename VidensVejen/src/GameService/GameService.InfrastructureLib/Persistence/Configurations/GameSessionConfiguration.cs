using GameService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class GameSessionConfiguration : IEntityTypeConfiguration<GameSession>
{
    void IEntityTypeConfiguration<GameSession>.Configure(EntityTypeBuilder<GameSession> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new GameSessionId(value));

        builder.Property(x => x.QuizId)
            .HasConversion(
            id => id.Value,
            value => new QuizId(value))
            .IsRequired();

        builder.HasIndex(x => x.QuizId);

        builder.OwnsMany(x => x.Answers, answers => answers.ToJson());
    }
}

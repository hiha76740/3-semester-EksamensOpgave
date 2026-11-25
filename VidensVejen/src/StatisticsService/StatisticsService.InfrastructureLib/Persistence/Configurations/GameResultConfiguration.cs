using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StatisticsService.DomainLib.Entities;

namespace StatisticsService.InfrastructureLib.Persistence.Configurations;

internal sealed class GameResultConfiguration : IEntityTypeConfiguration<GameResult>
{
    void IEntityTypeConfiguration<GameResult>.Configure(EntityTypeBuilder<GameResult> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new GameResultId(value));

        builder.Property(x => x.QuizTitle)
            .HasMaxLength(500)
            .IsRequired();
    }
}

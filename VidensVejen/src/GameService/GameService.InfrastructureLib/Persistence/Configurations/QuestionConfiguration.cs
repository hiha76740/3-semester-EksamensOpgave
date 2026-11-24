using GameService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    void IEntityTypeConfiguration<Question>.Configure(EntityTypeBuilder<Question> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new QuestionId(value));

        builder.Property<QuizId>("QuizId")
            .HasConversion(
            id => id.Value,
            value => new QuizId(value));

        builder.Property(x => x.Text)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.TextEn)
            .HasMaxLength(500);

        builder.OwnsMany(x => x.Choices, choices => choices.ToJson());
    }
}

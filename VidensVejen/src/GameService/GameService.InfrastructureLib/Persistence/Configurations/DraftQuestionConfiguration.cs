using GameService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class DraftQuestionConfiguration : IEntityTypeConfiguration<DraftQuestion>
{
    void IEntityTypeConfiguration<DraftQuestion>.Configure(EntityTypeBuilder<DraftQuestion> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new QuestionId(value));

        builder.Property<QuizDraftId>("QuizDraftId")
            .HasConversion(
            id => id.Value,
            value => new QuizDraftId(value));

        builder.Property(x => x.Text)
            .HasMaxLength(500);

        builder.Property(x => x.CorrectFeedback)
            .HasMaxLength(500);

        builder.Property(x => x.IncorrectFeedback)
            .HasMaxLength(500);

        builder.OwnsMany(x => x.Choices, choices => choices.ToJson());
    }
}

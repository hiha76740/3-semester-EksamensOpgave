using GameService.DomainLib.Entities.Drafts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class DraftQuestionConfiguration : IEntityTypeConfiguration<QuestionDraft>
{
    void IEntityTypeConfiguration<QuestionDraft>.Configure(EntityTypeBuilder<QuestionDraft> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new QuestionDraftId(value));

        builder.Property<QuizDraftId>("QuizDraftId")
            .HasConversion(
            id => id.Value,
            value => new QuizDraftId(value));

        builder.Property(x => x.Text)
            .HasMaxLength(500);

        builder.Property(x => x.TextEn)
            .HasMaxLength(500);

        builder.OwnsMany(x => x.Choices, choices => choices.ToJson());
    }
}

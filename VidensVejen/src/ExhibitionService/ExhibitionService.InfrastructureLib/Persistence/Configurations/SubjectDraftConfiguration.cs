using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExhibitionService.InfrastructureLib.Persistence.Configurations;

internal sealed class SubjectDraftConfiguration : IEntityTypeConfiguration<SubjectDraft>
{
    void IEntityTypeConfiguration<SubjectDraft>.Configure(EntityTypeBuilder<SubjectDraft> builder)
    {
        builder.HasKey(sd => sd.Id);

        builder.Property(sd => sd.Id)
            .HasConversion(
                id => id.Value,
                value => new SubjectDraftId(value));

        builder.Property(sd => sd.QuizId)
            .HasConversion(
                id => id!.Value,
                value => new QuizId(value));

        builder.Property(sd => sd.QuizDraftId)
            .HasConversion(
                id => id!.Value,
                value => new QuizDraftId(value));

        builder.Property(sd => sd.Title)
            .HasMaxLength(200);

        builder.Property(sd => sd.TitleEn)
            .HasMaxLength(200);

        builder.Property(sd => sd.Description)
            .HasMaxLength(700);

        builder.Property(sd => sd.DescriptionEn)
            .HasMaxLength(700);

        builder.Property(sd => sd.IsQuiz)
            .IsRequired();

        builder.Property(sd => sd.Position)
            .IsRequired();

        builder.OwnsMany(sd => sd.TopicDrafts, td => td.ToJson());
    }
}

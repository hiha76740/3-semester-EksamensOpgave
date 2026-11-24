using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class QuizDraftConfiguration : IEntityTypeConfiguration<QuizDraft>
{
    void IEntityTypeConfiguration<QuizDraft>.Configure(EntityTypeBuilder<QuizDraft> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new QuizDraftId(value));

        builder.Property(x => x.ExhibitionId)
            .HasConversion(
            id => id!.Value,
            value => new ExhibitionId(value))
            .IsRequired(false);

        builder.Property(x => x.Title)
            .HasMaxLength(500);

        builder.Property(x => x.TitleEn)
            .HasMaxLength(500);

        builder.Property(x => x.QuizLayout)
            .HasMaxLength(500);

        builder.Property(x => x.EndMessage)
            .HasMaxLength(500);

        builder.Property(x => x.EndMessageEn)
            .HasMaxLength(500);

        builder.HasMany(x => x.Questions)
            .WithOne()
            .HasForeignKey("QuizDraftId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Questions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

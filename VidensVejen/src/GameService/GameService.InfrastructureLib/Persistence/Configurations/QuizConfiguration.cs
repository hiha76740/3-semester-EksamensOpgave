using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameService.InfrastructureLib.Persistence.Configurations;

internal sealed class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    void IEntityTypeConfiguration<Quiz>.Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new QuizId(value));

        builder.Property(x => x.ExhibitionId)
            .HasConversion(
            id => id!.Value,
            value => new ExhibitionId(value))
            .IsRequired(false);

        builder.Property(x => x.Title)
            .HasMaxLength(500);

        builder.Property(x => x.QuizLayout)
            .HasMaxLength(500);

        builder.Property(x => x.EndMessage)
            .HasMaxLength(500);

        builder.HasIndex(x => x.ExhibitionId);

        builder.HasMany(x => x.Questions)
            .WithOne()
            .HasForeignKey("QuizId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Questions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

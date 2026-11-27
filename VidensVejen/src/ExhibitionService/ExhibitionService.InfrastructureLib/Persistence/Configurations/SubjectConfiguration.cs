using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExhibitionService.InfrastructureLib.Persistence.Configurations;

internal sealed class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    void IEntityTypeConfiguration<Subject>.Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => new SubjectId(value));

        builder.Property(s => s.QuizId)
            .HasConversion(
                id => id!.Value,
                value => new QuizId(value));

        builder.Property(s => s.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.TitleEn)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(s => s.Slug)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(700)
            .IsRequired();

        builder.Property(s => s.DescriptionEn)
            .HasMaxLength(700)
            .IsRequired();

        builder.OwnsMany(s => s.Topics, t => t.ToJson());
    }
}

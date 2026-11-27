using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExhibitionService.InfrastructureLib.Persistence.Configurations;

internal sealed class ExhibitionDraftConfiguration : IEntityTypeConfiguration<ExhibitionDraft>
{
    void IEntityTypeConfiguration<ExhibitionDraft>.Configure(EntityTypeBuilder<ExhibitionDraft> builder)
    {
        builder.HasKey(ed => ed.Id);

        builder.Property(ed => ed.Id)
            .HasConversion(
            id => id.Value,
            value => new ExhibitionDraftId(value));

        builder.Property(ed => ed.SourceExhibitionId)
            .HasConversion(
            id => id!.Value,
            value => new ExhibitionId(value));

        builder.Property(ed => ed.Name)
            .HasMaxLength(200);

        builder.Property(ed => ed.ThemeImageId)
            .HasConversion(
            id => id!.Value,
            value => new ImageId(value))
            .IsRequired(false);

        builder.HasMany(ed => ed.SubjectDrafts)
            .WithOne()
            .HasForeignKey("ExhibitionDraftId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}

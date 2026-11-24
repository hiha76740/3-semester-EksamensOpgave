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

        builder.Property(sd => sd.Title)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(s => s.Description)
            .HasMaxLength(700)
            .IsRequired(false);

        builder.OwnsMany(sd => sd.TopicDrafts, td => td.ToJson());
    }
}

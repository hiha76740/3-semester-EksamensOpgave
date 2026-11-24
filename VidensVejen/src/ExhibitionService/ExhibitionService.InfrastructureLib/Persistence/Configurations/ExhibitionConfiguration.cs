using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExhibitionService.InfrastructureLib.Persistence.Configurations;

internal sealed class ExhibitionConfiguration : IEntityTypeConfiguration<Exhibition>
{
    void IEntityTypeConfiguration<Exhibition>.Configure(EntityTypeBuilder<Exhibition> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasConversion(
            id => id.Value,
            value => new ExhibitionId(value));

        builder.Property(e => e.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.ThemeImageId)
            .HasConversion(
            id => id.Value,
            value => new ImageId(value));

        builder.HasMany(e => e.Subjects)
            .WithOne()
            .HasForeignKey("ExhibitionId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}

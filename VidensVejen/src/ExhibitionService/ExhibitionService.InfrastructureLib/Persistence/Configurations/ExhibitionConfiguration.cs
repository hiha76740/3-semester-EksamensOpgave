using ExhibitionService.DomainLib.Entities;
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
    }
}

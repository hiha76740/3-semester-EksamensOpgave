using ImageService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImageService.InfrastructureLib.Persistence.Configurations;

internal sealed class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    void IEntityTypeConfiguration<Image>.Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new ImageId(value));
    }
}

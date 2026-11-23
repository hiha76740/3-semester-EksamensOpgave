using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.DomainLib.Entities;
using UserService.DomainLib.ValueObjects;

namespace UserService.InfrastructureLib.Persistence.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    void IEntityTypeConfiguration<User>.Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new UserId(value));

        builder.Property(x => x.EmailAddress)
        .HasConversion(
            email => email.Value,
            value => new Email(value))
        .HasMaxLength(254)
        .IsRequired();

        builder.HasIndex(x => x.EmailAddress)
            .IsUnique();

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}

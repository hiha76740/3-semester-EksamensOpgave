using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.InfrastructureLib.Persistence.Configurations;

internal sealed class AuthUserConfiguration : IEntityTypeConfiguration<AuthUser>
{
    void IEntityTypeConfiguration<AuthUser>.Configure(EntityTypeBuilder<AuthUser> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
            id => id.Value,
            value => new AuthUserId(value));

        builder.Property(x => x.Status)
            .HasConversion<string>();

        builder.Property(x => x.Email)
        .HasConversion(
            email => email.Value,
            value => new Email(value))
        .HasMaxLength(254)
        .IsRequired();

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity(p => p.ToTable("AuthUserAccessPermissions"));

        builder.Property(x => x.RowVersion)
            .IsRowVersion();
    }
}

using AuthService.ApplicationLib.Authentication;
using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;
using AuthService.InfrastructureLib.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VidensVejen.ContractsLib;

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

    internal static void SeedAdmin(AuthUserDbContext context, string email, string? password)
    {
        var id = new AuthUserId(BootstrapAdmin.Id);
        var admin = context.AuthUsers
            .Include(user => user.Permissions)
            .SingleOrDefault(user => user.Id == id);
        var permissions = context.Permissions.ToList();

        if (admin == null)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "BootstrapAdmin:Password is required when creating the administrator.");
            }

            IPasswordHashService passwordHasher = new PasswordHashService();
            admin = AuthUser.Create(email, passwordHasher.Hash(password), permissions, id);
            admin.SetStatusActive();
            context.AuthUsers.Add(admin);
        }
        else
        {
            foreach (var permission in permissions.Except(admin.Permissions))
            {
                admin.AddPermission(permission);
            }
        }

        context.SaveChanges();
    }
}

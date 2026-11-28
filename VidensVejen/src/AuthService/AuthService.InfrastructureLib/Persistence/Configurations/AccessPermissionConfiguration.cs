using AuthService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.InfrastructureLib.Persistence.Configurations;

internal sealed class AccessPermissionConfiguration : IEntityTypeConfiguration<AccessPermission>
{
    void IEntityTypeConfiguration<AccessPermission>.Configure(EntityTypeBuilder<AccessPermission> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(f => f.Id)
            .HasConversion(
            id => id.Value,
            value => new AccessPermissionId(value)
            );

        builder.HasData(
            new { Id = new AccessPermissionId(Guid.Parse("68a14b26-15d6-4831-a876-221b01260c6b")), Name = "User.Create" },
            new { Id = new AccessPermissionId(Guid.Parse("389ee76d-5478-43a4-a5f9-94a9f8133eb9")), Name = "User.Edit" },
            new { Id = new AccessPermissionId(Guid.Parse("ab46c03f-4440-4ef9-8137-9dc09f82b94c")), Name = "User.Delete" },
            new { Id = new AccessPermissionId(Guid.Parse("1eff118c-87f2-45f6-8f1e-2a4ce1f1ccff")), Name = "User.Read" },
            new { Id = new AccessPermissionId(Guid.Parse("f595c2a6-13b0-438c-bc63-c08fff41ddc4")), Name = "Exhibition.Publish" },
            new { Id = new AccessPermissionId(Guid.Parse("1d4d28b2-edc2-4aaa-be9e-e855f4dd7cd3")), Name = "Exhibition.Edit" },
            new { Id = new AccessPermissionId(Guid.Parse("0fb02b10-29ea-4616-83d8-5e7870e24c3f")), Name = "Exhibition.Delete" },
            new { Id = new AccessPermissionId(Guid.Parse("420dedfe-0bed-40ab-b036-63fbe52fbbab")), Name = "Exhibition.Read" },
            new { Id = new AccessPermissionId(Guid.Parse("35ae0b5a-36cf-4242-a7f3-f10a616e6626")), Name = "Game.Publish" },
            new { Id = new AccessPermissionId(Guid.Parse("a5d70bf0-67d1-4a14-bf0b-2857d3307e11")), Name = "Game.Edit" },
            new { Id = new AccessPermissionId(Guid.Parse("577779d0-f015-4827-8535-0318210db489")), Name = "Game.Delete" },
            new { Id = new AccessPermissionId(Guid.Parse("efcc418e-1113-4d82-b67e-3f6efc06765d")), Name = "Game.Read" },
            new { Id = new AccessPermissionId(Guid.Parse("e48eb056-2c92-4642-90a9-dd685edb7222")), Name = "Image.Upload" },
            new { Id = new AccessPermissionId(Guid.Parse("f5ad63ca-85e0-40f8-9315-f6f7cd088408")), Name = "Image.Delete" },
            new { Id = new AccessPermissionId(Guid.Parse("a6ae8565-c1ee-4fc9-bcba-050dbb28e3b7")), Name = "Image.Read" }
            );
    }
}

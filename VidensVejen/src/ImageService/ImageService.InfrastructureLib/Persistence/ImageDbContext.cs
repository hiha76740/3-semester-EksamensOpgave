using ImageService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;

namespace ImageService.InfrastructureLib.Persistence;

public class ImageDbContext : DbContext
{
    public DbSet<Image> Images { get; set; }

    public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ImageDbContext).Assembly);
    }
}

using ExhibitionService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionService.InfrastructureLib.Persistence;

public class ExhibitionDbContext : DbContext
{
    public DbSet<Exhibition> Exhibitions { get; set; }
    public DbSet<Subject> Subjects { get; set; }

    public ExhibitionDbContext(DbContextOptions<ExhibitionDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExhibitionDbContext).Assembly);
    }
}

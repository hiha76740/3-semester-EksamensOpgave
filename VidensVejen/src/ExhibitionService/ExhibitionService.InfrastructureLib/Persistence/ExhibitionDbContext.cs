using ExhibitionService.DomainLib.Entities;
using Microsoft.EntityFrameworkCore;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;

namespace ExhibitionService.InfrastructureLib.Persistence;

public class ExhibitionDbContext : DbContext
{
    public DbSet<Exhibition> Exhibitions { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<ExhibitionDraft> ExhibitionDrafts { get; set; }
    public DbSet<SubjectDraft> SubjectDrafts { get; set; }
    public DbSet<OutboxMessage> Outbox { get; set; }


    public ExhibitionDbContext(DbContextOptions<ExhibitionDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ExhibitionDbContext).Assembly);
    }
}

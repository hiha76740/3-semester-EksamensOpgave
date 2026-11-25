using Microsoft.EntityFrameworkCore;
using StatisticsService.DomainLib.Entities;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;

namespace StatisticsService.InfrastructureLib.Persistence;

/// <summary>
/// 
/// </summary>
public class StatisticsDbContext : DbContext
{
    /// <summary>
    /// 
    /// </summary>
    public DbSet<GameResult> GameResults { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public DbSet<InboxMessage> Inbox { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) : base(options) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StatisticsDbContext).Assembly);
    }
}

using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using StatisticsService.InfrastructureLib.Persistence;
using VidensVejen.BuildingBlocksLib.UnitOfWork;

namespace StatisticsService.InfrastructureLib.UnitOfWorks;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class EFUnitOfWork(StatisticsDbContext db) : IUnitOfWork
{
    private IDbContextTransaction _transaction = null!;

    void IUnitOfWork.BeginTransaction(IsolationLevel isolationLevel)
    {
        if (db.Database.CurrentTransaction != null)
            throw new InvalidOperationException("Transaction could not start");

        _transaction = db.Database.BeginTransaction(isolationLevel);
    }

    void IUnitOfWork.Commit()
    {
        _transaction.Commit();
        _transaction.Dispose();
    }

    void IUnitOfWork.Rollback()
    {
        _transaction.Rollback();
        _transaction.Dispose();
    }

    async Task IUnitOfWork.SaveChangesAsync()
    {
        await db.SaveChangesAsync();
    }
}

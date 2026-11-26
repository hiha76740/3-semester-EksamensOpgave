using System.Data;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.InfrastructureLib.UnitOfWorks;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class EFUnitOfWork(GameDbContext db) : IUnitOfWork
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
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyConflictException("The data was changed or deleted by another request", ex);
        }
    }
}

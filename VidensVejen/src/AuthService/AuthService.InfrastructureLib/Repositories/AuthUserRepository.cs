using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;
using AuthService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class AuthUserRepository(AuthUserDbContext db) : IAuthUserRepository
{
    void IAuthUserRepository.Delete(AuthUser authUser)
    {
        db.AuthUsers.Remove(authUser);
    }

    async Task<bool> IAuthUserRepository.EmailExistAsync(Email email)
    {
        return await db.AuthUsers.AnyAsync(e => e.Email == email);
    }

    async Task<AuthUser?> IAuthUserRepository.GetAuthUserByEmailAsync(Email email)
    {
        return await db.AuthUsers
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(e => e.Email == email);
    }

    async Task<AuthUser?> IAuthUserRepository.GetAuthUserByIdAsync(AuthUserId id)
    {
        return await db.AuthUsers
            .Include(u => u.Permissions)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    async Task IAuthUserRepository.RegisterAsync(AuthUser authUser)
    {
        await db.AuthUsers.AddAsync(authUser);
    }

    async Task IAuthUserRepository.SaveAsync()
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

    void IAuthUserRepository.SetOriginalRowVersion(AuthUser authUser, byte[] originalRowVersion)
    {
        db.Entry(authUser)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }
}

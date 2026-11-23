using Microsoft.EntityFrameworkCore;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using UserService.InfrastructureLib.Persistence;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class UserRepository(UserDbContext db) : IUserRepository
{
    async Task IUserRepository.AddAsync(User user)
    {
        await db.Users.AddAsync(user);
    }

    void IUserRepository.Delete(User user)
    {
        db.Users.Remove(user);
    }

    async Task<User?> IUserRepository.GetUserByIdAsync(UserId userId)
    {
        return await db.Users.FindAsync(userId);
    }

    async Task IUserRepository.SaveAsync()
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

    void IUserRepository.SetOriginalRowVersion(User user, byte[] originalRowVersion)
    {
        db.Entry(user)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }

    async Task<bool> IUserRepository.UserExistsAsync(UserId id)
    {
        return await db.Users.AnyAsync(u => u.Id == id);
    }
}

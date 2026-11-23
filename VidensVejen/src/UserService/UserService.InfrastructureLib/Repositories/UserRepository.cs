using Microsoft.EntityFrameworkCore;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using UserService.InfrastructureLib.Persistence;

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
        await db.SaveChangesAsync();
    }

    async Task<bool> IUserRepository.UserExistsAsync(UserId id)
    {
        return await db.Users.AnyAsync(u => u.Id == id);
    }
}

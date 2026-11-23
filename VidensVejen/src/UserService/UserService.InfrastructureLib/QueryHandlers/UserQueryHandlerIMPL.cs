using Microsoft.EntityFrameworkCore;
using UserService.DomainLib.Entities;
using UserService.FacadeLib.Queries.DTOs;
using UserService.FacadeLib.Queries.Interfaces;
using UserService.InfrastructureLib.Persistence;

namespace UserService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class UserQueryHandlerIMPL(UserDbContext db) : IUserQueries
{
    async Task<IReadOnlyList<UserDto>> IUserQueries.GetAllUsersAsync()
    {
        return await db.Users
            .AsNoTracking()
            .Select(u => new UserDto(
                u.Id.Value,
                u.Firstname,
                u.Surname,
                u.EmailAddress.Value,
                u.RowVersion))
            .ToListAsync();
    }

    async Task<UserDto?> IUserQueries.GetUserByIdAsync(Guid id)
    {
        var userKey = new UserId(id);

        return await db.Users
            .AsNoTracking()
            .Where(u => u.Id == userKey)
            .Select(u => new UserDto(
                u.Id.Value,
                u.Firstname,
                u.Surname,
                u.EmailAddress.Value,
                u.RowVersion))
            .FirstOrDefaultAsync();
    }
}

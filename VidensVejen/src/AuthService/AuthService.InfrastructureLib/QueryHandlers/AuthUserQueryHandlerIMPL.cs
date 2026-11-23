using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Queries.DTOs;
using AuthService.FacadeLib.Queries.Interfaces;
using AuthService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AuthService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class AuthUserQueryHandlerIMPL(AuthUserDbContext db) : IAuthUsersQueries
{
    async Task<AuthUserDto?> IAuthUsersQueries.GetByIdAsync(Guid id)
    {
        var authUserKey = new AuthUserId(id);

        return await db.AuthUsers
            .AsNoTracking()
            .Where(u => u.Id == authUserKey)
            .Select(
            u => new AuthUserDto(
                u.Id.Value,
                u.Email.Value,
                u.Permissions.Select(p => new AccessPermissionDto(p.Id.Value, p.Name)).ToList(),
                u.RowVersion))
            .FirstOrDefaultAsync();
    }

    async Task<AuthUserDto?> IAuthUsersQueries.GetCurrentUserInfo(Guid id)
    {
        var authUserId = new AuthUserId(id);

        return await db.AuthUsers
            .AsNoTracking()
            .Where(u => u.Id == authUserId)
            .Select(u => new AuthUserDto(
                u.Id.Value,
                u.Email.Value,
                u.Permissions
                .Select(
                    p => new AccessPermissionDto(
                        p.Id.Value,
                        p.Name))
                .ToList(),
                u.RowVersion))
            .FirstOrDefaultAsync();

    }
}

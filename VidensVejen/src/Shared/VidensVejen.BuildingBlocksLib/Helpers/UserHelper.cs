using System.Security.Claims;

namespace VidensVejen.BuildingBlocksLib.Helpers;

/// <summary>
/// 
/// </summary>
public static class UserHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public static Guid? GetCurrentUserId(ClaimsPrincipal user)
    {
        var stringUserId = user.FindFirstValue(ClaimTypes.NameIdentifier);

        var idIsValid = Guid.TryParse(stringUserId, out var id);

        if (idIsValid == false)
            return null;

        return id;
    }
}

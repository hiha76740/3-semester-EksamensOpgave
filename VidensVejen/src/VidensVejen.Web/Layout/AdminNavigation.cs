namespace VidensVejen.Web.Layout;

internal sealed record AdminNavigationItem(string Title, string Icon, string? Path);

internal static class AdminNavigation
{
    internal static IReadOnlyList<AdminNavigationItem> Items { get; } =
    [
        new("Forside", "home", ""),
        new("Udstillinger", "layers", "exhibition/create"),
        new("Quizzer", "quiz", "games"),
        new("Mediebibliotek", "perm_media", "images"),
        new("Brugere", "group", "users")
    ];

    internal static string? GetSectionTitle(string relativeUri)
    {
        var path = relativeUri.Split('?', '#')[0].Trim('/');
        var item = Items
            .Where(item => item.Path is string route && Matches(path, route.Trim('/')))
            .MaxBy(item => item.Path?.Length ?? 0);

        return item?.Title ?? (path.ToLowerInvariant() switch
        {
            "login" => "Log ind",
            "user/profile" => "Min profil",
            _ => null
        });
    }

    private static bool Matches(string path, string route)
    {
        return path.Equals(route, StringComparison.OrdinalIgnoreCase)
            || (route.Length > 0 && path.StartsWith(route + "/", StringComparison.OrdinalIgnoreCase));
    }
}

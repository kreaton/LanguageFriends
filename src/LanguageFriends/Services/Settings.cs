using Microsoft.JSInterop;

namespace LanguageFriends.Services;

/// <summary>
/// What is remembered in the browser (localStorage): the two languages, the level, the effect sounds,
/// and the best number of stars for each language pair, game and level.
/// </summary>
public sealed class Settings(IJSRuntime js)
{
    private const string Prefix = "languagefriends.";

    public async Task<string?> GetAsync(string key)
    {
        try { return await js.InvokeAsync<string?>("friends.load", Prefix + key); }
        catch (JSException) { return null; }
    }

    public async Task SetAsync(string key, string value)
    {
        try { await js.InvokeVoidAsync("friends.save", Prefix + key, value); }
        catch (JSException) { }
    }

    public static string StarsKey(string from, string to, GameMode mode, int level) => $"stars.{from}.{to}.{mode}.{level}";

    public async Task<int> GetStarsAsync(string from, string to, GameMode mode, int level) =>
        int.TryParse(await GetAsync(StarsKey(from, to, mode, level)), out var stars) ? stars : 0;

    /// <summary>Keeps the best result. Returns true when this is a new best.</summary>
    public async Task<bool> SaveStarsAsync(string from, string to, GameMode mode, int level, int stars)
    {
        if (stars <= await GetStarsAsync(from, to, mode, level)) return false;
        await SetAsync(StarsKey(from, to, mode, level), stars.ToString(System.Globalization.CultureInfo.InvariantCulture));
        return true;
    }
}

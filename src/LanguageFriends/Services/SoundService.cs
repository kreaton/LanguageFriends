using Microsoft.JSInterop;

namespace LanguageFriends.Services;

/// <summary>
/// Speaks letters, numbers and words (wwwroot/audio, made with edge-tts) and plays the small success and
/// "try again" sounds, which are made with Web Audio in js/friends.js. The speech is the game, so it always
/// plays; the 🔔 button only turns the little effect sounds on and off.
/// </summary>
public sealed class SoundService(IJSRuntime js)
{
    public bool Effects { get; set; } = true;

    /// <summary>Plays the file and returns when it has finished (or failed, or was cut off by another sound).</summary>
    public async Task SayAsync(string src)
    {
        try { await js.InvokeVoidAsync("friends.play", src); }
        catch (JSException) { }
        catch (TaskCanceledException) { }
    }

    public async Task SuccessAsync()
    {
        if (!Effects) return;
        try { await js.InvokeVoidAsync("friends.success"); }
        catch (JSException) { }
    }

    public async Task FailAsync()
    {
        if (!Effects) return;
        try { await js.InvokeVoidAsync("friends.fail"); }
        catch (JSException) { }
    }

    public async Task FanfareAsync()
    {
        if (!Effects) return;
        try { await js.InvokeVoidAsync("friends.fanfare"); }
        catch (JSException) { }
    }

    public async Task StopAsync()
    {
        try { await js.InvokeVoidAsync("friends.stop"); }
        catch (JSException) { }
    }
}

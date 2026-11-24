using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace LanguageFriends.Services;

/// <summary>
/// Letters and numbers of one language, from data/friends.json. <see cref="NumbersCsv"/> holds the CSV text of
/// the number words that build_data.py changed (NUMBER_FIXES), keyed by the number.
/// </summary>
public sealed record LanguageData(
    string Code, List<string> Alphabet, List<string> Numbers, Dictionary<string, string>? NumbersCsv = null);

/// <summary>
/// A picture word: the emoji, its number in dictionary.csv ("001") and the word in every language.
/// <see cref="Skip"/> lists the languages it is left out of (SKIP_WORDS in tools/build_data.py), usually
/// because the recording is wrong; the text and the recording are kept so they can be checked.
/// </summary>
public sealed record Word(string N, string Emoji, Dictionary<string, string> Text, List<string>? Skip = null)
{
    public bool IsSkipped(string code) => Skip?.Contains(code) == true;
}

public sealed record FriendsData(List<LanguageData> Languages, List<Word> Words);

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, PropertyNameCaseInsensitive = true)]
[JsonSerializable(typeof(FriendsData))]
public partial class AppJsonContext : JsonSerializerContext;

/// <summary>
/// Loads data/friends.json (written by tools/build_data.py from the Audio project) and knows where every sound is:
/// audio/&lt;language&gt;/alphabet/A01.mp3 (the letters in alphabet order), numbers/N01.mp3 (1–10) and
/// dictionary/D001.mp3 (the picture words).
/// </summary>
public sealed class GameData(HttpClient http)
{
    private Dictionary<string, LanguageData> _languages = [];

    public IReadOnlyList<Word> Words { get; private set; } = [];

    public bool Loaded { get; private set; }

    public async Task LoadAsync()
    {
        if (Loaded) return;
        var data = await http.GetFromJsonAsync("data/friends.json", AppJsonContext.Default.FriendsData)
                   ?? throw new InvalidOperationException("data/friends.json is empty");
        _languages = data.Languages.ToDictionary(l => l.Code);
        Words = data.Words;
        Loaded = true;
    }

    public LanguageData this[string code] => _languages[code];

    public static string LetterAudio(string code, int index) => $"audio/{code}/alphabet/A{index + 1:00}.mp3";

    public static string NumberAudio(string code, int number) => $"audio/{code}/numbers/N{number:00}.mp3";

    public static string WordAudio(string code, Word word) => $"audio/{code}/dictionary/D{word.N}.mp3";
}

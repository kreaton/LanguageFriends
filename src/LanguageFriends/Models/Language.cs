namespace LanguageFriends.Models;

/// <summary>
/// One language of the game: its kid (name, look, what they say) and the game's texts in that language.
/// The letters, numbers and words come from data/friends.json (see <see cref="Services.GameData"/>).
/// </summary>
public sealed record Language(
    string Code,
    string NativeName,
    string KidName,
    string Hello,
    string Praise,
    string TryAgain,
    KidLook Look,
    UiText Ui)
{
    public bool Rtl => Code.StartsWith("ar", StringComparison.Ordinal);

    public string Dir => Rtl ? "rtl" : "ltr";

    /// <summary>The language part of the code, for the HTML lang attribute ("da", "uk", "ar").</summary>
    public string Lang => Code[..2];

    /// <summary>Lower case of a letter. Turkish has dotted/dotless i, which the invariant culture gets wrong.</summary>
    public string Lower(string letter) => (Code, letter) switch
    {
        ("tr-TR", "I") => "ı",
        ("tr-TR", "İ") => "i",
        _ when Rtl => "",
        _ => letter.ToLowerInvariant(),
    };
}

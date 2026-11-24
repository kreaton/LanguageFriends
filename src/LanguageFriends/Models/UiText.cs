namespace LanguageFriends.Models;

/// <summary>The game's own texts in one language. The game speaks to the child in the language they chose as their own.</summary>
public sealed record UiText(
    string LearnWhich,
    string Letters,
    string Words,
    string Numbers,
    string Level1,
    string Level2,
    string Level3,
    string HearAgain,
    string MyLanguage,
    string PlayAgain,
    string Voices,
    string NotInMine);

using LanguageFriends.Models;

namespace LanguageFriends.Services;

public enum GameMode { Letters, Words, Numbers }

/// <summary>A tile the child can tap: a letter, a picture or a number. <see cref="Sub"/> is the small text under it.</summary>
public sealed record Choice(string Key, string Display, string? Sub = null);

/// <summary>
/// One question: what is said in the language being learned (<see cref="TargetAudio"/>), the same thing in the
/// child's own language for help (<see cref="OwnAudio"/>, null when there is none, e.g. a letter their alphabet
/// doesn't have), the written forms, and the tiles to choose from.
/// </summary>
public sealed record Question(
    string AnswerKey,
    string TargetText,
    string? OwnText,
    string TargetAudio,
    string? OwnAudio,
    IReadOnlyList<Choice> Choices);

/// <summary>
/// The rules of a round of <see cref="Length"/> questions.
/// <list type="bullet">
/// <item>Level 1 "see and hear": the question is written in both languages (letters: the letter is shown);
///   letters and numbers in order; 3 pictures to choose from.</item>
/// <item>Level 2 "read and hear": written only in the language being learned (letters: not written);
///   6 pictures.</item>
/// <item>Level 3 "just listen": nothing written; letters and numbers shuffled; 9 pictures.</item>
/// </list>
/// The answer is always written in both languages once it has been found.
/// </summary>
public sealed class Round
{
    public const int Length = 10;

    private readonly List<Question> _questions;

    public GameMode Mode { get; }
    public int Level { get; }
    public Language From { get; }
    public Language To { get; }

    public int Index { get; private set; }
    public int Score { get; private set; }

    /// <summary>Wrong taps on the current question.</summary>
    public int Misses { get; private set; }

    public HashSet<string> Wrong { get; } = [];

    public bool Solved { get; private set; }

    public Question Current => _questions[Index];
    public bool IsLast => Index == _questions.Count - 1;
    public int Count => _questions.Count;

    /// <summary>The question shows its text in the language being learned.</summary>
    public bool ShowTarget => Level == 1 || (Level == 2 && Mode != GameMode.Letters);

    /// <summary>The question also shows the text in the child's own language.</summary>
    public bool ShowOwn => Level == 1 && Mode != GameMode.Letters;

    /// <summary>After two misses the right tile starts to glow.</summary>
    public bool ShowHint => Misses >= 2;

    public Round(GameData data, GameMode mode, int level, Language from, Language to, Random? random = null)
    {
        var rnd = random ?? Random.Shared;
        Mode = mode;
        Level = Math.Clamp(level, 1, 3);
        From = from;
        To = to;
        _questions = mode switch
        {
            GameMode.Letters => LetterQuestions(data, rnd),
            GameMode.Numbers => NumberQuestions(data, rnd),
            _ => WordQuestions(data, rnd),
        };
    }

    /// <summary>Records a tap. Returns true when it is the right tile.</summary>
    public bool Answer(string key)
    {
        if (Solved || Wrong.Contains(key)) return false;
        if (key == Current.AnswerKey)
        {
            Solved = true;
            if (Misses == 0) Score++;
            return true;
        }
        Wrong.Add(key);
        Misses++;
        return false;
    }

    public void Next()
    {
        if (IsLast) return;
        Index++;
        Misses = 0;
        Solved = false;
        Wrong.Clear();
    }

    /// <summary>1–3 stars: everything right first time is 3, at least 7 is 2, and finishing always gives 1.</summary>
    public int Stars => Score >= Count ? 3 : Score >= 7 ? 2 : 1;

    private List<Question> LetterQuestions(GameData data, Random rnd)
    {
        var target = data[To.Code].Alphabet;
        var own = data[From.Code].Alphabet;
        var tiles = target.Select(l => new Choice(l, l, Level == 1 ? To.Lower(l) : null)).ToList();
        if (Level == 3) Shuffle(tiles, rnd);

        var picks = Enumerable.Range(0, target.Count).ToList();
        Shuffle(picks, rnd);
        return picks.Take(Length).Select(i =>
        {
            var letter = target[i];
            var ownIndex = own.IndexOf(letter);
            return new Question(
                letter, letter, ownIndex >= 0 ? letter : null,
                GameData.LetterAudio(To.Code, i),
                ownIndex >= 0 ? GameData.LetterAudio(From.Code, ownIndex) : null,
                tiles);
        }).ToList();
    }

    private List<Question> NumberQuestions(GameData data, Random rnd)
    {
        var target = data[To.Code].Numbers;
        var own = data[From.Code].Numbers;
        var tiles = Enumerable.Range(1, target.Count)
            .Select(n => new Choice(n.ToString(), n.ToString(), Level == 1 ? target[n - 1] : null))
            .ToList();
        if (Level == 3) Shuffle(tiles, rnd);

        var picks = Enumerable.Range(1, target.Count).ToList();
        Shuffle(picks, rnd);
        return picks.Take(Length).Select(n => new Question(
            n.ToString(), target[n - 1], own[n - 1],
            GameData.NumberAudio(To.Code, n), GameData.NumberAudio(From.Code, n),
            tiles)).ToList();
    }

    private List<Question> WordQuestions(GameData data, Random rnd)
    {
        var words = data.Words.ToList();
        var choices = Level switch { 1 => 3, 2 => 6, _ => 9 };
        var picks = words.ToList();
        Shuffle(picks, rnd);
        return picks.Take(Length).Select(word =>
        {
            var others = words.Where(w => w != word).ToList();
            Shuffle(others, rnd);
            var tiles = others.Take(choices - 1).Append(word)
                .Select(w => new Choice(w.N, w.Emoji))
                .ToList();
            Shuffle(tiles, rnd);
            return new Question(
                word.N, word.Text[To.Code], word.Text[From.Code],
                GameData.WordAudio(To.Code, word), GameData.WordAudio(From.Code, word),
                tiles);
        }).ToList();
    }

    private static void Shuffle<T>(IList<T> list, Random rnd)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = rnd.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

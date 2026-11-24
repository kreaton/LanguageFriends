namespace LanguageFriends.Models;

public enum HairStyle { Short, Spiky, Curly, CurlyLong, Bob, Long, Pigtails, Ponytail, Braid }

public enum Accessory { None, Glasses, Bow, Beanie, Cap, FlowerCrown, Headband, Flower, HairClip }

/// <summary>How a kid looks and feels right now. Drives the face and the CSS animation.</summary>
public enum Mood { Idle, Talk, Happy, Oops }

/// <summary>The look of one cartoon kid. Colours are CSS colours.</summary>
public sealed record KidLook(
    string Skin,
    string Hair,
    HairStyle Style,
    string Shirt,
    string Trim,
    Accessory Accessory = Accessory.None,
    string AccessoryColor = "#e03131",
    bool Freckles = false,
    bool Stripes = false);

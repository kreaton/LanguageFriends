namespace LanguageFriends.Models;

/// <summary>
/// The 16 languages and their kids. Danish comes first, since helping children learn Danish is the main idea;
/// the rest follow speech_voices.csv. The texts were written by hand and should be checked by native speakers.
/// </summary>
public static class Languages
{
    private static readonly UiText Arabic = new(
        "أي لغة تريد أن تتعلم؟", "الحروف", "الكلمات", "الأرقام",
        "انظر واستمع", "اقرأ واستمع", "استمع فقط",
        "استمع مرة أخرى", "بلغتي", "العب مرة أخرى",
        "الأصوات من صنع الحاسوب (تحويل النص إلى كلام من Microsoft).",
        "هذا الحرف ليس في أبجديتي.");

    public static IReadOnlyList<Language> All { get; } =
    [
        new("da-DK", "Dansk", "Freja", "Hej!", "Flot!", "Prøv igen!",
            new("#f9dcc8", "#f2c14e", HairStyle.Pigtails, "#e03131", "#ffffff", Accessory.None, Freckles: true),
            new("Hvilket sprog vil du lære?", "Bogstaver", "Ord", "Tal",
                "Se og hør", "Læs og hør", "Bare lyt",
                "Hør igen", "På mit sprog", "Spil igen",
                "Stemmerne er lavet af en computer (Microsoft tekst-til-tale).",
                "Mit alfabet har ikke det bogstav.")),

        new("en-GB", "English", "Oliver", "Hello!", "Well done!", "Try again!",
            new("#fbe3d3", "#d9622b", HairStyle.Short, "#364fc7", "#fcc419", Accessory.None, Freckles: true),
            new("Which language do you want to learn?", "Letters", "Words", "Numbers",
                "See and hear", "Read and hear", "Just listen",
                "Hear it again", "In my language", "Play again",
                "The voices are made by a computer (Microsoft text-to-speech).",
                "My alphabet doesn't have this letter.")),

        new("sv-SE", "Svenska", "Elias", "Hej!", "Bra jobbat!", "Försök igen!",
            new("#f5d6c1", "#c4955a", HairStyle.Spiky, "#fab005", "#1c7ed6"),
            new("Vilket språk vill du lära dig?", "Bokstäver", "Ord", "Tal",
                "Se och lyssna", "Läs och lyssna", "Bara lyssna",
                "Lyssna igen", "På mitt språk", "Spela igen",
                "Rösterna är gjorda av en dator (Microsoft text-till-tal).",
                "Mitt alfabet har inte den bokstaven.")),

        new("nb-NO", "Norsk", "Nora", "Hei!", "Bra!", "Prøv igjen!",
            new("#f7d8c5", "#8d5a3b", HairStyle.Long, "#2f9e44", "#ffffff", Accessory.Beanie, "#e03131"),
            new("Hvilket språk vil du lære?", "Bokstaver", "Ord", "Tall",
                "Se og hør", "Les og hør", "Bare lytt",
                "Hør igjen", "På mitt språk", "Spill igjen",
                "Stemmene er laget av en datamaskin (Microsoft tekst-til-tale).",
                "Alfabetet mitt har ikke den bokstaven.")),

        new("fr-FR", "Français", "Léa", "Salut !", "Bravo !", "Essaie encore !",
            new("#efc3a4", "#4a2c1d", HairStyle.Bob, "#ffffff", "#1c3f94", Accessory.Bow, "#e03131", Stripes: true),
            new("Quelle langue veux-tu apprendre ?", "Lettres", "Mots", "Nombres",
                "Regarde et écoute", "Lis et écoute", "Écoute seulement",
                "Réécouter", "Dans ma langue", "Rejouer",
                "Les voix sont faites par un ordinateur (synthèse vocale Microsoft).",
                "Mon alphabet n'a pas cette lettre.")),

        new("es-ES", "Español", "Mateo", "¡Hola!", "¡Muy bien!", "¡Inténtalo otra vez!",
            new("#e3b08a", "#2b1b12", HairStyle.Curly, "#fd7e14", "#ffe066"),
            new("¿Qué idioma quieres aprender?", "Letras", "Palabras", "Números",
                "Mira y escucha", "Lee y escucha", "Solo escucha",
                "Escuchar otra vez", "En mi idioma", "Jugar otra vez",
                "Las voces las hace un ordenador (texto a voz de Microsoft).",
                "Mi alfabeto no tiene esta letra.")),

        new("it-IT", "Italiano", "Giulia", "Ciao!", "Bravo!", "Riprova!",
            new("#ebbd98", "#3b2416", HairStyle.Long, "#12b886", "#ffffff", Accessory.Headband, "#fcc419"),
            new("Quale lingua vuoi imparare?", "Lettere", "Parole", "Numeri",
                "Guarda e ascolta", "Leggi e ascolta", "Solo ascolto",
                "Ascolta di nuovo", "Nella mia lingua", "Gioca ancora",
                "Le voci sono fatte da un computer (sintesi vocale Microsoft).",
                "Il mio alfabeto non ha questa lettera.")),

        new("nl-NL", "Nederlands", "Daan", "Hoi!", "Goed zo!", "Probeer het nog eens!",
            new("#f9dfcc", "#ffd43b", HairStyle.Short, "#ff922b", "#ffffff", Accessory.Cap, "#1c7ed6"),
            new("Welke taal wil je leren?", "Letters", "Woorden", "Getallen",
                "Kijk en luister", "Lees en luister", "Alleen luisteren",
                "Nog een keer horen", "In mijn taal", "Opnieuw spelen",
                "De stemmen zijn door een computer gemaakt (Microsoft tekst-naar-spraak).",
                "Mijn alfabet heeft deze letter niet.")),

        new("de-DE", "Deutsch", "Mia", "Hallo!", "Super!", "Versuch es noch mal!",
            new("#f6cfb7", "#a0522d", HairStyle.Ponytail, "#7048e8", "#ffffff", Accessory.HairClip, "#ff6b6b"),
            new("Welche Sprache möchtest du lernen?", "Buchstaben", "Wörter", "Zahlen",
                "Sehen und hören", "Lesen und hören", "Nur zuhören",
                "Nochmal hören", "In meiner Sprache", "Nochmal spielen",
                "Die Stimmen macht ein Computer (Microsoft Text-to-Speech).",
                "Mein Alphabet hat diesen Buchstaben nicht.")),

        new("pl-PL", "Polski", "Kuba", "Cześć!", "Brawo!", "Spróbuj jeszcze raz!",
            new("#f8d7c2", "#e9c46a", HairStyle.Spiky, "#15aabf", "#ffffff"),
            new("Jakiego języka chcesz się uczyć?", "Litery", "Słowa", "Liczby",
                "Patrz i słuchaj", "Czytaj i słuchaj", "Tylko słuchaj",
                "Posłuchaj jeszcze raz", "W moim języku", "Zagraj jeszcze raz",
                "Głosy są tworzone przez komputer (syntezator mowy Microsoft).",
                "W moim alfabecie nie ma tej litery.")),

        new("uk-UA", "Українська", "Софійка", "Привіт!", "Молодець!", "Спробуй ще раз!",
            new("#f9dbc8", "#f0cf85", HairStyle.Braid, "#339af0", "#ffd43b", Accessory.FlowerCrown),
            new("Яку мову ти хочеш вивчати?", "Літери", "Слова", "Числа",
                "Дивись і слухай", "Читай і слухай", "Тільки слухай",
                "Послухати ще раз", "Моєю мовою", "Грати ще раз",
                "Голоси створив комп’ютер (синтез мовлення Microsoft).",
                "У моєму алфавіті немає цієї літери.")),

        new("ro-RO", "Română", "Andrei", "Salut!", "Bravo!", "Mai încearcă!",
            new("#edc3a2", "#3d2b1f", HairStyle.Short, "#1971c2", "#ffd43b", Accessory.Glasses, "#343a40"),
            new("Ce limbă vrei să înveți?", "Litere", "Cuvinte", "Numere",
                "Privește și ascultă", "Citește și ascultă", "Doar ascultă",
                "Ascultă din nou", "În limba mea", "Joacă din nou",
                "Vocile sunt făcute de un computer (sinteză vocală Microsoft).",
                "Alfabetul meu nu are această literă.")),

        new("bg-BG", "Български", "Мила", "Здравей!", "Браво!", "Опитай пак!",
            new("#ebc1a1", "#2d1e16", HairStyle.Ponytail, "#e64980", "#ffffff", Accessory.Flower, "#ffffff"),
            new("Кой език искаш да учиш?", "Букви", "Думи", "Числа",
                "Гледай и слушай", "Чети и слушай", "Само слушай",
                "Чуй отново", "На моя език", "Играй отново",
                "Гласовете са направени от компютър (синтез на реч Microsoft).",
                "В моята азбука няма тази буква.")),

        new("tr-TR", "Türkçe", "Emir", "Merhaba!", "Aferin!", "Tekrar dene!",
            new("#dba47c", "#1f1410", HairStyle.Short, "#0ca678", "#ffffff"),
            new("Hangi dili öğrenmek istiyorsun?", "Harfler", "Kelimeler", "Sayılar",
                "Bak ve dinle", "Oku ve dinle", "Sadece dinle",
                "Tekrar dinle", "Benim dilimde", "Tekrar oyna",
                "Sesler bir bilgisayar tarafından yapıldı (Microsoft metin okuma).",
                "Alfabemde bu harf yok.")),

        new("ar-LB", "العربية · لبنان", "ليلى", "مرحبا!", "أحسنت!", "حاول مرة أخرى!",
            new("#d49c73", "#2a1a12", HairStyle.CurlyLong, "#f783ac", "#ffffff", Accessory.Flower, "#ffd43b"),
            Arabic),

        new("ar-SA", "العربية الفصحى", "عمر", "مرحبا!", "أحسنت!", "حاول مرة أخرى!",
            new("#b97a50", "#140d09", HairStyle.Curly, "#94d82d", "#ffffff"),
            Arabic),
    ];

    public static Language? Find(string? code) => All.FirstOrDefault(l => l.Code == code);
}

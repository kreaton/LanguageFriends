"""Builds Language Friends's game data from the Audio project.

Reads alphabets.csv, dictionary.csv, numbertext_selected.csv and speech_voices.csv, checks that every
letter, word and number has its MP3 in outputaudio/, then writes

    src/LanguageFriends/wwwroot/data/friends.json
    src/LanguageFriends/wwwroot/audio/<language>/{alphabet,dictionary,numbers}/*.mp3   (copied)

Run from the repository root:

    python -X utf8 tools/build_data.py ..\\..\\..\\Audio

The audio is copied only when it is missing or has changed, so running it again is cheap.
"""

import csv
import filecmp
import json
import shutil
import sys
from pathlib import Path

REPO = Path(__file__).resolve().parent.parent
WWWROOT = REPO / "src" / "LanguageFriends" / "wwwroot"


# The number sounds are the voice reading the digits "1" … "10", so the written word must be what the
# voice says. CLDR (numbertext_selected.csv) has the counting form "et" for Danish 1, but the Danish voice
# reads "1" as "en", which is also what most Danes say when counting.
NUMBER_FIXES = {("da-DK", 1): "en"}


def read_csv(path: Path) -> list[dict[str, str]]:
    with path.open(encoding="utf-8-sig", newline="") as f:
        return list(csv.DictReader(f))


def main(audio_root: Path) -> None:
    voices = read_csv(audio_root / "speech_voices.csv")
    languages = [v["language"] for v in voices]
    alphabets = {r["language"]: r["alphabet"] for r in read_csv(audio_root / "alphabets.csv")}
    numbers: dict[str, list[str]] = {}
    for r in read_csv(audio_root / "numbertext_selected.csv"):
        numbers.setdefault(r["language"], []).append(r["text"])
    dictionary = read_csv(audio_root / "dictionary.csv")

    out_audio = WWWROOT / "audio"
    missing: list[str] = []
    copied = 0

    def need(lang: str, category: str, name: str) -> None:
        nonlocal copied
        src = audio_root / "outputaudio" / lang / category / f"{name}_female.mp3"
        dst = out_audio / lang / category / f"{name}.mp3"
        if not src.is_file():
            missing.append(str(src))
            return
        if dst.is_file() and filecmp.cmp(src, dst, shallow=False):
            return
        dst.parent.mkdir(parents=True, exist_ok=True)
        shutil.copyfile(src, dst)
        copied += 1

    data_languages = []
    for lang in languages:
        letters = list(alphabets[lang])
        nums = [NUMBER_FIXES.get((lang, i + 1), text) for i, text in enumerate(numbers[lang])]
        if len(nums) != 10:
            sys.exit(f"{lang}: expected the numbers 1-10, found {len(nums)}")
        for i in range(len(letters)):
            need(lang, "alphabet", f"A{i + 1:02}")
        for i in range(len(nums)):
            need(lang, "numbers", f"N{i + 1:02}")
        for row in dictionary:
            need(lang, "dictionary", f"D{row['number']}")
        data_languages.append({"code": lang, "alphabet": letters, "numbers": nums})

    words = [
        {"n": row["number"], "emoji": row["emoji"], "text": {lang: row[lang] for lang in languages}}
        for row in dictionary
    ]

    if missing:
        sys.exit("Missing audio:\n  " + "\n  ".join(missing))

    data_dir = WWWROOT / "data"
    data_dir.mkdir(parents=True, exist_ok=True)
    with (data_dir / "friends.json").open("w", encoding="utf-8", newline="\n") as f:
        json.dump({"languages": data_languages, "words": words}, f, ensure_ascii=False, separators=(",", ":"))
        f.write("\n")

    print(f"{len(data_languages)} languages, {len(words)} words; {copied} audio files copied")


if __name__ == "__main__":
    if len(sys.argv) != 2:
        sys.exit(__doc__)
    main(Path(sys.argv[1]).resolve())

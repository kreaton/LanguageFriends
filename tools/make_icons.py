"""Renders art/icon.svg to the app icons in src/LanguageFriends/wwwroot/icons/.

Microsoft Edge (headless) draws the SVG at 1024 px and Pillow scales it down. The maskable icon has a
square background and the content shrunk to 80 % so it stays inside the safe zone.

    python -X utf8 tools/make_icons.py [--front-page ..\\KidsGames\\billeder]

--front-page also copies the 512 px icon there as languagefriends.png, for the KidsGames front page.
"""

import argparse
import shutil
import subprocess
import tempfile
from pathlib import Path

from PIL import Image

REPO = Path(__file__).resolve().parent.parent
SVG = REPO / "art" / "icon.svg"
OUT = REPO / "src" / "LanguageFriends" / "wwwroot" / "icons"
EDGE_PATHS = [
    Path(r"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe"),
    Path(r"C:\Program Files\Microsoft\Edge\Application\msedge.exe"),
]


def render(svg_text: str, png: Path, size: int = 1024) -> None:
    edge = next((p for p in EDGE_PATHS if p.is_file()), None)
    if edge is None:
        raise SystemExit("Microsoft Edge was not found")
    with tempfile.TemporaryDirectory() as tmp:
        html = Path(tmp) / "icon.html"
        html.write_text(
            "<!doctype html><meta charset='utf-8'>"
            "<link href='https://fonts.googleapis.com/css2?family=Fredoka:wght@700&display=block' rel='stylesheet'>"
            f"<style>html,body{{margin:0;background:transparent}}svg{{display:block;width:{size}px;height:{size}px}}</style>"
            + svg_text,
            encoding="utf-8",
        )
        subprocess.run(
            [str(edge), "--headless=new", "--disable-gpu", "--hide-scrollbars",
             "--default-background-color=00000000", "--virtual-time-budget=3000",
             f"--user-data-dir={Path(tmp) / 'profile'}",
             f"--window-size={size},{size}", f"--screenshot={png}", html.as_uri()],
            check=True, capture_output=True,
        )


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--front-page", type=Path)
    args = parser.parse_args()

    svg = SVG.read_text(encoding="utf-8")
    # Maskable: a full square background, the content scaled to 80 % around the centre.
    maskable = (svg.replace('rx="112"', 'rx="0"')
                   .replace('<g id="content">', '<g id="content" transform="translate(51.2 51.2) scale(0.8)">'))

    OUT.mkdir(parents=True, exist_ok=True)
    with tempfile.TemporaryDirectory() as tmp:
        big = Path(tmp) / "any.png"
        big_mask = Path(tmp) / "maskable.png"
        render(svg, big)
        render(maskable, big_mask)
        any_img = Image.open(big).convert("RGBA")
        mask_img = Image.open(big_mask).convert("RGBA")
        for name, size in {"icon-1024.png": 1024, "icon-512.png": 512, "icon-192.png": 192, "favicon-32.png": 32}.items():
            any_img.resize((size, size), Image.LANCZOS).save(OUT / name)
        mask_img.resize((512, 512), Image.LANCZOS).save(OUT / "icon-512-maskable.png")
        # iOS rounds the corners itself and wants a square, opaque icon.
        apple = mask_img.resize((180, 180), Image.LANCZOS)
        opaque = Image.new("RGB", apple.size, "#4dabf7")
        opaque.paste(apple, mask=apple.split()[3])
        opaque.save(OUT / "apple-touch-icon.png")

    if args.front_page:
        shutil.copyfile(OUT / "icon-512.png", args.front_page / "languagefriends.png")
    print("Icons written to", OUT)


if __name__ == "__main__":
    main()

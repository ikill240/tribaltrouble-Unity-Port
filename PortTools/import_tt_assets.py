#!/usr/bin/env python3
"""
Copy + convert the original Tribal Trouble assets into this Unity project.

    python import_tt_assets.py <tribaltrouble repo> [<unity project>]

  * assets/geometry/**      -> Assets/TribalTrouble/Art/Models/<group>/<sprite>.glb + catalog.json
  * assets/textures/**      -> Assets/TribalTrouble/Art/Textures/**        (PNG, unchanged)
  * tt/src/main/resources/sfx, music, gui -> Assets/TribalTrouble/Audio/..., Art/Textures/gui_resources
  * translations (*.properties) -> Assets/TribalTrouble/Localization/Source/  (as .txt TextAssets)

Re-runnable: files are overwritten, nothing outside Assets/TribalTrouble is touched.
Needs Python 3.8+ and numpy (pip install numpy).
"""
import os
import shutil
import subprocess
import sys

HERE = os.path.dirname(os.path.abspath(__file__))


def copy_tree(src, dst, exts=None, rename_ext=None):
    n = 0
    for root, _, files in os.walk(src):
        for f in files:
            if exts and os.path.splitext(f)[1].lower() not in exts:
                continue
            rel = os.path.relpath(os.path.join(root, f), src)
            if rename_ext:
                rel = os.path.splitext(rel)[0] + rename_ext
            out = os.path.join(dst, rel)
            os.makedirs(os.path.dirname(out), exist_ok=True)
            shutil.copyfile(os.path.join(root, f), out)
            n += 1
    return n


def main():
    if len(sys.argv) < 2:
        print(__doc__)
        return 2
    repo = os.path.abspath(sys.argv[1])
    project = os.path.abspath(sys.argv[2]) if len(sys.argv) > 2 else os.path.dirname(HERE)
    tt = os.path.join(project, "Assets", "TribalTrouble")
    res = os.path.join(repo, "tt", "src", "main", "resources")

    print("== models")
    rc = subprocess.call([sys.executable, os.path.join(HERE, "tt_geometry_to_gltf.py"),
                          os.path.join(repo, "assets", "geometry"), os.path.join(tt, "Art", "Models")])
    if rc != 0:
        print("model conversion reported failures (see above)")

    print("== textures:", copy_tree(os.path.join(repo, "assets", "textures"),
                                    os.path.join(tt, "Art", "Textures"), {".png"}))
    print("== sfx:", copy_tree(os.path.join(res, "sfx"), os.path.join(tt, "Audio", "Sfx"), {".ogg", ".wav"}))
    print("== music:", copy_tree(os.path.join(res, "music"), os.path.join(tt, "Audio", "Music"), {".ogg", ".wav", ".mp3"}))
    print("== gui resources:", copy_tree(os.path.join(res, "gui"), os.path.join(tt, "Art", "Textures", "gui_resources")))
    n = 0
    for root, _, files in os.walk(res):
        for f in files:
            if f.endswith(".properties"):
                rel = os.path.relpath(os.path.join(root, f), res)
                out = os.path.join(tt, "Localization", "Source", os.path.splitext(rel)[0] + ".txt")
                os.makedirs(os.path.dirname(out), exist_ok=True)
                shutil.copyfile(os.path.join(root, f), out)
                n += 1
    print("== translations:", n)
    return rc


if __name__ == "__main__":
    sys.exit(main())

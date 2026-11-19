"""Builds the kit: textures (+ team masks) and models into an output folder, and preview sheets.
usage: python3 build.py <out dir> [preview dir] [model names...]"""
import os
import sys

import numpy as np
from PIL import Image

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import designs  # noqa: E402
import kit  # noqa: E402
import textures  # noqa: E402
sys.path.insert(0, '/home/claude/forkpush/skins')
import gen_elemental  # noqa: E402


def main():
    out = sys.argv[1]
    prev = sys.argv[2] if len(sys.argv) > 2 else None
    only = set(sys.argv[3:])
    os.makedirs(out, exist_ok=True)
    texs = {}
    for name, fn in textures.TEXTURES.items():
        rgb, mask = fn()
        rgb = np.clip(rgb, 0, 1)
        Image.fromarray((rgb * 255 + 0.5).astype(np.uint8)).save(os.path.join(out, name + '.png'), optimize=True)
        # the Elemental tribe's version (chapter 3): basalt, ochre and moss, ember cracks
        el = np.clip(gen_elemental.elemental(rgb.astype(np.float32), seed=11), 0, 1)
        Image.fromarray((el * 255 + 0.5).astype(np.uint8)).save(os.path.join(out, name + '_e.png'), optimize=True)
        if mask is not None:
            mk = (np.clip(mask, 0, 1) * 255 + 0.5).astype(np.uint8)
            Image.fromarray(np.stack([mk] * 3, -1)).save(os.path.join(out, name + '_team.png'), optimize=True)
        texs[name] = (rgb[::2, ::2], None if mask is None else mask[::2, ::2])
    # the existing textures the kit also uses (bark, shields): their Elemental versions too
    for name in ('palisade_bark', 'shield_viking', 'shield_native'):
        p = os.path.join('/home/claude/forkpush/buildings/existing', name + '.png')
        if os.path.exists(p):
            im = np.asarray(Image.open(p).convert('RGB').resize((512, 512))).astype(np.float32) / 255
            el = np.clip(gen_elemental.elemental(im, seed=11), 0, 1)
            Image.fromarray((el * 255 + 0.5).astype(np.uint8)).save(os.path.join(out, name + '_e.png'), optimize=True)
    # the existing textures the kit also uses (bark, shields), when present for the preview
    for name in ('palisade_bark', 'shield_viking', 'shield_native'):
        for d in ('/home/claude/forkpush/buildings/existing',):
            p = os.path.join(d, name + '.png')
            if os.path.exists(p):
                im = np.asarray(Image.open(p).convert('RGB').resize((256, 256))).astype(float) / 255
                mp = os.path.join(d, name + '_team.png')
                mk = np.asarray(Image.open(mp).convert('L').resize((256, 256))).astype(float) / 255 if os.path.exists(mp) else None
                texs[name] = (im, mk)
    tiles = []
    for fn in designs.ALL:
        m = fn()
        if only and m.name not in only:
            continue
        tris = kit.export(m, os.path.join(out, 'bk_' + m.name + '.bytes'))
        print(m.name, tris, 'triangles', flush=True)
        if prev:
            a = kit.render(m, texs, az=35, el=35)
            b = kit.render(m, texs, az=215, el=35)
            tiles.append(np.concatenate([a, b], 1))
    if prev and tiles:
        cols = 2
        th, tw = tiles[0].shape[:2]
        rows = (len(tiles) + cols - 1) // cols
        sheet = np.zeros((rows * th, cols * tw, 3))
        for i, t in enumerate(tiles):
            r, c = divmod(i, cols)
            sheet[r * th:(r + 1) * th, c * tw:(c + 1) * tw] = t
        Image.fromarray((sheet * 255).astype(np.uint8)).save(os.path.join(prev, 'buildings_sheet.png'))


if __name__ == '__main__':
    main()

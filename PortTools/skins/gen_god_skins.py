"""The gods' own body skins, painted on the chieftain model they are built on (New Tides).

The gods used to be their tribe's chieftain grown and tinted gold, and read as a big chief. Each now wears a body of
its own, laid out in the model's rest-pose 3D space (as gen_skins.py does for the warriors):
  Valkyrie      (viking minor)  silver scale mail, sky-blue cloth, pale gold braids
  Thunder Lord  (viking major)  storm-grey skin with glowing lightning veins, black iron, white beard
  Jaguar Spirit (native minor)  golden pelt with black rosettes, dark jungle-green cloth, a black-and-gold face
  Great Tiki    (native major)  carved wood grain, painted red/teal bands, a white-red-black mask
  Risen Chieftain (Zombies mode boss, both tribes)  rot, bruises, stitched wounds, torn cloth, green eyes
Writes <out>/<race>_<kind>_chief.jpg (and a preview sheet).
usage: python3 gen_god_skins.py <assets dir> <out dir> [preview dir] [kinds, comma-separated]
"""
import os
import sys

import numpy as np
from PIL import Image

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import gen_skins as G  # noqa: E402
import uvmap  # noqa: E402

G.MESHES = {
    'native': ('natives/chieftain/native_chieftain_mesh.xml', 'natives/chieftain/native_chieftain_low_poly_mesh.xml'),
    'viking': ('vikings/chieftain/viking_chief.xml', 'vikings/chieftain/viking_chief_lo.xml'),
}
SIZE = 2048
G.SIZE = SIZE
col, recolour, paint, soften, fbm, worley, smooth, lum, hsv = G.col, G.recolour, G.paint, G.soften, G.fbm, G.worley, G.smooth, G.lum, G.hsv


def regions(img, B):
    h, s, v = hsv(img)
    skin = ((h > 0.0) & (h < 0.11) & (s > 0.18) & (s < 0.8) & (v > 0.3)) & B.body
    metal = (s < 0.18) & (v > 0.45) & B.body
    hair = (v < 0.3) & (s < 0.35) & B.head
    cloth = B.body & ~skin & ~metal & ~hair
    return soften(skin, 2), soften(cloth, 1), soften(metal, 1), soften(hair, 1)


def valkyrie(img, B):
    skin, cloth, metal, hair = regions(img, B)
    P, h = B.pos, B.h
    torso = (B.torso | B.pelvis | B.upper).astype(float)
    # scale mail over the body: silver scales, each with a dark rim
    d, _ = worley(P * np.array([1.0, 1.0, 1.6]), 26, 5)
    scales = smooth(0.18, 0.42, d)
    mail = np.clip(torso * (1 - skin * 0.7), 0, 1)
    silver = col('#c9d3e0')[None, None, :] * (0.55 + 0.45 * scales[..., None])
    img = img * (1 - mail[..., None] * 0.85) + silver * mail[..., None] * 0.85
    img = recolour(img, cloth * (1 - torso), col('#7fb2e8'), amount=0.85)      # sky-blue skirt and sleeves
    img = recolour(img, metal, col('#e8eef6'), amount=0.8)                     # polished steel
    img = recolour(img, hair, col('#e7c873'), amount=0.9, keep=0.6)            # pale gold braids
    bands = ((np.abs(h - 0.5) < 0.014) | (np.abs(h - 0.27) < 0.01)) & B.body & ~B.head & ~B.upper & ~B.fore
    img = paint(img, soften(bands, 1), col('#f0c75a'), 0.9)                    # a gold belt and hem
    img = recolour(img, skin * B.head, col('#f3d6c4'), amount=0.5)             # a pale face
    return img


def thunder_lord(img, B):
    skin, cloth, metal, hair = regions(img, B)
    P = B.pos
    img = recolour(img, skin, col('#8f9aaa'), amount=0.8)                      # storm-grey skin
    img = recolour(img, cloth, col('#1d2433'), amount=0.9)                     # night-blue cloth
    img = recolour(img, metal, col('#3a3f48'), amount=0.85)                    # black iron
    img = recolour(img, hair, col('#e6e8ee'), amount=0.95, keep=0.5)          # a white storm beard
    # lightning veins: thin ridges of 3D noise, brightest on the arms and chest
    n = fbm(P, 5.0, 11, 4)
    ridge = 1 - np.abs(n - 0.5) * 2
    veins = smooth(0.9, 0.97, ridge) * (B.upper | B.fore | B.torso | B.thigh).astype(float) * B.body
    img = img * (1 - veins[..., None]) + col('#8ee9ff')[None, None, :] * veins[..., None]
    glow = soften(veins > 0.3, 3) * 0.35
    img = np.clip(img + col('#3aa6ff')[None, None, :] * glow[..., None], 0, 1)
    # the eyes: two bright points on the face
    eyes = B.head & (B.front > 0.45) & (np.abs(np.abs(B.x) - 0.085) < 0.04) & (np.abs(B.h - 0.92) < 0.025)
    img = paint(img, soften(eyes, 1), col('#d8f8ff'), 1.0)
    return img


def jaguar_spirit(img, B):
    skin, cloth, metal, hair = regions(img, B)
    P = B.pos
    fur = np.clip(skin + (B.limbs | B.torso).astype(float) * 0.6, 0, 1) * B.body
    img = recolour(img, fur, col('#d99a2b'), amount=0.9)                       # golden pelt
    # rosettes: a dark ring round each cell, a darker tan inside
    d, v = worley(P * np.array([1.0, 1.0, 0.8]), 18, 7)
    ring = smooth(0.24, 0.3, d) * (1 - smooth(0.36, 0.44, d))
    inner = 1 - smooth(0.2, 0.26, d)
    img = paint(img, inner * fur * (v > 0.25), col('#b06d1e'), 0.6)
    img = paint(img, ring * fur, col('#1a120a'), 0.9)
    img = recolour(img, cloth * (1 - fur), col('#1f4a2a'), amount=0.85)       # jungle green
    img = recolour(img, metal, col('#e8b23a'), amount=0.8)                     # gold
    face = B.head & (B.front > 0.2) & (B.h > 0.86)
    img = paint(img, soften(face & (np.sin(B.x * 70) > 0.4), 1), col('#14100c'), 0.75)   # face stripes
    return img


def great_tiki(img, B):
    skin, cloth, metal, hair = regions(img, B)
    P, h = B.pos, B.h
    wood = B.body.astype(float) * (1 - B.head * 0.0)
    grain = np.sin(P[..., 2] * 60 + fbm(P, 3.0, 21, 3) * 14)
    woodc = col('#7a4a26')[None, None, :] * (0.75 + 0.25 * grain[..., None]) + col('#2a170b')[None, None, :] * (grain[..., None] < -0.7) * 0.4
    shade = (lum(img) / (np.median(lum(img)[B.body]) + 1e-3))[..., None] ** 0.5
    img = img * (1 - wood[..., None] * 0.9) + np.clip(woodc * shade, 0, 1) * wood[..., None] * 0.9
    # painted bands round the body: red, teal, white zigzags
    zig = np.abs(((np.arctan2(P[..., 1], P[..., 0]) * 6 / np.pi) % 2) - 1)
    for y0, c in ((0.62, '#b8322a'), (0.5, '#1f8f88'), (0.36, '#b8322a')):
        band = (np.abs(h - y0 - zig * 0.025) < 0.018) & B.body & ~B.head
        img = paint(img, soften(band, 1), col(c), 0.9)
    # the mask face: white field, red mouth, black eyes
    face = B.head & (B.front > 0.25)
    img = paint(img, soften(face, 2), col('#ece4d2'), 0.8)
    eyes = face & (np.abs(np.abs(B.x) - 0.085) < 0.05) & (np.abs(h - 0.925) < 0.03)
    img = paint(img, soften(eyes, 1), col('#120c08'), 1.0)
    mouth = face & (np.abs(B.x) < 0.09) & (np.abs(h - 0.86) < 0.02)
    img = paint(img, soften(mouth, 1), col('#c02a20'), 1.0)
    return img


def risen_chief(img, B):
    """The Risen Chieftain (Zombies mode): the chieftain's own body, dead - grey-green rotting skin with dark bruises
    and stitched wounds, the cloth faded and torn to near-black, the metal tarnished green, the hair gone ash-grey,
    and two sickly green eyes."""
    skin, cloth, metal, hair = regions(img, B)
    P = B.pos
    h_, s_, v_ = hsv(img)
    warm = soften(((h_ < 0.13) & (s_ > 0.1) & (v_ > 0.25) & B.body & (B.fore | B.upper | B.head | B.calf | B.thigh)), 2)
    skin = np.maximum(skin, warm * (1 - metal))                               # every bit of bare skin, hands too
    img = recolour(img, skin, col('#7e8a68'), amount=0.85)                     # rot
    n = fbm(P, 4.0, 47, 4)
    img = paint(img, soften((n > 0.6) & (skin > 0.3), 2), col('#3a2c34'), 0.6)            # bruises
    img = paint(img, soften((n < 0.3) & (skin > 0.3), 2), col('#a3ac84'), 0.35)           # pale blotches
    sc = np.abs(np.sin(P[..., 0] * 4 + P[..., 2] * 3)) < 0.028
    st = sc & (np.sin((P[..., 0] - P[..., 2]) * 120) > 0.3)
    img = paint(img, soften(sc & (skin > 0.3), 1), col('#5a1e1a'), 0.85)                  # wounds
    img = paint(img, st & (skin > 0.3), col('#d8cfb8'), 0.9)                              # stitches
    img = recolour(img, cloth, col('#2e2b26'), amount=0.75)                    # faded, near-black cloth
    tear = fbm(P, 9.0, 53, 3)
    img = paint(img, soften((tear > 0.66) & (cloth > 0.3), 1), col('#14110f'), 0.8)       # holes and tears
    img = recolour(img, metal, col('#5f7a5e'), amount=0.7)                     # tarnished, verdigris metal
    img = recolour(img, hair, col('#9a9a92'), amount=0.9, keep=0.55)          # ash-grey hair
    # death drains it all: the colour half gone and a grave-green cast over everything
    grey = lum(img)[..., None]
    dead = B.used[..., None].astype(float)                                     # (the shield and gear too)
    img = img * (1 - 0.45 * dead) + (grey * 0.9 + col('#2c3a22')[None, None, :] * 0.25) * 0.45 * dead
    eyes = B.head & (B.front > 0.45) & (np.abs(np.abs(B.x) - 0.085) < 0.04) & (np.abs(B.h - 0.92) < 0.025)
    img = paint(img, soften(eyes, 1), col('#b8ff5a'), 1.0)                     # the green eyes
    return img


RECIPES = {('viking', 'minor_god'): valkyrie, ('viking', 'major_god'): thunder_lord,
           ('native', 'minor_god'): jaguar_spirit, ('native', 'major_god'): great_tiki,
           ('viking', 'zombie_boss'): risen_chief, ('native', 'zombie_boss'): risen_chief}


KINDS = ('minor_god', 'major_god', 'zombie_boss')


def main():
    global KINDS
    assets, out = sys.argv[1], sys.argv[2]
    if len(sys.argv) > 4:
        KINDS = tuple(sys.argv[4].split(','))
    prev = sys.argv[3] if len(sys.argv) > 3 else None
    os.makedirs(out, exist_ok=True)
    tiles = []
    for race in ('viking', 'native'):
        B = G.Body(assets, race)
        tris, _ = uvmap.load(os.path.join(assets, 'geometry', G.MESHES[race][0]))
        src = Image.open(os.path.join(assets, 'textures', 'models', '%s_chieftain.png' % race)).convert('RGBA')
        if src.size != (SIZE, SIZE):
            src = src.resize((SIZE, SIZE), Image.LANCZOS)
        base = np.asarray(src).astype(np.float32)[..., :3] / 255.0
        for kind in KINDS:
            rgb = RECIPES[(race, kind)](base.copy(), B)
            rgb = np.where(B.used[..., None], rgb, base)   # leave the gutters as they were
            Image.fromarray((np.clip(rgb, 0, 1) * 255 + 0.5).astype(np.uint8), 'RGB').save(
                os.path.join(out, '%s_%s_chief.jpg' % (race, kind)), quality=90, optimize=True)
            if prev:
                small = np.asarray(Image.fromarray((np.clip(rgb, 0, 1) * 255).astype(np.uint8)).resize((512, 512))).astype(float) / 255
                tiles.append(np.concatenate([G.render(tris, small, 1), G.render(tris, small, -1)], 1))
            print(race, kind, flush=True)
    if prev and tiles:
        th, tw = tiles[0].shape[:2]
        sheet = np.full((2 * th, 2 * tw, 3), 0.1)
        for i, t in enumerate(tiles):
            r, c = divmod(i, 2)
            sheet[r * th:(r + 1) * th, c * tw:(c + 1) * tw] = t
        Image.fromarray((np.clip(sheet, 0, 1) * 255).astype(np.uint8)).save(os.path.join(prev, 'gods_sheet.png'))


if __name__ == '__main__':
    main()

"""Unique body skins for the Extended (New Tides) warriors.

Every new unit kind reuses the classic warrior model; this paints each one its own body texture from the warrior
texture it is built on: war paint, pelts, quilted armour, chainmail, soot, woad, rot. The patterns are laid out in the
model's rest-pose 3D space (via uvmap.py), so stripes run round the body and spots don't break at UV seams.

Writes <out>/<race>_<kind>.jpg and preview sheets.
usage: python3 gen_skins.py <assets dir> <out dir> [preview dir]
"""
import colorsys
import os
import sys

import numpy as np
from PIL import Image

sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
import uvmap  # noqa: E402

SIZE = 1024

# unit kind -> the warrior tier texture its template uses (RacesResources)
KINDS = {
    'native': {
        'champion': 'iron', 'scout': 'rock', 'shaman': 'rock', 'slinger': 'rock', 'melee_light': 'rock',
        'melee_heavy': 'iron', 'unique1': 'rock', 'unique2': 'rubber', 'crystal': 'rubber', 'diamond': 'iron',
        'unique3': 'rock', 'minor_god': 'rubber', 'major_god': 'iron', 'zombie': 'rock', 'zombie_brute': 'iron',
        'elite1': 'iron', 'elite2': 'iron', 'elite3': 'rock',
    },
    'viking': {
        'champion': 'iron', 'scout': 'rock', 'shaman': 'rock', 'slinger': 'rock', 'melee_light': 'rock',
        'melee_heavy': 'iron', 'unique1': 'rubber', 'unique2': 'rubber', 'crystal': 'rubber', 'diamond': 'iron',
        'unique3': 'iron', 'minor_god': 'iron', 'major_god': 'iron', 'zombie': 'rock', 'zombie_brute': 'iron',
        'elite1': 'iron', 'elite2': 'iron', 'elite3': 'rock',
    },
}

MESHES = {
    'native': ('natives/warrior/native_warrior_mesh.xml', 'natives/warrior/native_warrior_low_poly_mesh.xml'),
    'viking': ('vikings/warrior/warrior_mesh.xml', 'vikings/warrior/warrior_low_poly_mesh.xml'),
}


# ------------------------------------------------------------------ noise in 3D (seamless across the UV islands)

def _hash(ix, iy, iz, seed):
    h = (ix * 73856093) ^ (iy * 19349663) ^ (iz * 83492791) ^ (seed * 2654435761)
    h = (h ^ (h >> 13)) * 1274126177
    return ((h ^ (h >> 16)) & 0xFFFFFF) / float(0xFFFFFF)


def vnoise(p, freq, seed=0):
    q = p * freq
    i = np.floor(q).astype(np.int64)
    f = q - i
    f = f * f * (3 - 2 * f)
    out = 0
    for dx in (0, 1):
        for dy in (0, 1):
            for dz in (0, 1):
                w = (f[..., 0] if dx else 1 - f[..., 0]) * (f[..., 1] if dy else 1 - f[..., 1]) * (f[..., 2] if dz else 1 - f[..., 2])
                out = out + w * _hash(i[..., 0] + dx, i[..., 1] + dy, i[..., 2] + dz, seed)
    return out


def fbm(p, freq, seed=0, octaves=3):
    s, a, tot = 0, 1.0, 0
    for o in range(octaves):
        s = s + a * vnoise(p, freq * (2 ** o), seed + o * 17)
        tot += a
        a *= 0.5
    return s / tot


def worley(p, freq, seed=0):
    """Distance to the nearest feature point (cells of 1/freq), and that cell's random value."""
    q = p * freq
    i = np.floor(q).astype(np.int64)
    best = np.full(q.shape[:-1], 9.0)
    val = np.zeros(q.shape[:-1])
    for dx in (-1, 0, 1):
        for dy in (-1, 0, 1):
            for dz in (-1, 0, 1):
                cx, cy, cz = i[..., 0] + dx, i[..., 1] + dy, i[..., 2] + dz
                fx = cx + _hash(cx, cy, cz, seed); fy = cy + _hash(cx, cy, cz, seed + 1); fz = cz + _hash(cx, cy, cz, seed + 2)
                d = np.sqrt((q[..., 0] - fx) ** 2 + (q[..., 1] - fy) ** 2 + (q[..., 2] - fz) ** 2)
                m = d < best
                best = np.where(m, d, best)
                val = np.where(m, _hash(cx, cy, cz, seed + 3), val)
    return best, val


def smooth(e0, e1, x):
    t = np.clip((x - e0) / (e1 - e0), 0, 1)
    return t * t * (3 - 2 * t)


# ------------------------------------------------------------------ the body map

class Body:
    def __init__(self, assets, race):
        hi, lo = MESHES[race]
        g = os.path.join(assets, 'geometry')
        tris_hi, names_hi = uvmap.load(os.path.join(g, hi))
        tris_lo, names_lo = uvmap.load(os.path.join(g, lo))
        p1, n1, b1 = uvmap.raster(tris_hi, SIZE, dilate=0)
        p2, n2, b2 = uvmap.raster(tris_lo, SIZE, dilate=0)
        # the low-poly mesh has islands of its own (natives): fill where the high one left nothing
        names = {}
        def remap(b, nm):
            out = -np.ones_like(b)
            for i, n in nm.items():
                n = n.replace('warrior  ', '').strip()
                names.setdefault(n, len(names))
                out[b == i] = names[n]
            return out
        b1 = remap(b1, names_hi); b2 = remap(b2, names_lo)
        take = (b1 < 0) & (b2 >= 0)
        b1[take] = b2[take]; p1[take] = p2[take]; n1[take] = n2[take]
        self.used = b1 >= 0
        # grow into the gutters (mip / filter bleed)
        pos, nrm, bone = p1, n1, b1
        for _ in range(8):
            empty = bone < 0
            for dy, dx in ((0, 1), (0, -1), (1, 0), (-1, 0)):
                sb = np.roll(bone, (dy, dx), (0, 1)); sp = np.roll(pos, (dy, dx), (0, 1)); sn = np.roll(nrm, (dy, dx), (0, 1))
                t = empty & (sb >= 0)
                bone[t] = sb[t]; pos[t] = sp[t]; nrm[t] = sn[t]
                empty = bone < 0
        self.pos, self.nrm, self.bone = pos, nrm, bone
        self.names = {i: n for n, i in names.items()}
        inv = names
        def has(*parts):
            ids = [i for n, i in inv.items() if any(n.startswith(p) for p in parts)]
            return np.isin(bone, ids) if ids else np.zeros_like(self.used)
        self.head = has('Head', 'Neck', 'Ponytail')
        self.torso = has('Spine', 'L Clavicle', 'R Clavicle')
        self.pelvis = has('Pelvis', 'Tail')
        self.upper = has('L UpperArm', 'R UpperArm')
        self.fore = has('L Forearm', 'R Forearm')
        self.hands = has('L Hand', 'R Hand', 'L Finger', 'R Finger')
        self.thigh = has('L Thigh', 'R Thigh')
        self.calf = has('L Calf', 'R Calf')
        self.feet = has('L Foot', 'R Foot', 'L Toe', 'R Toe')
        self.prop = has('Prop')
        self.body = self.used & ~self.prop
        self.limbs = self.upper | self.fore | self.thigh | self.calf
        z = pos[..., 2]
        head_only = has('Head')
        top = np.percentile(z[head_only], 90) if head_only.any() else z.max()
        # feathers / horns above the crown keep their own colours
        self.headdress = self.used & (z > top + 0.06)
        self.body = self.body & ~self.headdress
        self.h = np.clip(z / max(top, 1e-3), 0, 1.2)   # 0 feet .. 1 crown
        self.front = -nrm[..., 1]                       # +1 faces forward (-y)
        self.x = pos[..., 0]


# ------------------------------------------------------------------ colour helpers

def hsv(img):
    r, g, b = img[..., 0], img[..., 1], img[..., 2]
    mx = img.max(-1); mn = img.min(-1); d = mx - mn + 1e-6
    h = np.where(mx == r, ((g - b) / d) % 6, np.where(mx == g, (b - r) / d + 2, (r - g) / d + 4)) / 6.0
    s = (mx - mn) / (mx + 1e-6)
    return h, s, mx


def lum(img):
    return img[..., 0] * 0.3 + img[..., 1] * 0.59 + img[..., 2] * 0.11


def col(hexs):
    hexs = hexs.lstrip('#')
    return np.array([int(hexs[i:i + 2], 16) / 255.0 for i in (0, 2, 4)])


def recolour(img, mask, c, keep=1.0, amount=1.0):
    """Paints colour c over mask keeping the texture's shading (its luminance relative to its mean)."""
    L = lum(img)
    m = mask.astype(float) if mask.dtype == bool else mask
    ref = np.median(L[m > 0.5]) if (m > 0.5).any() else 0.5
    shade = (L / max(ref, 0.05)) ** keep
    out = np.clip(c[None, None, :] * shade[..., None], 0, 1)
    a = (m * amount)[..., None]
    return img * (1 - a) + out * a


def paint(img, mask, c, amount=1.0, keep=0.35):
    """War paint: a flat pigment that still lets the surface detail through a little."""
    L = lum(img)
    m = (mask.astype(float) if mask.dtype == bool else mask)[..., None] * amount
    detail = (L / (np.mean(L) + 1e-3))[..., None] ** keep
    return img * (1 - m) + np.clip(c[None, None, :] * detail, 0, 1) * m


def classify(img, race, B):
    h, s, v = hsv(img)
    if race == 'native':
        skin = (h > 0.03) & (h < 0.12) & (s > 0.25) & (v > 0.25) & B.body
        cloth = B.body & ~skin
    else:
        skin = ((h < 0.06) | (h > 0.95)) & (s > 0.2) & (s < 0.75) & (v > 0.55) & B.body
        hair = (s < 0.22) & (v < 0.45) & B.head
        metal = (h > 0.5) & (h < 0.7) & (s > 0.12) & (v > 0.35)
        cloth = B.body & ~skin & ~metal
        return skin, cloth, metal, hair
    metal = (h > 0.5) & (h < 0.7) & (s > 0.2)
    return skin, cloth, metal, np.zeros_like(skin)


def soften(m, r=2):
    m = m.astype(float)
    for _ in range(r):
        m = (m + np.roll(m, 1, 0) + np.roll(m, -1, 0) + np.roll(m, 1, 1) + np.roll(m, -1, 1)) / 5
    return m


# ------------------------------------------------------------------ the recipes

def skin_native(kind, img, B):
    skin, cloth, metal, _ = classify(img, 'native', B)
    P, h, fr, x = B.pos, B.h, B.front, B.x
    sk = soften(skin, 2)
    if kind == 'champion':   # red ochre body, gold-banded arms, a black-and-gold face
        img = recolour(img, sk, col('#b0482c'), amount=0.55)
        bands = (np.sin(h * 90) > 0.6) & (B.upper | B.fore)
        img = paint(img, soften(bands & skin, 1), col('#e8b23a'), 0.85)
        img = paint(img, soften(B.head & skin & (B.pos[..., 2] > np.percentile(P[..., 2][B.head], 55)), 1), col('#1c1410'), 0.75)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#7a1d14'), amount=0.8)
    elif kind == 'scout':    # leaf-green and earth camouflage stripes, a green loincloth
        n = fbm(P, 2.2, 3)
        stripes = np.sin(P[..., 0] * 9 + P[..., 2] * 7 + n * 6) > 0.35
        img = paint(img, soften(stripes & skin, 1), col('#4d6b2a'), 0.7)
        img = paint(img, soften((np.sin(P[..., 0] * 6 - P[..., 2] * 11 + n * 4) > 0.75) & skin, 1), col('#2f2516'), 0.6)
        img = recolour(img, soften(cloth & (B.pelvis | B.thigh), 1), col('#3f6a2c'), amount=0.8)
    elif kind == 'shaman':   # white bone paint: a skull face, ribs on the chest, dots down the arms
        img = paint(img, soften(B.head & skin & (fr > 0.0), 1), col('#ece6d6'), 0.8)
        eyes = B.head & (fr > 0.4) & (np.abs(np.abs(x) - 0.09) < 0.05) & (np.abs(B.h - 0.93) < 0.03)
        img = paint(img, soften(eyes, 1), col('#141010'), 0.9)
        ribs = B.torso & (fr > 0.2) & (np.sin(P[..., 2] * 55) > 0.55) & (np.abs(x) > 0.04) & (np.abs(x) < 0.32)
        img = paint(img, soften(ribs & skin, 1), col('#ece6d6'), 0.85)
        dots, _ = worley(P, 9, 5)
        img = paint(img, soften((dots < 0.22) & (B.upper | B.fore | B.thigh) & skin, 1), col('#ece6d6'), 0.85)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#5a3a6e'), amount=0.6)
    elif kind == 'slinger':  # red ochre dots in rows, a sandy loincloth
        d, _ = worley(P, 12, 9)
        img = paint(img, soften((d < 0.28) & skin & ~B.head, 1), col('#a8361e'), 0.8)
        img = paint(img, soften(B.head & skin & (np.abs(B.h - 0.9) < 0.025), 1), col('#a8361e'), 0.8)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#b89a62'), amount=0.7)
    elif kind == 'melee_light':   # Club Warrior: one half of the body red ochre, white bands, a black jaw
        img = paint(img, soften(skin & (x > 0.0) & ~B.head, 2), col('#a3301c'), 0.75)
        bands = (np.sin(P[..., 2] * 16) > 0.7) & (B.torso | B.thigh | B.upper)
        img = paint(img, soften(bands & skin, 1), col('#e9e2cf'), 0.85)
        img = paint(img, soften(B.head & skin & (B.h < 0.86) & (fr > 0.2), 1), col('#1b1714'), 0.8)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#6e2a1a'), amount=0.6)
    elif kind == 'melee_heavy':   # quilted cotton armour (ichcahuipilli) over the chest, red trim
        q = (np.abs(np.sin((P[..., 0] + P[..., 2]) * 22)) < 0.22) | (np.abs(np.sin((P[..., 0] - P[..., 2]) * 22)) < 0.22)
        armour = (B.torso | B.pelvis) & B.body
        img = recolour(img, soften(armour, 1), col('#d9cdb0'), amount=0.9, keep=0.6)
        img = paint(img, soften(q & armour, 1), col('#8e7f62'), 0.55)
        trim = armour & (np.abs(B.h - np.percentile(B.h[B.pelvis], 15)) < 0.025)
        img = paint(img, soften(trim, 1), col('#a5281c'), 0.9)
        img = paint(img, soften(B.head & skin & (np.abs(B.h - 0.88) < 0.02), 1), col('#a5281c'), 0.85)
    elif kind == 'unique1':  # Blowgunner: poison-frog colours, yellow and teal speckles
        d, v = worley(P, 14, 11)
        img = paint(img, soften((d < 0.3) & (v > 0.5) & skin, 1), col('#e9d23c'), 0.85)
        img = paint(img, soften((d < 0.3) & (v <= 0.5) & skin, 1), col('#1aa39a'), 0.85)
        img = paint(img, soften(B.head & skin & (fr > 0.3) & (np.abs(B.h - 0.92) < 0.03), 1), col('#1aa39a'), 0.85)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#2c6e45'), amount=0.7)
    elif kind == 'unique2':  # Jaguar Warrior: the whole body a jaguar's coat
        img = recolour(img, sk, col('#d99a3a'), amount=0.8)
        d, _ = worley(P, 10, 13)
        ring = (d > 0.17) & (d < 0.3)
        img = paint(img, soften(ring & skin, 1), col('#1d140b'), 0.9)
        img = paint(img, soften((d <= 0.17) & skin, 1), col('#a8641f'), 0.5)
    elif kind == 'crystal':  # pale skin with glowing cyan veins
        img = recolour(img, sk, col('#9a8a86'), amount=0.35)
        n = fbm(P, 3.5, 21, 3)
        veins = np.abs(n - 0.5) < 0.025
        img = paint(img, soften(veins & skin, 1), col('#6ff4ff'), 0.95, keep=0.05)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#2b6c86'), amount=0.7)
    elif kind == 'diamond':  # white clay paint with gold trims, a gold collar
        img = paint(img, soften(skin & (B.torso | B.upper | B.head), 2), col('#e8e8ee'), 0.6)
        img = paint(img, soften(skin & B.torso & (np.abs(B.h - np.percentile(B.h[B.torso], 92)) < 0.03), 1), col('#e8b847'), 0.9)
        bands = (np.sin(h * 70) > 0.75) & (B.fore | B.calf)
        img = paint(img, soften(bands & skin, 1), col('#e8b847'), 0.85)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#e9e4d8'), amount=0.75)
    elif kind == 'unique3':  # Dart Master: an indigo mask over the eyes, indigo zig-zags on the arms
        img = paint(img, soften(B.head & skin & (np.abs(B.h - 0.92) < 0.035), 1), col('#22306e'), 0.9)
        zz = np.abs(((P[..., 2] * 14 + np.abs(((P[..., 0] * 14) % 2) - 1)) % 2) - 1) < 0.18
        img = paint(img, soften(zz & (B.upper | B.fore | B.thigh) & skin, 1), col('#22306e'), 0.85)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#30407e'), amount=0.6)
    elif kind == 'minor_god':  # Jaguar Spirit: ghost-pale, green spirals of light
        img = recolour(img, sk, col('#c9d6c4'), amount=0.75)
        d, _ = worley(P, 7, 17)
        spiral = (np.abs(np.sin(d * 40)) < 0.25) & (d < 0.35)
        img = paint(img, soften(spiral & skin, 1), col('#45f08c'), 0.9, keep=0.05)
    elif kind == 'major_god':  # Great Tiki: carved wood, grain and painted bands
        grain = fbm(P * np.array([6, 6, 1.2]), 2.0, 31, 4)
        wood = col('#7a4a26') * (0.75 + 0.5 * grain[..., None])
        a = soften(B.body, 1)[..., None] * 0.85
        L = (lum(img) / (np.mean(lum(img)) + 1e-3))[..., None] ** 0.4
        img = img * (1 - a) + np.clip(wood * L, 0, 1) * a
        bands = (np.sin(P[..., 2] * 12) > 0.82) & B.body
        img = paint(img, soften(bands, 1), col('#c9361f'), 0.8)
        img = paint(img, soften(B.head & (fr > 0.4) & (np.abs(B.h - 0.92) < 0.04), 1), col('#f0d27a'), 0.9)
    elif kind == 'elite1':  # Eagle Knight: an eagle's plumage - brown feathers over the body, a white chest, a gold beak mask
        img = recolour(img, sk, col('#8a5a30'), amount=0.55)
        d, v = worley(P * np.array([1, 1, 2.2]), 16, 91)
        feathers = (d > 0.2) & skin & ~B.head
        img = paint(img, soften(feathers, 1), col('#4a2c14'), 0.6)
        chest = B.torso & (fr > 0.35) & skin
        img = paint(img, soften(chest, 2), col('#ece4d2'), 0.75)
        img = paint(img, soften(B.head & skin & (fr > 0.3) & (np.abs(B.h - 0.9) < 0.04), 1), col('#e0a82e'), 0.85)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#5b2a12'), amount=0.8)
    elif kind == 'elite2':  # Hawk Eye: slate-grey paint, white streaks through the eyes, a grey loincloth
        img = recolour(img, sk, col('#6a7076'), amount=0.5)
        streak = B.head & skin & (np.abs(B.h - 0.925) < 0.018)
        img = paint(img, soften(streak, 1), col('#f2f2ee'), 0.9)
        n = fbm(P, 3, 93)
        img = paint(img, soften((np.sin(P[..., 2] * 30 + n * 5) > 0.85) & (B.upper | B.fore) & skin, 1), col('#f2f2ee'), 0.7)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#4c5258'), amount=0.8)
    elif kind == 'elite3':  # War Drummer: black and white bands round the body, red hands, a red loincloth
        bands = (np.sin(P[..., 2] * 20) > 0.3) & (B.torso | B.thigh | B.calf | B.upper)
        img = paint(img, soften(bands & skin, 1), col('#1a1614'), 0.8)
        img = paint(img, soften(~bands & skin & (B.torso | B.thigh), 1), col('#ece6d6'), 0.55)
        img = paint(img, soften(B.fore & skin & (B.h < np.percentile(B.h[B.fore], 45)), 1), col('#b02818'), 0.85)
        img = recolour(img, soften(cloth & B.pelvis, 1), col('#a32418'), amount=0.8)
    elif kind in ('zombie', 'zombie_brute'):  # grey-green rot, dark bruises, (brute) stitched scars
        img = recolour(img, sk, col('#7d8b6a'), amount=0.8)
        n = fbm(P, 4, 41)
        img = paint(img, soften((n > 0.62) & skin, 2), col('#3b2e33'), 0.55)
        img = recolour(img, soften(cloth, 1), col('#4a463c'), amount=0.5)
        if kind == 'zombie_brute':
            sc = np.abs(np.sin(P[..., 0] * 4 + P[..., 2] * 3)) < 0.03
            st = sc & (np.sin((P[..., 0] - P[..., 2]) * 120) > 0.3)
            img = paint(img, soften(sc & skin, 1), col('#5a1e1a'), 0.85)
            img = paint(img, st & skin, col('#d8cfb8'), 0.9)
    return img


def skin_viking(kind, img, B):
    skin, cloth, metal, hair = classify(img, 'viking', B)
    P, h, fr, x = B.pos, B.h, B.front, B.x
    sk = soften(skin, 2)
    cl = soften(cloth & ~B.head, 1)
    mt = soften(metal, 1)
    if kind == 'champion':   # gilded mail and helm, a red tunic
        img = recolour(img, mt, col('#d6a542'), amount=0.75, keep=1.2)
        img = recolour(img, cl, col('#7c1d17'), amount=0.55)
    elif kind == 'scout':    # forest green, light leather, a green hood
        img = recolour(img, cl, col('#35552a'), amount=0.65)
        img = paint(img, soften(B.head & skin & (np.abs(B.h - 0.93) < 0.02) & (fr > 0.3), 1), col('#2a3a1c'), 0.6)
    elif kind == 'shaman':   # Skald: a blue robe stitched with gold runes, a white beard
        img = recolour(img, cl, col('#1e3a7a'), amount=0.7)
        r = (np.abs(np.sin(P[..., 0] * 40)) < 0.12) & (np.sin(P[..., 2] * 25) > 0.0) & (np.abs(np.sin((P[..., 0] + P[..., 2]) * 30)) < 0.4)
        img = paint(img, soften(r & cloth & (B.torso | B.pelvis), 1), col('#e0b84a'), 0.8)
        img = recolour(img, soften(hair, 1), col('#d8d6cf'), amount=0.8)
    elif kind == 'slinger':  # brown hide, freckled, ginger
        img = recolour(img, cl, col('#6e4a2c'), amount=0.6)
        img = recolour(img, soften(hair, 1), col('#b4552a'), amount=0.8)
        d, _ = worley(P, 30, 7)
        img = paint(img, (d < 0.18) & skin & B.head, col('#b06a46'), 0.5)
    elif kind == 'melee_light':   # Axeman: a mail shirt over the torso
        ring = np.sin(np.arange(SIZE)[None, :] * 0.9) * np.sin(np.arange(SIZE)[:, None] * 0.9 + np.arange(SIZE)[None, :] * 0.45)
        mail = cloth & (B.torso | B.pelvis | B.upper)
        img = recolour(img, soften(mail, 1), col('#7c8288'), amount=0.75, keep=0.6)
        img = img * (1 - 0.25 * soften(mail, 1)[..., None] * (ring[..., None] < 0))
    elif kind == 'melee_heavy':   # Huscarl: heavy mail, a red surcoat, dark leather
        ring = np.sin(np.arange(SIZE)[None, :] * 0.9) * np.sin(np.arange(SIZE)[:, None] * 0.9 + np.arange(SIZE)[None, :] * 0.45)
        mail = cloth & (B.upper | B.fore | B.thigh)
        img = recolour(img, soften(mail, 1), col('#6c7278'), amount=0.75, keep=0.6)
        img = img * (1 - 0.3 * soften(mail, 1)[..., None] * (ring[..., None] < 0))
        img = recolour(img, soften(cloth & (B.torso | B.pelvis), 1), col('#8a1e16'), amount=0.7)
    elif kind == 'unique1':  # Fire Thrower: soot-black, singed, ember-orange cracks
        n = fbm(P, 3, 51)
        img = img * (1 - 0.6 * smooth(0.3, 0.7, n)[..., None] * soften(B.body, 1)[..., None])
        cracks = np.abs(fbm(P, 5, 53) - 0.5) < 0.03
        img = paint(img, soften(cracks & skin, 1), col('#ff7a1a'), 0.95, keep=0.05)
        img = paint(img, soften(cracks & cloth & B.torso, 1), col('#ff9a3a'), 0.7, keep=0.1)
        img = recolour(img, cl, col('#3a2a22'), amount=0.5)
    elif kind == 'unique2':  # Berserker: blue woad swirls, wild red hair, bare and bloodied
        d, _ = worley(P, 6, 61)
        swirl = (np.abs(np.sin(d * 34)) < 0.3) & (d < 0.32)
        img = paint(img, soften(swirl & skin, 1), col('#2a4ca0'), 0.75)
        img = recolour(img, soften(hair, 1), col('#a8381c'), amount=0.85)
        img = recolour(img, cl, col('#4a3a30'), amount=0.4)
    elif kind == 'crystal':  # cyan-glassed metal, a teal tunic
        img = recolour(img, mt, col('#7fd8ea'), amount=0.7, keep=1.2)
        img = recolour(img, cl, col('#1f5a66'), amount=0.55)
    elif kind == 'diamond':  # polished white steel and gold
        img = recolour(img, mt, col('#e8ecf2'), amount=0.8, keep=1.3)
        img = recolour(img, cl, col('#e6e0d0'), amount=0.55)
        trim = cloth & (np.sin(P[..., 2] * 30) > 0.9)
        img = paint(img, soften(trim, 1), col('#e0b442'), 0.85)
    elif kind == 'unique3':  # Shield Maiden: golden braids, a crimson dress with a white border
        img = recolour(img, soften(hair, 1), col('#e0b456'), amount=0.85)
        img = recolour(img, cl, col('#9b2320'), amount=0.6)
        border = cloth & B.pelvis & (B.h < np.percentile(B.h[B.pelvis], 25))
        img = paint(img, soften(border, 1), col('#ece4d0'), 0.8)
    elif kind == 'minor_god':  # Valkyrie: silver and white, golden hair
        img = recolour(img, mt, col('#dfe4ec'), amount=0.8, keep=1.3)
        img = recolour(img, cl, col('#ece8e0'), amount=0.7)
        img = recolour(img, soften(hair, 1), col('#f0c860'), amount=0.85)
    elif kind == 'major_god':  # Thunder Lord: storm-blue, lightning across the body
        img = recolour(img, cl, col('#24345a'), amount=0.7)
        img = recolour(img, mt, col('#9fb4d8'), amount=0.5, keep=1.2)
        bolt = np.abs(fbm(P * np.array([1, 1, 0.4]), 4, 71) - 0.5) < 0.018
        img = paint(img, soften(bolt & B.body, 1), col('#cfe8ff'), 0.95, keep=0.05)
        img = recolour(img, soften(hair, 1), col('#c8ccd6'), amount=0.7)
    elif kind == 'elite1':  # Jarl: bright mail, a deep blue tunic with gold trim, a dark beard
        img = recolour(img, mt, col('#c8ced6'), amount=0.7, keep=1.25)
        img = recolour(img, cl, col('#1d2f6e'), amount=0.7)
        trim = cloth & (np.sin(P[..., 2] * 28) > 0.88)
        img = paint(img, soften(trim, 1), col('#e0b442'), 0.9)
        img = recolour(img, soften(hair, 1), col('#2a1e18'), amount=0.8)
    elif kind == 'elite2':  # Huntress: green-brown leathers, dark-red braids
        img = recolour(img, cl, col('#4a5530'), amount=0.65)
        img = recolour(img, mt, col('#7a5a3a'), amount=0.5)
        img = recolour(img, soften(hair, 1), col('#7a2418'), amount=0.85)
        n = fbm(P, 3, 97)
        img = paint(img, soften((n > 0.6) & cloth, 1), col('#2e3a1c'), 0.5)
    elif kind == 'elite3':  # Horn Bearer: a bearskin over a black-and-yellow striped tunic
        stripes = cloth & (np.sin((P[..., 0] + P[..., 2]) * 26) > 0.2) & (B.torso | B.pelvis)
        img = recolour(img, cl, col('#5a3a20'), amount=0.6)
        img = paint(img, soften(stripes, 1), col('#d8a830'), 0.8)
        fur = cloth & (B.upper | B.head) 
        img = recolour(img, soften(fur, 1), col('#3e2a1a'), amount=0.75)
        img = recolour(img, soften(hair, 1), col('#8a6a3a'), amount=0.7)
    elif kind in ('zombie', 'zombie_brute'):
        img = recolour(img, sk, col('#8a9474'), amount=0.8)
        n = fbm(P, 4, 81)
        img = paint(img, soften((n > 0.62) & skin, 2), col('#40303a'), 0.55)
        img = recolour(img, cl, col('#46423a'), amount=0.5)
        if kind == 'zombie_brute':
            sc = np.abs(np.sin(P[..., 0] * 4 + P[..., 2] * 3)) < 0.03
            img = paint(img, soften(sc & skin, 1), col('#5a1e1a'), 0.85)
    return img


# ------------------------------------------------------------------ previews (a tiny software renderer)

def render(tris, tex, view, W=200, H=260):
    """Orthographic front (+) or back (-) view of the mesh with tex (H,W,3 float)."""
    img = np.full((H, W, 3), 0.18)
    zb = np.full((H, W), 1e9)
    th, tw = tex.shape[:2]
    for t in tris:
        p = np.array([[a[0] * view, a[2], a[1] * view] for a in t])
        uv = np.array([[a[6], a[7]] for a in t])
        sx = W / 2 + p[:, 0] * 75; sy = H - 8 - p[:, 1] * 75
        x0, x1 = int(max(sx.min(), 0)), int(min(sx.max(), W - 1)); y0, y1 = int(max(sy.min(), 0)), int(min(sy.max(), H - 1))
        if x1 < x0 or y1 < y0:
            continue
        xs, ys = np.meshgrid(np.arange(x0, x1 + 1) + 0.5, np.arange(y0, y1 + 1) + 0.5)
        d = (sy[1] - sy[2]) * (sx[0] - sx[2]) + (sx[2] - sx[1]) * (sy[0] - sy[2])
        if abs(d) < 1e-9:
            continue
        l0 = ((sy[1] - sy[2]) * (xs - sx[2]) + (sx[2] - sx[1]) * (ys - sy[2])) / d
        l1 = ((sy[2] - sy[0]) * (xs - sx[2]) + (sx[0] - sx[2]) * (ys - sy[2])) / d
        l2 = 1 - l0 - l1
        m = (l0 >= 0) & (l1 >= 0) & (l2 >= 0)
        z = l0 * p[0, 2] + l1 * p[1, 2] + l2 * p[2, 2]
        sub = zb[y0:y1 + 1, x0:x1 + 1]
        m &= z < sub
        if not m.any():
            continue
        u = l0 * uv[0, 0] + l1 * uv[1, 0] + l2 * uv[2, 0]; v = l0 * uv[0, 1] + l1 * uv[1, 1] + l2 * uv[2, 1]
        tx = np.clip((u * tw).astype(int), 0, tw - 1); ty = np.clip(((1 - v) * th).astype(int), 0, th - 1)
        sub[m] = z[m]
        img[y0:y1 + 1, x0:x1 + 1][m] = tex[ty[m], tx[m]]
    return img


def main():
    assets, out = sys.argv[1], sys.argv[2]
    prev = sys.argv[3] if len(sys.argv) > 3 else None
    only = os.environ.get('ONLY')
    os.makedirs(out, exist_ok=True)
    for race in ('native', 'viking'):
        B = Body(assets, race)
        tris, _ = uvmap.load(os.path.join(assets, 'geometry', MESHES[race][0]))
        tiles = []
        for kind, tier in KINDS[race].items():
            if only and kind not in only.split(','):
                continue
            src = Image.open(os.path.join(assets, 'textures', 'models', '%s_warrior_%s.png' % (race, tier))).convert('RGBA')
            if src.size != (SIZE, SIZE):
                src = src.resize((SIZE, SIZE), Image.LANCZOS)
            a = np.asarray(src).astype(np.float32) / 255.0
            rgb = a[..., :3].copy()
            rgb = (skin_native if race == 'native' else skin_viking)(kind, rgb, B)
            # the body textures don't use alpha (only foliage clips): a JPEG keeps the 30 skins small
            Image.fromarray((np.clip(rgb, 0, 1) * 255 + 0.5).astype(np.uint8), 'RGB').save(os.path.join(out, '%s_%s.jpg' % (race, kind)), quality=90, optimize=True)
            if prev:
                small = np.asarray(Image.fromarray((rgb * 255).astype(np.uint8)).resize((512, 512))).astype(float) / 255
                f = render(tris, small, 1); b = render(tris, small, -1)
                tiles.append((kind, np.concatenate([f, b], 1)))
            print(race, kind, flush=True)
        if prev and tiles:
            cols = 4
            th, tw = tiles[0][1].shape[:2]
            rows = (len(tiles) + cols - 1) // cols
            sheet = np.full((rows * th, cols * tw, 3), 0.1)
            for i, (k, t) in enumerate(tiles):
                r, c = divmod(i, cols)
                sheet[r * th:(r + 1) * th, c * tw:(c + 1) * tw] = t
            Image.fromarray((sheet * 255).astype(np.uint8)).save(os.path.join(prev, race + '_sheet.png'))


if __name__ == '__main__':
    main()

"""HD textures for the resource rocks (the Modern look).

The rock and iron deposits wore the Java game's 128 px generated textures (GeneratorRock / GeneratorIron), and gold,
coal and diamonds were the same rock tinted. These are seamless 1024 px textures with the same broad cell pattern,
each its own material:
  ore_rock      grey stone: cracked cells, grain, mineral flecks, a little lichen
  ore_iron      dark red-brown iron ore: rust stains and dark metallic nodules with bright edges
  ore_gold      grey quartz rock with bright gold veins and nuggets
  ore_coal      near-black rock with glossy black seams
  ore_diamond   pale blue-grey rock studded with white sparkles and a few blue stones
usage: python3 gen_ore_textures.py <out dir> [preview.png]
"""
import os
import sys

import numpy as np
from PIL import Image

N = 1024
rng = np.random.default_rng(1729)


def col(h):
    h = h.lstrip('#')
    return np.array([int(h[i:i + 2], 16) / 255.0 for i in (0, 2, 4)], np.float32)


def periodic_noise(cells, seed):
    """Value noise on a periodic lattice of cells x cells (seamless), smooth-step interpolated."""
    r = np.random.default_rng(seed).random((cells, cells)).astype(np.float32)
    t = np.arange(N, dtype=np.float32) * cells / N
    i0 = np.floor(t).astype(int)
    f = t - i0
    f = f * f * (3 - 2 * f)
    i1 = (i0 + 1) % cells
    i0 %= cells
    a = r[i0][:, i0]; b = r[i0][:, i1]; c = r[i1][:, i0]; d = r[i1][:, i1]
    fx = f[None, :]; fy = f[:, None]
    return (a * (1 - fx) + b * fx) * (1 - fy) + (c * (1 - fx) + d * fx) * fy


def fbm(base, octaves, seed, gain=0.5):
    out = np.zeros((N, N), np.float32); amp = 1.0; tot = 0.0
    for o in range(octaves):
        out += amp * periodic_noise(base * 2 ** o, seed + o * 31)
        tot += amp; amp *= gain
    return out / tot


_WARP = {}


def voronoi(cells, seed, warp=0.0):
    """Seamless Voronoi: F1, F2 distances (in cell units) and a per-cell random value; warp bends the cells (in
    cells) with seamless noise, so the edges wander like real cracks instead of running straight."""
    r = np.random.default_rng(seed)
    gx, gy = np.meshgrid(np.arange(cells), np.arange(cells))
    px = (gx + r.random((cells, cells))).astype(np.float32)
    py = (gy + r.random((cells, cells))).astype(np.float32)
    val = r.random((cells, cells)).astype(np.float32)
    t = (np.arange(N, dtype=np.float32) + 0.5) * cells / N
    X, Y = np.meshgrid(t, t)
    if warp > 0:
        if 'x' not in _WARP:
            _WARP['x'] = fbm(6, 4, 5) - 0.5
            _WARP['y'] = fbm(6, 4, 6) - 0.5
        X = X + _WARP['x'] * warp * 2
        Y = Y + _WARP['y'] * warp * 2
    cx = np.floor(X).astype(int); cy = np.floor(Y).astype(int)
    f1 = np.full((N, N), 9.0, np.float32); f2 = np.full((N, N), 9.0, np.float32); v1 = np.zeros((N, N), np.float32)
    for dy in (-1, 0, 1):
        for dx in (-1, 0, 1):
            nx = cx + dx; ny = cy + dy
            wx = nx % cells; wy = ny % cells
            qx = px[wy, wx] + (nx - wx); qy = py[wy, wx] + (ny - wy)
            d = np.sqrt((X - qx) ** 2 + (Y - qy) ** 2)
            closer = d < f1
            f2 = np.where(closer, f1, np.minimum(f2, d))
            v1 = np.where(closer, val[wy, wx], v1)
            f1 = np.where(closer, d, f1)
    return f1, f2, v1


def smooth(a, b, x):
    t = np.clip((x - a) / (b - a), 0, 1)
    return t * t * (3 - 2 * t)


def shade(height, strength=2.2):
    """Light from the upper left on a seamless height field (gradients wrap)."""
    gx = (np.roll(height, -1, 1) - np.roll(height, 1, 1)) * N / 2
    gy = (np.roll(height, -1, 0) - np.roll(height, 1, 0)) * N / 2
    nx, ny, nz = -gx * strength / N * 8, -gy * strength / N * 8, np.ones_like(height)
    ln = np.sqrt(nx * nx + ny * ny + nz * nz)
    L = np.array([-0.5, -0.55, 0.67]); L /= np.linalg.norm(L)
    return np.clip((nx * L[0] + ny * L[1] + nz * L[2]) / ln, 0, 1)


def stone(seed, cells=(3, 7, 15)):
    """Shared stone: broad, bent facets with a few wandering cracks, per-facet tone and grain; returns
    (height, tone, cracks, grain)."""
    h = np.zeros((N, N), np.float32); tone = np.zeros((N, N), np.float32); cracks = np.zeros((N, N), np.float32)
    for k, c in enumerate(cells):
        f1, f2, v = voronoi(c, seed + k * 101, warp=(0.45, 0.6, 0.8)[k])
        edge = f2 - f1
        w = (0.5, 0.3, 0.2)[k]
        h += w * smooth(0.0, 0.45, edge) * (0.7 + 0.6 * v)
        tone += w * (v - 0.5)
        if k == 0:   # only the big facets crack, and only along some of their edges
            keep = smooth(0.45, 0.6, fbm(5, 3, seed + 3))
            cracks = (1 - smooth(0.0, 0.03, edge)) * keep
    grain = fbm(32, 4, seed + 7)
    h += 0.3 * grain + 0.08 * fbm(128, 2, seed + 9)
    return h, tone, cracks, grain


def compose(base, h, cracks, crack_col, light=0.55):
    s = shade(h)
    img = base * (1 - light + light * 1.25 * s[..., None])
    img = img * (1 - cracks[..., None] * 0.75) + crack_col[None, None, :] * cracks[..., None] * 0.75
    return np.clip(img, 0, 1)


def ore_rock():
    h, tone, cracks, grain = stone(11)
    base = col('#8d877f')[None, None, :] * (0.82 + 0.35 * tone[..., None] + 0.25 * (grain[..., None] - 0.5))
    warm = fbm(6, 3, 21)[..., None]
    base = base * (1 - 0.25 * warm) + col('#9a8a74')[None, None, :] * 0.25 * warm
    fleck = (fbm(256, 1, 31) > 0.82)[..., None]
    base = np.where(fleck, base * 1.25, base)
    lichen = smooth(0.66, 0.76, fbm(10, 5, 41)) * smooth(0.3, 0.6, fbm(48, 2, 43))
    base = base * (1 - lichen[..., None] * 0.45) + col('#7d8452')[None, None, :] * lichen[..., None] * 0.45
    return compose(base, h, cracks, col('#2e2a27'))


def ore_iron():
    h, tone, cracks, grain = stone(13)
    base = col('#5e4c44')[None, None, :] * (0.85 + 0.3 * tone[..., None] + 0.25 * (grain[..., None] - 0.5))
    rust = smooth(0.5, 0.68, fbm(8, 5, 53))
    base = base * (1 - rust[..., None] * 0.7) + col('#a2522a')[None, None, :] * rust[..., None] * 0.7
    # lumps of dark, metallic ore: irregular blobs with a steely sheen where the light catches them
    blob = fbm(14, 4, 61)
    nod = smooth(0.6, 0.66, blob)
    h = h + 0.3 * nod + 0.15 * nod * fbm(64, 2, 63)
    base = base * (1 - nod[..., None] * 0.9) + col('#3a3c42')[None, None, :] * nod[..., None] * 0.9
    img = compose(base, h, cracks, col('#24180f'))
    sheen = smooth(0.62, 0.9, shade(h, 3.0)) * nod
    img = img + col('#c4c9d2')[None, None, :] * (sheen * 0.55)[..., None]
    return np.clip(img, 0, 1)


def ore_gold():
    h, tone, cracks, grain = stone(17)
    base = col('#9a958c')[None, None, :] * (0.85 + 0.3 * tone[..., None] + 0.2 * (grain[..., None] - 0.5))
    quartz = smooth(0.55, 0.7, fbm(12, 4, 71))
    base = base * (1 - quartz[..., None] * 0.5) + col('#d9d4c8')[None, None, :] * quartz[..., None] * 0.5
    n = fbm(5, 5, 73)
    vein = 1 - smooth(0.0, 0.03, np.abs(n - 0.5))
    n2 = fbm(9, 4, 79)
    vein = np.maximum(vein, 1 - smooth(0.0, 0.022, np.abs(n2 - 0.5)))
    vein = vein * smooth(0.35, 0.55, fbm(4, 3, 81))   # the veins come and go
    f1, f2, v = voronoi(18, 83, warp=0.5)
    nug = (1 - smooth(0.1, 0.2, f1)) * (v > 0.88) * smooth(0.4, 0.6, fbm(16, 2, 85))
    gold = np.clip(vein + nug, 0, 1)
    h = h + 0.2 * gold
    img = compose(base, h, cracks, col('#3a352e'))
    s = shade(h)[..., None]
    goldc = col('#d9a02a')[None, None, :] * (0.7 + 0.5 * s) + col('#fff1a8')[None, None, :] * np.clip(s - 0.75, 0, 1) * 2.5
    img = img * (1 - gold[..., None]) + np.clip(goldc, 0, 1) * gold[..., None]
    return np.clip(img, 0, 1)


def ore_coal():
    h, tone, cracks, grain = stone(19)
    yy = np.arange(N, dtype=np.float32)[:, None] / N
    warp = fbm(4, 4, 91)
    strata = np.sin((yy * 7 + warp * 2.2) * np.pi * 2) * 0.5 + 0.5
    seam = smooth(0.66, 0.82, strata) * (0.55 + 0.45 * smooth(0.35, 0.6, fbm(6, 3, 93)))
    chunks = smooth(0.66, 0.72, fbm(20, 3, 95))
    seam = np.maximum(seam, chunks)
    base = col('#3a3936')[None, None, :] * (0.85 + 0.3 * tone[..., None] + 0.25 * (grain[..., None] - 0.5))
    base = base * (1 - seam[..., None] * 0.85) + col('#0f0f11')[None, None, :] * seam[..., None] * 0.85
    h = h + 0.15 * seam
    img = compose(base, h, cracks, col('#0a0a0a'))
    glint = smooth(0.8, 0.95, shade(h)) * seam * (fbm(64, 2, 97) > 0.55)
    img = img + col('#7d8aa0')[None, None, :] * (glint * 0.6)[..., None]
    return np.clip(img, 0, 1)


def ore_diamond():
    h, tone, cracks, grain = stone(23)
    base = col('#aab2bd')[None, None, :] * (0.85 + 0.3 * tone[..., None] + 0.2 * (grain[..., None] - 0.5))
    img = compose(base, h, cracks, col('#4e5560'))
    f1, f2, v = voronoi(40, 101)
    spark = (1 - smooth(0.05, 0.13, f1)) * (v > 0.78)
    star = spark * 1.0
    img = img * (1 - star[..., None]) + col('#ffffff')[None, None, :] * star[..., None]
    f1b, _, vb = voronoi(14, 103)
    gem = (1 - smooth(0.1, 0.16, f1b)) * (vb > 0.86)
    s = shade(h + 0.3 * gem)[..., None]
    img = img * (1 - gem[..., None]) + np.clip(col('#7fc4ff')[None, None, :] * (0.6 + 0.6 * s), 0, 1) * gem[..., None]
    return np.clip(img, 0, 1)


KINDS = {'ore_rock': ore_rock, 'ore_iron': ore_iron, 'ore_gold': ore_gold, 'ore_coal': ore_coal, 'ore_diamond': ore_diamond}


def main():
    out = sys.argv[1]
    prev = sys.argv[2] if len(sys.argv) > 2 else None
    os.makedirs(out, exist_ok=True)
    tiles = []
    for name, f in KINDS.items():
        img = f()
        im = Image.fromarray((img * 255 + 0.5).astype(np.uint8), 'RGB')
        im.save(os.path.join(out, name + '.png'), optimize=True)
        tiles.append(im.resize((384, 384), Image.LANCZOS))
        print(name, flush=True)
    if prev:
        sheet = Image.new('RGB', (384 * len(tiles), 384 * 2))
        for i, t in enumerate(tiles):
            sheet.paste(t, (i * 384, 0))
            big = t.crop((0, 0, 192, 192)).resize((384, 384))   # a close-up (and the seam check: tiled 2x2 below)
            sheet.paste(big, (i * 384, 384))
        sheet.save(prev)


if __name__ == '__main__':
    main()

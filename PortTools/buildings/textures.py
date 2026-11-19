"""Tileable textures for the New Tides building kit (512 px). Each returns (rgb float HxWx3, team mask HxW or None)."""
import numpy as np

N = 512


def _grid_noise(size, px, py, rng):
    g = rng.rand(py, px)
    xs = np.arange(size) * px / size
    ys = np.arange(size) * py / size
    x0 = np.floor(xs).astype(int); fx = xs - x0; x1 = (x0 + 1) % px
    y0 = np.floor(ys).astype(int); fy = ys - y0; y1 = (y0 + 1) % py
    fx = fx * fx * (3 - 2 * fx); fy = fy * fy * (3 - 2 * fy)
    a = g[np.ix_(y0, x0)]; b = g[np.ix_(y0, x1)]; c = g[np.ix_(y1, x0)]; d = g[np.ix_(y1, x1)]
    FX = fx[None, :]; FY = fy[:, None]
    return (a * (1 - FX) + b * FX) * (1 - FY) + (c * (1 - FX) + d * FX) * FY


def noise(px, py=None, seed=0, octaves=4, size=N):
    """Periodic fbm: px, py lattice cells across the texture at the lowest octave."""
    py = px if py is None else py
    rng = np.random.RandomState(seed)
    s, a, t = 0, 1.0, 0
    for o in range(octaves):
        s = s + a * _grid_noise(size, px * 2 ** o, py * 2 ** o, rng)
        t += a
        a *= 0.5
    return s / t


def worley(cells, seed=0, size=N):
    """Periodic cellular noise: (distance to nearest point in cell units, its cell id)."""
    rng = np.random.RandomState(seed)
    pts = rng.rand(cells, cells, 2)
    ys, xs = np.mgrid[0:size, 0:size] * (cells / size)
    best = np.full((size, size), 9.0); second = np.full((size, size), 9.0); cid = np.zeros((size, size), int)
    ix = np.floor(xs).astype(int); iy = np.floor(ys).astype(int)
    for dy in (-1, 0, 1):
        for dx in (-1, 0, 1):
            cx = ix + dx; cy = iy + dy
            p = pts[cy % cells, cx % cells]
            d = np.hypot(xs - (cx + p[..., 0]), ys - (cy + p[..., 1]))
            m = d < best
            second = np.where(m, best, np.minimum(second, d))
            cid = np.where(m, (cy % cells) * cells + (cx % cells), cid)
            best = np.where(m, d, best)
    return best, second, cid


def C(h):
    h = h.lstrip('#')
    return np.array([int(h[i:i + 2], 16) / 255.0 for i in (0, 2, 4)])


def lerp(a, b, t):
    t = np.asarray(t)
    if t.ndim == 2:
        t = t[..., None]
    return a * (1 - t) + b * t


def smooth(e0, e1, x):
    t = np.clip((x - e0) / (e1 - e0), 0, 1)
    return t * t * (3 - 2 * t)


Y, X = np.mgrid[0:N, 0:N] / N   # v (down the image), u


# ------------------------------------------------------------------ wood

def planks(dark=False, seed=1):
    rows = 8
    row = np.floor(Y * rows).astype(int)
    fy = Y * rows - row
    rng = np.random.RandomState(seed)
    seams = rng.rand(rows)            # where each board's butt joint is
    tint = rng.rand(rows, 3)
    grain = noise(2, 24, seed + 3, 4)
    warp = noise(4, 4, seed + 5, 3)
    rings = np.sin((fy * 3 + warp * 6 + grain * 2) * np.pi * 2) * 0.5 + 0.5
    base = lerp(C('#8a5a32'), C('#b07a46'), rings * 0.6 + grain * 0.4)
    base = base * (0.85 + 0.3 * tint[row][..., :1])
    base = lerp(base, C('#6e4a2c'), tint[row][..., 1:2] * 0.35)
    # joints between boards and the butt joints along them
    gap = smooth(0.0, 0.06, fy) * smooth(1.0, 0.94, fy)
    su = (X - seams[row]) % 1.0
    butt = smooth(0.0, 0.006, su) * smooth(1.0, 0.994, su)
    base = base * (0.35 + 0.65 * (gap * butt))[..., None]
    # pegs near the joints
    for k in (0.025, 0.975):
        d = np.hypot(((su - k + 0.5) % 1 - 0.5) * 1.0, (fy - 0.5) / rows * 1.0)
        base = lerp(base, C('#2e2218'), smooth(0.009, 0.006, d))
    if dark:
        base = base * C('#7a6a62') * 1.1
    return base, None


def logs_vertical(seed=2):
    """Bark-free split logs (stave wall), vertical."""
    cols = 7
    col = np.floor(X * cols).astype(int)
    fx = X * cols - col
    rng = np.random.RandomState(seed)
    tint = rng.rand(cols)
    grain = noise(24, 2, seed, 4)
    base = lerp(C('#7a4e2a'), C('#a4703c'), grain)
    round_ = np.sin(fx * np.pi) ** 0.5
    base = base * (0.45 + 0.55 * round_)[..., None] * (0.85 + 0.3 * tint[col])[..., None]
    return base, None


def carved(seed=3, race='native'):
    """A carved, painted totem pole (v: bottom 1 .. top 0 of the image; wraps round in u)."""
    grain = noise(16, 3, seed, 4)
    base = lerp(C('#6b4022'), C('#93613a'), grain)
    paint = np.zeros((N, N)); col = np.zeros((N, N, 3))
    reds, yel, teal, blk, wht = C('#b5321e'), C('#e2b13a'), C('#2a9a8c'), C('#18120e'), C('#e8dcc0')
    faces = 4
    band = np.floor(Y * faces).astype(int); fy = Y * faces - band
    # one stacked face per band: brow, eyes, a mouth with teeth; bands alternate paint
    u = (X * 2) % 1.0   # two faces round the pole
    brow = (np.abs(fy - 0.22) < 0.05)
    eyes = (np.hypot((np.abs(u - 0.5) - 0.17) * 1.3, fy - 0.38) < 0.075)
    pupil = (np.hypot((np.abs(u - 0.5) - 0.17) * 1.3, fy - 0.38) < 0.03)
    mouth = (np.abs(fy - 0.68) < 0.07) & (np.abs(u - 0.5) < 0.28)
    teeth = mouth & (np.sin(u * 60) > 0.2) & (np.abs(fy - 0.68) < 0.03)
    nose = (np.abs(u - 0.5) < 0.05) & (fy > 0.4) & (fy < 0.58)
    zig = np.abs(((X * 12) % 1.0) - 0.5) * 0.12 + 0.9
    zigzag = np.abs(fy - zig) < 0.025
    for i, c in enumerate([reds, teal, yel, reds]):
        m = band == i
        base = lerp(base, c * (0.8 + 0.3 * grain[..., None]), (brow & m) * 0.9)
        base = lerp(base, wht, (eyes & m) * 0.9)
        base = lerp(base, blk, (pupil & m))
        base = lerp(base, blk, (mouth & m) * 0.85)
        base = lerp(base, wht, (teeth & m))
        base = lerp(base, c * 0.8, (nose & m) * 0.8)
        base = lerp(base, yel if i % 2 else teal, (zigzag & m) * 0.9)
    # carving grooves: darken just above/below the bands
    groove = smooth(0.03, 0.0, fy) + smooth(0.97, 1.0, fy)
    base = base * (1 - 0.5 * np.clip(groove, 0, 1))[..., None]
    return base, None


# ------------------------------------------------------------------ roofs

def thatch(seed=4):
    rows = 6
    row = np.floor(Y * rows).astype(int); fy = Y * rows - row
    strands = noise(96, 3, seed, 3)
    strands2 = noise(160, 2, seed + 7, 2)
    clump = noise(6, 6, seed + 9, 3)
    straw = lerp(C('#8c6a34'), C('#c9a35a'), np.clip(strands * 0.7 + strands2 * 0.5 - 0.1, 0, 1))
    straw = lerp(straw, C('#6e6a58'), clump * 0.35)   # weathered grey
    # each course: lighter at the bottom tips, shadowed at the top where the course above overlaps
    shade = 0.55 + 0.45 * smooth(0.0, 0.7, fy)
    tips = smooth(0.8, 1.0, fy) * (strands2 > 0.5)
    base = straw * shade[..., None]
    base = lerp(base, C('#d8b46a'), tips * 0.4)
    return base, None


def shingles(seed=5):
    rows = 10
    row = np.floor(Y * rows).astype(int); fy = Y * rows - row
    cols = 6
    off = (row % 2) * 0.5
    cx = (X * cols + off); col = np.floor(cx).astype(int); fx = cx - col
    rng = np.random.RandomState(seed)
    tint = rng.rand(rows, cols + 1)
    grain = noise(48, 4, seed, 3)
    base = lerp(C('#5e4a36'), C('#8a7154'), grain)
    base = base * (0.8 + 0.35 * tint[row, col % (cols + 1)])[..., None]
    edge = smooth(0.0, 0.05, fx) * smooth(1.0, 0.95, fx)
    shadow = 0.5 + 0.5 * smooth(0.0, 0.35, fy)
    rounded = 1 - smooth(0.85, 1.0, fy) * (np.abs(fx - 0.5) > 0.42)
    base = base * (shadow * (0.5 + 0.5 * edge) * rounded)[..., None]
    lichen = (noise(10, 10, seed + 2, 3) > 0.68)
    base = lerp(base, C('#8f9a5a'), lichen * 0.35)
    return base, None


def turf(seed=6):
    g = noise(64, 64, seed, 4)
    clumps = noise(8, 8, seed + 1, 3)
    grass = lerp(C('#3d5a22'), C('#6f8f38'), g)
    grass = lerp(grass, C('#8a8a48'), smooth(0.6, 0.8, clumps) * 0.6)   # dry patches
    blades = noise(160, 40, seed + 4, 2)
    grass = grass * (0.8 + 0.35 * blades)[..., None]
    return grass, None


# ------------------------------------------------------------------ stone, clay

def stone_blocks(seed=7):
    rows = 6
    row = np.floor(Y * rows).astype(int); fy = Y * rows - row
    rng = np.random.RandomState(seed)
    # each row: block boundaries at random positions (wrapping)
    cuts = [np.sort(rng.rand(rng.randint(3, 5))) for _ in range(rows)]
    blockid = np.zeros((N, N), int); ver = np.ones((N, N))
    for r in range(rows):
        m = row == r
        c = cuts[r]
        u = X[m]
        idx = np.searchsorted(c, u) % len(c)
        prev = c[idx - 1]; nxt = c[idx]
        ver[m] = np.minimum((u - prev) % 1.0, (nxt - u) % 1.0)
        blockid[m] = r * 10 + idx
    tint = rng.rand(rows * 10 + 10)
    n = noise(24, 24, seed + 1, 4)
    chips = noise(64, 64, seed + 2, 2)
    base = lerp(C('#8b8478'), C('#b5ad9c'), n * 0.7 + chips * 0.3)
    base = base * (0.78 + 0.32 * tint[blockid])[..., None]
    base = lerp(base, C('#a39070'), (tint[blockid] > 0.8) * 0.4)   # a few warmer stones
    hor = np.minimum(fy, 1 - fy) / rows
    edge = np.minimum(hor, ver)
    mortar = smooth(0.003, 0.007, edge)
    bevel = smooth(0.004, 0.03, edge)
    base = base * (0.7 + 0.3 * bevel)[..., None]
    base = lerp(C('#5e584e') * (0.8 + 0.3 * n[..., None]), base, mortar)
    return base, None


def fieldstone(seed=8):
    d, d2, cid = worley(8, seed)
    rng = np.random.RandomState(seed)
    tint = rng.rand(64)
    n = noise(32, 32, seed + 1, 4)
    base = lerp(C('#77736a'), C('#a59f90'), n)
    base = base * (0.75 + 0.4 * tint[cid])[..., None]
    edge = d2 - d
    gap = smooth(0.02, 0.12, edge)
    round_ = smooth(0.0, 0.5, edge)
    base = base * (0.55 + 0.45 * round_)[..., None]
    base = lerp(C('#3a372f'), base, gap)
    moss = (noise(6, 6, seed + 3, 3) > 0.62) & (gap < 0.9)
    base = lerp(base, C('#4f6a2a'), moss * 0.6)
    return base, None


def adobe(seed=9):
    n = noise(8, 8, seed, 5)
    fine = noise(64, 64, seed + 1, 3)
    base = lerp(C('#a77a4e'), C('#c99d6a'), n)
    base = base * (0.9 + 0.15 * fine)[..., None]
    # hand-smoothed streaks and a few cracks
    cracks = np.abs(noise(6, 6, seed + 2, 4) - 0.5) < 0.008
    base = lerp(base, C('#6a4a2e'), cracks * 0.8)
    # a painted band (white with red triangles) round the bottom quarter
    band = (Y > 0.78) & (Y < 0.86)
    tri = (((X * 16) % 1.0) < (Y - 0.78) / 0.08)
    base = lerp(base, C('#e6dcc4'), band * 0.85)
    base = lerp(base, C('#a5321e'), (band & tri) * 0.9)
    return base, None


def runestone(seed=10):
    """A Viking rune stone: grey granite, lichen, a carved serpent band painted red with runes inside it."""
    n = noise(32, 32, seed, 5)
    speck = noise(200, 200, seed + 1, 1)
    base = lerp(C('#6f6c66'), C('#9c978c'), n)
    base = lerp(base, C('#3a3834'), (speck > 0.8) * 0.4)
    base = lerp(base, C('#d8d2c4'), (speck < 0.12) * 0.3)
    lich, _, _ = worley(10, seed + 2)
    base = lerp(base, C('#b8b45a'), (lich < 0.12) * (noise(4, 4, seed + 3, 2) > 0.55) * 0.6)
    # the serpent: a band snaking up the face (x across the face, y up), its body red, edges carved dark
    cx = 0.5 + 0.28 * np.sin(Y * np.pi * 2 * 1.5)
    d = np.abs(X - cx)
    body = d < 0.07
    edge = (d > 0.07) & (d < 0.085)
    base = lerp(base, C('#a72a1a') * (0.8 + 0.3 * n[..., None]), body * 0.9)
    base = lerp(base, C('#2a2622'), edge * 0.9)
    # runes inside the band: short strokes
    t = (Y * 28) % 1.0
    stroke = body & (((np.abs(X - cx) < 0.012)) | (np.abs((X - cx) - (t - 0.5) * 0.08) < 0.01)) & (t > 0.15) & (t < 0.85)
    base = lerp(base, C('#1c1814'), stroke * 0.95)
    return base, None


# ------------------------------------------------------------------ hides, cloth, banners

def hide(seed=11):
    n = noise(6, 6, seed, 5)
    blot = noise(3, 3, seed + 1, 3)
    base = lerp(C('#b88d5c'), C('#dcc08e'), n)
    base = lerp(base, C('#8d6640'), smooth(0.55, 0.8, blot) * 0.5)
    mask = np.zeros((N, N))
    # painted bands from the bottom (v: image bottom = 1): a team band, red zig-zags, black triangles, red dots
    tband = (Y > 0.80) & (Y < 0.88)
    mask[tband] = 0.85
    z = 0.68 + 0.05 * np.abs(((X * 12) % 1.0) - 0.5) * 2
    zig = np.abs(Y - z) < 0.012
    base = lerp(base, C('#a5281c'), zig * 0.95)
    tri = (Y > 0.55) & (Y < 0.62) & (((X * 10) % 1.0) < (0.62 - Y) / 0.07 * 0.5 + 0.25) & (((X * 10) % 1.0) > 0.75 - (0.62 - Y) / 0.07 * 0.5)
    base = lerp(base, C('#1e1612'), tri * 0.9)
    dots = (np.hypot(((X * 20) % 1.0) - 0.5, (Y - 0.45) * 20) < 0.25)
    base = lerp(base, C('#a5281c'), dots * 0.9)
    # sun emblems higher up
    su = (X * 4) % 1.0
    r = np.hypot((su - 0.5) * 4, (Y - 0.28) * 16)
    base = lerp(base, C('#d99a2a'), (r < 0.55) * 0.9)
    rays = (r > 0.6) & (r < 0.95) & (np.sin(np.arctan2(Y - 0.28, (su - 0.5) / 4) * 12) > 0.3)
    base = lerp(base, C('#d99a2a'), rays * 0.8)
    # seams where hides are stitched
    seam = np.abs(((X * 6) % 1.0) - 0.5) > 0.49
    base = lerp(base, C('#5e4228'), seam * 0.7)
    return base, mask


def cloth(seed=12):
    """Awning cloth: wide stripes, every other one in the team colour, with a hem."""
    weave = noise(64, 64, seed, 1) * 0.5 + noise(32, 2, seed + 1, 1) * 0.5
    stripes = 6
    s = np.floor(X * stripes).astype(int)
    team = (s % 2) == 0
    base = np.where(team[..., None], C('#e8e0d0'), C('#ece4cf'))
    base = base * (0.93 + 0.08 * weave)[..., None]
    mask = np.where(team, 0.82, 0.0)
    # thin red line at each stripe edge, a fringe at the hem (v near 1)
    fx = X * stripes - s
    line = (np.abs(fx - 0.03) < 0.012) | (np.abs(fx - 0.97) < 0.012)
    base = lerp(base, C('#9b2a1c'), line * 0.9)
    mask = np.where(line, 0, mask)
    hem = Y > 0.92
    base = lerp(base, C('#9b2a1c'), hem * 0.9)
    mask = np.where(hem, 0, mask)
    fringe = (Y > 0.96) & ((X * 64) % 1.0 > 0.5)
    base = lerp(base, C('#5e1a12'), fringe)
    folds = 0.92 + 0.08 * np.sin(X * np.pi * 2 * 12)
    base = base * folds[..., None]
    return base, mask


def banner(seed=13):
    """A war banner: the team colour with a dark emblem (a ring of rays) and a border."""
    weave = noise(64, 64, seed, 1)
    base = C('#e8e2d6') * (0.94 + 0.08 * weave)[..., None]
    mask = np.full((N, N), 0.9)
    r = np.hypot(X - 0.5, (Y - 0.42) * 0.9)
    ring = (r > 0.12) & (r < 0.17)
    rays = (r > 0.2) & (r < 0.3) & (np.sin(np.arctan2(Y - 0.42, X - 0.5) * 8) > 0.5)
    core = r < 0.07
    emb = ring | rays | core
    base = lerp(base, C('#1c1612'), emb * 0.95)
    mask = np.where(emb, 0.0, mask)
    border = (X < 0.06) | (X > 0.94) | (Y < 0.05)
    base = lerp(base, C('#d4a640'), border * 0.95)
    mask = np.where(border, 0.0, mask)
    tails = Y > 0.82
    mask = np.where(tails & (np.abs(X - 0.5) < 0.12), 0.0, mask)
    return base, mask


def feathers(seed=14):
    """Rows of feathers (wings of the War Totem): team tips, white-and-brown barbs."""
    rows = 4
    row = np.floor(Y * rows).astype(int); fy = Y * rows - row
    cols = 8
    off = (row % 2) * 0.5
    cx = X * cols + off; fx = cx - np.floor(cx)
    shape = np.abs(fx - 0.5) * 2 < (1 - fy) ** 0.6 * 0.95
    barbs = np.sin((fx - 0.5) * 40 + fy * 30) * 0.5 + 0.5
    base = lerp(C('#f0ead8'), C('#7a5232'), smooth(0.35, 0.55, fy))
    base = base * (0.85 + 0.2 * barbs)[..., None]
    base = np.where(shape[..., None], base, C('#3a2a1c'))
    shaft = np.abs(fx - 0.5) < 0.02
    base = lerp(base, C('#2a1e14'), shaft * 0.8)
    mask = np.where(shape & (fy < 0.3), 0.85, 0.0)
    return base, mask


def iron(seed=15):
    n = noise(16, 16, seed, 5)
    base = lerp(C('#3c4046'), C('#626870'), n)
    rust = smooth(0.6, 0.8, noise(5, 5, seed + 1, 4))
    base = lerp(base, C('#6e4228'), rust * 0.45)
    # rivet rows
    r = np.hypot(((X * 8) % 1.0) - 0.5, ((Y * 4) % 1.0) - 0.5)
    base = lerp(base, C('#9aa0a8'), smooth(0.12, 0.08, r))
    base = lerp(base, C('#202226'), (smooth(0.16, 0.13, r) - smooth(0.12, 0.09, r)).clip(0, 1) * 0.8)
    return base, None


def clay(seed=16):
    """Pottery: terracotta with painted black and white bands."""
    n = noise(8, 8, seed, 4)
    base = lerp(C('#9a4e2a'), C('#c06a3a'), n)
    b1 = (np.abs(Y - 0.3) < 0.04); b2 = (np.abs(Y - 0.5) < 0.015)
    zz = np.abs(Y - (0.42 + 0.04 * np.abs(((X * 10) % 1.0) - 0.5) * 2)) < 0.012
    base = lerp(base, C('#1c1410'), (b1 | zz) * 0.9)
    base = lerp(base, C('#e2d4b4'), b2 * 0.9)
    return base, None


def goods(seed=17):
    """Market goods, a 4x4 atlas of tileable patches: grain, fruit, fish, furs, cloth bolts, amber, pots, apples..."""
    base = np.zeros((N, N, 3))
    k = 4; s = N // k
    rng = np.random.RandomState(seed)
    cols = ['#d9b45a', '#c0392b', '#e67e22', '#8e5a3a', '#6b8e23', '#f1c40f', '#7f8c8d', '#2e86c1',
            '#a04000', '#d4ac0d', '#5d4037', '#e8daef', '#b03a2e', '#239b56', '#f0b27a', '#85929e']
    n = noise(32, 32, seed, 3)
    d, d2, cid = worley(16, seed + 1)
    for i in range(k * k):
        r, c = divmod(i, k)
        sl = (slice(r * s, (r + 1) * s), slice(c * s, (c + 1) * s))
        cc = C(cols[i])
        lumps = smooth(0.0, 0.25, (d2 - d)[sl])
        patch = cc * (0.55 + 0.45 * lumps)[..., None] * (0.85 + 0.25 * n[sl])[..., None]
        base[sl] = patch
    return base, None


TEXTURES = {
    'bk_planks': planks, 'bk_darkwood': lambda: planks(dark=True, seed=21), 'bk_staves': logs_vertical,
    'bk_carved': carved, 'bk_thatch': thatch, 'bk_shingles': shingles, 'bk_turf': turf,
    'bk_stone': stone_blocks, 'bk_fieldstone': fieldstone, 'bk_adobe': adobe, 'bk_runestone': runestone,
    'bk_hide': hide, 'bk_cloth': cloth, 'bk_banner': banner, 'bk_feathers': feathers, 'bk_iron': iron,
    'bk_clay': clay, 'bk_goods': goods,
}


# ------------------------------------------------------------------ shields (the champions' and guards' shields)

def shield_viking():
    """The Viking warrior's own painted shield (cut out of his texture: iron-rimmed boards, team-coloured), on a disc."""
    from PIL import Image as _I
    base = '/home/claude/omaramokhtar/tribaltrouble/assets/textures/'
    im = _I.open(base + 'models/viking_warrior_iron.png').convert('RGB')
    tm = _I.open(base + 'teamdecals/viking_warrior_iron_team.png').convert('L').resize(im.size)
    cx, cy, r = 728, 910, 96
    rgb = np.asarray(im.crop((cx - r, cy - r, cx + r, cy + r)).resize((N, N), _I.LANCZOS)).astype(float) / 255
    mask = np.asarray(tm.crop((cx - r, cy - r, cx + r, cy + r)).resize((N, N), _I.LANCZOS)).astype(float) / 255
    # outside the octagon: the rim's colour, so the round disc's edge reads as rim
    rr = np.hypot(X - .5, Y - .5)
    rim = rgb[int(N * .5), int(N * .04)]
    rgb = np.where((rr > .47)[..., None], rim * (0.9 + 0.1 * noise(32, 32, 3, 2))[..., None], rgb)
    mask = np.where(rr > .47, 0, np.clip(mask * 1.2, 0, .9))
    return rgb, mask


def shield_native():
    """A Native hide shield in the warriors' colours: tan hide, a dark blue rim with white dots, red quarters, a team ring."""
    n = noise(6, 6, 41, 4)
    rr = np.hypot(X - .5, Y - .5) * 2
    ang = np.arctan2(Y - .5, X - .5)
    base = lerp(C('#b88d5c'), C('#d9b98a'), n)
    mask = np.zeros((N, N))
    rim = rr > .86
    base = lerp(base, C('#1e2c4a') * (0.85 + 0.2 * n[..., None]), rim)
    dots = rim & (np.abs(rr - .93) < .03) & (np.cos(ang * 16) > .7)
    base = lerp(base, C('#ece6d6'), dots)
    team = (rr > .62) & (rr < .76)
    base = lerp(base, C('#e8e0d0'), team)
    mask = np.where(team, .85, 0)
    quarters = (rr < .58) & (np.cos(ang * 2) > .55)
    base = lerp(base, C('#a5321e'), quarters * .9)
    boss = rr < .16
    base = lerp(base, C('#1e2c4a'), boss)
    base = lerp(base, C('#5e4228'), (np.abs(rr - .6) < .012) | (np.abs(rr - .78) < .012))
    return base, mask


TEXTURES['bk_shield_viking'] = shield_viking
TEXTURES['bk_shield_native'] = shield_native

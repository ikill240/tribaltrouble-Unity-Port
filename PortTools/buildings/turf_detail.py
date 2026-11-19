"""Turf roof texture with detail: the sod laid in overlapping strips (a shaded lip along each), moss and dry patches,
tufts, a few wildflowers and stones. Tileable. Writes over bk_turf.png (the old one, a plain field of grass, made the
Mead Hall's roof read as a flat green slab).
usage: python3 turf_detail.py <bk_turf.png in> <out.png>  (run after build.py: textures.py makes the plain field it starts from)"""
import sys
import numpy as np
from PIL import Image

N = 512
rng = np.random.default_rng(7)


def tnoise(cells, seed):
    """Tileable value noise: a cells x cells random grid, wrapped, smoothly interpolated to N x N."""
    g = np.random.default_rng(seed).random((cells, cells))
    x = np.arange(N) * cells / N
    i0 = np.floor(x).astype(int); f = x - i0; f = f * f * (3 - 2 * f); i1 = (i0 + 1) % cells
    a = g[i0][:, i0] * (1 - f)[None, :] + g[i0][:, i1] * f[None, :]
    b = g[i1][:, i0] * (1 - f)[None, :] + g[i1][:, i1] * f[None, :]
    return a * (1 - f)[:, None] + b * f[:, None]


def fbm(seed, base=4, octaves=4):
    s, a, t = 0, 1.0, 0
    for o in range(octaves):
        s = s + a * tnoise(base * 2 ** o, seed + o * 31); t += a; a *= 0.5
    return s / t


src = np.asarray(Image.open(sys.argv[1]).convert('RGB')).astype(float) / 255
img = src.copy()
lum = img.mean(-1, keepdims=True)
# colour patches: dark moss, fresh green, dry straw
n1, n2 = fbm(1, 3), fbm(2, 5)
moss = np.array([0.16, 0.22, 0.09]); fresh = np.array([0.3, 0.4, 0.15]); straw = np.array([0.5, 0.45, 0.26])
tint = fresh[None, None] * (1 - n1[..., None]) + moss[None, None] * n1[..., None]
dry = np.clip((n2 - 0.58) * 4, 0, 1)[..., None]
tint = tint * (1 - dry) + straw[None, None] * dry
img = np.clip(tint * (0.5 + 0.75 * lum / lum.mean()), 0, 1)
img = img * (0.95 + 0.3 * fbm(4, 2)[..., None])   # big soft light and dark areas
# the sod strips: 8 rows down the roof, each overlapping the next with a shaded lip and a lighter top edge
y = np.arange(N)[:, None] + (tnoise(6, 9) * 10 - 5)          # wavy rows
row = (y % 64) / 64.0
lip = np.exp(-((row - 0.02) / 0.04) ** 2)                   # the dark shadow under the strip above
top = np.exp(-((row - 0.12) / 0.06) ** 2)                   # the light edge of this strip
shade = 1 - 0.32 * lip[..., None] + 0.08 * top[..., None]
# ... and the strips broken into sods (vertical joins, offset each row)
rowi = (y // 64).astype(int)
x = (np.arange(N)[None, :] + rowi * 37 + tnoise(5, 11) * 8) % 96
join = np.exp(-((x - 1) / 1.6) ** 2) * (row > 0.08)
shade = shade * (1 - 0.12 * join[..., None])
img = np.clip(img * shade, 0, 1)
# tufts: small bright blade clusters
for _ in range(900):
    cx, cy = rng.integers(0, N, 2); c = np.array([0.42, 0.52, 0.2]) * rng.uniform(0.8, 1.2)
    for k in range(5):
        dx = rng.integers(-3, 4); L = rng.integers(3, 8)
        for t in range(L):
            px, py = (cx + dx + t * rng.integers(-1, 2) // 2) % N, (cy - t) % N
            img[py, px] = img[py, px] * 0.4 + c * 0.6
# wildflowers and a few stones
for _ in range(70):
    cx, cy = rng.integers(0, N, 2)
    c = [np.array([0.95, 0.9, 0.85]), np.array([0.95, 0.82, 0.25]), np.array([0.75, 0.45, 0.85])][rng.integers(0, 3)]
    for dy in range(-1, 2):
        for dx in range(-1, 2):
            if abs(dx) + abs(dy) < 2: img[(cy + dy) % N, (cx + dx) % N] = c
for _ in range(45):
    cx, cy = rng.integers(0, N, 2); r = rng.uniform(2.5, 5.5); g = rng.uniform(0.45, 0.6)
    for dy in range(-6, 7):
        for dx in range(-6, 7):
            d = (dx / r) ** 2 + (dy / (r * 0.75)) ** 2
            if d < 1: img[(cy + dy) % N, (cx + dx) % N] = np.array([g, g, g * 0.96]) * (1.1 - 0.35 * d) * (0.85 if dy > 0 else 1.05)
Image.fromarray((np.clip(img, 0, 1) * 255 + 0.5).astype(np.uint8)).save(sys.argv[2])
print('ok')

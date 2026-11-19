"""Tileable detail textures for Tribal Trouble's "Modern" look.

Every texture is periodic (spectral synthesis / wrapped Worley), so it tiles without seams.
<m>_detail.png : RGB albedo modulation (128 = x1.0, multiply by 2*v), A = height
<m>_normal.png : RGB tangent-space normal (OpenGL, +Y up), A = roughness
"""
import numpy as np
from numpy.fft import fft2, ifft2, fftfreq
from PIL import Image
import os, sys

OUT = sys.argv[1] if len(sys.argv) > 1 else 'out'
os.makedirs(OUT, exist_ok=True)
N = int(__import__("os").environ.get("TTN", "1024"))


def rng(seed):
    return np.random.default_rng(seed)


def spectral(n, seed, beta=2.0, fmin=1, fmax=None, aniso=None):
    """Periodic fBm by filtering white noise with 1/f^beta (optionally elongated along an angle)."""
    r = rng(seed)
    w = r.standard_normal((n, n))
    fx = fftfreq(n) * n
    FX, FY = np.meshgrid(fx, fx)
    if aniso is not None:
        ang, ratio = aniso
        c, s = np.cos(ang), np.sin(ang)
        u = FX * c + FY * s
        v = -FX * s + FY * c
        f = np.sqrt(u * u + (v * ratio) ** 2)
    else:
        f = np.sqrt(FX * FX + FY * FY)
    f[0, 0] = 1
    filt = 1.0 / f ** (beta / 2)
    filt[f < fmin] = 0
    if fmax:
        filt *= np.exp(-(f / fmax) ** 2)
    filt[0, 0] = 0
    h = np.real(ifft2(fft2(w) * filt))
    h -= h.mean()
    h /= (h.std() + 1e-9)
    return h


def norm01(a):
    a = a - a.min()
    return a / (a.max() + 1e-9)


def worley(n, cells, seed, jitter=1.0):
    """Wrapped Worley noise: F1, F2 distances (in cell units) and the id of the nearest cell."""
    r = rng(seed)
    pts = (r.random((cells, cells, 2)) - 0.5) * jitter + 0.5
    ids = r.random((cells, cells))
    y, x = np.mgrid[0:n, 0:n] / n * cells
    cx, cy = np.floor(x).astype(int), np.floor(y).astype(int)
    f1 = np.full((n, n), 9.0); f2 = np.full((n, n), 9.0); idn = np.zeros((n, n))
    for dy in (-1, 0, 1):
        for dx in (-1, 0, 1):
            gx = (cx + dx) % cells; gy = (cy + dy) % cells
            px = cx + dx + pts[gy, gx, 0]; py = cy + dy + pts[gy, gx, 1]
            d = np.sqrt((x - px) ** 2 + (y - py) ** 2)
            closer = d < f1
            f2 = np.where(closer, f1, np.minimum(f2, d))
            idn = np.where(closer, ids[gy, gx], idn)
            f1 = np.where(closer, d, f1)
    return f1, f2, idn


def normal_from_height(h, strength):
    """Tangent-space normal from a periodic height map (h in 0..1, strength = height in texels at h=1)."""
    strength = strength * h.shape[0] / 1024.0   # same slopes at any resolution
    dx = (np.roll(h, -1, 1) - np.roll(h, 1, 1)) * 0.5 * strength
    dy = (np.roll(h, -1, 0) - np.roll(h, 1, 0)) * 0.5 * strength
    # image rows go down, texture v goes up: flip dy for OpenGL convention
    nx, ny, nz = -dx, dy, np.ones_like(h)
    l = np.sqrt(nx * nx + ny * ny + nz * nz)
    return np.stack([nx / l, ny / l, nz / l], -1)


def save(name, alb_mod, height, normal, rough):
    # a modulation must not shift the base colour on average: normalise each channel's mean to x1.0
    alb_mod = alb_mod / alb_mod.reshape(-1, 3).mean(0)
    a = np.clip(alb_mod * 0.5, 0, 1)
    img = np.concatenate([a, height[..., None]], -1)
    Image.fromarray((np.clip(img, 0, 1) * 255 + 0.5).astype(np.uint8), 'RGBA').save(f'{OUT}/{name}_detail.png')
    nm = np.concatenate([normal * 0.5 + 0.5, rough[..., None]], -1)
    Image.fromarray((np.clip(nm, 0, 1) * 255 + 0.5).astype(np.uint8), 'RGBA').save(f'{OUT}/{name}_normal.png')


def tint(lum, r=1, g=1, b=1):
    return np.stack([lum * r, lum * g, lum * b], -1)


# ---------------------------------------------------------------- sand: wind ripples + grains
def sand():
    y, x = np.mgrid[0:N, 0:N] / N
    warp = spectral(N, 11, beta=3.5, fmax=6) * 0.035
    phase = (x * 22 + y * 5 + warp * 22)
    rip = np.sin(2 * np.pi * phase)
    rip = np.sign(rip) * np.abs(rip) ** 0.7          # sharper crests
    rip_amp = norm01(spectral(N, 12, beta=3, fmax=4)) * 0.8 + 0.2
    grain = spectral(N, 13, beta=0.6, fmin=120)
    h = norm01(rip * rip_amp ** 2 * 0.45 + grain * 0.12 + spectral(N, 14, beta=3, fmax=8) * 0.5)
    speck = spectral(N, 15, beta=0.2, fmin=200)
    dark = (speck > 2.2) * 0.25
    lum = 1.0 + (h - 0.5) * 0.10 + speck * 0.035 - dark
    alb = tint(lum, 1.0, 0.995, 0.985)
    save('sand', alb, h, normal_from_height(h, 6), np.full_like(h, 0.9))


# ---------------------------------------------------------------- grass: blades in clumps
def grass():
    h = np.zeros((N, N))
    clumps = norm01(spectral(N, 21, beta=3, fmax=12))
    for k, ang in enumerate(np.linspace(0, np.pi, 9, endpoint=False)):
        blades = spectral(N, 30 + k, beta=0.9, fmin=60, aniso=(ang + 0.2 * k, 9.0))
        mask = norm01(spectral(N, 40 + k, beta=3.5, fmax=10))
        mask = np.clip((mask - 0.5) * 5 + 0.5, 0, 1)
        h = np.maximum(h, np.maximum(blades, 0) * mask * (0.7 + 0.3 * np.cos(k)))
    h = norm01(h * (0.5 + 0.8 * clumps))
    h = h ** 0.8
    dry = np.clip((norm01(spectral(N, 23, beta=3, fmax=6)) - 0.45) * 2.5, 0, 1)
    tips = np.clip((h - 0.6) * 2.5, 0, 1)
    lum = 0.55 + h * 0.75
    col = tint(lum, 1, 1, 1)
    # sun-bleached tips, dry patches (yellow), dark soil showing between blades
    col *= (1 + np.stack([tips * 0.12 + dry * 0.35, tips * 0.10 + dry * 0.18, -dry * 0.15], -1))
    save('grass', col, h, normal_from_height(h, 14), 0.8 + 0.15 * (1 - h))


# ---------------------------------------------------------------- dirt / soil: clods + small stones
def dirt(name='dirt', seed=0):
    base = spectral(N, 50 + seed, beta=2.2, fmax=200)
    f1, f2, idn = worley(N, 48, 51 + seed)
    stones = np.clip(1 - f1 / 0.35, 0, 1) ** 0.6 * (idn > 0.72)
    cracks = np.clip(1 - (f2 - f1) / 0.06, 0, 1) * (norm01(spectral(N, 52 + seed, beta=3, fmax=8)) > 0.55)
    h = norm01(base * 0.25 + stones * 0.9 - cracks * 0.35 + spectral(N, 53 + seed, beta=0.8, fmin=150) * 0.05)
    lum = 1 + base * 0.07 + stones * (idn - 0.85) * 1.2 - cracks * 0.25
    col = tint(lum, 1, 0.99, 0.97)
    save(name, col, h, normal_from_height(h, 12), 0.95 - stones * 0.15)


# ---------------------------------------------------------------- gravel / rubble: packed pebbles
def gravel():
    h = np.zeros((N, N)); lum = np.zeros((N, N)); hue = np.zeros((N, N))
    for k, cells in enumerate((40, 70, 24)):
        f1, f2, idn = worley(N, cells, 60 + k, jitter=0.9)
        pebble = np.clip(1 - (f1 / np.maximum(f2, 1e-3)) ** 1.6, 0, 1)
        pebble = np.sqrt(pebble)
        w = (0.6, 0.35, 0.8)[k]
        top = pebble * w * (0.7 + idn * 0.6)
        sel = top > h
        h = np.maximum(h, top)
        lum = np.where(sel, 0.8 + idn * 0.45, lum)
        hue = np.where(sel, idn, hue)
    h = norm01(h + spectral(N, 64, beta=0.8, fmin=200) * 0.03)
    gaps = np.clip(1 - h * 3, 0, 1)
    lum = lum * (1 - gaps * 0.55)
    col = np.stack([lum * (1 + (hue - 0.5) * 0.12), lum, lum * (1 - (hue - 0.5) * 0.12)], -1)
    save('gravel', col, h, normal_from_height(h, 14), 0.7 + gaps * 0.25)


# ---------------------------------------------------------------- rock: ridged fbm, cracks, strata
def rock():
    y, x = np.mgrid[0:N, 0:N] / N
    warp = spectral(N, 73, beta=3, fmax=6)
    # weathered layered stone: warped strata, ridged fbm, a few deep fractures, blocky facets
    strata = np.sin(2 * np.pi * (y * 9 + warp * 0.6 + spectral(N, 76, beta=2.5, fmax=30) * 0.05))
    strata = np.sign(strata) * np.abs(strata) ** 0.4
    ridged = 1 - np.abs(spectral(N, 70, beta=2.2, fmax=160))
    f1, f2, idn = worley(N, 7, 71)
    fract = np.clip(1 - (f2 - f1) / 0.025, 0, 1) * np.clip((norm01(spectral(N, 77, beta=3, fmax=8)) - 0.55) * 4, 0, 1)
    facets = norm01(spectral(N, 79, beta=2.8, fmax=20)) * 0.6
    h = norm01(strata * 0.25 + ridged * 0.5 + facets * 0.5 - fract * 0.6 + spectral(N, 74, beta=0.9, fmin=120) * 0.07)
    lichen = np.clip((norm01(spectral(N, 75, beta=2.5, fmax=40)) - 0.7) * 5, 0, 1) * (h > 0.5)
    stain = norm01(spectral(N, 78, beta=3, fmax=12))
    lum = 0.7 + h * 0.5 - fract * 0.35 + (stain - 0.5) * 0.2
    col = tint(lum, 1.0, 0.99, 0.97)
    col = col * (1 - lichen[..., None] * 0.3) + lichen[..., None] * np.array([0.75, 0.76, 0.55]) * 0.3 * lum[..., None]
    save('rock', col, h, normal_from_height(h, 18), 0.7 + fract * 0.25)


# ---------------------------------------------------------------- snow: wind-sculpted, sparkly
def snow():
    y, x = np.mgrid[0:N, 0:N] / N
    warp = spectral(N, 81, beta=3, fmax=5) * 0.06
    sastrugi = np.abs(np.sin(2 * np.pi * (x * 9 + y * 3 + warp * 9))) ** 3
    h = norm01(spectral(N, 80, beta=3, fmax=30) * 0.6 + sastrugi * 0.35 * norm01(spectral(N, 82, beta=3, fmax=6))
               + spectral(N, 83, beta=1, fmin=150) * 0.04)
    sparkle = (spectral(N, 84, beta=0, fmin=300) > 2.6).astype(float)
    lum = 0.96 + h * 0.06 + sparkle * 0.06
    col = tint(lum, 0.99, 1.0, 1.02)
    save('snow', col, h, normal_from_height(h, 6), 0.5 - sparkle * 0.3)


# ---------------------------------------------------------------- water: Phillips-spectrum height -> normal
def water():
    n = 512
    r = rng(90)
    fx = fftfreq(n) * n
    KX, KY = np.meshgrid(fx, fx)
    k = np.sqrt(KX ** 2 + KY ** 2); k[0, 0] = 1
    wind = np.array([1.0, 0.3]); wind /= np.linalg.norm(wind)
    L = 18.0
    ph = np.exp(-1 / (k * k / L / L * 40)) / k ** 4 * (np.abs((KX * wind[0] + KY * wind[1]) / k) ** 2 * 0.8 + 0.2)
    ph *= np.exp(-(k / 120) ** 2)
    ph[0, 0] = 0
    h0 = (r.standard_normal((n, n)) + 1j * r.standard_normal((n, n))) * np.sqrt(ph)
    h = np.real(ifft2(h0)); h = norm01(h)
    nm = normal_from_height(h, 18)
    img = np.concatenate([nm * 0.5 + 0.5, h[..., None]], -1)
    Image.fromarray((np.clip(img, 0, 1) * 255 + 0.5).astype(np.uint8), 'RGBA').save(f'{OUT}/water_normal.png')
    # foam: bubbly cells with holes
    f1, f2, idn = worley(n, 40, 91)
    f1b, f2b, _ = worley(n, 90, 92)
    foam = np.clip((f2 - f1) * 4, 0, 1) * 0.6 + np.clip((f2b - f1b) * 5, 0, 1) * 0.5
    foam *= norm01(spectral(n, 93, beta=2, fmax=20)) * 0.7 + 0.5
    foam = norm01(foam) ** 1.5
    # caustics: sharp bright network
    # curvy caustic network: ridged band-limited noise, two octaves
    r1 = 1 - np.abs(spectral(n, 94, beta=1.0, fmin=6, fmax=14)) / 2.2
    r2 = 1 - np.abs(spectral(n, 96, beta=1.0, fmin=12, fmax=26)) / 2.2
    caus = np.clip(r1, 0, 1) ** 10 * 0.8 + np.clip(r2, 0, 1) ** 14 * 0.5
    caus = np.clip(caus * 1.3, 0, 1)
    w3 = spectral(n, 95, beta=3, fmax=6)
    img2 = np.stack([foam, caus, norm01(w3), np.ones_like(foam)], -1)
    Image.fromarray((np.clip(img2, 0, 1) * 255 + 0.5).astype(np.uint8), 'RGBA').save(f'{OUT}/water_foam_caustics.png')


# ---------------------------------------------------------------- clouds: perlin-worley + worley fbm
def clouds():
    n = 512
    p = norm01(spectral(n, 100, beta=2.6, fmin=2, fmax=40))
    w = np.zeros((n, n))
    for i, c in enumerate((6, 12, 24)):
        f1, _, _ = worley(n, c, 101 + i)
        w += (1 - np.clip(f1, 0, 1)) * (0.6 ** i)
    w = norm01(w)
    base = norm01(p * 0.6 + w * 0.6)
    mid = norm01(spectral(n, 105, beta=2, fmin=8, fmax=90))
    f1, _, _ = worley(n, 40, 106)
    hi = norm01((1 - np.clip(f1, 0, 1)) * 0.6 + spectral(n, 107, beta=1.5, fmin=30) * 0.15)
    img = np.stack([base, mid, hi, np.ones_like(base)], -1)
    Image.fromarray((img * 255 + 0.5).astype(np.uint8), 'RGBA').save(f'{OUT}/cloud_noise.png')


for f in (sand, grass, dirt, gravel, rock, snow, water, clouds):
    f()
    print('done', f.__name__)

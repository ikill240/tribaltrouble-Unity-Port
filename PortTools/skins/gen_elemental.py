"""The Elemental tribe's look (New Tides campaign, chapter 3): the game's own unit and building textures recoloured -
skin and stone turned to basalt with glowing ember cracks, cloth, hide and wood to desert ochre or bog moss in large
patches, metal to burnt copper. Works in texture space on any texture (no model needed).
usage: gen_elemental.py <out dir> <size> <png>...   (writes el_<name>.jpg)"""
import os
import sys

import numpy as np
from PIL import Image


def _grid(size, cells, rng):
    g = rng.rand(cells + 1, cells + 1)
    xs = np.linspace(0, cells, size, endpoint=False)
    i = np.floor(xs).astype(int); f = xs - i
    f = f * f * (3 - 2 * f)
    a = g[np.ix_(i, i)]; b = g[np.ix_(i, i + 1)]; c = g[np.ix_(i + 1, i)]; d = g[np.ix_(i + 1, i + 1)]
    fx = f[None, :]; fy = f[:, None]
    return (a * (1 - fx) + b * fx) * (1 - fy) + (c * (1 - fx) + d * fx) * fy


def fbm(size, cells, seed, octaves=4):
    rng = np.random.RandomState(seed)
    s, a, t = 0, 1.0, 0
    for o in range(octaves):
        s = s + a * _grid(size, cells * 2 ** o, rng)
        t += a
        a *= .5
    return s / t


def elemental(rgb, seed=1):
    h, w = rgb.shape[:2]
    n = max(h, w)
    r, g, b = rgb[..., 0], rgb[..., 1], rgb[..., 2]
    mx = rgb.max(-1); mn = rgb.min(-1)
    sat = (mx - mn) / (mx + 1e-6)
    hue = np.zeros_like(mx)
    d = mx - mn + 1e-6
    hue = np.where(mx == r, ((g - b) / d) % 6, np.where(mx == g, (b - r) / d + 2, (r - g) / d + 4)) / 6
    lum = r * .3 + g * .59 + b * .11
    shade = (lum / (np.mean(lum) + 1e-3))[..., None]
    skin = ((hue < .12) | (hue > .95)) & (sat > .2) & (sat < .75) & (mx > .35)   # warm, mid saturation
    metal = (hue > .5) & (hue < .7) & (sat > .1) & (mx > .3)
    rest = ~skin & ~metal
    patch = fbm(n, 3, seed, 3)[:h, :w]
    ridge = np.abs(fbm(n, 4, seed + 5, 3)[:h, :w] - .5)
    sparse = fbm(n, 2, seed + 9, 2)[:h, :w] > .42   # cracks only in some places
    basalt = np.array([.20, .17, .16])
    ochre = np.array([.78, .58, .32])
    moss = np.array([.34, .42, .22])
    copper = np.array([.62, .40, .26])
    out = rgb.copy()
    out[skin] = np.clip(basalt * np.minimum(shade[skin], 1.4) * 1.1, 0, 1)
    cloth = np.where((patch > .5)[..., None], ochre, moss)
    out[rest] = np.clip(rgb[rest] * .45 + cloth[rest] * np.minimum(shade[rest], 1.5) * .5, 0, 1)
    out[metal] = np.clip(copper * shade[metal], 0, 1)
    # ember cracks on skin and stone/wood, glowing
    crack = (ridge < .011) & sparse & (skin | (rest & (lum < .5)))
    soft = (ridge < .028) & sparse & (skin | (rest & (lum < .5)))
    out[soft] = out[soft] * .6 + np.array([.9, .35, .08]) * .4
    out[crack] = np.array([1.0, .62, .18])
    return out


def main():
    out, size = sys.argv[1], int(sys.argv[2])
    os.makedirs(out, exist_ok=True)
    for i, p in enumerate(sys.argv[3:]):
        name = os.path.splitext(os.path.basename(p))[0]
        im = Image.open(p).convert('RGB')
        if im.size[0] > size:
            im = im.resize((size, size), Image.LANCZOS)
        rgb = np.asarray(im).astype(np.float32) / 255
        res = elemental(rgb, seed=7)
        Image.fromarray((np.clip(res, 0, 1) * 255 + .5).astype(np.uint8)).save(os.path.join(out, 'el_' + name + '.jpg'),
                                                                                quality=88, optimize=True)
        print(name, flush=True)


if __name__ == '__main__':
    main()

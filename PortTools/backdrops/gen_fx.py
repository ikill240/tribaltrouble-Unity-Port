"""Particle sprites from ComfyUI renders on a dark background (h_fx_*): the background level is taken from the
corners and removed, alpha comes from what's left, faded to nothing at the rim (no square edges).
usage: gen_fx.py <renders dir> <out dir>"""
import os, sys
import numpy as np
from PIL import Image

PICK = {'smoke': 'h_fx_smoke_2.png', 'dust': 'h_fx_dust_3.png', 'spark': 'h_fx_spark_0.png'}
src, out = sys.argv[1], sys.argv[2]
for name, f in PICK.items():
    a = np.asarray(Image.open(os.path.join(src, f)).convert('RGB').resize((256, 256), Image.LANCZOS), np.float32) / 255
    n = a.shape[0]
    corners = np.concatenate([a[:16, :16], a[:16, -16:], a[-16:, :16], a[-16:, -16:]]).reshape(-1, 3)
    bg = np.median(corners, axis=0)
    lum = a.max(axis=2)
    d = np.clip((lum - bg.max() - 0.02) / max(1e-3, np.percentile(lum, 99.5) - bg.max()), 0, 1)
    yy, xx = np.mgrid[0:n, 0:n] / (n - 1) * 2 - 1
    r = np.sqrt(xx * xx + yy * yy)
    d *= np.clip((1.0 - r) / 0.25, 0, 1)            # nothing at the rim
    if name == 'spark':                               # additive: the colour is the light
        rgb = np.clip((a - bg) / max(1e-3, (a - bg).max()), 0, 1) * d[..., None]
        alpha = d
    else:                                             # alpha-blended puffs: their own colour, density as alpha
        rgb = np.clip((a - bg * (1 - d[..., None])) / np.maximum(d[..., None], 0.05), 0, 1)
        alpha = np.clip(d * 1.15, 0, 1) ** 1.1
    img = np.concatenate([rgb, alpha[..., None]], axis=2)
    Image.fromarray((img * 255).astype(np.uint8), 'RGBA').save(os.path.join(out, 'fx_%s.png' % name))
    print('fx_%s.png' % name)

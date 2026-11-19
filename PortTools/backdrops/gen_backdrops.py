"""Horizon panoramas for TTBackdrop: distant islands (tropical), a stormy snow-capped mountain range (northern) and
smoking volcanic isles (the Rift). Silhouettes are made here (periodic profiles: the panorama wraps round), filled
with SDXL textures from ComfyUI (src/g_tex_*.png), hazed for distance in two layers.
usage: gen_backdrops.py <out dir> [preview.png]"""
import math, os, sys
import numpy as np
from PIL import Image, ImageFilter

HERE = os.path.dirname(os.path.abspath(__file__))
W, H = 4096, 512
BASE = 22            # rows from the bottom up to sea level (TTBackdrop: 40 m below sea of 940 m)
rng = np.random.default_rng(7)


def tex(name, i=0):
    return np.asarray(Image.open(os.path.join(HERE, 'src', 'g_tex_%s_%d.png' % (name, i))).convert('RGB'), np.float32) / 255.0


def sample(t, xs, ys, scale):
    """Ping-pong tiled sampling (no seams) of texture t at pixel coords scaled."""
    th, tw = t.shape[:2]
    u = (xs * scale) % (2 * tw)
    v = (ys * scale) % (2 * th)
    u = np.where(u >= tw, 2 * tw - 1 - u, u).astype(np.int32)
    v = np.where(v >= th, 2 * th - 1 - v, v).astype(np.int32)
    return t[v.clip(0, th - 1), u.clip(0, tw - 1)]


def periodic_noise(n, octaves, seed, rough=0.55):
    r = np.random.default_rng(seed)
    x = np.arange(n) / n * 2 * math.pi
    out = np.zeros(n)
    amp = 1.0
    for o in range(octaves):
        f = 2 ** (o + 1)
        for k in range(3):
            fk = f + k
            out += amp * np.sin(x * fk + r.uniform(0, 2 * math.pi)) / 3
        amp *= rough
    return out


def island_profile(n, count, hmin, hmax, wmin, wmax, seed, volcanic=False):
    r = np.random.default_rng(seed)
    h = np.zeros(n)
    xs = np.arange(n)
    det = periodic_noise(n, 7, seed + 1, 0.6)
    for _ in range(count):
        c = r.uniform(0, n)
        w = r.uniform(wmin, wmax)
        top = r.uniform(hmin, hmax)
        d = np.abs(((xs - c + n / 2) % n) - n / 2) / (w / 2)
        if volcanic:
            shape = np.clip(1 - d, 0, 1) ** 1.25
            shape = np.where(d < 0.12, shape.max() * 0.93, shape)   # the crater rim
        else:
            shape = np.exp(-d * d * 2.2) * (1 + 0.25 * np.sin(d * 9 + c))
            shape = np.where(d < 1, shape, 0) * np.clip(1.15 - d, 0, 1)
        h = np.maximum(h, top * shape * (1 + 0.12 * det))
    return h


def range_profile(n, base, amp, seed):
    p = base + amp * periodic_noise(n, 8, seed, 0.62)
    ridge = 1 - np.abs(periodic_noise(n, 6, seed + 5, 0.7))     # jagged ridges
    p = p + amp * 0.55 * ridge ** 3
    return np.clip(p, 0, None)


def layer(img, profile, fill, haze_col, haze, rim=(1, 1, 1), snow=None, seed=0):
    ys = np.arange(H)[:, None].astype(np.float32)
    xs = np.arange(W)[None, :].astype(np.float32)
    ground = H - BASE
    top = ground - profile[None, :]
    a = np.clip(ys - top + 0.5, 0, 1)                         # 1px soft edge
    a = a * (profile[None, :] > 1.0)   # (no land, nothing: not even a line at the waterline)
    col = fill.copy()
    # height in the silhouette: darker toward the foot, a faint lit rim along the top
    rel = np.clip((ground - ys) / np.maximum(profile[None, :], 1), 0, 1)
    col *= (0.72 + 0.28 * rel)[..., None]
    rimm = np.clip(1 - (ys - top) / 6, 0, 1)[..., None] * 0.18
    col = col * (1 - rimm) + np.array(rim, np.float32) * rimm
    if snow is not None:
        line = ground - profile[None, :] * snow + 10 * periodic_noise(W, 6, seed + 9, 0.7)[None, :]
        s = np.clip((line - ys) / 14, 0, 1)[..., None]
        col = col * (1 - s * 0.85) + np.array([0.93, 0.95, 1.0], np.float32) * s * 0.85
    col = col * (1 - haze) + np.array(haze_col, np.float32) * haze
    al = a[..., None]
    img[..., :3] = img[..., :3] * (1 - al) + col * al
    img[..., 3:] = np.maximum(img[..., 3:], al)


def clouds(img, t, row0, row1, dark, alpha, seed):
    """A band of storm cloud behind everything drawn so far (under it in alpha)."""
    th, tw = t.shape[:2]
    band = t[: int(th * 0.55)]                                  # the cloud part of the image
    ys = np.arange(H)[:, None].astype(np.float32)
    xs = np.arange(W)[None, :].astype(np.float32)
    v = ((ys - row0) / max(1, row1 - row0)).clip(0, 1) * (band.shape[0] - 1)
    c = sample(band, xs + 0 * ys, v + 0 * xs, 1.0) * dark
    edge = periodic_noise(W, 6, seed, 0.65)[None, :]
    top = row0 + (row1 - row0) * (0.15 + 0.12 * edge)
    a = np.clip((ys - top) / 90, 0, 1) * np.clip((row1 - ys) / 70, 0, 1) * alpha
    a = a[..., None]
    under = 1 - img[..., 3:]
    img[..., :3] = img[..., :3] + c * a * under
    img[..., 3:] = img[..., 3:] + a * under


def make(kind):
    img = np.zeros((H, W, 4), np.float32)
    ys = np.arange(H)[:, None].astype(np.float32)
    xs = np.arange(W)[None, :].astype(np.float32)
    if kind == 'tropic':
        t0, t1 = tex('tropic', 0), tex('tropic', 1)
        haze = (0.70, 0.80, 0.88)
        far = island_profile(W, 11, 40, 120, 160, 520, 11)
        near = island_profile(W, 6, 80, 210, 220, 640, 23)
        layer(img, near, sample(t1, xs + 0 * ys, ys + 0 * xs, 0.9), haze, 0.38, seed=1)
        # far islands behind the near ones: draw into a separate image and put it under
        back = np.zeros_like(img)
        layer(back, far, sample(t0, xs + 0 * ys, ys + 0 * xs, 0.7), haze, 0.62, seed=2)
    elif kind == 'north':
        t0, t1, st = tex('north', 0), tex('north', 1), tex('storm', 0)
        haze = (0.60, 0.65, 0.72)
        far = range_profile(W, 150, 70, 31)
        near = np.maximum(range_profile(W, 70, 80, 41) - 30 * (1 + periodic_noise(W, 2, 43, 0.5)), 0)
        layer(img, near, sample(t1, xs + 0 * ys, ys + 0 * xs, 0.8), haze, 0.30, snow=0.45, seed=3)
        back = np.zeros_like(img)
        layer(back, far, sample(t0, xs + 0 * ys, ys + 0 * xs, 0.6), haze, 0.55, snow=0.6, seed=4)
        clouds(back, st, 20, H - BASE - 110, 0.75, 0.62, 51)      # the storm over the peaks
    else:
        t0, t1 = tex('rift', 0), tex('rift', 1)
        haze = (0.38, 0.33, 0.33)
        far = island_profile(W, 9, 50, 140, 200, 480, 61, volcanic=True)
        near = island_profile(W, 5, 90, 220, 260, 600, 71, volcanic=True)
        layer(img, near, sample(t1, xs + 0 * ys, ys + 0 * xs, 0.8), haze, 0.25, rim=(1.0, 0.55, 0.25), seed=5)
        back = np.zeros_like(img)
        layer(back, far, sample(t0, xs + 0 * ys, ys + 0 * xs, 0.6), haze, 0.5, rim=(1.0, 0.5, 0.2), seed=6)
        # smoke over the peaks
        smoke = np.zeros((H, W), np.float32)
        prof = np.maximum(far, near)
        peaks = [x for x in range(2, W - 2, 3) if prof[x] > 110 and prof[x] >= prof[max(0, x - 60):x + 60].max()]
        for c in peaks[:7]:
            for k in range(36):
                yy = H - BASE - prof[c] - 8 - k * 7
                xx = c + 25 * math.sin(k * 0.25) + k * 4
                rr = 10 + k * 1.8
                d2 = (np.arange(W)[None, :] - xx) ** 2 + (np.arange(H)[:, None] - yy) ** 2
                smoke = np.maximum(smoke, np.exp(-d2 / (2 * rr * rr)) * (1 - k / 40))
        sa = (smoke * 0.55)[..., None]
        under = 1 - back[..., 3:]
        back[..., :3] += np.array([0.22, 0.2, 0.2], np.float32) * sa * under
        back[..., 3:] += sa * under
    # the far layer under the near one
    out = img.copy()
    a = img[..., 3:]
    out[..., :3] = img[..., :3] * a + back[..., :3] * (1 - a)
    out[..., 3:] = a + back[..., 3:] * (1 - a)
    # straight alpha: colour un-premultiplied where alpha is partial
    rgb = np.where(out[..., 3:] > 1e-3, out[..., :3] / np.maximum(out[..., 3:], 1e-3), 0)
    rgb = np.where(img[..., 3:] > 0.999, img[..., :3], rgb)
    return np.concatenate([np.clip(rgb, 0, 1), np.clip(out[..., 3:], 0, 1)], axis=2)


if __name__ == '__main__':
    out = sys.argv[1]
    os.makedirs(out, exist_ok=True)
    prev = []
    for k in ('tropic', 'north', 'rift'):
        a = make(k)
        im = Image.fromarray((a * 255).astype(np.uint8), 'RGBA')
        im.save(os.path.join(out, 'backdrop_%s.png' % k))
        prev.append(im)
        print('backdrop_%s.png' % k)
    if len(sys.argv) > 2:
        sky = Image.new('RGBA', (W, H * 3), (120, 160, 205, 255))
        for i, im in enumerate(prev):
            sky.alpha_composite(im, (0, i * H))
        sky.convert('RGB').resize((W // 3, H)).save(sys.argv[2])

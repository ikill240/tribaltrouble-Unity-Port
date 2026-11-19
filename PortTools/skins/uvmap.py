"""Rasterises a Tribal Trouble mesh xml into texture space: for every texel, the rest-pose position, the normal and the
dominant bone of the surface that uses it. Used by gen_skins.py to paint war paint / armour where it belongs on the body."""
import xml.etree.ElementTree as ET
import numpy as np


def load(path):
    root = ET.parse(path).getroot()
    tris = []
    bones = {}
    for poly in root.iter('polygon'):
        vs = []
        for v in poly.iter('vertex'):
            best, bw = None, -1
            for s in v.iter('skin'):
                w = float(s.get('weight'))
                if w > bw:
                    bw, best = w, s.get('bone')
            bones.setdefault(best, len(bones))
            vs.append((float(v.get('x')), float(v.get('y')), float(v.get('z')),
                       float(v.get('nx')), float(v.get('ny')), float(v.get('nz')),
                       float(v.get('u')), float(v.get('v')), bones[best]))
        tris.append(vs)
    return tris, {i: n for n, i in bones.items()}


def raster(tris, size, flip_v=True, dilate=6):
    """Returns pos (H,W,3), nrm (H,W,3), bone (H,W) int (-1 empty)."""
    H = W = size
    pos = np.zeros((H, W, 3), np.float32)
    nrm = np.zeros((H, W, 3), np.float32)
    bone = -np.ones((H, W), np.int32)
    for t in tris:
        P = np.array([[a[6] * W, ((1 - a[7]) if flip_v else a[7]) * H] for a in t])
        X = np.array([a[0:3] for a in t]); N = np.array([a[3:6] for a in t])
        # the dominant bone of the triangle: the most common of its three
        bs = [a[8] for a in t]
        b = max(set(bs), key=bs.count)
        x0, y0 = np.floor(P.min(0)).astype(int); x1, y1 = np.ceil(P.max(0)).astype(int)
        x0 = max(x0, 0); y0 = max(y0, 0); x1 = min(x1, W - 1); y1 = min(y1, H - 1)
        if x1 < x0 or y1 < y0:
            continue
        xs, ys = np.meshgrid(np.arange(x0, x1 + 1) + 0.5, np.arange(y0, y1 + 1) + 0.5)
        (ax, ay), (bx, by), (cx, cy) = P
        d = (by - cy) * (ax - cx) + (cx - bx) * (ay - cy)
        if abs(d) < 1e-9:
            continue
        l0 = ((by - cy) * (xs - cx) + (cx - bx) * (ys - cy)) / d
        l1 = ((cy - ay) * (xs - cx) + (ax - cx) * (ys - cy)) / d
        l2 = 1 - l0 - l1
        m = (l0 >= -0.01) & (l1 >= -0.01) & (l2 >= -0.01)
        if not m.any():
            continue
        yy, xx = np.nonzero(m)
        yy += y0; xx += x0
        L = np.stack([l0[m], l1[m], l2[m]], 1)
        pos[yy, xx] = L @ X
        nn = L @ N
        nrm[yy, xx] = nn / (np.linalg.norm(nn, axis=1, keepdims=True) + 1e-9)
        bone[yy, xx] = b
    # grow the islands a few texels so filtering / mips don't bleed the old colours at the seams
    for _ in range(dilate):
        empty = bone < 0
        for dy, dx in ((0, 1), (0, -1), (1, 0), (-1, 0)):
            sb = np.roll(bone, (dy, dx), (0, 1)); sp = np.roll(pos, (dy, dx), (0, 1)); sn = np.roll(nrm, (dy, dx), (0, 1))
            take = empty & (sb >= 0)
            bone[take] = sb[take]; pos[take] = sp[take]; nrm[take] = sn[take]
            empty = bone < 0
    return pos, nrm, bone

"""The New Tides building kit: modelled offline, previewed here, exported for Unity.

Each building is a set of parts (one material each). Geometry is in the building's Unity frame (y up, ground at 0,
footings sunk to -0.3). UVs are laid out in metres (one texture tile = TILE m) so every surface shows the same texel
density. Triangles are oriented from the intended normals, so winding never needs thinking about.

Export: Resources/TTExtended/bk_<model>.bytes
  'TTBK' int32 version(2) f32 scale int32 parts; per part: str name, str tex, str mask, f32 gloss, i32 nv, nv*(px py pz nx ny nz u v),
  i32 ni, ni*i32   (str = i32 length + utf-8; the triangles front-facing in Unity as they are)
"""
import math
import struct

import numpy as np

TILE = 2.0


def v3(x, y, z):
    return np.array([x, y, z], float)


def norm(a):
    n = np.linalg.norm(a)
    return a / n if n > 1e-9 else a


def roty(a):
    c, s = math.cos(a), math.sin(a)
    return np.array([[c, 0, s], [0, 1, 0], [-s, 0, c]])


class Part:
    def __init__(self, name, tex, mask='', gloss=0.3):
        self.name, self.tex, self.mask, self.gloss = name, tex, mask, gloss
        self.p, self.n, self.uv, self.t = [], [], [], []

    def vert(self, p, n, uv):
        self.p.append(np.asarray(p, float)); self.n.append(norm(np.asarray(n, float))); self.uv.append(uv)
        return len(self.p) - 1

    def tri(self, a, b, c):
        """Indices; flipped if needed so the face points along its vertices' normals (right-handed CCW)."""
        pa, pb, pc = self.p[a], self.p[b], self.p[c]
        fn = np.cross(pb - pa, pc - pa)
        if np.dot(fn, self.n[a] + self.n[b] + self.n[c]) < 0:
            b, c = c, b
        self.t.append((a, b, c))


class Model:
    def __init__(self, name, scale=1.0):
        self.name = name
        self.parts = {}
        self.M = np.eye(3); self.O = np.zeros(3)
        self.scale = scale   # the whole model (UVs keep their pre-scale metres, so the textures grow with it)

    def part(self, name, tex, mask='', gloss=0.3):
        key = name
        if key not in self.parts:
            self.parts[key] = Part(name, tex, mask, gloss)
        return self.parts[key]

    # --- placement of a group: rotate about y, then offset
    def at(self, offset=(0, 0, 0), ang=0.0):
        m = self
        class Ctx:
            def __enter__(s):
                s.saved = (m.M.copy(), m.O.copy())
                R = roty(ang)
                m.O = m.O + m.M @ np.asarray(offset, float)
                m.M = m.M @ R
            def __exit__(s, *a):
                m.M, m.O = s.saved
        return Ctx()

    def P(self, p):
        return (self.M @ np.asarray(p, float) + self.O) * self.scale

    def N(self, n):
        return self.M @ np.asarray(n, float)

    # ------------------------------------------------------------------ primitives (local coordinates)

    def quad(self, part, a, b, c, d, uvs=None, normal=None, tile=TILE, both=False):
        """a b c d round the face (any order consistent with the normal). UVs default: metres along a->b and a->d."""
        a, b, c, d = (np.asarray(x, float) for x in (a, b, c, d))
        n = normal if normal is not None else np.cross(b - a, d - a)
        if uvs is None:
            ub = np.linalg.norm(b - a) / tile; vd = np.linalg.norm(d - a) / tile
            uvs = [(0, 0), (ub, 0), (ub, vd), (0, vd)]
        P = self.parts[part] if isinstance(part, str) else part
        wn = self.N(n)
        ids = [P.vert(self.P(x), wn, uv) for x, uv in zip((a, b, c, d), uvs)]
        P.tri(ids[0], ids[1], ids[2]); P.tri(ids[0], ids[2], ids[3])
        if both:
            ids = [P.vert(self.P(x), -wn, uv) for x, uv in zip((a, b, c, d), uvs)]
            P.tri(ids[0], ids[1], ids[2]); P.tri(ids[0], ids[2], ids[3])

    def tri(self, part, a, b, c, uvs, normal=None, both=False):
        a, b, c = (np.asarray(x, float) for x in (a, b, c))
        n = normal if normal is not None else np.cross(b - a, c - a)
        P = self.parts[part] if isinstance(part, str) else part
        wn = self.N(n)
        ids = [P.vert(self.P(x), wn, uv) for x, uv in zip((a, b, c), uvs)]
        P.tri(*ids)
        if both:
            ids = [P.vert(self.P(x), -wn, uv) for x, uv in zip((a, b, c), uvs)]
            P.tri(*ids)

    def obox(self, part, c, ax, ay, az, hx, hy, hz, tile=TILE, taper=1.0, bottom=False, top_part=None, uoff=0.0):
        """An oriented box: centre c, unit axes, half sizes; the top can taper. top_part: another part for the top."""
        c = np.asarray(c, float); ax, ay, az = (norm(np.asarray(x, float)) for x in (ax, ay, az))
        def B(sx, sz): return c + ax * sx * hx + az * sz * hz - ay * hy
        def T(sx, sz): return c + ax * sx * hx * taper + az * sz * hz * taper + ay * hy
        H = 2 * hy / tile
        sides = [((-1, 1), (1, 1), az, 2 * hx), ((1, 1), (1, -1), ax, 2 * hz), ((1, -1), (-1, -1), -az, 2 * hx), ((-1, -1), (-1, 1), -ax, 2 * hz)]
        u = uoff
        for (s0, s1, n, w) in sides:
            uw = w / tile
            self.quad(part, B(*s0), B(*s1), T(*s1), T(*s0), [(u, 0), (u + uw, 0), (u + uw, H), (u, H)], normal=n)
            u += uw
        tp = top_part or part
        self.quad(tp, T(-1, 1), T(1, 1), T(1, -1), T(-1, -1),
                  [(0, 0), (2 * hx * taper / tile, 0), (2 * hx * taper / tile, 2 * hz * taper / tile), (0, 2 * hz * taper / tile)], normal=ay)
        if bottom:
            self.quad(part, B(-1, -1), B(1, -1), B(1, 1), B(-1, 1), normal=-ay)

    def box(self, part, c, h, ang=0.0, **kw):
        R = roty(ang)
        self.obox(part, c, R @ v3(1, 0, 0), v3(0, 1, 0), R @ v3(0, 0, 1), h[0], h[1], h[2], **kw)

    def beam(self, part, a, b, w, h, up=(0, 1, 0), tile=TILE):
        """A square-section beam from a to b (w wide, h tall)."""
        a = np.asarray(a, float); b = np.asarray(b, float)
        ax = b - a; L = np.linalg.norm(ax); ax = ax / L
        upv = np.asarray(up, float)
        side = norm(np.cross(upv, ax))
        if np.linalg.norm(side) < 1e-6:
            side = norm(np.cross(v3(1, 0, 0), ax))
        upn = norm(np.cross(ax, side))
        # obox with its "y" along the beam
        self.obox(part, (a + b) / 2, side, ax, upn, w / 2, L / 2, h / 2, tile=tile, bottom=True)

    def cyl(self, part, a, b, r0, r1, sides=8, tile=TILE, caps=True, uwrap=None, vspan=None, uoff=0.0):
        """A tapered cylinder a->b. uwrap: texture repeats round it (else metres); vspan: v at b (else metres)."""
        a = np.asarray(a, float); b = np.asarray(b, float)
        ax = norm(b - a); L = np.linalg.norm(b - a)
        u = norm(np.cross(ax, v3(0, 0, 1) if abs(ax[2]) < 0.9 else v3(1, 0, 0)))
        w = np.cross(ax, u)
        P = self.parts[part] if isinstance(part, str) else part
        circ = 2 * math.pi * max(r0, r1)
        U = uwrap if uwrap is not None else circ / tile
        V = vspan if vspan is not None else L / tile
        slope = (r0 - r1) / max(L, 1e-6)
        ring0, ring1 = [], []
        for i in range(sides + 1):
            an = i * 2 * math.pi / sides
            d = u * math.cos(an) + w * math.sin(an)
            n = norm(d + ax * slope)
            ring0.append(P.vert(self.P(a + d * r0), self.N(n), (uoff + U * i / sides, 0)))
            ring1.append(P.vert(self.P(b + d * r1), self.N(n), (uoff + U * i / sides, V)))
        for i in range(sides):
            P.tri(ring0[i], ring0[i + 1], ring1[i + 1]); P.tri(ring0[i], ring1[i + 1], ring1[i])
        if caps:
            for (cpt, r, nn) in ((a, r0, -ax), (b, r1, ax)):
                if r <= 1e-4:
                    continue
                cid = P.vert(self.P(cpt), self.N(nn), (0.5, 0.5))
                rim = []
                for i in range(sides):
                    an = i * 2 * math.pi / sides
                    d = u * math.cos(an) + w * math.sin(an)
                    rim.append(P.vert(self.P(cpt + d * r), self.N(nn), (0.5 + 0.5 * math.cos(an) * r / tile * 2, 0.5 + 0.5 * math.sin(an) * r / tile * 2)))
                for i in range(sides):
                    P.tri(cid, rim[i], rim[(i + 1) % sides])

    def log(self, part, a, b, r, sides=8, tip=0.0, uoff=0.0):
        """A round log, optionally sharpened at b."""
        a = np.asarray(a, float); b = np.asarray(b, float)
        self.cyl(part, a, b, r, r, sides, caps=tip <= 0, uoff=uoff)
        if tip > 0:
            ax = norm(b - a)
            self.cone(part, b, b + ax * tip, r, sides)

    def cone(self, part, base, tip, r, sides=8, tile=TILE, cap=False):
        base = np.asarray(base, float); tip = np.asarray(tip, float)
        ax = norm(tip - base); L = np.linalg.norm(tip - base)
        u = norm(np.cross(ax, v3(0, 0, 1) if abs(ax[2]) < 0.9 else v3(1, 0, 0)))
        w = np.cross(ax, u)
        P = self.parts[part] if isinstance(part, str) else part
        for i in range(sides):
            a0 = i * 2 * math.pi / sides; a1 = (i + 1) * 2 * math.pi / sides
            d0 = u * math.cos(a0) + w * math.sin(a0); d1 = u * math.cos(a1) + w * math.sin(a1)
            n0 = norm(d0 * L + ax * r); n1 = norm(d1 * L + ax * r)
            U = 2 * math.pi * r / tile
            i0 = P.vert(self.P(base + d0 * r), self.N(n0), (U * i / sides, 0))
            i1 = P.vert(self.P(base + d1 * r), self.N(n1), (U * (i + 1) / sides, 0))
            i2 = P.vert(self.P(tip), self.N(norm(n0 + n1)), (U * (i + 0.5) / sides, L / tile))
            P.tri(i0, i1, i2)
        if cap:
            self.disk(part, base, r, -ax, sides)

    def disk(self, part, c, r, n, sides=12, uv_center=(0.5, 0.5), uv_r=0.5):
        c = np.asarray(c, float); n = norm(np.asarray(n, float))
        u = norm(np.cross(n, v3(0, 0, 1) if abs(n[2]) < 0.9 else v3(1, 0, 0)))
        w = np.cross(n, u)
        P = self.parts[part] if isinstance(part, str) else part
        cid = P.vert(self.P(c), self.N(n), uv_center)
        rim = [P.vert(self.P(c + (u * math.cos(i * 2 * math.pi / sides) + w * math.sin(i * 2 * math.pi / sides)) * r), self.N(n),
                      (uv_center[0] + uv_r * math.cos(i * 2 * math.pi / sides), uv_center[1] + uv_r * math.sin(i * 2 * math.pi / sides)))
               for i in range(sides)]
        for i in range(sides):
            P.tri(cid, rim[i], rim[(i + 1) % sides])

    def lathe(self, part, c, profile, sides=12, tile=TILE, uwrap=None, vspan=None, top=True, uvrect=None):
        """Revolves [(r, y), ...] (bottom to top) about the vertical through c. uvrect: (u0, v0, u1, v1) to squeeze
        the whole surface into one atlas cell."""
        c = np.asarray(c, float)
        P = self.parts[part] if isinstance(part, str) else part
        lens = [0.0]
        for i in range(1, len(profile)):
            lens.append(lens[-1] + math.hypot(profile[i][0] - profile[i - 1][0], profile[i][1] - profile[i - 1][1]))
        circ = 2 * math.pi * max(r for r, _ in profile)
        U = uwrap if uwrap is not None else circ / tile
        rings = []
        for k, (r, y) in enumerate(profile):
            # normal from the profile's slope
            r0, y0 = profile[max(k - 1, 0)]; r1, y1 = profile[min(k + 1, len(profile) - 1)]
            dr, dy = r1 - r0, y1 - y0
            ring = []
            for i in range(sides + 1):
                an = i * 2 * math.pi / sides
                d = v3(math.cos(an), 0, math.sin(an))
                n = norm(d * dy - v3(0, 1, 0) * dr) if abs(dr) + abs(dy) > 1e-9 else d
                uu, vv = U * i / sides, (lens[k] / tile if vspan is None else vspan * lens[k] / max(lens[-1], 1e-6))
                if uvrect:
                    uu = uvrect[0] + (uvrect[2] - uvrect[0]) * i / sides
                    vv = uvrect[1] + (uvrect[3] - uvrect[1]) * lens[k] / max(lens[-1], 1e-6)
                ring.append(P.vert(self.P(c + d * r + v3(0, y, 0)), self.N(n), (uu, vv)))
            rings.append(ring)
        for k in range(len(rings) - 1):
            a, b = rings[k], rings[k + 1]
            for i in range(sides):
                if profile[k][0] > 1e-5 or profile[k + 1][0] > 1e-5:
                    P.tri(a[i], a[i + 1], b[i + 1]); P.tri(a[i], b[i + 1], b[i])
        if top and profile[-1][0] > 1e-4:
            r, y = profile[-1]
            uvc = ((uvrect[0] + uvrect[2]) / 2, (uvrect[1] + uvrect[3]) / 2) if uvrect else (0.5, 0.5)
            uvr = (uvrect[2] - uvrect[0]) / 2 if uvrect else 0.5
            self.disk(part, c + v3(0, y, 0), r, v3(0, 1, 0), sides, uvc, uvr)

    def slab(self, top_part, side_part, a, b, c, d, thick, tile=TILE, uvs=None):
        """A roof slab: a b c d its top face (a->b along the eave, a->d up the slope), thickness below it."""
        a, b, c, d = (np.asarray(x, float) for x in (a, b, c, d))
        n = norm(np.cross(b - a, d - a))
        if n[1] < 0:
            n = -n
        o = -n * thick
        self.quad(top_part, a, b, c, d, uvs, normal=n, tile=tile)
        self.quad(side_part, a + o, b + o, c + o, d + o, normal=-n, tile=tile)
        for (p, q) in ((a, b), (b, c), (c, d), (d, a)):
            en = norm(np.cross(q - p, n)) * -1
            mid = (p + q) / 2 - (a + b + c + d) / 4
            if np.dot(en, mid) < 0:
                en = -en
            self.quad(side_part, p, q, q + o, p + o, normal=en, tile=tile)

    def shield(self, part, c, facing, r=0.42):
        """A round shield on a wall (the classic shield texture: a disc mapped over the whole image)."""
        n = norm(np.asarray(facing, float))
        c = np.asarray(c, float)
        P = self.parts[part] if isinstance(part, str) else part
        u = norm(np.cross(n, v3(0, 1, 0))); w = np.cross(n, u)
        seg = 16
        cid = P.vert(self.P(c + n * 0.06), self.N(n), (0.5, 0.5))
        rim = []
        for i in range(seg):
            an = i * 2 * math.pi / seg
            d = u * math.cos(an) + w * math.sin(an)
            rim.append(P.vert(self.P(c + d * r), self.N(norm(n + d * 0.3)), (0.5 + 0.5 * math.cos(an), 0.5 + 0.5 * math.sin(an))))
        for i in range(seg):
            P.tri(cid, rim[i], rim[(i + 1) % seg])


# ------------------------------------------------------------------ export

def export(model, path, wall_tier=True):
    out = bytearray()
    def s(x):
        b = x.encode('utf-8'); out.extend(struct.pack('<i', len(b))); out.extend(b)
    parts = [p for p in model.parts.values() if p.t]
    out.extend(b'TTBK'); out.extend(struct.pack('<ifi', 2, model.scale, len(parts)))   # v2: the model's scale
    for P in parts:
        s(P.name); s(P.tex); s(P.mask); out.extend(struct.pack('<f', P.gloss))
        out.extend(struct.pack('<i', len(P.p)))
        arr = np.zeros((len(P.p), 8), np.float32)
        for i in range(len(P.p)):
            arr[i, 0:3] = P.p[i]; arr[i, 3:6] = P.n[i]; arr[i, 6:8] = P.uv[i]
        out.extend(arr.tobytes())
        # (kept as built: counter-clockwise about the normal in these right-handed coordinates is clockwise seen from
        # the front in Unity's left-handed frame with the same numbers - the front face. Swapping two corners here
        # turned every face inside out: the outsides were culled and the insides / undersides showed through.)
        tris = np.array(P.t, np.int32)
        out.extend(struct.pack('<i', tris.size)); out.extend(tris.tobytes())
    open(path, 'wb').write(bytes(out))
    return sum(len(P.t) for P in parts)


# ------------------------------------------------------------------ preview (software renderer)

def render(model, texs, W=420, H=360, az=35, el=38, scale=None, team=(0.25, 0.45, 0.95), center=None):
    """texs: name -> (rgb HxWx3, mask HxW or None)."""
    allp = np.concatenate([np.array(P.p) for P in model.parts.values() if P.p])
    ca, sa = math.cos(math.radians(az)), math.sin(math.radians(az))
    ce, se = math.cos(math.radians(el)), math.sin(math.radians(el))
    def view(p):
        x = p[..., 0] * ca - p[..., 2] * sa
        z = p[..., 0] * sa + p[..., 2] * ca
        y = p[..., 1]
        sy = y * ce - z * se
        depth = y * se + z * ce
        return x, sy, depth
    vx, vy, _ = view(allp)
    if scale is None:
        scale = 0.85 * min(W / (vx.max() - vx.min() + 1e-6), H / (vy.max() - vy.min() + 1e-6))
    cx = (vx.max() + vx.min()) / 2 if center is None else center[0]
    cy = (vy.max() + vy.min()) / 2 if center is None else center[1]
    img = np.zeros((H, W, 3)); img[:] = (0.55, 0.62, 0.45)
    zb = np.full((H, W), -1e9)
    sun = norm(v3(-0.5, 0.8, 0.35))
    teamc = np.array(team)
    for P in model.parts.values():
        if not P.t:
            continue
        tex, mask = texs.get(P.tex, (None, None))
        pts = np.array(P.p); nrm = np.array(P.n); uvs = np.array(P.uv, float)
        x, y, d = view(pts)
        sx = (x - cx) * scale + W / 2; sy = H / 2 - (y - cy) * scale
        for (a, b, c) in P.t:
            X = np.array([sx[a], sx[b], sx[c]]); Y = np.array([sy[a], sy[b], sy[c]])
            x0, x1 = int(max(X.min(), 0)), int(min(X.max(), W - 1)); y0, y1 = int(max(Y.min(), 0)), int(min(Y.max(), H - 1))
            if x1 < x0 or y1 < y0:
                continue
            den = (Y[1] - Y[2]) * (X[0] - X[2]) + (X[2] - X[1]) * (Y[0] - Y[2])
            if abs(den) < 1e-9:
                continue
            gx, gy = np.meshgrid(np.arange(x0, x1 + 1) + 0.5, np.arange(y0, y1 + 1) + 0.5)
            l0 = ((Y[1] - Y[2]) * (gx - X[2]) + (X[2] - X[1]) * (gy - Y[2])) / den
            l1 = ((Y[2] - Y[0]) * (gx - X[2]) + (X[0] - X[2]) * (gy - Y[2])) / den
            l2 = 1 - l0 - l1
            m = (l0 >= -1e-4) & (l1 >= -1e-4) & (l2 >= -1e-4)
            dd = l0 * d[a] + l1 * d[b] + l2 * d[c]
            sub = zb[y0:y1 + 1, x0:x1 + 1]
            m &= dd > sub
            if not m.any():
                continue
            # back faces (seen from behind) are drawn dark: they'd be culled in the game
            fn = (nrm[a] + nrm[b] + nrm[c])
            vdir = v3(sa * ce, se, ca * ce) * -1   # towards the viewer is -(view axis)
            vdir = v3(-sa * -1, 0, 0)
            uu = l0 * uvs[a, 0] + l1 * uvs[b, 0] + l2 * uvs[c, 0]
            vv = l0 * uvs[a, 1] + l1 * uvs[b, 1] + l2 * uvs[c, 1]
            if tex is not None:
                th, tw = tex.shape[:2]
                tx = ((uu % 1.0) * tw).astype(int) % tw; ty = (((1 - vv) % 1.0) * th).astype(int) % th
                col = tex[ty, tx]
                if mask is not None:
                    mk = mask[ty, tx][..., None]
                    col = col * (1 - mk) + teamc * mk * (0.55 + 0.45 * col.mean(-1, keepdims=True))
            else:
                col = np.full(gx.shape + (3,), 0.8)
            nn = norm(l0[..., None] * nrm[a] + l1[..., None] * nrm[b] + l2[..., None] * nrm[c]) if False else None
            nI = l0[..., None] * nrm[a] + l1[..., None] * nrm[b] + l2[..., None] * nrm[c]
            nI = nI / (np.linalg.norm(nI, axis=-1, keepdims=True) + 1e-9)
            lam = np.clip(nI @ sun, 0, 1)
            shade = 0.38 + 0.75 * lam
            sub[m] = dd[m]
            img[y0:y1 + 1, x0:x1 + 1][m] = np.clip(col[m] * shade[m][..., None], 0, 1)
    return img

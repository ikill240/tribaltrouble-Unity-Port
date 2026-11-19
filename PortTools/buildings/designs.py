"""The New Tides buildings, both races. Sizes follow the old procedural models (same footprints and effect anchors)."""
import math

import numpy as np

from kit import Model, v3, norm, roty

BARK = 'palisade_bark'


def parts_common(m, wall_tex, vik):
    m.part('wall', wall_tex, gloss=0.25)
    m.part('trim', 'bk_darkwood', gloss=0.3)
    m.part('logs', BARK, gloss=0.2)


# ------------------------------------------------------------------ helpers

def gable_roof(m, top, under, x0, x1, z0, z1, eave_y, ridge_y, thick=0.22, overhang=0.45, curve=0.0):
    """A gable roof along z: slopes down to x0 / x1. curve > 0 sags the slopes (two segments)."""
    zc0, zc1 = z0 - overhang, z1 + overhang
    xm = (x0 + x1) / 2
    for side in (-1, 1):
        xe = (x0 - overhang) if side < 0 else (x1 + overhang)
        e0, e1 = v3(xe, eave_y, zc0), v3(xe, eave_y, zc1)
        r0, r1 = v3(xm, ridge_y, zc0 - 0.1), v3(xm, ridge_y, zc1 + 0.1)
        if curve > 0:
            mid0 = (e0 + r0) / 2 + v3(0, -curve, 0); mid1 = (e1 + r1) / 2 + v3(0, -curve, 0)
            m.slab(top, under, e0, e1, mid1, mid0, thick)
            m.slab(top, under, mid0, mid1, r1, r0, thick)
        else:
            m.slab(top, under, e0, e1, r1, r0, thick)


def gable_roof_x(m, top, under, x0, x1, z0, z1, eave_y, ridge_y, thick=0.22, overhang=0.45):
    """A gable roof along x: slopes down to z0 / z1."""
    xc0, xc1 = x0 - overhang, x1 + overhang
    zm = (z0 + z1) / 2
    for side in (-1, 1):
        ze = (z0 - overhang) if side < 0 else (z1 + overhang)
        e0, e1 = v3(xc0, eave_y, ze), v3(xc1, eave_y, ze)
        r0, r1 = v3(xc0 - 0.1, ridge_y, zm), v3(xc1 + 0.1, ridge_y, zm)
        m.slab(top, under, e0, e1, r1, r0, thick)


def ring_of_stones(m, part, c, R, n, size=0.22, seed=0, h=0.25):
    rng = np.random.RandomState(seed)
    for i in range(n):
        a = i * 2 * math.pi / n + rng.rand() * 0.2
        p = v3(c[0] + math.cos(a) * R, -0.1 + h * 0.5, c[2] + math.sin(a) * R)
        s = size * (0.8 + 0.5 * rng.rand())
        m.box(part, p, (s, h * (0.7 + 0.5 * rng.rand()), s * 0.8), ang=a + rng.rand(), taper=0.75)


def barrel(m, part, c, r=0.32, h=0.75):
    prof = [(r * 0.86, 0), (r, h * 0.5), (r * 0.86, h)]
    m.lathe(part, c, prof, sides=10, uwrap=1.0, vspan=h / 2)
    for y in (h * 0.15, h * 0.85):
        rr = r * (0.86 + 0.14 * (1 - abs(y - h / 2) / (h / 2))) + 0.01
        m.cyl('iron', v3(c[0], c[1] + y - 0.03, c[2]), v3(c[0], c[1] + y + 0.03, c[2]), rr, rr, 10, caps=False)


def crate(m, part, c, s, ang=0.0):
    m.box(part, c, (s, s, s), ang=ang, tile=1.0)


def banner_pole(m, base, height, w=0.7, h=1.3, ang=0.0):
    m.log('logs', base, base + v3(0, height, 0), 0.07, 6, tip=0.15)
    top = base + v3(0, height - 0.15, 0)
    R = roty(ang)
    d = R @ v3(1, 0, 0)
    m.beam('trim', top - d * 0.05, top + d * (w + 0.1), 0.06, 0.06)
    a = top + d * 0.05 - v3(0, 0.05, 0); b = top + d * w - v3(0, 0.05, 0)
    m.quad('banner', a - v3(0, h, 0), b - v3(0, h, 0), b, a, [(0, 0), (1, 0), (1, 1), (0, 1)], normal=R @ v3(0, 0, 1), both=True)


def feather(m, part, root, d, length, width, normal, k=0):
    """One feather (a tapered blade, both sides), mapped onto column k of the feather texture's top row."""
    root = np.asarray(root, float); d = norm(np.asarray(d, float)); nn = norm(np.asarray(normal, float))
    side = norm(np.cross(nn, d)) * width / 2
    u0 = (k % 8) / 8.0; uc = u0 + 1 / 16.0; hw = 0.95 / 16.0
    tip = root + d * length; mid = root + d * length * 0.5
    pts = [root, mid - side * 0.63, tip - side, tip + side, mid + side * 0.63]
    uvs = [(uc, 0.751), (uc - hw * 0.63, 0.875), (uc - hw, 0.999), (uc + hw, 0.999), (uc + hw * 0.63, 0.875)]
    m.tri(part, pts[0], pts[1], pts[4], [uvs[0], uvs[1], uvs[4]], normal=nn, both=True)
    m.quad(part, pts[1], pts[2], pts[3], pts[4], [uvs[1], uvs[2], uvs[3], uvs[4]], normal=nn, both=True)


def wing(m, part, root, out, up, n=7, length=1.6, spread=1.1, width=0.3, k0=0):
    """A fan of feathers from root: out the main direction, up the plane's other axis; spread in radians."""
    out = norm(np.asarray(out, float)); up = norm(np.asarray(up, float))
    nn = np.cross(out, up)
    for i in range(n):
        a = -spread / 2 + spread * i / max(n - 1, 1)
        d = out * math.cos(a) + up * math.sin(a)
        L = length * (0.75 + 0.25 * math.cos(a * 1.6))
        feather(m, part, np.asarray(root, float) + up * (i - n / 2) * 0.02, d, L, width, nn, k0 + i)


# ------------------------------------------------------------------ halls

def mead_hall():
    m = Model('hall_v', 1.7)
    parts_common(m, 'bk_staves', True)
    m.part('footing', 'bk_fieldstone', gloss=0.4)
    m.part('turf', 'bk_turf', gloss=0.1)
    m.part('roofwood', 'bk_planks', gloss=0.2)
    m.part('shields', 'shield_viking', 'shield_viking_team', gloss=0.4)
    m.part('door', 'bk_darkwood', gloss=0.2)
    m.part('iron', 'bk_iron', gloss=0.8)
    m.part('banner', 'bk_banner', 'bk_banner_team', gloss=0.1)
    L, W, H = 3.5, 2.25, 1.9
    seg = 7
    # the long walls, bowed like a ship's hull: stone footing, stave walls on it
    for side in (-1, 1):
        for i in range(seg):
            z0 = -L + 2 * L * i / seg; z1 = -L + 2 * L * (i + 1) / seg
            b0 = 1 - 0.1 * (z0 / L) ** 2; b1 = 1 - 0.1 * (z1 / L) ** 2
            x0, x1 = side * W * b0, side * W * b1
            n = v3(side, 0, 0)
            m.quad('footing', v3(x0 + side * 0.08, -0.3, z0), v3(x1 + side * 0.08, -0.3, z1), v3(x1 + side * 0.08, 0.45, z1), v3(x0 + side * 0.08, 0.45, z0), normal=n)
            m.quad('footing', v3(x0 + side * 0.08, 0.45, z0), v3(x1 + side * 0.08, 0.45, z1), v3(x1, 0.45, z1), v3(x0, 0.45, z0), normal=v3(0, 1, 0))
            m.quad('wall', v3(x0, 0.45, z0), v3(x1, 0.45, z1), v3(x1, H, z1), v3(x0, H, z0), normal=n)
            # posts at the joints
            m.log('logs', v3(x0 + side * 0.05, 0.35, z0), v3(x0 + side * 0.05, H + 0.05, z0), 0.12, 6)
        m.log('logs', v3(side * W * 0.9 + side * 0.05, 0.35, L), v3(side * W * 0.9 + side * 0.05, H + 0.05, L), 0.14, 6)
    # end walls (door at +z) and gables
    for end in (-1, 1):
        z = end * L
        xw = W * 0.9
        n = v3(0, 0, end)
        m.quad('footing', v3(-xw, -0.3, z + end * 0.08), v3(xw, -0.3, z + end * 0.08), v3(xw, 0.45, z + end * 0.08), v3(-xw, 0.45, z + end * 0.08), normal=n)
        if end > 0:
            m.quad('wall', v3(-xw, 0.45, z), v3(-0.6, 0.45, z), v3(-0.6, H, z), v3(-xw, H, z), normal=n)
            m.quad('wall', v3(0.6, 0.45, z), v3(xw, 0.45, z), v3(xw, H, z), v3(0.6, H, z), normal=n)
            m.quad('wall', v3(-0.6, 1.65, z), v3(0.6, 1.65, z), v3(0.6, H, z), v3(-0.6, H, z), normal=n)
            # the doorway: a dark recess, a frame, an iron-banded door swung half open
            m.quad('door', v3(-0.6, 0.0, z - 0.6), v3(0.6, 0.0, z - 0.6), v3(0.6, 1.65, z - 0.6), v3(-0.6, 1.65, z - 0.6), normal=n)
            for sx in (-1, 1):
                m.quad('door', v3(sx * 0.6, 0.0, z), v3(sx * 0.6, 0.0, z - 0.6), v3(sx * 0.6, 1.65, z - 0.6), v3(sx * 0.6, 1.65, z), normal=v3(-sx, 0, 0))
            m.quad('door', v3(-0.6, 1.65, z), v3(0.6, 1.65, z), v3(0.6, 1.65, z - 0.6), v3(-0.6, 1.65, z - 0.6), normal=v3(0, -1, 0))
            m.quad('door', v3(-0.6, 0.0, z), v3(0.6, 0.0, z), v3(0.6, 0.0, z - 0.6), v3(-0.6, 0.0, z - 0.6), normal=v3(0, 1, 0))
            for sx in (-1, 1):
                m.beam('trim', v3(sx * 0.66, -0.1, z + 0.06), v3(sx * 0.66, 1.75, z + 0.06), 0.16, 0.16)
            m.beam('trim', v3(-0.8, 1.72, z + 0.06), v3(0.8, 1.72, z + 0.06), 0.16, 0.18)
            # a door leaf, swung out
            m.box('trim', v3(-0.95, 0.82, z + 0.45), (0.05, 0.82, 0.55), ang=0.15)
            for y in (0.35, 1.3):
                m.box('iron', v3(-0.89, y, z + 0.45), (0.02, 0.05, 0.5), ang=0.15)
        else:
            m.quad('wall', v3(-xw, 0.45, z), v3(xw, 0.45, z), v3(xw, H, z), v3(-xw, H, z), normal=n)
        ridge = H + 2.1
        m.tri('wall', v3(-xw - 0.1, H, z), v3(xw + 0.1, H, z), v3(0, ridge - 0.1, z), [(0, 0), (2.3, 0), (1.15, 1.1)], normal=n)
    # the roof: a turf roof on planking, sagging a little like the old halls; ridge beam
    gable_roof(m, 'turf', 'roofwood', -W, W, -L, L, H - 0.15, H + 2.1, thick=0.3, overhang=0.5, curve=0.12)
    m.beam('trim', v3(0, H + 2.13, -L - 0.75), v3(0, H + 2.13, L + 0.75), 0.18, 0.16)
    # crossed bargeboards at both gables, ending in carved dragon heads above the ridge
    for end in (-1, 1):
        z = end * (L + 0.55)
        for side in (-1, 1):
            a = v3(side * (W + 0.55), H - 0.25, z)
            b = v3(-side * 0.32, H + 2.1 + 0.42, z)
            m.beam('trim', a, b, 0.09, 0.22, up=(0, 0, 1))
            m.box('trim', b + v3(-side * 0.08, 0.05, 0), (0.16, 0.1, 0.07))
            m.cone('trim', b + v3(-side * 0.22, 0.08, 0), b + v3(-side * 0.48, 0.0, 0), 0.08, 5)
        # a smoke hole hood on the ridge
    m.box('roofwood', v3(0, H + 2.3, -1.0), (0.35, 0.18, 0.45), taper=0.8)
    # shields along the long walls
    for side in (-1, 1):
        for i in range(5):
            z = -L * 0.8 + i * (1.6 * L / 4)
            b = 1 - 0.1 * (z / L) ** 2
            m.shield('shields', v3(side * (W * b + 0.08), 1.1, z), v3(side, 0, 0), r=0.42)
    # a woodpile by the wall
    for i in range(6):
        y = 0.12 + (i // 3) * 0.2
        zz = -2.0 + (i % 3) * 0.24 + (i // 3) * 0.12
        m.log('logs', v3(W + 0.45, y, zz), v3(W + 1.25, y, zz), 0.11, 6)
    # torches either side of the door
    for sx in (-1, 1):
        m.log('logs', v3(sx * 0.95, 1.1, L + 0.12), v3(sx * 0.95, 1.65, L + 0.15), 0.05, 5)
    # a porch over the door: two carved posts on stone pads, a small gabled turf roof, a step of flagstones
    PZ = L + 1.45
    for sx in (-1, 1):
        m.box('footing', v3(sx * 1.1, -0.05, PZ), (0.22, 0.18, 0.22))
        m.log('logs', v3(sx * 1.1, 0.1, PZ), v3(sx * 1.1, 2.15, PZ), 0.13, 7)
        m.beam('trim', v3(sx * 1.1, 2.1, PZ + 0.2), v3(sx * 1.1, 2.1, L - 0.1), 0.12, 0.14)
        m.beam('trim', v3(sx * 1.1, 1.5, PZ), v3(sx * 0.6, 2.05, PZ), 0.07, 0.07)   # knee braces
    m.beam('trim', v3(-1.35, 2.15, PZ), v3(1.35, 2.15, PZ), 0.14, 0.16)
    gable_roof(m, 'turf', 'roofwood', -1.25, 1.25, L + 0.1, PZ, 2.1, 3.0, thick=0.18, overhang=0.25)
    m.tri('roofwood', v3(-1.3, 2.2, PZ + 0.02), v3(1.3, 2.2, PZ + 0.02), v3(0, 2.98, PZ + 0.02), [(0, 0), (1.3, 0), (0.65, 0.4)], normal=v3(0, 0, 1), both=True)
    for side in (-1, 1):   # little dragon-head bargeboards on the porch too
        a = v3(side * 1.55, 2.0, PZ + 0.28); b = v3(-side * 0.18, 3.18, PZ + 0.28)
        m.beam('trim', a, b, 0.06, 0.15, up=(0, 0, 1))
        m.cone('trim', b + v3(-side * 0.1, 0.05, 0), b + v3(-side * 0.32, 0.0, 0), 0.06, 5)
    m.box('footing', v3(0, -0.06, L + 0.75), (0.95, 0.1, 0.7))
    # banner poles flanking the porch, a rack of spears
    banner_pole(m, v3(-2.0, -0.1, L + 1.9), 4.0, w=0.6, h=1.2, ang=math.pi)
    banner_pole(m, v3(2.0, -0.1, L + 1.9), 4.0, w=0.6, h=1.2, ang=0.0)
    m.beam('trim', v3(W + 0.5, 1.1, 0.4), v3(W + 0.5, 1.1, 1.6), 0.06, 0.06)
    for k in range(5):
        zz = 0.5 + k * 0.25
        m.beam('logs', v3(W + 0.75, 0.0, zz), v3(W + 0.45, 1.9, zz), 0.03, 0.03)
        m.cone('iron', v3(W + 0.45, 1.9, zz), v3(W + 0.42, 2.15, zz), 0.035, 4)
    return m


def spirit_lodge():
    m = Model('hall_n', 1.6)
    m.part('cover', 'bk_hide', 'bk_hide_team', gloss=0.15)
    m.part('logs', BARK, gloss=0.2)
    m.part('inside', 'bk_darkwood', gloss=0.1)
    m.part('stones', 'bk_fieldstone', gloss=0.4)
    m.part('carved', 'bk_carved', gloss=0.3)
    m.part('feathers', 'bk_feathers', 'bk_feathers_team', gloss=0.2)
    n, R, H = 14, 3.1, 5.4
    apex = v3(0, H, 0)
    rng = np.random.RandomState(17)
    for i in range(n):
        a = i * 2 * math.pi / n + 0.12
        base = v3(math.cos(a) * R, -0.25, math.sin(a) * R)
        top = base + (apex - base) * 1.13
        m.log('logs', base, top, 0.1, 6, uoff=rng.rand())
    # the hide cover: a bellied cone (three rings), the door gap at +x, smoke flaps at the top
    seg = 28
    rings = [(R * 0.99, 0.02, 0.0), (R * 0.66, H * 0.38, 0.42), (R * 0.2, H * 0.83, 1.0)]
    for i in range(seg):
        a0, a1 = i * 2 * math.pi / seg, (i + 1) * 2 * math.pi / seg
        mid = (a0 + a1) / 2
        door = abs(math.atan2(math.sin(mid), math.cos(mid))) < math.radians(17)
        for k in range(len(rings) - 1):
            r0, y0, v0 = rings[k]; r1, y1, v1 = rings[k + 1]
            if door and k == 0:
                continue
            A = v3(math.cos(a0) * r0, y0, math.sin(a0) * r0); B = v3(math.cos(a1) * r0, y0, math.sin(a1) * r0)
            Cc = v3(math.cos(a1) * r1, y1, math.sin(a1) * r1); D = v3(math.cos(a0) * r1, y1, math.sin(a0) * r1)
            u0, u1 = 4 * i / seg, 4 * (i + 1) / seg
            nrm = v3(math.cos(mid) * (y1 - y0), r0 - r1, math.sin(mid) * (y1 - y0))
            m.quad('cover', A, B, Cc, D, [(u0, v0), (u1, v0), (u1, v1), (u0, v1)], normal=nrm)
            # the inside, seen through the door
            m.quad('inside', A * 0.97, B * 0.97, Cc * 0.97, D * 0.97, normal=-nrm)
    # the doorway's dark interior and the rolled-back door flap
    for k in range(6):
        a0 = -math.radians(17) + k * math.radians(34) / 6; a1 = a0 + math.radians(34) / 6
        r0, y0 = R * 0.75, 0.0
        A = v3(math.cos(a0) * r0 * 0.4, y0, math.sin(a0) * r0 * 0.4); B = v3(math.cos(a1) * r0 * 0.4, y0, math.sin(a1) * r0 * 0.4)
        m.quad('inside', A, B, B + v3(0, 2.0, 0), A + v3(0, 2.0, 0), normal=v3(1, 0, 0))
    fa = math.radians(22)
    m.quad('cover', v3(math.cos(fa) * R, 0.05, math.sin(fa) * R), v3(math.cos(fa) * R * 1.05 + 0.2, 0.05, math.sin(fa) * R + 0.6),
           v3(math.cos(fa) * R * 0.8 + 0.2, 2.0, math.sin(fa) * R * 0.8 + 0.5), v3(math.cos(fa) * R * 0.72, 2.0, math.sin(fa) * R * 0.72),
           [(0, 0), (0.5, 0), (0.5, 0.42), (0, 0.42)], both=True)
    # smoke flaps
    for s in (-1, 1):
        a = math.radians(180 + s * 35)
        p = v3(math.cos(a) * R * 0.22, H * 0.8, math.sin(a) * R * 0.22)
        q = v3(math.cos(a) * R * 0.45, H * 0.95, math.sin(a) * R * 0.45)
        t = v3(-math.sin(a), 0, math.cos(a)) * 0.45
        m.quad('cover', p - t, p + t, q + t * 1.3, q - t * 1.3, [(0, 0.85), (0.4, 0.85), (0.4, 1.0), (0, 1.0)], both=True)
    # stones round the hem
    ring_of_stones(m, 'stones', v3(0, 0, 0), R * 1.02, 24, size=0.2, seed=3, h=0.22)
    # carved posts with feather wings flanking the door
    for s in (-1, 1):
        p = v3(R + 0.55, -0.2, s * 1.15)
        m.cyl('carved', p, p + v3(0, 2.5, 0), 0.17, 0.15, 10, uwrap=1.0, vspan=1.0)
        m.cone('carved', p + v3(0, 2.5, 0), p + v3(0, 2.85, 0), 0.17, 10)
        top = p + v3(0, 2.3, 0)
        for w in (-1, 1):
            wing(m, 'feathers', top + v3(0, 0, w * 0.12), v3(0, 0.35, w), v3(0, 1, 0), n=5, length=0.8, spread=1.0, width=0.2)
    return m


# ------------------------------------------------------------------ forges

def rune_forge():
    """The Vikings' smithy: a timber workshop on a stone floor, open at the front (+x) under a shingled gable roof with
    dragon-headed bargeboards; a big stone hearth and hood against the back wall with its chimney through the roof,
    the anvil on its stump out front, bellows, grindstone, quench trough, a coal heap, weapons and shields on show."""
    m = Model('forge_v', 1.8)
    m.part('wall', 'bk_staves', gloss=0.25)        # (rebuilt in stone when the forge is upgraded)
    m.part('stone', 'bk_stone', gloss=0.45)
    m.part('footing', 'bk_fieldstone', gloss=0.4)
    m.part('roof', 'bk_shingles', gloss=0.2)
    m.part('roofwood', 'bk_planks', gloss=0.2)
    m.part('trim', 'bk_darkwood', gloss=0.3)
    m.part('logs', BARK, gloss=0.2)
    m.part('iron', 'bk_iron', gloss=0.9)
    m.part('coal', 'bk_iron', gloss=0.05)
    m.part('planks', 'bk_planks', gloss=0.25)
    m.part('hide', 'bk_hide', gloss=0.15)
    m.part('shields', 'shield_viking', 'shield_viking_team', gloss=0.4)
    X0, X1, Z, H, RIDGE = -2.2, 1.75, 1.95, 1.9, 3.55
    xr = (X0 + X1) / 2
    # the stone floor, a step at the open front
    m.box('footing', v3(-0.2, -0.12, 0), (2.15, 0.22, 2.15))
    m.box('footing', v3(2.05, -0.2, 0), (0.25, 0.14, 1.6))
    # the closed back half: back wall and side walls, on a footing course
    m.box('wall', v3(X0 + 0.12, H / 2 + 0.05, 0), (0.12, H / 2, Z))
    for s in (-1, 1):
        m.box('wall', v3(-1.25, H / 2 + 0.05, s * Z), (0.95, H / 2, 0.12))
        m.box('footing', v3(-1.25, 0.25, s * (Z + 0.06)), (1.0, 0.2, 0.1))
        # corner and door posts; the front posts carrying the roof
        for x in (X0, -0.3, 0.75, X1):
            m.log('logs', v3(x, -0.1, s * Z), v3(x, H + 0.02, s * Z), 0.13 if x in (X0, X1) else 0.11, 7)
        m.beam('trim', v3(X0 - 0.1, H - 0.03, s * Z), v3(X1 + 0.15, H - 0.03, s * Z), 0.16, 0.16)
        m.beam('trim', v3(-0.3, 1.25, s * Z), v3(0.75, 1.25, s * Z), 0.09, 0.09)    # the open side's rail
    m.beam('trim', v3(X1, H - 0.03, -Z - 0.15), v3(X1, H - 0.03, Z + 0.15), 0.18, 0.18)   # the front tie beam
    m.beam('trim', v3(xr, H - 0.03, -Z - 0.1), v3(xr, H - 0.03, Z + 0.1), 0.13, 0.13)
    # the gables (planked) and the shingled roof on its ridge beam
    for end in (-1, 1):
        z = end * Z
        m.tri('roofwood', v3(X0 - 0.05, H, z), v3(X1 + 0.05, H, z), v3(xr, RIDGE - 0.12, z), [(0, 0), (2.6, 0), (1.3, 1.1)],
              normal=v3(0, 0, end), both=True)
    gable_roof(m, 'roof', 'roofwood', X0, X1, -Z, Z, H - 0.12, RIDGE, thick=0.2, overhang=0.42, curve=0.06)
    m.beam('trim', v3(xr, RIDGE + 0.05, -Z - 0.6), v3(xr, RIDGE + 0.05, Z + 0.6), 0.16, 0.15)
    # crossed bargeboards ending in dragon heads above the ridge, both gables
    for end in (-1, 1):
        z = end * (Z + 0.46)
        for side in (-1, 1):
            a = v3(xr + side * ((X1 - X0) / 2 + 0.45), H - 0.2, z)
            b = v3(xr - side * 0.3, RIDGE + 0.42, z)
            m.beam('trim', a, b, 0.08, 0.2, up=(0, 0, 1))
            m.box('trim', b + v3(-side * 0.08, 0.05, 0), (0.15, 0.09, 0.07))
            m.cone('trim', b + v3(-side * 0.2, 0.08, 0), b + v3(-side * 0.45, 0.0, 0), 0.075, 5)
    # the hearth: a raised stone bed of coals under a hood, against the back wall; the chimney through the roof
    m.box('stone', v3(-1.15, 0.42, 0), (0.62, 0.38, 0.85))
    m.box('coal', v3(-1.0, 0.82, 0), (0.42, 0.03, 0.62))
    m.box('stone', v3(-1.3, 1.62, 0), (0.62, 0.42, 0.88), taper=0.6)          # the hood
    m.box('stone', v3(-1.55, 3.0, 0), (0.4, 1.05, 0.45), taper=0.85)          # the chimney
    m.box('stone', v3(-1.55, 4.12, 0), (0.42, 0.1, 0.47))                     # its cap
    for s in (-1, 1):
        m.box('stone', v3(-1.55, 4.32, s * 0.25), (0.08, 0.12, 0.08))
    m.beam('iron', v3(-0.55, 0.92, -0.6), v3(-0.55, 0.92, 0.6), 0.05, 0.05)   # the bed's iron lip
    # the bellows beside the hearth, its lever on a post
    m.box('hide', v3(-1.15, 0.95, 1.15), (0.45, 0.14, 0.26), taper=0.55)
    m.box('trim', v3(-1.15, 1.1, 1.15), (0.47, 0.03, 0.28))
    m.log('logs', v3(-0.5, -0.1, 1.45), v3(-0.5, 1.3, 1.45), 0.06, 5)
    m.beam('trim', v3(-1.5, 1.35, 1.2), v3(-0.3, 1.25, 1.45), 0.05, 0.05)
    # the anvil on its stump, out in the light; hammers and tongs on the stump's side
    ax, az = 0.75, 0.35
    m.log('logs', v3(ax, 0.0, az), v3(ax, 0.55, az), 0.3, 9)
    m.box('iron', v3(ax, 0.72, az), (0.18, 0.18, 0.34), taper=1.2)
    m.box('iron', v3(ax, 0.95, az), (0.23, 0.05, 0.42))
    m.cone('iron', v3(ax, 0.95, az + 0.42), v3(ax, 0.95, az + 0.78), 0.07, 6)
    m.beam('trim', v3(ax + 0.28, 0.1, az - 0.1), v3(ax + 0.32, 0.6, az - 0.1), 0.04, 0.04)
    m.box('iron', v3(ax + 0.32, 0.62, az - 0.1), (0.05, 0.05, 0.1))
    # the quench trough and a water barrel
    m.box('planks', v3(1.1, 0.3, -1.25), (0.5, 0.22, 0.28))
    m.box('trim', v3(1.1, 0.53, -1.25), (0.44, 0.01, 0.22))
    barrel(m, 'planks', v3(1.45, 0.08, 1.45), 0.3, 0.8)
    # a grindstone in its frame
    m.cyl('stone', v3(0.05, 0.58, -1.38), v3(0.05, 0.58, -1.2), 0.38, 0.38, 14)
    m.beam('iron', v3(0.05, 0.58, -1.55), v3(0.05, 0.58, -1.03), 0.04, 0.04)
    for zz in (-1.52, -1.06):
        m.beam('trim', v3(-0.25, 0.1, zz), v3(0.05, 0.6, zz), 0.06, 0.06)
        m.beam('trim', v3(0.35, 0.1, zz), v3(0.05, 0.6, zz), 0.06, 0.06)
    # the coal heap and a stack of iron bars
    m.cone('coal', v3(-0.35, 0.1, -1.45), v3(-0.32, 0.6, -1.4), 0.45, 8)
    for i in range(5):
        y = 0.14 + (i // 3) * 0.07; zz = 1.0 + (i % 3) * 0.12 + (i // 3) * 0.06
        m.box('iron', v3(0.25, y, zz), (0.28, 0.03, 0.045))
    # a weapon rack at the open side: axes and spears leaning
    m.beam('trim', v3(1.55, 0.95, -0.15), v3(1.55, 0.95, -0.95), 0.06, 0.06)
    for k in range(4):
        zz = -0.25 - k * 0.22
        m.beam('logs', v3(1.75, 0.12, zz), v3(1.52, 1.25, zz), 0.035, 0.035)
        if k % 2 == 0:
            m.box('iron', v3(1.53, 1.15, zz), (0.03, 0.1, 0.09))
        else:
            m.cone('iron', v3(1.52, 1.25, zz), v3(1.47, 1.5, zz), 0.035, 4)
    # tools hanging from the front tie beam
    for k, zz in enumerate((-0.9, -0.55, 0.6, 0.95)):
        m.beam('iron', v3(X1, H - 0.12, zz), v3(X1, H - 0.55 - 0.08 * (k % 2), zz), 0.025, 0.025)
        m.box('iron', v3(X1, H - 0.6 - 0.08 * (k % 2), zz), (0.04, 0.06, 0.08))
    # shields on the outer walls, in the tribe's colour
    for s in (-1, 1):
        for x in (-1.85, -1.25, -0.65):
            m.shield('shields', v3(x, 1.15, s * (Z + 0.14)), v3(0, 0, s), r=0.3)
        m.shield('shields', v3(X0 - 0.02, 1.15, s * 0.95), v3(-1, 0, 0), r=0.32)
    # a woodpile against the back wall
    for i in range(7):
        y = 0.1 + (i // 4) * 0.2; zz = 0.6 + (i % 4) * 0.25 + (i // 4) * 0.12
        m.log('logs', v3(X0 - 0.35, y, zz - 0.4), v3(X0 - 0.35, y, zz + 0.35), 0.1, 6)
    return m


def crystal_forge():
    m = Model('forge_n', 1.9)
    m.part('wall', 'bk_adobe', gloss=0.15)
    m.part('roof', 'bk_thatch', gloss=0.1)
    m.part('logs', BARK, gloss=0.2)
    m.part('stone', 'bk_fieldstone', gloss=0.4)
    m.part('clay', 'bk_clay', gloss=0.3)
    m.part('inside', 'bk_darkwood', gloss=0.1)
    m.part('crystal', 'bk_runestone', gloss=0.95)
    m.part('hide', 'bk_hide', 'bk_hide_team', gloss=0.12)
    # a horseshoe of adobe wall open to the front (+x)
    c = v3(-0.2, 0, 0); R = 2.0
    seg = 14
    a_from, a_to = math.radians(55), math.radians(305)
    for i in range(seg):
        a0 = a_from + (a_to - a_from) * i / seg; a1 = a_from + (a_to - a_from) * (i + 1) / seg
        am = (a0 + a1) / 2
        p = c + v3(math.cos(am) * R, 0.75, math.sin(am) * R)
        w = R * (a_to - a_from) / seg / 2 + 0.03
        m.box('wall', p, (0.2, 1.05, w), ang=-am)
    # thatched cone roof on poles
    for i in range(6):
        a = i * 2 * math.pi / 6 + 0.3
        m.log('logs', c + v3(math.cos(a) * 2.25, -0.2, math.sin(a) * 2.25), c + v3(math.cos(a) * 2.25, 2.0, math.sin(a) * 2.25), 0.11, 6)
    m.lathe('roof', c + v3(0, 1.85, 0), [(2.5, 0.0), (2.3, 0.12), (1.3, 0.95), (0.3, 1.8), (0.0, 1.95)], sides=18)
    m.lathe('inside', c + v3(0, 1.84, 0), [(0.0, 1.8), (0.3, 1.66), (1.3, 0.85), (2.3, 0.02)], sides=18, top=False)
    # the kiln: a clay dome at the back with its chimney, its mouth facing the open front
    k = v3(-1.45, -0.15, 0)
    m.lathe('wall', k, [(0.95, 0.0), (0.98, 0.5), (0.8, 1.1), (0.45, 1.45), (0.35, 1.6)], sides=14, top=False)
    m.lathe('wall', k + v3(0, 1.55, 0), [(0.35, 0.0), (0.3, 1.6), (0.36, 1.75), (0.36, 1.85)], sides=10, top=False)
    m.quad('inside', v3(-0.5, 0.15, -0.38), v3(-0.5, 0.15, 0.38), v3(-0.48, 0.95, 0.3), v3(-0.48, 0.95, -0.3), normal=v3(1, 0, 0))
    # the altar where crystals are worked: a flat stone on two stones; pots of pigment and water
    for s in (-1, 1):
        m.box('stone', v3(1.2, 0.15, 0.3 + s * 0.3), (0.16, 0.35, 0.16), taper=0.85)
    m.box('stone', v3(1.2, 0.58, 0.3), (0.32, 0.08, 0.55))
    for (x, z, r, h) in ((1.6, -1.3, 0.28, 0.55), (1.15, -1.55, 0.2, 0.42), (0.6, 1.6, 0.25, 0.5), (1.85, 1.2, 0.18, 0.36)):
        m.lathe('clay', v3(x, -0.05, z), [(r * 0.55, 0), (r, h * 0.45), (r * 0.6, h * 0.85), (r * 0.7, h)], sides=10, uwrap=1.0, vspan=1.0)
    # a crystal heap glinting by the kiln (the glow is drawn on the altar)
    rng = np.random.RandomState(5)
    for i in range(9):
        p = v3(0.35 + rng.rand() * 0.55, -0.05, -1.0 + rng.rand() * 0.6)
        m.cone('crystal', p, p + v3((rng.rand() - 0.5) * 0.25, 0.3 + rng.rand() * 0.45, (rng.rand() - 0.5) * 0.25), 0.08 + rng.rand() * 0.06, 5)
    # a hide awning out front on two forked poles, bellows of hide by the kiln mouth, a drying rack of spear shafts
    for sz in (-1, 1):
        m.log('logs', v3(2.75, -0.15, sz * 1.5), v3(2.75, 1.85, sz * 1.5), 0.08, 6)
    seg = 5
    for i in range(seg):
        za = -1.6 + 3.2 * i / seg; zb = -1.6 + 3.2 * (i + 1) / seg
        m.quad('hide', v3(1.95, 2.05, za), v3(1.95, 2.05, zb), v3(2.85, 1.78, zb), v3(2.85, 1.78, za),
               [(i / seg, 1), ((i + 1) / seg, 1), ((i + 1) / seg, 0.55), (i / seg, 0.55)], normal=v3(0.3, 1, 0), both=True)
    m.box('hide', v3(-0.55, 0.32, 0.75), (0.32, 0.12, 0.2), taper=0.55)
    m.box('inside', v3(-0.55, 0.45, 0.75), (0.34, 0.025, 0.22))
    m.beam('inside', v3(-0.9, 0.5, 0.75), v3(-0.35, 0.95, 0.75), 0.04, 0.04)
    for k in range(5):
        zz = 1.2 + k * 0.18
        m.beam('logs', v3(1.95, 0.0, zz), v3(1.75, 1.6, zz), 0.03, 0.03)
        m.cone('crystal', v3(1.75, 1.6, zz), v3(1.73, 1.85, zz), 0.04, 4)
    return m


# ------------------------------------------------------------------ totems

def rune_stone():
    m = Model('totem_v', 2.0)
    m.part('stone', 'bk_runestone', gloss=0.35)
    m.part('cairn', 'bk_fieldstone', gloss=0.4)
    # the standing stone: a thick slab, wide face to +x/-x, with a rounded top; the serpent band on both faces
    prof = [(-0.62, 0.0), (0.62, 0.0), (0.6, 1.5), (0.5, 2.5), (0.3, 3.0), (0.0, 3.15), (-0.32, 3.0), (-0.52, 2.45), (-0.6, 1.4)]
    th = 0.26
    pts = [v3(0, y - 0.2, z) for (z, y) in prof]
    n = len(pts)
    for sx in (-1, 1):
        for i in range(1, n - 1):
            a, b, c = pts[0], pts[i], pts[i + 1]
            def uv(p): return ((p[2] + 0.65) / 1.3, (p[1] + 0.2) / 3.35)
            m.tri('stone', a + v3(sx * th, 0, 0), b + v3(sx * th * (1 - 0.2 * b[1] / 3), 0, 0), c + v3(sx * th * (1 - 0.2 * c[1] / 3), 0, 0),
                  [uv(a), uv(b), uv(c)], normal=v3(sx, 0, 0))
    for i in range(n):
        p, q = pts[i], pts[(i + 1) % n]
        tp = th * (1 - 0.2 * p[1] / 3); tq = th * (1 - 0.2 * q[1] / 3)
        e = q - p
        nn = norm(np.cross(e, v3(1, 0, 0)))
        if np.dot(nn, (p + q) / 2 - v3(0, 1.3, 0)) < 0:
            nn = -nn
        m.quad('stone', p + v3(-tp, 0, 0), p + v3(tp, 0, 0), q + v3(tq, 0, 0), q + v3(-tq, 0, 0),
               [(0.02, 0.1), (0.06, 0.1), (0.06, 0.12), (0.02, 0.12)], normal=nn)
    # a cairn round its foot, two lesser stones
    ring_of_stones(m, 'cairn', v3(0, 0, 0), 0.95, 12, size=0.24, seed=9, h=0.35)
    m.box('cairn', v3(0.9, 0.25, 1.3), (0.2, 0.5, 0.28), ang=0.4, taper=0.7)
    m.box('cairn', v3(-0.8, 0.15, -1.25), (0.22, 0.4, 0.25), ang=-0.3, taper=0.65)
    return m


def war_totem():
    m = Model('totem_n', 2.0)
    m.part('carved', 'bk_carved', gloss=0.3)
    m.part('feathers', 'bk_feathers', 'bk_feathers_team', gloss=0.2)
    m.part('stones', 'bk_fieldstone', gloss=0.4)
    m.part('logs', BARK, gloss=0.2)
    # the pole: four carved faces stacked, a beaked cap
    m.cyl('carved', v3(0, -0.25, 0), v3(0, 3.15, 0), 0.42, 0.36, 12, uwrap=1.0, vspan=1.0)
    m.lathe('carved', v3(0, 3.1, 0), [(0.42, 0.0), (0.46, 0.08), (0.3, 0.22), (0.0, 0.3)], sides=12, uvrect=(0, 0.9, 1, 1))
    m.cone('carved', v3(0.3, 2.9, 0), v3(0.85, 2.75, 0), 0.12, 6)   # the beak
    # outspread wings of feathers
    for s in (-1, 1):
        wing(m, 'feathers', v3(0.05, 2.75, s * 0.35), v3(0.05, 0.45, s), v3(0, 1, 0), n=9, length=1.9, spread=1.5, width=0.34)
    # a stone ring and four stakes hung with feathers
    ring_of_stones(m, 'stones', v3(0, 0, 0), 0.85, 12, size=0.22, seed=12, h=0.3)
    for i in range(4):
        a = i * math.pi / 2 + math.pi / 4
        p = v3(math.cos(a) * 1.5, -0.2, math.sin(a) * 1.5)
        m.log('logs', p, p + v3(0, 1.3, 0), 0.06, 5, tip=0.15)
        for j in range(3):
            feather(m, 'feathers', p + v3(0, 1.0, 0), v3(math.cos(a + 0.6 * (j - 1)), -1.6, math.sin(a + 0.6 * (j - 1))), 0.55, 0.16, v3(-math.sin(a), 0, math.cos(a)), i * 3 + j)
    return m


# ------------------------------------------------------------------ dock, market, shrine

def dock(vik):
    m = Model('dock_' + ('v' if vik else 'n'), 1.45)
    m.part('deck', 'bk_planks', gloss=0.25)
    m.part('trim', 'bk_darkwood', gloss=0.3)
    m.part('logs', BARK, gloss=0.2)
    m.part('roof', 'bk_shingles' if vik else 'bk_thatch', gloss=0.15)
    m.part('wall', 'bk_planks' if vik else 'bk_staves', gloss=0.2)
    m.part('crates', 'bk_planks', gloss=0.25)
    m.part('iron', 'bk_iron', gloss=0.8)
    m.part('rope', 'bk_thatch', gloss=0.1)
    m.part('banner', 'bk_banner', 'bk_banner_team', gloss=0.1)
    # the deck: planks along x on a frame of beams (Vikings); lashed logs laid across (Natives); on posts
    x0, x1, zh = -2.4, 3.6, 2.2
    if vik:
        m.slab('deck', 'trim', v3(x0, 0.67, -zh), v3(x1, 0.67, -zh), v3(x1, 0.67, zh), v3(x0, 0.67, zh), 0.12)
    else:
        m.slab('trim', 'trim', v3(x0, 0.5, -zh), v3(x1, 0.5, -zh), v3(x1, 0.5, zh), v3(x0, 0.5, zh), 0.08)
        nlog = 21
        for i in range(nlog):
            x = x0 + 0.14 + (x1 - x0 - 0.28) * i / (nlog - 1)
            m.log('logs', v3(x, 0.6, -zh - 0.05), v3(x, 0.6, zh + 0.05), 0.145, 7, uoff=(i * 0.37) % 1)
        for z in (-zh + 0.3, zh - 0.3):
            m.cyl('rope', v3(x0, 0.75, z), v3(x1, 0.75, z), 0.035, 0.035, 5, caps=False)
    for s in (-1, 1):
        m.beam('trim', v3(x0, 0.5, s * (zh + 0.05)), v3(x1, 0.5, s * (zh + 0.05)), 0.16, 0.22)
    for x in (-2.0, 0.2, 2.4, 3.5):
        for s in (-1, 1):
            m.log('logs', v3(x, -0.9, s * 2.0), v3(x, 0.95, s * 2.0), 0.15, 7)
    # bollards with rope, a low rail along the sides
    for s in (-1, 1):
        m.log('logs', v3(3.35, 0.6, s * 1.75), v3(3.35, 1.05, s * 1.75), 0.11, 7)
        m.cyl('rope', v3(3.35, 0.8, s * 1.75), v3(3.35, 0.9, s * 1.75), 0.14, 0.14, 8, caps=False)
    for s in (-1, 1):
        m.beam('trim', v3(-0.6, 1.25, s * 2.05), v3(3.5, 1.25, s * 2.05), 0.07, 0.07)
        for x in (-0.6, 0.8, 2.2, 3.5):
            m.beam('trim', v3(x, 0.67, s * 2.05), v3(x, 1.3, s * 2.05), 0.08, 0.08)
    # the boat shed at the landward end
    sx0, sx1, sz0, sz1 = -2.35, -0.75, 0.25, 2.15
    for (a, b, n) in ((v3(sx0, 0.67, sz0), v3(sx0, 0.67, sz1), v3(-1, 0, 0)), (v3(sx0, 0.67, sz1), v3(sx1, 0.67, sz1), v3(0, 0, 1)),
                      (v3(sx1, 0.67, sz0), v3(sx0, 0.67, sz0), v3(0, 0, -1))):
        m.quad('wall', a, b, b + v3(0, 1.6, 0), a + v3(0, 1.6, 0), normal=n)
    m.quad('trim', v3(sx1, 0.67, sz1), v3(sx1, 0.67, sz0), v3(sx1, 2.27, sz0), v3(sx1, 2.27, sz1), normal=v3(1, 0, 0))   # dark open front
    gable_roof(m, 'roof', 'trim', sx0, sx1, sz0, sz1, 2.2, 3.1, thick=0.12, overhang=0.3)
    for z in (sz0, sz1):
        m.tri('wall', v3(sx0, 2.27, z), v3(sx1, 2.27, z), v3((sx0 + sx1) / 2, 3.05, z), [(0, 0), (0.8, 0), (0.4, 0.4)], normal=v3(0, 0, 1 if z > 0 else -1))
    # the crane: mast, arm, brace, rope and a hanging load
    mast = v3(-1.8, 0.67, -1.4)
    m.log('logs', mast, mast + v3(0, 3.6, 0), 0.15, 8)
    m.beam('trim', mast + v3(-0.25, 3.45, 0), v3(0.75, 4.1, -1.4), 0.14, 0.16)
    m.beam('trim', mast + v3(0, 2.2, 0), v3(-0.6, 3.75, -1.4), 0.09, 0.09)
    m.cyl('rope', v3(0.6, 4.0, -1.4), v3(0.6, 3.25, -1.4), 0.025, 0.025, 4, caps=False)
    crate(m, 'crates', v3(0.6, 3.0, -1.4), 0.28, 0.3)
    # cargo on the deck: crates, barrels, a coil of rope
    crate(m, 'crates', v3(1.6, 1.0, 1.2), 0.33, 0.2); crate(m, 'crates', v3(2.3, 0.97, 1.0), 0.3, -0.3)
    crate(m, 'crates', v3(1.85, 1.6, 1.15), 0.27, 0.6)
    barrel(m, 'deck', v3(0.6, 0.67, 1.5), 0.3, 0.7); barrel(m, 'deck', v3(0.95, 0.67, 1.0), 0.28, 0.65)
    m.lathe('rope', v3(2.6, 0.67, -1.2), [(0.2, 0), (0.42, 0.05), (0.42, 0.18), (0.2, 0.22)], sides=12, top=True)
    banner_pole(m, v3(3.45, 0.67, -2.0), 3.4, ang=math.pi / 2)
    # a boat moored alongside: a clinker-built faering (Vikings) or a dugout canoe (Natives), with its oars / paddles
    boat(m, v3(1.6, 0.05, 3.05), 3.2 if vik else 3.6, 0.62 if vik else 0.42, 0.55 if vik else 0.4, 'deck' if vik else 'logs', vik)
    # a net hung to dry on the rail, a lantern on the end post
    m.quad('rope', v3(-0.4, 1.25, 2.12), v3(0.9, 1.25, 2.12), v3(0.85, 0.45, 2.16), v3(-0.35, 0.5, 2.16),
           [(0, 0.2), (0.7, 0.2), (0.7, 0.6), (0, 0.6)], normal=v3(0, 0, 1), both=True)
    m.box('iron', v3(3.5, 1.45, 2.05), (0.09, 0.13, 0.09), taper=0.7)
    return m


def boat(m, c, length, beam, depth, part, vik):
    """A small open boat along x centred on c (the water line at c's height): a hull of strakes, thwarts, oars."""
    n = 9
    st = 3 if vik else 2
    ring = []
    for i in range(n + 1):
        t = i / n
        x = c[0] - length / 2 + length * t
        w = beam * math.sin(math.pi * t) ** 0.7
        rise = (0.18 if vik else 0.05) * (2 * t - 1) ** 2   # the sheer: stem and stern rise
        ring.append((x, w, rise))
    for k in range(st):
        f0, f1 = k / st, (k + 1) / st
        for i in range(n):
            (xa, wa, ra), (xb, wb, rb) = ring[i], ring[i + 1]
            for side in (-1, 1):
                def P(x, w, r, f):   # f = 0 keel ... 1 gunwale
                    return v3(x, c[1] - depth * (1 - f) + r * f, c[2] + side * w * (f ** 0.6))
                A, B = P(xa, wa, ra, f0), P(xb, wb, rb, f0)
                C, D = P(xb, wb, rb, f1), P(xa, wa, ra, f1)
                nrm = v3(0, -0.3, side)
                m.quad(part, A, B, C, D, normal=nrm, both=True)
    # gunwale rails, thwarts and the oars resting across
    for i in range(n):
        (xa, wa, ra), (xb, wb, rb) = ring[i], ring[i + 1]
        for side in (-1, 1):
            m.beam('trim', v3(xa, c[1] + ra, c[2] + side * wa), v3(xb, c[1] + rb, c[2] + side * wb), 0.05, 0.05)
    for t in (0.35, 0.65):
        x = c[0] - length / 2 + length * t
        w = beam * math.sin(math.pi * t) ** 0.7
        m.beam('trim', v3(x, c[1] - 0.1, c[2] - w), v3(x, c[1] - 0.1, c[2] + w), 0.18, 0.05, up=(1, 0, 0))
    if vik:
        m.cone('trim', v3(c[0] + length / 2 - 0.05, c[1] + 0.2, c[2]), v3(c[0] + length / 2 + 0.25, c[1] + 0.55, c[2]), 0.06, 5)
    for k in (-1, 1):
        a = v3(c[0] + 0.3 * k - 0.6, c[1] + 0.05, c[2] - beam * 0.6)
        b = v3(c[0] + 0.3 * k + 0.9, c[1] + 0.05, c[2] + beam * 0.9)
        m.beam('trim', a, b, 0.04, 0.04)
        m.box('trim', b + v3(0.1, 0, 0.12), (0.06, 0.02, 0.16), ang=-0.6)


def market(vik):
    return viking_market() if vik else native_market()


def goods_heap(m, c, cell, r=0.26, h=0.18):
    cu, cv = cell % 4, cell // 4
    m.lathe('goods', c, [(r, 0), (r * 0.8, h * 0.6), (0.0, h)], sides=9,
            uvrect=(cu * 0.25 + 0.02, 1 - (cv + 1) * 0.25 + 0.02, cu * 0.25 + 0.23, 1 - cv * 0.25 - 0.02), top=False)


def viking_market():
    """The Vikings' trading hall: two plank stalls under one shingled roof on carved posts, a hide awning out front,
    furs and fish drying on racks, barrels and a chest of silver, a shield wall on show."""
    m = Model('market_v', 1.6)
    m.part('counter', 'bk_planks', gloss=0.25)
    m.part('trim', 'bk_darkwood', gloss=0.3)
    m.part('logs', BARK, gloss=0.2)
    m.part('roof', 'bk_shingles', gloss=0.2)
    m.part('roofwood', 'bk_planks', gloss=0.2)
    m.part('awning', 'bk_hide', 'bk_hide_team', gloss=0.12)
    m.part('goods', 'bk_goods', gloss=0.35)
    m.part('baskets', 'bk_thatch', gloss=0.1)
    m.part('jars', 'bk_planks', gloss=0.3)
    m.part('banner', 'bk_banner', 'bk_banner_team', gloss=0.1)
    m.part('iron', 'bk_iron', gloss=0.8)
    m.part('footing', 'bk_fieldstone', gloss=0.4)
    m.part('shields', 'shield_viking', 'shield_viking_team', gloss=0.4)
    m.part('fur', 'bk_hide', gloss=0.1)
    # a plank floor on a low stone kerb
    m.box('footing', v3(0, -0.12, 0), (2.5, 0.2, 1.9))
    m.slab('counter', 'trim', v3(-2.35, 0.1, -1.75), v3(2.35, 0.1, -1.75), v3(2.35, 0.1, 1.75), v3(-2.35, 0.1, 1.75), 0.06)
    # the back wall (-z): staves between posts
    m.box('counter', v3(0, 1.15, -1.55), (2.25, 1.05, 0.08))
    # corner and middle posts, carved heads
    for x in (-2.2, 0.0, 2.2):
        for z in (-1.6, 1.6):
            m.log('logs', v3(x, 0.0, z), v3(x, 2.35, z), 0.12, 7)
            m.box('trim', v3(x, 2.42, z), (0.15, 0.08, 0.15))
    for z in (-1.6, 1.6):
        m.beam('trim', v3(-2.45, 2.32, z), v3(2.45, 2.32, z), 0.16, 0.16)
    # the shingled gable roof along x, planked gables, a ridge beam ending in dragon heads
    gable_roof_x(m, 'roof', 'roofwood', -2.2, 2.2, -1.6, 1.6, 2.3, 3.55, thick=0.16, overhang=0.4)
    m.beam('trim', v3(-2.85, 3.58, 0), v3(2.85, 3.58, 0), 0.14, 0.14)
    for sx in (-1, 1):
        x = sx * 2.2
        m.tri('roofwood', v3(x, 2.3, -1.6), v3(x, 2.3, 1.6), v3(x, 3.5, 0), [(0, 0), (1.6, 0), (0.8, 0.6)], normal=v3(sx, 0, 0), both=True)
        m.cone('trim', v3(sx * 2.85, 3.58, 0), v3(sx * 3.25, 3.75, 0), 0.09, 5)
    # two counters at the open front (+z), goods on them
    for sx in (-1, 1):
        m.box('counter', v3(sx * 1.1, 0.55, 1.15), (0.95, 0.45, 0.32), top_part='trim')
        for k, x in enumerate((sx * 1.6, sx * 1.1, sx * 0.6)):
            cell = [0, 1, 3, 4, 2, 13][(k + (sx > 0) * 3) % 6]
            if k % 2 == 0:
                m.lathe('baskets', v3(x, 1.0, 1.15), [(0.18, 0), (0.25, 0.14), (0.27, 0.18)], sides=10, top=False)
                goods_heap(m, v3(x, 1.15, 1.15), cell, 0.24, 0.15)
            else:
                goods_heap(m, v3(x, 1.0, 1.15), cell, 0.27, 0.18)
    # scales on the left counter, a chest of silver on the right
    m.cyl('iron', v3(-0.75, 1.0, 1.05), v3(-0.75, 1.42, 1.05), 0.025, 0.02, 5)
    m.beam('iron', v3(-1.0, 1.42, 1.05), v3(-0.5, 1.42, 1.05), 0.03, 0.03)
    for x in (-1.0, -0.5):
        m.lathe('iron', v3(x, 1.16, 1.05), [(0.0, 0), (0.1, 0.03), (0.12, 0.05)], sides=8, top=False)
    m.box('trim', v3(0.0, 0.32, 0.6), (0.38, 0.22, 0.26))
    m.box('iron', v3(0.0, 0.32, 0.87), (0.05, 0.08, 0.02))
    goods_heap(m, v3(0.0, 0.54, 0.6), 5, 0.3, 0.14)
    # the hide awning stretched out in front on two poles
    for sx in (-1, 1):
        m.log('logs', v3(sx * 2.0, -0.1, 3.0), v3(sx * 2.0, 1.95, 3.0), 0.07, 6)
    seg = 6
    for i in range(seg):
        xa = -2.1 + 4.2 * i / seg; xb = -2.1 + 4.2 * (i + 1) / seg
        sag = lambda x: 0.12 * (1 - (x / 2.1) ** 2)
        m.quad('awning', v3(xa, 2.25, 1.6), v3(xb, 2.25, 1.6), v3(xb, 1.92 - sag(xb), 3.05), v3(xa, 1.92 - sag(xa), 3.05),
               [(i / seg, 1), ((i + 1) / seg, 1), ((i + 1) / seg, 0), (i / seg, 0)], normal=v3(0, 1, 0.3), both=True)
    # furs on a rack at the side, fish drying on another
    m.beam('trim', v3(2.75, 1.6, -1.2), v3(2.75, 1.6, 1.0), 0.07, 0.07)
    for z in (-1.2, 1.0):
        m.log('logs', v3(2.75, -0.1, z), v3(2.75, 1.7, z), 0.06, 5)
    for k, z in enumerate((-0.9, -0.3, 0.35)):
        a = v3(2.78, 1.55, z - 0.22); b = v3(2.78, 1.55, z + 0.22)
        m.quad('fur', a - v3(0, 0.9 + 0.1 * k, 0), b - v3(0, 0.95, 0), b, a, [(0, 0), (0.5, 0), (0.5, 0.5), (0, 0.5)], normal=v3(1, 0, 0), both=True)
    m.beam('trim', v3(-2.8, 1.45, -1.0), v3(-2.8, 1.45, 1.1), 0.06, 0.06)
    for z in (-1.0, 1.1):
        m.log('logs', v3(-2.8, -0.1, z), v3(-2.8, 1.55, z), 0.06, 5)
    for k in range(6):
        z = -0.75 + k * 0.33
        m.box('goods', v3(-2.8, 1.18, z), (0.03, 0.24, 0.07))
    # shields on the back wall, barrels and sacks about
    for x in (-1.5, -0.5, 0.5, 1.5):
        m.shield('shields', v3(x, 1.45, -1.45), v3(0, 0, 1), r=0.36)
    for (x, z) in ((-2.75, 1.7), (-2.4, 2.25), (2.55, 1.85)):
        barrel(m, 'jars', v3(x, 0.0, z), 0.28, 0.66)
    for (x, z) in ((2.7, 2.5), (-1.7, -2.2)):
        m.lathe('baskets', v3(x, -0.05, z), [(0.25, 0), (0.33, 0.25), (0.22, 0.5), (0.08, 0.6), (0.0, 0.62)], sides=9, top=False)
    banner_pole(m, v3(2.6, -0.1, -2.1), 3.9, w=0.6, h=1.1, ang=0.0)
    return m


def native_market():
    """The Natives' trading ground: a palm-thatch canopy on forked poles over woven mats laid with fruit, fish, pots
    and crystals; a feather garland, carved trade posts, baskets and big clay jars round it."""
    m = Model('market_n', 1.6)
    m.part('roof', 'bk_thatch', gloss=0.1)
    m.part('logs', BARK, gloss=0.2)
    m.part('trim', 'bk_darkwood', gloss=0.3)
    m.part('mats', 'bk_cloth', 'bk_cloth_team', gloss=0.1)
    m.part('goods', 'bk_goods', gloss=0.35)
    m.part('baskets', 'bk_thatch', gloss=0.1)
    m.part('jars', 'bk_clay', gloss=0.3)
    m.part('carved', 'bk_carved', gloss=0.3)
    m.part('feathers', 'bk_feathers', 'bk_feathers_team', gloss=0.2)
    m.part('banner', 'bk_banner', 'bk_banner_team', gloss=0.1)
    m.part('stones', 'bk_fieldstone', gloss=0.4)
    # forked poles: six round the edge, a tall one in the middle
    poles = [(math.cos(a) * 1.75, math.sin(a) * 1.75) for a in (i * math.pi / 3 + 0.3 for i in range(6))]
    for (x, z) in poles:
        m.log('logs', v3(x, -0.15, z), v3(x, 2.05, z), 0.09, 6)
        m.log('logs', v3(x, 1.9, z), v3(x - 0.12, 2.25, z), 0.04, 5)
        m.log('logs', v3(x, 1.9, z), v3(x + 0.12, 2.25, z), 0.04, 5)
    m.log('logs', v3(0, -0.15, 0), v3(0, 3.0, 0), 0.11, 7)
    # the canopy: a low hip of thatch, its eaves ragged (two layers)
    m.lathe('roof', v3(0, 2.0, 0), [(2.35, 0.0), (1.9, 0.38), (1.0, 0.95), (0.2, 1.35), (0.0, 1.42)], sides=12)
    m.lathe('roof', v3(0, 1.86, 0), [(2.5, 0.0), (2.3, 0.16)], sides=12, top=False)
    m.lathe('trim', v3(0, 1.99, 0), [(0.0, 1.35), (0.2, 1.3), (1.0, 0.88), (1.9, 0.33), (2.35, -0.01)], sides=12, top=False)
    m.log('logs', v3(0, 3.3, 0), v3(0, 3.75, 0), 0.06, 5, tip=0.2)
    # woven mats on the ground, goods laid out on them
    rng = np.random.RandomState(31)
    mats = [(-2.55, -1.15, 0.5), (2.55, -1.1, -0.45), (-2.5, 1.35, -0.4), (2.6, 1.3, 0.35), (0.0, -2.7, 0.0)]
    for k, (x, z, a) in enumerate(mats):
        with m.at((x, 0.03, z), a):
            m.quad('mats', v3(-0.85, 0, -0.6), v3(0.85, 0, -0.6), v3(0.85, 0, 0.6), v3(-0.85, 0, 0.6),
                   [(0, 0.1), (0.9, 0.1), (0.9, 0.9), (0, 0.9)], normal=v3(0, 1, 0))
            for j, (gx, gz) in enumerate(((-0.45, -0.25), (0.15, -0.3), (0.5, 0.2), (-0.3, 0.3))):
                cell = [0, 1, 3, 4, 2, 13, 9, 14][(k * 3 + j) % 8]
                if j % 2 == 0:
                    goods_heap(m, v3(gx, 0.0, gz), cell, 0.22, 0.16)
                else:
                    m.lathe('jars', v3(gx, 0.0, gz), [(0.1, 0), (0.18, 0.15), (0.14, 0.3), (0.08, 0.36), (0.1, 0.4)], sides=9, uwrap=1.0, vspan=1.0)
    # crystals glinting by the middle pole
    for i in range(4):
        p = v3(0.35 + 0.15 * (i - 1.5), 0.0, 0.3 + 0.05 * i)
        m.cone('stones', p, p + v3(0.03, 0.25 + 0.1 * (i % 2), 0.02), 0.07, 5)
    # a garland of feathers between the front poles
    for i in range(9):
        t = i / 8
        a = 0.3 + math.pi / 3 * t
        x = math.cos(a) * 2.0; z = math.sin(a) * 2.0; y = 1.85 - 0.3 * math.sin(math.pi * t)
        feather(m, 'feathers', v3(x, y, z), v3(0.1, -1, 0.15), 0.4, 0.12, v3(math.cos(a), 0, math.sin(a)), i)
    # carved trade posts at the entrance, big jars and baskets round the edge
    for sx in (-1, 1):
        p = v3(sx * 0.9, -0.2, 3.2)
        m.cyl('carved', p, p + v3(0, 1.9, 0), 0.15, 0.13, 10, uwrap=1.0, vspan=1.0)
        m.cone('carved', p + v3(0, 1.9, 0), p + v3(0, 2.15, 0), 0.15, 10)
    for (x, z) in ((-3.4, 0.2), (-3.3, -0.55), (3.45, 0.1), (3.3, -0.55)):
        m.lathe('jars', v3(x, -0.05, z), [(0.15, 0), (0.32, 0.3), (0.27, 0.62), (0.13, 0.78), (0.17, 0.86)], sides=10, uwrap=1.0, vspan=1.0)
    for (x, z) in ((-1.6, -2.85), (1.7, -2.7), (1.6, 2.9)):
        m.lathe('baskets', v3(x, -0.05, z), [(0.25, 0), (0.33, 0.25), (0.22, 0.5), (0.08, 0.6), (0.0, 0.62)], sides=9, top=False)
    banner_pole(m, v3(-1.9, -0.1, 3.0), 3.6, w=0.55, h=1.0, ang=math.pi)
    return m


def shrine(vik):
    m = Model('shrine_' + ('v' if vik else 'n'), 1.45)
    m.part('stones', 'bk_runestone' if vik else 'bk_fieldstone', gloss=0.4)
    m.part('dais', 'bk_stone', gloss=0.45)
    m.part('carved', 'bk_carved' if not vik else 'bk_runestone', gloss=0.3)
    m.part('feathers', 'bk_feathers', 'bk_feathers_team', gloss=0.2)
    m.part('clay', 'bk_clay', gloss=0.3)
    rng = np.random.RandomState(77)
    # eight standing stones on a circle
    for i in range(8):
        a = i * 2 * math.pi / 8
        c = v3(math.cos(a) * 3.1, -0.2, math.sin(a) * 3.1)
        h = 1.9 + rng.rand() * 0.8
        w, d = 0.38 + rng.rand() * 0.1, 0.24
        m.obox('stones', c + v3(0, h / 2, 0), v3(-math.sin(a), 0, math.cos(a)), v3(0, 1, 0), v3(math.cos(a), 0, math.sin(a)),
               w, h / 2, d, taper=0.72, tile=3.4, uoff=rng.rand())
    # a stepped dais
    m.lathe('dais', v3(0, -0.25, 0), [(1.35, 0), (1.35, 0.4), (1.0, 0.4), (1.0, 0.7)], sides=16, top=True)
    # the centre: a carved pole (Natives) / a rune pillar (Vikings)
    if vik:
        m.box('carved', v3(0, 2.1, 0), (0.33, 1.45, 0.33), taper=0.75, tile=3.0)
        m.lathe('clay', v3(0, 3.5, 0), [(0.1, 0), (0.35, 0.15), (0.42, 0.3)], sides=12, top=True)   # the fire bowl
    else:
        m.cyl('carved', v3(0, 0.4, 0), v3(0, 3.7, 0), 0.34, 0.3, 12, uwrap=1.0, vspan=1.0)
        m.lathe('carved', v3(0, 3.65, 0), [(0.34, 0), (0.38, 0.06), (0.0, 0.2)], sides=12, uvrect=(0, 0.9, 1, 1))
        for s in (-1, 1):
            wing(m, 'feathers', v3(0, 3.25, s * 0.3), v3(0, 0.5, s), v3(0, 1, 0), n=7, length=1.3, spread=1.3, width=0.26)
    # offering bowls round the dais
    for i in range(4):
        a = i * math.pi / 2 + 0.4
        m.lathe('clay', v3(math.cos(a) * 1.7, -0.1, math.sin(a) * 1.7), [(0.08, 0), (0.22, 0.12), (0.25, 0.2)], sides=10, top=True)
    return m


ALL = [mead_hall, spirit_lodge, rune_forge, crystal_forge, rune_stone, war_totem,
       lambda: dock(True), lambda: dock(False), lambda: market(True), lambda: market(False),
       lambda: shrine(True), lambda: shrine(False)]

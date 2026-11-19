#!/usr/bin/env python3
"""
Tribal Trouble geometry -> glTF 2.0 (.glb) converter for the Unity port.

Reads the original asset sources (assets/geometry/geometry.xml plus the
mesh/skeleton/animation XML files it references) and writes, per sprite:

    <out>/<group>/<sprite>.glb      geometry + skin + animations (no materials)
    <out>/catalog.json              everything the Unity side needs to rebuild
                                    materials, LODs and animation timing

Conventions reproduced from the Java pipeline (tools/.../converter):
  * XML matrix attributes mRC are JOML-style: math matrix A[row][col] = m{col}{row}
    (translation lives in m30, m31, m32).
  * Skeleton/animation matrices are ABSOLUTE (model space). Java skinning is
    v' = sum w * (F * I^-1) * v, with I = init pose, F = frame pose.
    glTF expresses this exactly: inverseBindMatrix = I^-1, joint world = F.
  * Bone hierarchy: parent map from <bones>; extra roots are re-parented under
    the first root, exactly like SkeletonLoader does.
  * Sprite "scale" multiplies mesh vertices only (only used on static props).
  * TT is right-handed Z-up; glTF is right-handed Y-up: (x, y, z) -> (x, z, -y).
  * Animation timing: TT samples frame = (anim_ticks / wpc) * num_frames, with
    linear blending and wrap-around for LOOP / clamp for PLAIN. We author each
    clip as exactly 1 second == 1 cycle: key i at t = i / N, and LOOP clips get
    frame 0 repeated at t = 1.0. Unity then samples clip time = anim_ticks / wpc.

Only numpy is required. Usage:
    python3 tt_geometry_to_gltf.py <assets/geometry dir> <output dir> [--only group/sprite ...]
"""
import json
import math
import os
import struct
import sys
import xml.etree.ElementTree as ET

import numpy as np

# TT (Z-up) -> glTF (Y-up), a proper rotation (det = +1) so winding is preserved.
C = np.array([[1, 0, 0, 0],
              [0, 0, 1, 0],
              [0, -1, 0, 0],
              [0, 0, 0, 1]], dtype=np.float64)
C_INV = C.T


def conv_matrix(a):
    return C @ a @ C_INV


def conv_vec(v):
    # v: (n, 3)
    return np.stack([v[:, 0], v[:, 2], -v[:, 1]], axis=1)


def parse_transform(t):
    return np.array([[float(t.get(f"m{c}{r}")) for c in range(4)] for r in range(4)], dtype=np.float64)


def parse_frame(node):
    return {t.get("name"): parse_transform(t) for t in node.findall("transform")}


# ---------------------------------------------------------------- skeleton

class Skeleton:
    def __init__(self, path):
        root = ET.parse(path).getroot()
        self.init_pose = parse_frame(root.find("init_pose"))
        parent_map = {}
        for b in root.find("bones").findall("bone"):
            parent_map[b.get("name")] = b.get("parent")
        # Same root resolution as SkeletonLoader (keeps first root found).
        first_root = None
        order = list(parent_map.keys())
        for name in order:
            p = parent_map[name]
            if p not in parent_map:
                if first_root is None:
                    first_root = name
                    parent_map[name] = None
                else:
                    print(f"  WARNING multiple roots in {os.path.basename(path)}: {name} -> {first_root}")
                    parent_map[name] = first_root
        self.parent = parent_map
        # Topological order (parents before children), stable.
        children = {n: [] for n in parent_map}
        for n, p in parent_map.items():
            if p is not None:
                children[p].append(n)
        self.order = []

        def visit(n):
            self.order.append(n)
            for c in children[n]:
                visit(c)
        visit(first_root)
        missing = [n for n in parent_map if n not in self.order]
        if missing:
            raise ValueError(f"unreachable bones: {missing}")
        self.index = {n: i for i, n in enumerate(self.order)}


def to_trs(m):
    t = m[:3, 3].copy()
    r = m[:3, :3]
    s = np.linalg.norm(r, axis=0)
    if (s < 1e-6).any():
        # Degenerate (bone collapsed to hide a prop, e.g. peon paddles): keep the
        # rotation of the non-degenerate axes where possible, else identity.
        if (s >= 1e-6).sum() == 0:
            return t, np.array([0.0, 0.0, 0.0, 1.0]), np.zeros(3)
        rn = np.zeros((3, 3))
        for i in range(3):
            rn[:, i] = r[:, i] / s[i] if s[i] >= 1e-6 else 0
        u, _, vt = np.linalg.svd(rn)
        rot = u @ vt
        if np.linalg.det(rot) < 0:
            u[:, -1] = -u[:, -1]
            rot = u @ vt
        s = np.where(s < 1e-6, 0.0, s)
        return t, mat_to_quat(rot), s
    rn = r / s
    if np.linalg.det(rn) < 0:  # reflection guard (should not happen with Max rigs)
        s[0] = -s[0]
        rn[:, 0] = -rn[:, 0]
    # Re-orthonormalise via SVD to get a clean rotation.
    u, _, vt = np.linalg.svd(rn)
    rn = u @ vt
    q = mat_to_quat(rn)
    return t, q, s


def safe_inv(m):
    try:
        if abs(np.linalg.det(m[:3, :3])) > 1e-12:
            return np.linalg.inv(m)
    except np.linalg.LinAlgError:
        pass
    return np.linalg.pinv(m)


def mat_to_quat(m):
    tr = m[0, 0] + m[1, 1] + m[2, 2]
    if tr > 0:
        s = math.sqrt(tr + 1.0) * 2
        w = 0.25 * s
        x = (m[2, 1] - m[1, 2]) / s
        y = (m[0, 2] - m[2, 0]) / s
        z = (m[1, 0] - m[0, 1]) / s
    elif m[0, 0] > m[1, 1] and m[0, 0] > m[2, 2]:
        s = math.sqrt(1.0 + m[0, 0] - m[1, 1] - m[2, 2]) * 2
        w = (m[2, 1] - m[1, 2]) / s
        x = 0.25 * s
        y = (m[0, 1] + m[1, 0]) / s
        z = (m[0, 2] + m[2, 0]) / s
    elif m[1, 1] > m[2, 2]:
        s = math.sqrt(1.0 + m[1, 1] - m[0, 0] - m[2, 2]) * 2
        w = (m[0, 2] - m[2, 0]) / s
        x = (m[0, 1] + m[1, 0]) / s
        y = 0.25 * s
        z = (m[1, 2] + m[2, 1]) / s
    else:
        s = math.sqrt(1.0 + m[2, 2] - m[0, 0] - m[1, 1]) * 2
        w = (m[1, 0] - m[0, 1]) / s
        x = (m[0, 2] + m[2, 0]) / s
        y = (m[1, 2] + m[2, 1]) / s
        z = 0.25 * s
    q = np.array([x, y, z, w])
    return q / np.linalg.norm(q)


# ---------------------------------------------------------------- mesh

def load_mesh(path, skeleton, scale):
    root = ET.parse(path).getroot()
    tex_attr = root.get("texture", "")
    polys = root.find("polygons").findall("polygon")
    keys = {}
    pos, nrm, uv, col, joints, weights = [], [], [], [], [], []
    indices = []
    max_infl = 0
    for poly in polys:
        verts = poly.findall("vertex")
        if len(verts) != 3:
            raise ValueError(f"{path}: mesh not triangulated")
        for v in verts:
            p = (float(v.get("x")) * scale, float(v.get("y")) * scale, float(v.get("z")) * scale)
            n = np.array([float(v.get("nx", 0)), float(v.get("ny", 0)), float(v.get("nz", 0))])
            ln = np.linalg.norm(n)
            n = tuple(n / ln) if ln > 0 else (0.0, 0.0, 1.0)
            t = (float(v.get("u")), float(v.get("v")))
            c = (float(v.get("r", 1)), float(v.get("g", 1)), float(v.get("b", 1)), float(v.get("a", 1)))
            skins = []
            for s in v.findall("skin"):
                w = float(s.get("weight", 1))
                bi = skeleton.index[s.get("bone")] if skeleton is not None else 0
                skins.append((w, bi))
            max_infl = max(max_infl, len(skins))
            skins.sort(key=lambda x: -x[0])
            skins = skins[:4]
            tw = sum(w for w, _ in skins) or 1.0
            j4 = [b for _, b in skins] + [0] * (4 - len(skins))
            w4 = [w / tw for w, _ in skins] + [0.0] * (4 - len(skins))
            if skeleton is None:
                j4, w4 = [0, 0, 0, 0], [1.0, 0, 0, 0]
            key = (tuple(round(x, 6) for x in p), tuple(round(x, 5) for x in n), tuple(round(x, 6) for x in t),
                   c, tuple(j4), tuple(round(x, 5) for x in w4))
            idx = keys.get(key)
            if idx is None:
                idx = len(pos)
                keys[key] = idx
                pos.append(p); nrm.append(n); uv.append(t); col.append(c); joints.append(j4); weights.append(w4)
            indices.append(idx)
    return {
        "texture_attr": tex_attr,
        "pos": conv_vec(np.array(pos, dtype=np.float64)).astype(np.float32),
        "nrm": conv_vec(np.array(nrm, dtype=np.float64)).astype(np.float32),
        # OpenGL bottom-left UV origin -> glTF top-left.
        "uv": np.array([(u, 1.0 - v) for u, v in uv], dtype=np.float32),
        "col": np.array(col, dtype=np.float32),
        "joints": np.array(joints, dtype=np.uint16),
        "weights": np.array(weights, dtype=np.float32),
        "indices": np.array(indices, dtype=np.uint32),
        "max_influences": max_infl,
        "triangles": len(polys),
    }


# ---------------------------------------------------------------- glTF writer

class GltfBuilder:
    def __init__(self):
        self.bin = bytearray()
        self.gltf = {"asset": {"version": "2.0", "generator": "tt_geometry_to_gltf"},
                     "buffers": [], "bufferViews": [], "accessors": [], "nodes": [], "meshes": [],
                     "scenes": [{"nodes": []}], "scene": 0}

    def _view(self, data, target=None):
        while len(self.bin) % 4:
            self.bin.append(0)
        off = len(self.bin)
        self.bin.extend(data)
        bv = {"buffer": 0, "byteOffset": off, "byteLength": len(data)}
        if target:
            bv["target"] = target
        self.gltf["bufferViews"].append(bv)
        return len(self.gltf["bufferViews"]) - 1

    def accessor(self, arr, gltf_type, comp, target=None, minmax=False, normalized=False):
        arr = np.ascontiguousarray(arr)
        view = self._view(arr.tobytes(), target)
        count = arr.shape[0]
        acc = {"bufferView": view, "componentType": comp, "count": int(count), "type": gltf_type}
        if normalized:
            acc["normalized"] = True
        if minmax:
            a2 = arr.reshape(count, -1)
            acc["min"] = [float(x) for x in a2.min(axis=0)]
            acc["max"] = [float(x) for x in a2.max(axis=0)]
        self.gltf["accessors"].append(acc)
        return len(self.gltf["accessors"]) - 1

    def node(self, **kw):
        self.gltf["nodes"].append(kw)
        return len(self.gltf["nodes"]) - 1

    def write_glb(self, path):
        self.gltf["buffers"] = [{"byteLength": len(self.bin)}]
        js = json.dumps(self.gltf, separators=(",", ":")).encode("utf-8")
        while len(js) % 4:
            js += b" "
        b = bytes(self.bin)
        while len(b) % 4:
            b += b"\0"
        total = 12 + 8 + len(js) + 8 + len(b)
        with open(path, "wb") as f:
            f.write(struct.pack("<4sII", b"glTF", 2, total))
            f.write(struct.pack("<I4s", len(js), b"JSON"))
            f.write(js)
            f.write(struct.pack("<I4s", len(b), b"BIN\0"))
            f.write(b)


FLOAT, USHORT, UINT = 5126, 5123, 5125
ARRAY_BUFFER, ELEMENT_ARRAY_BUFFER = 34962, 34963


def _path(el):
    """A file reference: the 1.4 format writes it as the element's text, the Steam release as a path attribute."""
    return (el.get("path") or el.text or "").strip()


# Steam release: the sprite attributes the registry is built from (an item in a slot of its base sprite, a skin
# standing in for another sprite, scenery of an event)
STEAM_SPRITE_ATTRS = ("base", "slot", "default", "event", "skin", "replaces", "decoration", "count")
STEAM_TEXTURE_ATTRS = ("emissive", "normal", "light_color", "light_strength", "light_reach")


def build_sprite(geom_dir, sprite_el, group_sprites=None):
    name = sprite_el.get("name")
    scale = float(sprite_el.get("scale", 1))
    skel_el = sprite_el.find("skeleton")
    anim_els = sprite_el.findall("animation")
    # Steam: base="peon" makes an item share its base sprite's skeleton and clips (ConvertToBinary does the same)
    base_el, depth = sprite_el, 0
    while skel_el is None and not anim_els and base_el.get("base") and group_sprites:
        base_el = group_sprites.get(base_el.get("base"))
        depth += 1
        if base_el is None or depth > len(group_sprites):
            raise ValueError("unknown or circular base of " + name)
        skel_el, anim_els = base_el.find("skeleton"), base_el.findall("animation")
    skeleton = Skeleton(os.path.join(geom_dir, _path(skel_el))) if skel_el is not None else None

    g = GltfBuilder()
    root_node = g.node(name=name, children=[])
    g.gltf["scenes"][0]["nodes"].append(root_node)
    info = {"name": name, "scale": scale, "skinned": skeleton is not None, "models": [], "animations": []}
    for a in STEAM_SPRITE_ATTRS:
        if sprite_el.get(a) is not None:
            info[a] = sprite_el.get(a)

    joint_nodes = []
    skin_index = None
    if skeleton is not None:
        abs_init = {n: conv_matrix(skeleton.init_pose[n]) for n in skeleton.order}
        for n in skeleton.order:
            p = skeleton.parent[n]
            local = abs_init[n] if p is None else safe_inv(abs_init[p]) @ abs_init[n]
            t, q, s = to_trs(local)
            joint_nodes.append(g.node(name=n, translation=[float(x) for x in t],
                                      rotation=[float(x) for x in q], scale=[float(x) for x in s]))
        for n in skeleton.order:
            p = skeleton.parent[n]
            ni = joint_nodes[skeleton.index[n]]
            if p is None:
                g.gltf["nodes"][root_node]["children"].append(ni)
            else:
                pn = g.gltf["nodes"][joint_nodes[skeleton.index[p]]]
                pn.setdefault("children", []).append(ni)
        ibm = np.stack([np.linalg.inv(abs_init[n]).T.reshape(16) for n in skeleton.order]).astype(np.float32)
        g.gltf["skins"] = [{"joints": joint_nodes, "skeleton": joint_nodes[0],
                            "inverseBindMatrices": g.accessor(ibm, "MAT4", FLOAT)}]
        skin_index = 0

    for mi, model_el in enumerate(sprite_el.findall("model")):
        mesh_rel = _path(model_el)
        mesh = load_mesh(os.path.join(geom_dir, mesh_rel), skeleton, scale)
        attrs = {
            "POSITION": g.accessor(mesh["pos"], "VEC3", FLOAT, ARRAY_BUFFER, minmax=True),
            "NORMAL": g.accessor(mesh["nrm"], "VEC3", FLOAT, ARRAY_BUFFER),
            "TEXCOORD_0": g.accessor(mesh["uv"], "VEC2", FLOAT, ARRAY_BUFFER),
        }
        if not np.allclose(mesh["col"], 1.0):
            attrs["COLOR_0"] = g.accessor(mesh["col"], "VEC4", FLOAT, ARRAY_BUFFER)
        if skeleton is not None:
            attrs["JOINTS_0"] = g.accessor(mesh["joints"], "VEC4", USHORT, ARRAY_BUFFER)
            attrs["WEIGHTS_0"] = g.accessor(mesh["weights"], "VEC4", FLOAT, ARRAY_BUFFER)
        nverts = mesh["pos"].shape[0]
        idx = mesh["indices"].astype(np.uint16 if nverts < 65536 else np.uint32)
        prim = {"attributes": attrs,
                "indices": g.accessor(idx, "SCALAR", USHORT if idx.dtype == np.uint16 else UINT,
                                      ELEMENT_ARRAY_BUFFER),
                "mode": 4}
        g.gltf["meshes"].append({"name": f"lod{mi}", "primitives": [prim]})
        node_kw = {"name": f"lod{mi}", "mesh": len(g.gltf["meshes"]) - 1}
        if skin_index is not None:
            node_kw["skin"] = skin_index
        g.gltf["nodes"][root_node]["children"].append(g.node(**node_kw))
        textures = []
        for t in model_el.findall("texture"):
            tex = {"name": t.get("name"), "team": t.get("team")}
            for a in STEAM_TEXTURE_ATTRS:
                if t.get(a) is not None:
                    tex[a] = t.get(a)
            textures.append(tex)
        info["models"].append({
            "lod": mi, "source": mesh_rel, "textures": textures,
            "clearColor": [int(model_el.get("r")) / 255, int(model_el.get("g")) / 255, int(model_el.get("b")) / 255],
            "vertices": int(nverts), "triangles": mesh["triangles"], "maxInfluences": mesh["max_influences"],
            "boundsMin": [float(x) for x in mesh["pos"].min(axis=0)],
            "boundsMax": [float(x) for x in mesh["pos"].max(axis=0)],
        })

    if skeleton is not None:
        anims = []
        for anim_el in anim_els:
            aroot = ET.parse(os.path.join(geom_dir, _path(anim_el))).getroot()
            frames = {}
            for f in aroot.findall("frame"):
                frames[int(f.get("index"))] = parse_frame(f)
            nframes = len(frames)
            frame_list = [frames[i] for i in range(nframes)]
            loop = anim_el.get("type") == "loop"
            times = [i / nframes for i in range(nframes)]
            if loop:
                frame_list = frame_list + [frame_list[0]]
                times.append(1.0)
            t_acc = g.accessor(np.array(times, dtype=np.float32), "SCALAR", FLOAT, minmax=True)
            samplers, channels = [], []
            for n in skeleton.order:
                p = skeleton.parent[n]
                ts, qs, ss = [], [], []
                prev_q = None
                for fr in frame_list:
                    a = conv_matrix(fr[n])
                    local = a if p is None else safe_inv(conv_matrix(fr[p])) @ a
                    t, q, s = to_trs(local)
                    if prev_q is not None and np.dot(prev_q, q) < 0:
                        q = -q  # keep quaternion hemisphere continuous for LINEAR slerp
                    prev_q = q
                    ts.append(t); qs.append(q); ss.append(s)
                node = joint_nodes[skeleton.index[n]]
                for path, data, typ in (("translation", ts, "VEC3"), ("rotation", qs, "VEC4"), ("scale", ss, "VEC3")):
                    arr = np.array(data, dtype=np.float32)
                    if path == "scale" and np.allclose(arr, 1.0, atol=1e-4):
                        continue
                    samplers.append({"input": t_acc, "output": g.accessor(arr, typ, FLOAT), "interpolation": "LINEAR"})
                    channels.append({"sampler": len(samplers) - 1, "target": {"node": node, "path": path}})
            anims.append({"name": anim_el.get("name"), "samplers": samplers, "channels": channels})
            info["animations"].append({"name": anim_el.get("name"), "wpc": float(anim_el.get("wpc")),
                                       "type": anim_el.get("type"), "frames": nframes})
        if anims:
            g.gltf["animations"] = anims
    return g, info


def main(argv):
    if len(argv) < 3:
        print(__doc__)
        return 2
    geom_dir, out_dir = argv[1], argv[2]
    only = set(argv[argv.index("--only") + 1:]) if "--only" in argv else None
    root = ET.parse(os.path.join(geom_dir, "geometry.xml")).getroot()
    catalog = {"source": "assets/geometry/geometry.xml", "units": "meters", "upAxis": "Y (converted from TT Z-up)",
               "animationClipSeconds": 1.0, "groups": []}
    failures = []
    for group in root.findall("group"):
        gname = group.get("name")
        ginfo = {"name": gname, "sprites": []}
        os.makedirs(os.path.join(out_dir, gname), exist_ok=True)
        group_sprites = {sp.get("name"): sp for sp in group.findall("sprite")}
        for sprite in group.findall("sprite"):
            key = f"{gname}/{sprite.get('name')}"
            if only and key not in only:
                continue
            try:
                g, info = build_sprite(geom_dir, sprite, group_sprites)
                info["file"] = f"{gname}/{info['name']}.glb"
                g.write_glb(os.path.join(out_dir, info["file"]))
                ginfo["sprites"].append(info)
                print(f"ok   {key}: {len(info['models'])} lod(s), {len(info['animations'])} anim(s)")
            except Exception as e:  # keep going; report at the end
                failures.append((key, repr(e)))
                print(f"FAIL {key}: {e!r}")
        catalog["groups"].append(ginfo)
    with open(os.path.join(out_dir, "catalog.json"), "w") as f:
        json.dump(catalog, f, indent=1)
    print(f"\n{sum(len(g['sprites']) for g in catalog['groups'])} sprites written, {len(failures)} failed")
    for k, e in failures:
        print("  ", k, e)
    return 1 if failures else 0


if __name__ == "__main__":
    sys.exit(main(sys.argv))

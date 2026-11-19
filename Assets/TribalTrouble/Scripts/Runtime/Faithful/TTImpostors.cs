using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Impostors for the realistic trees and palms: each variant is photographed once (front and side, albedo only)
    /// into a texture, and far trees are drawn as two crossed cards with that picture. They keep the realistic look
    /// all the way to the horizon (no switch back to the original models) at a tiny fraction of the cost.
    /// </summary>
    public static class TTImpostors
    {
        static readonly Dictionary<TTRealTrees.Variant, TTRealTrees.Part> s_cache = new Dictionary<TTRealTrees.Variant, TTRealTrees.Part>();
        static Shader s_capture;
        static readonly Dictionary<Material, Material> s_captureMats = new Dictionary<Material, Material>();
        static int s_dumped;
        static Material s_plainCapture;

        const float Cutoff = 0.4f;

        /// <summary>
        /// Reads the capture back and builds the mip chain so each level keeps the leaf coverage of the full picture
        /// (plain averaging thins alpha-tested foliage away in the small mips: far trees turned into bare sticks).
        /// </summary>
        static Mesh s_marker;
        static Mesh Marker()
        {
            if (s_marker != null) return s_marker;
            s_marker = new Mesh { name = "Impostor marker", hideFlags = HideFlags.DontSave };
            s_marker.SetVertices(new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 1, 0), new Vector3(0, 1, 0) });
            s_marker.SetUVs(0, new List<Vector2> { Vector2.zero, Vector2.right, Vector2.one, Vector2.up });
            s_marker.SetTriangles(new[] { 0, 2, 1, 0, 3, 2 }, 0);
            return s_marker;
        }

        /// <summary>Finds the orientation marker (bottom-left of the picture, or top-left when the API flipped it),
        /// flips the rows if needed and erases the marker.</summary>
        static void FixOrientation(Texture2D tex, int markerPx)
        {
            int w = tex.width, h = tex.height;
            var px = tex.GetPixels32(0);
            bool bottom = px[1 * w + 1].a > 128, top = px[(h - 2) * w + 1].a > 128;
            if (top && !bottom)
            {
                var row = new Color32[w];
                for (int y = 0; y < h / 2; y++)
                {
                    System.Array.Copy(px, y * w, row, 0, w);
                    System.Array.Copy(px, (h - 1 - y) * w, px, y * w, w);
                    System.Array.Copy(row, 0, px, (h - 1 - y) * w, w);
                }
            }
            for (int y = 0; y < markerPx + 2 && y < h; y++)
                for (int x = 0; x < markerPx + 2 && x < w; x++) px[y * w + x].a = 0;
            tex.SetPixels32(px, 0);
        }

        static Texture2D CoveragePreservingTexture(RenderTexture rt, float cutoff, int markerPx)
        {
            var prev = RenderTexture.active;
            RenderTexture.active = rt;
            var tex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, true, false)
            {
                name = "Tree impostor", wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Trilinear, anisoLevel = 4, hideFlags = HideFlags.DontSave,
            };
            tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0, false);
            RenderTexture.active = prev;
            FixOrientation(tex, markerPx);
            tex.Apply(true, false);
            if (Application.isEditor && s_dumped < 6)
            {
                // debugging aid: the first few captures go to <persistentDataPath>/impostors
                try
                {
                    var dir = System.IO.Path.Combine(Application.persistentDataPath, "impostors");
                    System.IO.Directory.CreateDirectory(dir);
                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(dir, "impostor" + s_dumped + ".png"), tex.EncodeToPNG());
                }
                catch (System.Exception) { }
                s_dumped++;
            }
            float target = Coverage(tex.GetPixels32(0), cutoff, 1f);
            for (int mip = 1; mip < tex.mipmapCount; mip++)
            {
                var px = tex.GetPixels32(mip);
                float lo = 1f, hi = 8f;
                for (int it = 0; it < 12; it++)
                {
                    float mid = (lo + hi) * 0.5f;
                    if (Coverage(px, cutoff, mid) < target) lo = mid; else hi = mid;
                }
                for (int i = 0; i < px.Length; i++) px[i].a = (byte)Mathf.Min(255, px[i].a * hi);
                tex.SetPixels32(px, mip);
            }
            tex.Apply(false, true);   // done: free the CPU copy
            return tex;
        }

        static float Coverage(Color32[] px, float cutoff, float scale)
        {
            int n = 0, limit = Mathf.CeilToInt(cutoff * 255f / scale);
            for (int i = 0; i < px.Length; i++) if (px[i].a >= limit) n++;
            return n / (float)px.Length;
        }

        public static TTRealTrees.Part Get(TTRealTrees.Variant v, Material baseMaterial)
        {
            if (v == null) return null;
            if (s_cache.TryGetValue(v, out var p)) return p;
            if (v.tintOf != null)
            {
                // a shade of another model: its impostor, tinted (no capture of its own)
                var bp = Get(v.tintOf, baseMaterial);
                if (bp != null)
                {
                    var m = new Material(bp.material) { name = bp.material.name + " shaded" };
                    if (m.HasProperty("_Tint")) m.SetColor("_Tint", bp.material.GetColor("_Tint") * v.tintMul);
                    p = new TTRealTrees.Part { mesh = bp.mesh, submesh = bp.submesh, material = m, local = bp.local };
                }
                s_cache[v] = p;
                return p;
            }
            p = Build(v, baseMaterial);
            s_cache[v] = p;
            return p;
        }

        static Material CaptureMaterial(Material src)
        {
            if (src == null && s_plainCapture != null) return s_plainCapture;
            if (src != null && s_captureMats.TryGetValue(src, out var m)) return m;
            var tex = src != null && src.HasProperty("_BaseMap") ? src.GetTexture("_BaseMap") : null;
            m = new Material(s_capture) { hideFlags = HideFlags.DontSave };
            m.SetTexture("_BaseMap", tex != null ? tex : Texture2D.whiteTexture);
            m.SetFloat("_Cutoff", src != null && src.IsKeywordEnabled("_ALPHATEST_ON") ? Mathf.Clamp(src.GetFloat("_Cutoff"), 0.2f, 0.5f) : -1f);
            m.SetColor("_Tint", src != null && src.HasProperty("_Tint") ? src.GetColor("_Tint") : Color.white);
            if (src != null) s_captureMats[src] = m; else s_plainCapture = m;
            return m;
        }

        static TTRealTrees.Part Build(TTRealTrees.Variant v, Material baseMaterial)
        {
            s_capture ??= Shader.Find("TribalTrouble/ImpostorCapture");
            if (s_capture == null || v.parts.Count == 0) return null;
            // bounds of the tree in its own space
            var min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var max = -min;
            foreach (var part in v.parts)
            {
                var b = part.mesh.bounds;
                for (int i = 0; i < 8; i++)
                {
                    var c = new Vector3((i & 1) != 0 ? b.max.x : b.min.x, (i & 2) != 0 ? b.max.y : b.min.y, (i & 4) != 0 ? b.max.z : b.min.z);
                    var w = part.local.MultiplyPoint3x4(c);
                    min = Vector3.Min(min, w);
                    max = Vector3.Max(max, w);
                }
            }
            float half = Mathf.Max(Mathf.Max(Mathf.Abs(min.x), Mathf.Abs(max.x)), Mathf.Max(Mathf.Abs(min.z), Mathf.Abs(max.z))) * 1.02f;
            float bottom = Mathf.Min(0f, min.y), top = max.y * 1.01f, h = top - bottom;
            if (half <= 0.01f || h <= 0.01f) return null;

            // two views side by side: front (looking along +z) and side (looking along -x)
            const int H = 512;
            int W = Mathf.Clamp(Mathf.NextPowerOfTwo(Mathf.RoundToInt(H * (2f * half) / h)), 64, 512);
            var rt = new RenderTexture(W * 2, H, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB)
            {
                useMipMap = false, wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Bilinear,
                name = "Tree impostor capture", hideFlags = HideFlags.DontSave,
            };
            rt.Create();
            var cmd = new CommandBuffer { name = "Tree impostor" };
            cmd.SetRenderTarget(rt);
            cmd.ClearRenderTarget(true, true, new Color(0.22f, 0.27f, 0.13f, 0f));
            // CommandBuffer.SetViewProjectionMatrices makes the matrix GPU-ready for the render target itself
            // (converting it here as well flipped the picture upside down on DirectX)
            var proj = Matrix4x4.Ortho(-half, half, bottom, top, 0.01f, 4f * half + 10f);
            for (int view = 0; view < 2; view++)
            {
                cmd.SetViewport(new Rect(view * W, 0, W, H));
                // camera at distance looking at the trunk: front = from -z, side = from +x
                var rot = view == 0 ? Quaternion.identity : Quaternion.Euler(0, -90f, 0);
                var pos = rot * new Vector3(0, 0, -(2f * half + 5f));
                var camToWorld = Matrix4x4.TRS(pos, rot, new Vector3(1, 1, -1));
                cmd.SetViewProjectionMatrices(camToWorld.inverse, proj);
                foreach (var part in v.parts) cmd.DrawMesh(part.mesh, part.local, CaptureMaterial(part.material), part.submesh, 0);
                // orientation marker in the bottom-left corner of the front view: whether the picture comes back
                // upside down depends on the graphics API, so the readback looks for it and flips if needed
                if (view == 0) cmd.DrawMesh(Marker(), Matrix4x4.TRS(new Vector3(-half, bottom, 0), Quaternion.identity, Vector3.one * (4f * 2f * half / W)), CaptureMaterial(null), 0, 0);
            }
            Graphics.ExecuteCommandBuffer(cmd);
            cmd.Release();
            var tex = CoveragePreservingTexture(rt, Cutoff, 4);
            rt.Release();
            Object.Destroy(rt);

            // two crossed cards, base at the root, each showing its view
            var mesh = new Mesh { name = "Tree impostor cards" };
            var verts = new List<Vector3>(); var uvs = new List<Vector2>(); var nrm = new List<Vector3>(); var tris = new List<int>();
            void Card(Vector3 axis, float u0, float u1)
            {
                int b0 = verts.Count;
                verts.Add(-axis * half + Vector3.up * bottom); verts.Add(axis * half + Vector3.up * bottom);
                verts.Add(axis * half + Vector3.up * top); verts.Add(-axis * half + Vector3.up * top);
                uvs.Add(new Vector2(u0, 0)); uvs.Add(new Vector2(u1, 0)); uvs.Add(new Vector2(u1, 1)); uvs.Add(new Vector2(u0, 1));
                // normals lean up so the cards light like a canopy, not like flat walls
                var n = (Vector3.Cross(axis, Vector3.up).normalized * 0.35f + Vector3.up).normalized;
                for (int i = 0; i < 4; i++) nrm.Add(n);
                tris.AddRange(new[] { b0, b0 + 2, b0 + 1, b0, b0 + 3, b0 + 2 });
            }
            Card(Vector3.right, 0f, 0.5f);      // front view, in the x-y plane
            Card(Vector3.forward, 0.5f, 1f);    // side view, in the z-y plane
            mesh.SetVertices(verts); mesh.SetUVs(0, uvs); mesh.SetNormals(nrm); mesh.SetTriangles(tris, 0);
            mesh.RecalculateBounds();

            var mat = baseMaterial != null ? new Material(baseMaterial) : new Material(Shader.Find("TribalTrouble/Sprite"));
            mat.name = "Tree impostor";
            mat.enableInstancing = true;
            mat.SetTexture("_BaseMap", tex);
            mat.SetColor("_Tint", Color.white);
            mat.SetFloat("_UseTeamMask", 0f);
            mat.SetFloat("_Wind", 1f);   // the same sway as the full tree it stands in for
            mat.SetFloat("_Gloss", 0.2f);
            mat.SetFloat("_Relief", 0f);
            mat.EnableKeyword("_ALPHATEST_ON");
            mat.SetFloat("_AlphaClip", 1f);
            mat.SetFloat("_Cutoff", Cutoff);
            mat.SetFloat("_Cull", 0f);
            return new TTRealTrees.Part { mesh = mesh, submesh = 0, material = mat, local = Matrix4x4.identity };
        }
    }
}

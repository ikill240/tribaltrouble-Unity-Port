using System.Collections.Generic;
using TribalTrouble.FaithfulGame;
using UnityEditor;
using UnityEngine;

namespace TribalTrouble.EditorTools
{
    /// <summary>
    /// Builds Resources/TTRealWeapons.asset: the axe and spear meshes that replace the thrown Viking axe and Native
    /// spear in the Modern look (from Assets/ThirdParty/MeleeWeapons, copied from your Stick City project's
    /// MeleeWeaponsPack). Each FBX is merged into one mesh stored inside the asset. Runs once automatically.
    /// </summary>
    [InitializeOnLoad]
    static class TTRealWeaponsBuilder
    {
        const string AssetPath = "Assets/TribalTrouble/Resources/TTRealWeapons.asset";
        const string Root = "Assets/ThirdParty/MeleeWeapons/FBX/";

        static TTRealWeaponsBuilder()
        {
            EditorApplication.delayCall += () =>
            {
                var existing = AssetDatabase.LoadAssetAtPath<TTRealWeaponSet>(AssetPath);
                if ((existing == null || existing.axeFit == null || existing.spearFit == null || existing.version < TTRealWeaponSet.CurrentVersion) && AssetDatabase.IsValidFolder("Assets/ThirdParty/MeleeWeapons")) Build();
            };
        }

        static Mesh Merge(TTRealWeaponSet set, string file)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(Root + file);
            if (go == null) return null;
            var parts = new List<CombineInstance>();
            foreach (var f in go.GetComponentsInChildren<MeshFilter>(true))
            {
                if (f.sharedMesh == null) continue;
                for (int s = 0; s < f.sharedMesh.subMeshCount; s++)
                    parts.Add(new CombineInstance { mesh = f.sharedMesh, subMeshIndex = s, transform = go.transform.worldToLocalMatrix * f.transform.localToWorldMatrix });
            }
            if (parts.Count == 0) return null;
            var m = new Mesh { name = System.IO.Path.GetFileNameWithoutExtension(file) };
            m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            m.CombineMeshes(parts.ToArray(), true, true);
            // clean vertex data: imported normals / tangents can hold NaNs (degenerate triangles), which shade as
            // magenta in the editor and as nothing in the game
            int bad = 0;
            var nrm = m.normals;
            for (int i = 0; i < nrm.Length; i++)
                if (float.IsNaN(nrm[i].x) || float.IsNaN(nrm[i].y) || float.IsNaN(nrm[i].z) || nrm[i].sqrMagnitude < 1e-8f) bad++;
            var uv = m.uv;
            int badUv = 0;
            foreach (var u in uv) if (float.IsNaN(u.x) || float.IsNaN(u.y) || float.IsInfinity(u.x) || float.IsInfinity(u.y)) badUv++;
            if (bad > 0 || nrm.Length == 0) m.RecalculateNormals();
            m.RecalculateTangents();
            m.RecalculateBounds();
            Debug.Log($"[Tribal Trouble] {m.name}: {m.vertexCount} vertices, {bad} bad normals, {badUv} bad uvs, uv count {uv.Length}");
            AssetDatabase.AddObjectToAsset(m, set);
            return m;
        }

        /// <summary>Puts each original weapon (left) next to its replacement for every tier into the open scene.</summary>
        [MenuItem("Tools/Tribal Trouble/Preview Real Weapons", priority = 32)]
        public static void Preview()
        {
            var catalog = TribalTrouble.Presentation.TTSpriteCatalog.Load();
            if (catalog == null) { Debug.LogWarning("No sprite catalog"); return; }
            foreach (var old in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include))
                if (old != null && old.parent == null && old.name == "Real weapon preview") Object.DestroyImmediate(old.gameObject);
            var root = new GameObject("Real weapon preview");
            int row = 0;
            foreach (var (g, n) in new[] { ("vikings", "axe"), ("natives", "spear") })
            {
                var def = catalog.Find(g, n);
                if (def == null || def.model == null) continue;
                for (int t = 0; t < 3; t++)
                {
                    var o = (GameObject)Object.Instantiate(def.model, root.transform);
                    o.name = $"{n} original {t}";
                    o.transform.localPosition = new Vector3(t * 1.5f, row * 2f, 0);
                    var part = TTRealWeapons.Get(def, t, catalog.baseMaterial);
                    if (part == null) { Debug.LogWarning($"No real {n} for tier {t}"); continue; }
                    var r = new GameObject($"{n} real {t}");
                    r.transform.SetParent(root.transform, false);
                    var at = Matrix4x4.Translate(new Vector3(t * 1.5f + 5f, row * 2f, 0)) * part.local;
                    r.transform.localPosition = at.GetColumn(3);
                    r.transform.localRotation = at.rotation;
                    r.transform.localScale = at.lossyScale;
                    r.AddComponent<MeshFilter>().sharedMesh = part.mesh;
                    r.AddComponent<MeshRenderer>().sharedMaterial = part.material;
                }
                row++;
            }
            Selection.activeGameObject = root;
            SceneView.lastActiveSceneView?.FrameSelected();
        }

        /// <summary>
        /// The weapons painted into the warrior meshes: triangles skinned to the weapon ("Prop") bone are cut out and
        /// the real axe / spear is fitted onto them in that bone's space.
        /// </summary>
        static List<TTRealWeaponSet.HeldWeapon> BuildHeld(TTRealWeaponSet set, TribalTrouble.Presentation.TTSpriteCatalog catalog)
        {
            var list = new List<TTRealWeaponSet.HeldWeapon>();
            if (catalog == null) return list;
            foreach (var (group, spear) in new[] { ("vikings", false), ("natives", true) })
            {
                var def = catalog.Find(group, "warrior");
                if (def == null || def.model == null) continue;
                foreach (var smr in def.model.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    var mesh = smr.sharedMesh;
                    if (mesh == null || smr.bones == null) continue;
                    // the weapon is skinned to the "Prop" bones (Vikings: Prop1; Natives: Prop1 + Prop3 at the spear's ends)
                    var props = new HashSet<int>();
                    for (int i = 0; i < smr.bones.Length; i++)
                        if (smr.bones[i] != null && smr.bones[i].name.Contains("Prop")) props.Add(i);
                    var names = string.Join(", ", System.Array.ConvertAll(smr.bones, b => b != null ? b.name : "-"));
                    if (props.Count == 0) { Debug.LogWarning($"[Tribal Trouble] {group}/warrior {smr.name}: no weapon (Prop) bone among: {names}"); continue; }
                    var weights = mesh.boneWeights;
                    var verts = mesh.vertices;
                    var isWeapon = new bool[verts.Length];
                    var perBone = new Dictionary<int, int>();
                    for (int i = 0; i < verts.Length && i < weights.Length; i++)
                    {
                        var w = weights[i];
                        int dom = w.weight0 >= w.weight1 && w.weight0 >= w.weight2 && w.weight0 >= w.weight3 ? w.boneIndex0
                                : w.weight1 >= w.weight2 && w.weight1 >= w.weight3 ? w.boneIndex1 : w.weight2 >= w.weight3 ? w.boneIndex2 : w.boneIndex3;
                        if (props.Contains(dom)) { isWeapon[i] = true; perBone[dom] = perBone.TryGetValue(dom, out var c) ? c + 1 : 1; }
                    }
                    int prop = -1, best = 0;
                    foreach (var kv in perBone) if (kv.Value > best) { best = kv.Value; prop = kv.Key; }
                    if (prop < 0) { Debug.LogWarning($"[Tribal Trouble] {group}/warrior {smr.name}: no vertices on the Prop bones"); continue; }
                    var pts = new List<Vector3>();
                    var bind = mesh.bindposes[prop];
                    for (int i = 0; i < verts.Length; i++) if (isWeapon[i]) pts.Add(bind.MultiplyPoint3x4(verts[i]));
                    if (pts.Count < 4) { Debug.LogWarning($"[Tribal Trouble] {group}/warrior {smr.name}: only {pts.Count} vertices on {smr.bones[prop].name}"); continue; }
                    var stripped = Object.Instantiate(mesh);
                    stripped.name = mesh.name + " (no weapon)";
                    for (int sub = 0; sub < stripped.subMeshCount; sub++)
                    {
                        var tris = stripped.GetTriangles(sub);
                        var keep = new List<int>(tris.Length);
                        for (int t = 0; t < tris.Length; t += 3)
                            if (!(isWeapon[tris[t]] && isWeapon[tris[t + 1]] && isWeapon[tris[t + 2]])) { keep.Add(tris[t]); keep.Add(tris[t + 1]); keep.Add(tris[t + 2]); }
                        stripped.SetTriangles(keep, sub);
                    }
                    AssetDatabase.AddObjectToAsset(stripped, set);
                    var h = new TTRealWeaponSet.HeldWeapon { sprite = group + "/warrior", original = mesh, stripped = stripped, bone = smr.bones[prop].name, spear = spear };
                    var fr = TTRealWeapons.Frame.Of(pts);
                    h.frameCenter = fr.center; h.frameAxis = fr.a1; h.frameSide = fr.a2; h.frameLength = fr.length; h.frameShaft = fr.shaft;
                    // a second weapon bone (the far end of a spear): remember where the weapon sits on it
                    int prop2 = -1, best2 = 2;
                    foreach (var kv in perBone) if (kv.Key != prop && kv.Value > best2) { best2 = kv.Value; prop2 = kv.Key; }
                    if (prop2 >= 0)
                    {
                        var c = Vector3.zero; int cn = 0;
                        for (int i = 0; i < verts.Length && i < weights.Length; i++)
                        {
                            var w = weights[i];
                            int dom = w.weight0 >= w.weight1 && w.weight0 >= w.weight2 && w.weight0 >= w.weight3 ? w.boneIndex0
                                    : w.weight1 >= w.weight2 && w.weight1 >= w.weight3 ? w.boneIndex1 : w.weight2 >= w.weight3 ? w.boneIndex2 : w.boneIndex3;
                            if (dom == prop2) { c += verts[i]; cn++; }
                        }
                        c /= Mathf.Max(1, cn);
                        h.bone2 = smr.bones[prop2].name;
                        h.anchorInBone = bind.MultiplyPoint3x4(c);
                        h.anchorInBone2 = mesh.bindposes[prop2].MultiplyPoint3x4(c);
                    }
                    if (TTRealWeapons.FitPoints(pts, spear ? set.spears : set.axes, out var wm, out var wl)) { h.weapon = wm; h.local = wl; }
                    list.Add(h);
                    Debug.Log($"[Tribal Trouble] Held weapon: {group}/warrior {smr.name}: {pts.Count} weapon vertices on '{h.bone}', real model {(wm != null ? wm.name : "none")}");
                }
            }
            return list;
        }

        [MenuItem("Tools/Tribal Trouble/Build Real Weapons", priority = 31)]
        public static void Build()
        {
            if (System.IO.File.Exists(AssetPath)) AssetDatabase.DeleteAsset(AssetPath);
            var set = ScriptableObject.CreateInstance<TTRealWeaponSet>();
            AssetDatabase.CreateAsset(set, AssetPath);
            var axes = new List<Mesh>();
            var spears = new List<Mesh>();
            for (int i = 1; i <= 4; i++)
            {
                var a = Merge(set, $"Axe_{i}.fbx"); if (a != null) axes.Add(a);
                var s = Merge(set, $"Spear_{i}.fbx"); if (s != null) spears.Add(s);
            }
            set.axes = axes.ToArray();
            set.spears = spears.ToArray();
            var catalog = TribalTrouble.Presentation.TTSpriteCatalog.Load();
            if (catalog != null)
            {
                if (TTRealWeapons.Fit(catalog.Find("vikings", "axe"), set.axes, out var am, out var al)) { set.axeFit = am; set.axeLocal = al; }
                if (TTRealWeapons.Fit(catalog.Find("natives", "spear"), set.spears, out var sm, out var sl)) { set.spearFit = sm; set.spearLocal = sl; }
            }
            set.held = BuildHeld(set, catalog).ToArray();
            set.version = TTRealWeaponSet.CurrentVersion;
            if (set.axeFit == null || set.spearFit == null) Debug.LogWarning("[Tribal Trouble] Real weapons: could not fit the axe / spear (sprite catalog missing?)");
            EditorUtility.SetDirty(set);
            AssetDatabase.SaveAssets();
            Debug.Log($"[Tribal Trouble] Real weapons: {set.axes.Length} axes, {set.spears.Length} spears.");
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace TribalTroubleSteam.Presentation
{
    /// <summary>
    /// Renders one TT sprite instance: chooses a LOD and texture variant, applies the team
    /// colour, and poses the skeleton from "animation ticks" exactly like the Java client:
    ///     cycle position = anim_ticks / wpc   (LOOP wraps, PLAIN clamps)
    /// Clips are authored as 1 second == 1 cycle by the converter, so clip time == cycle position.
    /// The simulation will own anim_ticks; this component only presents them.
    /// </summary>
    public sealed class TTSpriteView : MonoBehaviour
    {
        static readonly int BaseMapId = Shader.PropertyToID("_BaseMap");
        static readonly int TeamMaskId = Shader.PropertyToID("_TeamMask");
        static readonly int UseTeamMaskId = Shader.PropertyToID("_UseTeamMask");
        static readonly int TeamColorId = Shader.PropertyToID("_TeamColor");
        static readonly Dictionary<(TTSpriteDefinition, int, int), Material> SharedMaterials =
            new Dictionary<(TTSpriteDefinition, int, int), Material>();

        public TTSpriteDefinition Definition { get; private set; }
        public int Lod { get; private set; }
        public int Variant { get; private set; }
        public Color TeamColor { get; private set; } = Color.white;
        public int AnimationIndex { get; private set; } = -1;
        public float AnimationTicks { get; private set; }

        GameObject _instance;
        GameObject _animationRoot;
        TTPoseCache _poses;
        Transform[] _bones;
        Renderer[] _lodRenderers = new Renderer[0];
        MaterialPropertyBlock _block;
        Material _baseMaterial;

        public void Init(TTSpriteDefinition definition, Material baseMaterial, Color teamColor, int lod = 0, int variant = 0)
        {
            Clear();
            Definition = definition;
            _baseMaterial = baseMaterial;
            if (definition == null || definition.model == null)
                return;

            _instance = Instantiate(definition.model, transform, false);
            _instance.name = definition.Key;
            _animationRoot = FindAnimationRoot(_instance);
            _poses = definition.animations.Length > 0 ? TTPoseCache.For(definition) : null;
            _bones = _poses != null ? _poses.Resolve(_animationRoot) : null;

            _lodRenderers = new Renderer[definition.lods.Length];
            for (int i = 0; i < definition.lods.Length; i++)
            {
                var node = FindChildRecursive(_instance.transform, definition.lods[i].nodeName);
                _lodRenderers[i] = node != null ? node.GetComponent<Renderer>() : null;
                if (_lodRenderers[i] is SkinnedMeshRenderer smr && smr.sharedMesh != null)
                {
                    // die / attack poses leave the bind-pose bounds: a fixed, generous box instead of recomputing the
                    // skinned bounds of every unit every frame (updateWhenOffscreen)
                    var b = smr.sharedMesh.bounds;
                    float r = Mathf.Max(b.size.x, Mathf.Max(b.size.y, b.size.z)) * 1.6f + 0.5f;
                    smr.updateWhenOffscreen = false;
                    smr.localBounds = new Bounds(b.center, new Vector3(r, r, r) * 2f);
                }
            }

            SetLod(lod, variant);
            SetTeamColor(teamColor);
            if (definition.animations.Length > 0)
                SetAnimation(0, 0f);
        }

        public void SetLod(int lod, int variant)
        {
            if (Definition == null || Definition.lods.Length == 0)
                return;
            Lod = Mathf.Clamp(lod, 0, Definition.lods.Length - 1);
            var info = Definition.lods[Lod];
            Variant = info.textures.Length == 0 ? 0 : Mathf.Clamp(variant, 0, info.textures.Length - 1);
            for (int i = 0; i < _lodRenderers.Length; i++)
            {
                if (_lodRenderers[i] == null)
                    continue;
                _lodRenderers[i].enabled = i == Lod;
                if (i == Lod)
                    _lodRenderers[i].sharedMaterial = GetMaterial(Definition, Lod, Variant, _baseMaterial);
            }
            ApplyTeamColor();
        }

        public void SetTeamColor(Color color)
        {
            TeamColor = color;
            ApplyTeamColor();
        }

        /// <summary>Selects an animation and poses it at the given TT animation ticks.</summary>
        public void SetAnimation(int animationIndex, float animTicks)
        {
            AnimationIndex = animationIndex;
            AnimationTicks = animTicks;
            Pose();
        }

        public void SetAnimationTicks(float animTicks)
        {
            AnimationTicks = animTicks;
            Pose();
        }

        /// <summary>Clip time for the current animation (TT Sprite.getAnimationState mapping).</summary>
        public float CycleTime
        {
            get
            {
                if (Definition == null || AnimationIndex < 0 || AnimationIndex >= Definition.animations.Length)
                    return 0f;
                var a = Definition.animations[AnimationIndex];
                float position = AnimationTicks / Mathf.Max(a.wpc, 1e-6f);
                if (a.loop)
                    return position - Mathf.Floor(position);
                // PLAIN: clamp to the last frame, which sits at (frames - 1) / frames.
                float last = a.frames > 0 ? (a.frames - 1f) / a.frames : 1f;
                return Mathf.Clamp(position, 0f, last);
            }
        }

        bool _posePending;
        int _posedIndex = -1;
        float _posedTime = float.NaN;

        void Pose()
        {
            if (Definition == null || _animationRoot == null || AnimationIndex < 0 ||
                AnimationIndex >= Definition.animations.Length)
                return;
            // sampling a clip is the costly part of a unit: skip it while the unit can't be seen (not even its
            // shadow - isVisible counts shadow casters too) and catch up once it shows (PoseIfPending)
            var r = Lod < _lodRenderers.Length ? _lodRenderers[Lod] : null;
            if (r != null && !r.isVisible && _posedIndex >= 0)
            {
                _posePending = true;
                return;
            }
            _posePending = false;
            float time = CycleTime;
            if (AnimationIndex == _posedIndex && time == _posedTime)
                return; // same frame of the same clip (a PLAIN clip held on its last frame)
            // the poses of the clip's frames are worked out once per model (TTPoseCache) and blended here, the way
            // the Java renderer tweens between two frames; sampling the clip is the fallback
            if (_bones == null || !_poses.Apply(AnimationIndex, time, _bones))
            {
                var clip = Definition.animations[AnimationIndex].clip;
                if (clip != null)
                    clip.SampleAnimation(_animationRoot, time);
            }
            _posedIndex = AnimationIndex;
            _posedTime = time;
        }

        /// <summary>Applies a pose skipped while off screen, once the unit can be seen again.</summary>
        public void PoseIfPending()
        {
            if (_posePending && Lod < _lodRenderers.Length && _lodRenderers[Lod] != null && _lodRenderers[Lod].isVisible)
                Pose();
        }

        void ApplyTeamColor()
        {
            if (Definition == null)
                return;
            _block ??= new MaterialPropertyBlock();
            foreach (var r in _lodRenderers)
            {
                if (r == null)
                    continue;
                r.GetPropertyBlock(_block);
                _block.SetColor(TeamColorId, TeamColor);
                r.SetPropertyBlock(_block);
            }
        }

        internal static Material GetMaterial(TTSpriteDefinition def, int lod, int variant, Material baseMaterial)
        {
            var key = (def, lod, variant);
            if (SharedMaterials.TryGetValue(key, out var mat) && mat != null)
                return mat;
            var shader = baseMaterial != null ? baseMaterial.shader : Shader.Find("TribalTrouble/Sprite");
            mat = baseMaterial != null ? new Material(baseMaterial) : new Material(shader);
            var info = def.lods[lod];
            mat.name = $"{def.Key} lod{lod} v{variant}";
            if (info.textures.Length > 0)
            {
                var tv = info.textures[variant];
                var bmTex = tv.baseMap != null ? tv.baseMap : TribalTroubleSteam.TTGeneratedTextures.Get(tv.name);
                if (bmTex != null && bmTex.anisoLevel < 8) bmTex.anisoLevel = 8;   // crisp at the RTS camera's grazing angles
                mat.SetTexture(BaseMapId, bmTex);
                mat.SetTexture(TeamMaskId, tv.teamMask);
                mat.SetFloat(UseTeamMaskId, tv.teamMask != null ? 1f : 0f);
            }
            if (info.alphaClip)
            {
                mat.EnableKeyword("_ALPHATEST_ON");
                mat.SetFloat("_AlphaClip", 1f);
                mat.SetFloat("_Cull", 0f); // two-sided foliage
            }
            SteamSpriteLook.Apply(mat, def, info.alphaClip);   // the Steam release's lighting, faces and alpha test
            SharedMaterials[key] = mat;
            return mat;
        }

        internal static GameObject FindAnimationRoot(GameObject root)
        {
            // glTFast puts an Animation (legacy) or Animator (Mecanim) on the object the clip
            // paths are relative to. We drive poses ourselves, so disable it to stop autoplay.
            var legacy = root.GetComponentInChildren<Animation>(true);
            if (legacy != null)
            {
                legacy.playAutomatically = false;
                legacy.Stop();
                legacy.enabled = false;
                return legacy.gameObject;
            }
            var animator = root.GetComponentInChildren<Animator>(true);
            if (animator != null)
            {
                animator.enabled = false;
                return animator.gameObject;
            }
            return root;
        }

        internal static Transform FindChildRecursive(Transform parent, string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return null;
            foreach (Transform c in parent)
            {
                if (c.name == childName)
                    return c;
                var found = FindChildRecursive(c, childName);
                if (found != null)
                    return found;
            }
            return null;
        }

        void Clear()
        {
            if (_instance != null)
                Destroy(_instance);
            _instance = null;
            _animationRoot = null;
            _poses = null;
            _bones = null;
            _lodRenderers = new Renderer[0];
            AnimationIndex = -1;
            _posedIndex = -1;
            _posedTime = float.NaN;
            _posePending = false;
        }
    }
}

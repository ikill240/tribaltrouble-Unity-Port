using System.Collections.Generic;
using com.oddlabs.tt.model;
using TribalTrouble.Faithful;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Modern combat and damage effects, driven only by what the simulation shows (nothing here touches the sim):
    /// <list type="bullet">
    /// <item>thrown weapons that land: a dust puff (rock), sparks (iron) or a bright splat (rubber), plus a thud;</item>
    /// <item>units that fall: a burst of dust and a little blood;</item>
    /// <item>buildings below half health: flames and embers that grow as they burn down, with a crackle;</item>
    /// <item>buildings that collapse: a big dust cloud.</item>
    /// </list>
    /// Effects use one pooled ParticleSystem per kind (TribalTrouble/Particle shader, generated textures).
    /// Settings → Graphics → "Combat effects" turns them off.
    /// </summary>
    public sealed class TTCombatFx
    {
        public static TribalTrouble.Game.SfxPlayer Sfx;

        readonly ParticleSystem _dust, _blood, _sparks, _fire, _embers, _debris;
        Dictionary<object, Vector3> _weapons = new Dictionary<object, Vector3>(RefEq.Instance), _weaponsNext = new Dictionary<object, Vector3>(RefEq.Instance);
        readonly Dictionary<object, int> _weaponTier = new Dictionary<object, int>(RefEq.Instance);
        readonly HashSet<object> _deadUnits = new HashSet<object>(RefEq.Instance);
        Dictionary<object, (Vector3 pos, float size)> _buildings = new Dictionary<object, (Vector3, float)>(RefEq.Instance), _buildingsNext = new Dictionary<object, (Vector3, float)>(RefEq.Instance);
        readonly Dictionary<object, float> _crackleAt = new Dictionary<object, float>(RefEq.Instance);
        readonly System.Random _rng = new System.Random(7);
        readonly Transform _root;

        public TTCombatFx(Transform parent)
        {
            _root = new GameObject("Combat effects").transform;
            _root.SetParent(parent, false);
            var soft = SoftTexture(64, 2.2f);
            var flame = FlameTexture(64);
            _dust = Make("Dust", TTFxTextures.Get("dust", soft), false, 2000);   // (ComfyUI sprites in the modern look)
            _blood = Make("Blood", SoftTexture(32, 3f), false, 600);
            _sparks = Make("Sparks", SoftTexture(16, 4f), true, 800);
            _fire = Make("Fire", TTFxTextures.Get("fire", flame), true, 3000);
            _embers = Make("Embers", TTFxTextures.Get("spark", SoftTexture(16, 4f)), true, 1600);
            _debris = Make("Debris", SoftTexture(16, 0.35f), false, 1500);   // chunks of a collapsing building
        }

        ParticleSystem Make(string name, Texture2D tex, bool additive, int max)
        {
            var go = new GameObject(name);
            go.transform.SetParent(_root, false);
            var ps = go.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = ps.main;
            main.loop = false; main.playOnAwake = false; main.maxParticles = max;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = name == "Blood" ? 1.6f : name == "Debris" ? 1.9f : name == "Sparks" ? 0.9f : name == "Embers" ? -0.08f : name == "Fire" ? -0.25f : 0f;
            var emission = ps.emission; emission.enabled = false;
            var shape = ps.shape; shape.enabled = false;
            // fade out and grow over the particle's life
            var col = ps.colorOverLifetime; col.enabled = true;
            var g = new Gradient();
            g.SetKeys(new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                      new[] { new GradientAlphaKey(name == "Fire" ? 0f : 1f, 0f), new GradientAlphaKey(1f, name == "Fire" ? 0.15f : 0.1f), new GradientAlphaKey(0f, 1f) });
            col.color = g;
            var size = ps.sizeOverLifetime; size.enabled = true;
            size.size = new ParticleSystem.MinMaxCurve(1f, name == "Dust" ? AnimationCurve.EaseInOut(0, 0.5f, 1, 1.6f) : name == "Fire" ? AnimationCurve.EaseInOut(0, 1f, 1, 0.35f) : AnimationCurve.Linear(0, 1f, 1, 0.6f));
            if (name == "Dust" || name == "Fire")
            {
                var vel = ps.limitVelocityOverLifetime; vel.enabled = true; vel.dampen = 0.08f; vel.limit = 6f;
            }
            var r = go.GetComponent<ParticleSystemRenderer>();
            var mat = new Material(Shader.Find("TribalTrouble/Particle") ?? Shader.Find("Sprites/Default")) { name = "FX " + name };
            mat.mainTexture = tex;
            mat.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetFloat("_DstBlend", (float)(additive ? UnityEngine.Rendering.BlendMode.One : UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha));
            r.sharedMaterial = mat;
            r.renderMode = name == "Sparks" ? ParticleSystemRenderMode.Stretch : ParticleSystemRenderMode.Billboard;
            if (name == "Sparks") { r.velocityScale = 0.04f; r.lengthScale = 1f; }
            r.sortingFudge = additive ? -5 : 0;
            ps.Play();
            return ps;
        }

        static Texture2D SoftTexture(int n, float power)
        {
            var t = new Texture2D(n, n, TextureFormat.RGBA32, true) { wrapMode = TextureWrapMode.Clamp, name = "FX soft" };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float dx = (x + .5f) / n * 2 - 1, dy = (y + .5f) / n * 2 - 1;
                    float a = Mathf.Pow(Mathf.Clamp01(1 - Mathf.Sqrt(dx * dx + dy * dy)), power);
                    px[y * n + x] = new Color32(255, 255, 255, (byte)(a * 255));
                }
            t.SetPixels32(px); t.Apply(true, true);
            return t;
        }

        static Texture2D FlameTexture(int n)
        {
            var t = new Texture2D(n, n, TextureFormat.RGBA32, true) { wrapMode = TextureWrapMode.Clamp, name = "FX flame" };
            var px = new Color32[n * n];
            for (int y = 0; y < n; y++)
                for (int x = 0; x < n; x++)
                {
                    float u = (x + .5f) / n * 2 - 1, v = (y + .5f) / n;   // v: 0 bottom .. 1 top
                    float width = Mathf.Lerp(0.85f, 0.15f, v);
                    float a = Mathf.Clamp01(1 - Mathf.Abs(u) / width) * Mathf.Clamp01((1 - v) * 1.6f) * Mathf.Clamp01(v * 6f + 0.3f);
                    a *= 0.75f + 0.25f * Mathf.PerlinNoise(x * 0.25f, y * 0.18f);
                    // hot core: white-yellow; edges: orange-red
                    float core = Mathf.Clamp01(1 - Mathf.Abs(u) / (width * 0.5f)) * (1 - v);
                    var c = Color.Lerp(new Color(1f, 0.35f, 0.08f), new Color(1f, 0.9f, 0.55f), core);
                    px[y * n + x] = new Color(c.r, c.g, c.b, a);
                }
            t.SetPixels32(px); t.Apply(true, true);
            return t;
        }

        float R(float a, float b) => a + (float)_rng.NextDouble() * (b - a);
        Vector3 RUnit() { var v = new Vector3(R(-1, 1), R(-1, 1), R(-1, 1)); return v.sqrMagnitude > 1e-4f ? v.normalized : Vector3.up; }

        void Emit(ParticleSystem ps, Vector3 pos, Vector3 vel, float size, float life, Color color)
        {
            var e = new ParticleSystem.EmitParams { position = pos, velocity = vel, startSize = size, startLifetime = life, startColor = color, applyShapeToPosition = false };
            ps.Emit(e, 1);
        }

        public void Dust(Vector3 at, float strength)
        {
            int n = Mathf.RoundToInt(6 * strength);
            for (int i = 0; i < n; i++)
            {
                var d = RUnit(); d.y = Mathf.Abs(d.y) * 0.6f + 0.2f;
                Emit(_dust, at + new Vector3(R(-.3f, .3f), R(0, .2f), R(-.3f, .3f)) * strength, d * R(0.6f, 2.2f) * Mathf.Sqrt(strength),
                     R(0.8f, 1.6f) * strength, R(0.9f, 1.8f), new Color(0.62f, 0.55f, 0.44f, R(0.35f, 0.6f)));
            }
        }

        void Sparks(Vector3 at)
        {
            for (int i = 0; i < 14; i++)
            {
                var d = RUnit(); d.y = Mathf.Abs(d.y) + 0.3f;
                Emit(_sparks, at, d * R(3f, 8f), R(0.05f, 0.1f), R(0.2f, 0.5f), new Color(1f, R(0.6f, 0.85f), 0.3f, 1f));
            }
        }

        /// <summary>A building comes down: beams and stones thrown out, a shower of embers, and a second, lower dust cloud
        /// rolling out along the ground.</summary>
        void Collapse(Vector3 at, float size)
        {
            int n = Mathf.RoundToInt(14 * size);
            for (int i = 0; i < n; i++)
            {
                var d = RUnit(); d.y = Mathf.Abs(d.y) * 1.2f + 0.5f;
                bool wood = _rng.NextDouble() < 0.6;
                Emit(_debris, at + new Vector3(R(-.4f, .4f), R(0.5f, 1.8f), R(-.4f, .4f)) * size * 0.5f, d * R(2.5f, 6.5f),
                     R(0.12f, 0.32f) * Mathf.Sqrt(size), R(1.1f, 1.9f), wood ? new Color(R(0.3f, 0.42f), R(0.2f, 0.27f), R(0.1f, 0.15f), 1f) : new Color(0.45f, 0.43f, 0.4f, 1f));
            }
            for (int i = 0; i < 12; i++)
                Emit(_embers, at + Vector3.up * R(0.5f, 2f), new Vector3(R(-1.5f, 1.5f), R(1.5f, 4f), R(-1.5f, 1.5f)), R(0.08f, 0.16f), R(0.8f, 1.6f), new Color(1f, R(0.45f, 0.7f), 0.15f, 1f));
            for (int i = 0; i < 10; i++)
            {
                var d = RUnit(); d.y = 0f;
                Emit(_dust, at + d.normalized * size * 0.6f + Vector3.up * 0.3f, d.normalized * R(2f, 4.5f) + Vector3.up * 0.3f, R(1.2f, 2.2f) * size * 0.6f, R(1.4f, 2.4f), new Color(0.6f, 0.55f, 0.46f, R(0.3f, 0.5f)));
            }
        }

        /// <summary>A working building (New Tides): the forge smokes and throws a spark now and then, smoke curls out of
        /// the Mead Hall's / Spirit Lodge's smoke hole.</summary>
        void Lived(LandBuilding b, Vector3 p, float size, float dt)
        {
            int id = b.getTemplate() is BuildingTemplate bt ? bt.getTemplateID() : -1;
            if (id == Race.BUILDING_FORGE)
            {
                var at = p + Vector3.up * size * 1.3f;
                int n = Count(5f, dt);
                for (int i = 0; i < n; i++)
                    Emit(_dust, at + new Vector3(R(-.2f, .2f), 0f, R(-.2f, .2f)), new Vector3(R(0.1f, 0.5f), R(1.2f, 2f), R(-0.1f, 0.3f)), R(0.7f, 1.2f), R(2.5f, 4f), new Color(0.32f, 0.31f, 0.32f, R(0.3f, 0.45f)));
                int k = Count(2.5f, dt);
                for (int i = 0; i < k; i++)
                    Emit(_embers, at, new Vector3(R(-0.6f, 0.6f), R(1.5f, 3f), R(-0.6f, 0.6f)), R(0.05f, 0.1f), R(0.5f, 1f), new Color(1f, R(0.5f, 0.75f), 0.2f, 1f));
            }
            else if (id == Race.BUILDING_HALL)
            {
                int n = Count(2.2f, dt);
                for (int i = 0; i < n; i++)
                    Emit(_dust, p + Vector3.up * size * 1.45f, new Vector3(R(0.1f, 0.4f), R(0.8f, 1.4f), R(-0.1f, 0.2f)), R(0.6f, 1f), R(2.5f, 4f), new Color(0.55f, 0.55f, 0.57f, R(0.2f, 0.32f)));
            }
        }

        // ------------------------------------------------------------------ the gods' auras (New Tides)

        int Count(float rate, float dt) { float x = rate * dt; int n = (int)x; return n + (_rng.NextDouble() < x - n ? 1 : 0); }

        /// <summary>
        /// What hangs about a god: lightning crackling round the Thunder Lord, gold-white motes drifting off the
        /// Valkyrie's wings, green spirit wisps rising from the Jaguar Spirit, embers and flame off the Great Tiki's
        /// shoulder fires.
        /// </summary>
        void GodAura(Vector3 p, Unit u, int kind, float dt)
        {
            bool vik = TTExtendedArt.Vikings(u), major = kind == Race.UNIT_MAJOR_GOD;
            float h = 2.1f * TTExtendedArt.UnitScale(u, kind);   // its height (the chieftain's, grown)
            var fwd = new Vector3(u.getDirectionX(), 0f, u.getDirectionY());
            if (fwd.sqrMagnitude < 1e-4f) fwd = Vector3.forward;
            fwd.Normalize();
            var side = new Vector3(-fwd.z, 0f, fwd.x);
            if (vik && major)        // the Thunder Lord
            {
                int n = Count(22f, dt);
                for (int i = 0; i < n; i++)
                {
                    var at = p + Vector3.up * h * R(0.55f, 1.0f) + RUnit() * h * 0.3f;
                    Emit(_sparks, at, RUnit() * R(2f, 5f), R(0.06f, 0.12f), R(0.12f, 0.3f), new Color(R(0.55f, 0.8f), R(0.85f, 1f), 1f, 1f));
                }
                if (_rng.NextDouble() < dt * 0.9)   // now and then a burst off the hammer
                {
                    var at = p + Vector3.up * h * 0.75f - side * h * 0.3f + fwd * h * 0.2f;
                    for (int i = 0; i < 12; i++) Emit(_sparks, at, RUnit() * R(4f, 8f), R(0.08f, 0.14f), R(0.15f, 0.35f), new Color(0.75f, 0.95f, 1f, 1f));
                }
            }
            else if (vik)            // the Valkyrie
            {
                int n = Count(12f, dt);
                for (int i = 0; i < n; i++)
                {
                    float s = _rng.NextDouble() < 0.5 ? -1f : 1f;
                    var at = p + Vector3.up * h * R(0.45f, 0.95f) + side * s * h * R(0.2f, 0.75f) - fwd * h * 0.2f;
                    Emit(_embers, at, new Vector3(R(-0.3f, 0.3f), R(-0.4f, 0.2f), R(-0.3f, 0.3f)), R(0.07f, 0.15f), R(1.2f, 2.2f), new Color(1f, R(0.88f, 0.98f), R(0.65f, 0.85f), 0.9f));
                }
            }
            else if (!major)         // the Jaguar Spirit
            {
                int n = Count(12f, dt);
                for (int i = 0; i < n; i++)
                {
                    var off = RUnit(); off.y = 0f;
                    var at = p + Vector3.up * h * R(0.1f, 0.9f) + off * h * 0.3f;
                    Emit(_embers, at, new Vector3(R(-0.2f, 0.2f), R(0.6f, 1.3f), R(-0.2f, 0.2f)), R(0.1f, 0.2f), R(0.8f, 1.5f), new Color(R(0.3f, 0.5f), 1f, R(0.45f, 0.65f), 0.85f));
                }
            }
            else                     // the Great Tiki
            {
                int n = Count(26f, dt);
                for (int i = 0; i < n; i++)
                {
                    float s = _rng.NextDouble() < 0.5 ? -1f : 1f;
                    var at = p + Vector3.up * h * R(0.85f, 1.0f) + side * s * h * 0.3f;
                    Emit(_embers, at, new Vector3(R(-0.4f, 0.4f), R(1f, 2.6f), R(-0.4f, 0.4f)), R(0.07f, 0.16f), R(0.6f, 1.2f), new Color(1f, R(0.45f, 0.7f), 0.15f, 1f));
                }
                int f = Count(5f, dt);
                for (int i = 0; i < f; i++)
                {
                    float s = _rng.NextDouble() < 0.5 ? -1f : 1f;
                    Emit(_fire, p + Vector3.up * h * 0.9f + side * s * h * 0.3f, new Vector3(0f, R(0.4f, 0.9f), 0f), R(0.35f, 0.6f) * h * 0.25f, R(0.4f, 0.7f), Color.white);
                }
            }
        }

        /// <summary>
        /// The Risen Chieftain (Zombies mode): a sickly green mist curling about its feet, grave-light motes rising off
        /// it, and two green sparks where the eyes are.
        /// </summary>
        void BossAura(Vector3 p, Unit u, float dt)
        {
            float h = 2.1f * TTExtendedArt.UnitScale(u, Race.UNIT_ZOMBIE_BOSS);
            var fwd = new Vector3(u.getDirectionX(), 0f, u.getDirectionY());
            if (fwd.sqrMagnitude < 1e-4f) fwd = Vector3.forward;
            fwd.Normalize();
            var side = new Vector3(-fwd.z, 0f, fwd.x);
            int m = Count(9f, dt);
            for (int i = 0; i < m; i++)   // mist round the feet
            {
                var off = RUnit(); off.y = 0f;
                Emit(_dust, p + Vector3.up * 0.15f + off * h * 0.35f, new Vector3(off.x * 0.4f, R(0.05f, 0.25f), off.z * 0.4f),
                     R(1.1f, 1.9f), R(1.4f, 2.4f), new Color(R(0.3f, 0.4f), R(0.62f, 0.78f), R(0.25f, 0.35f), 0.5f));
            }
            int n = Count(20f, dt);
            for (int i = 0; i < n; i++)   // grave-light motes
            {
                var off = RUnit(); off.y = 0f;
                Emit(_embers, p + Vector3.up * h * R(0.1f, 0.85f) + off * h * 0.3f, new Vector3(R(-0.15f, 0.15f), R(0.5f, 1.1f), R(-0.15f, 0.15f)),
                     R(0.07f, 0.15f), R(0.9f, 1.6f), new Color(R(0.55f, 0.75f), 1f, R(0.25f, 0.4f), 0.9f));
            }
            if (_rng.NextDouble() < dt * 6.0)   // the eyes
                for (int s = -1; s <= 1; s += 2)
                    Emit(_sparks, p + Vector3.up * h * 0.9f + fwd * h * 0.09f + side * s * h * 0.045f, Vector3.zero, R(0.08f, 0.11f), 0.2f, new Color(0.75f, 1f, 0.35f, 1f));
        }

        /// <summary>A glint now and then off a gold, crystal or diamond deposit, where the light catches it.</summary>
        void OreGlint(Vector3 p, int kind, float dt)
        {
            int n = Count(kind == 1 ? 1.1f : 0.8f, dt);
            for (int i = 0; i < n; i++)
            {
                var off = RUnit(); off.y = 0f;
                var at = p + Vector3.up * R(0.25f, kind == 1 ? 1.3f : 0.8f) + off * R(0.2f, 0.7f);
                var c = kind == 0 ? new Color(1f, R(0.82f, 0.92f), 0.45f, 1f) : kind == 1 ? new Color(0.6f, R(0.9f, 1f), 1f, 1f) : new Color(0.9f, 0.96f, 1f, 1f);
                Emit(_sparks, at, Vector3.up * R(0f, 0.15f), R(0.12f, 0.2f), R(0.25f, 0.45f), c);
            }
        }

        void Blood(Vector3 at)
        {
            for (int i = 0; i < 10; i++)
            {
                var d = RUnit(); d.y = Mathf.Abs(d.y) + 0.4f;
                Emit(_blood, at, d * R(1f, 3.2f), R(0.08f, 0.18f), R(0.35f, 0.7f), new Color(R(0.35f, 0.5f), 0.02f, 0.02f, 0.9f));
            }
        }

        // ------------------------------------------------------------------ blows that land (melee, the chieftain)

        readonly Dictionary<object, float> _strikePhase = new Dictionary<object, float>(RefEq.Instance);

        /// <summary>
        /// A unit's attack animation passing the moment its blow lands: melee warriors throw a few sparks off the one
        /// they hit (the chicken whirlers a ring of dust and sparks all round them); the chieftain's blow leaves a golden
        /// slash in front of him - or, when Chief's Courage has lengthened his reach, a golden streak to whoever he
        /// strikes, so it can be seen that his blows have become ranged.
        /// </summary>
        void Strikes(in Renderable it, Unit u, Vector3 camPos)
        {
            if (u.isDead() || it.anim != Unit.Animation.THROWING || !(u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour ab))
            {
                _strikePhase.Remove(it.key);
                return;
            }
            var f = u.getWeaponFactory();
            bool chief = u.getAbilities().hasAbilities(Abilities.MAGIC);
            var melee = f as com.oddlabs.tt.model.weapon.MeleeFactory;
            if (melee == null && !chief) return;
            float r = f.getReleaseRatio(), p = it.ticks - Mathf.Floor(it.ticks);
            bool had = _strikePhase.TryGetValue(it.key, out var last);
            _strikePhase[it.key] = p;
            if (!had || !(last < r && p >= r)) return;
            var at = TTSpace.ToUnity(u.getPositionX(), u.getPositionY(), u.getPositionZ());
            if ((at - camPos).sqrMagnitude > 130f * 130f) return;
            var tg = ab.getTarget();
            var to = TTSpace.ToUnity(tg.getPositionX(), tg.getPositionY(), tg.getPositionZ()) + Vector3.up * 0.9f;
            var chest = at + Vector3.up * 1.1f;
            if (melee != null && melee.getSpinRadius() > 0f)
            {
                float rad = melee.getSpinRadius();
                for (int i = 0; i < 26; i++)
                {
                    float a = i * Mathf.PI * 2f / 26f;
                    var d = new Vector3(Mathf.Cos(a), 0f, Mathf.Sin(a));
                    Emit(_dust, at + d * 0.6f + Vector3.up * 0.3f, d * (rad * 1.6f) + Vector3.up * 0.4f, R(0.5f, 0.9f), R(0.5f, 0.8f), new Color(0.75f, 0.68f, 0.52f, 0.5f));
                    Emit(_sparks, chest + d * 0.5f, (d * 7f + new Vector3(-d.z, 0f, d.x) * 5f), R(0.05f, 0.09f), R(0.2f, 0.35f), new Color(1f, 0.93f, 0.62f, 1f));
                }
                return;
            }
            if (chief)
            {
                var gold = new Color(1f, 0.82f, 0.32f, 1f);
                float dist = (to - chest).magnitude;
                if (dist > 2.6f)
                {
                    // ranged: a golden streak to the one struck
                    int n = Mathf.Clamp((int)(dist * 2.5f), 6, 60);
                    for (int i = 0; i <= n; i++)
                    {
                        var q = Vector3.Lerp(chest, to, i / (float)n);
                        Emit(_embers, q, RUnit() * 0.3f, R(0.18f, 0.3f), R(0.25f, 0.45f), gold);
                    }
                    for (int i = 0; i < 18; i++) Emit(_sparks, to, RUnit() * R(3f, 7f), R(0.06f, 0.11f), R(0.2f, 0.4f), gold);
                }
                else
                {
                    // up close: a golden slash across the front of him
                    var fwd = (to - chest); fwd.y = 0f;
                    if (fwd.sqrMagnitude < 1e-4f) fwd = TTSpace.ToUnity(u.getDirectionX(), u.getDirectionY(), 0f);
                    fwd.Normalize();
                    var side = new Vector3(-fwd.z, 0f, fwd.x);
                    for (int i = 0; i < 16; i++)
                    {
                        float s = i / 15f - 0.5f;
                        var q = chest + fwd * (0.9f - Mathf.Abs(s) * 0.5f) + side * (s * 1.6f) + Vector3.up * (s * 0.6f);
                        Emit(_embers, q, side * 4f * (s < 0 ? 1f : 0.6f) + fwd * 1.5f, R(0.16f, 0.26f), R(0.18f, 0.3f), gold);
                    }
                    for (int i = 0; i < 10; i++) Emit(_sparks, to, RUnit() * R(3f, 6f), R(0.05f, 0.09f), R(0.2f, 0.35f), gold);
                }
                return;
            }
            Sparks(to);   // a melee warrior's blow
        }

        /// <summary>Call once per rendered frame with the collected scene.</summary>
        public void Update(FaithfulScene scene, Camera cam)
        {
            if (!TribalTrouble.Game.GameSettings.combatEffects || cam == null) { _weapons.Clear(); _buildings.Clear(); TTExtendedArt.BlowPuffs.Clear(); return; }
            var camPos = cam.transform.position;
            // the blowgunners' shots: a breath of pale dust from the muzzle, a wisp at the mouthpiece
            foreach (var (muzzle, dir, master) in TTExtendedArt.BlowPuffs)
            {
                for (int i = 0; i < 7; i++)
                    Emit(_dust, muzzle + RUnit() * 0.04f, dir * R(1.2f, 3.2f) + RUnit() * 0.35f, R(0.12f, 0.26f), R(0.35f, 0.7f), new Color(0.86f, 0.84f, 0.78f, R(0.25f, 0.45f)));
                if (master)   // the Dart Master's crystal tip glints as it leaves
                    for (int i = 0; i < 4; i++) Emit(_sparks, muzzle, dir * R(4f, 7f) + RUnit() * 1.2f, R(0.04f, 0.07f), R(0.12f, 0.25f), new Color(0.7f, 0.95f, 1f, 1f));
            }
            TTExtendedArt.BlowPuffs.Clear();
            float dt = Time.deltaTime, now = Time.time;
            _weaponsNext.Clear();
            _buildingsNext.Clear();
            var items = scene.items;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it.kind != RKind.Weapon && it.kind != RKind.Unit && it.kind != RKind.Building && it.kind != RKind.Supply) continue; // plants, trees...
                if (it.kind == RKind.Supply)   // (only gold, crystal and diamond deposits near the camera: their glints)
                {
                    int ok = TTExtendedArt.RareKind(it.model);
                    if (ok != 0 && ok != 1 && ok != 4) continue;
                    var sm = TTSpace.ToUnity(it.xf);
                    var sp = new Vector3(sm.m03, sm.m13, sm.m23);
                    if ((sp - camPos).sqrMagnitude < 75f * 75f) OreGlint(sp, ok, dt);
                    continue;
                }
                var mm = TTSpace.ToUnity(it.xf);
                var p = new Vector3(mm.m03, mm.m13, mm.m23);
                switch (it.kind)
                {
                    case RKind.Weapon:
                        _weaponsNext[it.key] = p;
                        _weaponTier[it.key] = it.texture;
                        break;
                    case RKind.Unit:
                        if (it.model is Unit u && u.isDead() && _deadUnits.Add(it.key) && (p - camPos).sqrMagnitude < 150f * 150f)
                        {
                            Dust(p + Vector3.up * 0.2f, 0.8f);
                            if (TribalTrouble.Game.GameSettings.combatBlood) Blood(p + Vector3.up * 1.0f);
                        }
                        else if (it.model is Unit au && !au.isDead() && (au.isBurning() || au.isPoisoned()) && (p - camPos).sqrMagnitude < 150f * 150f)
                            Afflicted(p, au.isBurning(), au.isPoisoned(), dt);   // Extended rules: on fire / poisoned
                        if (it.model is Unit su && su.getOwner().getWorld().isExtended())
                        {
                            Strikes(it, su, camPos);   // blows that land
                            if (!su.isDead() && (p - camPos).sqrMagnitude < 140f * 140f)
                            {
                                int gk = TTExtendedArt.UnitKind(su);
                                if (gk == Race.UNIT_MINOR_GOD || gk == Race.UNIT_MAJOR_GOD) GodAura(p, su, gk, dt);   // the gods' auras
                                else if (gk == Race.UNIT_ZOMBIE_BOSS) BossAura(p, su, dt);   // the Risen Chieftain's grave mist
                            }
                        }
                        break;
                    case RKind.Building:
                        if (it.model is Building b && !b.isDead())
                        {
                            float size = b.getTemplate() is BuildingTemplate bt ? Mathf.Max(1.5f, bt.getPlacingSize() * 0.8f) : 3f;
                            _buildingsNext[it.key] = (p, size);
                            if (b is LandBuilding lb && lb.isComplete())
                            {
                                float hp = lb.getHitPoints() / (float)Mathf.Max(1, lb.getEffectiveMaxHitPoints());
                                if (hp < 0.5f && (p - camPos).sqrMagnitude < 220f * 220f) Burn(it.key, p, size, 1f - hp / 0.5f, dt, now);
                                else if (lb.isBurning() && (p - camPos).sqrMagnitude < 220f * 220f) Burn(it.key, p, size, 0.5f, dt, now);   // set alight (Extended)
                                else if ((p - camPos).sqrMagnitude < 160f * 160f && lb.getOwner().getWorld().isExtended()) Lived(lb, p, size, dt);   // smoke from the forge and the hall
                            }
                        }
                        break;
                }
            }
            // weapons that vanished since last frame have landed
            foreach (var kv in _weapons)
            {
                if (_weaponsNext.ContainsKey(kv.Key)) continue;
                var at = kv.Value;
                if ((at - camPos).sqrMagnitude > 140f * 140f) { _weaponTier.Remove(kv.Key); continue; }
                int tier = _weaponTier.TryGetValue(kv.Key, out var tt) ? tt : 0;
                if (tier == 1) Sparks(at); else Dust(at, tier == 2 ? 0.45f : 0.6f);
                _weaponTier.Remove(kv.Key);
            }
            // buildings that vanished collapsed (or were finished off)
            foreach (var kv in _buildings)
            {
                if (_buildingsNext.ContainsKey(kv.Key)) continue;
                if ((kv.Value.pos - camPos).sqrMagnitude < 200f * 200f) { Dust(kv.Value.pos + Vector3.up, kv.Value.size * 1.3f); Collapse(kv.Value.pos, kv.Value.size); }
                _crackleAt.Remove(kv.Key);
            }
            (_weapons, _weaponsNext) = (_weaponsNext, _weapons); // the old map is cleared at the start of the next frame
            (_buildings, _buildingsNext) = (_buildingsNext, _buildings);
            if (_deadUnits.Count > 4096) _deadUnits.Clear();
            if (_strikePhase.Count > 8192) _strikePhase.Clear();
            UpdateSpells(scene, camPos, dt, now);
        }

        // ================================================================== Extended spells and projectiles

        Mesh _ringMesh;
        Material _ringAdd;
        readonly MaterialPropertyBlock _ringBlock = new MaterialPropertyBlock();

        /// <summary>A flat ring (inner radius 0.82) with a soft radial falloff in its UVs, drawn additive.</summary>
        void EnsureRing()
        {
            if (_ringMesh != null) return;
            const int seg = 72;
            var v = new Vector3[seg * 3];
            var uv = new Vector2[seg * 3];
            var t = new int[seg * 12];
            for (int i = 0; i < seg; i++)
            {
                float a = i * Mathf.PI * 2f / seg;
                var d = new Vector3(Mathf.Cos(a), 0, Mathf.Sin(a));
                v[i * 3] = d * 0.72f; v[i * 3 + 1] = d * 0.9f; v[i * 3 + 2] = d;
                float u = i / (float)seg;
                uv[i * 3] = new Vector2(u, 0f); uv[i * 3 + 1] = new Vector2(u, 1f); uv[i * 3 + 2] = new Vector2(u, 0f);
                int n = (i + 1) % seg, k = i * 12;
                for (int r = 0; r < 2; r++)
                {
                    int a0 = i * 3 + r, a1 = i * 3 + r + 1, b0 = n * 3 + r, b1 = n * 3 + r + 1;
                    t[k + r * 6] = a0; t[k + r * 6 + 1] = b0; t[k + r * 6 + 2] = a1;
                    t[k + r * 6 + 3] = a1; t[k + r * 6 + 4] = b0; t[k + r * 6 + 5] = b1;
                }
            }
            _ringMesh = new Mesh { name = "Spell ring", vertices = v, uv = uv, triangles = t };
            _ringMesh.RecalculateBounds();
            // u runs round the ring, v across it: a bright band with runes (bumps) along it
            const int w = 256, h = 16;
            var tex = new Texture2D(w, h, TextureFormat.RGBA32, true) { name = "FX spell ring", wrapMode = TextureWrapMode.Repeat };
            var px = new Color32[w * h];
            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    float vv = y / (float)(h - 1), band = Mathf.Pow(Mathf.Sin(vv * Mathf.PI), 1.5f);
                    float rune = 0.55f + 0.45f * Mathf.Abs(Mathf.Sin(x / (float)w * Mathf.PI * 24f)) * (0.6f + 0.4f * Mathf.PerlinNoise(x * 0.11f, y * 0.4f));
                    px[y * w + x] = new Color32(255, 255, 255, (byte)(Mathf.Clamp01(band * rune) * 255));
                }
            tex.SetPixels32(px); tex.Apply(true, true);
            _ringAdd = new Material(Shader.Find("TribalTrouble/Particle") ?? Shader.Find("Sprites/Default")) { name = "FX spell ring", mainTexture = tex };
            _ringAdd.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _ringAdd.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.One);
        }

        void Ring(Vector3 at, float radius, float spin, Color c)
        {
            EnsureRing();
            _ringBlock.SetColor("_Color", c);
            var m = Matrix4x4.TRS(at, Quaternion.Euler(0f, spin, 0f), new Vector3(radius, 1f, radius));
            Graphics.RenderMesh(new RenderParams(_ringAdd) { matProps = _ringBlock, shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off }, _ringMesh, 0, m);
        }

        int Rate(float perSecond, float dt) { float n = perSecond * dt; return (int)n + (R(0, 1) < n % 1f ? 1 : 0); }

        /// <summary>The new chieftain spells, on top of their particles: Ember Storm (a burning ring, flames bursting
        /// from the ground, embers, a dark cloud) and Healing Rain (a glowing green ring, motes rising, sparkles), and the
        /// race units' projectiles (burning axes, poison darts, sling stones, spirit bolts).</summary>
        void UpdateSpells(FaithfulScene scene, Vector3 camPos, float dt, float now)
        {
            foreach (var e in scene.emitters)
            {
                int tag = e.getFxTag();
                if (tag == 0) continue;
                var p = e.getPosition();
                var c = TTSpace.ToUnity(p.x(), p.y(), p.z() - 9f);   // cast 9 m below the emitter
                if ((c - camPos).sqrMagnitude > 260f * 260f) continue;
                float r = Mathf.Max(2f, e.getFxRadius());
                float pulse = 0.5f + 0.5f * Mathf.Cos((now % 1f) * Mathf.PI * 2f);   // the spell pulses once a second
                if (tag == com.oddlabs.tt.particle.Emitter.FX_EMBER_STORM)
                {
                    Ring(c + Vector3.up * 0.25f, r, now * 25f, new Color(1.6f, 0.55f, 0.12f, 0.55f + 0.35f * pulse));
                    Ring(c + Vector3.up * 0.3f, r * 0.55f, -now * 40f, new Color(1.2f, 0.3f, 0.05f, 0.35f + 0.3f * pulse));
                    for (int i = Rate(14f * r / 6f, dt); i > 0; i--)
                    {
                        var o = Random2(r);
                        Emit(_fire, c + new Vector3(o.x, R(0.1f, 0.5f), o.y), new Vector3(R(-.2f, .2f), R(1.5f, 3.2f), R(-.2f, .2f)), R(0.6f, 1.3f), R(0.4f, 0.8f), Color.white);
                    }
                    for (int i = Rate(10f * r / 6f, dt); i > 0; i--)
                    {
                        var o = Random2(r);
                        Emit(_embers, c + new Vector3(o.x, R(0.2f, 1f), o.y), new Vector3(R(-.6f, .6f), R(2f, 5f), R(-.6f, .6f)), R(0.05f, 0.11f), R(1f, 2f), new Color(1f, R(0.4f, 0.7f), 0.12f, 1f));
                    }
                    for (int i = Rate(4f * r / 6f, dt); i > 0; i--)
                    {
                        var o = Random2(r * 0.9f);
                        Emit(_dust, c + new Vector3(o.x, R(8f, 10f), o.y), new Vector3(R(-.3f, .3f), R(-.1f, .2f), R(-.3f, .3f)), R(2.5f, 4f), R(1.5f, 2.5f), new Color(0.16f, 0.1f, 0.08f, R(0.35f, 0.55f)));
                    }
                }
                else if (tag == com.oddlabs.tt.particle.Emitter.FX_HEALING_RAIN)
                {
                    Ring(c + Vector3.up * 0.25f, r, -now * 18f, new Color(0.45f, 1.5f, 0.85f, 0.45f + 0.35f * pulse));
                    Ring(c + Vector3.up * 0.3f, r * 0.6f, now * 30f, new Color(0.6f, 1.2f, 1.4f, 0.25f + 0.3f * pulse));
                    for (int i = Rate(16f * r / 6f, dt); i > 0; i--)
                    {
                        var o = Random2(r);
                        Emit(_embers, c + new Vector3(o.x, R(0.1f, 0.6f), o.y), new Vector3(R(-.1f, .1f), R(0.6f, 1.6f), R(-.1f, .1f)), R(0.08f, 0.16f), R(1.2f, 2.2f), new Color(R(0.4f, 0.7f), 1f, R(0.6f, 0.9f), 1f));
                    }
                    for (int i = Rate(5f * r / 6f, dt); i > 0; i--)
                    {
                        var o = Random2(r * 0.8f);
                        Emit(_sparks, c + new Vector3(o.x, R(1f, 3f), o.y), new Vector3(0f, R(-3f, -1.5f), 0f), R(0.04f, 0.08f), R(0.3f, 0.6f), new Color(0.75f, 1f, 1f, 1f));
                    }
                }
            }
            // the race units' projectiles
            var items = scene.items;
            for (int i = 0; i < items.Count; i++)
            {
                var it = items[i];
                if (it.kind != RKind.Weapon || !(it.model is com.oddlabs.tt.model.weapon.ThrowingWeapon tw)) continue;
                var thrower = tw.getThrower();
                if (thrower == null || !thrower.getOwner().getWorld().isExtended()) continue;
                var mm = TTSpace.ToUnity(it.xf);
                var p = new Vector3(mm.m03, mm.m13, mm.m23);
                if ((p - camPos).sqrMagnitude > 140f * 140f) continue;
                int kind = TTExtendedArt.UnitKind(thrower);
                bool vik = TTExtendedArt.Vikings(thrower);
                bool fireAxes = vik && thrower.getOwner().getUpgradeLevel(ResearchContainer.RACE_1) > 0 && thrower.isWarrior();
                bool poisonTips = !vik && thrower.getOwner().getUpgradeLevel(ResearchContainer.RACE_1) > 0 && thrower.isWarrior();
                if (kind == Race.UNIT_UNIQUE_1 && vik || fireAxes)
                    for (int k = Rate(40f, dt); k > 0; k--)
                        Emit(_fire, p + RUnit() * 0.15f, RUnit() * 0.3f, R(0.25f, 0.45f), R(0.15f, 0.3f), Color.white);
                else if (kind == Race.UNIT_UNIQUE_1 || poisonTips)
                    for (int k = Rate(30f, dt); k > 0; k--)
                        Emit(_embers, p + RUnit() * 0.08f, RUnit() * 0.2f, R(0.05f, 0.09f), R(0.3f, 0.6f), new Color(0.4f, 1f, 0.3f, 1f));
                else if (kind == Race.UNIT_SHAMAN)
                    for (int k = Rate(45f, dt); k > 0; k--)
                        Emit(_embers, p + RUnit() * 0.12f, RUnit() * 0.25f, R(0.12f, 0.22f), R(0.25f, 0.45f), vik ? new Color(1f, 0.6f, 0.25f, 1f) : new Color(0.45f, 1f, 0.8f, 1f));
                else if (kind == Race.UNIT_SLINGER)
                    for (int k = Rate(12f, dt); k > 0; k--)
                        Emit(_dust, p, RUnit() * 0.2f, R(0.15f, 0.3f), R(0.3f, 0.5f), new Color(0.6f, 0.55f, 0.45f, 0.35f));
            }
        }

        Vector2 Random2(float r)
        {
            float a = R(0f, Mathf.PI * 2f), d = Mathf.Sqrt(R(0f, 1f)) * r;
            return new Vector2(Mathf.Cos(a) * d, Mathf.Sin(a) * d);
        }

        /// <summary>Extended rules: a unit on fire (flames licking up) or poisoned (green bubbles rising).</summary>
        void Afflicted(Vector3 at, bool burning, bool poisoned, float dt)
        {
            if (burning)
            {
                int n = (int)(18f * dt) + (R(0, 1) < (18f * dt) % 1f ? 1 : 0);
                for (int i = 0; i < n; i++)
                    Emit(_fire, at + new Vector3(R(-.3f, .3f), R(0.4f, 1.6f), R(-.3f, .3f)), new Vector3(R(-.2f, .2f), R(0.8f, 1.6f), R(-.2f, .2f)), R(0.35f, 0.6f), R(0.35f, 0.6f), Color.white);
            }
            if (poisoned)
            {
                int n = (int)(10f * dt) + (R(0, 1) < (10f * dt) % 1f ? 1 : 0);
                for (int i = 0; i < n; i++)
                    Emit(_embers, at + new Vector3(R(-.35f, .35f), R(0.5f, 1.7f), R(-.35f, .35f)), new Vector3(R(-.1f, .1f), R(0.4f, 0.9f), R(-.1f, .1f)), R(0.07f, 0.13f), R(0.8f, 1.4f), new Color(0.35f, 1f, 0.25f, 1f));
            }
        }

        void Burn(object key, Vector3 at, float size, float intensity, float dt, float now)
        {
            // flames: rate grows as the building burns down
            float rate = Mathf.Lerp(12f, 55f, intensity) * size / 3f;
            int n = (int)(rate * dt) + (R(0, 1) < (rate * dt) % 1f ? 1 : 0);
            for (int i = 0; i < n; i++)
            {
                var o = new Vector3(R(-1, 1), R(0.3f, 1.8f), R(-1, 1)) * size * 0.7f;
                o.y = R(0.5f, size * 1.4f);
                Emit(_fire, at + o, new Vector3(R(-.3f, .3f), R(1.2f, 2.6f), R(-.3f, .3f)), R(0.9f, 1.8f) * Mathf.Lerp(0.7f, 1.2f, intensity), R(0.5f, 1.0f), Color.white);
                if (R(0, 1) < 0.25f)
                    Emit(_embers, at + o, new Vector3(R(-.8f, .8f), R(1.5f, 4f), R(-.8f, .8f)), R(0.04f, 0.09f), R(1.2f, 2.4f), new Color(1f, R(0.45f, 0.7f), 0.15f, 1f));
            }
            if (!_crackleAt.TryGetValue(key, out float next) || now >= next)
            {
                _crackleAt[key] = now + R(3f, 6f);
                Sfx?.Play("crackling_cloud", at);
            }
        }
    }
}

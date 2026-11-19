// Engine-free snapshot of what the Java renderer would draw this frame: walks the element tree and the
// tree quadtree with the same visitors RenderState / TreeRenderer use and records, per model, the sprite
// (location + texture variant), the animation (index + ticks) and the exact TT-space transform each
// ModelVisitor.getTransform builds. The Unity view turns these into GameObjects / instanced draws.
using System;
using System.Collections.Generic;
using com.oddlabs.tt.model;
using com.oddlabs.tt.landscape;

namespace TribalTrouble.Faithful
{
    /// <summary>Affine TT-space transform (Z up), row-major 3x4: p' = A * p + t.</summary>
    public struct Xf
    {
        public float a00, a01, a02, a03, a10, a11, a12, a13, a20, a21, a22, a23;

        public static Xf TRz(float x, float y, float z, float angle)
        {
            float c = (float)Math.Cos(angle), s = (float)Math.Sin(angle);
            return new Xf { a00 = c, a01 = -s, a02 = 0, a03 = x, a10 = s, a11 = c, a12 = 0, a13 = y, a20 = 0, a21 = 0, a22 = 1, a23 = z };
        }

        public static Xf FromJoml(org.joml.Matrix4f m) => new Xf
        {
            a00 = m.m00(), a01 = m.m10(), a02 = m.m20(), a03 = m.m30(),
            a10 = m.m01(), a11 = m.m11(), a12 = m.m21(), a13 = m.m31(),
            a20 = m.m02(), a21 = m.m12(), a22 = m.m22(), a23 = m.m32(),
        };

        /// <summary>this * R where R rotates by angle about the local axis (JOML Matrix4f.rotate post-multiplies).</summary>
        public Xf MulRot(float angle, int axis)
        {
            float c = (float)Math.Cos(angle), s = (float)Math.Sin(angle);
            // columns of R
            float r00 = 1, r10 = 0, r20 = 0, r01 = 0, r11 = 1, r21 = 0, r02 = 0, r12 = 0, r22 = 1;
            if (axis == 0) { r11 = c; r21 = s; r12 = -s; r22 = c; }
            else if (axis == 1) { r00 = c; r20 = -s; r02 = s; r22 = c; }
            else { r00 = c; r10 = s; r01 = -s; r11 = c; }
            return new Xf
            {
                a00 = a00 * r00 + a01 * r10 + a02 * r20, a01 = a00 * r01 + a01 * r11 + a02 * r21, a02 = a00 * r02 + a01 * r12 + a02 * r22, a03 = a03,
                a10 = a10 * r00 + a11 * r10 + a12 * r20, a11 = a10 * r01 + a11 * r11 + a12 * r21, a12 = a10 * r02 + a11 * r12 + a12 * r22, a13 = a13,
                a20 = a20 * r00 + a21 * r10 + a22 * r20, a21 = a20 * r01 + a21 * r11 + a22 * r21, a22 = a20 * r02 + a21 * r12 + a22 * r22, a23 = a23,
            };
        }

        public Xf MulScale(float s) => new Xf
        {
            a00 = a00 * s, a01 = a01 * s, a02 = a02 * s, a03 = a03,
            a10 = a10 * s, a11 = a11 * s, a12 = a12 * s, a13 = a13,
            a20 = a20 * s, a21 = a21 * s, a22 = a22 * s, a23 = a23,
        };

        /// <summary>this * Translate(x, y, z).</summary>
        public Xf MulTranslate(float x, float y, float z)
        {
            var r = this;
            r.a03 += a00 * x + a01 * y + a02 * z;
            r.a13 += a10 * x + a11 * y + a12 * z;
            r.a23 += a20 * x + a21 * y + a22 * z;
            return r;
        }
    }

    public enum RKind { Unit, Building, Supply, Scenery, Plants, Rubber, Weapon, Tree, Carry }

    public struct Renderable
    {
        public RKind kind;
        public object key;            // identity of the sim object (stable while it lives)
        public Model model;           // null for trees
        public string location;       // "res:/geometry/<group>/<name>.binsprite" (trees: tree type name)
        public int texture;           // texture variant index
        public int anim;
        public float ticks;
        public Xf xf;
        public float r, g, b;         // team colour (white for scenery)
        public bool selectable;
    }

    public sealed class FaithfulScene : java.lang.Object, ElementVisitor, ElementNodeVisitor, TreeNodeVisitor
    {
        public readonly List<Renderable> items = new List<Renderable>(4096);
        public readonly List<Renderable> trees = new List<Renderable>(8192);
        public readonly List<Selectable> selectables = new List<Selectable>(1024);
        public readonly List<com.oddlabs.tt.particle.Emitter> emitters = new List<com.oddlabs.tt.particle.Emitter>();
        public readonly List<com.oddlabs.tt.particle.Lightning> lightnings = new List<com.oddlabs.tt.particle.Lightning>();
        public readonly List<com.oddlabs.tt.landscape.LandscapeTargetRespond> responds = new List<com.oddlabs.tt.landscape.LandscapeTargetRespond>();
        public readonly List<com.oddlabs.tt.particle.SonicBlastEffect> sonicBlasts = new List<com.oddlabs.tt.particle.SonicBlastEffect>();

        World _world;
        HeightMap _hm;
        public World World => _world;

        public static readonly string[] TreeNames = { "jungle_tree", "palm", "oak_tree", "pine_tree" };   // AbstractTreeGroup.TreeType order

        /// <summary>Collects everything. Trees only change when felled, so they can be refreshed less often.</summary>
        public void Collect(World world, bool includeTrees)
        {
            _world = world;
            _hm = world.getHeightMap();
            _offscreen = 0;
            items.Clear(); selectables.Clear(); emitters.Clear(); lightnings.Clear(); responds.Clear(); sonicBlasts.Clear();
            world.getElementRoot().visit(this);
            if (includeTrees)
            {
                trees.Clear();
                world.getTreeRoot().visit(this);
            }
        }

        float Height(float x, float y) => _hm.computeInterpolatedHeight(0, x, y);
        static float Yaw(Element e) => (float)Math.Atan2(e.getDirectionY(), e.getDirectionX());

        void Add(RKind kind, Model m, com.oddlabs.tt.render.SpriteKey key, Xf xf, org.joml.Vector4fc team, object id = null)
        {
            if (key == null || key.location == null) return;
            if (_offscreen > 0 && StaticProp(kind)) return;
            items.Add(new Renderable
            {
                kind = kind, key = id ?? m, model = m, location = key.location, texture = key.texture,
                anim = m.getAnimation(), ticks = m.getAnimationTicks(), xf = xf,
                r = team != null ? team.x() : 1f, g = team != null ? team.y() : 1f, b = team != null ? team.z() : 1f,
                selectable = m is Selectable,
            });
        }

        // ---------------- ElementNodeVisitor (whole tree; Unity does its own culling)
        // Groups outside the view still visit their units / buildings (selection, sounds and live objects need them),
        // but their static props (plants, rocks, scenery) are skipped: on big islands that is most of the work.
        int _offscreen;
        /// <summary>Skip groups outside the view altogether, units and buildings too (the view keeps their objects,
        /// switched off, until they come back). Off: only their static props are skipped.</summary>
        public bool cullDynamic = true;
        public void visitNode(ElementNode node)
        {
            bool off = _offscreen == 0 && !Visible(node);
            if (off && cullDynamic && cullTest != null) return;
            if (off) _offscreen++;
            node.visitChildren(this); node.visitElements(this);
            if (off) _offscreen--;
        }
        public void visitLeaf(ElementLeaf leaf)
        {
            bool off = _offscreen == 0 && !Visible(leaf);
            if (off && cullDynamic && cullTest != null) return;
            if (off) _offscreen++;
            leaf.visitElements(this);
            if (off) _offscreen--;
        }
        static bool StaticProp(RKind k) => k == RKind.Plants || k == RKind.Supply || k == RKind.Scenery;
        public void visit(Element element) { element.visit(this); }

        // ---------------- ElementVisitor (RenderState)
        /// <summary>The gods are drawn as their tribe's chieftain, grown (a chieftain-like god, not a warrior): the
        /// chieftain's model has the same animation slots (idle, walk, strike, die, magic).</summary>
        static com.oddlabs.tt.render.SpriteKey GodLook(Unit unit)
        {
            var race = unit.getOwner().getRace();
            var t = unit.getTemplate();
            if (t != race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_MINOR_GOD) && t != race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_MAJOR_GOD))
                return null;
            return race.getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN).getSpriteRenderer();
        }

        public void visitUnit(Unit unit)
        {
            Element reference = unit.getReference();
            float z = reference == null ? Height(unit.getPositionX(), unit.getPositionY()) + unit.getOffsetZ() : reference.getPositionZ() + unit.getOffsetZ();
            var xf = Xf.TRz(unit.getPositionX(), unit.getPositionY(), z, Yaw(unit));
            var team = unit.getOwner().getColor();
            Add(RKind.Unit, unit, GodLook(unit) ?? unit.getSpriteRenderer(), xf, team);
            selectables.Add(unit);
            // carried logs / rocks (RenderState.unit_visitor.markDetailPolygon)
            var sc = unit.getSupplyContainer();
            if (unit.getAbilities().hasAbilities(Abilities.BUILD) && sc != null && sc.getSupplyType() != null && sc.getNumSupplies() > 0)
            {
                var carry = sc.getSupplySpriteRenderer(sc.getSupplyType());
                if (carry != null && carry.location != null)
                    items.Add(new Renderable
                    {
                        kind = RKind.Carry, key = sc, model = unit, location = carry.location, texture = carry.texture,
                        anim = unit.getAnimation(), ticks = unit.getAnimationTicks(), xf = xf, r = team.x(), g = team.y(), b = team.z(),
                    });
            }
        }

        public void visitBuilding(Building building)
        {
            var xf = Xf.TRz(building.getPositionX(), building.getPositionY(), building.getPositionZ(), Yaw(building));
            Add(RKind.Building, building, building.getSpriteRenderer(), xf, building.getOwner().getTeamBuildingColor());
            selectables.Add(building);
        }

        public void visitEmitter(com.oddlabs.tt.particle.Emitter emitter)
        {
            emitters.Add(emitter);
            // sprite-based particles (EmitterRenderer: a SpriteRenderer per particle type, LOW_POLY at the particle position)
            var sprites = emitter.getSpriteRenderers();
            if (emitter.getTextures() != null || sprites == null) return;
            var lists = emitter.getParticles();
            for (int j = 0; j < lists.Length && j < sprites.Length; j++)
            {
                var key = sprites[j];
                if (key == null || key.location == null) continue;
                foreach (com.oddlabs.tt.particle.Particle p in (java.util.List)lists[j])
                    items.Add(new Renderable
                    {
                        kind = RKind.Scenery, key = p, model = p, location = key.location, texture = key.texture,
                        xf = Xf.TRz(p.getPosX(), p.getPosY(), p.getPosZ(), 0f), r = 1, g = 1, b = 1,
                    });
            }
        }
        public void visitLightning(com.oddlabs.tt.particle.Lightning lightning) => lightnings.Add(lightning);
        public void visitSonicBlastEffect(com.oddlabs.tt.particle.SonicBlastEffect effect) => sonicBlasts.Add(effect);
        public void visitRespond(com.oddlabs.tt.landscape.LandscapeTargetRespond respond) => responds.Add(respond);

        public void visitSupplyModel(SupplyModel model)
        {
            var xf = Xf.TRz(model.getPositionX(), model.getPositionY(), model.getPositionZ(), (float)(model.getRotation() * Math.PI / 180.0));
            Add(RKind.Supply, model, model.getSpriteRenderer(), xf, null);
        }

        /// <summary>Set by the view: draws a plant or a piece of scenery from what it worked out the first time (true),
        /// or leaves it to be collected (false).</summary>
        public System.Func<Model, bool> staticHandler;

        public void visitSceneryModel(SceneryModel model)
        {
            if (_offscreen > 0) return;
            if (staticHandler != null && staticHandler(model)) return;
            var xf = Xf.TRz(model.getPositionX(), model.getPositionY(), model.getPositionZ(), Yaw(model));
            Add(RKind.Scenery, model, model.getSpriteRenderer(), xf, null);
        }

        public void visitRubberSupply(RubberSupply model)
        {
            float z = Height(model.getPositionX(), model.getPositionY()) + model.getOffsetZ();
            var xf = Xf.TRz(model.getPositionX(), model.getPositionY(), z, Yaw(model));
            Add(RKind.Rubber, model, model.getSpriteRenderer(), xf, null);
        }

        public void visitDirectedThrowingWeapon(com.oddlabs.tt.model.weapon.DirectedThrowingWeapon model)
        {
            var xf = Xf.TRz(model.getPositionX(), model.getPositionY(), model.getPositionZ(), Yaw(model))
                .MulRot(-(float)(model.getAngle() * Math.PI / 180.0), 1);
            Add(RKind.Weapon, model, model.getSpriteRenderer(), xf, null);
        }

        public void visitRotatingThrowingWeapon(com.oddlabs.tt.model.weapon.RotatingThrowingWeapon model)
        {
            var xf = Xf.TRz(model.getPositionX(), model.getPositionY(), model.getPositionZ(), Yaw(model))
                .MulRot((float)(model.getAngle() * Math.PI / 180.0), 1);
            Add(RKind.Weapon, model, model.getSpriteRenderer(), xf, null);
        }

        public void visitPlants(Plants plants)
        {
            if (_offscreen > 0) return;
            if (staticHandler != null && staticHandler(plants)) return;
            var xf = Xf.TRz(plants.getPositionX(), plants.getPositionY(), plants.getPositionZ(), Yaw(plants));
            Add(RKind.Plants, plants, plants.getSpriteRenderer(), xf, null);
        }

        // ---------------- TreeNodeVisitor (TreeRenderer.prepareMatrix)
        // Tree culling (TreeRenderer does the same with its frustum): whole groups outside the view or beyond the
        // draw distance are skipped, so huge islands cost only what is on screen.
        /// <summary>Set by the view: (minX, minY, minZ, maxX, maxY, maxZ) in Unity space (y up) -> visible? null: no culling.</summary>
        public System.Func<float, float, float, float, float, float, bool> cullTest;
        public float cullMargin = 25f;

        // a usable range: ordered and finite (unset bounds are +-infinity or +-float.MaxValue)
        static bool Sane(float a, float b) => a <= b && a > -1e6f && b < 1e6f;

        bool Visible(com.oddlabs.tt.util.BoundingBox b)
        {
            if (cullTest == null || !Sane(b.bmin_x, b.bmax_x) || !Sane(b.bmin_y, b.bmax_y)) return true;
            // element nodes only grow their height range as models are inserted: an empty one has none yet
            float z0 = b.bmin_z, z1 = b.bmax_z;
            // and only from the models' own boxes: cover the whole terrain height so nothing on screen is dropped
            if (!Sane(z0, z1)) { z0 = -10f; z1 = 120f; }
            else { z0 = Math.Min(z0, -10f); z1 = Math.Max(z1, 120f); }
            // TT x, y horizontal and z up -> Unity x, z and y
            float m = cullMargin;
            return cullTest(b.bmin_x - m, z0 - m, b.bmin_y - m, b.bmax_x + m, z1 + m, b.bmax_y + m);
        }

        public void visitNode(TreeGroup group) { if (Visible(group)) group.visitChildren(this); }
        /// <summary>Set by the view: takes a whole visible leaf of standing trees from its own cache (true), or leaves
        /// its trees to be collected one by one (false).</summary>
        public System.Func<TreeLeaf, bool> leafHandler;
        public void visitLeaf(TreeLeaf leaf)
        {
            if (!Visible(leaf)) return;
            if (leafHandler != null && leafHandler(leaf)) return;
            leaf.visitTrees(this);
        }

        /// <summary>A standing tree as visitTree records it.</summary>
        public static Renderable StandingTree(TreeSupply tree) =>
            new Renderable { kind = RKind.Tree, key = tree, location = TreeNames[tree.getTreeType().ordinal()], xf = Xf.FromJoml(tree.getMatrix()).MulScale(tree.getScale()), r = 1, g = 1, b = 1 };

        public void visitTree(TreeSupply tree)
        {
            if (tree.isHidden()) return;
            var xf = Xf.FromJoml(tree.getMatrix());
            if (tree.isEmpty())
            {
                float time = tree.getTreeFallProgress();
                float t2 = time * time, t6 = t2 * t2 * t2;
                xf = xf.MulTranslate(0f, 0f, -13f * t6).MulRot((float)(90f * t2 * Math.PI / 180.0), 0);
            }
            else
            {
                xf = xf.MulScale(tree.getScale());
            }
            trees.Add(new Renderable { kind = RKind.Tree, key = tree, location = TreeNames[tree.getTreeType().ordinal()], xf = xf, r = 1, g = 1, b = 1 });
        }
    }
}

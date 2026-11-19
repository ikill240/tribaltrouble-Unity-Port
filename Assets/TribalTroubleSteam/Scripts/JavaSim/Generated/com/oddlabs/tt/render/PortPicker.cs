// Translated from PortPicker.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 41
    public class PortPicker : java.lang.Object
    {
        public const float TREE_SELECTION_RADIUS = 1.5f;
        public static java.util.ArrayList responds;
        // Java: 47
        public static void addRespond(com.oddlabs.tt.landscape.LandscapeTargetRespond respond)
        {
            lock ((com.oddlabs.tt.render.PortPicker.responds))
            {
                com.oddlabs.tt.render.PortPicker.responds.add(respond);
            }
        }
        // Java: 54
        public static com.oddlabs.tt.landscape.LandscapeTargetRespond[] getResponds()
        {
            lock ((com.oddlabs.tt.render.PortPicker.responds))
            {
                com.oddlabs.tt.render.PortPicker.responds.removeIf(new java.util.function.Predicate_L((object r_l) =>
                {
                    com.oddlabs.tt.landscape.LandscapeTargetRespond r = (com.oddlabs.tt.landscape.LandscapeTargetRespond)r_l;
                    return r.getProgress() <= 0.0f;
                }));
                return ((com.oddlabs.tt.landscape.LandscapeTargetRespond[])com.oddlabs.tt.render.PortPicker.responds.toArray(new com.oddlabs.tt.landscape.LandscapeTargetRespond[0]));
            }
        }
        public com.oddlabs.tt.util.BoundingBox box;
        public float[][] frustum;
        public com.oddlabs.tt.landscape.World world;
        public java.util.List targets;
        public java.util.List trees;
        public com.oddlabs.tt.model.ElementNodeVisitor node_visitor;
        public com.oddlabs.tt.model.ElementVisitor element_visitor;
        public com.oddlabs.tt.landscape.TreeNodeVisitor tree_visitor;
        // Java: 189
        public void selectable(com.oddlabs.tt.model.Selectable selectable_v, float z_offset, float radius, float height)
        {
            if (!selectable_v.isEnabled())
            {
                return;
            }
            box.setBounds((float)((float)(-radius) + selectable_v.getPositionX()), (float)(radius + selectable_v.getPositionX()), (float)((float)(-radius) + selectable_v.getPositionY()), (float)(radius + selectable_v.getPositionY()), z_offset, (float)(z_offset + height));
            if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(box, frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
            {
                targets.add(selectable_v);
            }
        }
        // Java: 198
        public void model(com.oddlabs.tt.model.Model model_v)
        {
            if (!((model_v is com.oddlabs.tt.util.Target target)))
            {
                return;
            }
            if (com.oddlabs.tt.render.PortPicker.hasBounds(model_v))
            {
                if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(model_v, frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
                {
                    targets.add(target);
                }
                return;
            }
            float r = ((model_v is com.oddlabs.tt.model.RubberSupply) ? 0.6f : ((model_v is com.oddlabs.tt.model.SupplyModel) ? 1.4f : 2.0f));
            float h = ((model_v is com.oddlabs.tt.model.RubberSupply) ? 0.9f : ((model_v is com.oddlabs.tt.model.SupplyModel) ? 2.2f : 3.0f));
            box.setBounds((float)(model_v.getPositionX() - r), (float)(model_v.getPositionX() + r), (float)(model_v.getPositionY() - r), (float)(model_v.getPositionY() + r), model_v.getPositionZ(), (float)(model_v.getPositionZ() + h));
            if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(box, frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
            {
                targets.add(target);
            }
        }
        // Java: 216
        public static bool hasBounds(com.oddlabs.tt.util.BoundingBox b)
        {
            return b.bmin_x <= b.bmax_x && b.bmin_y <= b.bmax_y && b.bmin_z <= b.bmax_z;
        }
        // Java: 224
        public static float cameraDistanceSquared(com.oddlabs.tt.util.Target target, float cx, float cy, float cz)
        {
            if ((target is com.oddlabs.tt.util.BoundingBox b) && com.oddlabs.tt.render.PortPicker.hasBounds(b))
            {
                return com.oddlabs.tt.render.RenderTools.getCameraDistanceSquared(b, cx, cy, cz);
            }
            float x = target.getPositionX();
            float y = target.getPositionY();
            float z = default;
            if ((target is com.oddlabs.tt.model.Unit unit))
            {
                z = (float)(unit.getPositionZ() + (float)(((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getSelectionHeight() * 0.5f));
            }
            else if ((target is com.oddlabs.tt.model.Building building))
            {
                z = (float)(building.getPositionZ() + (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getBuiltSelectionHeight() * 0.5f));
            }
            else if ((target is com.oddlabs.tt.model.Model model_v))
            {
                z = (float)(model_v.getPositionZ() + 1.0f);
            }
            else
            {
                z = cz;
            }
            float dx = (float)(cx - x);
            float dy = (float)(cy - y);
            float dz = (float)(cz - z);
            return (float)((float)((float)(dx * dx) + (float)(dy * dy)) + (float)(dz * dz));
        }
        // Java: 242
        public void pickObjects(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.camera.CameraState camera, java.util.List @out)
        {
            this.world = world;
            this.frustum = camera.getFrustum();
            this.targets = @out;
            world.getElementRoot().visit(node_visitor);
            this.targets = null;
        }
        // Java: 251
        public void pickTrees(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.camera.CameraState camera, java.util.List @out)
        {
            this.world = world;
            this.frustum = camera.getFrustum();
            this.trees = @out;
            com.oddlabs.tt.landscape.AbstractTreeGroup root = world.getTreeRoot();
            root.visit(tree_visitor);
            this.trees = null;
        }
        static PortPicker()
        {
            PortPicker.responds = new java.util.ArrayList();
        }
        private void init_PortPicker()
        {
            this.box = new com.oddlabs.tt.util.BoundingBox();
            this.node_visitor = new PortPicker.Anon1_ElementNodeVisitor(this);
            this.element_visitor = new PortPicker.Anon2_ElementVisitor(this);
            this.tree_visitor = new PortPicker.Anon3_TreeNodeVisitor(this);
        }
        public PortPicker() : base()
        {
            init_PortPicker();
        }
        // Java: anonymous class at line 68
        public sealed class Anon1_ElementNodeVisitor : java.lang.Object, com.oddlabs.tt.model.ElementNodeVisitor
        {
            public com.oddlabs.tt.render.PortPicker this_0;
            public Anon1_ElementNodeVisitor(com.oddlabs.tt.render.PortPicker this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 69
            public void visitNode(com.oddlabs.tt.model.ElementNode node)
            {
                if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(node, this_0.frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
                {
                    node.visitChildren(this);
                    node.visitElements(this);
                }
            }
            // Java: 77
            public void visitLeaf(com.oddlabs.tt.model.ElementLeaf leaf)
            {
                if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(leaf, this_0.frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
                {
                    leaf.visitElements(this);
                }
            }
            // Java: 83
            public void visit(com.oddlabs.tt.model.Element element)
            {
                element.visit(this_0.element_visitor);
            }
        }
        // Java: anonymous class at line 89
        public sealed class Anon2_ElementVisitor : java.lang.Object, com.oddlabs.tt.model.ElementVisitor
        {
            public com.oddlabs.tt.render.PortPicker this_0;
            public Anon2_ElementVisitor(com.oddlabs.tt.render.PortPicker this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 90
            public void visitUnit(com.oddlabs.tt.model.Unit unit)
            {
                com.oddlabs.tt.model.Element @ref = unit.getReference();
                float z_offset = (@ref == null ? (float)(this_0.world.getHeightMap().computeInterpolatedHeight(0, unit.getPositionX(), unit.getPositionY()) + unit.getOffsetZ()) : (float)(@ref.getPositionZ() + unit.getOffsetZ()));
                this_0.selectable(unit, z_offset, ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getSelectionRadius(), ((com.oddlabs.tt.model.UnitTemplate)unit.getTemplate()).getSelectionHeight());
            }
            // Java: 98
            public void visitBuilding(com.oddlabs.tt.model.Building building)
            {
                float radius = ((building.getRenderLevel()).ordinal() switch { 0 /*START*/ => (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getStartSelectionRadius()), 1 /*HALFBUILT*/ => (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getHalfbuiltSelectionRadius()), 2 /*BUILT*/ => (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getBuiltSelectionRadius()), _ => throw new System.InvalidOperationException("MatchException") });
                float height = ((building.getRenderLevel()).ordinal() switch { 0 /*START*/ => (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getStartSelectionHeight()), 1 /*HALFBUILT*/ => (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getHalfbuiltSelectionHeight()), 2 /*BUILT*/ => (float)(((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getBuiltSelectionHeight()), _ => throw new System.InvalidOperationException("MatchException") });
                this_0.selectable(building, building.getPositionZ(), radius, height);
            }
            // Java: 113
            public void visitSupplyModel(com.oddlabs.tt.model.SupplyModel model_v)
            {
                this_0.model(model_v);
            }
            // Java: 118
            public void visitRubberSupply(com.oddlabs.tt.model.RubberSupply model_v)
            {
                this_0.model(model_v);
            }
            // Java: 123
            public void visitSceneryModel(com.oddlabs.tt.model.SceneryModel model_v)
            {
                this_0.model(model_v);
            }
            // Java: 128
            public void visitEmitter(com.oddlabs.tt.particle.Emitter emitter)
            {
            }
            // Java: 132
            public void visitLightning(com.oddlabs.tt.particle.Lightning lightning)
            {
            }
            // Java: 136
            public void visitSonicBlastEffect(com.oddlabs.tt.particle.SonicBlastEffect effect)
            {
            }
            // Java: 140
            public void visitRespond(com.oddlabs.tt.landscape.LandscapeTargetRespond respond)
            {
            }
            // Java: 144
            public void visitDirectedThrowingWeapon(com.oddlabs.tt.model.weapon.DirectedThrowingWeapon model_v)
            {
            }
            // Java: 148
            public void visitRotatingThrowingWeapon(com.oddlabs.tt.model.weapon.RotatingThrowingWeapon model_v)
            {
            }
            // Java: 152
            public void visitPlants(com.oddlabs.tt.model.Plants plants)
            {
            }
        }
        // Java: anonymous class at line 157
        public sealed class Anon3_TreeNodeVisitor : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public com.oddlabs.tt.render.PortPicker this_0;
            public Anon3_TreeNodeVisitor(com.oddlabs.tt.render.PortPicker this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 158
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf tree_leaf)
            {
                if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(tree_leaf, this_0.frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
                {
                    tree_leaf.visitTrees(this);
                }
            }
            // Java: 164
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup tree_group)
            {
                if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(tree_group, this_0.frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
                {
                    tree_group.visitChildren(this);
                }
            }
            // Java: 170
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply tree_supply)
            {
                if (tree_supply.isHidden() || tree_supply.isDead())
                {
                    return;
                }
                float scale = ((tree_supply.getTreeType()).ordinal() switch { 0 /*JUNGLE*/ => (float)(0.9f), 1 /*PALM*/ => (float)(0.95f), 2 /*OAK*/ => (float)(0.7f), 3 /*PINE*/ => (float)(0.65f), _ => throw new System.InvalidOperationException("MatchException") });
                this_0.box.setBounds((float)((float)(-com.oddlabs.tt.render.PortPicker.TREE_SELECTION_RADIUS) + tree_supply.getPositionX()), (float)(com.oddlabs.tt.render.PortPicker.TREE_SELECTION_RADIUS + tree_supply.getPositionX()), (float)((float)(-com.oddlabs.tt.render.PortPicker.TREE_SELECTION_RADIUS) + tree_supply.getPositionY()), (float)(com.oddlabs.tt.render.PortPicker.TREE_SELECTION_RADIUS + tree_supply.getPositionY()), tree_supply.bmin_z, (float)(tree_supply.bmin_z + (float)(((float)(tree_supply.bmax_z - tree_supply.bmin_z)) * scale)));
                if (((object)com.oddlabs.tt.render.RenderTools.inFrustum(this_0.box, this_0.frustum) != (object)com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE))
                {
                    this_0.trees.add(tree_supply);
                }
            }
        }
    }
}

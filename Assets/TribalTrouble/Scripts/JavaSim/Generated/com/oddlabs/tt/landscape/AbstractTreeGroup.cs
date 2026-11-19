// Translated from AbstractTreeGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 13
    public abstract class AbstractTreeGroup : com.oddlabs.tt.util.BoundingBox
    {
        // Java: 15
        public sealed class TreeType : java.lang.Enum
        {
            public static TreeType[] values() { return (TreeType[])VALUES_.Clone(); }
            public static TreeType valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly TreeType JUNGLE = new TreeType("JUNGLE", 0);
            public static readonly TreeType PALM = new TreeType("PALM", 1);
            public static readonly TreeType OAK = new TreeType("OAK", 2);
            public static readonly TreeType PINE = new TreeType("PINE", 3);
            public static readonly TreeType[] VALUES_ = new TreeType[] { JUNGLE, PALM, OAK, PINE };
            public TreeType(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.landscape.AbstractTreeGroup parent;
        public int num_responding_trees;
        // Java: 30
        public com.oddlabs.tt.landscape.AbstractTreeGroup getParent()
        {
            return parent;
        }
        // Java: 34
        public void changeRespondingTrees(int delta)
        {
            num_responding_trees += delta;
            if (parent != null)
            {
                parent.changeRespondingTrees(delta);
            }
        }
        // Java: 40
        public bool hasRespondingTrees()
        {
            return num_responding_trees > 0;
        }
        // Java: 44
        public static com.oddlabs.tt.landscape.AbstractTreeGroup newRoot(com.oddlabs.tt.landscape.World world, java.util.List tree_positions, java.util.List palm_tree_positions, com.oddlabs.tt.procedural.Landscape.TerrainType terrain)
        {
            com.oddlabs.tt.landscape.AbstractTreeGroup root = new com.oddlabs.tt.landscape.TreeGroup(null, 0);
            switch ((terrain.@base()).ordinal())   // (an Elemental island grows the Natives' trees: it had none, so no wood)
            {
                case 0 /*NATIVE*/:
                {
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.JUNGLE, tree_positions);
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.PALM, palm_tree_positions);
                    break;
                }
                case 1 /*VIKING*/:
                {
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.OAK, tree_positions);
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.PINE, palm_tree_positions);
                    break;
                }
            }
            root.initBounds();
            return root;
        }
        // Java: 64
        public class TreeKind : java.lang.Object
        {
            public int grid_size() { return grid_size_f; }
            public float radius() { return radius_f; }
            public float scale_factor() { return scale_factor_f; }
            public float min_size() { return min_size_f; }
            public TreeKind(int grid_size, float radius, float scale_factor, float min_size)
            {
                this.grid_size_f = grid_size;
                this.radius_f = radius;
                this.scale_factor_f = scale_factor;
                this.min_size_f = min_size;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is TreeKind r)) return false; return true && java.lang.JO.equals(grid_size_f, r.grid_size_f) && java.lang.JO.equals(radius_f, r.radius_f) && java.lang.JO.equals(scale_factor_f, r.scale_factor_f) && java.lang.JO.equals(min_size_f, r.min_size_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(grid_size_f); h = 31 * h + java.lang.JO.hashCode(radius_f); h = 31 * h + java.lang.JO.hashCode(scale_factor_f); h = 31 * h + java.lang.JO.hashCode(min_size_f); } return h; }
            public override string toString() { return "TreeKind[" + "grid_size=" + java.lang.JO.toString(grid_size_f) + ", radius=" + java.lang.JO.toString(radius_f) + ", scale_factor=" + java.lang.JO.toString(scale_factor_f) + ", min_size=" + java.lang.JO.toString(min_size_f) + "]"; }
            public int grid_size_f;
            public float radius_f;
            public float scale_factor_f;
            public float min_size_f;
            // Java: 65
            public static com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind of(com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type)
            {
                return ((tree_type).ordinal() switch { 0 /*JUNGLE*/ => (com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind)(new com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind(3, 2.3f, 0.25f, 0.75f)), 2 /*OAK*/ => (com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind)(new com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind(3, 2.3f, 0.5f, 1.0f)), 1 /*PALM*/ or 3 /*PINE*/ => (com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind)(new com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind(1, 1.6f, 0.5f, 1.0f)), _ => throw new System.InvalidOperationException("MatchException") });
            }
        }
        // Java: 74
        public void buildTrees(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type, java.util.List tree_positions)
        {
            foreach (int[] coords in java.lang.JIter.of(tree_positions))
            {
                plant(world, tree_type, coords[0], coords[1]);
            }
        }
        // Java: 84
        public com.oddlabs.tt.landscape.TreeSupply plantTree(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type, int grid_x, int grid_y)
        {
            return plant(world, tree_type, grid_x, grid_y);
        }
        // Java: 90
        public void removeTree(com.oddlabs.tt.landscape.TreeSupply tree)
        {
            findLeaf(tree.getWorld(), tree.getPositionX(), tree.getPositionY()).takeOut(tree);
            tree.leaveWorld();
        }
        // Java: 96
        public void refreshBounds()
        {
            initBounds();
        }
        // Java: 100
        public com.oddlabs.tt.landscape.TreeSupply plant(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type, int center_grid_x, int center_grid_y)
        {
            com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind kind = com.oddlabs.tt.landscape.AbstractTreeGroup.TreeKind.of(tree_type);
            float radius = kind.radius();
            org.joml.Matrix4f matrix2 = new org.joml.Matrix4f();
            org.joml.Vector3f vector = new org.joml.Vector3f();
            float h = 15.0f;
            float[] tree_low_vertices = new float[] { (float)(-radius), (float)(-radius), 0, radius, (float)(-radius), 0, radius, radius, 0, (float)(-radius), radius, 0, (float)(-radius), (float)(-radius), h, radius, (float)(-radius), h, radius, radius, h, (float)(-radius), radius, h };
            org.joml.Matrix4f matrix = new org.joml.Matrix4f();
            float tree_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid_x);
            float tree_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid_y);
            float rotation = (float)(world.getRandom().nextFloat() * 360.0f);
            float scale_base = (float)((float)(world.getRandom().nextFloat() * kind.scale_factor()) + kind.min_size());
            float scale_x = (float)((float)(scale_base + (float)(world.getRandom().nextFloat() * 0.2f)) - 0.1f);
            float scale_y = (float)((float)(scale_base + (float)(world.getRandom().nextFloat() * 0.2f)) - 0.1f);
            float scale_z = (float)((float)(scale_base + (float)(world.getRandom().nextFloat() * 0.2f)) - 0.1f);
            matrix.identity();
            matrix.scale(scale_x, scale_y, scale_z);
            vector.set(0.0f, 0.0f, 1.0f);
            matrix.rotate(((float)(java.lang.Math.toRadians(rotation))), vector);
            matrix2.identity();
            matrix2.translate(tree_x, tree_y, world.getHeightMap().getNearestHeight(tree_x, tree_y));
            matrix2.mul(matrix, matrix);
            com.oddlabs.tt.landscape.TreeLeaf tree_leaf = findLeaf(world, tree_x, tree_y);
            com.oddlabs.tt.landscape.TreeSupply tree = new com.oddlabs.tt.landscape.TreeSupply(world, tree_leaf, tree_x, tree_y, center_grid_x, center_grid_y, kind.grid_size(), radius, matrix, tree_type, tree_low_vertices);
            tree_leaf.insertTree(tree);
            return tree;
        }
        // Java: 134
        public com.oddlabs.tt.landscape.TreeLeaf findLeaf(com.oddlabs.tt.landscape.World world, float tree_x, float tree_y)
        {
            com.oddlabs.tt.landscape.TreeLeaf[] found = new com.oddlabs.tt.landscape.TreeLeaf[1];
            visit(new AbstractTreeGroup.Anon1_TreeNodeVisitor(this, world, found, tree_x, tree_y));
            return found[0];
        }
        // Java: 182
        public abstract void visit(com.oddlabs.tt.landscape.TreeNodeVisitor visitor);
        // Java: 184
        public virtual bool initBounds()
        {
            return true;
        }
        private void init_AbstractTreeGroup()
        {
            this.num_responding_trees = 0;
        }
        public AbstractTreeGroup(com.oddlabs.tt.landscape.AbstractTreeGroup parent) : base()
        {
            init_AbstractTreeGroup();
            this.parent = parent;
        }
        // Java: anonymous class at line 136
        public sealed class Anon1_TreeNodeVisitor : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public com.oddlabs.tt.landscape.AbstractTreeGroup this_0;
            public com.oddlabs.tt.landscape.World world;
            public com.oddlabs.tt.landscape.TreeLeaf[] found;
            public float tree_x;
            public float tree_y;
            public Anon1_TreeNodeVisitor(com.oddlabs.tt.landscape.AbstractTreeGroup this_0_, com.oddlabs.tt.landscape.World world_c, com.oddlabs.tt.landscape.TreeLeaf[] found_c, float tree_x_c, float tree_y_c)
            {
                this.this_0 = this_0_;
                this.world = world_c;
                this.found = found_c;
                this.tree_x = tree_x_c;
                this.tree_y = tree_y_c;
                init_Anon1_TreeNodeVisitor();
            }
            public int child_size;
            public int x;
            public int y;
            // Java: 141
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf tree_leaf)
            {
                found[0] = tree_leaf;
            }
            // Java: 146
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup tree_group)
            {
                int old_x = x;
                int old_y = y;
                int old_size = child_size;
                child_size >>= 1;
                if (tree_x < x + child_size)
                {
                    if (tree_y < y + child_size)
                    {
                        tree_group.getChild0().visit(this);
                    }
                    else
                    {
                        y += child_size;
                        tree_group.getChild2().visit(this);
                    }
                }
                else
                {
                    if (tree_y < y + child_size)
                    {
                        x += child_size;
                        tree_group.getChild1().visit(this);
                    }
                    else
                    {
                        x += child_size;
                        y += child_size;
                        tree_group.getChild3().visit(this);
                    }
                }
                x = old_x;
                y = old_y;
                child_size = old_size;
            }
            // Java: 174
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply tree_supply)
            {
                throw new System.Exception();
            }
            private void init_Anon1_TreeNodeVisitor()
            {
                this.child_size = world.getHeightMap().getMetersPerWorld();
            }
        }
    }
}

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
            switch ((terrain).ordinal())
            {
                case 0 /*NATIVE*/:
                {
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.JUNGLE, 3, 2.3f, tree_positions, 0.25f, 0.75f);
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.PALM, 1, 1.6f, palm_tree_positions, 0.5f, 1.0f);
                    break;
                }
                case 1 /*VIKING*/:
                {
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.OAK, 3, 2.3f, tree_positions, 0.5f, 1.0f);
                    root.buildTrees(world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType.PINE, 1, 1.6f, palm_tree_positions, 0.5f, 1.0f);
                    break;
                }
            }
            root.initBounds();
            return root;
        }
        // Java: 63
        public void buildTrees(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type, int grid_size, float radius, java.util.List tree_positions, float scale_factor, float min_size)
        {
            org.joml.Matrix4f matrix2 = new org.joml.Matrix4f();
            org.joml.Vector3f vector = new org.joml.Vector3f();
            float h = 15.0f;
            float[] tree_low_vertices = new float[] { (float)(-radius), (float)(-radius), 0, radius, (float)(-radius), 0, radius, radius, 0, (float)(-radius), radius, 0, (float)(-radius), (float)(-radius), h, radius, (float)(-radius), h, radius, radius, h, (float)(-radius), radius, h };
            foreach (int[] coords in java.lang.JIter.of(tree_positions))
            {
                org.joml.Matrix4f matrix = new org.joml.Matrix4f();
                int center_grid_x = coords[0];
                int center_grid_y = coords[1];
                float tree_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid_x);
                float tree_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(center_grid_y);
                float rotation = (float)(world.getRandom().nextFloat() * 360.0f);
                float scale_base = (float)((float)(world.getRandom().nextFloat() * scale_factor) + min_size);
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
                visit(new AbstractTreeGroup.Anon1_TreeNodeVisitor(this, world, tree_x, tree_y, center_grid_x, center_grid_y, grid_size, radius, matrix, tree_type, tree_low_vertices));
            }
        }
        // Java: 138
        public abstract void visit(com.oddlabs.tt.landscape.TreeNodeVisitor visitor);
        // Java: 140
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
        // Java: anonymous class at line 90
        public sealed class Anon1_TreeNodeVisitor : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public com.oddlabs.tt.landscape.AbstractTreeGroup this_0;
            public com.oddlabs.tt.landscape.World world;
            public float tree_x;
            public float tree_y;
            public int center_grid_x;
            public int center_grid_y;
            public int grid_size;
            public float radius;
            public org.joml.Matrix4f matrix;
            public com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type;
            public float[] tree_low_vertices;
            public Anon1_TreeNodeVisitor(com.oddlabs.tt.landscape.AbstractTreeGroup this_0_, com.oddlabs.tt.landscape.World world_c, float tree_x_c, float tree_y_c, int center_grid_x_c, int center_grid_y_c, int grid_size_c, float radius_c, org.joml.Matrix4f matrix_c, com.oddlabs.tt.landscape.AbstractTreeGroup.TreeType tree_type_c, float[] tree_low_vertices_c)
            {
                this.this_0 = this_0_;
                this.world = world_c;
                this.tree_x = tree_x_c;
                this.tree_y = tree_y_c;
                this.center_grid_x = center_grid_x_c;
                this.center_grid_y = center_grid_y_c;
                this.grid_size = grid_size_c;
                this.radius = radius_c;
                this.matrix = matrix_c;
                this.tree_type = tree_type_c;
                this.tree_low_vertices = tree_low_vertices_c;
                init_Anon1_TreeNodeVisitor();
            }
            public int child_size;
            public int x;
            public int y;
            // Java: 95
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf tree_leaf)
            {
                com.oddlabs.tt.landscape.TreeSupply tree = new com.oddlabs.tt.landscape.TreeSupply(world, tree_leaf, tree_x, tree_y, center_grid_x, center_grid_y, grid_size, radius, matrix, tree_type, tree_low_vertices);
                tree_leaf.insertTree(tree);
            }
            // Java: 102
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
            // Java: 130
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

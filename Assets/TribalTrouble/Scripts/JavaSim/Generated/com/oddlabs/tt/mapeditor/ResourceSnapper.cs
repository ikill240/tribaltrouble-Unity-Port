// Translated from ResourceSnapper.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 30
    public class ResourceSnapper : java.lang.Object
    {
        public const float MARGIN = 4.0f;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.landscape.HeightMap height_map;
        public java.util.List models;
        public float x0;
        public float y0;
        public float x1;
        public float y1;
        // Java: 50
        public void snap(int grid_x0, int grid_y0, int grid_x1, int grid_y1)
        {
            x0 = (float)(grid_x0 * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID - com.oddlabs.tt.mapeditor.ResourceSnapper.MARGIN);
            y0 = (float)(grid_y0 * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID - com.oddlabs.tt.mapeditor.ResourceSnapper.MARGIN);
            x1 = (float)(grid_x1 * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID + com.oddlabs.tt.mapeditor.ResourceSnapper.MARGIN);
            y1 = (float)(grid_y1 * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID + com.oddlabs.tt.mapeditor.ResourceSnapper.MARGIN);
            snapModels();
            snapTrees(world.getTreeRoot());
        }
        // Java: 59
        public bool inside(float x, float y)
        {
            return x >= x0 && x <= x1 && y >= y0 && y <= y1;
        }
        // Java: 63
        public bool overlaps(com.oddlabs.tt.util.BoundingBox box)
        {
            return box.bmin_x <= x1 && box.bmax_x >= x0 && box.bmin_y <= y1 && box.bmax_y >= y0;
        }
        // Java: 69
        public void snapModels()
        {
            com.oddlabs.tt.model.AbstractElementNode root = world.getElementRoot();
            root.visit(new ResourceSnapper.Anon1_ElementNodeVisitor(this));
            foreach (com.oddlabs.tt.model.Model model in java.lang.JIter.of(models))
            {
                model.setPosition(model.getPositionX(), model.getPositionY());
            }
            models.clear();
        }
        // Java: 107
        public bool snapTrees(com.oddlabs.tt.landscape.AbstractTreeGroup group)
        {
            bool has_bounds = group.bmin_x <= group.bmax_x;
            if (!has_bounds || !overlaps(group))
            {
                return has_bounds;
            }
            if ((group is com.oddlabs.tt.landscape.TreeLeaf leaf))
            {
                return snapLeaf(leaf);
            }
            com.oddlabs.tt.landscape.TreeGroup node = ((com.oddlabs.tt.landscape.TreeGroup)group);
            com.oddlabs.tt.landscape.AbstractTreeGroup[] children = new com.oddlabs.tt.landscape.AbstractTreeGroup[] { node.getChild0(), node.getChild1(), node.getChild2(), node.getChild3() };
            bool node_bounds = false;
            foreach (com.oddlabs.tt.landscape.AbstractTreeGroup child in children)
            {
                if (!snapTrees(child))
                {
                    continue;
                }
                if (node_bounds)
                {
                    node.checkBounds(child);
                }
                else
                {
                    node.setBounds(child);
                    node_bounds = true;
                }
            }
            return node_bounds;
        }
        // Java: 129
        public bool snapLeaf(com.oddlabs.tt.landscape.TreeLeaf leaf)
        {
            bool[] leaf_bounds = new bool[] { false };
            leaf.visitTrees(new ResourceSnapper.Anon2_TreeNodeVisitor(this, leaf_bounds, leaf));
            return leaf_bounds[0];
        }
        private void init_ResourceSnapper()
        {
            this.models = new java.util.ArrayList();
        }
        public ResourceSnapper(com.oddlabs.tt.landscape.World world) : base()
        {
            init_ResourceSnapper();
            this.world = world;
            this.height_map = world.getHeightMap();
        }
        // Java: anonymous class at line 72
        public sealed class Anon1_ElementNodeVisitor : java.lang.Object, com.oddlabs.tt.model.ElementNodeVisitor
        {
            public com.oddlabs.tt.mapeditor.ResourceSnapper this_0;
            public Anon1_ElementNodeVisitor(com.oddlabs.tt.mapeditor.ResourceSnapper this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 73
            public void visitNode(com.oddlabs.tt.model.ElementNode node)
            {
                if (this_0.overlaps(node))
                {
                    node.visitElements(this);
                    node.visitChildren(this);
                }
            }
            // Java: 81
            public void visitLeaf(com.oddlabs.tt.model.ElementLeaf leaf)
            {
                if (this_0.overlaps(leaf))
                {
                    leaf.visitElements(this);
                }
            }
            // Java: 87
            public void visit(com.oddlabs.tt.model.Element model_p)
            {
                com.oddlabs.tt.model.Model model = (com.oddlabs.tt.model.Model)model_p;
                if (((model is com.oddlabs.tt.model.SupplyModel) || (model is com.oddlabs.tt.model.SceneryModel)) && this_0.inside(model.getPositionX(), model.getPositionY()))
                {
                    this_0.models.add(model);
                }
            }
        }
        // Java: anonymous class at line 131
        public sealed class Anon2_TreeNodeVisitor : java.lang.Object, com.oddlabs.tt.landscape.TreeNodeVisitor
        {
            public com.oddlabs.tt.mapeditor.ResourceSnapper this_0;
            public bool[] leaf_bounds;
            public com.oddlabs.tt.landscape.TreeLeaf leaf;
            public Anon2_TreeNodeVisitor(com.oddlabs.tt.mapeditor.ResourceSnapper this_0_, bool[] leaf_bounds_c, com.oddlabs.tt.landscape.TreeLeaf leaf_c)
            {
                this.this_0 = this_0_;
                this.leaf_bounds = leaf_bounds_c;
                this.leaf = leaf_c;
            }
            // Java: 132
            public void visitTree(com.oddlabs.tt.landscape.TreeSupply tree)
            {
                if (this_0.inside(tree.getPositionX(), tree.getPositionY()))
                {
                    float z = this_0.height_map.getNearestHeight(tree.getPositionX(), tree.getPositionY());
                    float dz = (float)(z - tree.getMatrix().m32());
                    tree.getMatrix().m32(z);
                    tree.bmin_z += dz;
                    tree.bmax_z += dz;
                }
                if (leaf_bounds[0])
                {
                    leaf.checkBounds(tree);
                }
                else
                {
                    leaf.setBounds(tree);
                    leaf_bounds[0] = true;
                }
            }
            // Java: 150
            public void visitLeaf(com.oddlabs.tt.landscape.TreeLeaf tree_leaf)
            {
                throw new System.InvalidOperationException();
            }
            // Java: 155
            public void visitNode(com.oddlabs.tt.landscape.TreeGroup tree_group)
            {
                throw new System.InvalidOperationException();
            }
        }
    }
}

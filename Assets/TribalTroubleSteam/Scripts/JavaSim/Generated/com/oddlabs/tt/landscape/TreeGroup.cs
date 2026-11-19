// Translated from TreeGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 5
    public class TreeGroup : com.oddlabs.tt.landscape.AbstractTreeGroup
    {
        public const int LANDSCAPE_TREES_MAX_LEVEL = 5;
        public com.oddlabs.tt.landscape.AbstractTreeGroup child0;
        public com.oddlabs.tt.landscape.AbstractTreeGroup child1;
        public com.oddlabs.tt.landscape.AbstractTreeGroup child2;
        public com.oddlabs.tt.landscape.AbstractTreeGroup child3;
        // Java: 28
        public com.oddlabs.tt.landscape.AbstractTreeGroup createChild(int level)
        {
            if (level < com.oddlabs.tt.landscape.TreeGroup.LANDSCAPE_TREES_MAX_LEVEL)
            {
                return new com.oddlabs.tt.landscape.TreeGroup(this, level + 1);
            }
            else
            {
                return new com.oddlabs.tt.landscape.TreeLeaf(this);
            }
        }
        // Java: 36
        public override void visit(com.oddlabs.tt.landscape.TreeNodeVisitor visitor)
        {
            visitor.visitNode(this);
        }
        // Java: 41
        public com.oddlabs.tt.landscape.AbstractTreeGroup getChild0()
        {
            return child0;
        }
        // Java: 45
        public com.oddlabs.tt.landscape.AbstractTreeGroup getChild1()
        {
            return child1;
        }
        // Java: 49
        public com.oddlabs.tt.landscape.AbstractTreeGroup getChild2()
        {
            return child2;
        }
        // Java: 53
        public com.oddlabs.tt.landscape.AbstractTreeGroup getChild3()
        {
            return child3;
        }
        // Java: 57
        public void visitChildren(com.oddlabs.tt.landscape.TreeNodeVisitor visitor)
        {
            child0.visit(visitor);
            child1.visit(visitor);
            child2.visit(visitor);
            child3.visit(visitor);
        }
        // Java: 64
        public override bool initBounds()
        {
            bool child0_bounds = child0.initBounds();
            bool child1_bounds = child1.initBounds();
            bool child2_bounds = child2.initBounds();
            bool child3_bounds = child3.initBounds();
            bool node_bounds = false;
            node_bounds = checkBounds(child0, child0_bounds, node_bounds);
            node_bounds = checkBounds(child1, child1_bounds, node_bounds);
            node_bounds = checkBounds(child2, child2_bounds, node_bounds);
            node_bounds = checkBounds(child3, child3_bounds, node_bounds);
            base.initBounds();
            return node_bounds;
        }
        // Java: 79
        public bool checkBounds(com.oddlabs.tt.landscape.AbstractTreeGroup child, bool child_bounds, bool node_bounds)
        {
            if (!child_bounds)
            {
                return node_bounds;
            }
            if (node_bounds)
            {
                checkBounds(child);
            }
            else
            {
                setBounds(child);
            }
            return true;
        }
        public TreeGroup(com.oddlabs.tt.landscape.AbstractTreeGroup parent, int level) : base(parent)
        {
            child0 = createChild(level);
            child1 = createChild(level);
            child2 = createChild(level);
            child3 = createChild(level);
        }
    }
}

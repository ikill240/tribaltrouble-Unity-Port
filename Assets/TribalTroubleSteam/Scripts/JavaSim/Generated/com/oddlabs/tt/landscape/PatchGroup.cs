// Translated from PatchGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 5
    public class PatchGroup : com.oddlabs.tt.landscape.AbstractPatchGroup
    {
        public com.oddlabs.tt.landscape.AbstractPatchGroup child0;
        public com.oddlabs.tt.landscape.AbstractPatchGroup child1;
        public com.oddlabs.tt.landscape.AbstractPatchGroup child2;
        public com.oddlabs.tt.landscape.AbstractPatchGroup child3;
        // Java: 36
        public override void visit(com.oddlabs.tt.landscape.PatchGroupVisitor visitor)
        {
            visitor.visitGroup(this);
        }
        // Java: 41
        public void visitChildren(com.oddlabs.tt.landscape.PatchGroupVisitor visitor)
        {
            child0.visit(visitor);
            child1.visit(visitor);
            child2.visit(visitor);
            child3.visit(visitor);
        }
        // Java: 48
        public com.oddlabs.tt.landscape.AbstractPatchGroup createChild(com.oddlabs.tt.landscape.World world, int size, int x, int y, int level)
        {
            if (size == 1)
            {
                com.oddlabs.tt.landscape.LandscapeLeaf leaf = new com.oddlabs.tt.landscape.LandscapeLeaf(world, x, y, this);
                return leaf;
            }
            else
            {
                com.oddlabs.tt.landscape.PatchGroup group = new com.oddlabs.tt.landscape.PatchGroup(world, size, x, y, level + 1, this);
                return group;
            }
        }
        public PatchGroup(com.oddlabs.tt.landscape.World world) : this(world, world.getHeightMap().getPatchesPerWorld(), 0, 0, 0, null)
        {
        }
        public PatchGroup(com.oddlabs.tt.landscape.World world, int size, int x, int y, int level, com.oddlabs.tt.landscape.AbstractPatchGroup parent) : base(world.getHeightMap(), size, x, y, parent)
        {
            int child_size = size >> 1;
            child0 = createChild(world, child_size, x, y, level);
            child1 = createChild(world, child_size, x + child_size, y, level);
            child2 = createChild(world, child_size, x, y + child_size, level);
            child3 = createChild(world, child_size, x + child_size, y + child_size, level);
            setBounds(child0);
            checkBounds(child1);
            checkBounds(child2);
            checkBounds(child3);
        }
    }
}

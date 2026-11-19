// Translated from AbstractPatchGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 7
    public abstract class AbstractPatchGroup : com.oddlabs.tt.util.BoundingBox
    {
        public com.oddlabs.tt.landscape.AbstractPatchGroup parent;
        // Java: 15
        public void editHeight(float height)
        {
            checkBoundsZ(height);
            if (parent != null)
            {
                parent.editHeight(height);
            }
        }
        // Java: 21
        public abstract void visit(com.oddlabs.tt.landscape.PatchGroupVisitor visitor);
        public AbstractPatchGroup(com.oddlabs.tt.landscape.HeightMap heightmap, float patch_size, int x, int y, com.oddlabs.tt.landscape.AbstractPatchGroup parent) : base()
        {
            this.parent = parent;
        }
    }
}

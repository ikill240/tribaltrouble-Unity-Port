// Translated from LandscapeLeaf.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 5
    public class LandscapeLeaf : com.oddlabs.tt.landscape.AbstractPatchGroup
    {
        public int patch_x;
        public int patch_y;
        // Java: 9
        public float getMaxError()
        {
            return 0;
        }
        // Java: 27
        public float[] getErrors()
        {
            return new float[0];
        }
        // Java: 31
        public int getPatchX()
        {
            return patch_x;
        }
        // Java: 35
        public int getPatchY()
        {
            return patch_y;
        }
        // Java: 39
        public override void visit(com.oddlabs.tt.landscape.PatchGroupVisitor visitor)
        {
            visitor.visitLeaf(this);
        }
        public LandscapeLeaf(com.oddlabs.tt.landscape.World world, int index_x, int index_y, com.oddlabs.tt.landscape.AbstractPatchGroup parent) : base(world.getHeightMap(), 1, index_x, index_y, parent)
        {
            com.oddlabs.tt.landscape.HeightMap heightmap = world.getHeightMap();
            this.patch_x = index_x;
            this.patch_y = index_y;
            int patch_offset_x = index_x * heightmap.getMetersPerPatch();
            int patch_offset_y = index_y * heightmap.getMetersPerPatch();
            setBoundsFromLandscape(heightmap, index_x * heightmap.getGridUnitsPerPatch(), index_y * heightmap.getGridUnitsPerPatch(), heightmap.getGridUnitsPerPatch(), heightmap.getGridUnitsPerPatch());
            checkBoundsZ(heightmap.getSeaLevelMeters());
            heightmap.registerLeaf(patch_x, patch_y, this);
        }
    }
}

// Translated from TreeScanFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 12
    public class TreeScanFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public float x;
        public float y;
        public float radius;
        public java.util.List result;
        // Java: 24
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 29
        public int getMaxRadius()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(radius);
        }
        // Java: 34
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if ((occ is com.oddlabs.tt.landscape.TreeSupply tree) && !tree.isDead())
            {
                float dx = (float)(tree.getPositionX() - x);
                float dy = (float)(tree.getPositionY() - y);
                if ((float)((float)(dx * dx) + (float)(dy * dy)) < (float)(radius * radius) && !result.contains(tree))
                {
                    result.add(tree);
                }
            }
            return false;
        }
        // Java: 45
        public java.util.List getResult()
        {
            return result;
        }
        private void init_TreeScanFilter()
        {
            this.result = new java.util.ArrayList();
        }
        public TreeScanFilter(float x, float y, float radius) : base()
        {
            init_TreeScanFilter();
            this.x = x;
            this.y = y;
            this.radius = radius;
        }
    }
}

// Translated from FindOccupantFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 10
    public class FindOccupantFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public float x;
        public float y;
        public float radius;
        public com.oddlabs.tt.model.Selectable src;
        public System.Type type;
        public java.util.List result;
        // Java: 28
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 33
        public int getMaxRadius()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(radius);
        }
        // Java: 38
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if (((object)occ != (object)src) && type.isInstance(occ))
            {
                com.oddlabs.tt.model.Selectable s = ((com.oddlabs.tt.model.Selectable)occ);
                float dx = (float)(s.getPositionX() - x);
                float dy = (float)(s.getPositionY() - y);
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                if (!result.contains(s) && squared_dist < (float)(radius * radius))
                {
                    result.add(s);
                }
            }
            return false;
        }
        // Java: 53
        public java.lang.Iterable getResult()
        {
            return result;
        }
        public FindOccupantFilter(float x, float y, float radius, com.oddlabs.tt.model.Selectable src, System.Type type) : base()
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            this.src = src;
            this.type = type;
            result = new java.util.ArrayList();
        }
    }
}

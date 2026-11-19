// Translated from LandscapeTarget.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 7
    public class LandscapeTarget : java.lang.Object, com.oddlabs.tt.util.Target
    {
        public int grid_x;
        public int grid_y;
        // Java: 16
        public float getPositionX()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
        }
        // Java: 21
        public float getPositionY()
        {
            return com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
        }
        // Java: 26
        public int getGridX()
        {
            return grid_x;
        }
        // Java: 31
        public int getGridY()
        {
            return grid_y;
        }
        // Java: 36
        public float getSize()
        {
            return 0;
        }
        // Java: 41
        public bool isDead()
        {
            return false;
        }
        // Java: 46
        public override string toString()
        {
            return "LandscapeTarget: grid_x = " + (grid_x) + " | grid_y = " + (grid_y);
        }
        // Java: 51
        public void startRespond()
        {
            throw new System.Exception();
        }
        // Java: 55
        public void stopRespond()
        {
            throw new System.Exception();
        }
        public LandscapeTarget(int grid_x, int grid_y) : base()
        {
            this.grid_x = grid_x;
            this.grid_y = grid_y;
        }
        // mixin of default method Target.isAlive
        public virtual bool isAlive()
        {
            return !isDead();
        }
    }
}

// Translated from NearPointTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 7
    public class NearPointTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public int grid_x;
        public int grid_y;
        public int r;
        public com.oddlabs.tt.model.Unit unit;
        public java.lang.Runnable runnable;
        // Java: 23
        public override void check()
        {
            if (!unit.isDead())
            {
                int dx = unit.getGridX() - grid_x;
                int dy = unit.getGridY() - grid_y;
                int squared_dist = dx * dx + dy * dy;
                if (squared_dist < r * r)
                {
                    triggered();
                }
            }
        }
        // Java: 34
        public override void done()
        {
            runnable.run();
        }
        public NearPointTrigger(int grid_x, int grid_y, int r, com.oddlabs.tt.model.Unit unit, java.lang.Runnable runnable) : base(unit.getOwner().getWorld(), 0.25f, 0.0f)
        {
            this.grid_x = grid_x;
            this.grid_y = grid_y;
            this.r = r;
            this.unit = unit;
            this.runnable = runnable;
        }
    }
}

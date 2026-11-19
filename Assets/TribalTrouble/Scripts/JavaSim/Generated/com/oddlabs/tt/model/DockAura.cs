// Translated from DockAura.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 10
    public class DockAura : java.lang.Object
    {
        public const float PERIOD = 2.0f;
        public const float REACH = 14.0f;
        public const int REPAIR = 3;
        public com.oddlabs.tt.model.LandBuilding dock;
        public float time;
        // Java: 22
        public void animate(float t)
        {
            time += t;
            if (time < com.oddlabs.tt.model.DockAura.PERIOD)
            {
                return;
            }
            time -= com.oddlabs.tt.model.DockAura.PERIOD;
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(dock.getPositionX(), dock.getPositionY(), com.oddlabs.tt.model.DockAura.REACH, dock, typeof(com.oddlabs.tt.model.Ship));
            dock.getUnitGrid().scan(filter, dock.getGridX(), dock.getGridY());
            foreach (com.oddlabs.tt.model.Ship s in java.lang.JIter.of(filter.getResult()))
            {
                if (!s.isDead() && s.isPlaced() && !dock.getOwner().isEnemy(s.getOwner()) && s.isDamaged())
                {
                    s.repair(com.oddlabs.tt.model.DockAura.REPAIR);
                }
            }
        }
        public DockAura(com.oddlabs.tt.model.LandBuilding dock) : base()
        {
            this.dock = dock;
        }
    }
}

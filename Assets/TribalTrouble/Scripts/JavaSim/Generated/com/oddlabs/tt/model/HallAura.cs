// Translated from HallAura.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 20
    public class HallAura : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public const float PERIOD = 2.0f;
        public const int RADIUS = 8;
        public com.oddlabs.tt.model.LandBuilding hall;
        public bool vikings;
        public java.util.List found;
        public float time;
        // Java: 34
        public void animate(float t)
        {
            time += t;
            if (time < com.oddlabs.tt.model.HallAura.PERIOD)
            {
                return;
            }
            time -= com.oddlabs.tt.model.HallAura.PERIOD;
            found.clear();
            hall.getUnitGrid().scan(this, hall.getGridX(), hall.getGridY());
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(found))
            {
                if (!((s is com.oddlabs.tt.model.Unit u)) || u.isDead())
                {
                    continue;
                }
                bool friend = !hall.getOwner().isEnemy(u.getOwner());
                if (vikings)
                {
                    if (friend)
                    {
                        u.heal(1);
                        u.applyCourageBuff((float)(com.oddlabs.tt.model.HallAura.PERIOD * 0.95f), 1.0f, 0.0f, 1.15f, 1.0f, 0.0f);
                    }
                }
                else if (friend)
                {
                    if (u.hasAfflictions())
                    {
                        u.afflictions().cure();
                    }
                }
                else
                {
                    u.afflictions().slow((float)(com.oddlabs.tt.model.HallAura.PERIOD + 0.5f));
                }
            }
            found.clear();
        }
        // Java: 60
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 65
        public int getMaxRadius()
        {
            return com.oddlabs.tt.model.HallAura.RADIUS;
        }
        // Java: 70
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if ((occ is com.oddlabs.tt.model.Unit u) && !u.isDead() && !found.contains(u))
            {
                found.add(u);
            }
            return false;
        }
        private void init_HallAura()
        {
            this.found = new java.util.ArrayList();
        }
        public HallAura(com.oddlabs.tt.model.LandBuilding hall, bool vikings) : base()
        {
            init_HallAura();
            this.hall = hall;
            this.vikings = vikings;
        }
    }
}

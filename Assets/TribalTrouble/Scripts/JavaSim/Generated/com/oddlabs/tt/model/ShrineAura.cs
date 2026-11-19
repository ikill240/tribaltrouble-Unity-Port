// Translated from ShrineAura.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 13
    public class ShrineAura : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public const float PERIOD = 2.0f;
        public const int RADIUS = 7;
        public const int UNIT_HEAL = 1;
        public const int BUILDING_REPAIR = 3;
        public com.oddlabs.tt.model.LandBuilding shrine;
        public java.util.List found;
        public float time;
        // Java: 27
        public void animate(float t)
        {
            time += t;
            if (time < com.oddlabs.tt.model.ShrineAura.PERIOD)
            {
                return;
            }
            time -= com.oddlabs.tt.model.ShrineAura.PERIOD;
            found.clear();
            shrine.getUnitGrid().scan(this, shrine.getGridX(), shrine.getGridY());
            int blessing = shrine.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.BLESSING);
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(found))
            {
                if (s.isDead())
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Unit u))
                {
                    u.heal(com.oddlabs.tt.model.ShrineAura.UNIT_HEAL + blessing);
                }
                else if ((s is com.oddlabs.tt.model.LandBuilding b))
                {
                    if (b.isPlaced() && b.isComplete() && b.isDamaged())
                    {
                        b.repair(com.oddlabs.tt.model.ShrineAura.BUILDING_REPAIR);
                    }
                }
            }
            found.clear();
        }
        // Java: 48
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 53
        public int getMaxRadius()
        {
            return com.oddlabs.tt.model.ShrineAura.RADIUS + 2 * shrine.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.BLESSING);
        }
        // Java: 58
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if ((occ is com.oddlabs.tt.model.Selectable s) && !s.isDead() && ((object)s.getOwnerNoCheck() == (object)shrine.getOwnerNoCheck()) && !found.contains(s))
            {
                found.add(s);
            }
            return false;
        }
        private void init_ShrineAura()
        {
            this.found = new java.util.ArrayList();
        }
        public ShrineAura(com.oddlabs.tt.model.LandBuilding shrine) : base()
        {
            init_ShrineAura();
            this.shrine = shrine;
        }
    }
}

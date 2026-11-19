// Translated from TotemAura.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 25
    public class TotemAura : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public const float PERIOD = 2.0f;
        public const int RADIUS = 7;
        public com.oddlabs.tt.model.LandBuilding totem;
        public bool vikings;
        public java.util.List found;
        public float time;
        public int beat;
        // Java: 40
        public void animate(float t)
        {
            time += t;
            if (time < com.oddlabs.tt.model.TotemAura.PERIOD)
            {
                return;
            }
            time -= com.oddlabs.tt.model.TotemAura.PERIOD;
            found.clear();
            totem.getUnitGrid().scan(this, totem.getGridX(), totem.getGridY());
            com.oddlabs.tt.player.Player owner = totem.getOwner();
            int might = owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RUNE_MIGHT);
            int ward = owner.getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RUNE_WARD);
            bool mend = ward >= 2 || ward == 1 && (beat & 1) == 0;
            beat++;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(found))
            {
                if (!((s is com.oddlabs.tt.model.Unit u)) || u.isDead() || !u.isWarrior() || owner.isEnemy(u.getOwner()))
                {
                    continue;
                }
                if (vikings)
                {
                    u.applyCourageBuff((float)(com.oddlabs.tt.model.TotemAura.PERIOD * 0.99f), 1.0f, 0.0f, (float)(1.0f + (float)(0.05f * might)), (float)(1.2f + (float)(0.1f * might)), 0.0f);
                }
                else
                {
                    u.applyCourageBuff((float)(com.oddlabs.tt.model.TotemAura.PERIOD * 0.99f), (float)(1.1f + (float)(0.05f * might)), (float)(1.0f + (float)(0.5f * might)), 1.0f, 1.0f, 0.0f);
                }
                if (mend)
                {
                    u.heal(1);
                }
            }
            found.clear();
        }
        // Java: 65
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 70
        public int getMaxRadius()
        {
            return com.oddlabs.tt.model.TotemAura.RADIUS + 2 * totem.getOwner().getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.RUNE_REACH);
        }
        // Java: 75
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if ((occ is com.oddlabs.tt.model.Unit u) && !u.isDead() && !found.contains(u))
            {
                found.add(u);
            }
            return false;
        }
        private void init_TotemAura()
        {
            this.found = new java.util.ArrayList();
        }
        public TotemAura(com.oddlabs.tt.model.LandBuilding totem, bool vikings) : base()
        {
            init_TotemAura();
            this.totem = totem;
            this.vikings = vikings;
        }
    }
}

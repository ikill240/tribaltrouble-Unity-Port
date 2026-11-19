// Translated from ShamanAura.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 15
    public class ShamanAura : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        public const float PERIOD = 3.0f;
        public const int RADIUS = 5;
        public const int HEAL = 1;
        public com.oddlabs.tt.model.Unit caster;
        public bool skald;
        public java.util.List found;
        public float time;
        // Java: 30
        public void animate(float t)
        {
            time += t;
            if (time < com.oddlabs.tt.model.ShamanAura.PERIOD)
            {
                return;
            }
            time -= com.oddlabs.tt.model.ShamanAura.PERIOD;
            found.clear();
            caster.getUnitGrid().scan(this, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(caster.getPositionX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(caster.getPositionY()));
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(found))
            {
                if (u.isDead())
                {
                    continue;
                }
                if (skald)
                {
                    u.applyCourageBuff(com.oddlabs.tt.model.ShamanAura.PERIOD, 1.15f, 0.0f, 1.2f, 1.1f, 0.0f);
                }
                else
                {
                    u.heal(com.oddlabs.tt.model.ShamanAura.HEAL);
                }
            }
            found.clear();
        }
        // Java: 49
        public int getMinRadius()
        {
            return 0;
        }
        // Java: 54
        public int getMaxRadius()
        {
            return com.oddlabs.tt.model.ShamanAura.RADIUS;
        }
        // Java: 59
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if ((occ is com.oddlabs.tt.model.Unit u) && ((object)u != (object)caster) && !u.isDead() && ((object)u.getOwnerNoCheck() == (object)caster.getOwnerNoCheck()) && !found.contains(u))
            {
                found.add(u);
            }
            return false;
        }
        private void init_ShamanAura()
        {
            this.found = new java.util.ArrayList();
        }
        public ShamanAura(com.oddlabs.tt.model.Unit caster, bool skald) : base()
        {
            init_ShamanAura();
            this.caster = caster;
            this.skald = skald;
        }
    }
}

// Translated from TowerAttackScanFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 34
    public class TowerAttackScanFilter : java.lang.Object, com.oddlabs.tt.model.TargetPickingScanFilter
    {
        public const int PEON_RUSH_THRESHOLD = 4;
        public int max_range;
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.model.Selectable best_other;
        public com.oddlabs.tt.model.AttackScanFilter.Priority best_other_priority;
        public com.oddlabs.tt.model.Unit best_rubber_or_iron;
        public com.oddlabs.tt.model.Unit best_rock;
        public com.oddlabs.tt.model.Unit best_chief;
        public com.oddlabs.tt.model.Unit best_peon;
        public int peon_count;
        // Java: 55
        public com.oddlabs.tt.model.Selectable removeTarget()
        {
            com.oddlabs.tt.model.Selectable result = default;
            if (best_other != null && ((object)best_other_priority == (object)com.oddlabs.tt.model.AttackScanFilter.Priority.SHIP))
            {
                result = best_other;
            }
            else if (peon_count > com.oddlabs.tt.model.TowerAttackScanFilter.PEON_RUSH_THRESHOLD && best_peon != null)
            {
                result = best_peon;
            }
            else if (best_rubber_or_iron != null)
            {
                result = best_rubber_or_iron;
            }
            else if (best_rock != null)
            {
                result = best_rock;
            }
            else if (best_chief != null)
            {
                result = best_chief;
            }
            else if (best_peon != null)
            {
                result = best_peon;
            }
            else
            {
                result = best_other;
            }
            best_other = null;
            best_other_priority = com.oddlabs.tt.model.AttackScanFilter.Priority.NONE;
            best_rubber_or_iron = null;
            best_rock = null;
            best_chief = null;
            best_peon = null;
            peon_count = 0;
            return result;
        }
        // Java: 83
        public int getMinRadius()
        {
            return 1;
        }
        // Java: 88
        public int getMaxRadius()
        {
            return max_range;
        }
        // Java: 93
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if (!((occ is com.oddlabs.tt.model.Selectable s)) || s.isDead() || !owner.isEnemy(s.getOwner()))
            {
                return false;
            }
            if (!((s is com.oddlabs.tt.model.Unit unit)))
            {
                com.oddlabs.tt.model.AttackScanFilter.Priority priority = s.getAttackPriority();
                if (best_other_priority.@value < priority.@value)
                {
                    best_other_priority = priority;
                    best_other = s;
                }
                return false;
            }
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                if (best_chief == null)
                {
                    best_chief = unit;
                }
                return false;
            }
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
            {
                peon_count++;
                if (best_peon == null)
                {
                    best_peon = unit;
                }
                return false;
            }
            System.Type weapon_type = unit.getWeaponFactory().getType();
            if (((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)) || ((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)weapon_type == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)))
            {
                if (best_rubber_or_iron == null)
                {
                    best_rubber_or_iron = unit;
                }
            }
            else
            {
                if (best_rock == null)
                {
                    best_rock = unit;
                }
            }
            return false;
        }
        private void init_TowerAttackScanFilter()
        {
            this.best_other_priority = com.oddlabs.tt.model.AttackScanFilter.Priority.NONE;
            this.peon_count = 0;
        }
        public TowerAttackScanFilter(com.oddlabs.tt.player.Player owner, int max_range) : base()
        {
            init_TowerAttackScanFilter();
            this.owner = owner;
            this.max_range = max_range;
        }
    }
}

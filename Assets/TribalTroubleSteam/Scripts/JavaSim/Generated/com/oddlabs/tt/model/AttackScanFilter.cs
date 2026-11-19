// Translated from AttackScanFilter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 9
    public class AttackScanFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
    {
        // Java: 10
        public sealed class Priority : java.lang.Enum
        {
            public static Priority[] values() { return (Priority[])VALUES_.Clone(); }
            public static Priority valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Priority NONE = new Priority("NONE", 0, 0);
            public static readonly Priority QUARTERS = new Priority("QUARTERS", 1, 1);
            public static readonly Priority ARMORY = new Priority("ARMORY", 2, 1);
            public static readonly Priority TOWER = new Priority("TOWER", 3, 2);
            public static readonly Priority PEON = new Priority("PEON", 4, 3);
            public static readonly Priority WARRIOR = new Priority("WARRIOR", 5, 4);
            public static readonly Priority SHIP = new Priority("SHIP", 6, 5);
            public int @value;
            public static readonly Priority[] VALUES_ = new Priority[] { NONE, QUARTERS, ARMORY, TOWER, PEON, WARRIOR, SHIP };
            public Priority(string name_, int ordinal_, int @value) : base(name_, ordinal_)
            {
                this.@value = @value;
            }
        }
        public const int UNIT_RANGE = 8;
        public const int TOWER_RANGE = 14;
        public int max_range;
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.model.Selectable target;
        public com.oddlabs.tt.model.AttackScanFilter.Priority target_priority;
        // Java: 41
        public com.oddlabs.tt.model.Selectable removeTarget()
        {
            com.oddlabs.tt.model.Selectable result = target;
            target = null;
            target_priority = com.oddlabs.tt.model.AttackScanFilter.Priority.NONE;
            return result;
        }
        // Java: 48
        public int getMinRadius()
        {
            return 1;
        }
        // Java: 53
        public int getMaxRadius()
        {
            return max_range;
        }
        // Java: 58
        public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
        {
            if ((occ is com.oddlabs.tt.model.Selectable s) && !s.isDead() && owner.isEnemy(s.getOwner()))
            {
                com.oddlabs.tt.model.AttackScanFilter.Priority priority = s.getAttackPriority();
                if (target_priority.@value < priority.@value)
                {
                    target_priority = priority;
                    target = s;
                }
            }
            return false;
        }
        private void init_AttackScanFilter()
        {
            this.target = null;
            this.target_priority = com.oddlabs.tt.model.AttackScanFilter.Priority.NONE;
        }
        public AttackScanFilter(com.oddlabs.tt.player.Player owner, int max_range) : base()
        {
            init_AttackScanFilter();
            this.owner = owner;
            this.max_range = max_range;
        }
    }
}

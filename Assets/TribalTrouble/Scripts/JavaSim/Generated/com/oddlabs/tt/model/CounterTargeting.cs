// Translated from CounterTargeting.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 18
    public class CounterTargeting : java.lang.Object
    {
        public const int RADIUS = 8;
        public const float BETTER = 0.2f;
        public const float HOLD = 2.5f;
        public static bool enabled;
        public static int switches;
        // Java: 32
        public static bool look(com.oddlabs.tt.model.Unit u)
        {
            if (!com.oddlabs.tt.model.CounterTargeting.enabled || u.isDead() || u.isMounted() || !u.isWarrior() || u.getOwner().usesReferenceCommandRules())
            {
                return false;
            }
            com.oddlabs.tt.model.behaviour.Controller c = u.getCurrentController();
            com.oddlabs.tt.model.Selectable current = default;
            if ((c is com.oddlabs.tt.model.behaviour.HuntController h))
            {
                if (h.isOrdered() && !h.getTarget().isDead())
                {
                    return false;
                }
                current = h.getTarget();
            }
            else if ((c is com.oddlabs.tt.model.behaviour.AttackController a))
            {
                current = a.getTarget();
            }
            else
            {
                return false;
            }
            if (current == null || current.isDead() || (current is com.oddlabs.tt.model.Ship))
            {
                return false;
            }
            com.oddlabs.tt.model.Unit current_unit = ((current is com.oddlabs.tt.model.Unit cu) ? cu : (com.oddlabs.tt.model.Unit)(null));
            bool soft = current_unit == null || !current_unit.isWarrior();
            if (!soft && u.retargetHold() > 0.0f)
            {
                return false;
            }
            com.oddlabs.tt.model.CounterTargeting.Scan scan = new com.oddlabs.tt.model.CounterTargeting.Scan(u);
            u.getUnitGrid().scan(scan, u.getGridX(), u.getGridY());
            com.oddlabs.tt.model.Unit best = scan.best;
            if (best == null || ((object)best == (object)current))
            {
                return false;
            }
            if (!soft)
            {
                float now = com.oddlabs.tt.model.CounterTargeting.score(u, current_unit);
                if (scan.best_score < (float)(now + com.oddlabs.tt.model.CounterTargeting.BETTER) || com.oddlabs.tt.model.CounterTargeting.dist2(u, best) > (float)(com.oddlabs.tt.model.CounterTargeting.dist2(u, current_unit) + 4.0f))
                {
                    return false;
                }
            }
            u.setRetargetHold(com.oddlabs.tt.model.CounterTargeting.HOLD);
            com.oddlabs.tt.model.CounterTargeting.switches++;
            u.swapController(new com.oddlabs.tt.model.behaviour.HuntController(u, best));
            return true;
        }
        // Java: 68
        public static float dist2(com.oddlabs.tt.model.Unit a, com.oddlabs.tt.model.Selectable b)
        {
            float dx = (float)(a.getPositionX() - b.getPositionX());
            float dy = (float)(a.getPositionY() - b.getPositionY());
            return (float)((float)(dx * dx) + (float)(dy * dy));
        }
        // Java: 74
        public static float score(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Unit t)
        {
            float f = com.oddlabs.tt.model.Counters.factor(com.oddlabs.tt.model.Counters.role(u), com.oddlabs.tt.model.Counters.role(t));
            float hp = java.lang.Math.max(1, t.getHitPoints());
            float danger = java.lang.Math.min(1.0f, (float)((float)(((t.isWarrior() ? 2.0f : 0.5f)) / hp) * 4.0f));
            return (float)(f + (float)(0.15f * danger));
        }
        // Java: 81
        public class Scan : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
        {
            public com.oddlabs.tt.model.Unit me;
            public com.oddlabs.tt.model.Unit best;
            public float best_score;
            public float best_d;
            // Java: 91
            public int getMinRadius()
            {
                return 0;
            }
            // Java: 96
            public int getMaxRadius()
            {
                return com.oddlabs.tt.model.CounterTargeting.RADIUS;
            }
            // Java: 101
            public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
            {
                if (!((occ is com.oddlabs.tt.model.Unit t)) || t.isDead() || !t.isWarrior() || !me.getOwner().isEnemy(t.getOwner()))
                {
                    return false;
                }
                float s = com.oddlabs.tt.model.CounterTargeting.score(me, t);
                float d = com.oddlabs.tt.model.CounterTargeting.dist2(me, t);
                if (s > (float)(best_score + 1.0E-4f) || java.lang.Math.abs((float)(s - best_score)) <= 1.0E-4f && d < best_d)
                {
                    best_score = s;
                    best_d = d;
                    best = t;
                }
                return false;
            }
            private void init_Scan()
            {
                this.best_score = (float)(-1.0f);
                this.best_d = java.lang.Float.MAX_VALUE;
            }
            public Scan(com.oddlabs.tt.model.Unit me) : base()
            {
                init_Scan();
                this.me = me;
            }
        }
        static CounterTargeting()
        {
            CounterTargeting.enabled = true;
        }
        public CounterTargeting() : base()
        {
        }
    }
}

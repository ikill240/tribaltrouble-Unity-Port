// Translated from IdleController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 10
    public class IdleController : com.oddlabs.tt.model.behaviour.Controller
    {
        public const float MIN_SCAN_DELAY = 1.0f;
        public const float MAX_SCAN_DELAY = 2.0f;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.TargetPickingScanFilter scan_filter;
        public com.oddlabs.tt.model.behaviour.IdleBehaviour idle_behaviour;
        public bool can_move;
        public float redecide_time;
        // Java: 32
        public bool shouldSleep(float t)
        {
            redecide_time -= t;
            return redecide_time > 0;
        }
        // Java: 37
        public override void decide()
        {
            unit.setBehaviour(idle_behaviour);
            if (shouldSleep(0.0f))
            {
                return;
            }
            redecide_time = (float)(com.oddlabs.tt.model.behaviour.IdleController.MIN_SCAN_DELAY + (float)(unit.getOwner().getWorld().getRandom().nextFloat() * ((float)(com.oddlabs.tt.model.behaviour.IdleController.MAX_SCAN_DELAY - com.oddlabs.tt.model.behaviour.IdleController.MIN_SCAN_DELAY))));
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                unit.scanVicinity(scan_filter);
            }
            com.oddlabs.tt.model.Selectable s = scan_filter.removeTarget();
            if (s != null)
            {
                if (can_move)
                {
                    unit.pushControllers(new com.oddlabs.tt.model.behaviour.WalkController(unit, new com.oddlabs.tt.landscape.LandscapeTarget(unit.getGridX(), unit.getGridY()), true), new com.oddlabs.tt.model.behaviour.HuntController(unit, s));
                }
                else
                {
                    unit.pushController(new com.oddlabs.tt.model.behaviour.AttackController(unit, s));
                }
            }
        }
        // Java: 55
        public override string getKey()
        {
            return java.lang.JStr.valueOf(base.getKey()) + java.lang.JStr.valueOf(unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD)) + java.lang.JStr.valueOf(unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC));
        }
        public IdleController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.TargetPickingScanFilter filter, bool can_move) : base(0)
        {
            this.unit = unit;
            this.scan_filter = filter;
            this.idle_behaviour = new com.oddlabs.tt.model.behaviour.IdleBehaviour(this, unit);
            this.can_move = can_move;
        }
    }
}

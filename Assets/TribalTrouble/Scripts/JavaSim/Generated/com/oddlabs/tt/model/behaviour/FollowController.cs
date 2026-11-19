// Translated from FollowController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 26
    public class FollowController : com.oddlabs.tt.model.behaviour.Controller
    {
        public const float FOLLOW_DISTANCE = 3.0f;
        public const float MIN_RECHECK_DELAY = 0.5f;
        public const float MAX_RECHECK_DELAY = 1.0f;
        public com.oddlabs.tt.util.Target target;
        public com.oddlabs.tt.model.Unit unit;
        public bool aggressive;
        public com.oddlabs.tt.model.AttackScanFilter scan_filter;
        public com.oddlabs.tt.model.behaviour.FollowWaitBehaviour wait_behaviour;
        public float redecide_time;
        // Java: 51
        public bool shouldWait(float t)
        {
            redecide_time -= t;
            return redecide_time > 0;
        }
        // Java: 56
        public override void decide()
        {
            if (target.isDead() || ((object)target == (object)unit))
            {
                unit.popController();
                return;
            }
            if (aggressive && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                unit.scanVicinity(scan_filter);
                com.oddlabs.tt.model.Selectable enemy = scan_filter.removeTarget();
                if (enemy != null)
                {
                    unit.pushControllers(new com.oddlabs.tt.model.behaviour.FollowController(unit, target, true), new com.oddlabs.tt.model.behaviour.HuntController(unit, enemy));
                    return;
                }
            }
            if (unit.isCloseEnough(com.oddlabs.tt.model.behaviour.FollowController.FOLLOW_DISTANCE, target))
            {
                resetGiveUpCounter(0);
                unit.setBehaviour(wait_behaviour);
                redecide_time = (float)(com.oddlabs.tt.model.behaviour.FollowController.MIN_RECHECK_DELAY + (float)(unit.getOwner().getWorld().getRandom().nextFloat() * ((float)(com.oddlabs.tt.model.behaviour.FollowController.MAX_RECHECK_DELAY - com.oddlabs.tt.model.behaviour.FollowController.MIN_RECHECK_DELAY))));
            }
            else if (!shouldGiveUp(0))
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, target, com.oddlabs.tt.model.behaviour.FollowController.FOLLOW_DISTANCE, aggressive));
            }
            else
            {
                unit.popController();
            }
        }
        public FollowController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target target, bool aggressive) : base(1)
        {
            this.unit = unit;
            this.target = target;
            this.aggressive = aggressive;
            this.scan_filter = (aggressive ? new com.oddlabs.tt.model.AttackScanFilter(unit.getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE) : (com.oddlabs.tt.model.AttackScanFilter)(null));
            this.wait_behaviour = new com.oddlabs.tt.model.behaviour.FollowWaitBehaviour(this, unit);
        }
    }
}

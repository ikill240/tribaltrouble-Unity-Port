// Translated from LeashedHuntController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 17
    public class LeashedHuntController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.Selectable target;
        public com.oddlabs.tt.util.Target anchor;
        public float max_leash_distance;
        // Java: 32
        public bool canAttack()
        {
            return unit.isCloseEnough(unit.getRange(target), target);
        }
        // Java: 36
        public bool withinLeash()
        {
            float dx = (float)(target.getPositionX() - anchor.getPositionX());
            float dy = (float)(target.getPositionY() - anchor.getPositionY());
            return (float)((float)(dx * dx) + (float)(dy * dy)) <= (float)(max_leash_distance * max_leash_distance);
        }
        // Java: 42
        public override void decide()
        {
            if (target.isDead() || !withinLeash())
            {
                unit.popController();
            }
            else if (canAttack())
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.AttackBehaviour(unit, target));
                resetGiveUpCounter(0);
            }
            else if (!shouldGiveUp(0))
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, target, unit.getRange(target), false));
            }
            else
            {
                unit.popController();
            }
        }
        public LeashedHuntController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.util.Target anchor, float max_leash_distance) : base(1)
        {
            this.unit = unit;
            this.target = target;
            this.anchor = anchor;
            this.max_leash_distance = max_leash_distance;
        }
    }
}

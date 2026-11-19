// Translated from HuntController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 7
    public class HuntController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Selectable target;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 17
        public bool canAttack()
        {
            return unit.isCloseEnough(unit.getRange(target), target);
        }
        // Java: 21
        public override void decide()
        {
            if (target.isDead())
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
        public HuntController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target) : base(1)
        {
            this.unit = unit;
            this.target = target;
        }
    }
}

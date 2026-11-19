// Translated from AttackController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 10
    public class AttackController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Selectable target;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.model.ShipAllocation allocation;
        // Java: 33
        public bool canAttack()
        {
            return unit.isCloseEnough(unit.getRange(target), target, com.oddlabs.tt.pathfinder.UnitGrid.LAND) || unit.isCloseEnough(unit.getRange(target), target, com.oddlabs.tt.pathfinder.UnitGrid.SEA);
        }
        // Java: 40
        public com.oddlabs.tt.model.Selectable getTarget()
        {
            return target;
        }
        // Java: 44
        public override void decide()
        {
            if (target.isDead() || !canAttack())
            {
                unit.popController();
            }
            else
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.AttackBehaviour(unit, target, allocation, ship));
            }
        }
        public AttackController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target) : base(0)
        {
            this.unit = unit;
            this.target = target;
            this.ship = null;
            this.allocation = null;
        }
        public AttackController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.model.ShipAllocation allocation, com.oddlabs.tt.model.Ship ship) : base(0)
        {
            this.unit = unit;
            this.target = target;
            this.ship = ship;
            this.allocation = allocation;
        }
    }
}

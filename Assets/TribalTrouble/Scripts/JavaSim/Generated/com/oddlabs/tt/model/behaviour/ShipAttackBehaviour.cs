// Translated from ShipAttackBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 14
    public class ShipAttackBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public com.oddlabs.tt.model.behaviour.ShipAttackController controller;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.model.ShipAllocation allocation;
        public bool boarded;
        public float boarding_time;
        public const float TOTAL_BOARDING_TIME = 1.0f;
        // Java: 32
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (unit.isDead())
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            }
            if (ship.isMoving() || boarding_time >= com.oddlabs.tt.model.behaviour.ShipAttackBehaviour.TOTAL_BOARDING_TIME)
            {
                boarded = true;
            }
            if (!boarded)
            {
                com.oddlabs.tt.model.Building proxy = ship.getEntrance();
                boarding_time += t;
                unit.switchAnimation(3.0f, com.oddlabs.tt.model.Unit.Animation.MOVING);
                allocation.updateIntermediate(unit, ship, (float)(boarding_time / com.oddlabs.tt.model.behaviour.ShipAttackBehaviour.TOTAL_BOARDING_TIME));
                return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
            }
            else
            {
                unit.switchToIdleAnimation();
                allocation.updateFinal(unit, ship);
                if (!controller.shouldSleep(t))
                {
                    return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
                }
                else
                {
                    return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
                }
            }
        }
        // Java: 58
        public bool isBlocking()
        {
            return true;
        }
        // Java: 62
        public void forceInterrupted()
        {
        }
        private void init_ShipAttackBehaviour()
        {
            this.boarded = false;
            this.boarding_time = 0.0f;
        }
        public ShipAttackBehaviour(com.oddlabs.tt.model.behaviour.ShipAttackController controller, com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.ShipAllocation allocation, bool boarded) : base()
        {
            init_ShipAttackBehaviour();
            this.controller = controller;
            this.unit = unit;
            this.ship = ship;
            this.allocation = allocation;
            this.boarded = boarded;
        }
    }
}

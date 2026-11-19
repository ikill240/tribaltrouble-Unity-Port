// Translated from SittingBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 8
    public class SittingBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public com.oddlabs.tt.model.behaviour.SittingController controller;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.model.ShipAllocation allocation;
        public bool boarded;
        public float boarding_time;
        public const float TOTAL_BOARDING_TIME = 1.0f;
        // Java: 26
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (unit.isDead())
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            }
            if (ship.isMoving() || boarding_time >= com.oddlabs.tt.model.behaviour.SittingBehaviour.TOTAL_BOARDING_TIME)
            {
                boarded = true;
            }
            if (!boarded)
            {
                com.oddlabs.tt.model.Building proxy = ship.getEntrance();
                boarding_time += t;
                unit.switchAnimation(3.0f, com.oddlabs.tt.model.Unit.Animation.MOVING);
                allocation.updateIntermediate(unit, ship, (float)(boarding_time / com.oddlabs.tt.model.behaviour.SittingBehaviour.TOTAL_BOARDING_TIME));
            }
            else
            {
                switch ((allocation.getRole()))
                {
                    case 0:
                    {
                        unit.switchToSittingAnimation();
                        break;
                    }
                    case 4:
                    {
                        unit.switchToSteeringAnimation();
                        break;
                    }
                    case 2:
                    {
                        if (ship.isMoving())
                        {
                            unit.switchToRowingRightAnimation();
                        }
                        else
                        {
                            unit.switchToSittingAnimation();
                        }
                        break;
                    }
                    case 1:
                    {
                        if (ship.isMoving())
                        {
                            unit.switchToRowingLeftAnimation();
                        }
                        else
                        {
                            unit.switchToSittingAnimation();
                        }
                        break;
                    }
                }
                allocation.updateFinal(unit, ship);
            }
            return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
        }
        // Java: 67
        public bool isBlocking()
        {
            return false;
        }
        // Java: 71
        public void forceInterrupted()
        {
        }
        private void init_SittingBehaviour()
        {
            this.boarded = false;
            this.boarding_time = 0.0f;
        }
        public SittingBehaviour(com.oddlabs.tt.model.behaviour.SittingController controller, com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.ShipAllocation allocation, bool boarded) : base()
        {
            init_SittingBehaviour();
            this.controller = controller;
            this.unit = unit;
            this.ship = ship;
            this.allocation = allocation;
            this.boarded = boarded;
        }
    }
}

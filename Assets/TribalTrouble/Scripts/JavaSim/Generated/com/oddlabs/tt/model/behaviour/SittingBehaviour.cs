// Translated from SittingBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 9
    public class SittingBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public com.oddlabs.tt.model.behaviour.SittingController controller;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.model.ShipAllocation allocation;
        // Java: 23
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (unit.isDead())
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            }
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
            float x = ship.getPositionX();
            float y = ship.getPositionY();
            float dx = ship.getDirectionX();
            float dy = ship.getDirectionY();
            float ox = allocation.getOffset().x_f;
            float oy = allocation.getOffset().y_f;
            float gx = (float)((float)(x + (float)(dx * ox)) - (float)(dy * oy));
            float gy = (float)((float)(y + (float)(dy * ox)) + (float)(dx * oy));
            unit.setPosition(gx, gy);
            int gridSize = ship.getUnitGrid().getGridSize();
            int gridX = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gx), 0, gridSize - 1);
            int gridY = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gy), 0, gridSize - 1);
            unit.setGridPosition(gridX, gridY);
            unit.setDirection((float)(-dy), dx);
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
        public SittingBehaviour(com.oddlabs.tt.model.behaviour.SittingController controller, com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.ShipAllocation allocation) : base()
        {
            this.controller = controller;
            this.unit = unit;
            this.ship = ship;
            this.allocation = allocation;
        }
    }
}

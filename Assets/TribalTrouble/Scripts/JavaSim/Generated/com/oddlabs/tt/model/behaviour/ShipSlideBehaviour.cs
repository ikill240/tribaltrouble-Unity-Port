// Translated from ShipSlideBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 11
    public class ShipSlideBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public com.oddlabs.tt.model.Ship ship;
        public bool blocked;
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint curr;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public int grid_size;
        // Java: 27
        public void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append("ShipSlideBehaviour: ");
            if (blocked)
            {
                tool_tip_box.append("BLOCKED");
            }
            else
            {
                tool_tip_box.append("MOVING");
            }
        }
        // Java: 36
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (ship.isDead())
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            }
            if (t == 0.0f)
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
            }
            ship.setLayer(com.oddlabs.tt.pathfinder.UnitGrid.SEA);
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint shipPt = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(ship);
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint next = curr.moved((float)((float)(-1.4f) * t));
            if (com.oddlabs.tt.pathfinder.ShipTrajectory.checkShipsCollision(grid, ship, curr, next.moved((-8))))
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
            }
            blocked = false;
            curr = next;
            ship.free();
            ship.setPosition(curr.positionX, curr.positionY);
            ship.setGridPosition(curr.gridX, curr.gridY);
            ship.occupy();
            if (grid.isDeepWater(curr.gridX, curr.gridY))
            {
                ship.endSlide();
                return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            }
            return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
        }
        // Java: 73
        public bool isBlocking()
        {
            return blocked;
        }
        // Java: 77
        public void forceInterrupted()
        {
        }
        private void init_ShipSlideBehaviour()
        {
            this.blocked = false;
        }
        public ShipSlideBehaviour(com.oddlabs.tt.model.Ship ship) : base()
        {
            init_ShipSlideBehaviour();
            this.ship = ship;
            grid = ship.getUnitGrid();
            grid_size = grid.getGridSize();
            curr = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(ship);
        }
    }
}

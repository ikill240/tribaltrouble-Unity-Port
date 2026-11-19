// Translated from SailBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 11
    public class SailBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const float SHIP_SPEED = 0.45f;
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint next_pose;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.util.Target target;
        public int prev_target_x;
        public int prev_target_y;
        public com.oddlabs.tt.pathfinder.ShipTrajectory trajectory;
        public bool blocked;
        public bool stuck;
        // Java: 31
        public void replanIfNeeded()
        {
            bool no_traj = (trajectory == null || !trajectory.exists());
            bool ship_moved = ((target is com.oddlabs.tt.model.Ship)) && (prev_target_x != target.getGridX() || prev_target_y != target.getGridY());
            if (no_traj || ship_moved)
            {
                this.trajectory = new com.oddlabs.tt.pathfinder.ShipTrajectory(ship, target);
                this.prev_target_x = target.getGridX();
                this.prev_target_y = target.getGridY();
            }
        }
        // Java: 42
        public bool isBlocking()
        {
            return blocked;
        }
        // Java: 46
        public bool isStuck()
        {
            return stuck;
        }
        // Java: 50
        public com.oddlabs.tt.pathfinder.ShipTrajectory getTrajectory()
        {
            return trajectory;
        }
        // Java: 54
        public void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append("SailBehaviour: ");
            if (blocked)
            {
                tool_tip_box.append("BLOCKED");
            }
            else
            {
                tool_tip_box.append("MOVING");
            }
        }
        // Java: 63
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
            replanIfNeeded();
            ship.setLayer(com.oddlabs.tt.pathfinder.UnitGrid.SEA);
            if (trajectory == null || !trajectory.exists())
            {
                ship.endTrip();
                return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
            }
            int rowers = ship.getShipHR().countRowers() + 1;
            if (next_pose == null)
            {
                float speed = (float)((float)(rowers * com.oddlabs.tt.model.behaviour.SailBehaviour.SHIP_SPEED) * ((ship.getShipHR().hasChieftain() ? 2 : 1)));
                next_pose = trajectory.advance((float)(speed * t));
            }
            if (trajectory.reachedGoal())
            {
                ship.endTrip();
                return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
            }
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint fromPoint = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(ship);
            com.oddlabs.tt.pathfinder.UnitGrid grid = ship.getUnitGrid();
            if (fromPoint.distanceTo(next_pose) > 1.0E-4f)
            {
                com.oddlabs.tt.model.Ship blockingShip = com.oddlabs.tt.pathfinder.ShipTrajectory.checkShipsCollision(grid, ship, fromPoint, next_pose.moved(8));
                if (blockingShip != null)
                {
                    if (blockingShip.getOwner().getPlayerInfo().getTeam() == ship.getOwner().getPlayerInfo().getTeam())
                    {
                        stuck = true;
                        ship.reportStuck();
                    }
                    return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
                }
                if (com.oddlabs.tt.pathfinder.ShipTrajectory.checkLandCollision(grid, fromPoint, next_pose))
                {
                    stuck = true;
                    ship.reportStuck();
                    return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
                }
            }
            ship.free();
            ship.setPosition(next_pose.positionX, next_pose.positionY);
            ship.setGridPosition(next_pose.gridX, next_pose.gridY);
            ship.setDirection(next_pose.directionX, next_pose.directionY);
            next_pose = null;
            ship.occupy();
            return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
        }
        // Java: 125
        public void forceInterrupted()
        {
        }
        private void init_SailBehaviour()
        {
            this.next_pose = null;
            this.prev_target_x = 0;
            this.prev_target_y = 0;
            this.trajectory = null;
            this.blocked = false;
            this.stuck = false;
        }
        public SailBehaviour(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.util.Target t) : base()
        {
            init_SailBehaviour();
            this.ship = ship;
            this.target = t;
        }
    }
}

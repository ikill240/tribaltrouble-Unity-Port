// Translated from ShipAllocation.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 8
    public class ShipAllocation : java.lang.Object
    {
        public const int SITTING = 0;
        public const int ROWING_LEFT = 1;
        public const int ROWING_RIGHT = 2;
        public const int FIGHTING = 3;
        public const int STEERING = 4;
        public int role;
        public org.joml.Vector3f offset;
        public org.joml.Vector2f rotation;
        // Java: 29
        public void setOffset(float x, float y, float z)
        {
            offset = new org.joml.Vector3f(x, y, z);
        }
        // Java: 33
        public void setRotation(float x, float y)
        {
            rotation = new org.joml.Vector2f(x, y);
        }
        // Java: 37
        public void setRole(int role)
        {
            this.role = role;
        }
        // Java: 41
        public org.joml.Vector3f getOffset()
        {
            return offset;
        }
        // Java: 45
        public org.joml.Vector2f getRotation()
        {
            return rotation;
        }
        // Java: 49
        public int getRole()
        {
            return role;
        }
        // Java: 53
        public void updateFinal(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship)
        {
            float x = ship.getPositionX();
            float y = ship.getPositionY();
            float dx = ship.getDirectionX();
            float dy = ship.getDirectionY();
            float ox = offset.x_f;
            float oy = offset.y_f;
            float gx = (float)((float)(x + (float)(dx * ox)) - (float)(dy * oy));
            float gy = (float)((float)(y + (float)(dy * ox)) + (float)(dx * oy));
            int gridSize = ship.getUnitGrid().getGridSize();
            int gridX = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gx), 0, gridSize - 1);
            int gridY = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gy), 0, gridSize - 1);
            unit.setReference(ship);
            unit.setPosition(gx, gy);
            unit.setGridPosition(gridX, gridY);
            unit.setDirection((float)(-dy), dx);
            unit.setMountOffset(offset.z_f);
        }
        // Java: 72
        public void updateIntermediate(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship, float progress)
        {
            float x = ship.getPositionX();
            float y = ship.getPositionY();
            float dx = ship.getDirectionX();
            float dy = ship.getDirectionY();
            float ox = offset.x_f;
            float oy = offset.y_f;
            float gx = (float)((float)(x + (float)(dx * ox)) - (float)(dy * oy));
            float gy = (float)((float)(y + (float)(dy * ox)) + (float)(dx * oy));
            com.oddlabs.tt.model.Building proxy = ship.getEntrance();
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0 = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(proxy.getPositionX(), proxy.getPositionY());
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1 = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint(gx, gy);
            p0.setDirectionTo(p1);
            float d = (float)(p0.distanceTo(p1) * progress);
            p0.move(d);
            unit.setPosition(p0.positionX, p0.positionY);
            unit.setGridPosition(p0.gridX, p0.gridY);
            unit.setDirection(p0.directionX, p0.directionY);
            float z = (float)(progress * offset.z_f);
            unit.setMountOffset(z);
        }
        private void init_ShipAllocation()
        {
            this.role = com.oddlabs.tt.model.ShipAllocation.SITTING;
            this.offset = new org.joml.Vector3f(0.0f, 0.0f, 0.0f);
            this.rotation = new org.joml.Vector2f(0.0f, 1.0f);
        }
        public ShipAllocation() : base()
        {
            init_ShipAllocation();
        }
        public ShipAllocation(org.joml.Vector3f offset, org.joml.Vector2f rotation, int role) : base()
        {
            init_ShipAllocation();
            this.role = role;
            this.offset = offset;
            this.rotation = rotation;
        }
    }
}

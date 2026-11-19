// Translated from ShipTrajectoryPoint.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 7
    public class ShipTrajectoryPoint : java.lang.Object
    {
        public int gridX;
        public int gridY;
        public float positionX;
        public float positionY;
        public float directionX;
        public float directionY;
        // Java: 70
        public void setPosition(float x, float y)
        {
            this.positionX = x;
            this.positionY = y;
            this.gridX = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(this.positionX);
            this.gridY = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(this.positionY);
        }
        // Java: 77
        public void setPosition(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt)
        {
            this.positionX = pt.positionX;
            this.positionY = pt.positionY;
            this.gridX = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(this.positionX);
            this.gridY = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(this.positionY);
        }
        // Java: 84
        public void copyFrom(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt)
        {
            this.positionX = pt.positionX;
            this.positionY = pt.positionY;
            this.gridX = pt.gridX;
            this.gridY = pt.gridY;
            this.directionX = pt.directionX;
            this.directionY = pt.directionY;
        }
        // Java: 93
        public void copyDirection(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt)
        {
            this.directionX = pt.directionX;
            this.directionY = pt.directionY;
        }
        // Java: 98
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint moved(int distance)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint moved_v = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint();
            moved_v.gridX = gridX + (java.lang.StrictMath.round((float)(directionX * distance)));
            moved_v.gridY = gridY + (java.lang.StrictMath.round((float)(directionY * distance)));
            moved_v.positionX = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(moved_v.gridX);
            moved_v.positionY = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(moved_v.gridY);
            moved_v.directionX = directionX;
            moved_v.directionY = directionY;
            return moved_v;
        }
        // Java: 109
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint moved(float distance)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint moved_v = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint();
            moved_v.positionX = (float)(positionX + (float)(directionX * distance));
            moved_v.positionY = (float)(positionY + (float)(directionY * distance));
            moved_v.gridX = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(moved_v.positionX);
            moved_v.gridY = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(moved_v.positionY);
            moved_v.directionX = directionX;
            moved_v.directionY = directionY;
            return moved_v;
        }
        // Java: 120
        public void move(int distance)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint moved_v = moved(distance);
            gridX = moved_v.gridX;
            gridY = moved_v.gridY;
            positionX = moved_v.positionX;
            positionY = moved_v.positionY;
            directionX = moved_v.directionX;
            directionY = moved_v.directionY;
        }
        // Java: 130
        public void move(float distance)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint moved_v = moved(distance);
            gridX = moved_v.gridX;
            gridY = moved_v.gridY;
            positionX = moved_v.positionX;
            positionY = moved_v.positionY;
            directionX = moved_v.directionX;
            directionY = moved_v.directionY;
        }
        // Java: 140
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint rotated(float angle)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint rotated_v = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint();
            rotated_v.gridX = gridX;
            rotated_v.gridY = gridY;
            rotated_v.positionX = positionX;
            rotated_v.positionY = positionY;
            float radians = ((float)(java.lang.StrictMath.toRadians(angle)));
            float sin = ((float)(java.lang.StrictMath.sin(radians)));
            float cos = ((float)(java.lang.StrictMath.cos(radians)));
            rotated_v.directionX = (float)((float)(directionX * cos) - (float)(directionY * sin));
            rotated_v.directionY = (float)((float)(directionX * sin) + (float)(directionY * cos));
            float len = ((float)(java.lang.StrictMath.sqrt((float)((float)(rotated_v.directionX * rotated_v.directionX) + (float)(rotated_v.directionY * rotated_v.directionY)))));
            if (len > 1.0E-4f)
            {
                rotated_v.directionX /= len;
                rotated_v.directionY /= len;
            }
            else
            {
                rotated_v.directionX = directionX;
                rotated_v.directionY = directionY;
            }
            return rotated_v;
        }
        // Java: 167
        public void rotate(float angle)
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint rotated_v = rotated(angle);
            gridX = rotated_v.gridX;
            gridY = rotated_v.gridY;
            positionX = rotated_v.positionX;
            positionY = rotated_v.positionY;
            directionX = rotated_v.directionX;
            directionY = rotated_v.directionY;
        }
        // Java: 177
        public float gridDistanceTo(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p)
        {
            int dx = p.gridX - gridX;
            int dy = p.gridY - gridY;
            return ((float)(java.lang.StrictMath.sqrt(dx * dx + dy * dy)));
        }
        // Java: 183
        public float distanceTo(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p)
        {
            float dx = (float)(p.positionX - positionX);
            float dy = (float)(p.positionY - positionY);
            return ((float)(java.lang.StrictMath.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
        }
        // Java: 189
        public void setDirectionTo(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt)
        {
            directionX = (float)(pt.positionX - this.positionX);
            directionY = (float)(pt.positionY - this.positionY);
            float d = ((float)(java.lang.StrictMath.sqrt((float)((float)(directionX * directionX) + (float)(directionY * directionY)))));
            directionX /= d;
            directionY /= d;
        }
        // Java: 197
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint intersection(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt)
        {
            float cross = (float)((float)(pt.directionX * this.directionY) - (float)(pt.directionY * this.directionX));
            if (java.lang.StrictMath.abs(cross) < 1.0E-4f)
            {
                return null;
            }
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint intr = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint();
            float t = (float)(((float)((float)((float)((float)(pt.positionY * this.directionX) - (float)(this.positionY * this.directionX)) - (float)(pt.positionX * this.directionY)) + (float)(this.positionX * this.directionY))) / cross);
            intr.positionX = (float)(pt.positionX + (float)(t * pt.directionX));
            intr.positionY = (float)(pt.positionY + (float)(t * pt.directionY));
            intr.gridX = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(intr.positionX);
            intr.gridY = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(intr.positionY);
            intr.directionX = 0.0f;
            intr.directionY = 1.0f;
            return intr;
        }
        // Java: 214
        public override object clone()
        {
            com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt = new com.oddlabs.tt.pathfinder.ShipTrajectoryPoint();
            pt.positionX = this.positionX;
            pt.positionY = this.positionY;
            pt.directionX = this.directionX;
            pt.directionY = this.directionY;
            pt.gridX = this.gridX;
            pt.gridY = this.gridY;
            return pt;
        }
        // Java: 225
        public float angleRadTo(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pt)
        {
            return ((float)(java.lang.StrictMath.atan2((float)(pt.positionY - positionY), (float)(pt.positionX - positionX))));
        }
        // Java: 229
        public float angleRad()
        {
            return ((float)(java.lang.StrictMath.atan2(directionY, directionX)));
        }
        // Java: 233
        public float angleDeg()
        {
            return ((float)(java.lang.StrictMath.toDegrees(angleRad())));
        }
        // Java: 237
        public override string toString()
        {
            return (gridX) + " " + (gridY);
        }
        public ShipTrajectoryPoint() : base()
        {
        }
        public ShipTrajectoryPoint(int x, int y) : base()
        {
            gridX = x;
            gridY = y;
            positionX = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(gridX);
            positionY = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(gridY);
            directionX = 0.0f;
            directionY = 1.0f;
        }
        public ShipTrajectoryPoint(float x, float y) : base()
        {
            positionX = x;
            positionY = y;
            gridX = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(positionX);
            gridY = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(positionY);
            directionX = 0.0f;
            directionY = 1.0f;
        }
        public ShipTrajectoryPoint(int x, int y, float dx, float dy) : base()
        {
            gridX = x;
            gridY = y;
            positionX = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(gridX);
            positionY = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(gridY);
            directionX = dx;
            directionY = dy;
        }
        public ShipTrajectoryPoint(com.oddlabs.tt.util.Target t) : base()
        {
            gridX = t.getGridX();
            gridY = t.getGridY();
            positionX = t.getPositionX();
            positionY = t.getPositionY();
            if ((t is com.oddlabs.tt.model.Selectable))
            {
                com.oddlabs.tt.model.Selectable selectable = ((com.oddlabs.tt.model.Selectable)t);
                directionX = selectable.getDirectionX();
                directionY = selectable.getDirectionY();
            }
            else
            {
                directionX = 0.0f;
                directionY = 1.0f;
            }
        }
        public ShipTrajectoryPoint(com.oddlabs.tt.model.Ship s) : base()
        {
            gridX = s.getGridX();
            gridY = s.getGridY();
            positionX = s.getPositionX();
            positionY = s.getPositionY();
            directionX = s.getDirectionX();
            directionY = s.getDirectionY();
        }
    }
}

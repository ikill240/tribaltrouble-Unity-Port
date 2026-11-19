// Translated from ShipTrajectorySegment.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 3
    public class ShipTrajectorySegment : java.lang.Object
    {
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0;
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1;
        public com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center;
        public bool isStraight;
        public bool isPivot;
        public float radius;
        public float deltaAngle;
        public float length;
        public float cost;
        public int angle_sign;
        public float progress;
        // Java: 62
        public static float wrapToPi(float angle)
        {
            return ((float)(java.lang.StrictMath.IEEEremainder(angle, 2.0d * java.lang.StrictMath.PI)));
        }
        // Java: 66
        public float advance(float distance, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint pose)
        {
            if ((float)(progress + distance) >= cost)
            {
                pose.copyFrom(p1);
                progress = length;
                return (float)((float)(progress + distance) - length);
            }
            progress += distance;
            if (isStraight)
            {
                pose.setPosition(p0);
                pose.setDirectionTo(p1);
                pose.move(progress);
            }
            else if (isPivot)
            {
                float percent = (float)(progress / this.cost);
                pose.copyFrom(p0);
                pose.rotate((float)(percent * this.deltaAngle));
            }
            else
            {
                float percent_2 = (float)(progress / this.cost);
                pose.copyFrom(center);
                pose.setDirectionTo(p0);
                float deltaAngle = ((float)(java.lang.StrictMath.toDegrees((float)((float)(length * percent_2) / radius))));
                pose.rotate((float)(angle_sign * deltaAngle));
                pose.move(radius);
                pose.rotate((float)(angle_sign * 90.0f));
            }
            return 0.0f;
        }
        private void init_ShipTrajectorySegment()
        {
            this.progress = 0.0f;
        }
        public ShipTrajectorySegment(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1) : base()
        {
            init_ShipTrajectorySegment();
            this.p0 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)p0.clone());
            this.p1 = ((com.oddlabs.tt.pathfinder.ShipTrajectoryPoint)p1.clone());
            if (p0.gridX == p1.gridX && p0.gridY == p1.gridY)
            {
                this.isPivot = true;
                this.isStraight = false;
                this.length = 0.0f;
                float delta = (float)(p1.angleDeg() - p0.angleDeg());
                if (delta > 180.0f)
                {
                    delta = (float)(delta - 360.0f);
                }
                else if (delta < (float)(-180.0f))
                {
                    delta = (float)(delta + 360.0f);
                }
                this.deltaAngle = delta;
                this.cost = (float)(java.lang.Math.abs(delta) * 0.1f);
                this.angle_sign = (delta >= 0.0f ? 1 : (-1));
            }
            else
            {
                this.isStraight = true;
                this.isPivot = false;
                this.length = p0.distanceTo(p1);
                this.cost = this.length;
                this.angle_sign = 1;
                this.deltaAngle = 0.0f;
            }
            this.radius = 0.0f;
            this.center = null;
        }
        public ShipTrajectorySegment(com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p0, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint p1, float radius, com.oddlabs.tt.pathfinder.ShipTrajectoryPoint center) : base()
        {
            init_ShipTrajectorySegment();
            this.p0 = p0;
            this.p1 = p1;
            this.isStraight = false;
            this.isPivot = false;
            this.radius = radius;
            this.center = center;
            float swept = com.oddlabs.tt.pathfinder.ShipTrajectorySegment.wrapToPi((float)(center.angleRadTo(p1) - center.angleRadTo(p0)));
            this.angle_sign = (swept >= 0.0f ? +1 : (-1));
            this.length = (float)(java.lang.StrictMath.abs(swept) * radius);
            this.cost = length;
            this.deltaAngle = 0.0f;
        }
    }
}

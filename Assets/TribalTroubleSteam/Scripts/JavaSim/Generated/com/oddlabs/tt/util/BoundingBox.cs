// Translated from BoundingBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 8
    public class BoundingBox : java.lang.Object
    {
        public static org.joml.Vector4f temp_vec;
        public static org.joml.Vector4f temp_vec2;
        public float bmin_x;
        public float bmin_y;
        public float bmin_z;
        public float bmax_x;
        public float bmax_y;
        public float bmax_z;
        public float cx;
        public float cy;
        public float cz;
        // Java: 23
        public override string toString()
        {
            return "bmx " + java.lang.JStr.valueOf(bmin_x) + " bmy " + java.lang.JStr.valueOf(bmin_y) + " bmz " + java.lang.JStr.valueOf(bmin_z) + " bxx " + java.lang.JStr.valueOf(bmax_x) + " bxy " + java.lang.JStr.valueOf(bmax_y) + " bxz " + java.lang.JStr.valueOf(bmax_z);
        }
        // Java: 28
        public bool collides(float bmin_x, float bmax_x, float bmin_y, float bmax_y, float bmin_z, float bmax_z)
        {
            return (bmin_x <= this.bmax_x && bmax_x > this.bmin_x && bmin_y <= this.bmax_y && bmax_y > this.bmin_y && bmin_z <= this.bmax_z && bmax_z > this.bmin_z);
        }
        // Java: 35
        public bool collides(com.oddlabs.tt.util.BoundingBox other)
        {
            return collides(other.bmin_x, other.bmax_x, other.bmin_y, other.bmax_y, other.bmin_z, other.bmax_z);
        }
        // Java: 39
        public bool contains(float bmin_x, float bmax_x, float bmin_y, float bmax_y, float bmin_z, float bmax_z)
        {
            return (bmax_x <= this.bmax_x && bmin_x > this.bmin_x && bmax_y <= this.bmax_y && bmin_y > this.bmin_y && bmax_z <= this.bmax_z && bmin_z > this.bmin_z);
        }
        // Java: 46
        public bool contains(com.oddlabs.tt.util.BoundingBox other)
        {
            return contains(other.bmin_x, other.bmax_x, other.bmin_y, other.bmax_y, other.bmin_z, other.bmax_z);
        }
        // Java: 50
        public float computeMinDistanceSquared(float min_dist_to_box)
        {
            float box_radius = computeCenteredRadius();
            box_radius = (float)(box_radius + min_dist_to_box);
            return (float)(box_radius * box_radius);
        }
        // Java: 56
        public float computeCenteredRadius()
        {
            float radius_squared_x = (float)(cx - bmin_x);
            float radius_squared_y = (float)(cy - bmin_y);
            float radius_squared_z = (float)(cz - bmin_z);
            return ((float)(java.lang.Math.sqrt((float)((float)((float)(radius_squared_x * radius_squared_x) + (float)(radius_squared_y * radius_squared_y)) + (float)(radius_squared_z * radius_squared_z)))));
        }
        // Java: 64
        public float getCX()
        {
            return cx;
        }
        // Java: 68
        public float getCY()
        {
            return cy;
        }
        // Java: 72
        public float getCZ()
        {
            return cz;
        }
        // Java: 76
        public void checkBoundsY(float y)
        {
            if (y < bmin_y)
            {
                bmin_y = y;
            }
            else if (y > bmax_y)
            {
                bmax_y = y;
            }
            cy = (float)(((float)(bmax_y + bmin_y)) * 0.5f);
        }
        // Java: 84
        public void checkBoundsX(float x)
        {
            if (x < bmin_x)
            {
                bmin_x = x;
            }
            else if (x > bmax_x)
            {
                bmax_x = x;
            }
            cx = (float)(((float)(bmax_x + bmin_x)) * 0.5f);
        }
        // Java: 92
        public void checkBoundsZ(float z)
        {
            if (z < bmin_z)
            {
                bmin_z = z;
            }
            else if (z > bmax_z)
            {
                bmax_z = z;
            }
            cz = (float)(((float)(bmax_z + bmin_z)) * 0.5f);
        }
        // Java: 100
        public void checkBounds(float bmin_x, float bmax_x, float bmin_y, float bmax_y, float bmin_z, float bmax_z)
        {
            checkBoundsX(bmin_x);
            checkBoundsX(bmax_x);
            checkBoundsY(bmin_y);
            checkBoundsY(bmax_y);
            checkBoundsZ(bmin_z);
            checkBoundsZ(bmax_z);
        }
        // Java: 109
        public void checkBoundsXY(com.oddlabs.tt.util.BoundingBox other)
        {
            checkBoundsX(other.bmin_x);
            checkBoundsX(other.bmax_x);
            checkBoundsY(other.bmin_y);
            checkBoundsY(other.bmax_y);
        }
        // Java: 116
        public void checkBounds(com.oddlabs.tt.util.BoundingBox other)
        {
            checkBoundsXY(other);
            checkBoundsZ(other.bmin_z);
            checkBoundsZ(other.bmax_z);
        }
        // Java: 122
        public void setInfiniteBounds()
        {
            this.bmin_x = java.lang.Float.NEGATIVE_INFINITY;
            this.bmax_x = java.lang.Float.POSITIVE_INFINITY;
            this.bmin_y = java.lang.Float.NEGATIVE_INFINITY;
            this.bmax_y = java.lang.Float.POSITIVE_INFINITY;
            this.bmin_z = java.lang.Float.NEGATIVE_INFINITY;
            this.bmax_z = java.lang.Float.POSITIVE_INFINITY;
        }
        // Java: 131
        public void setBounds(com.oddlabs.tt.util.BoundingBox other)
        {
            this.bmin_x = other.bmin_x;
            this.bmax_x = other.bmax_x;
            this.bmin_y = other.bmin_y;
            this.bmax_y = other.bmax_y;
            this.bmin_z = other.bmin_z;
            this.bmax_z = other.bmax_z;
            this.cx = other.cx;
            this.cy = other.cy;
            this.cz = other.cz;
        }
        // Java: 143
        public void setBounds(float bmin_x, float bmax_x, float bmin_y, float bmax_y, float bmin_z, float bmax_z)
        {
            this.bmin_x = bmin_x;
            this.bmax_x = bmax_x;
            this.bmin_y = bmin_y;
            this.bmax_y = bmax_y;
            this.bmin_z = bmin_z;
            this.bmax_z = bmax_z;
            computeCenter();
        }
        // Java: 153
        public void computeXYCenter()
        {
            cx = (float)(((float)(bmax_x + bmin_x)) * 0.5f);
            cy = (float)(((float)(bmax_y + bmin_y)) * 0.5f);
        }
        // Java: 158
        public float computeCenteredXYRadius()
        {
            float radius_squared_x = (float)(cx - bmin_x);
            float radius_squared_y = (float)(cy - bmin_y);
            return ((float)(java.lang.Math.sqrt((float)((float)(radius_squared_x * radius_squared_x) + (float)(radius_squared_y * radius_squared_y)))));
        }
        // Java: 164
        public float computeXYRadius()
        {
            float longest_x = java.lang.Math.max(bmax_x, (float)(-bmin_x));
            float longest_y = java.lang.Math.max(bmax_y, (float)(-bmin_y));
            return ((float)(java.lang.Math.sqrt((float)((float)(longest_x * longest_x) + (float)(longest_y * longest_y)))));
        }
        // Java: 170
        public void transformBounds(org.joml.Matrix4f matrix)
        {
            com.oddlabs.tt.util.BoundingBox.temp_vec.set(bmin_x, bmin_y, bmin_z, 1.0f);
            matrix.transform(com.oddlabs.tt.util.BoundingBox.temp_vec, com.oddlabs.tt.util.BoundingBox.temp_vec2);
            bmin_x = com.oddlabs.tt.util.BoundingBox.temp_vec2.x_f;
            bmin_y = com.oddlabs.tt.util.BoundingBox.temp_vec2.y_f;
            bmin_z = com.oddlabs.tt.util.BoundingBox.temp_vec2.z_f;
            com.oddlabs.tt.util.BoundingBox.temp_vec.set(bmax_x, bmax_y, bmax_z, 1.0f);
            matrix.transform(com.oddlabs.tt.util.BoundingBox.temp_vec, com.oddlabs.tt.util.BoundingBox.temp_vec2);
            bmax_x = com.oddlabs.tt.util.BoundingBox.temp_vec2.x_f;
            bmax_y = com.oddlabs.tt.util.BoundingBox.temp_vec2.y_f;
            bmax_z = com.oddlabs.tt.util.BoundingBox.temp_vec2.z_f;
            computeCenter();
        }
        // Java: 184
        public void computeCenter()
        {
            computeXYCenter();
            cz = (float)(((float)(bmax_z + bmin_z)) * 0.5f);
        }
        // Java: 192
        public void maximizeXYPlane()
        {
            float len = computeXYRadius();
            bmin_x = (float)(-len);
            bmax_x = len;
            bmin_y = (float)(-len);
            bmax_y = len;
            computeXYCenter();
        }
        // Java: 203
        public void setBoundsFromLandscape(com.oddlabs.tt.landscape.HeightMap heightmap, int start_x, int start_y, int size_x, int size_y)
        {
            float corner1 = heightmap.getWrappedHeight(start_x, start_y);
            setBounds(start_x * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, (start_x + size_x) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, start_y * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, (start_y + size_y) * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID, corner1, corner1);
            for (int grid_y = 0; grid_y <= size_x; grid_y++)
            {
                for (int grid_x = 0; grid_x <= size_y; grid_x++)
                {
                    float height = heightmap.getWrappedHeight(start_x + grid_x, start_y + grid_y);
                    checkBoundsZ(height);
                }
            }
        }
        static BoundingBox()
        {
            BoundingBox.temp_vec = new org.joml.Vector4f();
            BoundingBox.temp_vec2 = new org.joml.Vector4f();
        }
        private void init_BoundingBox()
        {
            this.bmin_x = java.lang.Float.POSITIVE_INFINITY;
            this.bmin_y = java.lang.Float.POSITIVE_INFINITY;
            this.bmin_z = java.lang.Float.POSITIVE_INFINITY;
            this.bmax_x = java.lang.Float.NEGATIVE_INFINITY;
            this.bmax_y = java.lang.Float.NEGATIVE_INFINITY;
            this.bmax_z = java.lang.Float.NEGATIVE_INFINITY;
        }
        public BoundingBox() : base()
        {
            init_BoundingBox();
        }
    }
}

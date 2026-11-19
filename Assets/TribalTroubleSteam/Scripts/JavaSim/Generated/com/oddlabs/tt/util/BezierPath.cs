// Translated from BezierPath.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 11
    public class BezierPath : java.lang.Object
    {
        public const int PREVIOUS = 0;
        public const int START = 1;
        public const int END = 2;
        public const int NEXT = 3;
        public static float[] debug_point;
        public static float[] debug_dir;
        public float[][] points;
        public float[] current_point;
        public float[] current_dir;
        public float dt;
        public float t;
        // Java: 30
        public void initState()
        {
            t = 1.0f;
        }
        // Java: 34
        public bool isDone()
        {
            return t >= 1.0f;
        }
        // Java: 38
        public void computeCurvePoint(float speed)
        {
            computeCurvePointFromTime(t, current_point, current_dir);
            t += (float)(dt * speed);
        }
        // Java: 44
        public void dumpPoints()
        {
            foreach (float[] point in points)
            {
                java.lang.JSystem.@out.println("points[i][0] = " + java.lang.JStr.valueOf(point[0]) + " | points[i][1] = " + java.lang.JStr.valueOf(point[1]));
            }
        }
        // Java: 50
        public void computeCurvePointFromTime(float t, float[] point, float[] dir)
        {
            float t2 = (float)(t * t);
            float t3 = (float)(t2 * t);
            float b0 = (float)((float)((float)(1 - (float)(3 * t)) + (float)(3 * t2)) - t3);
            float b1 = (float)((float)((float)(3 * t3) - (float)(6 * t2)) + 4);
            float b2 = (float)((float)((float)((float)((-3) * t3) + (float)(3 * t2)) + (float)(3 * t)) + 1);
            float b3 = t3;
            point[0] = (float)(((float)(1.0f / 6.0f)) * ((float)((float)((float)((float)(points[com.oddlabs.tt.util.BezierPath.PREVIOUS][0] * b0) + (float)(points[com.oddlabs.tt.util.BezierPath.START][0] * b1)) + (float)(points[com.oddlabs.tt.util.BezierPath.END][0] * b2)) + (float)(points[com.oddlabs.tt.util.BezierPath.NEXT][0] * b3))));
            point[1] = (float)(((float)(1.0f / 6.0f)) * ((float)((float)((float)((float)(points[com.oddlabs.tt.util.BezierPath.PREVIOUS][1] * b0) + (float)(points[com.oddlabs.tt.util.BezierPath.START][1] * b1)) + (float)(points[com.oddlabs.tt.util.BezierPath.END][1] * b2)) + (float)(points[com.oddlabs.tt.util.BezierPath.NEXT][1] * b3))));
            float db0 = (float)((float)((-3) + (float)(6 * t)) - (float)(3 * t2));
            float db1 = (float)((float)(9 * t2) - (float)(12 * t));
            float db2 = (float)((float)((float)((-9) * t2) + (float)(6 * t)) + 3);
            float db3 = (float)(3 * t2);
            float dx = (float)(((float)(1.0f / 6.0f)) * ((float)((float)((float)((float)(points[com.oddlabs.tt.util.BezierPath.PREVIOUS][0] * db0) + (float)(points[com.oddlabs.tt.util.BezierPath.START][0] * db1)) + (float)(points[com.oddlabs.tt.util.BezierPath.END][0] * db2)) + (float)(points[com.oddlabs.tt.util.BezierPath.NEXT][0] * db3))));
            float dy = (float)(((float)(1.0f / 6.0f)) * ((float)((float)((float)((float)(points[com.oddlabs.tt.util.BezierPath.PREVIOUS][1] * db0) + (float)(points[com.oddlabs.tt.util.BezierPath.START][1] * db1)) + (float)(points[com.oddlabs.tt.util.BezierPath.END][1] * db2)) + (float)(points[com.oddlabs.tt.util.BezierPath.NEXT][1] * db3))));
            float dir_len_inv = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
            dir[0] = (float)(dx * dir_len_inv);
            dir[1] = (float)(dy * dir_len_inv);
            if (java.lang.Float.isNaN(dir[0]) || java.lang.Float.isNaN(dir[1]))
            {
                dir[0] = 1.0f;
                dir[1] = 0.0f;
            }
        }
        // Java: 75
        public float getCurrentDirectionX()
        {
            return current_dir[0];
        }
        // Java: 79
        public float getCurrentDirectionY()
        {
            return current_dir[1];
        }
        // Java: 83
        public float getCurrentX()
        {
            return current_point[0];
        }
        // Java: 87
        public float getCurrentY()
        {
            return current_point[1];
        }
        // Java: 91
        public void init(float inv_length, float x1, float y1, float x2, float y2)
        {
            float extrapolated_x = (float)(x1 + ((float)(x1 - x2)));
            float extrapolated_y = (float)(y1 + ((float)(y1 - y2)));
            points[com.oddlabs.tt.util.BezierPath.PREVIOUS][0] = extrapolated_x;
            points[com.oddlabs.tt.util.BezierPath.PREVIOUS][1] = extrapolated_y;
            points[com.oddlabs.tt.util.BezierPath.START][0] = extrapolated_x;
            points[com.oddlabs.tt.util.BezierPath.START][1] = extrapolated_y;
            points[com.oddlabs.tt.util.BezierPath.END][0] = x1;
            points[com.oddlabs.tt.util.BezierPath.END][1] = y1;
            points[com.oddlabs.tt.util.BezierPath.NEXT][0] = x2;
            points[com.oddlabs.tt.util.BezierPath.NEXT][1] = y2;
            initState();
            dt = inv_length;
        }
        // Java: 107
        public void nextPoint(float inv_length, float x, float y)
        {
            cyclePoints();
            points[com.oddlabs.tt.util.BezierPath.NEXT][0] = x;
            points[com.oddlabs.tt.util.BezierPath.NEXT][1] = y;
            t -= 1.0f;
            dt = inv_length;
        }
        // Java: 116
        public float getNextX()
        {
            return points[com.oddlabs.tt.util.BezierPath.NEXT][0];
        }
        // Java: 120
        public float getNextY()
        {
            return points[com.oddlabs.tt.util.BezierPath.NEXT][1];
        }
        // Java: 124
        public void cyclePoints()
        {
            float[] previous = points[com.oddlabs.tt.util.BezierPath.PREVIOUS];
            points[com.oddlabs.tt.util.BezierPath.PREVIOUS] = points[com.oddlabs.tt.util.BezierPath.START];
            points[com.oddlabs.tt.util.BezierPath.START] = points[com.oddlabs.tt.util.BezierPath.END];
            points[com.oddlabs.tt.util.BezierPath.END] = points[com.oddlabs.tt.util.BezierPath.NEXT];
            points[com.oddlabs.tt.util.BezierPath.NEXT] = previous;
        }
        // Java: 132
        public void endPath()
        {
            float next_x = (float)(points[com.oddlabs.tt.util.BezierPath.NEXT][0] + ((float)(points[com.oddlabs.tt.util.BezierPath.NEXT][0] - points[com.oddlabs.tt.util.BezierPath.END][0])));
            float next_y = (float)(points[com.oddlabs.tt.util.BezierPath.NEXT][1] + ((float)(points[com.oddlabs.tt.util.BezierPath.NEXT][1] - points[com.oddlabs.tt.util.BezierPath.END][1])));
            nextPoint(dt, next_x, next_y);
        }
        // Java: 138
        public void debugRender(com.oddlabs.tt.landscape.HeightMap heightmap)
        {
            float prev_x = 0;
            float prev_y = 0;
            float prev_z = 0;
            bool first = true;
            for (float t = 0.0f; t < 1.0f; t = (float)(t + 0.01f))
            {
                computeCurvePointFromTime(t, com.oddlabs.tt.util.BezierPath.debug_point, com.oddlabs.tt.util.BezierPath.debug_dir);
                float x = com.oddlabs.tt.util.BezierPath.debug_point[0];
                float y = com.oddlabs.tt.util.BezierPath.debug_point[1];
                float z = (float)(heightmap.getNearestHeight(x, y) + 0.5f);
                if (!first)
                {
                    com.oddlabs.tt.util.DebugRender.drawLine(prev_x, prev_y, prev_z, x, y, z, 1.0f, 1.0f, 1.0f);
                }
                prev_x = x;
                prev_y = y;
                prev_z = z;
                first = false;
            }
        }
        static BezierPath()
        {
            BezierPath.debug_point = new float[2];
            BezierPath.debug_dir = new float[2];
        }
        private void init_BezierPath()
        {
            this.points = java.lang.JA.newArray2<float>(4, 2);
            this.current_point = new float[2];
            this.current_dir = new float[2];
        }
        public BezierPath() : base()
        {
            init_BezierPath();
            initState();
        }
    }
}

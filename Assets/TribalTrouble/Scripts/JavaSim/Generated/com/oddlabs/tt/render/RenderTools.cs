// Translated from RenderTools.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 9
    public class RenderTools : java.lang.Object
    {
        // Java: 11
        public sealed class FrustumIntersection : java.lang.Enum
        {
            public static FrustumIntersection[] values() { return (FrustumIntersection[])VALUES_.Clone(); }
            public static FrustumIntersection valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly FrustumIntersection ALL_OUTSIDE = new FrustumIntersection("ALL_OUTSIDE", 0);
            public static readonly FrustumIntersection INTERSECTING = new FrustumIntersection("INTERSECTING", 1);
            public static readonly FrustumIntersection ALL_INSIDE = new FrustumIntersection("ALL_INSIDE", 2);
            public static readonly FrustumIntersection[] VALUES_ = new FrustumIntersection[] { ALL_OUTSIDE, INTERSECTING, ALL_INSIDE };
            public FrustumIntersection(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 21
        public static void translateAndRotate(com.oddlabs.tt.model.Model model, com.oddlabs.tt.render.MatrixStack stack)
        {
            com.oddlabs.tt.render.RenderTools.translateAndRotate(model.getPositionX(), model.getPositionY(), model.getPositionZ(), model.getDirectionX(), model.getDirectionY(), stack);
        }
        // Java: 30
        public static void translateAndRotate(float x, float y, float z, float dir_x, float dir_y, com.oddlabs.tt.render.MatrixStack stack)
        {
            float angle = ((float)(java.lang.Math.toDegrees(java.lang.Math.atan2(dir_y, dir_x))));
            stack.translate(x, y, z).rotate(angle, 0.0f, 0.0f, 1.0f);
        }
        // Java: 35
        public static com.oddlabs.tt.render.RenderTools.FrustumIntersection inFrustum(com.oddlabs.tt.util.BoundingBox box, float[][] frustum)
        {
            bool all_corners_in_all_planes = true;
            for (int f = 0; f < 6; f++)
            {
                bool any_corner_in_this_plane = false;
                float planeA = frustum[f][0];
                float planeB = frustum[f][1];
                float planeC = frustum[f][2];
                float planeD = frustum[f][3];
                for (int corners_x = 0; corners_x <= 1; corners_x++)
                {
                    float x = (0 == corners_x ? box.bmin_x : box.bmax_x);
                    for (int corners_y = 0; corners_y <= 1; corners_y++)
                    {
                        float y = (0 == corners_y ? box.bmin_y : box.bmax_y);
                        for (int corners_z = 0; corners_z <= 1; corners_z++)
                        {
                            float z = (0 == corners_z ? box.bmin_z : box.bmax_z);
                            float distance = (float)((float)((float)((float)(planeA * x) + (float)(planeB * y)) + (float)(planeC * z)) + planeD);
                            if (distance > 0)
                            {
                                any_corner_in_this_plane = true;
                            }
                            else
                            {
                                all_corners_in_all_planes = false;
                            }
                        }
                    }
                }
                if (!any_corner_in_this_plane)
                {
                    return com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_OUTSIDE;
                }
            }
            return (all_corners_in_all_planes ? com.oddlabs.tt.render.RenderTools.FrustumIntersection.ALL_INSIDE : com.oddlabs.tt.render.RenderTools.FrustumIntersection.INTERSECTING);
        }
        // Java: 78
        public static float getEyeDistanceSquared(com.oddlabs.tt.util.BoundingBox box, float camera_x, float camera_y, float camera_z)
        {
            float distx = (float)(camera_x - box.getCX());
            float disty = (float)(camera_y - box.getCY());
            float distz = (float)(camera_z - box.getCZ());
            float dist2 = (float)((float)((float)(distx * distx) + (float)(disty * disty)) + (float)(distz * distz));
            return dist2;
        }
        // Java: 86
        public static float getCameraDistanceXYSquared(com.oddlabs.tt.util.BoundingBox box, float camera_x, float camera_y)
        {
            float distx = (float)(camera_x - box.getCX());
            float disty = (float)(camera_y - box.getCY());
            float dist2 = (float)((float)(distx * distx) + (float)(disty * disty));
            return dist2;
        }
        // Java: 93
        public static float getCameraDistanceSquared(com.oddlabs.tt.util.BoundingBox box, float camera_x, float camera_y, float camera_z)
        {
            float distz = (float)(camera_z - box.getCZ());
            float dist2 = (float)(com.oddlabs.tt.render.RenderTools.getCameraDistanceXYSquared(box, camera_x, camera_y) + (float)(distz * distz));
            return dist2;
        }
        // Java: 99
        public static void draw(com.oddlabs.tt.util.BoundingBox box)
        {
            com.oddlabs.tt.render.RenderTools.draw(box, 1.0f, 1.0f, 1.0f);
        }
        // Java: 103
        public static void draw(com.oddlabs.tt.util.BoundingBox box, float r, float g, float b)
        {
            com.oddlabs.tt.util.DebugRender.drawBox(box.bmin_x, box.bmax_x, box.bmin_y, box.bmax_y, box.bmin_z, box.bmax_z, r, g, b);
        }
        // Java: 107
        public static void draw(com.oddlabs.tt.util.BoundingBox box, com.oddlabs.tt.global.BoundingMode bound_type, float r, float g, float b)
        {
            com.oddlabs.tt.render.RenderTools.draw(box, r, g, b);
        }
        public RenderTools() : base()
        {
        }
    }
}

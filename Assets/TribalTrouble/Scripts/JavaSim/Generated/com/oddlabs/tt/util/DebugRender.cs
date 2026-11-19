// Translated from DebugRender.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.util
{
    // Java: 14
    public class DebugRender : java.lang.Object
    {
        public static org.joml.Vector4fc AXIS_X_COLOR;
        public static org.joml.Vector4fc AXIS_Y_COLOR;
        public static org.joml.Vector4fc AXIS_Z_COLOR;
        public static org.joml.Vector4fc[] debug_colors;
        public const float CIRCLE_DELTA = 1.5707964f;
        public const float ANGLE_DELTA = 0.15707964f;
        public const float SUBDIV = 0.4f;
        public static com.oddlabs.tt.render.shader.DebugShaderRenderer shaderRenderer;
        public static com.oddlabs.tt.util.DebugRender.FloatList linesBuffer;
        public static com.oddlabs.tt.util.DebugRender.FloatList pointsBuffer;
        public static float currentPointSize;
        // Java: 46
        public static void setShaderRenderer(com.oddlabs.tt.render.shader.DebugShaderRenderer renderer)
        {
            com.oddlabs.tt.util.DebugRender.shaderRenderer = renderer;
        }
        // Java: 54
        public static void flush()
        {
            if (com.oddlabs.tt.util.DebugRender.shaderRenderer == null)
            {
                com.oddlabs.tt.util.DebugRender.linesBuffer.clear();
                com.oddlabs.tt.util.DebugRender.pointsBuffer.clear();
                return;
            }
            if (!com.oddlabs.tt.util.DebugRender.linesBuffer.isEmpty())
            {
                com.oddlabs.tt.util.DebugRender.shaderRenderer.begin(org.lwjgl.opengl.GL11.GL_LINES);
                try
                {
                    for (int i = 0; i < com.oddlabs.tt.util.DebugRender.linesBuffer.size(); i += 6)
                    {
                        com.oddlabs.tt.util.DebugRender.shaderRenderer.vertex(com.oddlabs.tt.util.DebugRender.linesBuffer.get(i), com.oddlabs.tt.util.DebugRender.linesBuffer.get(i + 1), com.oddlabs.tt.util.DebugRender.linesBuffer.get(i + 2), com.oddlabs.tt.util.DebugRender.linesBuffer.get(i + 3), com.oddlabs.tt.util.DebugRender.linesBuffer.get(i + 4), com.oddlabs.tt.util.DebugRender.linesBuffer.get(i + 5));
                    }
                }
                finally
                {
                    com.oddlabs.tt.util.DebugRender.shaderRenderer.end();
                }
                com.oddlabs.tt.util.DebugRender.linesBuffer.clear();
            }
            if (!com.oddlabs.tt.util.DebugRender.pointsBuffer.isEmpty())
            {
                com.oddlabs.tt.util.DebugRender.shaderRenderer.setPointSize(com.oddlabs.tt.util.DebugRender.currentPointSize);
                com.oddlabs.tt.util.DebugRender.shaderRenderer.begin(org.lwjgl.opengl.GL11.GL_POINTS);
                try
                {
                    for (int i_2 = 0; i_2 < com.oddlabs.tt.util.DebugRender.pointsBuffer.size(); i_2 += 6)
                    {
                        com.oddlabs.tt.util.DebugRender.shaderRenderer.vertex(com.oddlabs.tt.util.DebugRender.pointsBuffer.get(i_2), com.oddlabs.tt.util.DebugRender.pointsBuffer.get(i_2 + 1), com.oddlabs.tt.util.DebugRender.pointsBuffer.get(i_2 + 2), com.oddlabs.tt.util.DebugRender.pointsBuffer.get(i_2 + 3), com.oddlabs.tt.util.DebugRender.pointsBuffer.get(i_2 + 4), com.oddlabs.tt.util.DebugRender.pointsBuffer.get(i_2 + 5));
                    }
                }
                finally
                {
                    com.oddlabs.tt.util.DebugRender.shaderRenderer.end();
                }
                com.oddlabs.tt.util.DebugRender.pointsBuffer.clear();
            }
        }
        // Java: 96
        public static void drawBox(float bmin_x, float bmax_x, float bmin_y, float bmax_y, float bmin_z, float bmax_z, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            com.oddlabs.tt.util.DebugRender.drawLine(bmin_x, bmin_y, bmin_z, bmax_x, bmin_y, bmin_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmax_x, bmin_y, bmin_z, bmax_x, bmax_y, bmin_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmax_x, bmax_y, bmin_z, bmin_x, bmax_y, bmin_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmin_x, bmax_y, bmin_z, bmin_x, bmin_y, bmin_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmin_x, bmin_y, bmax_z, bmax_x, bmin_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmax_x, bmin_y, bmax_z, bmax_x, bmax_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmax_x, bmax_y, bmax_z, bmin_x, bmax_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmin_x, bmax_y, bmax_z, bmin_x, bmin_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmin_x, bmin_y, bmin_z, bmin_x, bmin_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmax_x, bmin_y, bmin_z, bmax_x, bmin_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmax_x, bmax_y, bmin_z, bmax_x, bmax_y, bmax_z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(bmin_x, bmax_y, bmin_z, bmin_x, bmax_y, bmax_z, r, g, b);
        }
        // Java: 122
        public static void drawPoint(float x, float y, float z, float size, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            com.oddlabs.tt.util.DebugRender.currentPointSize = size;
            com.oddlabs.tt.util.DebugRender.pointsBuffer.add(x);
            com.oddlabs.tt.util.DebugRender.pointsBuffer.add(y);
            com.oddlabs.tt.util.DebugRender.pointsBuffer.add(z);
            com.oddlabs.tt.util.DebugRender.pointsBuffer.add(r);
            com.oddlabs.tt.util.DebugRender.pointsBuffer.add(g);
            com.oddlabs.tt.util.DebugRender.pointsBuffer.add(b);
        }
        // Java: 137
        public static void drawLine(float x1, float y1, float z1, float x2, float y2, float z2, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(x1);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(y1);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(z1);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(r);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(g);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(b);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(x2);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(y2);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(z2);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(r);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(g);
            com.oddlabs.tt.util.DebugRender.linesBuffer.add(b);
        }
        // Java: 157
        public static void drawAxes(float center, float z)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            com.oddlabs.tt.util.DebugRender.drawLine(center, center, z, (float)(center + 10), center, z, com.oddlabs.tt.util.DebugRender.AXIS_X_COLOR.x(), com.oddlabs.tt.util.DebugRender.AXIS_X_COLOR.y(), com.oddlabs.tt.util.DebugRender.AXIS_X_COLOR.z());
            com.oddlabs.tt.util.DebugRender.drawLine(center, center, z, center, (float)(center + 10), z, com.oddlabs.tt.util.DebugRender.AXIS_Y_COLOR.x(), com.oddlabs.tt.util.DebugRender.AXIS_Y_COLOR.y(), com.oddlabs.tt.util.DebugRender.AXIS_Y_COLOR.z());
            com.oddlabs.tt.util.DebugRender.drawLine(center, center, z, center, center, (float)(z + 10), com.oddlabs.tt.util.DebugRender.AXIS_Z_COLOR.x(), com.oddlabs.tt.util.DebugRender.AXIS_Z_COLOR.y(), com.oddlabs.tt.util.DebugRender.AXIS_Z_COLOR.z());
        }
        // Java: 170
        public static void drawQuad(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, float z, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            com.oddlabs.tt.util.DebugRender.drawLine(x1, y1, z, x2, y2, z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(x2, y2, z, x3, y3, z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(x3, y3, z, x4, y4, z, r, g, b);
            com.oddlabs.tt.util.DebugRender.drawLine(x4, y4, z, x1, y1, z, r, g, b);
        }
        // Java: 182
        public static void drawCylinder(float origin_x, float origin_y, float origin_z, float radius, int num_circles, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            float z = 0.0f;
            for (int i = 0; i < num_circles; i++)
            {
                com.oddlabs.tt.util.DebugRender.drawCircle(radius, origin_x, origin_y, (float)(origin_z + z), r, g, b);
                z = (float)(z + com.oddlabs.tt.util.DebugRender.SUBDIV);
            }
        }
        // Java: 195
        public static void drawCircle(float radius, float origin_x, float origin_y, float origin_z, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            float prevX = (float)((float)(radius * ((float)(java.lang.StrictMath.cos(0)))) + origin_x);
            float prevY = (float)((float)(radius * ((float)(java.lang.StrictMath.sin(0)))) + origin_y);
            float startX = prevX;
            float startY = prevY;
            for (float phi = com.oddlabs.tt.util.DebugRender.ANGLE_DELTA; phi < (float)(((float)(java.lang.Math.PI)) * 2); phi = (float)(phi + com.oddlabs.tt.util.DebugRender.ANGLE_DELTA))
            {
                float x = (float)((float)(radius * ((float)(java.lang.StrictMath.cos(phi)))) + origin_x);
                float y = (float)((float)(radius * ((float)(java.lang.StrictMath.sin(phi)))) + origin_y);
                com.oddlabs.tt.util.DebugRender.drawLine(prevX, prevY, origin_z, x, y, origin_z, r, g, b);
                prevX = x;
                prevY = y;
            }
            com.oddlabs.tt.util.DebugRender.drawLine(prevX, prevY, origin_z, startX, startY, origin_z, r, g, b);
        }
        // Java: 216
        public static void drawSphere(float origin_x, float origin_y, float origin_z, float radius, float r, float g, float b)
        {
            if (null == com.oddlabs.tt.util.DebugRender.shaderRenderer)
            {
                return;
            }
            for (float phi = 0; phi < ((float)(java.lang.Math.PI)); phi = (float)(phi + com.oddlabs.tt.util.DebugRender.CIRCLE_DELTA))
            {
                float prevX = 0;
                float prevY = 0;
                float prevZ = 0;
                bool first = true;
                float startX = 0;
                float startY = 0;
                float startZ = 0;
                for (float rho = 0.0f; rho < (float)(((float)(java.lang.Math.PI)) * 2); rho = (float)(rho + com.oddlabs.tt.util.DebugRender.ANGLE_DELTA))
                {
                    float x_local = (float)(radius * ((float)(java.lang.StrictMath.cos(rho))));
                    float z_local = (float)(radius * ((float)(java.lang.StrictMath.sin(rho))));
                    float y = (float)(x_local * ((float)(java.lang.StrictMath.sin(phi))));
                    float x = (float)(x_local * ((float)(java.lang.StrictMath.cos(phi))));
                    float worldX = (float)(x + origin_x);
                    float worldY = (float)(y + origin_y);
                    float worldZ = (float)(z_local + origin_z);
                    if (!first)
                    {
                        com.oddlabs.tt.util.DebugRender.drawLine(prevX, prevY, prevZ, worldX, worldY, worldZ, r, g, b);
                    }
                    else
                    {
                        startX = worldX;
                        startY = worldY;
                        startZ = worldZ;
                        first = false;
                    }
                    prevX = worldX;
                    prevY = worldY;
                    prevZ = worldZ;
                }
                if (!first)
                {
                    com.oddlabs.tt.util.DebugRender.drawLine(prevX, prevY, prevZ, startX, startY, startZ, r, g, b);
                }
            }
            com.oddlabs.tt.util.DebugRender.drawCircle(radius, origin_x, origin_y, origin_z, r, g, b);
        }
        // Java: 253
        public class FloatList : java.lang.Object
        {
            public float[] elements;
            public int size_f;
            // Java: 261
            public virtual void add(float @value)
            {
                if (size_f == elements.Length)
                {
                    elements = java.util.Arrays.copyOf(elements, size_f * 2);
                }
                elements[size_f++] = @value;
            }
            // Java: 268
            public virtual float get(int index)
            {
                return elements[index];
            }
            // Java: 272
            public virtual int size()
            {
                return size_f;
            }
            // Java: 276
            public virtual void clear()
            {
                size_f = 0;
            }
            // Java: 280
            public virtual bool isEmpty()
            {
                return size_f == 0;
            }
            public FloatList(int initialCapacity) : base()
            {
                elements = new float[initialCapacity];
            }
        }
        static DebugRender()
        {
            DebugRender.AXIS_X_COLOR = com.oddlabs.util.Color.argb4v((-65536));
            DebugRender.AXIS_Y_COLOR = com.oddlabs.util.Color.argb4v((-16711936));
            DebugRender.AXIS_Z_COLOR = com.oddlabs.util.Color.argb4v((-16776961));
            DebugRender.debug_colors = new org.joml.Vector4fc[] { com.oddlabs.util.Color.argb4v((-8446177)), com.oddlabs.util.Color.argb4v((-8446208)), com.oddlabs.util.Color.argb4v((-8454113)), com.oddlabs.util.Color.argb4v((-12615936)), com.oddlabs.util.Color.argb4v((-16769249)), com.oddlabs.util.Color.argb4v((-16769280)), com.oddlabs.util.Color.argb4v((-16777185)), com.oddlabs.util.Color.argb4v((-16777216)), com.oddlabs.util.Color.argb4v((-16752801)), com.oddlabs.util.Color.argb4v((-16752896)), com.oddlabs.util.Color.argb4v((-16777121)), com.oddlabs.util.Color.argb4v((-10514545)), com.oddlabs.util.Color.argb4v((-12624097)), com.oddlabs.util.Color.argb4v((-10526833)), com.oddlabs.util.Color.argb4v((-12636321)), com.oddlabs.util.Color.argb4v((-12632257)), com.oddlabs.util.Color.argb4v((-10543329)), com.oddlabs.util.Color.argb4v((-10543265)), com.oddlabs.util.Color.argb4v((-10526945)), com.oddlabs.util.Color.argb4v((-10526881)) };
            DebugRender.linesBuffer = new com.oddlabs.tt.util.DebugRender.FloatList(1024);
            DebugRender.pointsBuffer = new com.oddlabs.tt.util.DebugRender.FloatList(128);
            DebugRender.currentPointSize = 1.0f;
        }
        public DebugRender() : base()
        {
        }
    }
}

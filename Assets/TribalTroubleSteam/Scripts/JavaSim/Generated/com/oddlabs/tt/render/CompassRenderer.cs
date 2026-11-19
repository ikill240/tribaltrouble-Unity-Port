// Translated from CompassRenderer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.render
{
    // Java: 17
    public class CompassRenderer : java.lang.Object
    {
        public const float RADIUS = 30.0f;
        public const float MARGIN = 50.0f;
        public const float BG_RADIUS = 32.0f;
        public const int BG_SEGMENTS = 32;
        public const float OUTLINE_RADIUS = 30.0f;
        public const float OUTLINE_THICKNESS = 1.5f;
        public const int OUTLINE_SEGMENTS = 30;
        public const float TICK_LENGTH = 6.0f;
        public const float NORTH_TICK_WIDTH = 2.5f;
        public const float TICK_WIDTH = 1.5f;
        public const float NEEDLE_LENGTH = 17.0f;
        public const float NEEDLE_WIDTH = 4.0f;
        public const float NEEDLE_START = 3.0f;
        public const float LABEL_OFFSET = 14.0f;
        public const float CENTER_DOT_SIZE = 3.0f;
        public static org.joml.Vector4fc BG_COLOR;
        public static org.joml.Vector4fc OUTLINE_COLOR;
        public static org.joml.Vector4fc NORTH_COLOR;
        public static org.joml.Vector4fc TICK_COLOR;
        public static org.joml.Vector4fc CENTER_COLOR;
        public static org.joml.Vector4fc LABEL_COLOR;
        public static org.joml.Vector4fc NORTH_LABEL_COLOR;
        public const float NORTH_ANGLE = 1.5707964f;
        public const float EAST_ANGLE = 0.0f;
        public const float SOUTH_ANGLE = (-1.5707964f);
        public const float WEST_ANGLE = 3.1415927f;
        // Java: 52
        public static void render(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.font.Font font, float horizAngle, int screenWidth, int screenHeight)
        {
            float cx = (float)((float)(screenWidth - com.oddlabs.tt.render.CompassRenderer.RADIUS) - com.oddlabs.tt.render.CompassRenderer.MARGIN);
            float cy = (float)(com.oddlabs.tt.render.CompassRenderer.RADIUS + com.oddlabs.tt.render.CompassRenderer.MARGIN);
            com.oddlabs.tt.render.CompassRenderer.drawCircle(renderer, cx, cy, com.oddlabs.tt.render.CompassRenderer.BG_RADIUS, com.oddlabs.tt.render.CompassRenderer.BG_SEGMENTS, com.oddlabs.tt.render.CompassRenderer.BG_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawRing(renderer, cx, cy, com.oddlabs.tt.render.CompassRenderer.OUTLINE_RADIUS, com.oddlabs.tt.render.CompassRenderer.OUTLINE_THICKNESS, com.oddlabs.tt.render.CompassRenderer.OUTLINE_SEGMENTS, com.oddlabs.tt.render.CompassRenderer.OUTLINE_COLOR);
            renderer.drawColoredQuad((float)(cx - (float)(com.oddlabs.tt.render.CompassRenderer.CENTER_DOT_SIZE / 2)), (float)(cy - (float)(com.oddlabs.tt.render.CompassRenderer.CENTER_DOT_SIZE / 2)), com.oddlabs.tt.render.CompassRenderer.CENTER_DOT_SIZE, com.oddlabs.tt.render.CompassRenderer.CENTER_DOT_SIZE, com.oddlabs.tt.render.CompassRenderer.CENTER_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawTick(renderer, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.NORTH_ANGLE, com.oddlabs.tt.render.CompassRenderer.NORTH_TICK_WIDTH, com.oddlabs.tt.render.CompassRenderer.NORTH_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawTick(renderer, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.EAST_ANGLE, com.oddlabs.tt.render.CompassRenderer.TICK_WIDTH, com.oddlabs.tt.render.CompassRenderer.TICK_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawTick(renderer, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.SOUTH_ANGLE, com.oddlabs.tt.render.CompassRenderer.TICK_WIDTH, com.oddlabs.tt.render.CompassRenderer.TICK_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawTick(renderer, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.WEST_ANGLE, com.oddlabs.tt.render.CompassRenderer.TICK_WIDTH, com.oddlabs.tt.render.CompassRenderer.TICK_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawNeedle(renderer, cx, cy, horizAngle);
            com.oddlabs.tt.render.CompassRenderer.drawLabel(renderer, font, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.NORTH_ANGLE, "N", com.oddlabs.tt.render.CompassRenderer.NORTH_LABEL_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawLabel(renderer, font, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.EAST_ANGLE, "E", com.oddlabs.tt.render.CompassRenderer.LABEL_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawLabel(renderer, font, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.SOUTH_ANGLE, "S", com.oddlabs.tt.render.CompassRenderer.LABEL_COLOR);
            com.oddlabs.tt.render.CompassRenderer.drawLabel(renderer, font, cx, cy, horizAngle, com.oddlabs.tt.render.CompassRenderer.WEST_ANGLE, "W", com.oddlabs.tt.render.CompassRenderer.LABEL_COLOR);
        }
        // Java: 86
        public static void drawCircle(com.oddlabs.tt.render.GUIRenderer renderer, float cx, float cy, float radius, int segments, org.joml.Vector4fc color)
        {
            com.oddlabs.tt.render.MatrixStack stack = renderer.getMatrixStack();
            float angleStep = (float)(360.0f / segments);
            for (int i = 0; i < segments; i++)
            {
                stack.push();
                stack.translate(cx, cy, 0);
                stack.rotate((float)(i * angleStep), 0, 0, 1);
                float halfWidth = (float)(radius * ((float)(java.lang.Math.sin(java.lang.Math.toRadians((float)(angleStep / 2))))));
                renderer.drawColoredQuad((float)(-halfWidth), 0, (float)(halfWidth * 2), radius, color);
                stack.pop();
            }
        }
        // Java: 104
        public static void drawRing(com.oddlabs.tt.render.GUIRenderer renderer, float cx, float cy, float radius, float thickness, int segments, org.joml.Vector4fc color)
        {
            com.oddlabs.tt.render.MatrixStack stack = renderer.getMatrixStack();
            float angleStep = (float)(360.0f / segments);
            float segmentLength = (float)((float)(2.0f * radius) * ((float)(java.lang.Math.sin(java.lang.Math.toRadians((float)(angleStep / 2))))));
            for (int i = 0; i < segments; i++)
            {
                stack.push();
                stack.translate(cx, cy, 0);
                stack.rotate((float)(i * angleStep), 0, 0, 1);
                renderer.drawColoredQuad((float)((float)(-segmentLength) / 2), (float)(radius - (float)(thickness / 2)), segmentLength, thickness, color);
                stack.pop();
            }
        }
        // Java: 123
        public static void drawTick(com.oddlabs.tt.render.GUIRenderer renderer, float cx, float cy, float horizAngle, float cardinalAngle, float width, org.joml.Vector4fc color)
        {
            float delta = (float)(cardinalAngle - horizAngle);
            float angleDeg = ((float)(java.lang.Math.toDegrees(delta)));
            com.oddlabs.tt.render.MatrixStack stack = renderer.getMatrixStack();
            stack.push();
            stack.translate(cx, cy, 0);
            stack.rotate(angleDeg, 0, 0, 1);
            float innerEdge = (float)(com.oddlabs.tt.render.CompassRenderer.RADIUS - com.oddlabs.tt.render.CompassRenderer.TICK_LENGTH);
            renderer.drawColoredQuad((float)((float)(-width) / 2), innerEdge, width, com.oddlabs.tt.render.CompassRenderer.TICK_LENGTH, color);
            stack.pop();
        }
        // Java: 142
        public static void drawNeedle(com.oddlabs.tt.render.GUIRenderer renderer, float cx, float cy, float horizAngle)
        {
            float delta = (float)(com.oddlabs.tt.render.CompassRenderer.NORTH_ANGLE - horizAngle);
            float angleDeg = ((float)(java.lang.Math.toDegrees(delta)));
            com.oddlabs.tt.render.MatrixStack stack = renderer.getMatrixStack();
            stack.push();
            stack.translate(cx, cy, 0);
            stack.rotate(angleDeg, 0, 0, 1);
            renderer.drawColoredQuad((float)((float)(-com.oddlabs.tt.render.CompassRenderer.NEEDLE_WIDTH) / 2), com.oddlabs.tt.render.CompassRenderer.NEEDLE_START, com.oddlabs.tt.render.CompassRenderer.NEEDLE_WIDTH, com.oddlabs.tt.render.CompassRenderer.NEEDLE_LENGTH, com.oddlabs.tt.render.CompassRenderer.NORTH_COLOR);
            stack.pop();
        }
        // Java: 159
        public static void drawLabel(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.font.Font font, float cx, float cy, float horizAngle, float cardinalAngle, string label, org.joml.Vector4fc color)
        {
            float delta = (float)(cardinalAngle - horizAngle);
            float dx = (float)(-((float)(java.lang.Math.sin(delta))));
            float dy = ((float)(java.lang.Math.cos(delta)));
            float labelDist = (float)(com.oddlabs.tt.render.CompassRenderer.RADIUS + com.oddlabs.tt.render.CompassRenderer.LABEL_OFFSET);
            float labelX = (float)(cx + (float)(dx * labelDist));
            float labelY = (float)(cy + (float)(dy * labelDist));
            com.oddlabs.util.Quad glyph = font.getQuad(label.charAt(0));
            if (glyph != null)
            {
                float charWidth = glyph.getWidth();
                float charHeight = glyph.getHeight();
                com.oddlabs.tt.font.TextLineRenderer.render(renderer, font, label, (float)(labelX - (float)(charWidth / 2)), (float)(labelY - (float)(charHeight / 2)), java.lang.Float.NEGATIVE_INFINITY, java.lang.Float.POSITIVE_INFINITY, color);
            }
        }
        static CompassRenderer()
        {
            CompassRenderer.BG_COLOR = new org.joml.Vector4f(0.0f, 0.0f, 0.0f, 0.4f);
            CompassRenderer.OUTLINE_COLOR = new org.joml.Vector4f(0.7f, 0.7f, 0.7f, 0.6f);
            CompassRenderer.NORTH_COLOR = new org.joml.Vector4f(0.9f, 0.2f, 0.2f, 0.85f);
            CompassRenderer.TICK_COLOR = new org.joml.Vector4f(0.7f, 0.7f, 0.7f, 0.6f);
            CompassRenderer.CENTER_COLOR = new org.joml.Vector4f(0.5f, 0.5f, 0.5f, 0.5f);
            CompassRenderer.LABEL_COLOR = new org.joml.Vector4f(0.9f, 0.9f, 0.9f, 1.0f);
            CompassRenderer.NORTH_LABEL_COLOR = new org.joml.Vector4f(1.0f, 0.3f, 0.3f, 1.0f);
        }
        public CompassRenderer() : base()
        {
        }
    }
}

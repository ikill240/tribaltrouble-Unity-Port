// GUI port: GUIRenderer collects the frame's quads (same arithmetic as the Java batcher: the current matrix of the
// stack applied to each corner) and hands them to the engine (TribalTrouble.GuiPort.Host.Backend).
#pragma warning disable
using J2CS.Ext;
using System.Collections.Generic;
using TribalTrouble.GuiPort;
namespace com.oddlabs.tt.render
{
    public sealed class GUIRenderer : java.lang.Object
    {
        readonly com.oddlabs.tt.render.MatrixStack matrixStack = new com.oddlabs.tt.render.MatrixStack();
        readonly List<GuiQuad> _quads = new List<GuiQuad>(1024);

        public GUIRenderer() { }

        // window pixels per GUI pixel: the scissor box (set in window pixels) back in GUI pixels
        float _scale = 1f;

        public void renderFrame(com.oddlabs.tt.render.state.RenderContext context, float width, float height, java.lang.Runnable frameCommands)
        {
            _quads.Clear();
            matrixStack.clear();
            _scale = width > 0f && Host.Backend.Width > 0 ? Host.Backend.Width / width : 1f;
            org.lwjgl.opengl.GL11.ScissorTest = false;
            frameCommands.run();
            Host.Backend.DrawGui(width, height, _quads);
        }

        public void drawColoredQuad(float x, float y, float w, float h, org.joml.Vector4fc color)
            => Put(x, y, w, h, 0f, 0f, 0f, 0f, -1, com.oddlabs.util.Color.abgri(color));

        public void drawModeIcon(com.oddlabs.tt.gui.ModeIconQuads iconQuad, com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode, float x, float y)
            => drawIcon(iconQuad.quad(skinMode), x, y);

        public void drawIcon(com.oddlabs.tt.gui.IconQuad q, float x, float y)
            => drawTexture(q.getTexture(), x, y, q.getWidth(), q.getHeight(), q.getU1(), q.getV1(), q.getU2(), q.getV2(), com.oddlabs.util.Color.WHITE);

        public void drawIcon(com.oddlabs.tt.gui.IconQuad q, float x, float y, org.joml.Vector4fc color)
            => drawTexture(q.getTexture(), x, y, q.getWidth(), q.getHeight(), q.getU1(), q.getV1(), q.getU2(), q.getV2(), color);

        public void drawIcon(com.oddlabs.tt.gui.IconQuad q, float x, float y, float w, float h)
            => drawTexture(q.getTexture(), x, y, w, h, q.getU1(), q.getV1(), q.getU2(), q.getV2(), com.oddlabs.util.Color.WHITE);

        public void drawTexture(com.oddlabs.tt.render.Texture texture, float x, float y, float w, float h, float u1, float v1, float u2, float v2, org.joml.Vector4fc tint)
            => Put(x, y, w, h, u1, v1, u2, v2, texture != null ? texture.getHandle() : -1, com.oddlabs.util.Color.abgri(tint));

        void Put(float x, float y, float w, float h, float u1, float v1, float u2, float v2, int tex, int abgr)
        {
            var m = matrixStack.current();
            float x1 = x, y1 = y, x2 = x + w, y2 = y + h;
            if (org.lwjgl.opengl.GL11.ScissorTest && m.m01() == 0f && m.m10() == 0f)
            {
                // the scissor test (Renderable.render for Clipped components, ScrollableGroup): clip the quad to the
                // box, its texture coordinates with it
                float sx = m.m00(), sy = m.m11(), tx = m.m30(), ty = m.m31();
                float cx0 = org.lwjgl.opengl.GL11.ScissorX / _scale, cy0 = org.lwjgl.opengl.GL11.ScissorY / _scale;
                float cx1 = cx0 + org.lwjgl.opengl.GL11.ScissorW / _scale, cy1 = cy0 + org.lwjgl.opengl.GL11.ScissorH / _scale;
                if (sx == 0f || sy == 0f) return;
                // the box in the quad's own coordinates
                float lx0 = (cx0 - tx) / sx, lx1 = (cx1 - tx) / sx, ly0 = (cy0 - ty) / sy, ly1 = (cy1 - ty) / sy;
                if (lx0 > lx1) { float t = lx0; lx0 = lx1; lx1 = t; }
                if (ly0 > ly1) { float t = ly0; ly0 = ly1; ly1 = t; }
                float qx0 = System.Math.Min(x1, x2), qx1 = System.Math.Max(x1, x2), qy0 = System.Math.Min(y1, y2), qy1 = System.Math.Max(y1, y2);
                float nx0 = System.Math.Max(qx0, lx0), nx1 = System.Math.Min(qx1, lx1), ny0 = System.Math.Max(qy0, ly0), ny1 = System.Math.Min(qy1, ly1);
                if (nx0 >= nx1 || ny0 >= ny1) return;   // wholly outside
                if (nx0 != qx0 || nx1 != qx1 || ny0 != qy0 || ny1 != qy1)
                {
                    // u runs from u1 at x1 to u2 at x2, v from v1 at y1 to v2 at y2
                    float nu1 = Lerp(u1, u2, x1, x2, x1 <= x2 ? nx0 : nx1), nu2 = Lerp(u1, u2, x1, x2, x1 <= x2 ? nx1 : nx0);
                    float nv1 = Lerp(v1, v2, y1, y2, y1 <= y2 ? ny0 : ny1), nv2 = Lerp(v1, v2, y1, y2, y1 <= y2 ? ny1 : ny0);
                    if (x1 <= x2) { x1 = nx0; x2 = nx1; } else { x1 = nx1; x2 = nx0; }
                    if (y1 <= y2) { y1 = ny0; y2 = ny1; } else { y1 = ny1; y2 = ny0; }
                    u1 = nu1; u2 = nu2; v1 = nv1; v2 = nv2;
                }
            }
            _quads.Add(new GuiQuad
            {
                tex = tex,
                x0 = m.m00() * x1 + m.m10() * y1 + m.m30(), y0 = m.m01() * x1 + m.m11() * y1 + m.m31(),
                x1 = m.m00() * x2 + m.m10() * y1 + m.m30(), y1 = m.m01() * x2 + m.m11() * y1 + m.m31(),
                x2 = m.m00() * x2 + m.m10() * y2 + m.m30(), y2 = m.m01() * x2 + m.m11() * y2 + m.m31(),
                x3 = m.m00() * x1 + m.m10() * y2 + m.m30(), y3 = m.m01() * x1 + m.m11() * y2 + m.m31(),
                u1 = u1, v1 = v1, u2 = u2, v2 = v2, abgr = abgr,
            });
        }

        static float Lerp(float a, float b, float p0, float p1, float p) => p1 == p0 ? a : a + (b - a) * ((p - p0) / (p1 - p0));

        public void flush() { }

        public com.oddlabs.tt.render.MatrixStack getMatrixStack() => matrixStack;
    }
}

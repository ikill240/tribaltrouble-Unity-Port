// GUI port: the map editor's brush outline and start markers as dots the engine draws (TribalTrouble.GuiPort.EditorView)
// instead of GL decals. Same dot layout as BrushRenderer.java.
#pragma warning disable
using J2CS.Ext;
using TribalTrouble.GuiPort;
namespace com.oddlabs.tt.mapeditor
{
    public class BrushRenderer : java.lang.Object
    {
        public const float DOT_SPACING = 1.5f;
        public const float DOT_SIZE = 1.6f;
        public const int MIN_DOTS = 24;
        public const int MAX_DOTS = 360;

        public class Batch : java.lang.Object, java.lang.AutoCloseable
        {
            public Batch(com.oddlabs.tt.mapeditor.BrushRenderer this_0_, com.oddlabs.tt.render.LandscapeRenderer landscape, com.oddlabs.tt.render.MatrixStack model_view, com.oddlabs.tt.render.MatrixStack projection) : base()
            {
                EditorView.BeginDots();
            }

            public void circle(float cx, float cy, float radius, float r, float g, float b, float a)
            {
                int count = System.Math.Clamp((int)(2 * System.Math.PI * radius / DOT_SPACING), MIN_DOTS, MAX_DOTS);
                for (int i = 0; i < count; i++)
                {
                    double angle = 2 * System.Math.PI * i / count;
                    dot(cx + radius * (float)System.Math.Cos(angle), cy + radius * (float)System.Math.Sin(angle), r, g, b, a);
                }
            }

            public void line(float x0, float y0, float x1, float y1, float r, float g, float b, float a)
            {
                float dx = x1 - x0, dy = y1 - y0;
                int count = System.Math.Clamp((int)(System.Math.Sqrt(dx * dx + dy * dy) / (DOT_SPACING * 2)), 1, MAX_DOTS);
                for (int i = 0; i <= count; i++)
                {
                    float t = i / (float)count;
                    dot(x0 + dx * t, y0 + dy * t, r, g, b, a);
                }
            }

            public void dot(float x, float y, float r, float g, float b, float a) => EditorView.AddDot(x, y, r, g, b, a);

            public void close() => EditorView.EndDots();
        }

        public com.oddlabs.tt.mapeditor.BrushRenderer.Batch begin(com.oddlabs.tt.render.LandscapeRenderer landscape, com.oddlabs.tt.render.MatrixStack model_view, com.oddlabs.tt.render.MatrixStack projection)
            => new Batch(this, landscape, model_view, projection);

        public BrushRenderer() : base() { }
    }
}

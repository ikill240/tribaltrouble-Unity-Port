// GUI port: the map editor's access tint (which ground is reachable from where) handed to the engine as colours
// (TribalTrouble.GuiPort.EditorView) instead of a GL texture drawn over the landscape. Same colours as AccessOverlay.java.
#pragma warning disable
using J2CS.Ext;
using TribalTrouble.GuiPort;
namespace com.oddlabs.tt.mapeditor
{
    public class AccessOverlay : java.lang.Object, java.lang.AutoCloseable
    {
        // premultiplied colours: red, green, blue, opacity
        static readonly float[] REGION = { 0.15f, 0.85f, 0.25f, 0.35f };
        static readonly float[] CUT_OFF = { 1f, 0.6f, 0.05f, 0.45f };
        static readonly float[] CLIFF = { 0.95f, 0.1f, 0.1f, 0.5f };

        readonly com.oddlabs.tt.mapeditor.AccessMap map;
        readonly int size;
        bool visible, stale = true, closed;

        public AccessOverlay(com.oddlabs.tt.mapeditor.AccessMap map) : base()
        {
            this.map = map;
            this.size = map.getSize();
        }

        static int Texel(float[] c)
        {
            int r = (int)System.Math.Round(255 * c[0] * c[3]), g = (int)System.Math.Round(255 * c[1] * c[3]), b = (int)System.Math.Round(255 * c[2] * c[3]);
            int a = (int)System.Math.Round(255 * c[3]);
            return r | g << 8 | b << 16 | a << 24;
        }

        public bool isVisible() => visible;
        public void setVisible(bool visible) { this.visible = visible; EditorView.AccessVisible = visible && !closed; }
        public void mapChanged() => stale = true;

        public void update(com.oddlabs.tt.render.state.RenderContext context)
        {
            if (!visible || !stale || closed) return;
            stale = false;
            int region = Texel(REGION), cut = Texel(CUT_OFF), cliff = Texel(CLIFF);
            var colors = new int[size * size];
            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    var k = map.get(x, y);
                    colors[y * size + x] = k == AccessMap.Kind.REGION ? region : k == AccessMap.Kind.CUT_OFF ? cut : k == AccessMap.Kind.CLIFF ? cliff : 0;
                }
            EditorView.AccessColors = colors;
            EditorView.AccessSize = size;
            EditorView.AccessVersion++;
        }

        public void render(com.oddlabs.tt.render.state.RenderContext context, com.oddlabs.tt.render.LandscapeRenderer landscape, com.oddlabs.tt.camera.CameraState state) { }

        public void close() { closed = true; EditorView.AccessVisible = false; }
    }
}

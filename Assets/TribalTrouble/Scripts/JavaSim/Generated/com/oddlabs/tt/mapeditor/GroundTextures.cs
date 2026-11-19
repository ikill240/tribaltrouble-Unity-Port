// GUI port: GroundTextures without OpenGL. The same bookkeeping as GroundTextures.java (GroundAlphas, which cells
// changed, generated rocks that cast no shadow), but the recomputed ground alphas go to the engine
// (TribalTrouble.GuiPort.EditorView), which blends the island's ground from them itself. Changed heights are passed
// on too, so the engine reshapes its terrain.
#pragma warning disable
using J2CS.Ext;
using TribalTrouble.GuiPort;
namespace com.oddlabs.tt.mapeditor
{
    public class GroundTextures : java.lang.Object, java.lang.AutoCloseable
    {
        const int SHADOW_BLEND = 6;

        readonly com.oddlabs.tt.mapeditor.GroundAlphas alphas;
        readonly bool[] unshaded;
        readonly int size;
        int dirty_x0 = int.MaxValue, dirty_y0 = int.MaxValue, dirty_x1 = int.MinValue, dirty_y1 = int.MinValue;
        bool closed;

        public static com.oddlabs.tt.mapeditor.GroundTextures create(com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.mapeditor.MapSettings settings, com.oddlabs.tt.render.state.RenderContext context, bool generated_supplies)
            => new GroundTextures(world_info, settings, context, generated_supplies);

        public GroundTextures(com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.mapeditor.MapSettings settings, com.oddlabs.tt.render.state.RenderContext context, bool generated_supplies) : base()
        {
            EditorView.Reset();
            var heights = world_info.heightmap();
            size = heights.Length;
            unshaded = new bool[size * size];
            var shaded = new java.util.ArrayList();
            var shadow = world_info.blend_infos()[SHADOW_BLEND].getSourceImage();
            foreach (var supplies in new[] { world_info.rocks(), world_info.iron() })
                for (int i = 0; i < supplies.size(); i++)
                {
                    var p = (int[])supplies.get(i);
                    if (!generated_supplies || shadow.getPixel(p[0], p[1]) >= 128) shaded.add(p);
                    else unshaded[p[1] * size + p[0]] = true;
                }
            alphas = new com.oddlabs.tt.mapeditor.GroundAlphas(heights, settings, world_info.trees(), world_info.palm_trees(), shaded);
        }

        /// <summary>Notes changed heights (inclusive rectangle, in cells); the ground follows on the next update.</summary>
        public void heightsChanged(int x0, int y0, int x1, int y1)
        {
            alphas.heightsChanged(x0, y0, x1, y1);
            markDirty(x0, y0, x1, y1);
            EditorView.HeightsChanged(x0, y0, x1, y1);
        }

        public void resourcesChanged(com.oddlabs.tt.mapeditor.ResourceLayer layer, int x0, int y0, int x1, int y1)
        {
            int reach = alphas.getSupplyShadowReach();
            int sx0 = x0 - reach, sy0 = y0 - reach, sx1 = x1 + reach, sy1 = y1 + reach;
            var shaded = new java.util.ArrayList();
            foreach (var kind in new[] { Resource.ROCK, Resource.IRON })
            {
                var l = layer.positionsIn(kind, sx0, sy0, sx1, sy1);
                for (int i = 0; i < l.size(); i++) { var p = (int[])l.get(i); if (!unshaded[p[1] * size + p[0]]) shaded.add(p); }
            }
            for (int y = System.Math.Max(0, y0); y <= System.Math.Min(size - 1, y1); y++)
                for (int x = System.Math.Max(0, x0); x <= System.Math.Min(size - 1, x1); x++)
                    if (layer.get(x, y) != Resource.ROCK && layer.get(x, y) != Resource.IRON) unshaded[y * size + x] = false;
            alphas.restampSupplies(layer.positionsIn(Resource.TREE, sx0 - reach, sy0 - reach, sx1 + reach, sy1 + reach),
                layer.positionsIn(Resource.PALM, sx0 - reach, sy0 - reach, sx1 + reach, sy1 + reach), shaded, sx0, sy0, sx1, sy1);
            markDirty(sx0, sy0, sx1, sy1);
        }

        public void markDirty(int x0, int y0, int x1, int y1)
        {
            dirty_x0 = System.Math.Min(dirty_x0, x0); dirty_y0 = System.Math.Min(dirty_y0, y0);
            dirty_x1 = System.Math.Max(dirty_x1, x1); dirty_y1 = System.Math.Max(dirty_y1, y1);
        }

        public bool hasChanges() => !closed && dirty_x0 <= dirty_x1;

        public void update()
        {
            if (!hasChanges()) return;
            int[] r = alphas.reach(dirty_x0, dirty_y0, dirty_x1, dirty_y1);
            dirty_x0 = dirty_y0 = int.MaxValue; dirty_x1 = dirty_y1 = int.MinValue;
            rebuild(r[0], r[1], r[2], r[3]);
        }

        public void settle()
        {
            if (closed) return;
            update();
            if (alphas.rangesMoved()) rebuild(0, 0, size - 1, size - 1);
        }

        public void rebuildAll()
        {
            alphas.rangesMoved();
            rebuild(0, 0, size - 1, size - 1);
        }

        public void rebuild(int x0, int y0, int x1, int y1)
        {
            x0 = System.Math.Max(0, x0); y0 = System.Math.Max(0, y0); x1 = System.Math.Min(size - 1, x1); y1 = System.Math.Min(size - 1, y1);
            if (x0 > x1 || y0 > y1) return;
            int n = (x1 - x0 + 1) * (y1 - y0 + 1) * 4;
            var low = java.nio.ByteBuffer.allocate(n);
            var high = java.nio.ByteBuffer.allocate(n);
            alphas.compute(x0, y0, x1, y1, low, high);
            EditorView.AlphasChanged(x0, y0, x1, y1, (byte[])(System.Array)low.array(), (byte[])(System.Array)high.array());
        }

        public void close() => closed = true;
    }
}

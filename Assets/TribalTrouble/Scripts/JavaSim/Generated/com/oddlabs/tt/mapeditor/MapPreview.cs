// Translated from MapPreview.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 18
    public class MapPreview : java.lang.Object
    {
        public int size() { return size_f; }
        public sbyte[] rgb() { return rgb_f; }
        public MapPreview(int size_v, sbyte[] rgb_v)
        {
            if (size_v <= 0 || rgb_v.Length != size_v * size_v * 3)
            {
                throw new System.ArgumentException("Preview of size " + (size_v) + " has " + (rgb_v.Length) + " bytes");
            }
            this.size_f = size_v;
            this.rgb_f = rgb_v;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is MapPreview r)) return false; return true && java.lang.JO.equals(size_f, r.size_f) && java.lang.JO.equals(rgb_f, r.rgb_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(size_f); h = 31 * h + java.lang.JO.hashCode(rgb_f); } return h; }
        public override string toString() { return "MapPreview[" + "size=" + java.lang.JO.toString(size_f) + ", rgb=" + java.lang.JO.toString(rgb_f) + "]"; }
        public int size_f;
        public sbyte[] rgb_f;
        public const int MAX_SIZE = 256;
        public static float[] NATIVE_SHALLOW;
        public static float[] NATIVE_DEEP;
        public static float[] NATIVE_BEACH;
        public static float[] NATIVE_LOW;
        public static float[] NATIVE_HIGH;
        public static float[] NATIVE_STEEP;
        public static float[] VIKING_SHALLOW;
        public static float[] VIKING_DEEP;
        public static float[] VIKING_BEACH;
        public static float[] VIKING_LOW;
        public static float[] VIKING_HIGH;
        public static float[] VIKING_STEEP;
        public static float[] SNOW;
        public static float[] JUNGLE;
        public static float[] PALM;
        public static float[] OAK;
        public static float[] PINE;
        public static float[] ROCK;
        public static float[] IRON;
        // Java: 52
        public static com.oddlabs.tt.mapeditor.MapPreview render(float[][] heights, com.oddlabs.tt.mapeditor.MapSettings settings, com.oddlabs.tt.mapeditor.MapFile.Resources resources)
        {
            return com.oddlabs.tt.mapeditor.MapPreview.render(heights, settings, resources, com.oddlabs.tt.mapeditor.MapPreview.MAX_SIZE);
        }
        // Java: 58
        public static com.oddlabs.tt.mapeditor.MapPreview render(float[][] heights, com.oddlabs.tt.mapeditor.MapSettings settings, com.oddlabs.tt.mapeditor.MapFile.Resources resources, int max_size)
        {
            int grid = heights.Length;
            int size_v = java.lang.Math.min(grid, max_size);
            bool viking = ((object)com.oddlabs.tt.procedural.Landscape.TerrainType.values()[settings.terrain()] == (object)com.oddlabs.tt.procedural.Landscape.TerrainType.VIKING);
            float scale = settings.getHeightScale();
            float sea = (float)(com.oddlabs.tt.global.Globals.SEA_LEVEL * scale);
            float threshold = (float)(settings.getAccessThreshold() * scale);
            sbyte[] rgb_v = new sbyte[size_v * size_v * 3];
            float[] color = new float[3];
            for (int py = 0; py < size_v; py++)
            {
                int y = py * grid / size_v;
                for (int px = 0; px < size_v; px++)
                {
                    int x = px * grid / size_v;
                    float h = heights[y][x];
                    if (h <= sea)
                    {
                        float depth = ((float)(java.lang.Math.sqrt(java.lang.Math.clamp((float)(((float)(sea - h)) / sea), 0.0f, 1.0f))));
                        com.oddlabs.tt.mapeditor.MapPreview.mix((viking ? com.oddlabs.tt.mapeditor.MapPreview.VIKING_SHALLOW : com.oddlabs.tt.mapeditor.MapPreview.NATIVE_SHALLOW), (viking ? com.oddlabs.tt.mapeditor.MapPreview.VIKING_DEEP : com.oddlabs.tt.mapeditor.MapPreview.NATIVE_DEEP), depth, color);
                    }
                    else
                    {
                        com.oddlabs.tt.mapeditor.MapPreview.landColor(heights, x, y, h, sea, scale, threshold, viking, color);
                    }
                    com.oddlabs.tt.mapeditor.MapPreview.put(rgb_v, size_v, px, py, color);
                }
            }
            if (resources != null)
            {
                com.oddlabs.tt.mapeditor.MapPreview.mark(rgb_v, size_v, grid, resources.of(com.oddlabs.tt.mapeditor.Resource.TREE), (viking ? com.oddlabs.tt.mapeditor.MapPreview.OAK : com.oddlabs.tt.mapeditor.MapPreview.JUNGLE), 1);
                com.oddlabs.tt.mapeditor.MapPreview.mark(rgb_v, size_v, grid, resources.of(com.oddlabs.tt.mapeditor.Resource.PALM), (viking ? com.oddlabs.tt.mapeditor.MapPreview.PINE : com.oddlabs.tt.mapeditor.MapPreview.PALM), 1);
                com.oddlabs.tt.mapeditor.MapPreview.mark(rgb_v, size_v, grid, resources.of(com.oddlabs.tt.mapeditor.Resource.ROCK), com.oddlabs.tt.mapeditor.MapPreview.ROCK, 2);
                com.oddlabs.tt.mapeditor.MapPreview.mark(rgb_v, size_v, grid, resources.of(com.oddlabs.tt.mapeditor.Resource.IRON), com.oddlabs.tt.mapeditor.MapPreview.IRON, 2);
            }
            return new com.oddlabs.tt.mapeditor.MapPreview(size_v, rgb_v);
        }
        // Java: 92
        public static void landColor(float[][] heights, int x, int y, float h, float sea, float scale, float threshold, bool viking, float[] color)
        {
            int last = heights.Length - 1;
            float west = heights[y][java.lang.Math.max(x - 1, 0)];
            float east = heights[y][java.lang.Math.min(x + 1, last)];
            float south = heights[java.lang.Math.max(y - 1, 0)][x];
            float north = heights[java.lang.Math.min(y + 1, last)][x];
            float rise = (float)(((float)(h - sea)) / ((float)(scale - sea)));
            if (rise < 0.03f)
            {
                java.lang.JSystem.arraycopy((viking ? (object)(com.oddlabs.tt.mapeditor.MapPreview.VIKING_BEACH) : (object)(com.oddlabs.tt.mapeditor.MapPreview.NATIVE_BEACH)), 0, color, 0, 3);
            }
            else
            {
                com.oddlabs.tt.mapeditor.MapPreview.mix((viking ? com.oddlabs.tt.mapeditor.MapPreview.VIKING_LOW : com.oddlabs.tt.mapeditor.MapPreview.NATIVE_LOW), (viking ? com.oddlabs.tt.mapeditor.MapPreview.VIKING_HIGH : com.oddlabs.tt.mapeditor.MapPreview.NATIVE_HIGH), java.lang.Math.clamp(rise, 0.0f, 1.0f), color);
            }
            if (viking)
            {
                com.oddlabs.tt.mapeditor.MapPreview.mix(color, com.oddlabs.tt.mapeditor.MapPreview.SNOW, java.lang.Math.clamp((float)(((float)((float)(h / scale) - 0.5f)) / 0.1f), 0.0f, 1.0f), color);
            }
            float slope = java.lang.Math.max(java.lang.Math.max(java.lang.Math.abs((float)(h - west)), java.lang.Math.abs((float)(h - east))), java.lang.Math.max(java.lang.Math.abs((float)(h - south)), java.lang.Math.abs((float)(h - north))));
            com.oddlabs.tt.mapeditor.MapPreview.mix(color, (viking ? com.oddlabs.tt.mapeditor.MapPreview.VIKING_STEEP : com.oddlabs.tt.mapeditor.MapPreview.NATIVE_STEEP), java.lang.Math.clamp((float)(((float)(slope - (float)(0.75f * threshold))) / ((float)(0.5f * threshold))), 0.0f, 1.0f), color);
            float facing = (float)(((float)(((float)(east - west)) - ((float)(north - south)))) / ((float)(4.0f * com.oddlabs.tt.landscape.HeightMap.METERS_PER_UNIT_GRID)));
            float light = java.lang.Math.clamp((float)(1.0f + facing), 0.55f, 1.35f);
            for (int i = 0; i < 3; i++)
            {
                color[i] = java.lang.Math.min(1.0f, (float)(color[i] * light));
            }
        }
        // Java: 120
        public static void mark(sbyte[] rgb_v, int size_v, int grid, java.util.List positions, float[] color, int dot)
        {
            foreach (int[] position in java.lang.JIter.of(positions))
            {
                int px = java.lang.Math.min(position[0] * size_v / grid, size_v - dot);
                int py = java.lang.Math.min(position[1] * size_v / grid, size_v - dot);
                for (int dy = 0; dy < dot; dy++)
                {
                    for (int dx = 0; dx < dot; dx++)
                    {
                        com.oddlabs.tt.mapeditor.MapPreview.put(rgb_v, size_v, px + dx, py + dy, color);
                    }
                }
            }
        }
        // Java: 131
        public static void mix(float[] a, float[] b, float t, float[] @out)
        {
            for (int i = 0; i < 3; i++)
            {
                @out[i] = (float)(a[i] + (float)(((float)(b[i] - a[i])) * t));
            }
        }
        // Java: 136
        public static void put(sbyte[] rgb_v, int size_v, int px, int py, float[] color)
        {
            int i = (py * size_v + px) * 3;
            for (int c = 0; c < 3; c++)
            {
                rgb_v[i + c] = unchecked((sbyte)(java.lang.Math.round((float)(java.lang.Math.clamp(color[c], 0.0f, 1.0f) * 255.0f))));
            }
        }
        static MapPreview()
        {
            MapPreview.NATIVE_SHALLOW = new float[] { 0.3f, 0.62f, 0.72f };
            MapPreview.NATIVE_DEEP = new float[] { 0.06f, 0.2f, 0.4f };
            MapPreview.NATIVE_BEACH = new float[] { 0.86f, 0.8f, 0.58f };
            MapPreview.NATIVE_LOW = new float[] { 0.42f, 0.62f, 0.28f };
            MapPreview.NATIVE_HIGH = new float[] { 0.55f, 0.5f, 0.4f };
            MapPreview.NATIVE_STEEP = new float[] { 0.52f, 0.47f, 0.42f };
            MapPreview.VIKING_SHALLOW = new float[] { 0.25f, 0.45f, 0.55f };
            MapPreview.VIKING_DEEP = new float[] { 0.05f, 0.14f, 0.28f };
            MapPreview.VIKING_BEACH = new float[] { 0.62f, 0.6f, 0.55f };
            MapPreview.VIKING_LOW = new float[] { 0.36f, 0.52f, 0.34f };
            MapPreview.VIKING_HIGH = new float[] { 0.33f, 0.45f, 0.32f };
            MapPreview.VIKING_STEEP = new float[] { 0.45f, 0.45f, 0.47f };
            MapPreview.SNOW = new float[] { 0.93f, 0.94f, 0.96f };
            MapPreview.JUNGLE = new float[] { 0.1f, 0.32f, 0.1f };
            MapPreview.PALM = new float[] { 0.3f, 0.5f, 0.12f };
            MapPreview.OAK = new float[] { 0.15f, 0.3f, 0.12f };
            MapPreview.PINE = new float[] { 0.06f, 0.22f, 0.12f };
            MapPreview.ROCK = new float[] { 0.82f, 0.82f, 0.8f };
            MapPreview.IRON = new float[] { 0.75f, 0.38f, 0.22f };
        }
    }
}

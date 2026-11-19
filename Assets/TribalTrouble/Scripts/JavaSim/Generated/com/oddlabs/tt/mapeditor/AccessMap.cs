// Translated from AccessMap.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 16
    public class AccessMap : java.lang.Object
    {
        // Java: 17
        public sealed class Kind : java.lang.Enum
        {
            public static Kind[] values() { return (Kind[])VALUES_.Clone(); }
            public static Kind valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Kind NONE = new Kind("NONE", 0);
            public static readonly Kind REGION = new Kind("REGION", 1);
            public static readonly Kind CUT_OFF = new Kind("CUT_OFF", 2);
            public static readonly Kind CLIFF = new Kind("CLIFF", 3);
            public static readonly Kind[] VALUES_ = new Kind[] { NONE, REGION, CUT_OFF, CLIFF };
            public Kind(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public float[][] heights;
        public int size;
        public float height_scale;
        public float access_threshold;
        public bool archipelago;
        public com.oddlabs.tt.mapeditor.AccessMap.Kind[] kinds;
        public com.oddlabs.tt.mapeditor.AccessMap.Kind[] previous;
        public bool[] accessible;
        public bool[] steep;
        public bool[] land;
        public float[] normalized;
        public int[] component;
        public int[] queue;
        public bool stale;
        // Java: 64
        public int getSize()
        {
            return size;
        }
        // Java: 69
        public com.oddlabs.tt.mapeditor.AccessMap.Kind get(int x, int y)
        {
            return kinds[y * size + x];
        }
        // Java: 74
        public bool leftRegion(int x, int y)
        {
            int i = y * size + x;
            return ((object)previous[i] == (object)com.oddlabs.tt.mapeditor.AccessMap.Kind.REGION) && ((object)kinds[i] != (object)com.oddlabs.tt.mapeditor.AccessMap.Kind.REGION);
        }
        // Java: 80
        public void heightsChanged()
        {
            stale = true;
        }
        // Java: 84
        public bool isStale()
        {
            return stale;
        }
        // Java: 89
        public void compute()
        {
            stale = false;
            java.lang.JSystem.arraycopy(kinds, 0, previous, 0, kinds.Length);
            for (int y = 0; y < size; y++)
            {
                float[] row = heights[y];
                for (int x = 0; x < size; x++)
                {
                    normalized[y * size + x] = (float)(row[x] / height_scale);
                }
            }
            for (int y_2 = 0; y_2 < size; y_2++)
            {
                int up = ((y_2 == 0 ? size - 1 : y_2 - 1)) * size;
                int down = ((y_2 == size - 1 ? 0 : y_2 + 1)) * size;
                for (int x_2 = 0; x_2 < size; x_2++)
                {
                    int i = y_2 * size + x_2;
                    int left = (x_2 == 0 ? i + size - 1 : i - 1);
                    int right = (x_2 == size - 1 ? i - size + 1 : i + 1);
                    float h = normalized[i];
                    float slope = java.lang.Math.max(java.lang.Math.max(java.lang.Math.abs((float)(h - normalized[left])), java.lang.Math.abs((float)(h - normalized[right]))), java.lang.Math.max(java.lang.Math.abs((float)(h - normalized[up + x_2])), java.lang.Math.abs((float)(h - normalized[down + x_2]))));
                    bool walkable = slope >= 0.0f && slope <= access_threshold;
                    bool below_sea = h >= 0.0f && h <= com.oddlabs.tt.global.Globals.SEA_LEVEL;
                    bool edge = x_2 == 0 || y_2 == 0 || x_2 == size - 1 || y_2 == size - 1;
                    accessible[i] = walkable && !below_sea && !edge;
                    land[i] = !below_sea;
                    steep[i] = !walkable;
                }
            }
            int region = (archipelago ? (-1) : largestComponent());
            for (int i_2 = 0; i_2 < kinds.Length; i_2++)
            {
                if (accessible[i_2])
                {
                    kinds[i_2] = (archipelago || component[i_2] == region ? com.oddlabs.tt.mapeditor.AccessMap.Kind.REGION : com.oddlabs.tt.mapeditor.AccessMap.Kind.CUT_OFF);
                }
                else if (land[i_2] && steep[i_2])
                {
                    kinds[i_2] = com.oddlabs.tt.mapeditor.AccessMap.Kind.CLIFF;
                }
                else
                {
                    kinds[i_2] = com.oddlabs.tt.mapeditor.AccessMap.Kind.NONE;
                }
            }
        }
        // Java: 137
        public int largestComponent()
        {
            java.util.Arrays.fill(component, (-1));
            int largest = (-1);
            int largest_count = 0;
            int label = 0;
            for (int start = 0; start < accessible.Length; start++)
            {
                if (!accessible[start] || component[start] != (-1))
                {
                    continue;
                }
                int head = 0;
                int tail = 0;
                queue[tail++] = start;
                component[start] = label;
                while (head < tail)
                {
                    int i = queue[head++];
                    int x = i % size;
                    int y = i / size;
                    if (x > 0)
                    {
                        tail = visit(i - 1, label, tail);
                    }
                    if (x < size - 1)
                    {
                        tail = visit(i + 1, label, tail);
                    }
                    if (y > 0)
                    {
                        tail = visit(i - size, label, tail);
                    }
                    if (y < size - 1)
                    {
                        tail = visit(i + size, label, tail);
                    }
                }
                if (tail > largest_count)
                {
                    largest_count = tail;
                    largest = label;
                }
                label++;
            }
            return largest;
        }
        // Java: 171
        public int visit(int i, int label, int tail)
        {
            if (accessible[i] && component[i] == (-1))
            {
                component[i] = label;
                queue[tail++] = i;
            }
            return tail;
        }
        public AccessMap(float[][] heights, com.oddlabs.tt.mapeditor.MapSettings settings) : base()
        {
            this.heights = heights;
            this.size = heights.Length;
            this.height_scale = settings.getHeightScale();
            this.access_threshold = settings.getAccessThreshold();
            this.archipelago = settings.isArchipelago();
            this.kinds = new com.oddlabs.tt.mapeditor.AccessMap.Kind[size * size];
            this.previous = new com.oddlabs.tt.mapeditor.AccessMap.Kind[size * size];
            this.accessible = new bool[size * size];
            this.steep = new bool[size * size];
            this.land = new bool[size * size];
            this.normalized = new float[size * size];
            this.component = new int[size * size];
            this.queue = new int[size * size];
            compute();
            java.lang.JSystem.arraycopy(kinds, 0, previous, 0, kinds.Length);
        }
    }
}

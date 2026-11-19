// Translated from DistanceField.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 12
    public class DistanceField : java.lang.Object
    {
        public const int UNREACHABLE = 2147483647;
        public const char FAR = '\uffff';
        public int size;
        public int source_x;
        public int source_y;
        public int x0;
        public int y0;
        public int w;
        public int h;
        public int[] cost;
        public char[] compact_f;
        // Java: 45
        public int getSize()
        {
            return size;
        }
        // Java: 49
        public int getSourceX()
        {
            return source_x;
        }
        // Java: 53
        public int getSourceY()
        {
            return source_y;
        }
        // Java: 58
        public int[] raw()
        {
            return ((int[])java.util.Objects.requireNonNull(cost));
        }
        // Java: 62
        public int windowX()
        {
            return x0;
        }
        // Java: 66
        public int windowY()
        {
            return y0;
        }
        // Java: 70
        public int windowWidth()
        {
            return w;
        }
        // Java: 74
        public int windowHeight()
        {
            return h;
        }
        // Java: 82
        public void compact()
        {
            int[] c = cost;
            if (c == null)
            {
                return;
            }
            foreach (int v in c)
            {
                if (v != com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE && (v < 0 || v >= com.oddlabs.tt.player.ai.DistanceField.FAR))
                {
                    return;
                }
            }
            char[] small = new char[c.Length];
            for (int i = 0; i < c.Length; i++)
            {
                small[i] = (c[i] == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE ? com.oddlabs.tt.player.ai.DistanceField.FAR : unchecked((char)(c[i])));
            }
            compact_f = small;
            cost = null;
        }
        // Java: 96
        public int get(int x, int y)
        {
            if (x < 0 || y < 0 || x >= size || y >= size)
            {
                return com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
            }
            int lx = x - x0;
            int ly = y - y0;
            if (lx < 0 || ly < 0 || lx >= w || ly >= h)
            {
                return com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
            }
            char[] small = compact_f;
            if (small != null)
            {
                char v = small[ly * w + lx];
                return (v == com.oddlabs.tt.player.ai.DistanceField.FAR ? com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE : v);
            }
            return ((int[])java.util.Objects.requireNonNull(cost))[ly * w + lx];
        }
        // Java: 111
        public bool reachable(int x, int y)
        {
            return get(x, y) != com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
        }
        // Java: 118
        public int getAround(int x, int y, int radius)
        {
            int best = get(x, y);
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }
                    int c = get(x + dx, y + dy);
                    if (c < best)
                    {
                        best = c;
                    }
                }
            }
            return best;
        }
        // Java: 136
        public int[] stepTowardsSource(int x, int y, int max_meters)
        {
            int cx = x;
            int cy = y;
            int start = get(cx, cy);
            if (start == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
            {
                int[] near = nearestReachable(cx, cy, 12);
                if (near == null)
                {
                    return new int[] { x, y };
                }
                cx = near[0];
                cy = near[1];
                start = get(cx, cy);
            }
            int target = java.lang.Math.max(0, start - max_meters);
            while (get(cx, cy) > target)
            {
                int best = get(cx, cy);
                int bx = cx;
                int by = cy;
                for (int dy = (-1); dy <= 1; dy++)
                {
                    for (int dx = (-1); dx <= 1; dx++)
                    {
                        int c = get(cx + dx, cy + dy);
                        if (c < best)
                        {
                            best = c;
                            bx = cx + dx;
                            by = cy + dy;
                        }
                    }
                }
                if (bx == cx && by == cy)
                {
                    break;
                }
                cx = bx;
                cy = by;
            }
            return new int[] { cx, cy };
        }
        // Java: 171
        public int[] nearestReachable(int x, int y, int max_radius)
        {
            if (reachable(x, y))
            {
                return new int[] { x, y };
            }
            for (int r = 1; r <= max_radius; r++)
            {
                int best = com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
                int[] result = null;
                for (int dy = -r; dy <= r; dy++)
                {
                    for (int dx = -r; dx <= r; dx++)
                    {
                        if (java.lang.Math.abs(dx) != r && java.lang.Math.abs(dy) != r)
                        {
                            continue;
                        }
                        int c = get(x + dx, y + dy);
                        if (c < best)
                        {
                            best = c;
                            result = new int[] { x + dx, y + dy };
                        }
                    }
                }
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        public DistanceField(int size, int source_x, int source_y) : this(size, source_x, source_y, 0, 0, size, size)
        {
        }
        public DistanceField(int size, int source_x, int source_y, int x0, int y0, int w, int h) : base()
        {
            this.size = size;
            this.source_x = source_x;
            this.source_y = source_y;
            this.x0 = x0;
            this.y0 = y0;
            this.w = w;
            this.h = h;
            this.cost = new int[w * h];
            java.util.Arrays.fill(cost, com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE);
        }
    }
}

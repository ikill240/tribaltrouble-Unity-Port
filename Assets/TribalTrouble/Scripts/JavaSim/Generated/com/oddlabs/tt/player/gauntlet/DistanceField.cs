// Translated from DistanceField.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 12
    public class DistanceField : java.lang.Object
    {
        public const int UNREACHABLE = 2147483647;
        public int size;
        public int[] cost;
        public int source_x;
        public int source_y;
        // Java: 28
        public int getSize()
        {
            return size;
        }
        // Java: 32
        public int getSourceX()
        {
            return source_x;
        }
        // Java: 36
        public int getSourceY()
        {
            return source_y;
        }
        // Java: 40
        public int[] raw()
        {
            return cost;
        }
        // Java: 44
        public int get(int x, int y)
        {
            if (x < 0 || y < 0 || x >= size || y >= size)
            {
                return com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
            }
            return cost[y * size + x];
        }
        // Java: 50
        public bool reachable(int x, int y)
        {
            return get(x, y) != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
        }
        // Java: 57
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
        // Java: 75
        public int[] stepTowardsSource(int x, int y, int max_meters)
        {
            int cx = x;
            int cy = y;
            int start = get(cx, cy);
            if (start == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
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
        // Java: 110
        public int[] nearestReachable(int x, int y, int max_radius)
        {
            if (reachable(x, y))
            {
                return new int[] { x, y };
            }
            for (int r = 1; r <= max_radius; r++)
            {
                int best = com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
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
        public DistanceField(int size, int source_x, int source_y) : base()
        {
            this.size = size;
            this.source_x = source_x;
            this.source_y = source_y;
            this.cost = new int[size * size];
            java.util.Arrays.fill(cost, com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE);
        }
    }
}

// Translated from PathSmoother.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.pathfinder
{
    // Java: 14
    public class PathSmoother : java.lang.Object
    {
        public const int MAX_LOOKAHEAD = 40;
        public const int MAX_CELLS = 512;
        public static int[] XS;
        public static int[] YS;
        public static int[] SDX;
        public static int[] SDY;
        // Java: 23
        public static com.oddlabs.tt.pathfinder.GridPathNode smooth(com.oddlabs.tt.pathfinder.UnitGrid grid, int src_x, int src_y, com.oddlabs.tt.pathfinder.GridPathNode path)
        {
            if (path == null)
            {
                return null;
            }
            int[] xs = com.oddlabs.tt.pathfinder.PathSmoother.XS;
            int[] ys = com.oddlabs.tt.pathfinder.PathSmoother.YS;
            int n = 0;
            xs[0] = src_x;
            ys[0] = src_y;
            com.oddlabs.tt.pathfinder.GridPathNode node = path;
            while (node != null)
            {
                if (n >= com.oddlabs.tt.pathfinder.PathSmoother.MAX_CELLS)
                {
                    return path;
                }
                com.oddlabs.tt.pathfinder.DirectionNode d = node.getDirection();
                xs[n + 1] = xs[n] + d.getDirectionX();
                ys[n + 1] = ys[n] + d.getDirectionY();
                n++;
                node = ((com.oddlabs.tt.pathfinder.GridPathNode)node.getParent());
            }
            if (n < 3)
            {
                return path;
            }
            int[] sdx = com.oddlabs.tt.pathfinder.PathSmoother.SDX;
            int[] sdy = com.oddlabs.tt.pathfinder.PathSmoother.SDY;
            int steps = 0;
            int i = 0;
            while (i < n)
            {
                int best = i + 1;
                int max_j = java.lang.Math.min(n, i + com.oddlabs.tt.pathfinder.PathSmoother.MAX_LOOKAHEAD);
                for (int j = max_j; j > i + 1; j--)
                {
                    if (com.oddlabs.tt.pathfinder.PathSmoother.lineClear(grid, xs[i], ys[i], xs[j], ys[j]))
                    {
                        best = j;
                        break;
                    }
                }
                if (best == i + 1)
                {
                    if (steps >= sdx.Length)
                    {
                        return path;
                    }
                    sdx[steps] = xs[i + 1] - xs[i];
                    sdy[steps] = ys[i + 1] - ys[i];
                    steps++;
                }
                else
                {
                    int added = com.oddlabs.tt.pathfinder.PathSmoother.lineSteps(xs[i], ys[i], xs[best], ys[best], sdx, sdy, steps);
                    if (added < 0)
                    {
                        return path;
                    }
                    steps += added;
                }
                i = best;
            }
            com.oddlabs.tt.pathfinder.GridPathNode result = null;
            for (int k = steps - 1; k >= 0; k--)
            {
                com.oddlabs.tt.pathfinder.DirectionNode dir = com.oddlabs.tt.pathfinder.GridNode.lookupDirectionNode(sdx[k], sdy[k]);
                if (dir == null)
                {
                    return path;
                }
                result = new com.oddlabs.tt.pathfinder.GridPathNode(result, dir);
            }
            return result;
        }
        // Java: 81
        public static bool passable(com.oddlabs.tt.pathfinder.UnitGrid grid, int x, int y)
        {
            int size = grid.getGridSize();
            if (x < 0 || y < 0 || x >= size || y >= size)
            {
                return false;
            }
            if (grid.getRegion(x, y) == null)
            {
                return false;
            }
            com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
            return occ == null || occ.getPenalty() < com.oddlabs.tt.pathfinder.Occupant.STATIC;
        }
        // Java: 92
        public static bool lineClear(com.oddlabs.tt.pathfinder.UnitGrid grid, int x0, int y0, int x1, int y1)
        {
            int dx = java.lang.Math.abs(x1 - x0);
            int dy = java.lang.Math.abs(y1 - y0);
            int sx = (x0 < x1 ? 1 : (-1));
            int sy = (y0 < y1 ? 1 : (-1));
            int err = dx - dy;
            int x = x0;
            int y = y0;
            while (x != x1 || y != y1)
            {
                int e2 = 2 * err;
                int nx = x;
                int ny = y;
                if (e2 > -dy)
                {
                    err -= dy;
                    nx += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    ny += sy;
                }
                if (nx != x && ny != y && (!com.oddlabs.tt.pathfinder.PathSmoother.passable(grid, nx, y) || !com.oddlabs.tt.pathfinder.PathSmoother.passable(grid, x, ny)))
                {
                    return false;
                }
                if (!com.oddlabs.tt.pathfinder.PathSmoother.passable(grid, nx, ny))
                {
                    return false;
                }
                x = nx;
                y = ny;
            }
            return true;
        }
        // Java: 118
        public static int lineSteps(int x0, int y0, int x1, int y1, int[] sdx, int[] sdy, int at)
        {
            int dx = java.lang.Math.abs(x1 - x0);
            int dy = java.lang.Math.abs(y1 - y0);
            int sx = (x0 < x1 ? 1 : (-1));
            int sy = (y0 < y1 ? 1 : (-1));
            int err = dx - dy;
            int x = x0;
            int y = y0;
            int count = 0;
            while (x != x1 || y != y1)
            {
                int e2 = 2 * err;
                int nx = x;
                int ny = y;
                if (e2 > -dy)
                {
                    err -= dy;
                    nx += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    ny += sy;
                }
                if (at + count >= sdx.Length)
                {
                    return (-1);
                }
                sdx[at + count] = nx - x;
                sdy[at + count] = ny - y;
                count++;
                x = nx;
                y = ny;
            }
            return count;
        }
        static PathSmoother()
        {
            PathSmoother.XS = new int[com.oddlabs.tt.pathfinder.PathSmoother.MAX_CELLS + 1];
            PathSmoother.YS = new int[com.oddlabs.tt.pathfinder.PathSmoother.MAX_CELLS + 1];
            PathSmoother.SDX = new int[com.oddlabs.tt.pathfinder.PathSmoother.MAX_CELLS * 2];
            PathSmoother.SDY = new int[com.oddlabs.tt.pathfinder.PathSmoother.MAX_CELLS * 2];
        }
        public PathSmoother() : base()
        {
        }
    }
}

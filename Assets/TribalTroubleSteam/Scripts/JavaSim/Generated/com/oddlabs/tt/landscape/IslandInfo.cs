// Translated from IslandInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 8
    public class IslandInfo : java.lang.Object
    {
        public int id_f;
        public int area_f;
        public int trees_f;
        public int rocks_f;
        public int iron_f;
        public int min_x;
        public int min_y;
        public int max_x;
        public int max_y;
        public int start_x;
        public int start_y;
        public java.util.List contour_points;
        // Java: 38
        public void generateContourPoints(sbyte[][] dock_grid, com.oddlabs.procedural.Channel island_ids)
        {
            for (int y = min_y; y <= max_y; y++)
            {
                for (int x = min_x; x <= max_x; x++)
                {
                    if (dock_grid[y][x] == 1 && java.lang.StrictMath.round(island_ids.getPixel(x, y)) == id_f)
                    {
                        contour_points.add(new int[] { x, y });
                    }
                }
            }
        }
        // Java: 48
        public virtual java.util.List contourPoints()
        {
            return contour_points;
        }
        // Java: 52
        public virtual void setTrees(int trees_v)
        {
            this.trees_f = trees_v;
        }
        // Java: 56
        public virtual void setIron(int iron_v)
        {
            this.iron_f = iron_v;
        }
        // Java: 60
        public virtual void setRocks(int rocks_v)
        {
            this.rocks_f = rocks_v;
        }
        // Java: 64
        public virtual int rocks()
        {
            return rocks_f;
        }
        // Java: 68
        public virtual int iron()
        {
            return iron_f;
        }
        // Java: 72
        public virtual int trees()
        {
            return trees_f;
        }
        // Java: 76
        public virtual int minX()
        {
            return min_x;
        }
        // Java: 80
        public virtual int minY()
        {
            return min_y;
        }
        // Java: 84
        public virtual int maxX()
        {
            return max_x;
        }
        // Java: 88
        public virtual int maxY()
        {
            return max_y;
        }
        // Java: 92
        public virtual int centerX()
        {
            return (min_x + max_x) / 2;
        }
        // Java: 96
        public virtual int centerY()
        {
            return (min_y + max_y) / 2;
        }
        // Java: 100
        public virtual int startX()
        {
            return start_x;
        }
        // Java: 104
        public virtual int startY()
        {
            return start_y;
        }
        // Java: 108
        public virtual int area()
        {
            return area_f;
        }
        // Java: 112
        public virtual int id()
        {
            return id_f;
        }
        private void init_IslandInfo()
        {
            this.contour_points = new java.util.ArrayList();
        }
        public IslandInfo(int id_v, int area_v, int min_x, int min_y, int max_x, int max_y, int start_x, int start_y, sbyte[][] dock_grid, com.oddlabs.procedural.Channel island_ids) : base()
        {
            init_IslandInfo();
            this.id_f = id_v;
            this.area_f = area_v;
            this.min_x = min_x;
            this.min_y = min_y;
            this.max_x = max_x;
            this.max_y = max_y;
            this.start_x = start_x;
            this.start_y = start_y;
            this.rocks_f = 0;
            this.iron_f = 0;
            this.trees_f = 0;
            generateContourPoints(dock_grid, island_ids);
        }
    }
}

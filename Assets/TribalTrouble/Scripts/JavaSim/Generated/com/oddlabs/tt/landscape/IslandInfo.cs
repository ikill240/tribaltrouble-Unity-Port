// Translated from IslandInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 3
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
        // Java: 30
        public virtual void setTrees(int trees_v)
        {
            this.trees_f = trees_v;
        }
        // Java: 34
        public virtual void setIron(int iron_v)
        {
            this.iron_f = iron_v;
        }
        // Java: 38
        public virtual void setRocks(int rocks_v)
        {
            this.rocks_f = rocks_v;
        }
        // Java: 42
        public virtual int rocks()
        {
            return rocks_f;
        }
        // Java: 46
        public virtual int iron()
        {
            return iron_f;
        }
        // Java: 50
        public virtual int trees()
        {
            return trees_f;
        }
        // Java: 54
        public virtual int minX()
        {
            return min_x;
        }
        // Java: 58
        public virtual int minY()
        {
            return min_y;
        }
        // Java: 62
        public virtual int maxX()
        {
            return max_x;
        }
        // Java: 66
        public virtual int maxY()
        {
            return max_y;
        }
        // Java: 70
        public virtual int centerX()
        {
            return (min_x + max_x) / 2;
        }
        // Java: 74
        public virtual int centerY()
        {
            return (min_y + max_y) / 2;
        }
        // Java: 78
        public virtual int startX()
        {
            return start_x;
        }
        // Java: 82
        public virtual int startY()
        {
            return start_y;
        }
        // Java: 86
        public virtual int area()
        {
            return area_f;
        }
        // Java: 90
        public virtual int id()
        {
            return id_f;
        }
        public IslandInfo(int id_v, int area_v, int min_x, int min_y, int max_x, int max_y, int start_x, int start_y) : base()
        {
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
        }
    }
}

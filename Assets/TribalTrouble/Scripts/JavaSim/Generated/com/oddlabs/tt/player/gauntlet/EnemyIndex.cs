// Translated from EnemyIndex.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 16
    public class EnemyIndex : java.lang.Object
    {
        public const sbyte WARRIOR = unchecked((sbyte)0);
        public const sbyte CHIEFTAIN = unchecked((sbyte)1);
        public const sbyte PEON = unchecked((sbyte)2);
        public const int SHIFT = 4;
        public int side;
        public com.oddlabs.tt.model.Unit[] units;
        public int size;
        public int[] xs;
        public int[] ys;
        public sbyte[] groups;
        public int[] bucket_of;
        public int[] slot;
        public int[][] buckets;
        public int[] bucket_sizes;
        public int[] result;
        // Java: 43
        public void rebuild(java.util.List warriors, java.util.List chieftains, java.util.List peons)
        {
            java.util.Arrays.fill(units, 0, size, null);
            size = 0;
            java.util.Arrays.fill(bucket_sizes, 0);
            add(warriors, com.oddlabs.tt.player.gauntlet.EnemyIndex.WARRIOR);
            add(chieftains, com.oddlabs.tt.player.gauntlet.EnemyIndex.CHIEFTAIN);
            add(peons, com.oddlabs.tt.player.gauntlet.EnemyIndex.PEON);
        }
        // Java: 58
        public void refresh()
        {
            for (int i = 0; i < size; i++)
            {
                com.oddlabs.tt.model.Unit u = units[i];
                int x = u.getGridX();
                int y = u.getGridY();
                if (x == xs[i] && y == ys[i])
                {
                    continue;
                }
                xs[i] = x;
                ys[i] = y;
                int b = bucket(x, y);
                int old = bucket_of[i];
                if (b == old)
                {
                    continue;
                }
                int[] list = buckets[old];
                int last = list[--bucket_sizes[old]];
                list[slot[i]] = last;
                slot[last] = slot[i];
                append(b, i);
            }
        }
        // Java: 79
        public void add(java.util.List group_v, sbyte kind)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group_v))
            {
                int i = size;
                if (i == xs.Length)
                {
                    units = ((com.oddlabs.tt.model.Unit[])java.util.Arrays.copyOf(units, i * 2));
                    xs = java.util.Arrays.copyOf(xs, i * 2);
                    ys = java.util.Arrays.copyOf(ys, i * 2);
                    groups = java.util.Arrays.copyOf(groups, i * 2);
                    bucket_of = java.util.Arrays.copyOf(bucket_of, i * 2);
                    slot = java.util.Arrays.copyOf(slot, i * 2);
                }
                units[i] = u;
                size++;
                int x = u.getGridX();
                int y = u.getGridY();
                xs[i] = x;
                ys[i] = y;
                groups[i] = kind;
                append(bucket(x, y), i);
            }
        }
        // Java: 101
        public void append(int b, int i)
        {
            int[] list = buckets[b];
            if (list == null)
            {
                list = new int[8];
                buckets[b] = list;
            }
            else if (bucket_sizes[b] == list.Length)
            {
                list = java.util.Arrays.copyOf(list, list.Length * 2);
                buckets[b] = list;
            }
            bucket_of[i] = b;
            slot[i] = bucket_sizes[b];
            list[bucket_sizes[b]++] = i;
        }
        // Java: 115
        public int bucket(int x, int y)
        {
            int bx = java.lang.Math.clamp(x >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int by = java.lang.Math.clamp(y >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            return by * side + bx;
        }
        // Java: 125
        public int[] query(int x, int y, int r2)
        {
            int r = java.lang.JC.d2i(java.lang.Math.ceil(java.lang.Math.sqrt(r2)));
            return collect(x, y, r, r2, true);
        }
        // Java: 131
        public int[] queryUnordered(int x, int y, int r2)
        {
            int r = java.lang.JC.d2i(java.lang.Math.ceil(java.lang.Math.sqrt(r2)));
            return collect(x, y, r, r2, false);
        }
        // Java: 141
        public int groupsInBox(int x, int y, int c)
        {
            int bx0 = java.lang.Math.clamp((x - c) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int bx1 = java.lang.Math.clamp((x + c) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int by0 = java.lang.Math.clamp((y - c) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int by1 = java.lang.Math.clamp((y + c) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int mask = 0;
            for (int by = by0; by <= by1; by++)
            {
                for (int bx = bx0; bx <= bx1; bx++)
                {
                    int b = by * side + bx;
                    int[] list = buckets[b];
                    for (int k = 0; k < bucket_sizes[b]; k++)
                    {
                        int i = list[k];
                        if (java.lang.Math.abs(xs[i] - x) > c || java.lang.Math.abs(ys[i] - y) > c)
                        {
                            continue;
                        }
                        if (groups[i] == com.oddlabs.tt.player.gauntlet.EnemyIndex.WARRIOR)
                        {
                            return mask | 1 << com.oddlabs.tt.player.gauntlet.EnemyIndex.WARRIOR;
                        }
                        mask |= 1 << groups[i];
                    }
                }
            }
            return mask;
        }
        // Java: 163
        public int[] collect(int x, int y, int r, int r2, bool ordered)
        {
            int bx0 = java.lang.Math.clamp((x - r) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int bx1 = java.lang.Math.clamp((x + r) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int by0 = java.lang.Math.clamp((y - r) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int by1 = java.lang.Math.clamp((y + r) >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT, 0, side - 1);
            int n = 0;
            for (int by = by0; by <= by1; by++)
            {
                for (int bx = bx0; bx <= bx1; bx++)
                {
                    int b = by * side + bx;
                    int[] list = buckets[b];
                    for (int k = 0; k < bucket_sizes[b]; k++)
                    {
                        int i = list[k];
                        int dx = xs[i] - x;
                        int dy = ys[i] - y;
                        if (dx * dx + dy * dy > r2)
                        {
                            continue;
                        }
                        if (n == result.Length)
                        {
                            result = java.util.Arrays.copyOf(result, n * 2);
                        }
                        result[n++] = i;
                    }
                }
            }
            if (ordered)
            {
                java.util.Arrays.sort(result, 0, n);
            }
            count_f = n;
            return result;
        }
        public int count_f;
        // Java: 192
        public int count()
        {
            return count_f;
        }
        // Java: 196
        public com.oddlabs.tt.model.Unit unit(int i)
        {
            return units[i];
        }
        // Java: 202
        public sbyte group(int i)
        {
            return groups[i];
        }
        // Java: 206
        public bool isPeon(int i)
        {
            return groups[i] == com.oddlabs.tt.player.gauntlet.EnemyIndex.PEON;
        }
        private void init_EnemyIndex()
        {
            this.units = new com.oddlabs.tt.model.Unit[256];
            this.xs = new int[256];
            this.ys = new int[256];
            this.groups = new sbyte[256];
            this.bucket_of = new int[256];
            this.slot = new int[256];
            this.result = new int[64];
        }
        public EnemyIndex(int map_size) : base()
        {
            init_EnemyIndex();
            side = (map_size >> com.oddlabs.tt.player.gauntlet.EnemyIndex.SHIFT) + 1;
            buckets = new int[side * side][];
            bucket_sizes = new int[side * side];
        }
    }
}

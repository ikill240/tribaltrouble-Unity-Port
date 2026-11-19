// Translated from SiteFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 15
    public class SiteFinder : java.lang.Object
    {
        // Java: 16
        public interface Scorer
        {
            // Java: 18
            float score(int x, int y);
        }
        public sealed class Scorer_L : java.lang.Object, Scorer
        {
            readonly System.Func<int, int, float> f;
            public Scorer_L(System.Func<int, int, float> f) { this.f = f; }
            public float score(int p0, int p1) { return f(p0, p1); }
        }
        // Java: 22
        public class Reservation : java.lang.Object
        {
            public int x() { return x_f; }
            public int y() { return y_f; }
            public int size() { return size_f; }
            public Reservation(int x, int y, int size)
            {
                this.x_f = x;
                this.y_f = y;
                this.size_f = size;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Reservation r)) return false; return true && java.lang.JO.equals(x_f, r.x_f) && java.lang.JO.equals(y_f, r.y_f) && java.lang.JO.equals(size_f, r.size_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(x_f); h = 31 * h + java.lang.JO.hashCode(y_f); h = 31 * h + java.lang.JO.hashCode(size_f); } return h; }
            public override string toString() { return "Reservation[" + "x=" + java.lang.JO.toString(x_f) + ", y=" + java.lang.JO.toString(y_f) + ", size=" + java.lang.JO.toString(size_f) + "]"; }
            public int x_f;
            public int y_f;
            public int size_f;
        }
        public com.oddlabs.tt.player.ultra.MapAnalysis map;
        public java.util.List reservations;
        // Java: 32
        public void clearReservations()
        {
            reservations.clear();
        }
        // Java: 36
        public void reserve(int x, int y, int size)
        {
            reservations.add(new com.oddlabs.tt.player.ultra.SiteFinder.Reservation(x, y, size));
        }
        // Java: 40
        public void unreserve(int x, int y)
        {
            reservations.removeIf(new java.util.function.Predicate_L((object r_l) =>
            {
                com.oddlabs.tt.player.ultra.SiteFinder.Reservation r = (com.oddlabs.tt.player.ultra.SiteFinder.Reservation)r_l;
                return r.x() == x && r.y() == y;
            }));
        }
        // Java: 44
        public bool isReserved(int x, int y, int size)
        {
            foreach (com.oddlabs.tt.player.ultra.SiteFinder.Reservation r in java.lang.JIter.of(reservations))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(r.x(), r.y(), x, y) < r.size() + size - 1)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 53
        public bool isLegal(com.oddlabs.tt.model.BuildingTemplate template, int x, int y)
        {
            int size = template.getPlacingSize();
            if (!map.inside(x, y) || !map.heightmap.canBuild(x, y, size))
            {
                return false;
            }
            if (isReserved(x, y, size))
            {
                return false;
            }
            return com.oddlabs.tt.model.LandBuilding.isPlacingLegal(map.grid, template, x, y);
        }
        // Java: 66
        public int[] bestCoarse(com.oddlabs.tt.model.BuildingTemplate template, int cx, int cy, int radius, int stride, com.oddlabs.tt.player.ultra.SiteFinder.Scorer scorer)
        {
            return bestCoarse(template, cx, cy, radius, stride, scorer, scorer);
        }
        // Java: 75
        public int[] bestCoarse(com.oddlabs.tt.model.BuildingTemplate template, int cx, int cy, int radius, int stride, com.oddlabs.tt.player.ultra.SiteFinder.Scorer scorer, com.oddlabs.tt.player.ultra.SiteFinder.Scorer refine)
        {
            int size = template.getPlacingSize();
            int keep = 12;
            float[] scores = new float[keep];
            int[] xs = new int[keep];
            int[] ys = new int[keep];
            java.util.Arrays.fill(scores, java.lang.Float.NEGATIVE_INFINITY);
            int x0 = java.lang.Math.max(size, cx - radius);
            int y0 = java.lang.Math.max(size, cy - radius);
            int x1 = java.lang.Math.min(map.n - 1 - size, cx + radius);
            int y1 = java.lang.Math.min(map.n - 1 - size, cy + radius);
            for (int y = y0; y <= y1; y += stride)
            {
                for (int x = x0; x <= x1; x += stride)
                {
                    if (!map.heightmap.canBuild(x, y, size))
                    {
                        continue;
                    }
                    float s = scorer.score(x, y);
                    if (s == java.lang.Float.NEGATIVE_INFINITY || java.lang.Float.isNaN(s) || s <= scores[keep - 1])
                    {
                        continue;
                    }
                    int pos = keep - 1;
                    while (pos > 0 && scores[pos - 1] < s)
                    {
                        scores[pos] = scores[pos - 1];
                        xs[pos] = xs[pos - 1];
                        ys[pos] = ys[pos - 1];
                        pos--;
                    }
                    scores[pos] = s;
                    xs[pos] = x;
                    ys[pos] = y;
                }
            }
            int[] result = null;
            float result_score = java.lang.Float.NEGATIVE_INFINITY;
            for (int i = 0; i < keep; i++)
            {
                if (scores[i] == java.lang.Float.NEGATIVE_INFINITY)
                {
                    break;
                }
                int[] local = best(template, xs[i], ys[i], stride + 2, scorer);
                if (local == null)
                {
                    continue;
                }
                float s_2 = refine.score(local[0], local[1]);
                if (s_2 > result_score)
                {
                    result_score = s_2;
                    result = local;
                }
            }
            if (result == null)
            {
                result = best(template, cx, cy, radius, scorer);
            }
            return result;
        }
        // Java: 130
        public int[] best(com.oddlabs.tt.model.BuildingTemplate template, int cx, int cy, int radius, com.oddlabs.tt.player.ultra.SiteFinder.Scorer scorer)
        {
            int size = template.getPlacingSize();
            int x0 = java.lang.Math.max(size, cx - radius);
            int y0 = java.lang.Math.max(size, cy - radius);
            int x1 = java.lang.Math.min(map.n - 1 - size, cx + radius);
            int y1 = java.lang.Math.min(map.n - 1 - size, cy + radius);
            float best_score = java.lang.Float.NEGATIVE_INFINITY;
            int best_x = (-1);
            int best_y = (-1);
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    if (!map.heightmap.canBuild(x, y, size))
                    {
                        continue;
                    }
                    float s = scorer.score(x, y);
                    if (s == java.lang.Float.NEGATIVE_INFINITY || java.lang.Float.isNaN(s) || s <= best_score)
                    {
                        continue;
                    }
                    if (isReserved(x, y, size) || !com.oddlabs.tt.model.LandBuilding.isPlacingLegal(map.grid, template, x, y))
                    {
                        continue;
                    }
                    best_score = s;
                    best_x = x;
                    best_y = y;
                }
            }
            return (best_x < 0 ? (int[])(null) : new int[] { best_x, best_y });
        }
        // Java: 160
        public int[] bestRefined(com.oddlabs.tt.model.BuildingTemplate template, int cx, int cy, int radius, com.oddlabs.tt.player.ultra.SiteFinder.Scorer scorer, com.oddlabs.tt.player.ultra.SiteFinder.Scorer refine, int k)
        {
            int size = template.getPlacingSize();
            int x0 = java.lang.Math.max(size, cx - radius);
            int y0 = java.lang.Math.max(size, cy - radius);
            int x1 = java.lang.Math.min(map.n - 1 - size, cx + radius);
            int y1 = java.lang.Math.min(map.n - 1 - size, cy + radius);
            float[] scores = new float[k];
            int[] xs = new int[k];
            int[] ys = new int[k];
            java.util.Arrays.fill(scores, java.lang.Float.NEGATIVE_INFINITY);
            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    if (!map.heightmap.canBuild(x, y, size))
                    {
                        continue;
                    }
                    float s = scorer.score(x, y);
                    if (s == java.lang.Float.NEGATIVE_INFINITY || java.lang.Float.isNaN(s) || s <= scores[k - 1])
                    {
                        continue;
                    }
                    int near = (-1);
                    for (int i = 0; i < k && scores[i] != java.lang.Float.NEGATIVE_INFINITY; i++)
                    {
                        if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(xs[i], ys[i], x, y) < 3)
                        {
                            near = i;
                            break;
                        }
                    }
                    if (near >= 0 && scores[near] >= s)
                    {
                        continue;
                    }
                    if (isReserved(x, y, size) || !com.oddlabs.tt.model.LandBuilding.isPlacingLegal(map.grid, template, x, y))
                    {
                        continue;
                    }
                    int pos = (near >= 0 ? near : k - 1);
                    while (pos > 0 && scores[pos - 1] < s)
                    {
                        scores[pos] = scores[pos - 1];
                        xs[pos] = xs[pos - 1];
                        ys[pos] = ys[pos - 1];
                        pos--;
                    }
                    scores[pos] = s;
                    xs[pos] = x;
                    ys[pos] = y;
                }
            }
            int[] result = null;
            float result_score = java.lang.Float.NEGATIVE_INFINITY;
            for (int i_2 = 0; i_2 < k && scores[i_2] != java.lang.Float.NEGATIVE_INFINITY; i_2++)
            {
                float r = refine.score(xs[i_2], ys[i_2]);
                if (r > result_score || result == null)
                {
                    result_score = r;
                    result = new int[] { xs[i_2], ys[i_2] };
                }
            }
            return result;
        }
        private void init_SiteFinder()
        {
            this.reservations = new java.util.ArrayList();
        }
        public SiteFinder(com.oddlabs.tt.player.ultra.MapAnalysis map) : base()
        {
            init_SiteFinder();
            this.map = map;
        }
    }
}

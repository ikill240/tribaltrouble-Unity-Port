// Translated from SiteFinder.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 17
    public class SiteFinder : java.lang.Object
    {
        // Java: 19
        public interface Scorer
        {
            // Java: 21
            float score(int gx, int gy);
        }
        public sealed class Scorer_L : java.lang.Object, Scorer
        {
            readonly System.Func<int, int, float> f;
            public Scorer_L(System.Func<int, int, float> f) { this.f = f; }
            public float score(int p0, int p1) { return f(p0, p1); }
        }
        // Java: 24
        public class Site : java.lang.Object
        {
            public int gx() { return gx_f; }
            public int gy() { return gy_f; }
            public float score() { return score_f; }
            public Site(int gx, int gy, float score)
            {
                this.gx_f = gx;
                this.gy_f = gy;
                this.score_f = score;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Site r)) return false; return true && java.lang.JO.equals(gx_f, r.gx_f) && java.lang.JO.equals(gy_f, r.gy_f) && java.lang.JO.equals(score_f, r.score_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(gx_f); h = 31 * h + java.lang.JO.hashCode(gy_f); h = 31 * h + java.lang.JO.hashCode(score_f); } return h; }
            public override string toString() { return "Site[" + "gx=" + java.lang.JO.toString(gx_f) + ", gy=" + java.lang.JO.toString(gy_f) + ", score=" + java.lang.JO.toString(score_f) + "]"; }
            public int gx_f;
            public int gy_f;
            public float score_f;
        }
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.landscape.HeightMap map;
        // Java: 37
        public com.oddlabs.tt.model.BuildingTemplate template(int template_id)
        {
            return player.getRace().getBuildingTemplate(template_id);
        }
        // Java: 42
        public bool isLegal(int template_id, int gx, int gy)
        {
            if (gx < 0 || gy < 0 || gx >= grid.getGridSize() || gy >= grid.getGridSize())
            {
                return false;
            }
            com.oddlabs.tt.model.BuildingTemplate template_v = template(template_id);
            return map.canBuild(gx, gy, template_v.getPlacingSize()) && template_v.isPlacingLegal(grid, gx, gy) && grid.getRegion(gx, gy) != null;
        }
        // Java: 54
        public bool mayBuild(int template_id, int gx, int gy, int island)
        {
            if (gx < 0 || gy < 0 || gx >= grid.getGridSize() || gy >= grid.getGridSize())
            {
                return false;
            }
            return map.canBuild(gx, gy, template(template_id).getPlacingSize()) && map.getIslandId(gx, gy) == island;
        }
        // Java: 65
        public com.oddlabs.tt.player.fable.SiteFinder.Site best(int template_id, int cx, int cy, int radius, int island, com.oddlabs.tt.player.fable.SiteFinder.Scorer scorer)
        {
            com.oddlabs.tt.player.fable.SiteFinder.Site best_v = null;
            int step = (template_id == com.oddlabs.tt.model.Race.BUILDING_TOWER ? 1 : 2);
            for (int gy = cy - radius; gy <= cy + radius; gy += step)
            {
                for (int gx = cx - radius; gx <= cx + radius; gx += step)
                {
                    if (!mayBuild(template_id, gx, gy, island))
                    {
                        continue;
                    }
                    float s = scorer.score(gx, gy);
                    if (s == java.lang.Float.NEGATIVE_INFINITY || (best_v != null && s <= best_v.score()))
                    {
                        continue;
                    }
                    if (!isLegal(template_id, gx, gy))
                    {
                        continue;
                    }
                    best_v = new com.oddlabs.tt.player.fable.SiteFinder.Site(gx, gy, s);
                }
            }
            return best_v;
        }
        // Java: 84
        public float height(int gx, int gy)
        {
            return map.getWrappedHeight(gx, gy);
        }
        // Java: 88
        public com.oddlabs.tt.landscape.HeightMap getHeightMap()
        {
            return map;
        }
        // Java: 92
        public com.oddlabs.tt.pathfinder.UnitGrid getGrid()
        {
            return grid;
        }
        // Java: 97
        public static int halfFootprint(com.oddlabs.tt.model.BuildingTemplate template_v)
        {
            return template_v.getPlacingSize() - 2;
        }
        public SiteFinder(com.oddlabs.tt.player.Player player) : base()
        {
            this.player = player;
            this.grid = player.getWorld().getUnitGrid();
            this.map = player.getWorld().getHeightMap();
        }
    }
}

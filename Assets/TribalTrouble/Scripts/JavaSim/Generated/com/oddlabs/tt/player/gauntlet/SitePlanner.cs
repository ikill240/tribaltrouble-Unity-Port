// Translated from SitePlanner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 22
    public class SitePlanner : java.lang.Object
    {
        public const float ROUND_TRIP_SECONDS_PER_METER = 0.45f;
        public const int MISSING_DISTANCE = 140;
        public const int BUILDING_GAP = 3;
        public com.oddlabs.tt.player.gauntlet.MapAnalysis map;
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.player.gauntlet.Strategy strategy;
        public com.oddlabs.tt.player.gauntlet.DistanceField start_field;
        public com.oddlabs.tt.player.gauntlet.DistanceField enemy_field;
        public int start_x;
        public int start_y;
        public int enemy_x;
        public int enemy_y;
        public float threat_weight;
        // Java: 61
        public com.oddlabs.tt.model.BuildingTemplate template(int type)
        {
            return owner.getRace().getBuildingTemplate(type);
        }
        // Java: 66
        public float exposure(int x, int y)
        {
            int ours = start_field.get(x, y);
            int theirs = enemy_field.get(x, y);
            if (ours == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
            {
                return 1.0f;
            }
            if (theirs == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
            {
                return 0.0f;
            }
            return (float)(ours / ((float)(java.lang.Math.max(1, ours + theirs))));
        }
        // Java: 79
        public static float gatherSeconds(com.oddlabs.tt.player.gauntlet.DistanceField field, java.util.List supplies, int units, int per_node, int max_meters, float harvest_seconds)
        {
            int sx = field.getSourceX();
            int sy = field.getSourceY();
            int limit2 = (max_meters / 2) * (max_meters / 2);
            int[] dists = new int[supplies.size()];
            int n = 0;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, s.getGridX(), s.getGridY()) > limit2)
                {
                    continue;
                }
                int d = field.getAround(s.getGridX(), s.getGridY(), 1);
                if (d != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    dists[n++] = d;
                }
            }
            java.util.Arrays.sort(dists, 0, n);
            int remaining = units;
            float total = 0.0f;
            for (int i = 0; i < n && remaining > 0; i++)
            {
                int take = java.lang.Math.min(per_node, remaining);
                total = (float)(total + take * java.lang.Math.max(0, dists[i] - 5));
                remaining -= take;
            }
            total = (float)(total + (float)(remaining * ((float)(java.lang.Math.max(com.oddlabs.tt.player.gauntlet.SitePlanner.MISSING_DISTANCE, max_meters + 60)))));
            float avg = (float)(total / units);
            return (float)(harvest_seconds + (float)(com.oddlabs.tt.player.gauntlet.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * avg));
        }
        // Java: 113
        public float warriorGatherCost(com.oddlabs.tt.player.gauntlet.DistanceField field)
        {
            float tree = com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(field, map.getTrees(), 300, 10, 120, strategy.harvest_seconds);
            float iron = com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(field, map.getIron(), 150, 10, 200, strategy.harvest_seconds);
            return (float)((float)(2 * tree) + iron);
        }
        // Java: 120
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findArmorySite(java.util.List reserved)
        {
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site best = findArmorySite(reserved, strategy.max_armory_distance, start_field);
            if (best == null || (float)(-best.score) > strategy.armory_far_cost)
            {
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site further = findArmorySite(reserved, strategy.max_armory_distance * 2, start_field);
                if (further != null && (best == null || further.score > best.score))
                {
                    best = further;
                }
            }
            return best;
        }
        // Java: 136
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findExpansionSite(java.util.List reserved, com.oddlabs.tt.player.gauntlet.DistanceField from_field)
        {
            return findArmorySite(reserved, strategy.max_armory_distance, from_field);
        }
        // Java: 145
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findFarExpansionSite(java.util.List reserved)
        {
            return findArmorySite(reserved, strategy.max_armory_distance * 2, start_field);
        }
        // Java: 150
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findArmorySite(java.util.List reserved, int max_distance, com.oddlabs.tt.player.gauntlet.DistanceField from_field)
        {
            return findArmorySite(reserved, max_distance, from_field, 0, null, 0.0f, null, 0.0f);
        }
        public const int MAX_REJECTED = 200;
        public const float SEARCH_SECONDS = 60.0f;
        public bool gave_up;
        public int tree_failed;
        // Java: 166
        public bool lastSearchGaveUp()
        {
            return gave_up;
        }
        // Java: 170
        public int lastTreeFailed()
        {
            return tree_failed;
        }
        // Java: 181
        public class Search : java.lang.Object
        {
            public com.oddlabs.tt.player.gauntlet.DistanceField from;
            public int max_distance;
            public int min_cells;
            public float time;
            public java.util.List candidates;
            public java.util.Map costs;
            private void init_Search()
            {
                this.time = (float)(-1000.0f);
                this.candidates = new java.util.ArrayList();
                this.costs = new java.util.LinkedHashMap();
            }
            public Search() : base()
            {
                init_Search();
            }
        }
        // Java: 197
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findArmorySite(java.util.List reserved, int max_distance, com.oddlabs.tt.player.gauntlet.DistanceField from_field, com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok, com.oddlabs.tt.player.gauntlet.SitePlanner.Search search, float now)
        {
            return findArmorySite(reserved, max_distance, from_field, 0, ok, 0.0f, search, now);
        }
        // Java: 210
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findArmorySite(java.util.List reserved, int max_distance, com.oddlabs.tt.player.gauntlet.DistanceField from_field, int min_cells, com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok, float max_tree, com.oddlabs.tt.player.gauntlet.SitePlanner.Search search, float now)
        {
            com.oddlabs.tt.model.BuildingTemplate armory = template(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            int size = map.getSize();
            gave_up = false;
            tree_failed = 0;
            bool reuse = search != null && ((object)search.from == (object)from_field) && search.max_distance == max_distance && search.min_cells == min_cells && (float)(now - search.time) <= com.oddlabs.tt.player.gauntlet.SitePlanner.SEARCH_SECONDS;
            java.util.List candidates = (reuse ? search.candidates : (java.util.List)(new java.util.ArrayList()));
            if (!reuse)
            {
                int sx = from_field.getSourceX();
                int sy = from_field.getSourceY();
                java.util.List iron = map.getIron();
                for (int y = 2; y < size - 2; y += 2)
                {
                    for (int x = 2; x < size - 2; x += 2)
                    {
                        int d = from_field.get(x, y);
                        if (d > max_distance)
                        {
                            continue;
                        }
                        if (min_cells > 0 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, sx, sy) < min_cells * min_cells)
                        {
                            continue;
                        }
                        if (!map.canPlace(armory, x, y) || com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.ARMORY))
                        {
                            continue;
                        }
                        float iron_cycle = (float)(strategy.harvest_seconds + (float)(com.oddlabs.tt.player.gauntlet.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * java.lang.Math.max(0.0f, (float)((float)(1.25f * com.oddlabs.tt.player.gauntlet.SitePlanner.averageDistance(iron, x, y, 15)) - 5.0f))));
                        float tree_cycle = (float)(strategy.harvest_seconds + (float)(com.oddlabs.tt.player.gauntlet.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * java.lang.Math.max(0.0f, (float)((float)(1.25f * map.averageTreeDistance(x, y, 30, 60, 150.0f)) - 5.0f))));
                        float quick = (float)((float)((float)((float)((float)(2 * tree_cycle) + iron_cycle) + (float)((float)(strategy.armory_delay_weight * d) / 5.0f)) + (float)(strategy.armory_distance_weight * d)) + (float)(threat_weight * java.lang.Math.max(0.0f, (float)(exposure(x, y) - 0.42f))));
                        candidates.add(new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(x, y, (float)(-quick)));
                    }
                }
                candidates.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    com.oddlabs.tt.player.gauntlet.SitePlanner.Site a = (com.oddlabs.tt.player.gauntlet.SitePlanner.Site)a_l;
                    com.oddlabs.tt.player.gauntlet.SitePlanner.Site b = (com.oddlabs.tt.player.gauntlet.SitePlanner.Site)b_l;
                    return java.lang.Float.compare(b.score, a.score);
                }));
                if (search != null)
                {
                    search.from = from_field;
                    search.max_distance = max_distance;
                    search.min_cells = min_cells;
                    search.time = now;
                    search.candidates.clear();
                    search.candidates.addAll(candidates);
                }
            }
            if (search != null)
            {
                search.costs.values().removeIf(new java.util.function.Predicate_L((object c_l) =>
                {
                    float[] c = (float[])c_l;
                    return (float)(now - c[3]) > com.oddlabs.tt.player.gauntlet.SitePlanner.SEARCH_SECONDS;
                }));
            }
            if (candidates.isEmpty())
            {
                return null;
            }
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site best = null;
            float best_cost = java.lang.Float.MAX_VALUE;
            java.util.List evaluated = new java.util.ArrayList();
            java.util.List rejected = (ok != null ? (java.util.List)(new java.util.ArrayList()) : java.util.List_.of());
            foreach (com.oddlabs.tt.player.gauntlet.SitePlanner.Site c_2 in java.lang.JIter.of(candidates))
            {
                if (evaluated.size() >= 24)
                {
                    break;
                }
                bool near = false;
                foreach (com.oddlabs.tt.player.gauntlet.SitePlanner.Site e in java.lang.JIter.of(evaluated))
                {
                    near |= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.x, e.y, c_2.x, c_2.y) < 5 * 5;
                }
                if (near)
                {
                    continue;
                }
                if (reuse && (!map.canPlace(armory, c_2.x, c_2.y) || com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(reserved, c_2.x, c_2.y, com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.ARMORY)))
                {
                    continue;
                }
                if (ok != null)
                {
                    foreach (com.oddlabs.tt.player.gauntlet.SitePlanner.Site r in java.lang.JIter.of(rejected))
                    {
                        near |= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(r.x, r.y, c_2.x, c_2.y) < 8 * 8;
                    }
                    if (near)
                    {
                        continue;
                    }
                    if (!ok.test(c_2.x, c_2.y))
                    {
                        rejected.add(c_2);
                        if (rejected.size() >= com.oddlabs.tt.player.gauntlet.SitePlanner.MAX_REJECTED)
                        {
                            gave_up = true;
                            break;
                        }
                        continue;
                    }
                }
                evaluated.add(c_2);
                float[] known = (search != null ? ((float[])search.costs.get(c_2.y * size + c_2.x)) : (float[])(null));
                float gather = default;
                float build = default;
                float tree = 0.0f;
                if (known != null)
                {
                    gather = known[0];
                    build = known[1];
                    tree = known[2];
                }
                else
                {
                    com.oddlabs.tt.player.gauntlet.DistanceField field = map.computeField(c_2.x, c_2.y, 220);
                    gather = warriorGatherCost(field);
                    build = constructionSeconds(c_2.x, c_2.y, com.oddlabs.tt.player.gauntlet.SitePlanner.QUARTERS_WOOD, strategy.armory_builders);
                    if (search != null || max_tree > 0.0f)
                    {
                        tree = com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(field, map.getTrees(), 60, 10, 120, strategy.harvest_seconds);
                    }
                    if (search != null)
                    {
                        search.costs.put(c_2.y * size + c_2.x, new float[] { gather, build, tree, now });
                    }
                }
                int d_2 = from_field.get(c_2.x, c_2.y);
                float delay = (float)(strategy.armory_delay_weight * ((float)(build + (float)(d_2 / 5.0f))));
                float threat = (float)(threat_weight * java.lang.Math.max(0.0f, (float)(exposure(c_2.x, c_2.y) - 0.42f)));
                float cost = (float)((float)((float)((float)(gather + delay) + (float)(strategy.armory_distance_weight * d_2)) + threat) + ((com.oddlabs.tt.player.gauntlet.SitePlanner.hasNear(map.getRocks(), c_2.x, c_2.y, 45) ? 0.0f : 4.0f)));
                if (max_tree > 0.0f && tree >= max_tree)
                {
                    tree_failed++;
                    continue;
                }
                if (cost < best_cost)
                {
                    best_cost = cost;
                    best = new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(c_2.x, c_2.y, (float)(-cost));
                }
            }
            return best;
        }
        // Java: 329
        public float[] searched(com.oddlabs.tt.player.gauntlet.SitePlanner.Search search, int x, int y)
        {
            float[] c = ((float[])search.costs.get(y * map.getSize() + x));
            return (c != null ? new float[] { c[0], c[1], c[2] } : (float[])(null));
        }
        // Java: 335
        public static float averageDistance(java.util.List supplies, int x, int y, int count)
        {
            int[] best = new int[count];
            java.util.Arrays.fill(best, java.lang.Integer.MAX_VALUE);
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY());
                if (d2 >= best[count - 1])
                {
                    continue;
                }
                int i = count - 1;
                while (i > 0 && best[i - 1] > d2)
                {
                    best[i] = best[i - 1];
                    i--;
                }
                best[i] = d2;
            }
            float sum = 0.0f;
            foreach (int d2_2 in best)
            {
                sum = (float)(sum + (d2_2 == java.lang.Integer.MAX_VALUE ? 400.0f : (float)(((float)(java.lang.Math.sqrt(d2_2))) * 2.0f)));
            }
            return (float)(sum / count);
        }
        // Java: 357
        public static bool hasNear(java.util.List supplies, int x, int y, int radius)
        {
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (!s.isEmpty() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 369
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findQuartersSite(java.util.List reserved, int anchor_x, int anchor_y, int max_anchor_dist, com.oddlabs.tt.player.gauntlet.DistanceField anchor_field, int toward_x, int toward_y, int builders, float anchor_weight, float toward_weight)
        {
            com.oddlabs.tt.model.BuildingTemplate quarters = template(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site best = null;
            int r = max_anchor_dist / 2 + 2;
            for (int y = anchor_y - r; y <= anchor_y + r; y++)
            {
                for (int x = anchor_x - r; x <= anchor_x + r; x++)
                {
                    if (!map.inside(x, y))
                    {
                        continue;
                    }
                    int d_anchor = (anchor_field != null ? anchor_field.get(x, y) : java.lang.JC.f2i(com.oddlabs.tt.player.gauntlet.MapAnalysis.meters(anchor_x, anchor_y, x, y)));
                    if (d_anchor > max_anchor_dist)
                    {
                        continue;
                    }
                    if (!start_field.reachable(x, y))
                    {
                        continue;
                    }
                    if (!map.canPlace(quarters, x, y) || com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.QUARTERS))
                    {
                        continue;
                    }
                    float cost = (float)((float)((float)(constructionSeconds(x, y, com.oddlabs.tt.player.gauntlet.SitePlanner.QUARTERS_WOOD, builders) + (float)(anchor_weight * d_anchor)) + (float)(toward_weight * com.oddlabs.tt.player.gauntlet.MapAnalysis.meters(x, y, toward_x, toward_y))) + (float)(120.0f * java.lang.Math.max(0.0f, (float)(exposure(x, y) - 0.45f))));
                    if (best == null || (float)(-cost) > best.score)
                    {
                        best = new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(x, y, (float)(-cost));
                    }
                }
            }
            return best;
        }
        public const int QUARTERS_WOOD = 40;
        public const int TOWER_WOOD = 20;
        public const float REPAIR_SECONDS = 5.0f;
        // Java: 409
        public float constructionSeconds(int x, int y, int wood, int builders)
        {
            builders = java.lang.Math.max(1, builders);
            int trees = java.lang.Math.max(3, (builders + 2) / 3);
            float avg = map.averageTreeDistance(x, y, trees, 35, 90.0f, start_field);
            float walk = java.lang.Math.max(0.0f, (float)((float)(avg * 1.25f) - 5.0f));
            float cycle = (float)((float)(strategy.harvest_seconds + com.oddlabs.tt.player.gauntlet.SitePlanner.REPAIR_SECONDS) + (float)(com.oddlabs.tt.player.gauntlet.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * walk));
            return (float)((float)(wood * cycle) / builders);
        }
        // Java: 422
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findTowerSite(java.util.List reserved, int cx, int cy, int min_r, int max_r, java.util.List existing_towers, int face_x, int face_y)
        {
            return findTowerSite(reserved, cx, cy, min_r, max_r, existing_towers, face_x, face_y, null);
        }
        // Java: 429
        public interface CellOk
        {
            // Java: 431
            bool test(int x, int y);
        }
        public sealed class CellOk_L : java.lang.Object, CellOk
        {
            readonly System.Func<int, int, bool> f;
            public CellOk_L(System.Func<int, int, bool> f) { this.f = f; }
            public bool test(int p0, int p1) { return f(p0, p1); }
        }
        // Java: 435
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findTowerSite(java.util.List reserved, int cx, int cy, int min_r, int max_r, java.util.List existing_towers, int face_x, int face_y, com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok)
        {
            com.oddlabs.tt.model.BuildingTemplate tower = template(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            float fx = face_x - cx;
            float fy = face_y - cy;
            float flen = ((float)(java.lang.Math.sqrt((float)((float)(fx * fx) + (float)(fy * fy)))));
            if (flen > 0)
            {
                fx = (float)(fx / flen);
                fy = (float)(fy / flen);
            }
            float base_height = map.height(cx, cy);
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site best = null;
            for (int y = cy - max_r; y <= cy + max_r; y++)
            {
                for (int x = cx - max_r; x <= cx + max_r; x++)
                {
                    int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, x, y);
                    if (d2 < min_r * min_r || d2 > max_r * max_r)
                    {
                        continue;
                    }
                    if (!map.inside(x, y) || (!start_field.reachable(x, y) && !reachableNear(x, y)))
                    {
                        continue;
                    }
                    if (!map.canPlace(tower, x, y) || com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.TOWER))
                    {
                        continue;
                    }
                    if (ok != null && !ok.test(x, y))
                    {
                        continue;
                    }
                    float d = ((float)(java.lang.Math.sqrt(d2)));
                    float align = (float)(((float)((float)((x - cx) * fx) + (float)((y - cy) * fy))) / java.lang.Math.max(1.0f, d));
                    float spread = 0.0f;
                    int covering = 0;
                    foreach (int[] t in java.lang.JIter.of(existing_towers))
                    {
                        int td2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t[0], t[1], x, y);
                        if (strategy.tower_mutual)
                        {
                            if (td2 < 9 * 9)
                            {
                                spread = (float)(spread - 20.0f);
                            }
                            else if (td2 <= 13 * 13 && covering++ < 2)
                            {
                                spread = (float)(spread + 6.0f);
                            }
                        }
                        else if (td2 < 10 * 10)
                        {
                            spread = (float)(spread - 20.0f);
                        }
                        else if (td2 < 16 * 16)
                        {
                            spread = (float)(spread - 6.0f);
                        }
                    }
                    float score = (float)((float)((float)((float)(6.0f * align) + (float)(0.6f * java.lang.Math.min(8.0f, (float)(map.height(x, y) - base_height)))) - (float)(0.15f * constructionSeconds(x, y, com.oddlabs.tt.player.gauntlet.SitePlanner.TOWER_WOOD, strategy.tower_builders))) + spread);
                    if (best == null || score > best.score)
                    {
                        best = new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(x, y, score);
                    }
                }
            }
            return best;
        }
        // Java: 491
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findQuartersSiteLike(java.util.List reserved, int cx, int cy, int radius, int type)
        {
            return findQuartersSiteLike(reserved, cx, cy, radius, type, null);
        }
        // Java: 497
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site findQuartersSiteLike(java.util.List reserved, int cx, int cy, int radius, int type, com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok)
        {
            com.oddlabs.tt.model.BuildingTemplate t = template(type);
            int half = com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.of(type);
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site best = null;
            for (int y = cy - radius; y <= cy + radius; y++)
            {
                for (int x = cx - radius; x <= cx + radius; x++)
                {
                    if (!map.inside(x, y) || !reachableNear(x, y))
                    {
                        continue;
                    }
                    if (!map.canPlace(t, x, y) || com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(reserved, x, y, half))
                    {
                        continue;
                    }
                    if (ok != null && !ok.test(x, y))
                    {
                        continue;
                    }
                    float score = (float)((float)(0.5f * java.lang.Math.min(map.treesAround(x, y, 6), 12)) - ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(cx, cy, x, y)))));
                    if (best == null || score > best.score)
                    {
                        best = new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(x, y, score);
                    }
                }
            }
            return best;
        }
        // Java: 520
        public bool reachableNear(int x, int y)
        {
            return start_field.getAround(x, y, 3) != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE;
        }
        // Java: 527
        public static bool conflicts(java.util.List reserved, int x, int y, int half)
        {
            foreach (com.oddlabs.tt.player.gauntlet.SitePlanner.Site s in java.lang.JIter.of(reserved))
            {
                int gap = half + s.half + com.oddlabs.tt.player.gauntlet.SitePlanner.BUILDING_GAP;
                if (java.lang.Math.abs(s.x - x) < gap && java.lang.Math.abs(s.y - y) < gap)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 536
        public int getStartX()
        {
            return start_x;
        }
        // Java: 540
        public int getStartY()
        {
            return start_y;
        }
        // Java: 544
        public int getEnemyX()
        {
            return enemy_x;
        }
        // Java: 548
        public int getEnemyY()
        {
            return enemy_y;
        }
        // Java: 552
        public com.oddlabs.tt.player.gauntlet.DistanceField getStartField()
        {
            return start_field;
        }
        // Java: 557
        public com.oddlabs.tt.player.gauntlet.DistanceField getEnemyField()
        {
            return enemy_field;
        }
        // Java: 563
        public class RaceSizes : java.lang.Object
        {
            public const int QUARTERS = 4;
            public const int ARMORY = 4;
            public const int TOWER = 2;
            // Java: 568
            public static int of(int type)
            {
                return ((type) switch { 0 => (int)(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.QUARTERS), 1 => (int)(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.ARMORY), _ => (int)(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.TOWER) });
            }
            public RaceSizes() : base()
            {
            }
        }
        // Java: 581
        public class Site : java.lang.Object
        {
            public int x;
            public int y;
            public float score;
            public int half;
            // Java: 593
            public com.oddlabs.tt.player.gauntlet.SitePlanner.Site withHalf(int half)
            {
                this.half = half;
                return this;
            }
            private void init_Site()
            {
                this.half = com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.QUARTERS;
            }
            public Site(int x, int y, float score) : base()
            {
                init_Site();
                this.x = x;
                this.y = y;
                this.score = score;
            }
        }
        public SitePlanner(com.oddlabs.tt.player.gauntlet.MapAnalysis map, com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.gauntlet.Strategy strategy, int start_x, int start_y, int enemy_x, int enemy_y, com.oddlabs.tt.player.gauntlet.DistanceField start_field, com.oddlabs.tt.player.gauntlet.DistanceField enemy_field) : base()
        {
            this.map = map;
            this.owner = owner;
            this.strategy = strategy;
            this.start_x = start_x;
            this.start_y = start_y;
            this.enemy_x = enemy_x;
            this.enemy_y = enemy_y;
            this.start_field = start_field;
            this.enemy_field = enemy_field;
            int enemies = 0;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (owner.isEnemy(p))
                {
                    enemies++;
                }
            }
            this.threat_weight = (float)(strategy.armory_threat_weight * ((float)(1.0f + (float)(0.75f * java.lang.Math.min(strategy.armory_threat_cap, java.lang.Math.max(0, enemies - 1))))));
        }
    }
}

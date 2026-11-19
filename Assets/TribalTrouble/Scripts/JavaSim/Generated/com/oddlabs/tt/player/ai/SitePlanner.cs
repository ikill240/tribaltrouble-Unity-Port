// Translated from SitePlanner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 21
    public class SitePlanner : java.lang.Object
    {
        public const float HARVEST_SECONDS = 10.0f;
        public const float ROUND_TRIP_SECONDS_PER_METER = 0.45f;
        public const int MISSING_DISTANCE = 140;
        public const int BUILDING_GAP = 3;
        public static bool debug_sites;
        public com.oddlabs.tt.player.ai.MapAnalysis map;
        public com.oddlabs.tt.player.Player owner;
        public com.oddlabs.tt.player.ai.Strategy strategy;
        public com.oddlabs.tt.player.ai.DistanceField start_field;
        public com.oddlabs.tt.player.ai.DistanceField enemy_field;
        public int start_x;
        public int start_y;
        public int enemy_x;
        public int enemy_y;
        public float threat_weight;
        // Java: 62
        public com.oddlabs.tt.model.BuildingTemplate template(int type)
        {
            return owner.getRace().getBuildingTemplate(type);
        }
        // Java: 67
        public float exposure(int x, int y)
        {
            int ours = start_field.get(x, y);
            int theirs = enemy_field.get(x, y);
            if (ours == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
            {
                return 1.0f;
            }
            if (theirs == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
            {
                return 0.0f;
            }
            return (float)(ours / ((float)(java.lang.Math.max(1, ours + theirs))));
        }
        // Java: 80
        public static float gatherSeconds(com.oddlabs.tt.player.ai.DistanceField field, java.util.List supplies, int units, int per_node, int max_meters)
        {
            int sx = field.getSourceX();
            int sy = field.getSourceY();
            int limit2 = (max_meters / 2) * (max_meters / 2);
            java.util.List near = com.oddlabs.tt.player.ai.MapAnalysis.near(supplies, sx, sy, max_meters / 2);
            int[] dists = new int[near.size()];
            int n = 0;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(near))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(sx, sy, s.getGridX(), s.getGridY()) > limit2)
                {
                    continue;
                }
                int d = field.getAround(s.getGridX(), s.getGridY(), 1);
                if (d != com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
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
            total = (float)(total + (float)(remaining * ((float)(java.lang.Math.max(com.oddlabs.tt.player.ai.SitePlanner.MISSING_DISTANCE, max_meters + 60)))));
            float avg = (float)(total / units);
            return (float)(com.oddlabs.tt.player.ai.SitePlanner.HARVEST_SECONDS + (float)(com.oddlabs.tt.player.ai.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * avg));
        }
        // Java: 115
        public float warriorGatherCost(com.oddlabs.tt.player.ai.DistanceField field)
        {
            float tree = com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(field, map.getTrees(), 300, 10, 120);
            float iron = com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(field, map.getIron(), 150, 10, 200);
            return (float)((float)(2 * tree) + iron);
        }
        // Java: 122
        public com.oddlabs.tt.player.ai.SitePlanner.Site findArmorySite(java.util.List reserved)
        {
            com.oddlabs.tt.player.ai.SitePlanner.Site best = findArmorySite(reserved, strategy.max_armory_distance, start_field);
            if (best == null || (float)(-best.score) > strategy.armory_far_cost)
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site further = findArmorySite(reserved, strategy.max_armory_distance * 2, start_field);
                if (further != null && (best == null || further.score > best.score))
                {
                    best = further;
                }
            }
            return best;
        }
        // Java: 138
        public com.oddlabs.tt.player.ai.SitePlanner.Site findExpansionSite(java.util.List reserved, com.oddlabs.tt.player.ai.DistanceField from_field)
        {
            return findArmorySite(reserved, strategy.max_armory_distance, from_field);
        }
        // Java: 147
        public com.oddlabs.tt.player.ai.SitePlanner.Site findFarExpansionSite(java.util.List reserved)
        {
            return findArmorySite(reserved, strategy.max_armory_distance * 2, start_field);
        }
        // Java: 152
        public com.oddlabs.tt.player.ai.SitePlanner.Site findArmorySite(java.util.List reserved, int max_distance, com.oddlabs.tt.player.ai.DistanceField from_field)
        {
            com.oddlabs.tt.model.BuildingTemplate armory = template(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            int size = map.getSize();
            java.util.List candidates = new java.util.ArrayList();
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
                    if (!map.canPlace(armory, x, y) || com.oddlabs.tt.player.ai.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.ARMORY))
                    {
                        continue;
                    }
                    float iron_cycle = (float)(com.oddlabs.tt.player.ai.SitePlanner.HARVEST_SECONDS + (float)(com.oddlabs.tt.player.ai.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * java.lang.Math.max(0.0f, (float)((float)(1.25f * com.oddlabs.tt.player.ai.SitePlanner.averageDistance(iron, x, y, 15)) - 5.0f))));
                    float tree_cycle = (float)(com.oddlabs.tt.player.ai.SitePlanner.HARVEST_SECONDS + (float)(com.oddlabs.tt.player.ai.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * java.lang.Math.max(0.0f, (float)((float)(1.25f * map.averageTreeDistance(x, y, 30, 60, 150.0f)) - 5.0f))));
                    float quick = (float)((float)((float)((float)((float)(2 * tree_cycle) + iron_cycle) + (float)((float)(strategy.armory_delay_weight * d) / 5.0f)) + (float)(strategy.armory_distance_weight * d)) + (float)(threat_weight * java.lang.Math.max(0.0f, (float)(exposure(x, y) - 0.42f))));
                    candidates.add(new com.oddlabs.tt.player.ai.SitePlanner.Site(x, y, (float)(-quick)));
                }
            }
            if (candidates.isEmpty())
            {
                return null;
            }
            candidates.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site a = (com.oddlabs.tt.player.ai.SitePlanner.Site)a_l;
                com.oddlabs.tt.player.ai.SitePlanner.Site b = (com.oddlabs.tt.player.ai.SitePlanner.Site)b_l;
                return java.lang.Float.compare(b.score, a.score);
            }));
            com.oddlabs.tt.player.ai.SitePlanner.Site best = null;
            float best_cost = java.lang.Float.MAX_VALUE;
            java.util.List evaluated = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.ai.SitePlanner.Site c in java.lang.JIter.of(candidates))
            {
                if (evaluated.size() >= 24)
                {
                    break;
                }
                bool near = false;
                foreach (com.oddlabs.tt.player.ai.SitePlanner.Site e in java.lang.JIter.of(evaluated))
                {
                    near |= com.oddlabs.tt.player.ai.MapAnalysis.dist2(e.x, e.y, c.x, c.y) < 5 * 5;
                }
                if (near)
                {
                    continue;
                }
                evaluated.add(c);
                com.oddlabs.tt.player.ai.DistanceField field = map.computeField(c.x, c.y, 220);
                int d_2 = from_field.get(c.x, c.y);
                float gather = warriorGatherCost(field);
                float build = constructionSeconds(c.x, c.y, com.oddlabs.tt.player.ai.SitePlanner.QUARTERS_WOOD, strategy.armory_builders);
                float delay = (float)(strategy.armory_delay_weight * ((float)(build + (float)(d_2 / 5.0f))));
                float threat = (float)(threat_weight * java.lang.Math.max(0.0f, (float)(exposure(c.x, c.y) - 0.42f)));
                float cost = (float)((float)((float)((float)(gather + delay) + (float)(strategy.armory_distance_weight * d_2)) + threat) + ((com.oddlabs.tt.player.ai.SitePlanner.hasNear(map.getRocks(), c.x, c.y, 45) ? 0.0f : 4.0f)));
                if (com.oddlabs.tt.player.ai.SitePlanner.debug_sites)
                {
                    java.lang.JSystem.@out.println(java.lang.JString.format("armory candidate %d,%d: gather %.1f (tree %.1f iron %.1f) build %.0fs dist %dm threat %.1f -> %.1f", c.x, c.y, gather, com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(field, map.getTrees(), 300, 10, 120), com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(field, map.getIron(), 150, 10, 200), build, d_2, threat, cost));
                }
                if (cost < best_cost)
                {
                    best_cost = cost;
                    best = new com.oddlabs.tt.player.ai.SitePlanner.Site(c.x, c.y, (float)(-cost));
                }
            }
            return best;
        }
        // Java: 217
        public static float averageDistance(java.util.List supplies, int x, int y, int count)
        {
            int[] best = new int[count];
            java.util.Arrays.fill(best, java.lang.Integer.MAX_VALUE);
            if ((supplies is com.oddlabs.tt.player.ai.MapAnalysis.SupplyList list) && x >= 0 && y >= 0 && x / list.cell() < list.side() && y / list.cell() < list.side())
            {
                int side = list.side();
                int cbx = x / list.cell();
                int cby = y / list.cell();
                int rings = java.lang.Math.max(java.lang.Math.max(cbx, side - 1 - cbx), java.lang.Math.max(cby, side - 1 - cby));
                java.util.List ring = new java.util.ArrayList();
                com.oddlabs.tt.player.ai.MapAnalysis.SupplyList all = ((com.oddlabs.tt.player.ai.MapAnalysis.SupplyList)list);
                for (int r = 0; r <= rings; r++)
                {
                    long l = list.ringDistance(r);
                    if (l * l >= best[count - 1])
                    {
                        break;
                    }
                    com.oddlabs.tt.player.ai.MapAnalysis.ringBuckets(all, cbx, cby, r, 0, 0, side - 1, side - 1, ring);
                    foreach (java.util.List bucket in java.lang.JIter.of(ring))
                    {
                        com.oddlabs.tt.player.ai.SitePlanner.nearest(bucket, x, y, best);
                    }
                }
            }
            else
            {
                com.oddlabs.tt.player.ai.SitePlanner.nearest(supplies, x, y, best);
            }
            float sum = 0.0f;
            foreach (int d2 in best)
            {
                sum = (float)(sum + (d2 == java.lang.Integer.MAX_VALUE ? 400.0f : (float)(((float)(java.lang.Math.sqrt(d2))) * 2.0f)));
            }
            return (float)(sum / count);
        }
        // Java: 248
        public static void nearest(java.util.List supplies, int x, int y, int[] best)
        {
            int count = best.Length;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY());
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
        }
        // Java: 265
        public static bool hasNear(java.util.List supplies, int x, int y, int radius)
        {
            int r2 = radius * radius;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (!s.isEmpty() && com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY()) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 277
        public com.oddlabs.tt.player.ai.SitePlanner.Site findQuartersSite(java.util.List reserved, int anchor_x, int anchor_y, int max_anchor_dist, com.oddlabs.tt.player.ai.DistanceField anchor_field, int toward_x, int toward_y, int builders, float anchor_weight, float toward_weight)
        {
            com.oddlabs.tt.model.BuildingTemplate quarters = template(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            com.oddlabs.tt.player.ai.SitePlanner.Site best = null;
            int r = max_anchor_dist / 2 + 2;
            for (int y = anchor_y - r; y <= anchor_y + r; y++)
            {
                for (int x = anchor_x - r; x <= anchor_x + r; x++)
                {
                    if (!map.inside(x, y))
                    {
                        continue;
                    }
                    int d_anchor = (anchor_field != null ? anchor_field.get(x, y) : java.lang.JC.f2i(com.oddlabs.tt.player.ai.MapAnalysis.meters(anchor_x, anchor_y, x, y)));
                    if (d_anchor > max_anchor_dist)
                    {
                        continue;
                    }
                    if (!start_field.reachable(x, y))
                    {
                        continue;
                    }
                    if (!map.canPlace(quarters, x, y) || com.oddlabs.tt.player.ai.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.QUARTERS))
                    {
                        continue;
                    }
                    float cost = (float)((float)((float)(constructionSeconds(x, y, com.oddlabs.tt.player.ai.SitePlanner.QUARTERS_WOOD, builders) + (float)(anchor_weight * d_anchor)) + (float)(toward_weight * com.oddlabs.tt.player.ai.MapAnalysis.meters(x, y, toward_x, toward_y))) + (float)(120.0f * java.lang.Math.max(0.0f, (float)(exposure(x, y) - 0.45f))));
                    if (best == null || (float)(-cost) > best.score)
                    {
                        best = new com.oddlabs.tt.player.ai.SitePlanner.Site(x, y, (float)(-cost));
                    }
                }
            }
            return best;
        }
        public const int QUARTERS_WOOD = 40;
        public const int TOWER_WOOD = 20;
        public const float REPAIR_SECONDS = 5.0f;
        // Java: 317
        public float constructionSeconds(int x, int y, int wood, int builders)
        {
            builders = java.lang.Math.max(1, builders);
            int trees = java.lang.Math.max(3, (builders + 2) / 3);
            float avg = map.averageTreeDistance(x, y, trees, 35, 90.0f, start_field);
            float walk = java.lang.Math.max(0.0f, (float)((float)(avg * 1.25f) - 5.0f));
            float cycle = (float)((float)(com.oddlabs.tt.player.ai.SitePlanner.HARVEST_SECONDS + com.oddlabs.tt.player.ai.SitePlanner.REPAIR_SECONDS) + (float)(com.oddlabs.tt.player.ai.SitePlanner.ROUND_TRIP_SECONDS_PER_METER * walk));
            return (float)((float)(wood * cycle) / builders);
        }
        // Java: 330
        public com.oddlabs.tt.player.ai.SitePlanner.Site findTowerSite(java.util.List reserved, int cx, int cy, int min_r, int max_r, java.util.List existing_towers, int face_x, int face_y)
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
            com.oddlabs.tt.player.ai.SitePlanner.Site best = null;
            for (int y = cy - max_r; y <= cy + max_r; y++)
            {
                for (int x = cx - max_r; x <= cx + max_r; x++)
                {
                    int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(cx, cy, x, y);
                    if (d2 < min_r * min_r || d2 > max_r * max_r)
                    {
                        continue;
                    }
                    if (!map.inside(x, y) || (!start_field.reachable(x, y) && !reachableNear(x, y)))
                    {
                        continue;
                    }
                    if (!map.canPlace(tower, x, y) || com.oddlabs.tt.player.ai.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.TOWER))
                    {
                        continue;
                    }
                    float d = ((float)(java.lang.Math.sqrt(d2)));
                    float align = (float)(((float)((float)((x - cx) * fx) + (float)((y - cy) * fy))) / java.lang.Math.max(1.0f, d));
                    float spread = 0.0f;
                    foreach (int[] t in java.lang.JIter.of(existing_towers))
                    {
                        int td2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(t[0], t[1], x, y);
                        if (td2 < 10 * 10)
                        {
                            spread = (float)(spread - 20.0f);
                        }
                        else if (td2 < 16 * 16)
                        {
                            spread = (float)(spread - 6.0f);
                        }
                    }
                    float score = (float)((float)((float)((float)(6.0f * align) + (float)(0.6f * java.lang.Math.min(8.0f, (float)(map.height(x, y) - base_height)))) - (float)(0.15f * constructionSeconds(x, y, com.oddlabs.tt.player.ai.SitePlanner.TOWER_WOOD, strategy.tower_builders))) + spread);
                    if (best == null || score > best.score)
                    {
                        best = new com.oddlabs.tt.player.ai.SitePlanner.Site(x, y, score);
                    }
                }
            }
            return best;
        }
        // Java: 376
        public com.oddlabs.tt.player.ai.SitePlanner.Site findQuartersSiteLike(java.util.List reserved, int cx, int cy, int radius, int type)
        {
            com.oddlabs.tt.model.BuildingTemplate t = template(type);
            int half = com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.of(type);
            com.oddlabs.tt.player.ai.SitePlanner.Site best = null;
            for (int y = cy - radius; y <= cy + radius; y++)
            {
                for (int x = cx - radius; x <= cx + radius; x++)
                {
                    if (!map.inside(x, y) || !reachableNear(x, y))
                    {
                        continue;
                    }
                    if (!map.canPlace(t, x, y) || com.oddlabs.tt.player.ai.SitePlanner.conflicts(reserved, x, y, half))
                    {
                        continue;
                    }
                    float score = (float)((float)(0.5f * java.lang.Math.min(map.treesAround(x, y, 6), 12)) - ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ai.MapAnalysis.dist2(cx, cy, x, y)))));
                    if (best == null || score > best.score)
                    {
                        best = new com.oddlabs.tt.player.ai.SitePlanner.Site(x, y, score);
                    }
                }
            }
            return best;
        }
        // Java: 396
        public bool reachableNear(int x, int y)
        {
            return start_field.getAround(x, y, 3) != com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE;
        }
        // Java: 403
        public static bool conflicts(java.util.List reserved, int x, int y, int half)
        {
            foreach (com.oddlabs.tt.player.ai.SitePlanner.Site s in java.lang.JIter.of(reserved))
            {
                int gap = half + s.half + com.oddlabs.tt.player.ai.SitePlanner.BUILDING_GAP;
                if (java.lang.Math.abs(s.x - x) < gap && java.lang.Math.abs(s.y - y) < gap)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 413
        public static com.oddlabs.tt.landscape.TreeSupply nearestTree(java.util.List trees, int x, int y)
        {
            com.oddlabs.tt.landscape.TreeSupply best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.landscape.TreeSupply t in java.lang.JIter.of(trees))
            {
                if (t.isEmpty())
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(x, y, t.getGridX(), t.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = t;
                }
            }
            return best;
        }
        // Java: 428
        public int getStartX()
        {
            return start_x;
        }
        // Java: 432
        public int getStartY()
        {
            return start_y;
        }
        // Java: 436
        public int getEnemyX()
        {
            return enemy_x;
        }
        // Java: 440
        public int getEnemyY()
        {
            return enemy_y;
        }
        // Java: 444
        public com.oddlabs.tt.player.ai.DistanceField getStartField()
        {
            return start_field;
        }
        // Java: 449
        public com.oddlabs.tt.player.ai.DistanceField getEnemyField()
        {
            return enemy_field;
        }
        // Java: 455
        public class RaceSizes : java.lang.Object
        {
            public const int QUARTERS = 4;
            public const int ARMORY = 4;
            public const int TOWER = 2;
            // Java: 460
            public static int of(int type)
            {
                return ((type) switch { 0 => (int)(com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.QUARTERS), 1 => (int)(com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.ARMORY), 6 => (int)(3), _ => (int)(com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.TOWER) });
            }
            public RaceSizes() : base()
            {
            }
        }
        // Java: 474
        public class Site : java.lang.Object
        {
            public int x;
            public int y;
            public float score;
            public int half;
            // Java: 486
            public com.oddlabs.tt.player.ai.SitePlanner.Site withHalf(int half)
            {
                this.half = half;
                return this;
            }
            private void init_Site()
            {
                this.half = com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.QUARTERS;
            }
            public Site(int x, int y, float score) : base()
            {
                init_Site();
                this.x = x;
                this.y = y;
                this.score = score;
            }
        }
        static SitePlanner()
        {
            SitePlanner.debug_sites = false;
        }
        public SitePlanner(com.oddlabs.tt.player.ai.MapAnalysis map, com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.ai.Strategy strategy, int start_x, int start_y, int enemy_x, int enemy_y, com.oddlabs.tt.player.ai.DistanceField start_field, com.oddlabs.tt.player.ai.DistanceField enemy_field) : base()
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
            this.threat_weight = (float)(strategy.armory_threat_weight * ((float)(1.0f + (float)(0.75f * java.lang.Math.max(0, enemies - 1)))));
        }
    }
}

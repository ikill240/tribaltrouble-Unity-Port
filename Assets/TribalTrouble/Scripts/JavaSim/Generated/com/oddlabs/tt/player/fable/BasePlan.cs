// Translated from BasePlan.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 25
    public class BasePlan : java.lang.Object
    {
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.player.fable.ResourceMap resources;
        public com.oddlabs.tt.player.fable.SiteFinder sites;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public com.oddlabs.tt.landscape.HeightMap map;
        public int grid_size;
        public int start_x;
        public int start_y;
        public int enemy_x;
        public int enemy_y;
        public int island;
        public float ux;
        public float uy;
        public int armory_x;
        public int armory_y;
        public float armory_score;
        public int fallback_x;
        public int fallback_y;
        public int q1_x;
        public int q1_y;
        // Java: 65
        public void init()
        {
            start_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(player.getStartX());
            start_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(player.getStartY());
            island = map.getIslandId(start_x, start_y);
            int cx = grid_size / 2;
            int cy = grid_size / 2;
            enemy_x = cx;
            enemy_y = cy;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in player.getWorld().getPlayers())
            {
                if (!player.isEnemy(p))
                {
                    continue;
                }
                int ex = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                int ey = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                int d2 = (ex - start_x) * (ex - start_x) + (ey - start_y) * (ey - start_y);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    enemy_x = ex;
                    enemy_y = ey;
                }
            }
            float dx = enemy_x - start_x;
            float dy = enemy_y - start_y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < 1.0f)
            {
                dx = cx - start_x;
                dy = cy - start_y;
                len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            }
            ux = (len < 1.0f ? 1.0f : (float)(dx / len));
            uy = (len < 1.0f ? 0.0f : (float)(dy / len));
            chooseArmorySite();
            chooseFirstQuartersSite();
            chooseFallbackSite();
            log.info(new java.util.function.Supplier_L(() =>
            {
                return "plan: start " + (start_x) + "," + (start_y) + " enemy " + (enemy_x) + "," + (enemy_y) + " armory " + (armory_x) + "," + (armory_y) + " (score " + (java.lang.JC.f2i(armory_score)) + ") q1 " + (q1_x) + "," + (q1_y) + " fallback " + (fallback_x) + "," + (fallback_y) + " armory walk=" + (java.lang.JC.f2i(walk(start_x, start_y, armory_x, armory_y))) + "/" + (java.lang.JC.f2i(com.oddlabs.tt.player.fable.BasePlan.dist(start_x, start_y, armory_x, armory_y))) + " q1 walk=" + (java.lang.JC.f2i(walk(start_x, start_y, q1_x, q1_y))) + " iron " + java.lang.JStr.valueOf(nodesReport(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON)) + " rock " + java.lang.JStr.valueOf(nodesReport(com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK));
            }));
        }
        public java.util.Map walk_cache;
        // Java: 118
        public float walk(int ax, int ay, int bx, int by)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = player.getWorld().getUnitGrid();
            com.oddlabs.tt.pathfinder.Region src = grid.getRegion(ax, ay);
            com.oddlabs.tt.pathfinder.Region dst = grid.getRegion(bx, by);
            if (src == null || dst == null)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            float direct = com.oddlabs.tt.player.fable.BasePlan.dist(ax, ay, bx, by);
            if (((object)src == (object)dst))
            {
                return direct;
            }
            java.util.Map from = ((java.util.Map)walk_cache.computeIfAbsent(src, new java.util.function.Function_L((object k_l) =>
            {
                com.oddlabs.tt.pathfinder.Region k = (com.oddlabs.tt.pathfinder.Region)k_l;
                return new java.util.LinkedHashMap();
            })));
            float[] via = ((float[])from.get(dst));
            if (via == null)
            {
                via = com.oddlabs.tt.player.fable.BasePlan.regionLegs(grid, src, dst, ax, ay);
                from.put(dst, via);
            }
            if (via[0] < 0.0f)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            return java.lang.Math.max(direct, (float)(via[0] + com.oddlabs.tt.player.fable.BasePlan.dist(java.lang.Math.round(via[1]), java.lang.Math.round(via[2]), bx, by)));
        }
        // Java: 139
        public static float detourLimit(float direct)
        {
            return (float)((float)(1.5f * direct) + 40.0f);
        }
        // Java: 144
        public bool walkable(int gx, int gy)
        {
            return walk(start_x, start_y, gx, gy) <= com.oddlabs.tt.player.fable.BasePlan.detourLimit(com.oddlabs.tt.player.fable.BasePlan.dist(start_x, start_y, gx, gy));
        }
        // Java: 148
        public static float[] regionLegs(com.oddlabs.tt.pathfinder.UnitGrid grid, com.oddlabs.tt.pathfinder.Region src, com.oddlabs.tt.pathfinder.Region dst, int ax, int ay)
        {
            com.oddlabs.tt.pathfinder.Region end = com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(grid, src, dst);
            if (end == null)
            {
                return new float[] { (float)(-1.0f), 0.0f, 0.0f };
            }
            java.util.List centres = new java.util.ArrayList();
            for (com.oddlabs.tt.pathfinder.Region r = ((com.oddlabs.tt.pathfinder.Region)end.getParent()); r != null && ((object)r != (object)src); r = ((com.oddlabs.tt.pathfinder.Region)r.getParent()))
            {
                centres.add(new int[] { r.getGridX(), r.getGridY() });
            }
            float len = 0.0f;
            int px = ax;
            int py = ay;
            for (int i = centres.size() - 1; i >= 0; i--)
            {
                int[] c = ((int[])centres.get(i));
                len = (float)(len + com.oddlabs.tt.player.fable.BasePlan.dist(px, py, c[0], c[1]));
                px = c[0];
                py = c[1];
            }
            return new float[] { len, px, py };
        }
        // Java: 170
        public string nodesReport(com.oddlabs.tt.player.fable.ResourceMap.Kind kind)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(resources.nearestAlive(kind, armory_x, armory_y, 150, 3)))
            {
                if (sb.length() > 0)
                {
                    sb.append(' ');
                }
                float w = walk(armory_x, armory_y, n.grid_x, n.grid_y);
                sb.append((w == java.lang.Float.POSITIVE_INFINITY ? "inf" : java.lang.JString.valueOf(java.lang.JC.f2i(w)))).append('/').append(java.lang.JC.f2i(com.oddlabs.tt.player.fable.BasePlan.dist(armory_x, armory_y, n.grid_x, n.grid_y)));
            }
            return (sb.length() == 0 ? "none" : java.lang.JO.toString(sb));
        }
        // Java: 189
        public void chooseArmorySite()
        {
            int cx = grid_size / 2;
            int cy = grid_size / 2;
            float sdx = cx - start_x;
            float sdy = cy - start_y;
            float slen = ((float)(java.lang.Math.sqrt((float)((float)(sdx * sdx) + (float)(sdy * sdy)))));
            float sx = (slen < 1.0f ? ux : (float)(sdx / slen));
            float sy = (slen < 1.0f ? uy : (float)(sdy / slen));
            float cos_limit = ((float)(java.lang.StrictMath.cos(java.lang.StrictMath.toRadians(@params.armory_sector_deg))));
            int min_d = @params.armory_min_dist;
            int max_d = @params.armory_max_dist;
            com.oddlabs.tt.player.fable.SiteFinder.Site best = sites.best(com.oddlabs.tt.model.Race.BUILDING_ARMORY, start_x, start_y, max_d, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
            {
                float dx = gx - start_x;
                float dy = gy - start_y;
                float d = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
                if (d < min_d || d > max_d)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                if (d > 3.0f && (float)(((float)((float)(dx * sx) + (float)(dy * sy))) / d) < cos_limit)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                float w = walk(start_x, start_y, gx, gy);
                if (w > @params.armory_max_walk || w > (float)((float)(2.5f * d) + 60.0f))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                return (float)(-((float)(armoryCost(gx, gy, d) + (float)(@params.site_w_detour * ((float)(w - d))))));
            }));
            if (best == null)
            {
                best = sites.best(com.oddlabs.tt.model.Race.BUILDING_ARMORY, start_x, start_y, max_d, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx_2, int gy_2) =>
                {
                    float w_2 = walk(start_x, start_y, gx_2, gy_2);
                    return (w_2 > com.oddlabs.tt.player.fable.BasePlan.detourLimit(com.oddlabs.tt.player.fable.BasePlan.dist(gx_2, gy_2, start_x, start_y)) ? java.lang.Float.NEGATIVE_INFINITY : (float)(-w_2));
                }));
            }
            if (best != null)
            {
                armory_x = best.gx();
                armory_y = best.gy();
                armory_score = (float)(-best.score());
            }
            else
            {
                armory_x = start_x;
                armory_y = start_y;
                armory_score = java.lang.Float.MAX_VALUE;
            }
        }
        // Java: 239
        public com.oddlabs.tt.player.fable.SiteFinder.Site chooseExpansionSite(int main_x, int main_y, int max_dist)
        {
            java.util.List iron = resources.within(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, main_x, main_y, max_dist, true);
            java.util.List cands = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node n in java.lang.JIter.of(iron))
            {
                float d_main = com.oddlabs.tt.player.fable.BasePlan.dist(n.grid_x, n.grid_y, main_x, main_y);
                if (d_main < 28.0f || walk(main_x, main_y, n.grid_x, n.grid_y) > com.oddlabs.tt.player.fable.BasePlan.detourLimit(d_main))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.fable.BasePlan.dist(n.grid_x, n.grid_y, enemy_x, enemy_y) < com.oddlabs.tt.player.fable.BasePlan.dist(n.grid_x, n.grid_y, start_x, start_y))
                {
                    continue;
                }
                int cluster = resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, n.grid_x, n.grid_y, 30);
                cands.add(new float[] { n.grid_x, n.grid_y, (float)((float)(cluster * 10.0f) - (float)(0.15f * d_main)) });
            }
            if (cands.isEmpty())
            {
                return null;
            }
            cands.sort(new java.util.Comparator_L((object a_l, object b_l) =>
            {
                float[] a = (float[])a_l;
                float[] b = (float[])b_l;
                int c = java.lang.Float.compare(b[2], a[2]);
                if (c != 0)
                {
                    return c;
                }
                c = java.lang.Float.compare(a[0], b[0]);
                return (c != 0 ? c : java.lang.Float.compare(a[1], b[1]));
            }));
            com.oddlabs.tt.player.fable.SiteFinder.Site best = null;
            for (int i = 0; i < java.lang.Math.min(4, cands.size()); i++)
            {
                int cx = java.lang.JC.f2i(((float[])cands.get(i))[0]);
                int cy = java.lang.JC.f2i(((float[])cands.get(i))[1]);
                com.oddlabs.tt.player.fable.SiteFinder.Site s = sites.best(com.oddlabs.tt.model.Race.BUILDING_ARMORY, cx, cy, 14, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
                {
                    float d_m = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, main_x, main_y);
                    float w_m = walk(main_x, main_y, gx, gy);
                    if (d_m < 28.0f || w_m > com.oddlabs.tt.player.fable.BasePlan.detourLimit(d_m))
                    {
                        return java.lang.Float.NEGATIVE_INFINITY;
                    }
                    float d_i = ironDistanceMetres(gx, gy, 8, 40);
                    float d_w = meanDistanceMetres(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx, gy, 20, 30);
                    return (float)((float)(-((float)((float)((float)(0.6f * d_i) + (float)(0.9f * d_w)) + (float)(0.08f * w_m)))) + ((nearWater(gx, gy, 8) ? (float)(-15.0f) : 0.0f)));
                }));
                if (s != null && (best == null || s.score() > best.score()))
                {
                    best = s;
                }
            }
            return best;
        }
        // Java: 281
        public float armoryCost(int gx, int gy, float dist_start)
        {
            float d_w = meanDistanceMetres(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx, gy, 40, 40);
            float d_i = ironDistanceMetres(gx, gy, 15, java.lang.JC.f2i(((float)(@params.site_iron_radius * 2))));
            int iron_near = resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, gx, gy, java.lang.JC.f2i(@params.site_iron_radius));
            float cost = (float)((float)((float)(116.0f + (float)(@params.site_w_wood * d_w)) + (float)(@params.site_w_iron * d_i)) + (float)(@params.site_w_start * dist_start));
            if (iron_near < @params.site_min_iron_nodes)
            {
                cost = (float)(cost + (float)((float)(@params.site_penalty_few_iron * (@params.site_min_iron_nodes - iron_near)) / ((float)(@params.site_min_iron_nodes))));
            }
            if (nearWater(gx, gy, 8))
            {
                cost = (float)(cost + 15.0f);
            }
            cost = (float)(cost - (float)(0.2f * java.lang.Math.clamp((float)(sites.height(gx, gy) - sites.height(start_x, start_y)), (float)(-20.0f), 20.0f)));
            return cost;
        }
        // Java: 296
        public float meanDistanceMetres(com.oddlabs.tt.player.fable.ResourceMap.Kind kind, int gx, int gy, int n, int radius)
        {
            java.util.List nodes = resources.nearestAlive(kind, gx, gy, radius, n);
            if (nodes.isEmpty())
            {
                return (float)((float)(radius * 2.0f) * 2.0f);
            }
            float sum = 0.0f;
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node node in java.lang.JIter.of(nodes))
            {
                sum = (float)(sum + (float)(node.distanceTo(gx, gy) * 2.0f));
            }
            sum = (float)(sum + (float)((n - nodes.size()) * radius * 2.0f));
            return (float)(sum / n);
        }
        // Java: 309
        public float ironDistanceMetres(int gx, int gy, int n, int radius)
        {
            java.util.List nodes = resources.nearestAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON, gx, gy, radius, n);
            if (nodes.isEmpty())
            {
                return (float)((float)(radius * 2.0f) * 2.0f);
            }
            float sum = 0.0f;
            foreach (com.oddlabs.tt.player.fable.ResourceMap.Node node in java.lang.JIter.of(nodes))
            {
                float d = (float)(node.distanceTo(gx, gy) * 2.0f);
                if (ownership(node.grid_x, node.grid_y) < 0.5f)
                {
                    d = (float)(d * @params.site_enemy_iron_factor);
                }
                sum = (float)(sum + d);
            }
            sum = (float)(sum + (float)((n - nodes.size()) * radius * 2.0f));
            return (float)(sum / n);
        }
        // Java: 325
        public float ownership(int gx, int gy)
        {
            float ds = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, start_x, start_y);
            float de = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, enemy_x, enemy_y);
            return (float)(de / java.lang.Math.max(1.0f, (float)(ds + de)));
        }
        // Java: 331
        public bool nearWater(int gx, int gy, int r)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = player.getWorld().getUnitGrid();
            for (int y = gy - r; y <= gy + r; y += 2)
            {
                for (int x = gx - r; x <= gx + r; x += 2)
                {
                    if (x >= 0 && y >= 0 && x < grid_size && y < grid_size && grid.isWater(x, y))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 342
        public void chooseFirstQuartersSite()
        {
            com.oddlabs.tt.player.fable.SiteFinder.Site best = sites.best(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, start_x, start_y, 24, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
            {
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, start_x, start_y);
                if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, armory_x, armory_y) < @params.building_spacing)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                int trees = resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx, gy, 10);
                if (trees < 3 || !walkable(gx, gy))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                return (float)((float)(-d) + (float)(0.4f * java.lang.Math.min(trees, 12)));
            }));
            if (best == null)
            {
                q1_x = start_x;
                q1_y = start_y;
            }
            else
            {
                q1_x = best.gx();
                q1_y = best.gy();
            }
        }
        // Java: 362
        public void chooseFallbackSite()
        {
            int ax = java.lang.Math.round((float)(q1_x - (float)(ux * 22)));
            int ay = java.lang.Math.round((float)(q1_y - (float)(uy * 22)));
            com.oddlabs.tt.player.fable.SiteFinder.Site best = sites.best(com.oddlabs.tt.model.Race.BUILDING_ARMORY, ax, ay, 16, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, q1_x, q1_y) < @params.building_spacing || !walkable(gx, gy))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                return (float)((float)(-com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, ax, ay)) - (float)(0.3f * meanDistanceMetres(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx, gy, 20, 25)));
            }));
            if (best != null)
            {
                fallback_x = best.gx();
                fallback_y = best.gy();
            }
        }
        // Java: 381
        public com.oddlabs.tt.player.fable.SiteFinder.Site nextQuartersSite(int anchor_x, int anchor_y, java.util.List reserved)
        {
            int min_d = @params.quarters_min_dist;
            int max_d = @params.quarters_max_dist;
            com.oddlabs.tt.player.fable.SiteFinder.Site best = sites.best(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, anchor_x, anchor_y, max_d + 4, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
            {
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, anchor_x, anchor_y);
                if (d < min_d)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                if (tooClose(gx, gy, reserved))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                float dot = (float)((float)((gx - anchor_x) * ux) + (float)((gy - anchor_y) * uy));
                int live = resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx, gy, 10);
                if (live < 3 || !walkable(gx, gy))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                float trees = java.lang.Math.min(live, 12);
                float score = (float)((float)((float)(-d) + (float)(0.5f * trees)) - ((dot > 0 ? (float)(0.5f * dot) : 0.0f)));
                if (d > max_d)
                {
                    score = (float)(score - (float)(((float)(d - max_d)) * 2.0f));
                }
                return score;
            }));
            if (best == null)
            {
                best = sites.best(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, start_x, start_y, 40, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx_2, int gy_2) =>
                {
                    return (tooClose(gx_2, gy_2, reserved) || resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx_2, gy_2, 10) < 3 || !walkable(gx_2, gy_2) ? java.lang.Float.NEGATIVE_INFINITY : (float)(-com.oddlabs.tt.player.fable.BasePlan.dist(gx_2, gy_2, anchor_x, anchor_y)));
                }));
            }
            if (best == null)
            {
                best = sites.best(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, start_x, start_y, 40, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx_3, int gy_3) =>
                {
                    return (tooClose(gx_3, gy_3, reserved) || !walkable(gx_3, gy_3) ? java.lang.Float.NEGATIVE_INFINITY : (float)(-com.oddlabs.tt.player.fable.BasePlan.dist(gx_3, gy_3, anchor_x, anchor_y)));
                }));
            }
            if (best == null)
            {
                best = sites.best(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, anchor_x, anchor_y, 75, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx_4, int gy_4) =>
                {
                    return (tooClose(gx_4, gy_4, reserved) || resources.countAlive(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE, gx_4, gy_4, 10) < 3 || walk(anchor_x, anchor_y, gx_4, gy_4) > com.oddlabs.tt.player.fable.BasePlan.detourLimit(com.oddlabs.tt.player.fable.BasePlan.dist(gx_4, gy_4, anchor_x, anchor_y)) ? java.lang.Float.NEGATIVE_INFINITY : (float)(-walk(anchor_x, anchor_y, gx_4, gy_4)));
                }));
            }
            if (best == null)
            {
                best = sites.best(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, anchor_x, anchor_y, 75, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx_5, int gy_5) =>
                {
                    return (tooClose(gx_5, gy_5, reserved) || walk(anchor_x, anchor_y, gx_5, gy_5) > com.oddlabs.tt.player.fable.BasePlan.detourLimit(com.oddlabs.tt.player.fable.BasePlan.dist(gx_5, gy_5, anchor_x, anchor_y)) ? java.lang.Float.NEGATIVE_INFINITY : (float)(-walk(anchor_x, anchor_y, gx_5, gy_5)));
                }));
            }
            return best;
        }
        public static float[] RING_BEARINGS;
        // Java: 436
        public com.oddlabs.tt.player.fable.SiteFinder.Site nextTowerSite(int anchor_x, int anchor_y, int index, java.util.List existing_towers, java.util.List reserved)
        {
            float bearing = com.oddlabs.tt.player.fable.BasePlan.RING_BEARINGS[index % com.oddlabs.tt.player.fable.BasePlan.RING_BEARINGS.Length];
            float @base = ((float)(java.lang.StrictMath.atan2(uy, ux)));
            float a = (float)(@base + ((float)(java.lang.StrictMath.toRadians(bearing))));
            int r = @params.tower_ring_radius + (index / com.oddlabs.tt.player.fable.BasePlan.RING_BEARINGS.Length) * 6;
            int ix = java.lang.Math.round((float)(anchor_x + (float)(r * ((float)(java.lang.StrictMath.cos(a))))));
            int iy = java.lang.Math.round((float)(anchor_y + (float)(r * ((float)(java.lang.StrictMath.sin(a))))));
            float h0 = sites.height(anchor_x, anchor_y);
            return sites.best(com.oddlabs.tt.model.Race.BUILDING_TOWER, ix, iy, 7, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, anchor_x, anchor_y) < 7.0f)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                foreach (int[] t in java.lang.JIter.of(existing_towers))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, t[0], t[1]) < @params.tower_spacing_min)
                    {
                        return java.lang.Float.NEGATIVE_INFINITY;
                    }
                }
                foreach (int[] b in java.lang.JIter.of(reserved))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, b[0], b[1]) < 7.0f)
                    {
                        return java.lang.Float.NEGATIVE_INFINITY;
                    }
                }
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, ix, iy);
                if (walk(anchor_x, anchor_y, gx, gy) > com.oddlabs.tt.player.fable.BasePlan.detourLimit(com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, anchor_x, anchor_y)))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                float height = java.lang.Math.clamp((float)(sites.height(gx, gy) - h0), (float)(-20.0f), 20.0f);
                return (float)((float)(-d) + (float)(0.4f * height));
            }));
        }
        // Java: 463
        public com.oddlabs.tt.player.fable.SiteFinder.Site towerSiteNear(int px, int py, int radius, java.util.List reserved, int min_dist_from_point)
        {
            return sites.best(com.oddlabs.tt.model.Race.BUILDING_TOWER, px, py, radius, island, new com.oddlabs.tt.player.fable.SiteFinder.Scorer_L((int gx, int gy) =>
            {
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, px, py);
                if (d < min_dist_from_point)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                foreach (int[] b in java.lang.JIter.of(reserved))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, b[0], b[1]) < @params.tower_spacing_min)
                    {
                        return java.lang.Float.NEGATIVE_INFINITY;
                    }
                }
                if (walk(px, py, gx, gy) > com.oddlabs.tt.player.fable.BasePlan.detourLimit(d))
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                return (float)((float)(-d) + (float)(0.3f * java.lang.Math.clamp((float)(sites.height(gx, gy) - sites.height(px, py)), (float)(-20.0f), 20.0f)));
            }));
        }
        // Java: 480
        public bool tooClose(int gx, int gy, java.util.List reserved)
        {
            foreach (int[] b in java.lang.JIter.of(reserved))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist(gx, gy, b[0], b[1]) < @params.building_spacing)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 487
        public static float dist(int ax, int ay, int bx, int by)
        {
            float dx = ax - bx;
            float dy = ay - by;
            return ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
        }
        // Java: 493
        public static int dist2(int ax, int ay, int bx, int by)
        {
            int dx = ax - bx;
            int dy = ay - by;
            return dx * dx + dy * dy;
        }
        // Java: 500
        public int[] along(int x, int y, float cells)
        {
            return new int[] { java.lang.Math.clamp(java.lang.Math.round((float)(x + (float)(ux * cells))), 0, grid_size - 1), java.lang.Math.clamp(java.lang.Math.round((float)(y + (float)(uy * cells))), 0, grid_size - 1) };
        }
        // Java: 506
        public int[] toward(int x, int y, int tx, int ty, float cells)
        {
            float dx = tx - x;
            float dy = ty - y;
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < 0.5f)
            {
                return new int[] { x, y };
            }
            return new int[] { java.lang.Math.clamp(java.lang.Math.round((float)(x + (float)((float)(dx / len) * cells))), 0, grid_size - 1), java.lang.Math.clamp(java.lang.Math.round((float)(y + (float)((float)(dy / len) * cells))), 0, grid_size - 1) };
        }
        // Java: 516
        public bool sameIsland(int gx, int gy)
        {
            return gx >= 0 && gy >= 0 && gx < grid_size && gy < grid_size && map.getIslandId(gx, gy) == island;
        }
        // Java: 520
        public static int[] centre(com.oddlabs.tt.model.LandBuilding b)
        {
            return new int[] { b.getGridX(), b.getGridY() };
        }
        static BasePlan()
        {
            BasePlan.RING_BEARINGS = new float[] { 0.0f, 40.0f, (float)(-40.0f), 180.0f, 90.0f, (float)(-90.0f), 135.0f, (float)(-135.0f), 20.0f, (float)(-20.0f), 160.0f, (float)(-160.0f) };
        }
        private void init_BasePlan()
        {
            this.fallback_x = (-1);
            this.fallback_y = (-1);
            this.walk_cache = new java.util.LinkedHashMap();
        }
        public BasePlan(com.oddlabs.tt.player.Player player, com.oddlabs.tt.player.fable.ResourceMap resources, com.oddlabs.tt.player.fable.SiteFinder sites, com.oddlabs.tt.player.fable.Params @params, com.oddlabs.tt.player.fable.AiLog log) : base()
        {
            init_BasePlan();
            this.player = player;
            this.resources = resources;
            this.sites = sites;
            this.@params = @params;
            this.log = log;
            this.map = player.getWorld().getHeightMap();
            this.grid_size = player.getWorld().getUnitGrid().getGridSize();
        }
    }
}

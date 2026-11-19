// Translated from Decoys.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 37
    public class Decoys : java.lang.Object
    {
        public const int MIN_TOWER_CELLS = 11;
        public const int MAX_TOWER_CELLS = 14;
        public const float TOWER_REACH = 15.5f;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.List decoys;
        public float last_tick;
        public float nospot_trace;
        // Java: 49
        public class Decoy : java.lang.Object
        {
            public com.oddlabs.tt.player.Player target;
            public int x;
            public int y;
            public com.oddlabs.tt.model.Building site;
            public com.oddlabs.tt.model.Unit runner;
            public float ordered;
            public bool home;
            public Decoy(com.oddlabs.tt.player.Player target, int x, int y, float ordered) : base()
            {
                this.target = target;
                this.x = x;
                this.y = y;
                this.ordered = ordered;
            }
        }
        // Java: 74
        public bool isDecoy(com.oddlabs.tt.model.Building b)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Decoys.Decoy d in java.lang.JIter.of(decoys))
            {
                if (((object)d.site == (object)b))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 82
        public bool isRunner(com.oddlabs.tt.model.Unit u)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Decoys.Decoy d in java.lang.JIter.of(decoys))
            {
                if (((object)d.runner == (object)u))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 89
        public int count()
        {
            return decoys.size();
        }
        // Java: 93
        public void tick()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if ((!strategy.decoys && decoys.isEmpty()) || (float)(ai.time() - last_tick) < 1.0f)
            {
                return;
            }
            last_tick = ai.time();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            release();
            if (!strategy.decoys)
            {
                return;
            }
            if (ai.time() < strategy.decoy_time)
            {
                return;
            }
            java.util.List active = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                if (com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t))
                {
                    active.add(t);
                }
            }
            if (active.isEmpty())
            {
                return;
            }
            foreach (com.oddlabs.tt.player.Player p in ai.owner().getWorld().getPlayers())
            {
                if (!ai.owner().isEnemy(p) || !p.isAlive())
                {
                    continue;
                }
                int[] origin = com.oddlabs.tt.player.gauntlet.Decoys.waveOrigin(p);
                if (origin == null)
                {
                    continue;
                }
                steer(p, origin, active);
            }
        }
        // Java: 121
        public void release()
        {
            for (java.util.Iterator it = decoys.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.gauntlet.Decoys.Decoy d = ((com.oddlabs.tt.player.gauntlet.Decoys.Decoy)it.next());
                if (d.site != null && d.site.isDead())
                {
                    ai.aiLog().count("decoy_razed");
                    ai.log("decoy razed at " + (d.x) + "," + (d.y) + " (for " + java.lang.JStr.valueOf(d.target.getPlayerInfo().getName()) + ")");
                    it.remove();
                    continue;
                }
                if (d.runner != null && d.runner.isDead())
                {
                    d.runner = null;
                }
                if (d.site == null || (!d.site.isPlaced() && d.runner == null))
                {
                    if (d.runner == null || (float)(ai.time() - d.ordered) > ((d.home ? 200.0f : 90.0f)))
                    {
                        if (d.runner != null && !d.runner.isDead())
                        {
                            sendHome(d.runner);
                        }
                        it.remove();
                    }
                    continue;
                }
                if (d.site.isPlaced() && d.runner != null)
                {
                    sendHome(d.runner);
                    d.runner = null;
                }
                if (!d.target.isAlive())
                {
                    it.remove();
                }
            }
        }
        // Java: 151
        public void sendHome(com.oddlabs.tt.model.Unit runner)
        {
            com.oddlabs.tt.model.Building home = ai.intel().armory();
            if (home == null && !ai.intel().quarters.isEmpty())
            {
                home = ((com.oddlabs.tt.model.Building)ai.intel().quarters.getFirst());
            }
            if (home != null && !home.isDead())
            {
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(runner), home, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
        }
        // Java: 160
        public static int[] waveOrigin(com.oddlabs.tt.player.Player p)
        {
            com.oddlabs.tt.model.Building armory = null;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if (s.isDead())
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Unit u))
                {
                    if (u.isMounted() || !((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)))
                    {
                        continue;
                    }
                    if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) || u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        continue;
                    }
                    if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                    {
                        return new int[] { u.getGridX(), u.getGridY() };
                    }
                }
                else if (armory == null && (s is com.oddlabs.tt.model.Building b) && b.isComplete() && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    armory = b;
                }
            }
            if (armory != null)
            {
                return new int[] { armory.getGridX(), armory.getGridY() };
            }
            return null;
        }
        // Java: 182
        public void steer(com.oddlabs.tt.player.Player p, int[] origin, java.util.List active)
        {
            int ox = origin[0];
            int oy = origin[1];
            int real = nearestReal(ox, oy);
            foreach (com.oddlabs.tt.player.gauntlet.Decoys.Decoy d in java.lang.JIter.of(decoys))
            {
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(d.x, d.y, ox, oy);
                if (d2 < real)
                {
                    return;
                }
            }
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (decoys.size() >= strategy.decoy_max)
            {
                return;
            }
            int cap = ai.owner().getWorld().getMaxBuildingCount();
            if (ai.owner().getBuildingCountContainer().getNumSupplies() + strategy.decoy_free_slots >= cap)
            {
                return;
            }
            int[] spot = findSpot(ox, oy, real, active);
            if (spot == null)
            {
                ai.aiLog().count("decoy_nospot");
                if (ai.logging() && (float)(ai.time() - nospot_trace) >= 15.0f)
                {
                    nospot_trace = ai.time();
                    com.oddlabs.tt.model.Building nb = null;
                    int nd = java.lang.Integer.MAX_VALUE;
                    foreach (com.oddlabs.tt.model.Selectable sel in java.lang.JIter.of(ai.owner().getUnits().getSet()))
                    {
                        if ((sel is com.oddlabs.tt.model.Building b) && !b.isDead() && !isDecoy(b) && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
                        {
                            int d2_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), ox, oy);
                            if (d2_2 < nd)
                            {
                                nd = d2_2;
                                nb = b;
                            }
                        }
                    }
                    int td = java.lang.Integer.MAX_VALUE;
                    foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(active))
                    {
                        if (nb != null)
                        {
                            td = java.lang.Math.min(td, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), nb.getGridX(), nb.getGridY()));
                        }
                    }
                    com.oddlabs.tt.model.Building fb = nb;
                    int fnd = nd;
                    int ftd = td;
                    ai.log("decoy: no spot for " + java.lang.JStr.valueOf(p.getPlayerInfo().getName()) + " origin " + (ox) + "," + (oy) + ", nearest " + java.lang.JStr.valueOf(((fb == null ? "none" : (((com.oddlabs.tt.model.BuildingTemplate)fb.getTemplate()).getTemplateID()) + java.lang.JStr.valueOf(((fb.isComplete() ? "" : " site"))) + " at " + (fb.getGridX()) + "," + (fb.getGridY()) + " (" + (java.lang.JC.d2i(java.lang.Math.sqrt(fnd))) + " cells), nearest active tower to it " + (java.lang.JC.d2i(java.lang.Math.sqrt(ftd))) + " cells"))));
                }
                return;
            }
            com.oddlabs.tt.model.Unit runner = chooseRunner(spot[0], spot[1]);
            if (runner == null)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Decoys.Decoy d_2 = new com.oddlabs.tt.player.gauntlet.Decoys.Decoy(p, spot[0], spot[1], ai.time());
            d_2.runner = runner;
            d_2.site = ai.placeSite(java.util.List_.of(runner), com.oddlabs.tt.model.Race.BUILDING_TOWER, spot[0], spot[1]);
            if (d_2.site == null)
            {
                return;
            }
            decoys.add(d_2);
            ai.intel().peon_states.put(runner, com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD);
            ai.aiLog().count("decoy_placed");
            ai.log("decoy for " + java.lang.JStr.valueOf(p.getPlayerInfo().getName()) + " at " + (spot[0]) + "," + (spot[1]) + " (origin " + (ox) + "," + (oy) + ")");
        }
        public java.util.Map home_tried;
        // Java: 249
        public void placeHome(com.oddlabs.tt.player.Player p, int ox, int oy)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            float? tried = ((float?)home_tried.get(p));
            if (tried != null && (float)(ai.time() - ((float)tried)) < 20.0f)
            {
                return;
            }
            home_tried.put(p, ai.time());
            int homes = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Decoys.Decoy d in java.lang.JIter.of(decoys))
            {
                if (!d.home)
                {
                    continue;
                }
                if (((object)d.target == (object)p))
                {
                    return;
                }
                homes++;
            }
            if (homes >= strategy.site_max)
            {
                return;
            }
            int cap = ai.owner().getWorld().getMaxBuildingCount();
            if (ai.owner().getBuildingCountContainer().getNumSupplies() + 2 >= cap)
            {
                return;
            }
            int real = nearestReal(ox, oy);
            int[] spot = findHomeSpot(p, ox, oy, real);
            if (spot == null)
            {
                ai.aiLog().count("site_nospot");
                return;
            }
            com.oddlabs.tt.model.Unit runner = chooseRunner(spot[0], spot[1], 400);
            if (runner == null)
            {
                ai.aiLog().count("site_norunner");
                return;
            }
            com.oddlabs.tt.player.gauntlet.Decoys.Decoy d_2 = new com.oddlabs.tt.player.gauntlet.Decoys.Decoy(p, spot[0], spot[1], ai.time());
            d_2.home = true;
            d_2.runner = runner;
            d_2.site = ai.placeSite(java.util.List_.of(runner), com.oddlabs.tt.model.Race.BUILDING_TOWER, spot[0], spot[1]);
            if (d_2.site == null)
            {
                return;
            }
            decoys.add(d_2);
            ai.intel().peon_states.put(runner, com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD);
            ai.aiLog().count("site_placed");
            ai.log("site shepherd for " + java.lang.JStr.valueOf(p.getPlayerInfo().getName()) + " at " + (spot[0]) + "," + (spot[1]) + " (origin " + (ox) + "," + (oy) + ")");
        }
        // Java: 296
        public int[] findHomeSpot(com.oddlabs.tt.player.Player p, int ox, int oy, int real)
        {
            com.oddlabs.tt.model.BuildingTemplate template = ai.owner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int sx = ai.planner().getStartX();
            int sy = ai.planner().getStartY();
            float limit = (float)((float)(0.8f * 0.8f) * real);
            int[] best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            for (int r = 14; r <= 24; r += 2)
            {
                for (int a = 0; a < 24; a++)
                {
                    double ang = a * java.lang.Math.PI / 12;
                    int x = ox + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.cos(ang))));
                    int y = oy + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.sin(ang))));
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, ox, oy) >= limit)
                    {
                        continue;
                    }
                    if (!ai.map().inside(x, y) || !ai.planner().getStartField().reachable(x, y))
                    {
                        continue;
                    }
                    bool ok = true;
                    foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_warriors, intel.enemy_chieftains, intel.enemy_peons)))
                    {
                        foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(group))
                        {
                            if (!e.isDead() && java.lang.Math.abs(e.getGridX() - x) <= 10 && java.lang.Math.abs(e.getGridY() - y) <= 10)
                            {
                                ok = false;
                                break;
                            }
                        }
                    }
                    if (!ok)
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
                    {
                        if (b.isDead())
                        {
                            continue;
                        }
                        int keep = (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER ? 19 : (((object)b.getOwner() == (object)p) ? 17 : 0));
                        if (keep > 0 && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, b.getGridX(), b.getGridY()) <= keep * keep)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (!ok || !clearOfDecoys(x, y, 6) || !ai.map().canPlace(template, x, y))
                    {
                        continue;
                    }
                    float score = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, sx, sy))));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = new int[] { x, y };
                    }
                }
            }
            return best;
        }
        // Java: 345
        public int nearestReal(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if (!((s is com.oddlabs.tt.model.Building b)) || b.isDead() || isDecoy(b))
                {
                    continue;
                }
                if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getType() != com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
                {
                    continue;
                }
                best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y));
            }
            return best;
        }
        // Java: 362
        public int[] findSpot(int ox, int oy, int real, java.util.List active)
        {
            com.oddlabs.tt.model.BuildingTemplate template = ai.owner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List own = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getType() == com.oddlabs.tt.model.BuildingTemplate.TYPE_BUILDING)
                {
                    own.add(b);
                }
            }
            float limit = (float)((float)(ai.strategy().decoy_margin * ai.strategy().decoy_margin) * real);
            int[] best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            int far = 0;
            int crowded = 0;
            int hot = 0;
            int illegal = 0;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(active))
            {
                for (int r = com.oddlabs.tt.player.gauntlet.Decoys.MIN_TOWER_CELLS; r <= com.oddlabs.tt.player.gauntlet.Decoys.MAX_TOWER_CELLS; r++)
                {
                    for (int a = 0; a < 24; a++)
                    {
                        double ang = a * java.lang.Math.PI / 12;
                        int x = t.getGridX() + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.cos(ang))));
                        int y = t.getGridY() + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.sin(ang))));
                        int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, ox, oy);
                        if (d2 >= limit)
                        {
                            far++;
                            continue;
                        }
                        if (!com.oddlabs.tt.player.gauntlet.Decoys.clearOf(own, x, y, 10) || !clearOfDecoys(x, y, 4))
                        {
                            crowded++;
                            continue;
                        }
                        if (com.oddlabs.tt.player.gauntlet.Decoys.enemyNear(intel, x, y, 12))
                        {
                            hot++;
                            continue;
                        }
                        if (!ai.map().canPlace(template, x, y))
                        {
                            illegal++;
                            continue;
                        }
                        int reach = 0;
                        foreach (com.oddlabs.tt.model.Building o in java.lang.JIter.of(active))
                        {
                            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(o.getGridX(), o.getGridY(), x, y) <= (float)(com.oddlabs.tt.player.gauntlet.Decoys.TOWER_REACH * com.oddlabs.tt.player.gauntlet.Decoys.TOWER_REACH))
                            {
                                reach++;
                            }
                        }
                        float score = (float)((float)(reach * 100.0f) - ((float)(java.lang.Math.sqrt(d2))));
                        if (score > best_score)
                        {
                            best_score = score;
                            best = new int[] { x, y };
                        }
                    }
                }
            }
            if (best == null)
            {
                int m = java.lang.Math.max(java.lang.Math.max(far, crowded), java.lang.Math.max(hot, illegal));
                ai.aiLog().count((m == far ? "decoy_nospot_far" : (m == crowded ? "decoy_nospot_crowded" : (m == hot ? "decoy_nospot_hot" : "decoy_nospot_illegal"))));
            }
            return best;
        }
        // Java: 420
        public static bool clearOf(java.util.List own, int x, int y, int cells)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
            {
                if (java.lang.Math.abs(b.getGridX() - x) <= cells && java.lang.Math.abs(b.getGridY() - y) <= cells)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 427
        public bool clearOfDecoys(int x, int y, int cells)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Decoys.Decoy d in java.lang.JIter.of(decoys))
            {
                if (java.lang.Math.abs(d.x - x) <= cells && java.lang.Math.abs(d.y - y) <= cells)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 434
        public static bool enemyNear(com.oddlabs.tt.player.gauntlet.Intel intel, int x, int y, int cells)
        {
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y) <= cells * cells)
                {
                    return true;
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_chieftains))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), x, y) <= cells * cells)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 445
        public com.oddlabs.tt.model.Unit chooseRunner(int x, int y)
        {
            return chooseRunner(x, y, 70);
        }
        // Java: 449
        public com.oddlabs.tt.model.Unit chooseRunner(int x, int y, int range)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Unit best = null;
            int best_d = range * range;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if (((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON))
                {
                    continue;
                }
                if (isRunner(p) || intel.shepherds.contains(p) || intel.lures.contains(p) || com.oddlabs.tt.player.gauntlet.Decoys.enemyNear(intel, p.getGridX(), p.getGridY(), 10) || ai.economy().reservedPlacer(p))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), x, y);
                if (d < best_d)
                {
                    best_d = d;
                    best = p;
                }
            }
            return best;
        }
        // Java: 475
        public bool caged(com.oddlabs.tt.model.Unit e)
        {
            if (!ai.strategy().decoys || !ai.strategy().decoy_cage || e.isDead())
            {
                return false;
            }
            com.oddlabs.tt.model.behaviour.Controller current = e.getCurrentController();
            if ((current is com.oddlabs.tt.model.behaviour.HuntController) || (current is com.oddlabs.tt.model.behaviour.AttackController))
            {
                return false;
            }
            int x = e.getGridX();
            int y = e.getGridY();
            bool reach = false;
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y);
                if (java.lang.Math.abs(t.getGridX() - x) <= 9 && java.lang.Math.abs(t.getGridY() - y) <= 9)
                {
                    return false;
                }
                if (d2 <= (float)(com.oddlabs.tt.player.gauntlet.Decoys.TOWER_REACH * com.oddlabs.tt.player.gauntlet.Decoys.TOWER_REACH) && com.oddlabs.tt.player.gauntlet.Intel.isTowerActive(t))
                {
                    reach = true;
                }
            }
            if (!reach)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.quarters))
            {
                if (java.lang.Math.abs(b.getGridX() - x) <= 9 && java.lang.Math.abs(b.getGridY() - y) <= 9)
                {
                    return false;
                }
            }
            foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(intel.armories))
            {
                if (java.lang.Math.abs(b_2.getGridX() - x) <= 9 && java.lang.Math.abs(b_2.getGridY() - y) <= 9)
                {
                    return false;
                }
            }
            return true;
        }
        private void init_Decoys()
        {
            this.decoys = new java.util.ArrayList();
            this.last_tick = (float)(-10.0f);
            this.nospot_trace = (float)(-100.0f);
            this.home_tried = new java.util.LinkedHashMap();
        }
        public Decoys(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Decoys();
            this.ai = ai;
        }
    }
}

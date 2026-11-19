// Translated from Lures.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 28
    public class Lures : java.lang.Object
    {
        // Java: 29
        public sealed class Phase : java.lang.Enum
        {
            public static Phase[] values() { return (Phase[])VALUES_.Clone(); }
            public static Phase valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Phase APPROACH = new Phase("APPROACH", 0);
            public static readonly Phase BAIT = new Phase("BAIT", 1);
            public static readonly Phase RUN = new Phase("RUN", 2);
            public static readonly Phase[] VALUES_ = new Phase[] { APPROACH, BAIT, RUN };
            public Phase(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 35
        public class Lure : java.lang.Object
        {
            public com.oddlabs.tt.model.Unit peon;
            public com.oddlabs.tt.model.Building tower;
            public com.oddlabs.tt.model.Building refuge;
            public int bait_x;
            public int bait_y;
            public int[][] route;
            public int leg;
            public com.oddlabs.tt.player.gauntlet.Lures.Phase phase;
            public float since;
            public bool entering;
            public int hunters;
            private void init_Lure()
            {
                this.phase = com.oddlabs.tt.player.gauntlet.Lures.Phase.APPROACH;
            }
            public Lure(com.oddlabs.tt.model.Unit peon, com.oddlabs.tt.model.Building tower, com.oddlabs.tt.model.Building refuge, int bait_x, int bait_y, int[][] route, float since) : base()
            {
                init_Lure();
                this.peon = peon;
                this.tower = tower;
                this.refuge = refuge;
                this.bait_x = bait_x;
                this.bait_y = bait_y;
                this.route = route;
                this.since = since;
            }
        }
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.List lures;
        public java.util.List recent;
        public float last_plan;
        // Java: 73
        public void guard()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!strategy.lure)
            {
                return;
            }
            for (java.util.Iterator it = lures.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.gauntlet.Lures.Lure l = ((com.oddlabs.tt.player.gauntlet.Lures.Lure)it.next());
                if (l.peon.isDead())
                {
                    bool home = l.entering && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(l.peon.getGridX(), l.peon.getGridY(), l.refuge.getGridX(), l.refuge.getGridY()) <= 6 * 6;
                    ai.aiLog().count((home ? "lure_home" : "lure_lost"));
                    for (int i = 0; i < l.hunters; i++)
                    {
                        ai.aiLog().count("lure_hunters");
                    }
                    ai.log("lure " + java.lang.JStr.valueOf(((home ? "home" : "lost"))) + " after drawing " + (l.hunters) + " (" + java.lang.JStr.valueOf(l.phase) + ")");
                    ai.intel().lures.remove(l.peon);
                    it.remove();
                    continue;
                }
                if (!step(l))
                {
                    release(l);
                    it.remove();
                }
            }
            if ((float)(ai.time() - last_plan) >= 1.0f)
            {
                last_plan = ai.time();
                plan();
            }
        }
        // Java: 101
        public void release(com.oddlabs.tt.player.gauntlet.Lures.Lure l)
        {
            ai.intel().lures.remove(l.peon);
            if (!l.peon.isDead() && !l.refuge.isDead())
            {
                ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(l.peon), l.refuge, com.oddlabs.tt.model.Action.MOVE, false);
            }
        }
        // Java: 108
        public bool step(com.oddlabs.tt.player.gauntlet.Lures.Lure l)
        {
            com.oddlabs.tt.model.Unit p = l.peon;
            int px = p.getGridX();
            int py = p.getGridY();
            if (l.tower.isDead() || l.refuge.isDead())
            {
                return false;
            }
            int hunters = huntersOf(p);
            switch ((l.phase).ordinal())
            {
                case 0 /*APPROACH*/:
                {
                    if (hunters > 0 || nearestEnemy(px, py) <= 9 * 9)
                    {
                        run(l, hunters);
                    }
                    else if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(px, py, l.bait_x, l.bait_y) <= 2 * 2)
                    {
                        l.phase = com.oddlabs.tt.player.gauntlet.Lures.Phase.BAIT;
                        l.since = ai.time();
                    }
                    else if ((float)(ai.time() - l.since) > 60.0f)
                    {
                        ai.aiLog().count("lure_timeout");
                        return false;
                    }
                    break;
                }
                case 1 /*BAIT*/:
                {
                    if (hunters > 0 || nearestEnemy(px, py) <= 8 * 8)
                    {
                        run(l, hunters);
                    }
                    else if ((float)(ai.time() - l.since) > 8.0f)
                    {
                        ai.aiLog().count("lure_ignored");
                        return false;
                    }
                    break;
                }
                case 2 /*RUN*/:
                {
                    l.hunters = java.lang.Math.max(l.hunters, hunters);
                    if (!l.entering && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(px, py, l.route[l.leg][0], l.route[l.leg][1]) <= 3 * 3)
                    {
                        if (l.leg + 1 < l.route.Length)
                        {
                            l.leg++;
                            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(p), l.route[l.leg][0], l.route[l.leg][1], com.oddlabs.tt.model.Action.MOVE, false);
                        }
                        else
                        {
                            l.entering = true;
                            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(p), l.refuge, com.oddlabs.tt.model.Action.MOVE, false);
                        }
                    }
                    if ((float)(ai.time() - l.since) > 90.0f)
                    {
                        return false;
                    }
                    break;
                }
            }
            return true;
        }
        // Java: 154
        public void run(com.oddlabs.tt.player.gauntlet.Lures.Lure l, int hunters)
        {
            l.phase = com.oddlabs.tt.player.gauntlet.Lures.Phase.RUN;
            l.since = ai.time();
            l.hunters = hunters;
            ai.aiLog().count("lure_run");
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(l.peon), l.route[0][0], l.route[0][1], com.oddlabs.tt.model.Action.MOVE, false);
        }
        // Java: 163
        public int huntersOf(com.oddlabs.tt.model.Unit u)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (!e.isDead() && (e.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController h) && ((object)h.getTarget() == (object)u))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 172
        public int nearestEnemy(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (!e.isDead())
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()));
                }
            }
            foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(ai.intel().enemy_chieftains))
            {
                if (!e_2.isDead())
                {
                    best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_2.getGridX(), e_2.getGridY()));
                }
            }
            return best;
        }
        // Java: 183
        public void plan()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (lures.size() >= strategy.lure_max || ai.time() < strategy.lure_time)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            recent.removeIf(new java.util.function.Predicate_L((object r_l) =>
            {
                float[] r = (float[])r_l;
                return (float)(ai.time() - r[2]) > 30.0f;
            }));
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
            java.util.List own = intel.finishedBuildings();
            int range2 = strategy.lure_range * strategy.lure_range;
            java.util.List parked = new java.util.ArrayList();
            java.util.List awake = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead())
                {
                    continue;
                }
                if (!com.oddlabs.tt.player.gauntlet.Intel.isParked(e))
                {
                    awake.add(e);
                    continue;
                }
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
                {
                    if (!b.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), e.getGridX(), e.getGridY()) <= range2)
                    {
                        parked.add(e);
                        break;
                    }
                }
            }
            awake.addAll(intel.enemy_chieftains);
            com.oddlabs.tt.model.Unit best_seed = null;
            int best_n = 0;
            com.oddlabs.tt.model.Building best_tower = null;
            foreach (com.oddlabs.tt.model.Unit s in java.lang.JIter.of(parked))
            {
                int sx = s.getGridX();
                int sy = s.getGridY();
                bool fresh = true;
                foreach (float[] r_2 in java.lang.JIter.of(recent))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, java.lang.JC.f2i(r_2[0]), java.lang.JC.f2i(r_2[1])) <= 15 * 15)
                    {
                        fresh = false;
                    }
                }
                if (!fresh)
                {
                    continue;
                }
                com.oddlabs.tt.model.Building tower = null;
                int td = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(active))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, t_2.getGridX(), t_2.getGridY());
                    if (d < td)
                    {
                        td = d;
                        tower = t_2;
                    }
                }
                if (tower == null || td <= 16 * 16 || td > 45 * 45)
                {
                    continue;
                }
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(parked))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, e_2.getGridX(), e_2.getGridY()) <= 8 * 8)
                    {
                        n++;
                    }
                }
                if (n > best_n)
                {
                    best_n = n;
                    best_seed = s;
                    best_tower = tower;
                }
            }
            if (best_seed == null || best_n < strategy.lure_min)
            {
                return;
            }
            com.oddlabs.tt.model.Unit seed = best_seed;
            com.oddlabs.tt.model.Building tower_2 = best_tower;
            int sx_2 = seed.getGridX();
            int sy_2 = seed.getGridY();
            int td2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx_2, sy_2, tower_2.getGridX(), tower_2.getGridY());
            com.oddlabs.tt.model.Building refuge = null;
            int rbest = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(intel.homes()))
            {
                if (b_2.isDead() || !b_2.isComplete() || ai.economy().isEvacuating(b_2))
                {
                    continue;
                }
                int bd2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx_2, sy_2, b_2.getGridX(), b_2.getGridY());
                int tb2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tower_2.getGridX(), tower_2.getGridY(), b_2.getGridX(), b_2.getGridY());
                if (bd2 <= td2 || tb2 > 60 * 60)
                {
                    continue;
                }
                if (tb2 < rbest)
                {
                    rbest = tb2;
                    refuge = b_2;
                }
            }
            if (refuge == null)
            {
                ai.aiLog().count("lure_norefuge");
                return;
            }
            java.util.List blob = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(parked))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx_2, sy_2, e_3.getGridX(), e_3.getGridY()) <= 12 * 12)
                {
                    blob.add(e_3);
                }
            }
            int[] bait = null;
            float bait_score = (float)(-java.lang.Float.MAX_VALUE);
            int bait_seers = 0;
            for (int r_3 = 8; r_3 <= 15; r_3++)
            {
                for (int a = 0; a < 32; a++)
                {
                    double ang = a * java.lang.Math.PI / 16;
                    int x = sx_2 + unchecked((int)(java.lang.Math.round(r_3 * java.lang.StrictMath.cos(ang))));
                    int y = sy_2 + unchecked((int)(java.lang.Math.round(r_3 * java.lang.StrictMath.sin(ang))));
                    if (!ai.map().passable(x, y))
                    {
                        continue;
                    }
                    int seers = 0;
                    bool safe = true;
                    foreach (com.oddlabs.tt.model.Unit e_4 in java.lang.JIter.of(blob))
                    {
                        int dx = java.lang.Math.abs(e_4.getGridX() - x);
                        int dy = java.lang.Math.abs(e_4.getGridY() - y);
                        if (dx <= 8 && dy <= 8)
                        {
                            seers++;
                        }
                        if (dx * dx + dy * dy < 73)
                        {
                            safe = false;
                            break;
                        }
                    }
                    if (seers == 0 || !safe)
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.model.Unit e_5 in java.lang.JIter.of(awake))
                    {
                        if (!e_5.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_5.getGridX(), e_5.getGridY()) <= 10 * 10)
                        {
                            safe = false;
                            break;
                        }
                    }
                    if (!safe)
                    {
                        continue;
                    }
                    int d2t = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, tower_2.getGridX(), tower_2.getGridY());
                    if (d2t >= td2 + 8 * 8)
                    {
                        continue;
                    }
                    float score = (float)((float)(10.0f * seers) - (float)(0.2f * ((float)(java.lang.Math.sqrt(d2t)))));
                    if (score > bait_score)
                    {
                        bait_score = score;
                        bait = new int[] { x, y };
                        bait_seers = seers;
                    }
                }
            }
            if (bait == null)
            {
                ai.aiLog().count("lure_nobait");
                return;
            }
            com.oddlabs.tt.model.Unit peon = recruit(bait[0], bait[1], awake, parked);
            if (peon == null)
            {
                ai.aiLog().count("lure_nopeon");
                return;
            }
            int wx = tower_2.getGridX();
            int wy = tower_2.getGridY();
            float dx_2 = refuge.getGridX() - wx;
            float dy_2 = refuge.getGridY() - wy;
            float len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx_2 * dx_2) + (float)(dy_2 * dy_2))))));
            java.util.List route = new java.util.ArrayList();
            route.add(new int[] { wx + java.lang.Math.round((float)((float)(4 * dx_2) / len)), wy + java.lang.Math.round((float)((float)(4 * dy_2) / len)) });
            int gx = ai.military().stagingX();
            int gy = ai.military().stagingY();
            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(wx, wy, gx, gy) <= com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(wx, wy, refuge.getGridX(), refuge.getGridY()) + 15 * 15 && nearestEnemy(gx, gy) > 12 * 12)
            {
                route.add(new int[] { gx, gy });
            }
            com.oddlabs.tt.player.gauntlet.Lures.Lure l = new com.oddlabs.tt.player.gauntlet.Lures.Lure(peon, tower_2, refuge, bait[0], bait[1], ((int[][])route.toArray(new int[0][])), ai.time());
            for (int i = 0; i < bait_seers; i++)
            {
                ai.aiLog().count("lure_seers");
            }
            lures.add(l);
            intel.lures.add(peon);
            recent.add(new float[] { sx_2, sy_2, ai.time() });
            ai.landscapeOrder(com.oddlabs.tt.model.Selectable.newArray(peon), bait[0], bait[1], com.oddlabs.tt.model.Action.MOVE, false);
            ai.aiLog().count("lure_start");
            ai.log(java.lang.JString.format("lure for %d parked at %d,%d: bait %d,%d, tower %d,%d, refuge %d,%d", best_n, sx_2, sy_2, bait[0], bait[1], tower_2.getGridX(), tower_2.getGridY(), refuge.getGridX(), refuge.getGridY()));
        }
        // Java: 360
        public com.oddlabs.tt.model.Unit recruit(int x, int y, java.util.List awake, java.util.List parked)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Unit best = null;
            int best_d = 60 * 60;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState st = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p));
                if (((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) && ((object)st != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE))
                {
                    continue;
                }
                if (intel.shepherds.contains(p) || intel.lures.contains(p) || p.isDead() || ai.economy().reservedPlacer(p))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), x, y);
                if (d >= best_d)
                {
                    continue;
                }
                bool safe = true;
                foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(awake, parked)))
                {
                    foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(group))
                    {
                        if (!e.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(p.getGridX(), p.getGridY(), e.getGridX(), e.getGridY()) <= 12 * 12)
                        {
                            safe = false;
                            break;
                        }
                    }
                }
                if (!safe)
                {
                    continue;
                }
                best_d = d;
                best = p;
            }
            return best;
        }
        private void init_Lures()
        {
            this.lures = new java.util.ArrayList();
            this.recent = new java.util.ArrayList();
            this.last_plan = (float)(-100.0f);
        }
        public Lures(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Lures();
            this.ai = ai;
        }
    }
}

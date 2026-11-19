// Translated from GauntletAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 33
    public class GauntletAI : com.oddlabs.tt.player.AI
    {
        public const float INTEL_PERIOD = 0.5f;
        public const float ECONOMY_PERIOD = 1.0f;
        public const float PLAN_PERIOD = 3.0f;
        public const float SCAN_PERIOD = 30.0f;
        public com.oddlabs.tt.aikit.AiLog log_f;
        public com.oddlabs.tt.player.gauntlet.Strategy strategy_f;
        public com.oddlabs.tt.player.gauntlet.Intel intel_f;
        public com.oddlabs.tt.player.gauntlet.Reflexes reflexes;
        public com.oddlabs.tt.player.gauntlet.MapAnalysis map_f;
        public com.oddlabs.tt.player.gauntlet.SitePlanner planner_f;
        public com.oddlabs.tt.player.gauntlet.Economy economy_f;
        public com.oddlabs.tt.player.gauntlet.Military military_f;
        public com.oddlabs.tt.player.gauntlet.Chieftain chieftain_f;
        public com.oddlabs.tt.player.gauntlet.Decoys decoys_f;
        public com.oddlabs.tt.player.gauntlet.Shepherd shepherd_f;
        public com.oddlabs.tt.player.gauntlet.Lures lures_f;
        public com.oddlabs.tt.player.gauntlet.Dodges dodges_f;
        public com.oddlabs.tt.player.gauntlet.Jams jams;
        public com.oddlabs.tt.player.gauntlet.Freeze freeze_f;
        public int ticks_f;
        public float time_f;
        public float next_intel;
        public float next_economy;
        public float next_plan;
        public float next_scan;
        public float next_stat;
        public bool initialized;
        public bool logging_f;
        public const int PLAY_GAUNTLET = 0;
        public const int PLAY_EXPERT = 1;
        public const int PLAY_ULTRA = 2;
        public const int PLAY_FABLE = 3;
        public static string[] PLAY_NAMES;
        public const float JUDGE_START = 300.0f;
        public const float JUDGE_PERIOD = 20.0f;
        public const float BEHIND = 2.0f;
        public const float PLAY_MIN = 300.0f;
        public const int BEHIND_JUDGEMENTS = 6;
        public static int forced_opening;
        public int[] order;
        public com.oddlabs.tt.player.AI[] plays;
        public bool[] tried;
        public int play;
        public float play_since;
        public float next_judge;
        public int behind;
        public java.lang.StringBuilder play_trail;
        // Java: 113
        public static int[] openingOrder(com.oddlabs.tt.player.Player owner_v)
        {
            if (com.oddlabs.tt.player.gauntlet.GauntletAI.forced_opening >= 0)
            {
                int[] o = new int[] { com.oddlabs.tt.player.gauntlet.GauntletAI.forced_opening, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_EXPERT, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_ULTRA, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_FABLE };
                int[] r = new int[4];
                int n = 0;
                foreach (int p in o)
                {
                    bool seen = false;
                    for (int i = 0; i < n; i++)
                    {
                        seen |= r[i] == p;
                    }
                    if (!seen)
                    {
                        r[n++] = p;
                    }
                }
                return r;
            }
            if (owner_v.getWorld().isExtended())
            {
                return new int[] { com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_EXPERT, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_FABLE, com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_ULTRA };
            }
            return new int[] { com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET };
        }
        // Java: 136
        public void switchPlay(int n)
        {
            play_trail.append(' ').append(com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_NAMES[n]).append('@').append(java.lang.JString.format("%.1f", (float)(time_f / 60.0f)));
            tried[n] = true;
            play = n;
            play_since = time_f;
            if (n == com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET)
            {
                relayTo(null, time_f);
                return;
            }
            if (plays[n] == null)
            {
                com.oddlabs.tt.player.Player owner_v = getOwner();
                plays[n] = ((n) switch { 1 => (com.oddlabs.tt.player.AI)(((com.oddlabs.tt.player.ai.ExpertAI)com.oddlabs.tt.player.AI.makeBrain(new java.util.function.Supplier_L(() =>
                {
                    return new com.oddlabs.tt.player.ai.ExpertAI(owner_v, null);
                })))), 2 => (com.oddlabs.tt.player.AI)(((com.oddlabs.tt.player.ultra.UltraAI)com.oddlabs.tt.player.AI.makeBrain(new java.util.function.Supplier_L(() =>
                {
                    return new com.oddlabs.tt.player.ultra.UltraAI(owner_v, null);
                })))), _ => (com.oddlabs.tt.player.AI)(((com.oddlabs.tt.player.fable.FableAI)com.oddlabs.tt.player.AI.makeBrain(new java.util.function.Supplier_L(() =>
                {
                    return new com.oddlabs.tt.player.fable.FableAI(owner_v, null);
                })))) });
            }
            relayTo(plays[n], time_f);
        }
        // Java: 156
        public void judge()
        {
            if (time_f < next_judge)
            {
                return;
            }
            next_judge = (float)(time_f + com.oddlabs.tt.player.gauntlet.GauntletAI.JUDGE_PERIOD);
            if (!getOwner().isAlive())
            {
                return;
            }
            float own = com.oddlabs.tt.player.AI.tribePower(getOwner());
            float enemy = com.oddlabs.tt.player.AI.strongestEnemyPower(getOwner());
            behind = (enemy > (float)(own * com.oddlabs.tt.player.gauntlet.GauntletAI.BEHIND) ? behind + 1 : 0);
            if (behind < com.oddlabs.tt.player.gauntlet.GauntletAI.BEHIND_JUDGEMENTS || (float)(time_f - play_since) < com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_MIN)
            {
                return;
            }
            foreach (int p in order)
            {
                if (!tried[p])
                {
                    behind = 0;
                    switchPlay(p);
                    return;
                }
            }
        }
        // Java: 177
        public string getPlayTrail()
        {
            return (java.lang.JStr.valueOf(((order[0] == com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET ? "gauntlet@0.0" : ""))) + java.lang.JStr.valueOf(play_trail)).trim();
        }
        // Java: 181
        public int getPlay()
        {
            return play;
        }
        // Java: 185
        public static int countEnemies(com.oddlabs.tt.player.Player owner_v)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.Player p in owner_v.getWorld().getPlayers())
            {
                if (owner_v.isEnemy(p))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 193
        public override void lateStart(float match_seconds)
        {
            ticks_f = java.lang.Math.round((float)(match_seconds * com.oddlabs.tt.aikit.GameTime.TICKS_PER_SECOND));
            time_f = (float)(ticks_f / ((float)(com.oddlabs.tt.aikit.GameTime.TICKS_PER_SECOND)));
        }
        // Java: 199
        public override void animate(float t)
        {
            if (!com.oddlabs.tt.global.Globals.run_ai)
            {
                return;
            }
            ticks_f++;
            time_f = (float)(ticks_f / ((float)(com.oddlabs.tt.aikit.GameTime.TICKS_PER_SECOND)));
            try
            {
                judge();
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception)
            {
                log_f.error("judge", e);
            }
            if (relay() != null)
            {
                return;
            }
            try
            {
                if (initialized && strategy_f.weapon_sync)
                {
                    economy().weaponSyncObserve();
                }
                reflexes.tick();
                if (initialized)
                {
                    military().towerReflex();
                    military().armyReflex();
                }
                if (initialized && ticks_f % 5 == 0)
                {
                    shepherd().guard();
                    lures().guard();
                    dodges().guard();
                }
                think();
                if (initialized && strategy_f.weapon_sync)
                {
                    economy().weaponSync(t);
                }
            }
            catch (System.Exception e_2) when (e_2 is System.Exception || e_2 is System.Exception)
            {
                log_f.error("think", e_2);
            }
        }
        // Java: 237
        public void think()
        {
            if (!initialized)
            {
                initialize();
                initialized = true;
            }
            if (!getOwner().isAlive())
            {
                return;
            }
            bool due_intel = time_f >= next_intel;
            bool due_economy = time_f >= next_economy;
            bool due_plan = time_f >= next_plan;
            if (due_intel || due_economy || due_plan)
            {
                intel_f.update();
            }
            if (due_intel)
            {
                next_intel = (float)(time_f + com.oddlabs.tt.player.gauntlet.GauntletAI.INTEL_PERIOD);
                military().tick();
                chieftain().tick();
                shepherd().tick();
                if (jams != null)
                {
                    jams.tick();
                }
            }
            if (due_economy)
            {
                next_economy = (float)(time_f + com.oddlabs.tt.player.gauntlet.GauntletAI.ECONOMY_PERIOD);
                economy().tick();
                decoys().tick();
                freeze().tick();
            }
            if (due_plan)
            {
                next_plan = (float)(time_f + com.oddlabs.tt.player.gauntlet.GauntletAI.PLAN_PERIOD);
                economy().plan();
                military().plan();
            }
            if (logging_f && time_f >= next_stat)
            {
                next_stat = (float)(time_f + 30.0f);
                log_f.log("STAT", debugStatus());
            }
            if (time_f >= next_scan)
            {
                next_scan = (float)(time_f + com.oddlabs.tt.player.gauntlet.GauntletAI.SCAN_PERIOD);
                map().scanSupplies();
            }
            military().dodgeSpells();
        }
        // Java: 279
        public void initialize()
        {
            com.oddlabs.tt.player.Player owner_v = getOwner();
            map_f = new com.oddlabs.tt.player.gauntlet.MapAnalysis(owner_v.getWorld());
            int sx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner_v.getStartX());
            int sy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner_v.getStartY());
            com.oddlabs.tt.player.Player enemy = nearestEnemy(sx, sy);
            int ex = (enemy != null ? com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartX()) : map_f.getSize() - sx);
            int ey = (enemy != null ? com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartY()) : map_f.getSize() - sy);
            com.oddlabs.tt.player.gauntlet.DistanceField start_field = map_f.computeField(sx, sy, java.lang.Integer.MAX_VALUE);
            com.oddlabs.tt.player.gauntlet.DistanceField enemy_field = enemyStarts(ex, ey);
            planner_f = new com.oddlabs.tt.player.gauntlet.SitePlanner(map_f, owner_v, strategy_f, sx, sy, ex, ey, start_field, enemy_field);
            log(java.lang.JString.format("map %d cells, start %d,%d, nearest enemy starts %d,%d (%dm walk), %d enemies", map_f.getSize(), sx, sy, ex, ey, start_field.get(ex, ey), com.oddlabs.tt.player.gauntlet.GauntletAI.countEnemies(owner_v)));
            intel_f.update();
            freeze_f = new com.oddlabs.tt.player.gauntlet.Freeze(this);
            if (freeze_f.plan(start_field))
            {
                intel_f.update();
            }
            economy_f = new com.oddlabs.tt.player.gauntlet.Economy(this);
            military_f = new com.oddlabs.tt.player.gauntlet.Military(this);
            chieftain_f = new com.oddlabs.tt.player.gauntlet.Chieftain(this);
            decoys_f = new com.oddlabs.tt.player.gauntlet.Decoys(this);
            shepherd_f = new com.oddlabs.tt.player.gauntlet.Shepherd(this);
            lures_f = new com.oddlabs.tt.player.gauntlet.Lures(this);
            dodges_f = new com.oddlabs.tt.player.gauntlet.Dodges(this);
            jams = new com.oddlabs.tt.player.gauntlet.Jams(this);
            intel_f.decoys = decoys_f;
        }
        // Java: 309
        public com.oddlabs.tt.player.gauntlet.DistanceField enemyStarts(int nearest_x, int nearest_y)
        {
            java.util.List starts = new java.util.ArrayList();
            starts.add(new int[] { nearest_x, nearest_y });
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(p))
                {
                    continue;
                }
                int x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                int y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                if (x != nearest_x || y != nearest_y)
                {
                    starts.add(new int[] { x, y });
                }
            }
            int[] xs = new int[starts.size()];
            int[] ys = new int[starts.size()];
            for (int i = 0; i < xs.Length; i++)
            {
                xs[i] = ((int[])starts.get(i))[0];
                ys[i] = ((int[])starts.get(i))[1];
            }
            return map().computeField(xs, ys, java.lang.Integer.MAX_VALUE);
        }
        // Java: 330
        public int enemiesAlive()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (getOwner().isEnemy(p) && p.isAlive())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 339
        public float[] liveEnemyCenter()
        {
            long x = 0;
            long y = 0;
            int n = 0;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (getOwner().isEnemy(p) && p.isAlive())
                {
                    x += com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                    y += com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                    n++;
                }
            }
            return (n == 0 ? (float[])(null) : new float[] { (float)(x / ((float)(n))), (float)(y / ((float)(n))) });
        }
        // Java: 352
        public com.oddlabs.tt.player.Player nearestEnemy(int sx, int sy)
        {
            com.oddlabs.tt.player.Player best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(p))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY()));
                if (d < best_d)
                {
                    best_d = d;
                    best = p;
                }
            }
            return best;
        }
        // Java: 372
        public com.oddlabs.tt.model.Building placeSite(java.util.List builders, int type, int gx, int gy)
        {
            if (builders.isEmpty())
            {
                return null;
            }
            getOwner().placeBuilding(((com.oddlabs.tt.model.Selectable[])builders.toArray(new com.oddlabs.tt.model.Selectable[0])), type, gx, gy, false);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(builders))
            {
                if (u.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.behaviour.Controller c = u.getPrimaryController();
                if ((c is com.oddlabs.tt.model.behaviour.PlaceBuildingController placing))
                {
                    return placing.getBuilding();
                }
                if ((c is com.oddlabs.tt.model.behaviour.RepairController repairing))
                {
                    return repairing.getBuilding();
                }
            }
            return null;
        }
        // Java: 396
        public void landscapeOrder(com.oddlabs.tt.model.Selectable[] units, int gx, int gy, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            int size = getOwner().getWorld().getUnitGrid().getGridSize();
            int x = java.lang.Math.max(3, java.lang.Math.min(size - 4, gx));
            int y = java.lang.Math.max(3, java.lang.Math.min(size - 4, gy));
            getOwner().setLandscapeTarget(units, x, y, action, aggressive);
        }
        // Java: 403
        public com.oddlabs.tt.player.Player owner()
        {
            return getOwner();
        }
        // Java: 408
        public float time()
        {
            return time_f;
        }
        // Java: 413
        public int ticks()
        {
            return ticks_f;
        }
        // Java: 417
        public com.oddlabs.tt.player.gauntlet.Strategy strategy()
        {
            return strategy_f;
        }
        // Java: 422
        public com.oddlabs.tt.player.gauntlet.Intel intel()
        {
            return intel_f;
        }
        // Java: 427
        public com.oddlabs.tt.aikit.AiLog aiLog()
        {
            return log_f;
        }
        // Java: 432
        public com.oddlabs.tt.player.gauntlet.MapAnalysis map()
        {
            return map_f;
        }
        // Java: 438
        public com.oddlabs.tt.player.gauntlet.SitePlanner planner()
        {
            return planner_f;
        }
        // Java: 444
        public com.oddlabs.tt.player.gauntlet.Economy economy()
        {
            return economy_f;
        }
        // Java: 450
        public com.oddlabs.tt.player.gauntlet.Military military()
        {
            return military_f;
        }
        // Java: 456
        public com.oddlabs.tt.player.gauntlet.Shepherd shepherd()
        {
            return shepherd_f;
        }
        // Java: 462
        public com.oddlabs.tt.player.gauntlet.Lures lures()
        {
            return lures_f;
        }
        // Java: 467
        public com.oddlabs.tt.player.gauntlet.Dodges dodges()
        {
            return dodges_f;
        }
        // Java: 472
        public com.oddlabs.tt.player.gauntlet.Freeze freeze()
        {
            return freeze_f;
        }
        // Java: 478
        public com.oddlabs.tt.player.gauntlet.Decoys decoys()
        {
            return decoys_f;
        }
        // Java: 484
        public com.oddlabs.tt.player.gauntlet.Chieftain chieftain()
        {
            return chieftain_f;
        }
        // Java: 491
        public bool logging()
        {
            return logging_f;
        }
        // Java: 495
        public void log(string message)
        {
            if (logging_f)
            {
                log_f.log("AI", message);
            }
        }
        // Java: 501
        public string debugStatus()
        {
            if (economy_f == null || military_f == null)
            {
                return "starting";
            }
            return java.lang.JStr.valueOf(economy_f.debugStatus()) + " " + java.lang.JStr.valueOf(military_f.debugStatus());
        }
        static GauntletAI()
        {
            GauntletAI.PLAY_NAMES = new string[] { "gauntlet", "expert", "ultra", "fable" };
            GauntletAI.forced_opening = (-1);
        }
        private void init_GauntletAI()
        {
            this.next_economy = 0.25f;
            this.next_plan = 0.5f;
            this.next_scan = com.oddlabs.tt.player.gauntlet.GauntletAI.SCAN_PERIOD;
            this.next_stat = 30.0f;
            this.plays = new com.oddlabs.tt.player.AI[4];
            this.tried = new bool[4];
            this.play = com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET;
            this.next_judge = com.oddlabs.tt.player.gauntlet.GauntletAI.JUDGE_START;
            this.play_trail = new java.lang.StringBuilder();
        }
        public GauntletAI(com.oddlabs.tt.player.Player owner_v, com.oddlabs.tt.player.UnitInfo units, string spec_params) : base(owner_v, units)
        {
            init_GauntletAI();
            com.oddlabs.tt.player.NavalCommander.attach(owner_v);
            log_f = com.oddlabs.tt.aikit.AiLog.of(owner_v);
            com.oddlabs.tt.aikit.AiParams @params = com.oddlabs.tt.aikit.AiParams.parse(spec_params);
            strategy_f = com.oddlabs.tt.player.gauntlet.Strategy.forGame(owner_v.getWorld().getMapSize(), com.oddlabs.tt.player.gauntlet.GauntletAI.countEnemies(owner_v));
            strategy_f.apply(@params);
            string read = @params.done();
            log_f.log("PARAM", read);
            log_f.log("LOG", new java.util.function.Supplier_L(() =>
            {
                logging_f = true;
                return "decision log on";
            }));
            intel_f = new com.oddlabs.tt.player.gauntlet.Intel(owner_v);
            reflexes = new com.oddlabs.tt.player.gauntlet.Reflexes(this, strategy_f.swing_restart, strategy_f.stun_cancel, strategy_f.tower_unstun);
            order = com.oddlabs.tt.player.gauntlet.GauntletAI.openingOrder(owner_v);
            if (order[0] != com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET)
            {
                switchPlay(order[0]);
            }
            else
            {
                tried[com.oddlabs.tt.player.gauntlet.GauntletAI.PLAY_GAUNTLET] = true;
            }
        }
    }
}

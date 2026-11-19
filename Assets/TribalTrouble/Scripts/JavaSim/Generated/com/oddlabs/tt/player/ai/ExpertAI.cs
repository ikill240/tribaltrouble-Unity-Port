// Translated from ExpertAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 31
    public class ExpertAI : com.oddlabs.tt.player.AI
    {
        public const float INTEL_PERIOD = 0.5f;
        public const float ECONOMY_PERIOD = 1.0f;
        public const float PLAN_PERIOD = 3.0f;
        public const float SCAN_PERIOD = 30.0f;
        public com.oddlabs.tt.player.ai.Strategy strategy_f;
        public java.util.Random random_f;
        public com.oddlabs.tt.player.ai.Intel intel_f;
        public com.oddlabs.tt.player.ai.MapAnalysis map_f;
        public com.oddlabs.tt.player.ai.SitePlanner planner_f;
        public com.oddlabs.tt.player.ai.Economy economy_f;
        public com.oddlabs.tt.player.ai.Military military_f;
        public com.oddlabs.tt.player.ai.Chieftain chieftain_f;
        public static java.util.logging.Logger logger;
        public static bool strict;
        public int errors;
        public float time_f;
        public float next_intel;
        public float next_economy;
        public float next_plan;
        public float next_scan;
        public bool initialized;
        public com.oddlabs.tt.player.ai.GameLog game_log;
        public float next_report;
        public float next_flush;
        public bool game_over;
        // Java: 67
        public static com.oddlabs.tt.player.ai.ExpertAI withOverrides(com.oddlabs.tt.player.Player owner_v, com.oddlabs.tt.player.UnitInfo unit_info, string overrides)
        {
            com.oddlabs.tt.player.ai.Strategy strategy_v = com.oddlabs.tt.player.ai.Strategy.forWorld(owner_v.getWorld(), com.oddlabs.tt.player.ai.ExpertAI.countEnemies(owner_v));
            strategy_v.@override(overrides);
            return new com.oddlabs.tt.player.ai.ExpertAI(owner_v, unit_info, strategy_v);
        }
        // Java: 90
        public void logTo(java.nio.file.Path dir)
        {
            try
            {
                java.nio.file.Files.createDirectories(dir);
            }
            catch (System.IO.IOException e)
            {
                com.oddlabs.tt.player.ai.ExpertAI.logger.log(java.util.logging.Level.WARNING, "Cannot create " + java.lang.JStr.valueOf(dir) + " for the expert AI log", e);
                return;
            }
            string name = "expert-ai-" + java.lang.JStr.valueOf(getOwner().getPlayerInfo().getName().replaceAll("[^A-Za-z0-9_-]", "_"));
            java.nio.file.Path file = dir.resolve(java.lang.JStr.valueOf(name) + ".log");
            for (int n = 2; java.nio.file.Files.exists(file); n++)
            {
                file = dir.resolve(java.lang.JStr.valueOf(name) + "-" + (n) + ".log");
            }
            game_log = new com.oddlabs.tt.player.ai.GameLog(file);
            game_log.add("Expert AI game log, started " + java.lang.JStr.valueOf(java.time.LocalDateTime.now(java.time.ZoneId.systemDefault()).withNano(0)));
            com.oddlabs.tt.player.ai.ExpertAI.logger.info("Expert AI log: " + java.lang.JStr.valueOf(file.toAbsolutePath()));
        }
        // Java: 107
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
        // Java: 115
        public static int indexOf(com.oddlabs.tt.player.Player owner_v)
        {
            com.oddlabs.tt.player.Player[] players = owner_v.getWorld().getPlayers();
            for (int i = 0; i < players.Length; i++)
            {
                if (((object)players[i] == (object)owner_v))
                {
                    return i;
                }
            }
            return 0;
        }
        // Java: 123
        public override void lateStart(float match_seconds)
        {
            time_f = match_seconds;
        }
        // Java: 128
        public override void animate(float t)
        {
            if (!com.oddlabs.tt.global.Globals.run_ai)
            {
                return;
            }
            time_f += t;
            try
            {
                think();
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception)
            {
                if (com.oddlabs.tt.player.ai.ExpertAI.strict)
                {
                    throw e;
                }
                if (errors++ < 5)
                {
                    com.oddlabs.tt.player.ai.ExpertAI.logger.log(java.util.logging.Level.WARNING, "Expert AI error at " + java.lang.JStr.valueOf(time_f) + "s", e);
                }
                log("error: " + java.lang.JStr.valueOf(e));
            }
            if (game_log != null)
            {
                keepLog(game_log);
            }
        }
        // Java: 148
        public void keepLog(com.oddlabs.tt.player.ai.GameLog log_v)
        {
            if (game_over)
            {
                return;
            }
            int enemies_alive = 0;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (getOwner().isEnemy(p) && p.isAlive())
                {
                    enemies_alive++;
                }
            }
            bool over = !getOwner().isAlive() || enemies_alive == 0;
            if (time_f >= next_report || over)
            {
                next_report = (float)(time_f + 60.0f);
                log_v.add("---- " + java.lang.JStr.valueOf(com.oddlabs.tt.player.ai.GameLog.clock(time_f).trim()) + java.lang.JStr.valueOf(((over ? " game over" : ""))));
                foreach (com.oddlabs.tt.player.Player p_2 in getOwner().getWorld().getPlayers())
                {
                    if (p_2.getUnits().getSet().isEmpty() && !p_2.isAlive())
                    {
                        continue;
                    }
                    log_v.add("  " + java.lang.JStr.valueOf(com.oddlabs.tt.player.ai.GameLog.describe(p_2)));
                    if (((object)p_2 == (object)getOwner()))
                    {
                        log_v.add("    plan: " + java.lang.JStr.valueOf(debugStatus()));
                    }
                }
            }
            if (time_f >= next_flush || over)
            {
                next_flush = (float)(time_f + 3.0f);
                log_v.flush();
            }
            game_over = over;
        }
        // Java: 174
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
                next_intel = (float)(time_f + com.oddlabs.tt.player.ai.ExpertAI.INTEL_PERIOD);
                military().tick();
                chieftain().tick();
            }
            if (due_economy)
            {
                next_economy = (float)(time_f + com.oddlabs.tt.player.ai.ExpertAI.ECONOMY_PERIOD);
                economy().tick();
            }
            if (due_plan)
            {
                next_plan = (float)(time_f + com.oddlabs.tt.player.ai.ExpertAI.PLAN_PERIOD);
                economy().plan();
                military().plan();
            }
            if (time_f >= next_scan)
            {
                next_scan = (float)(time_f + com.oddlabs.tt.player.ai.ExpertAI.SCAN_PERIOD);
                map().scanSupplies();
            }
            military().dodgeSpells();
        }
        // Java: 207
        public void initialize()
        {
            com.oddlabs.tt.player.Player owner_v = getOwner();
            map_f = new com.oddlabs.tt.player.ai.MapAnalysis(owner_v.getWorld());
            int sx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner_v.getStartX());
            int sy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner_v.getStartY());
            com.oddlabs.tt.player.Player enemy = nearestEnemy(sx, sy);
            int ex = (enemy != null ? com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartX()) : map_f.getSize() - sx);
            int ey = (enemy != null ? com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartY()) : map_f.getSize() - sy);
            com.oddlabs.tt.player.ai.DistanceField start_field = map_f.computeField(sx, sy, java.lang.Integer.MAX_VALUE);
            com.oddlabs.tt.player.ai.DistanceField enemy_field = enemyStarts(ex, ey);
            planner_f = new com.oddlabs.tt.player.ai.SitePlanner(map_f, owner_v, strategy_f, sx, sy, ex, ey, start_field, enemy_field);
            log(java.lang.JString.format("map %d cells, start %d,%d, nearest enemy %s starts %d,%d (%dm walk)", map_f.getSize(), sx, sy, enemy, ex, ey, start_field.get(ex, ey)));
            intel_f.update();
            economy_f = new com.oddlabs.tt.player.ai.Economy(this);
            military_f = new com.oddlabs.tt.player.ai.Military(this);
            chieftain_f = new com.oddlabs.tt.player.ai.Chieftain(this);
        }
        // Java: 227
        public com.oddlabs.tt.player.ai.DistanceField enemyStarts(int nearest_x, int nearest_y)
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
        // Java: 248
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
        // Java: 256
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
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(sx, sy, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY()));
                if (d < best_d)
                {
                    best_d = d;
                    best = p;
                }
            }
            return best;
        }
        // Java: 272
        public com.oddlabs.tt.player.Player owner()
        {
            return getOwner();
        }
        // Java: 277
        public float time()
        {
            return time_f;
        }
        // Java: 281
        public java.util.Random random()
        {
            return random_f;
        }
        // Java: 286
        public com.oddlabs.tt.player.ai.Strategy strategy()
        {
            return strategy_f;
        }
        // Java: 291
        public com.oddlabs.tt.player.ai.Intel intel()
        {
            return intel_f;
        }
        // Java: 296
        public com.oddlabs.tt.player.ai.MapAnalysis map()
        {
            return map_f;
        }
        // Java: 302
        public com.oddlabs.tt.player.ai.SitePlanner planner()
        {
            return planner_f;
        }
        // Java: 308
        public com.oddlabs.tt.player.ai.Economy economy()
        {
            return economy_f;
        }
        // Java: 314
        public com.oddlabs.tt.player.ai.Military military()
        {
            return military_f;
        }
        // Java: 320
        public com.oddlabs.tt.player.ai.Chieftain chieftain()
        {
            return chieftain_f;
        }
        public static bool debug;
        public static int[] debug_battle;
        // Java: 331
        public static void debugSites(bool on)
        {
            com.oddlabs.tt.player.ai.SitePlanner.debug_sites = on;
        }
        // Java: 336
        public bool logging()
        {
            return com.oddlabs.tt.player.ai.ExpertAI.debug || game_log != null;
        }
        // Java: 340
        public void log(string message)
        {
            if (!logging())
            {
                return;
            }
            string line = "[" + java.lang.JStr.valueOf(com.oddlabs.tt.player.ai.GameLog.clock(time_f)) + "] " + java.lang.JStr.valueOf(getOwner()) + ": " + java.lang.JStr.valueOf(message);
            if (com.oddlabs.tt.player.ai.ExpertAI.debug)
            {
                java.lang.JSystem.@out.println(line);
            }
            if (game_log != null)
            {
                game_log.add(line);
            }
        }
        // Java: 351
        public string debugStatus()
        {
            if (economy_f == null || military_f == null)
            {
                return "starting";
            }
            return java.lang.JStr.valueOf(economy_f.debugStatus()) + " " + java.lang.JStr.valueOf(military_f.debugStatus());
        }
        static ExpertAI()
        {
            ExpertAI.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.player.ai.ExpertAI).getName());
            ExpertAI.strict = false;
            ExpertAI.debug = false;
        }
        private void init_ExpertAI()
        {
            this.next_economy = 0.25f;
            this.next_plan = 0.5f;
            this.next_scan = com.oddlabs.tt.player.ai.ExpertAI.SCAN_PERIOD;
        }
        public ExpertAI(com.oddlabs.tt.player.Player owner_v, com.oddlabs.tt.player.UnitInfo unit_info) : this(owner_v, unit_info, com.oddlabs.tt.player.ai.Strategy.forWorld(owner_v.getWorld(), com.oddlabs.tt.player.ai.ExpertAI.countEnemies(owner_v)))
        {
        }
        public ExpertAI(com.oddlabs.tt.player.Player owner_v, com.oddlabs.tt.player.UnitInfo unit_info, com.oddlabs.tt.player.ai.Strategy strategy_v) : base(owner_v, unit_info)
        {
            init_ExpertAI();
            com.oddlabs.tt.player.NavalCommander.attach(owner_v);
            com.oddlabs.tt.landscape.World world = owner_v.getWorld();
            world.getAnimationManagerRealTime().removeAnimation(this);
            world.getAnimationManagerGameTime().registerAnimation(this);
            this.strategy_f = strategy_v;
            this.random_f = new java.util.Random(7919L * (1 + com.oddlabs.tt.player.ai.ExpertAI.indexOf(owner_v)));
            this.intel_f = new com.oddlabs.tt.player.ai.Intel(owner_v);
        }
    }
}

// Translated from UltraAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 21
    public class UltraAI : com.oddlabs.tt.player.AI
    {
        // Java: 22
        public interface Task
        {
            // Java: 23
            void run();
        }
        public sealed class Task_L : java.lang.Object, Task
        {
            readonly System.Action f;
            public Task_L(System.Action f) { this.f = f; }
            public void run() { f(); }
        }
        public com.oddlabs.tt.player.ultra.Context ctx;
        public string[] task_names;
        public float[] periods;
        public float[] next;
        public com.oddlabs.tt.player.ultra.UltraAI.Task[] tasks;
        public bool started;
        // Java: 54
        public override void lateStart(float match_seconds)
        {
            ctx.now = match_seconds;
        }
        // Java: 59
        public override void animate(float t)
        {
            float dt = (float)((float)(getOwner().getWorld().getSecondsPerTick() * t) / com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK);
            if (dt <= 0.0f || !com.oddlabs.tt.global.Globals.run_ai)
            {
                return;
            }
            ctx.now += dt;
            if (!started)
            {
                started = true;
                guarded("start", new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { this.start(); }));
            }
            for (int i = 0; i < tasks.Length; i++)
            {
                if (ctx.now >= next[i])
                {
                    next[i] += periods[i];
                    if (next[i] < ctx.now)
                    {
                        next[i] = (float)(ctx.now + periods[i]);
                    }
                    guarded(task_names[i], tasks[i]);
                }
            }
        }
        // Java: 79
        public void start()
        {
            int enemies = 0;
            foreach (com.oddlabs.tt.player.Player pl in getOwner().getWorld().getPlayers())
            {
                if (((object)pl != (object)getOwner()) && getOwner().isEnemy(pl) && !pl.getUnits().getSet().isEmpty())
                {
                    enemies++;
                }
            }
            ctx.strategy.setEnemyCount(enemies);
            if (ctx.strategy.multiEnemy())
            {
                ctx.economy.readMultiParams();
                ctx.military.readMultiParams();
                int n = enemies;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "multi-enemy defaults on: " + (n) + " enemy players";
                }));
            }
            ctx.model.update();
            ctx.resources.update();
            ctx.build.planOpening(false);
        }
        // Java: 98
        public void sense()
        {
            ctx.model.update();
            ctx.pruneRoles();
            ctx.orders.prune();
            ctx.build.prune();
            ctx.military.prune();
        }
        // Java: 106
        public void micro()
        {
            ctx.military.microTick();
            ctx.towers.microTick();
            ctx.chief.tick();
            ctx.corps.microTick();
        }
        public float next_census;
        // Java: 115
        public void posture()
        {
            if (com.oddlabs.tt.player.ultra.UltraLog.enabled() && ctx.now >= next_census)
            {
                next_census = (float)(ctx.now + 30.0f);
                ctx.log(new java.util.function.Supplier_L(() => { return this.census(); }));
            }
            ctx.military.postureTick();
            ctx.corps.recruitTick();
            ctx.creep.tick();
            ctx.proxy.tick();
        }
        // Java: 127
        public string census()
        {
            com.oddlabs.tt.player.ultra.WorldModel.Side me = ctx.model.me;
            int[] act = new int[com.oddlabs.tt.player.ultra.WorldModel.Activity.values().Length];
            int[] gather = new int[4];
            int[] roles = new int[com.oddlabs.tt.player.ultra.Context.Role.values().Length];
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(me.peons))
            {
                act[com.oddlabs.tt.player.ultra.WorldModel.activityOf(u).ordinal()]++;
                if ((com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc))
                {
                    System.Type t = gc.getSupplyType();
                    gather[(((object)t == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? 0 : (((object)t == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? 1 : (((object)t == (object)typeof(com.oddlabs.tt.model.RockSupply)) ? 2 : 3)))]++;
                }
                com.oddlabs.tt.player.ultra.Context.Role r = ctx.roleOf(u);
                if (r != null)
                {
                    roles[r.ordinal()]++;
                }
            }
            java.lang.StringBuilder sb = new java.lang.StringBuilder("census units=").append(me.unit_count).append(" peons=").append(me.peons.size()).append(" inside=").append(me.peons_inside).append(" (armory ").append(me.armory_workers).append(") warriors=").append(me.warriors.size()).append(" garr=").append(me.garrisoned).append(" Q=").append(me.quarters.size()).append(" T=").append(me.towers.size()).append(" | act");
            foreach (com.oddlabs.tt.player.ultra.WorldModel.Activity a in com.oddlabs.tt.player.ultra.WorldModel.Activity.values())
            {
                if (act[a.ordinal()] > 0)
                {
                    sb.append(' ').append(a.name().toLowerCase(java.util.Locale.ROOT)).append('=').append(act[a.ordinal()]);
                }
            }
            sb.append(" | gather w/i/r/o=").append(gather[0]).append('/').append(gather[1]).append('/').append(gather[2]).append('/').append(gather[3]).append(" | roles");
            foreach (com.oddlabs.tt.player.ultra.Context.Role r_2 in com.oddlabs.tt.player.ultra.Context.Role.values())
            {
                sb.append(' ').append(r_2.name().toLowerCase(java.util.Locale.ROOT)).append('=').append(roles[r_2.ordinal()]);
            }
            sb.append(" | econ ").append(ctx.economy.phase()).append(" wood=").append(ctx.build.woodDemand()).append(" weapons r/i/u=").append(me.weapons_rock).append('/').append(me.weapons_iron).append('/').append(me.weapons_rubber).append(" harvested w=").append(ctx.owner.getTreeHarvested());
            com.oddlabs.tt.model.Unit chief = ctx.owner.getChieftain();
            if (chief != null && !chief.isDead())
            {
                sb.append(" | chief ").append(chief.getGridX()).append(',').append(chief.getGridY()).append(" hp=").append(chief.getHitPoints()).append(" stun=").append(java.lang.JString.format(java.util.Locale.ROOT, "%.2f", chief.getMagicProgress(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_STUN)));
            }
            else
            {
                sb.append(" | chief none training=").append(ctx.owner.isTrainingChieftain());
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 169
        public void economy()
        {
            ctx.economy.updateArmory();
            ctx.resources.update();
            ctx.build.tick(periods[2]);
            ctx.economy.tick();
        }
        // Java: 176
        public void guarded(string name, com.oddlabs.tt.player.ultra.UltraAI.Task task)
        {
            long start_v = (com.oddlabs.tt.player.ultra.UltraLog.enabled() ? java.lang.JSystem.nanoTime() : 0L);
            try
            {
                task.run();
                if (com.oddlabs.tt.player.ultra.UltraLog.enabled())
                {
                    long micros = (java.lang.JSystem.nanoTime() - start_v) / 1000;
                    if (micros >= 15000)
                    {
                        ctx.log(new java.util.function.Supplier_L(() =>
                        {
                            return "SLOW " + java.lang.JStr.valueOf(name) + " " + (micros / 1000) + " ms";
                        }));
                    }
                }
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception)
            {
                ctx.error_count++;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    java.lang.StringBuilder sb = new java.lang.StringBuilder("ERROR in " + java.lang.JStr.valueOf(name) + ": " + java.lang.JStr.valueOf(e));
                    java.lang.StackTraceElement[] st = e.getStackTrace();
                    for (int i = 0; i < java.lang.Math.min(8, st.Length); i++)
                    {
                        sb.append("\n    at ").append(st[i]);
                    }
                    return java.lang.JO.toString(sb);
                }));
                if (com.oddlabs.tt.player.ultra.UltraLog.isStrict())
                {
                    throw e;
                }
            }
        }
        // Java: 201
        public int getErrorCount()
        {
            return ctx.error_count;
        }
        // Java: 205
        public override void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            checksum.update(java.lang.Float.floatToIntBits(ctx.now));
            checksum.update(ctx.military.state().ordinal());
            checksum.update(ctx.military.armySize());
            checksum.update(ctx.error_count);
        }
        private void init_UltraAI()
        {
            this.task_names = new string[] { "sense", "micro", "econ", "threat", "posture", "plan", "garrison" };
            this.periods = new float[] { 0.25f, 0.25f, 0.5f, 0.5f, 1.0f, 2.0f, 1.0f };
            this.next = new float[] { 0.0f, 0.01f, 0.12f, 0.2f, 0.4f, 1.1f, 0.7f };
        }
        public UltraAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info) : this(owner, unit_info, null)
        {
        }
        public UltraAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, string @params) : base(owner, unit_info)
        {
            init_UltraAI();
            com.oddlabs.tt.player.NavalCommander.attach(owner);
            com.oddlabs.tt.player.ultra.Params p = new com.oddlabs.tt.player.ultra.Params(@params);
            ctx = new com.oddlabs.tt.player.ultra.Context(owner, p, com.oddlabs.tt.player.ultra.Strategy.forMap(p, owner.getWorld().getMapSize()));
            ctx.resources = new com.oddlabs.tt.player.ultra.ResourceTracker(ctx);
            ctx.build = new com.oddlabs.tt.player.ultra.BuildManager(ctx);
            ctx.economy = new com.oddlabs.tt.player.ultra.EconomyManager(ctx);
            ctx.military = new com.oddlabs.tt.player.ultra.MilitaryManager(ctx);
            ctx.towers = new com.oddlabs.tt.player.ultra.TowerManager(ctx);
            ctx.chief = new com.oddlabs.tt.player.ultra.ChieftainManager(ctx);
            ctx.corps = new com.oddlabs.tt.player.ultra.CorpsManager(ctx);
            ctx.creep = new com.oddlabs.tt.player.ultra.CreepManager(ctx);
            ctx.proxy = new com.oddlabs.tt.player.ultra.ProxyManager(ctx);
            tasks = new com.oddlabs.tt.player.ultra.UltraAI.Task[] { new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { this.sense(); }), new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { this.micro(); }), new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { this.economy(); }), new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { ctx.military.threatTick(); }), new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { this.posture(); }), new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { ctx.towers.planTick(); }), new com.oddlabs.tt.player.ultra.UltraAI.Task_L(() => { ctx.towers.garrisonTick(); }) };
        }
    }
}

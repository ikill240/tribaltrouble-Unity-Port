// Translated from CreepManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 14
    public class CreepManager : java.lang.Object
    {
        public const int ENEMY_TOWER_STANDOFF = 20;
        public const int SHELL_RANGE = 17;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public com.oddlabs.tt.player.ultra.BuildManager.Task task;
        public float last_try;
        public bool enabled;
        public int max_towers;
        public int built;
        public float last_staffed;
        public int failed_x;
        public int failed_y;
        public float failed_time;
        // Java: 39
        public void tick()
        {
            if (!enabled)
            {
                return;
            }
            if (task != null)
            {
                if (((object)task.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.DONE))
                {
                    built++;
                }
                if (((object)task.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.FAILED))
                {
                    failed_x = task.x;
                    failed_y = task.y;
                    failed_time = ctx.now;
                }
                if (!task.open())
                {
                    task = null;
                }
                else
                {
                    staff();
                    abandonIfStranded();
                    return;
                }
            }
            if (built >= max_towers || (float)(ctx.now - last_try) < 10.0f)
            {
                return;
            }
            last_try = ctx.now;
            com.oddlabs.tt.player.ultra.MilitaryManager.State s = ctx.military.state();
            if (((object)s != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE) && ((object)s != (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE))
            {
                return;
            }
            com.oddlabs.tt.model.Building target = ctx.military.objectiveBuilding();
            if (target == null || target.isDead() || !target.isComplete())
            {
                return;
            }
            int id = ((com.oddlabs.tt.model.BuildingTemplate)target.getTemplate()).getTemplateID();
            if (id != com.oddlabs.tt.model.Race.BUILDING_ARMORY && id != com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
            {
                return;
            }
            if (!ctx.military.holdingGround() || ctx.corps.size() < 6)
            {
                return;
            }
            if (!ctx.owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER) || ctx.owner.getBuildingCountContainer().getNumSupplies() + ctx.build.pendingSlots() >= ctx.world.getMaxBuildingCount() - 1)
            {
                return;
            }
            int[] site = siteNear(target);
            if (site == null)
            {
                return;
            }
            task = ctx.build.request(com.oddlabs.tt.model.Race.BUILDING_TOWER, site[0], site[1], 950, 0, "Creep");
            task.manual = true;
            last_staffed = ctx.now;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "tower creep at " + (site[0]) + "," + (site[1]) + " vs " + (target.getGridX()) + "," + (target.getGridY());
            }));
            staff();
        }
        // Java: 86
        public void staff()
        {
            com.oddlabs.tt.player.ultra.BuildManager.Task t = task;
            if (t == null)
            {
                return;
            }
            int builders = ctx.build.builderCount(t);
            if (builders > 0)
            {
                last_staffed = ctx.now;
            }
            int want = 8 - builders;
            com.oddlabs.tt.model.Building b = t.building;
            if (b != null && !b.isDead() && b.isPlaced())
            {
                want = java.lang.Math.min(want, java.lang.Math.max(0, ctx.build.woodNeed(t)));
            }
            if (want <= 0)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.corps.members()))
            {
                if (want <= 0)
                {
                    break;
                }
                if (u.isDead() || ((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS) || !ctx.orders.canOrder(u))
                {
                    continue;
                }
                if (!ctx.build.placeWith(t, u))
                {
                    break;
                }
                ctx.corps.release(u);
                last_staffed = ctx.now;
                want--;
            }
        }
        // Java: 114
        public void abandonIfStranded()
        {
            com.oddlabs.tt.player.ultra.BuildManager.Task t = task;
            if (t == null || ctx.build.builderCount(t) > 0 || ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED))
            {
                return;
            }
            com.oddlabs.tt.player.ultra.MilitaryManager.State s = ctx.military.state();
            bool fighting = ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH) || ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE) || ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE);
            if (fighting && (float)(ctx.now - last_staffed) < 20.0f)
            {
                return;
            }
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "creep abandoned " + java.lang.JStr.valueOf(t);
            }));
            ctx.build.cancel(t);
            task = null;
        }
        // Java: 128
        public int[] siteNear(com.oddlabs.tt.model.Building target)
        {
            int tx = target.getGridX();
            int ty = target.getGridY();
            int[] anchor = ctx.military.squadAnchor();
            int ax = (anchor != null ? anchor[0] : tx);
            int ay = (anchor != null ? anchor[1] : ty);
            return ctx.sites.best(ctx.race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER), tx, ty, com.oddlabs.tt.player.ultra.CreepManager.SHELL_RANGE, new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x, int y) =>
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, tx, ty) > com.oddlabs.tt.player.ultra.CreepManager.SHELL_RANGE)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                if ((float)(ctx.now - failed_time) < 120.0f && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, failed_x, failed_y) <= 8)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                foreach (com.oddlabs.tt.model.Building e in java.lang.JIter.of(ctx.model.enemy.towers))
                {
                    if (!e.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y) < com.oddlabs.tt.player.ultra.CreepManager.ENEMY_TOWER_STANDOFF * com.oddlabs.tt.player.ultra.CreepManager.ENEMY_TOWER_STANDOFF)
                    {
                        return java.lang.Float.NEGATIVE_INFINITY;
                    }
                }
                return (float)((float)(-((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(x, y, ax, ay))))) + (float)(0.3f * com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, tx, ty)));
            }));
        }
        private void init_CreepManager()
        {
            this.last_try = java.lang.Float.NEGATIVE_INFINITY;
            this.failed_x = (-100);
            this.failed_y = (-100);
            this.failed_time = java.lang.Float.NEGATIVE_INFINITY;
        }
        public CreepManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_CreepManager();
            this.ctx = ctx;
            this.enabled = ctx.strategy.b("creep");
            this.max_towers = ctx.strategy.i("creepMax");
        }
    }
}

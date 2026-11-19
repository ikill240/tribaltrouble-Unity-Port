// Translated from ProxyManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 18
    public class ProxyManager : java.lang.Object
    {
        public const int CREW = 12;
        public const int STANDOFF = 20;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public bool enabled;
        public float start_time;
        public int max_towers;
        public com.oddlabs.tt.player.ultra.BuildManager.Task task;
        public int built;
        public int failed;
        public float last_try;
        // Java: 38
        public void tick()
        {
            if (!enabled)
            {
                return;
            }
            if (task != null && !task.open())
            {
                if (((object)task.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.DONE))
                {
                    built++;
                }
                else
                {
                    failed++;
                }
                task = null;
            }
            if (task != null)
            {
                com.oddlabs.tt.player.ultra.BuildManager.Task t = task;
                if (t.drop_pending == 0 && ctx.build.builderCount(t) == 0 && ((object)t.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED) && (float)(ctx.now - t.drop_time) > 30.0f)
                {
                    com.oddlabs.tt.model.Building b = t.building;
                    if (b != null && !b.isDead() && b.isPlaced() && failed <= max_towers)
                    {
                        recrew(t);
                    }
                    else
                    {
                        ctx.build.cancel(t);
                    }
                }
                return;
            }
            if (built >= max_towers || failed > max_towers || ctx.now < start_time || (float)(ctx.now - last_try) < 15.0f)
            {
                return;
            }
            last_try = ctx.now;
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null || ctx.model.enemy.armories.isEmpty() || ctx.build.dropPending())
            {
                return;
            }
            int wood = a.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies();
            int inside = a.getUnitContainer().getNumSupplies();
            int n = java.lang.Math.min(com.oddlabs.tt.player.ultra.ProxyManager.CREW, inside - 2);
            bool carry = wood >= n + 4;
            com.oddlabs.tt.model.DeployType type = (carry ? com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE : com.oddlabs.tt.model.DeployType.PEON);
            com.oddlabs.tt.model.DeployContainer c = a.getDeployContainer(type);
            if (n < 8 || c == null || c.getNumSupplies() > 0)
            {
                return;
            }
            if (!ctx.owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER) || ctx.owner.getBuildingCountContainer().getNumSupplies() + ctx.build.pendingSlots() >= ctx.world.getMaxBuildingCount() - 1)
            {
                return;
            }
            int[] focus = gathererCluster();
            if (focus == null)
            {
                com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)ctx.model.enemy.armories.get(0));
                focus = new int[] { armory.getGridX(), armory.getGridY() };
            }
            int[] site_v = site(focus[0], focus[1]);
            int[] f = focus;
            if (site_v == null)
            {
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "proxy no site near " + (f[0]) + "," + (f[1]);
                }));
                return;
            }
            com.oddlabs.tt.player.ultra.BuildManager.Task t_2 = ctx.build.request(com.oddlabs.tt.model.Race.BUILDING_TOWER, site_v[0], site_v[1], 940, 0, "Proxy");
            t_2.manual = true;
            t_2.drop_pending = n;
            t_2.drop_plain = !carry;
            t_2.drop_time = ctx.now;
            task = t_2;
            ctx.orders.deploy(a, type, n);
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "proxy tower at " + (site_v[0]) + "," + (site_v[1]) + " crew " + (n) + java.lang.JStr.valueOf(((carry ? " with wood" : "")));
            }));
        }
        // Java: 99
        public void recrew(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null || ctx.build.dropPending())
            {
                return;
            }
            int n = java.lang.Math.min(com.oddlabs.tt.player.ultra.ProxyManager.CREW, a.getUnitContainer().getNumSupplies() - 2);
            com.oddlabs.tt.model.DeployContainer c = a.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON);
            if (n < 6 || c == null || c.getNumSupplies() > 0)
            {
                return;
            }
            failed++;
            t.drop_pending = n;
            t.drop_plain = true;
            t.drop_time = ctx.now;
            ctx.orders.deploy(a, com.oddlabs.tt.model.DeployType.PEON, n);
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "proxy recrew " + (n) + " -> " + java.lang.JStr.valueOf(t);
            }));
        }
        // Java: 116
        public int[] gathererCluster()
        {
            int best_n = 3;
            int[] best = null;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.peons))
            {
                if (((object)com.oddlabs.tt.player.ultra.WorldModel.visibleActivity(e) != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.GATHER))
                {
                    continue;
                }
                int x = e.getGridX();
                int y = e.getGridY();
                bool covered = false;
                foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
                {
                    if (!t.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) < com.oddlabs.tt.player.ultra.ProxyManager.STANDOFF * com.oddlabs.tt.player.ultra.ProxyManager.STANDOFF)
                    {
                        covered = true;
                        break;
                    }
                }
                if (covered)
                {
                    continue;
                }
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit o in java.lang.JIter.of(ctx.model.enemy.peons))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(o.getGridX(), o.getGridY(), x, y) <= 8)
                    {
                        n++;
                    }
                }
                if (n > best_n)
                {
                    best_n = n;
                    best = new int[] { x, y };
                }
            }
            return best;
        }
        // Java: 146
        public int[] site(int tx, int ty)
        {
            int hx = ctx.map.home_x;
            int hy = ctx.map.home_y;
            return ctx.sites.best(ctx.race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER), tx, ty, 16, new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x, int y) =>
            {
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, tx, ty);
                if (d < 4 || d > 13)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                foreach (com.oddlabs.tt.model.Building e in java.lang.JIter.of(ctx.model.enemy.towers))
                {
                    if (!e.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y) < com.oddlabs.tt.player.ultra.ProxyManager.STANDOFF * com.oddlabs.tt.player.ultra.ProxyManager.STANDOFF)
                    {
                        return java.lang.Float.NEGATIVE_INFINITY;
                    }
                }
                return (float)((float)((float)(-((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(x, y, hx, hy))))) / 10.0f) - d);
            }));
        }
        private void init_ProxyManager()
        {
            this.last_try = java.lang.Float.NEGATIVE_INFINITY;
        }
        public ProxyManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_ProxyManager();
            this.ctx = ctx;
            this.enabled = ctx.strategy.b("proxyTower");
            this.start_time = ctx.strategy.f("proxyTime");
            this.max_towers = ctx.strategy.i("proxyMax");
        }
    }
}

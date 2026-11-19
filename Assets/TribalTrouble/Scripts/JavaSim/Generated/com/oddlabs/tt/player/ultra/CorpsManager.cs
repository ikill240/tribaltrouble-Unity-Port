// Translated from CorpsManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 23
    public class CorpsManager : java.lang.Object
    {
        public const int PER_TOWER = 8;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public java.util.List corps;
        public java.util.Map target;
        public int pending_deploy;
        public bool pending_wood;
        public float pending_time;
        public bool enabled;
        public int size_f;
        // Java: 41
        public int size()
        {
            return corps.size();
        }
        // Java: 45
        public java.util.List members()
        {
            return new java.util.ArrayList(corps);
        }
        // Java: 51
        public void release(com.oddlabs.tt.model.Unit u)
        {
            corps.remove(u);
            target.remove(u);
            if (((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS))
            {
                ctx.clearRole(u);
            }
        }
        // Java: 59
        public void recruitTick()
        {
            corps.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || ((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS);
            }));
            target.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            com.oddlabs.tt.player.ultra.MilitaryManager.State s = ctx.military.state();
            bool attacking = ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.MARCH) || ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.ENGAGE) || ((object)s == (object)com.oddlabs.tt.player.ultra.MilitaryManager.State.SIEGE);
            if (!enabled || !attacking)
            {
                releaseAll();
                return;
            }
            int[] obj = ctx.military.objectivePoint();
            if (obj == null || !towersNear(obj[0], obj[1], 30))
            {
                if (corps.isEmpty())
                {
                    return;
                }
            }
            int want = size_f - corps.size() - pending_deploy;
            if (want <= 0)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (want <= 0)
                {
                    break;
                }
                if (u_2.isDead() || ctx.hasRole(u_2) || com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u_2) || !ctx.economy.isFree(u_2))
                {
                    continue;
                }
                recruit(u_2);
                want--;
            }
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (want > 0 && a != null && !a.isDead() && pending_deploy == 0)
            {
                int spare = a.getUnitContainer().getNumSupplies() - java.lang.Math.max(4, ctx.economy.workersTarget() / 2);
                int n = java.lang.Math.min(want, spare);
                if (n > 0)
                {
                    int wood = a.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies();
                    pending_wood = wood >= n + 8 && !ctx.build.dropPending();
                    ctx.orders.deploy(a, (pending_wood ? com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE : com.oddlabs.tt.model.DeployType.PEON), n);
                    pending_deploy = n;
                    pending_time = ctx.now;
                }
            }
            if (pending_deploy > 0 && (float)(ctx.now - pending_time) > 15.0f)
            {
                pending_deploy = 0;
            }
        }
        // Java: 103
        public void recruit(com.oddlabs.tt.model.Unit u)
        {
            ctx.setRole(u, com.oddlabs.tt.player.ultra.Context.Role.CORPS);
            corps.add(u);
        }
        // Java: 108
        public void releaseAll()
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(corps))
            {
                if (!u.isDead() && ((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.CORPS))
                {
                    ctx.clearRole(u);
                    com.oddlabs.tt.model.Building a = ctx.economy.armory();
                    if (a != null)
                    {
                        ctx.orders.enter(u, a);
                    }
                }
            }
            corps.clear();
            target.clear();
            pending_deploy = 0;
        }
        // Java: 122
        public bool towersNear(int x, int y, int r)
        {
            int r2 = r * r;
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= r2)
                {
                    return true;
                }
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.sites))
            {
                if (!b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER && com.oddlabs.tt.player.ultra.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y) <= r2)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 137
        public void microTick()
        {
            if (!enabled)
            {
                return;
            }
            claimDeployed();
            if (corps.isEmpty())
            {
                return;
            }
            int[] anchor = ctx.military.squadAnchor();
            if (anchor == null)
            {
                return;
            }
            java.util.List towers = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.enemy.towers))
            {
                if (!t.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), anchor[0], anchor[1]) <= 22)
                {
                    towers.add(t);
                }
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.sites))
            {
                if (!b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_TOWER && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(b.getGridX(), b.getGridY(), anchor[0], anchor[1]) <= 22)
                {
                    towers.add(b);
                }
            }
            towers.sort(new java.util.Comparator_L((object x_l, object y_l) =>
            {
                com.oddlabs.tt.model.Building x = (com.oddlabs.tt.model.Building)x_l;
                com.oddlabs.tt.model.Building y = (com.oddlabs.tt.model.Building)y_l;
                int px = priority(x, anchor);
                int py = priority(y, anchor);
                return java.lang.Integer.compare(px, py);
            }));
            java.util.Map assigned = new java.util.LinkedHashMap();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(corps))
            {
                com.oddlabs.tt.model.Selectable t_2 = ((com.oddlabs.tt.model.Selectable)target.get(u));
                if ((t_2 is com.oddlabs.tt.model.Building b_2) && !b_2.isDead() && towers.contains(b_2))
                {
                    assigned.merge(b_2, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                }
            }
            int[] rear = behind(anchor, 5);
            for (int extra = 0; extra < 4 && nearestWarrior(rear[0], rear[1]) < 11; extra++)
            {
                rear = behind(anchor, 9 + 4 * extra);
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(corps))
            {
                if (u_2.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.Selectable t_3 = ((com.oddlabs.tt.model.Selectable)target.get(u_2));
                if (t_3 != null && !t_3.isDead() && (t_3 is com.oddlabs.tt.model.Building b_3) && towers.contains(b_3))
                {
                    if (!((u_2.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController)) && !ctx.orders.recently(u_2, 2.0f))
                    {
                        ctx.orders.attack(u_2, b_3);
                    }
                    continue;
                }
                target.remove(u_2);
                if ((u_2.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hc) && (hc.getTarget() is com.oddlabs.tt.model.Unit))
                {
                    ctx.orders.move(u_2, rear[0], rear[1]);
                    continue;
                }
                com.oddlabs.tt.model.Building pick = null;
                foreach (com.oddlabs.tt.model.Building tw in java.lang.JIter.of(towers))
                {
                    int? n = ((int?)assigned.get(tw));
                    if ((n == null || ((int)n) < com.oddlabs.tt.player.ultra.CorpsManager.PER_TOWER) && safeToAssault(tw))
                    {
                        pick = tw;
                        break;
                    }
                }
                if (pick != null)
                {
                    assigned.merge(pick, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                    target.put(u_2, pick);
                    ctx.orders.attack(u_2, pick);
                    continue;
                }
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u_2.getGridX(), u_2.getGridY(), rear[0], rear[1]) > 4 && !ctx.orders.recently(u_2, com.oddlabs.tt.player.ultra.Orders.Kind.MOVE, null, rear[0], rear[1], 3.0f))
                {
                    ctx.orders.move(u_2, rear[0], rear[1]);
                }
            }
        }
        // Java: 207
        public float nearestWarrior(int x, int y)
        {
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                best = java.lang.Math.min(best, com.oddlabs.tt.player.ultra.MapAnalysis.dist2(e.getGridX(), e.getGridY(), x, y));
            }
            return (best == java.lang.Integer.MAX_VALUE ? java.lang.Float.MAX_VALUE : ((float)(java.lang.Math.sqrt(best))));
        }
        // Java: 214
        public int priority(com.oddlabs.tt.model.Building t, int[] anchor)
        {
            int d = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.getGridX(), t.getGridY(), anchor[0], anchor[1]);
            com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t);
            if (g == null)
            {
                return d;
            }
            if (com.oddlabs.tt.player.ultra.WorldModel.isStunned(g))
            {
                return d - 100;
            }
            return d + 30;
        }
        // Java: 225
        public bool safeToAssault(com.oddlabs.tt.model.Building t)
        {
            com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t);
            if (g != null && com.oddlabs.tt.player.ultra.WorldModel.isStunned(g))
            {
                return true;
            }
            if (ctx.military.stunSieging())
            {
                return false;
            }
            int ours = 0;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ctx.model.me.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(w.getGridX(), w.getGridY(), t.getGridX(), t.getGridY()) <= 14)
                {
                    ours++;
                }
            }
            int theirs = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(e.getGridX(), e.getGridY(), t.getGridX(), t.getGridY()) <= 14)
                {
                    theirs++;
                }
            }
            return ours >= 3 && ours >= 2 * theirs;
        }
        // Java: 244
        public int[] behind(int[] anchor, int dist)
        {
            int[] home = ctx.military.rallyPoint();
            int hx = (home != null ? home[0] : ctx.map.home_x);
            int hy = (home != null ? home[1] : ctx.map.home_y);
            int dx = hx - anchor[0];
            int dy = hy - anchor[1];
            float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
            if (len < 1.0f)
            {
                return anchor;
            }
            int x = ctx.map.clamp(java.lang.Math.round((float)(anchor[0] + (float)((float)(dx / len) * dist))));
            int y = ctx.map.clamp(java.lang.Math.round((float)(anchor[1] + (float)((float)(dy / len) * dist))));
            int[] acc = ctx.map.nearestAccessible(x, y);
            return (acc != null ? acc : anchor);
        }
        // Java: 260
        public void claimDeployed()
        {
            if (pending_deploy <= 0)
            {
                return;
            }
            int[] rally = ctx.military.rallyPoint();
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (rally == null || a == null)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (pending_deploy <= 0)
                {
                    return;
                }
                if (u.isDead() || ctx.hasRole(u) || (com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u) && !pending_wood))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.WorldModel.Activity act = com.oddlabs.tt.player.ultra.WorldModel.activityOf(u);
                if (((object)act != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.WALK) && ((object)act != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.IDLE))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), a.getGridX(), a.getGridY()) > 16 && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), rally[0], rally[1]) > 6)
                {
                    continue;
                }
                if (!u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                {
                    continue;
                }
                recruit(u);
                pending_deploy--;
            }
        }
        private void init_CorpsManager()
        {
            this.corps = new java.util.ArrayList();
            this.target = new java.util.LinkedHashMap();
        }
        public CorpsManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_CorpsManager();
            this.ctx = ctx;
            this.enabled = ctx.strategy.b("corps");
            this.size_f = ctx.strategy.i("corpsSize");
        }
    }
}

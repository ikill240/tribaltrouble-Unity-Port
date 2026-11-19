// Translated from TowerManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 22
    public class TowerManager : java.lang.Object
    {
        // Java: 23
        public class Slot : java.lang.Object
        {
            public string name;
            public float angle;
            public float radius;
            public com.oddlabs.tt.player.ultra.BuildManager.Task task;
            public Slot(string name, float angle_degrees, float radius) : base()
            {
                this.name = name;
                this.angle = ((float)(java.lang.StrictMath.toRadians(angle_degrees)));
                this.radius = radius;
            }
        }
        public com.oddlabs.tt.player.ultra.Context ctx;
        public java.util.List slots;
        public java.util.Map pending_garrison;
        public java.util.Map last_order;
        public java.util.Map evacuated_until;
        public java.util.Map repairer;
        public float c1_time;
        public float c2_time;
        public int tower_max;
        public int tower_builders;
        public int tower_threat_r;
        public bool tower_schedule;
        public int tower_early_cap;
        public float tower_early_time;
        public int tower_early_quarters;
        public float last_tower_request;
        public int late_towers;
        public float late_time;
        public int late_reserve;
        public int tower_open_max;
        public bool hot_now;
        public int tower_open_hot;
        public int tower_prio_low;
        // Java: 104
        public bool hot()
        {
            return hot_now;
        }
        // Java: 108
        public bool slotAvailableForQuarters()
        {
            int used = ctx.owner.getBuildingCountContainer().getNumSupplies() + ctx.build.pendingSlots();
            int reserve = java.lang.Math.max(0, java.lang.Math.min(4, tower_max) - ctx.model.me.towers.size());
            return used + 1 <= ctx.world.getMaxBuildingCount() - reserve;
        }
        // Java: 114
        public int desiredTowers()
        {
            if (tower_schedule)
            {
                return scheduledTowers();
            }
            int enemy_warriors = ctx.model.enemy.warriors.size();
            int n = 0;
            if (ctx.now >= c1_time || enemy_warriors >= 2)
            {
                n = 1;
            }
            if (ctx.now >= c2_time || enemy_warriors >= 4)
            {
                n = 2;
            }
            if (ctx.now >= (float)(c2_time + 120.0f) || enemy_warriors >= 10)
            {
                n = 3;
            }
            if (enemy_warriors >= 16 || ctx.now >= (float)(c2_time + 240.0f))
            {
                n = 4;
            }
            float ours = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(ctx.model.me.warriors);
            float theirs = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(ctx.model.enemy.warriors);
            com.oddlabs.tt.model.Building home = ctx.economy.armory();
            if (tower_threat_r > 0 && home != null && !home.isDead())
            {
                theirs = 0.0f;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(e.getGridX(), e.getGridY(), home.getGridX(), home.getGridY()) <= tower_threat_r)
                    {
                        theirs = (float)(theirs + com.oddlabs.tt.player.ultra.CombatModel.weight(e));
                    }
                }
            }
            n += java.lang.Math.max(0, java.lang.JC.f2i(((float)(((float)(theirs - ours)) / 6.0f))));
            n = java.lang.Math.min(n, java.lang.Math.min(tower_max, slots.size()));
            if (tower_early_cap > 0 && ctx.now < tower_early_time && ctx.model.me.quarters.size() < tower_early_quarters)
            {
                com.oddlabs.tt.model.Building a = ctx.economy.armory();
                bool raid = a != null && !a.isDead() && enemyWarriorsNear(a, 45) >= 4;
                if (!raid)
                {
                    n = java.lang.Math.min(n, tower_early_cap);
                }
            }
            if (late_towers > 0 && ctx.now >= late_time && ((object)ctx.economy.phase() == (object)com.oddlabs.tt.player.ultra.EconomyManager.Phase.CAP) && ctx.owner.getUnitCountContainer().getNumSupplies() >= ctx.world.getMaxUnitCount() - 15)
            {
                int used = ctx.owner.getBuildingCountContainer().getNumSupplies() + ctx.build.pendingSlots();
                int spare = ctx.world.getMaxBuildingCount() - late_reserve - used;
                int have = ctx.model.me.towers.size() + ctx.build.openCount(com.oddlabs.tt.model.Race.BUILDING_TOWER);
                if (spare > 0)
                {
                    n = java.lang.Math.max(n, java.lang.Math.min(slots.size(), java.lang.Math.min(tower_max + late_towers, have + 1)));
                }
            }
            return n;
        }
        // Java: 167
        public int scheduledTowers()
        {
            float t = ctx.now;
            int n = (t >= c2_time ? 2 : (t >= c1_time ? 1 : 0));
            if (t >= 480.0f)
            {
                n = 4;
            }
            if (t >= 720.0f)
            {
                n = 6 + java.lang.JC.f2i(((float)(((float)(t - 720.0f)) / 60.0f)));
            }
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a != null && !a.isDead() && enemyWarriorsNear(a, 45) >= 4)
            {
                n++;
            }
            int others = ctx.owner.getBuildingCountContainer().getNumSupplies() + ctx.build.pendingSlots() - ctx.model.me.towers.size() - ctx.build.openCount(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            n = java.lang.Math.min(n, java.lang.Math.max(2, ctx.world.getMaxBuildingCount() - 2 - others));
            return java.lang.Math.min(n, slots.size());
        }
        // Java: 186
        public void planTick()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null || a.isDead())
            {
                return;
            }
            int want = desiredTowers();
            int have = 0;
            foreach (com.oddlabs.tt.player.ultra.TowerManager.Slot s in java.lang.JIter.of(slots))
            {
                com.oddlabs.tt.player.ultra.BuildManager.Task t = s.task;
                if (t != null && t.open())
                {
                    have++;
                }
            }
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (!t_2.isDead() && nearHome(t_2))
                {
                    have++;
                }
            }
            int manned = 0;
            foreach (com.oddlabs.tt.model.Building t_3 in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (!t_3.isDead() && t_3.getUnitCount() > 0)
                {
                    manned++;
                }
            }
            int weapons = a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() + a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int crews = ctx.military.armySize() + weapons + manned;
            bool hot_v = ctx.economy.emergency || enemyWarriorsNear(a, 45) >= 4;
            hot_now = hot_v;
            bool low = tower_prio_low > 0 && !hot_v && ctx.model.me.quarters.size() < 4;
            if (tower_prio_low > 0 && !low)
            {
                foreach (com.oddlabs.tt.player.ultra.TowerManager.Slot s_2 in java.lang.JIter.of(slots))
                {
                    com.oddlabs.tt.player.ultra.BuildManager.Task t_4 = s_2.task;
                    if (t_4 != null && t_4.open() && t_4.priority < 800 - slots.indexOf(s_2))
                    {
                        ctx.build.reprioritize(t_4, 800 - slots.indexOf(s_2));
                    }
                }
            }
            if (have >= want || crews < have + 1)
            {
                return;
            }
            if (!ctx.owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER))
            {
                return;
            }
            if (tower_schedule && (float)(ctx.now - last_tower_request) < 15.0f)
            {
                return;
            }
            if (tower_open_max > 0)
            {
                int open = 0;
                foreach (com.oddlabs.tt.player.ultra.TowerManager.Slot s_3 in java.lang.JIter.of(slots))
                {
                    com.oddlabs.tt.player.ultra.BuildManager.Task t_5 = s_3.task;
                    if (t_5 != null && t_5.open())
                    {
                        open++;
                    }
                }
                int cap = (hot_v ? tower_open_hot : tower_open_max);
                if (cap > 0 && open >= cap)
                {
                    return;
                }
            }
            foreach (com.oddlabs.tt.player.ultra.TowerManager.Slot s_4 in java.lang.JIter.of(slots))
            {
                if (com.oddlabs.tt.player.ultra.TowerManager.held(s_4, a))
                {
                    continue;
                }
                int[] site = siteFor(s_4, a);
                if (site == null)
                {
                    continue;
                }
                int prio = ((low ? tower_prio_low : 800)) - slots.indexOf(s_4);
                s_4.task = ctx.build.request(com.oddlabs.tt.model.Race.BUILDING_TOWER, site[0], site[1], prio, tower_builders, s_4.name);
                last_tower_request = ctx.now;
                return;
            }
        }
        // Java: 255
        public int enemyWarriorsNear(com.oddlabs.tt.model.Building a, int r)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.warriors))
            {
                if (!e.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(e.getGridX(), e.getGridY(), a.getGridX(), a.getGridY()) <= r)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 268
        public bool nearHome(com.oddlabs.tt.model.Building t)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.me.buildings))
            {
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                if (!b.isDead() && (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY || id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS) && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(b.getGridX(), b.getGridY(), t.getGridX(), t.getGridY()) <= 25)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 278
        public static bool held(com.oddlabs.tt.player.ultra.TowerManager.Slot s, com.oddlabs.tt.model.Building a)
        {
            com.oddlabs.tt.player.ultra.BuildManager.Task t = s.task;
            if (t == null)
            {
                return false;
            }
            if (t.open())
            {
                return true;
            }
            com.oddlabs.tt.model.Building b = t.building;
            return b != null && !b.isDead() && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(b.getGridX(), b.getGridY(), a.getGridX(), a.getGridY()) <= (float)(s.radius + 8);
        }
        // Java: 289
        public int[] siteFor(com.oddlabs.tt.player.ultra.TowerManager.Slot s, com.oddlabs.tt.model.Building a)
        {
            float @base = ((float)(java.lang.StrictMath.atan2(ctx.map.enemy_y - a.getGridY(), ctx.map.enemy_x - a.getGridX())));
            float ang = (float)(@base + s.angle);
            int sx = java.lang.Math.round((float)(a.getGridX() + (float)(s.radius * ((float)(java.lang.StrictMath.cos(ang))))));
            int sy = java.lang.Math.round((float)(a.getGridY() + (float)(s.radius * ((float)(java.lang.StrictMath.sin(ang))))));
            int ax = a.getGridX();
            int ay = a.getGridY();
            return ctx.sites.best(ctx.race.getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER), sx, sy, 4, new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x, int y) =>
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, ax, ay) < 7)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                float d = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.ultra.MapAnalysis.dist2(x, y, sx, sy))));
                return (float)((float)(ctx.map.height(x, y) * 0.3f) - d);
            }));
        }
        // Java: 304
        public int[] replanSite(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            foreach (com.oddlabs.tt.player.ultra.TowerManager.Slot s in java.lang.JIter.of(slots))
            {
                if (((object)s.task == (object)t) && a != null)
                {
                    return siteFor(s, a);
                }
            }
            return null;
        }
        // Java: 315
        public bool isPendingGarrison(com.oddlabs.tt.model.Unit w)
        {
            return pending_garrison.containsValue(w);
        }
        // Java: 319
        public void garrisonTick()
        {
            java.util.List stale = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(pending_garrison.entrySet()))
            {
                com.oddlabs.tt.model.Building t = ((com.oddlabs.tt.model.Building)e.getKey());
                if (t.isDead() || (t.getUnitCount() > 0 && ((object)com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t) != (object)((com.oddlabs.tt.model.Unit)e.getValue()))))
                {
                    stale.add(t);
                }
            }
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(stale))
            {
                com.oddlabs.tt.model.Unit w = ((com.oddlabs.tt.model.Unit)pending_garrison.remove(t_2));
                if (w != null)
                {
                    ctx.military.releaseFromGarrison(w);
                }
            }
            evacuated_until.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            repairTowers();
            foreach (com.oddlabs.tt.model.Building t_3 in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (t_3.isDead())
                {
                    continue;
                }
                if (t_3.getUnitCount() > 0)
                {
                    pending_garrison.remove(t_3);
                    continue;
                }
                float? until = ((float?)evacuated_until.get(t_3));
                if (until != null && ctx.now < ((float)until) && t_3.getHitPoints() < 40)
                {
                    continue;
                }
                if (t_3.getHitPoints() <= 20 && enemiesAdjacent(t_3) > 0)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit w_2 = ((com.oddlabs.tt.model.Unit)pending_garrison.get(t_3));
                if (w_2 != null && !w_2.isDead() && !w_2.isMounted())
                {
                    if (!ctx.orders.recently(w_2, com.oddlabs.tt.player.ultra.Orders.Kind.ENTER, t_3, t_3.getGridX(), t_3.getGridY(), 6.0f))
                    {
                        ctx.orders.enter(w_2, t_3);
                    }
                    continue;
                }
                if (w_2 != null && !w_2.isMounted())
                {
                    ctx.military.releaseFromGarrison(w_2);
                }
                w_2 = ctx.military.takeForGarrison(t_3.getGridX(), t_3.getGridY());
                if (w_2 == null)
                {
                    continue;
                }
                pending_garrison.put(t_3, w_2);
                ctx.orders.enter(w_2, t_3);
            }
        }
        // Java: 362
        public void microTick()
        {
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ctx.model.me.towers))
            {
                com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t);
                if (g == null || com.oddlabs.tt.player.ultra.WorldModel.isStunned(g))
                {
                    continue;
                }
                evacuateIfDoomed(t);
                if (t.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.Selectable current = null;
                if ((g.getCurrentController() is com.oddlabs.tt.model.behaviour.AttackController ac))
                {
                    current = ac.getTarget();
                }
                bool idle = (g.getCurrentController() is com.oddlabs.tt.model.behaviour.IdleController);
                int cur_class = (current != null && !current.isDead() ? classOf(t, current) : 99);
                com.oddlabs.tt.model.Selectable best = null;
                int best_class = 99;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.units))
                {
                    int d2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(g.getGridX(), g.getGridY(), e.getGridX(), e.getGridY());
                    if (d2 > 300 || !g.isCloseEnough(g.getRange(e), e))
                    {
                        continue;
                    }
                    int c = classOf(t, e);
                    if (c < best_class || (c == best_class && d2 < best_d))
                    {
                        best_class = c;
                        best_d = d2;
                        best = e;
                    }
                }
                if (best == null)
                {
                    foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.enemy.buildings))
                    {
                        if (b.isDead() || !g.isCloseEnough(g.getRange(b), b))
                        {
                            continue;
                        }
                        int c_2 = classOf(t, b);
                        int d2_2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(g.getGridX(), g.getGridY(), b.getGridX(), b.getGridY());
                        if (c_2 < best_class || (c_2 == best_class && d2_2 < best_d))
                        {
                            best_class = c_2;
                            best_d = d2_2;
                            best = b;
                        }
                    }
                }
                if (best == null || ((object)best == (object)current))
                {
                    continue;
                }
                float? last = ((float?)last_order.get(t));
                bool rate_ok = last == null || (float)(ctx.now - ((float)last)) >= 1.5f || best_class <= 1;
                if ((idle || cur_class == 99 || best_class < cur_class) && rate_ok)
                {
                    ctx.orders.towerTarget(t, best);
                    last_order.put(t, ctx.now);
                }
            }
        }
        // Java: 414
        public int classOf(com.oddlabs.tt.model.Building tower, com.oddlabs.tt.model.Selectable e)
        {
            if ((e is com.oddlabs.tt.model.Unit u))
            {
                com.oddlabs.tt.player.ultra.WorldModel.Kind k = com.oddlabs.tt.player.ultra.WorldModel.kindOf(u);
                if (((object)k == (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.PEON) || ((object)k == (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.CHIEFTAIN))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), tower.getGridX(), tower.getGridY()) <= 3)
                    {
                        return 1;
                    }
                    if (((object)com.oddlabs.tt.player.ultra.WorldModel.visibleActivity(u) == (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.BUILD))
                    {
                        return 2;
                    }
                    return (((object)k == (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.CHIEFTAIN) ? 4 : 5);
                }
                return ((k).ordinal() switch { 3 /*RUBBER*/ => (int)(3), 2 /*IRON*/ => (int)(3), _ => (int)(4) });
            }
            if ((e is com.oddlabs.tt.model.Building b))
            {
                if (!b.isComplete() && b.getHitPoints() <= 20)
                {
                    return 2;
                }
                return 6;
            }
            return 7;
        }
        // Java: 438
        public int enemiesAdjacent(com.oddlabs.tt.model.Building t)
        {
            int adjacent = 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ctx.model.enemy.units))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(e.getGridX(), e.getGridY(), t.getGridX(), t.getGridY()) <= 3)
                {
                    adjacent++;
                }
            }
            return adjacent;
        }
        // Java: 448
        public void evacuateIfDoomed(com.oddlabs.tt.model.Building t)
        {
            if (t.getHitPoints() > 14 || !t.canExitTower())
            {
                return;
            }
            int adjacent = enemiesAdjacent(t);
            if (adjacent == 0)
            {
                return;
            }
            if (6 * adjacent < t.getHitPoints() - 6)
            {
                return;
            }
            com.oddlabs.tt.model.Unit g = com.oddlabs.tt.player.ultra.WorldModel.garrisonOf(t);
            if (ctx.orders.exitTower(t) && g != null)
            {
                pending_garrison.remove(t);
                evacuated_until.put(t, (float)(ctx.now + 20.0f));
                ctx.military.releaseFromGarrison(g);
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "evacuated tower at " + (t.getGridX()) + "," + (t.getGridY());
                }));
            }
        }
        // Java: 466
        public void repairTowers()
        {
            repairer.keySet().removeIf(new java.util.function.Predicate_L((object t_l) =>
            {
                com.oddlabs.tt.model.Building t = (com.oddlabs.tt.model.Building)t_l;
                return t.isDead() || !t.isDamaged();
            }));
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(ctx.model.me.towers))
            {
                if (t_2.isDead() || !t_2.isDamaged() || t_2.getHitPoints() > 85)
                {
                    continue;
                }
                if (ctx.resources.threatened(t_2.getGridX(), t_2.getGridY(), 12))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit r = ((com.oddlabs.tt.model.Unit)repairer.get(t_2));
                if (r != null && !r.isDead() && ((object)com.oddlabs.tt.player.ultra.WorldModel.buildTarget(r) == (object)t_2))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_d = 30 * 30;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
                {
                    bool wood_gatherer = !ctx.hasRole(u) && (com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc) && ((object)gc.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply));
                    if (u.isDead() || (!ctx.economy.isFree(u) && !wood_gatherer))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t_2.getGridX(), t_2.getGridY());
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
                if (best != null)
                {
                    ctx.orders.repair(best, t_2);
                    repairer.put(t_2, best);
                }
            }
        }
        private void init_TowerManager()
        {
            this.slots = new java.util.ArrayList();
            this.pending_garrison = new java.util.LinkedHashMap();
            this.last_order = new java.util.LinkedHashMap();
            this.evacuated_until = new java.util.LinkedHashMap();
            this.repairer = new java.util.LinkedHashMap();
            this.last_tower_request = java.lang.Float.NEGATIVE_INFINITY;
        }
        public TowerManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_TowerManager();
            this.ctx = ctx;
            c1_time = ctx.strategy.f("c1Time");
            c2_time = ctx.strategy.f("c2Time");
            tower_max = ctx.strategy.i("towerMax");
            tower_builders = ctx.strategy.i("towerBuilders");
            tower_threat_r = ctx.strategy.i("towerThreatR");
            tower_schedule = ctx.strategy.b("towerSchedule");
            tower_early_cap = ctx.strategy.i("towerEarlyCap");
            tower_early_time = ctx.strategy.f("towerEarlyTime");
            tower_early_quarters = ctx.strategy.i("towerEarlyQuarters");
            tower_open_max = ctx.strategy.i("towerOpenMax");
            tower_open_hot = ctx.strategy.i("towerOpenHot");
            tower_prio_low = ctx.strategy.i("towerPrioLow");
            float r = ctx.strategy.f("towerRadius");
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C1", 0.0f, r));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C2", 45.0f, (float)(r + 2)));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C3", (float)(-45.0f), (float)(r + 2)));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C4", 110.0f, r));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C5", (float)(-110.0f), r));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C6", 180.0f, r));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C7", 25.0f, (float)(r + 8)));
            slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C8", (float)(-25.0f), (float)(r + 8)));
            late_towers = java.lang.Math.max(0, java.lang.Math.min(4, ctx.strategy.i("lateTowers")));
            late_time = ctx.strategy.f("lateTowerTime");
            late_reserve = ctx.strategy.i("lateReserve");
            float[][] late = new float[][] { new float[] { 70.0f, (float)(r + 8) }, new float[] { (float)(-70.0f), (float)(r + 8) }, new float[] { 0.0f, (float)(r + 16) }, new float[] { 145.0f, (float)(r + 6) } };
            for (int i = 0; i < late_towers; i++)
            {
                slots.add(new com.oddlabs.tt.player.ultra.TowerManager.Slot("C" + ((9 + i)), late[i][0], late[i][1]));
            }
        }
    }
}

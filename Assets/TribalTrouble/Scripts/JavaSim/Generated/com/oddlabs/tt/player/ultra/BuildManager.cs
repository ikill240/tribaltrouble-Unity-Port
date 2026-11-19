// Translated from BuildManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 26
    public class BuildManager : java.lang.Object
    {
        // Java: 27
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State PLANNED = new State("PLANNED", 0);
            public static readonly State ORDERED = new State("ORDERED", 1);
            public static readonly State PLACED = new State("PLACED", 2);
            public static readonly State DONE = new State("DONE", 3);
            public static readonly State FAILED = new State("FAILED", 4);
            public static readonly State[] VALUES_ = new State[] { PLANNED, ORDERED, PLACED, DONE, FAILED };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 35
        public class Task : java.lang.Object
        {
            public int type;
            public int x;
            public int y;
            public int priority;
            public string name;
            public int desired;
            public com.oddlabs.tt.model.Building building;
            public com.oddlabs.tt.player.ultra.BuildManager.State state;
            public int tries;
            public float state_time;
            public float placer_missing;
            public bool manual;
            public int drop_pending;
            public float drop_time;
            public bool drop_plain;
            public int place_failures;
            public bool full_crew;
            // Java: 71
            public bool open()
            {
                return ((object)state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED) || ((object)state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED) || ((object)state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED);
            }
            // Java: 75
            public int maxHp(com.oddlabs.tt.player.ultra.Context ctx)
            {
                return ctx.race.getBuildingTemplate(type).getMaxHitPoints();
            }
            // Java: 79
            public override string toString()
            {
                return java.lang.JStr.valueOf(name) + "@" + (x) + "," + (y) + ":" + java.lang.JStr.valueOf(state);
            }
            private void init_Task()
            {
                this.state = com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED;
            }
            public Task(int type, int x, int y, int priority, int desired, string name) : base()
            {
                init_Task();
                this.type = type;
                this.x = x;
                this.y = y;
                this.priority = priority;
                this.desired = desired;
                this.name = name;
            }
        }
        public const int LONG_WALK = 14;
        public const int OPENING_BUILDER_CAP = 30;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public java.util.List tasks_f;
        public java.util.Map assignment;
        public com.oddlabs.tt.model.Unit scout;
        public bool opening;
        public int foundation_lead;
        public float j_walk;
        public float j_danger_r;
        public int hub_radius;
        public int q1_radius;
        public bool wood_drop;
        public int drop_wait_builders;
        public int unplaced_crew;
        public int armory_iron_units;
        public int hub_x;
        public int hub_y;
        // Java: 121
        public bool inOpening()
        {
            return opening;
        }
        // Java: 126
        public bool inOpeningPlan()
        {
            return tasks_f.isEmpty() && hub_x < 0;
        }
        // Java: 130
        public int hubX()
        {
            return hub_x;
        }
        // Java: 134
        public int hubY()
        {
            return hub_y;
        }
        // Java: 138
        public java.util.List tasks()
        {
            return tasks_f;
        }
        // Java: 143
        public com.oddlabs.tt.player.ultra.BuildManager.Task taskOf(com.oddlabs.tt.model.Unit u)
        {
            return ((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u));
        }
        // Java: 149
        public com.oddlabs.tt.model.Building completedBuilding(int type)
        {
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                com.oddlabs.tt.model.Building b = t.building;
                if (t.type == type && ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.DONE) && b != null && !b.isDead() && b.isComplete())
                {
                    return b;
                }
            }
            return null;
        }
        // Java: 159
        public int openCount(int type)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (t.type == type && t.open())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 169
        public int pendingSlots()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED) || ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 178
        public com.oddlabs.tt.player.ultra.BuildManager.Task request(int type, int x, int y, int priority, int desired, string name)
        {
            com.oddlabs.tt.player.ultra.BuildManager.Task t = new com.oddlabs.tt.player.ultra.BuildManager.Task(type, x, y, priority, desired, name);
            t.state_time = ctx.now;
            ctx.sites.reserve(x, y, ctx.race.getBuildingTemplate(type).getPlacingSize());
            int i = 0;
            while (i < tasks_f.size() && ((com.oddlabs.tt.player.ultra.BuildManager.Task)tasks_f.get(i)).priority >= priority)
            {
                i++;
            }
            tasks_f.add(i, t);
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "build request " + java.lang.JStr.valueOf(t);
            }));
            return t;
        }
        // Java: 192
        public void reprioritize(com.oddlabs.tt.player.ultra.BuildManager.Task t, int priority)
        {
            if (!tasks_f.remove(t))
            {
                return;
            }
            t.priority = priority;
            int i = 0;
            while (i < tasks_f.size() && ((com.oddlabs.tt.player.ultra.BuildManager.Task)tasks_f.get(i)).priority >= priority)
            {
                i++;
            }
            tasks_f.add(i, t);
        }
        // Java: 202
        public void cancel(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            if (!t.open())
            {
                return;
            }
            t.state = com.oddlabs.tt.player.ultra.BuildManager.State.FAILED;
            ctx.sites.unreserve(t.x, t.y);
            releaseBuildersOf(t);
        }
        // Java: 214
        public void planOpening(bool threatened)
        {
            java.util.List peons = ctx.model.me.peons;
            if (peons.isEmpty())
            {
                return;
            }
            ctx.map.refreshTrees(ctx.now, 0.0f);
            long sx = 0;
            long sy = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(peons))
            {
                sx += u.getGridX();
                sy += u.getGridY();
            }
            int cx = unchecked((int)((sx / peons.size())));
            int cy = unchecked((int)((sy / peons.size())));
            int[] hub = planArmorySite(ctx.map.home_x, ctx.map.home_y, hub_radius);
            if (hub == null)
            {
                hub = new int[] { ctx.map.home_x, ctx.map.home_y };
            }
            hub_x = hub[0];
            hub_y = hub[1];
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "hub " + (hub_x) + "," + (hub_y) + " home " + (ctx.map.home_x) + "," + (ctx.map.home_y);
            }));
            int[] order = ctx.strategy.openingOrder(threatened);
            int prev_x = cx;
            int prev_y = cy;
            int priority = 1000;
            int[] armory_site = null;
            int quarters_total = 0;
            foreach (int type in order)
            {
                if (type == com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
                {
                    quarters_total++;
                }
            }
            int quarters_done = 0;
            foreach (int type_2 in order)
            {
                if (type_2 == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    armory_site = (ctx.sites.isLegal(template(com.oddlabs.tt.model.Race.BUILDING_ARMORY), hub_x, hub_y) ? new int[] { hub_x, hub_y } : planArmorySite(hub_x, hub_y, 12));
                    if (armory_site == null)
                    {
                        armory_site = planArmorySite(ctx.map.home_x, ctx.map.home_y, hub_radius);
                    }
                    if (armory_site == null)
                    {
                        continue;
                    }
                    request(com.oddlabs.tt.model.Race.BUILDING_ARMORY, armory_site[0], armory_site[1], priority--, (-1), "A");
                    prev_x = armory_site[0];
                    prev_y = armory_site[1];
                }
                else
                {
                    bool first = quarters_done == 0;
                    int lx = prev_x;
                    int ly = prev_y;
                    if (!first)
                    {
                        float f = (float)(1.0f / (quarters_total - quarters_done + 1));
                        lx = java.lang.Math.round((float)(prev_x + (float)((hub_x - prev_x) * f)));
                        ly = java.lang.Math.round((float)(prev_y + (float)((hub_y - prev_y) * f)));
                    }
                    ctx.sites.reserve(hub_x, hub_y, 7);
                    int[] q = planQuartersSite(lx, ly, prev_x, prev_y, (first ? q1_radius : 36));
                    if (q == null)
                    {
                        q = planQuartersSite(lx, ly, prev_x, prev_y, 48);
                    }
                    ctx.sites.unreserve(hub_x, hub_y);
                    quarters_done++;
                    if (q == null)
                    {
                        continue;
                    }
                    request(type_2, q[0], q[1], priority--, (-1), "Q" + ((openCount(com.oddlabs.tt.model.Race.BUILDING_QUARTERS) + 1)));
                    prev_x = q[0];
                    prev_y = q[1];
                }
            }
            com.oddlabs.tt.player.ultra.BuildManager.Task first_2 = firstOpenTask();
            if (first_2 != null)
            {
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(peons))
                {
                    int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), first_2.x, first_2.y);
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u_2;
                    }
                }
                scout = best;
                if (scout != null)
                {
                    orderPlace(first_2, scout);
                }
            }
        }
        // Java: 300
        public com.oddlabs.tt.player.ultra.BuildManager.Task firstOpenTask()
        {
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (t.open())
                {
                    return t;
                }
            }
            return null;
        }
        public float last_report;
        // Java: 313
        public void tick(float dt)
        {
            if (com.oddlabs.tt.player.ultra.UltraLog.enabled() && (float)(ctx.now - last_report) >= 15.0f)
            {
                last_report = ctx.now;
                java.lang.StringBuilder sb = new java.lang.StringBuilder("tasks:");
                foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
                {
                    if (!t.open())
                    {
                        continue;
                    }
                    int hp = (t.building != null && !t.building.isDead() && t.building.isPlaced() ? t.building.getHitPoints() : 0);
                    sb.append(' ').append(t.name).append('[').append(t.state.name().charAt(0)).append(" hp=").append(hp).append(" b=").append(builderCount(t)).append('/').append(t.desired).append(" need=").append(woodNeed(t)).append(']');
                }
                foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t_2 in java.lang.JIter.of(tasks_f))
                {
                    if (!t_2.open() || t_2.building == null || ((object)t_2.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED) || woodNeed(t_2) > 0)
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
                    {
                        if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) != (object)t_2))
                        {
                            continue;
                        }
                        sb.append(java.lang.JSystem.lineSeparator()).append("      ").append(t_2.name).append(" builder ").append(u.getGridX()).append(',').append(u.getGridY()).append(' ').append(com.oddlabs.tt.player.ultra.WorldModel.activityOf(u)).append(" wood=").append(com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u)).append(" cur=").append(u.getCurrentController().GetType().getSimpleName()).append(" beh=").append((u.getCurrentBehaviour() == null ? "-" : u.getCurrentBehaviour().GetType().getSimpleName()));
                    }
                }
                string line = java.lang.JO.toString(sb);
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return line;
                }));
            }
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t_3 in java.lang.JIter.of(tasks_f))
            {
                if (t_3.open())
                {
                    updateState(t_3, dt);
                }
            }
            tasks_f.removeIf(new java.util.function.Predicate_L((object t_4_l) =>
            {
                com.oddlabs.tt.player.ultra.BuildManager.Task t_4 = (com.oddlabs.tt.player.ultra.BuildManager.Task)t_4_l;
                return !t_4.open() && (float)(ctx.now - t_4.state_time) > 30.0f;
            }));
            if (opening && ctx.economy.armory() != null)
            {
                endOpening();
            }
            if (opening)
            {
                safeSwitch();
                scoutTick();
            }
            if (!opening)
            {
                woodDrops();
                claimTransporters();
            }
            builderTick();
        }
        public const int DROP_RANGE = 36;
        public const int WOOD_KEEP = 6;
        // Java: 368
        public bool dropEligible(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            return wood_drop && a != null && !opening && t.desired >= 0 && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(t.x, t.y, a.getGridX(), a.getGridY()) <= com.oddlabs.tt.player.ultra.BuildManager.DROP_RANGE;
        }
        // Java: 375
        public bool dropPending()
        {
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (t.drop_pending > 0)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 384
        public int woodDemand()
        {
            int demand = 0;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (t.open() && dropEligible(t))
                {
                    demand += java.lang.Math.max(0, woodNeed(t) - t.drop_pending);
                }
            }
            return java.lang.Math.min(demand, 20);
        }
        // Java: 393
        public void woodDrops()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null || a.isDead() || !wood_drop)
            {
                return;
            }
            com.oddlabs.tt.model.DeployContainer c = a.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE);
            if (c == null || c.getNumSupplies() > 0)
            {
                return;
            }
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (t.drop_pending > 0 && (float)(ctx.now - t.drop_time) > 25.0f)
                {
                    t.drop_pending = 0;
                }
            }
            if (dropPending())
            {
                return;
            }
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t_2 in java.lang.JIter.of(tasks_f))
            {
                if (!t_2.open() || !dropEligible(t_2) || t_2.building == null || t_2.drop_pending > 0)
                {
                    continue;
                }
                if (((object)t_2.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED))
                {
                    continue;
                }
                if (ctx.resources.threatened(t_2.x, t_2.y, 14))
                {
                    continue;
                }
                int need = woodNeed(t_2);
                if (need <= 0)
                {
                    continue;
                }
                int stock = a.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies() - com.oddlabs.tt.player.ultra.BuildManager.WOOD_KEEP;
                int inside = a.getUnitContainer().getNumSupplies() - 2;
                int n = java.lang.Math.min(need, java.lang.Math.min(stock, inside));
                if (n <= 0 || (n < need && n < 8))
                {
                    continue;
                }
                ctx.orders.deploy(a, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, n);
                t_2.drop_pending = n;
                t_2.drop_time = ctx.now;
                int count = n;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "wood drop " + (count) + " -> " + java.lang.JStr.valueOf(t_2);
                }));
                return;
            }
        }
        // Java: 434
        public void claimTransporters()
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null)
            {
                return;
            }
            bool any = false;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (t.drop_pending > 0)
                {
                    any = true;
                    break;
                }
            }
            if (!any)
            {
                return;
            }
            int[] rally = ctx.military.rallyPoint();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (u.isDead() || ctx.hasRole(u))
                {
                    continue;
                }
                bool wood = com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u);
                if (!wood && com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u))
                {
                    continue;
                }
                bool near = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), a.getGridX(), a.getGridY()) <= 12 || (rally != null && com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), rally[0], rally[1]) <= 6);
                if (!near)
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.WorldModel.Activity act = com.oddlabs.tt.player.ultra.WorldModel.activityOf(u);
                if (((object)act != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.WALK) && ((object)act != (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.IDLE))
                {
                    continue;
                }
                if (!wood && ctx.orders.lastOrder(u) != null)
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.BuildManager.Task best = null;
                foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t_2 in java.lang.JIter.of(tasks_f))
                {
                    if (t_2.drop_pending > 0 && t_2.open() && (t_2.building != null || t_2.manual) && (wood || t_2.drop_plain))
                    {
                        best = t_2;
                        break;
                    }
                }
                if (best == null)
                {
                    continue;
                }
                com.oddlabs.tt.model.Building bb = best.building;
                if (bb == null || bb.isDead() || !bb.isPlaced() || ((object)best.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED))
                {
                    if (!placeWith(best, u))
                    {
                        break;
                    }
                    best.drop_pending--;
                }
                else
                {
                    best.drop_pending--;
                    ctx.setRole(u, com.oddlabs.tt.player.ultra.Context.Role.BUILDER);
                    assignment.put(u, best);
                    ctx.orders.build(u, best.building);
                }
            }
        }
        public bool safe_mode;
        // Java: 495
        public void safeSwitch()
        {
            if (safe_mode || !ctx.strategy.b("safeSwitch"))
            {
                return;
            }
            com.oddlabs.tt.player.ultra.WorldModel.Side e = ctx.model.enemy;
            float threat = com.oddlabs.tt.player.ultra.CombatModel.sumWeights(e.warriors);
            if (!e.armories.isEmpty() && e.quarters.size() < 3)
            {
                threat = (float)(threat + 2.5f);
            }
            if (threat < 2.5f)
            {
                return;
            }
            com.oddlabs.tt.player.ultra.BuildManager.Task armory = null;
            int top = java.lang.Integer.MIN_VALUE;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (!t.open())
                {
                    continue;
                }
                if (t.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY && armory == null)
                {
                    armory = t;
                }
                else if (t.type == com.oddlabs.tt.model.Race.BUILDING_QUARTERS && t.desired < 0 && (t.building == null || !t.building.isPlaced() || t.building.getHitPoints() < 120))
                {
                    top = java.lang.Math.max(top, t.priority);
                }
            }
            safe_mode = true;
            if (armory == null || top == java.lang.Integer.MIN_VALUE || armory.priority > top)
            {
                return;
            }
            armory.priority = top + 1;
            tasks_f.remove(armory);
            int i = 0;
            while (i < tasks_f.size() && ((com.oddlabs.tt.player.ultra.BuildManager.Task)tasks_f.get(i)).priority >= armory.priority)
            {
                i++;
            }
            tasks_f.add(i, armory);
            float th = threat;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("SAFE opening: enemy threat %.1f, armory promoted", th);
            }));
        }
        // Java: 530
        public void endOpening()
        {
            opening = false;
            if (scout != null && ((object)ctx.roleOf(scout) == (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER) && ((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(scout)) == null)
            {
                ctx.clearRole(scout);
            }
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "opening done";
            }));
        }
        // Java: 537
        public void updateState(com.oddlabs.tt.player.ultra.BuildManager.Task t, float dt)
        {
            com.oddlabs.tt.model.Building b = t.building;
            switch ((t.state).ordinal())
            {
                case 0 /*PLANNED*/:
                {
                    if (b != null && !b.isDead() && b.isPlaced())
                    {
                        setState(t, com.oddlabs.tt.player.ultra.BuildManager.State.PLACED);
                        break;
                    }
                    if (!t.manual && (!opening || t.type == com.oddlabs.tt.model.Race.BUILDING_TOWER))
                    {
                        placeWithBuilders(t);
                    }
                    break;
                }
                case 1 /*ORDERED*/:
                {
                    if (b == null)
                    {
                        t.state = com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED;
                    }
                    else if (b.isDead())
                    {
                        replan(t);
                    }
                    else if (b.isPlaced())
                    {
                        setState(t, com.oddlabs.tt.player.ultra.BuildManager.State.PLACED);
                    }
                    else if (placerOf(t) == null)
                    {
                        t.placer_missing += dt;
                        if (t.placer_missing > 1.0f)
                        {
                            retryPlacement(t);
                        }
                    }
                    else
                    {
                        t.placer_missing = 0.0f;
                    }
                    break;
                }
                case 2 /*PLACED*/:
                {
                    if (b == null || b.isDead())
                    {
                        replan(t);
                    }
                    else if (b.isComplete())
                    {
                        setState(t, com.oddlabs.tt.player.ultra.BuildManager.State.DONE);
                        ctx.sites.unreserve(t.x, t.y);
                        onComplete(t);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 579
        public void setState(com.oddlabs.tt.player.ultra.BuildManager.Task t, com.oddlabs.tt.player.ultra.BuildManager.State s)
        {
            t.state = s;
            t.state_time = ctx.now;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "task " + java.lang.JStr.valueOf(t);
            }));
        }
        // Java: 585
        public com.oddlabs.tt.model.Unit placerOf(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if ((com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.PlaceBuildingController pc) && ((object)pc.getBuilding() == (object)t.building))
                {
                    return u;
                }
            }
            return null;
        }
        // Java: 593
        public void retryPlacement(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            t.tries++;
            t.placer_missing = 0.0f;
            if (t.tries > 4)
            {
                replan(t);
                return;
            }
            com.oddlabs.tt.model.BuildingTemplate tpl = template(t.type);
            ctx.sites.unreserve(t.x, t.y);
            bool legal = ctx.sites.isLegal(tpl, t.x, t.y);
            ctx.sites.reserve(t.x, t.y, tpl.getPlacingSize());
            if (!legal)
            {
                replan(t);
                return;
            }
            if (!t.manual && ctx.resources.threatened(t.x, t.y, 14))
            {
                t.state = com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED;
                t.placer_missing = 0.0f;
                return;
            }
            com.oddlabs.tt.model.Unit placer = (t.manual ? nearestAssigned(t) : nearestBuilderOrFree(t));
            if (placer != null)
            {
                orderPlace(t, placer);
            }
            else
            {
                t.state = com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED;
                t.placer_missing = 0.0f;
            }
        }
        // Java: 624
        public com.oddlabs.tt.model.Unit nearestAssigned(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) != (object)t) || !ctx.orders.canOrder(u))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t.x, t.y);
                if (d < best_d)
                {
                    best_d = d;
                    best = u;
                }
            }
            return best;
        }
        // Java: 640
        public void replan(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            ctx.sites.unreserve(t.x, t.y);
            t.building = null;
            int[] site = ((System.Func<int[]>)(() =>
            {
                switch ((t.type))
                {
                    case 1:
                    {
                        int[] a = planArmorySite(t.x, t.y, 16);
                        if (a == null)
                        {
                            a = planArmorySite(t.x, t.y, 40);
                        }
                        if (a == null)
                        {
                            a = planArmorySite(ctx.map.home_x, ctx.map.home_y, hub_radius);
                        }
                        return a;
                    }
                    case 0:
                    {
                        return planQuartersSite(t.x, t.y, t.x, t.y, 16);
                    }
                    default:
                    {
                        return ctx.towers.replanSite(t);
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }))();
            if (site == null)
            {
                t.state = com.oddlabs.tt.player.ultra.BuildManager.State.FAILED;
                t.state_time = ctx.now;
                releaseBuildersOf(t);
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "task failed " + java.lang.JStr.valueOf(t);
                }));
                return;
            }
            t.x = site[0];
            t.y = site[1];
            t.tries = 0;
            ctx.sites.reserve(t.x, t.y, template(t.type).getPlacingSize());
            t.state = com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "replanned " + java.lang.JStr.valueOf(t);
            }));
        }
        // Java: 675
        public bool orderPlace(com.oddlabs.tt.player.ultra.BuildManager.Task t, com.oddlabs.tt.model.Unit placer)
        {
            com.oddlabs.tt.model.Building old = t.building;
            if (old == null || old.isDead() || !old.isPlaced())
            {
                java.util.List crew = new java.util.ArrayList();
                crew.add(placer);
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
                {
                    if (((object)u != (object)placer) && ((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) == (object)t) && !u.isDead())
                    {
                        crew.add(u);
                    }
                }
                bool orderable = false;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(crew))
                {
                    orderable |= ctx.orders.canOrder(u_2);
                }
                if (!orderable)
                {
                    return false;
                }
                com.oddlabs.tt.model.Building b = ctx.orders.place(crew, t.type, t.x, t.y);
                if (b == null)
                {
                    if (++t.place_failures >= 8)
                    {
                        t.place_failures = 0;
                        replan(t);
                    }
                    return false;
                }
                t.place_failures = 0;
                t.building = b;
            }
            else
            {
                ctx.orders.build(placer, old);
            }
            ctx.setRole(placer, com.oddlabs.tt.player.ultra.Context.Role.BUILDER);
            assignment.put(placer, t);
            t.state = com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED;
            t.placer_missing = 0.0f;
            t.state_time = ctx.now;
            return true;
        }
        // Java: 714
        public bool placeWith(com.oddlabs.tt.player.ultra.BuildManager.Task t, com.oddlabs.tt.model.Unit placer)
        {
            com.oddlabs.tt.model.Building b = t.building;
            bool alive = b != null && !b.isDead();
            if (alive && (b.isPlaced() || placerOf(t) != null))
            {
                assign(placer, t);
                return true;
            }
            return orderPlace(t, placer);
        }
        // Java: 725
        public void placeWithBuilders(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            if (!ctx.owner.canBuild(t.type) || ctx.resources.threatened(t.x, t.y, 14))
            {
                return;
            }
            com.oddlabs.tt.model.Unit placer = nearestBuilderOrFree(t);
            if (placer != null)
            {
                orderPlace(t, placer);
            }
        }
        // Java: 733
        public com.oddlabs.tt.model.Unit nearestBuilderOrFree(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                bool mine = ((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) == (object)t);
                if ((!mine && !ctx.economy.isFree(u)) || !ctx.orders.canOrder(u))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u.getGridX(), u.getGridY(), t.x, t.y) - ((mine ? 400 : 0));
                if (d < best_d)
                {
                    best_d = d;
                    best = u;
                }
            }
            return best;
        }
        // Java: 749
        public void scoutTick()
        {
            if (scout == null || scout.isDead() || ((object)ctx.roleOf(scout) != (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER))
            {
                scout = null;
                com.oddlabs.tt.player.ultra.BuildManager.Task next = null;
                foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
                {
                    if (t.open() && ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED))
                    {
                        next = t;
                        break;
                    }
                }
                if (next == null)
                {
                    return;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
                {
                    if (u.isDead() || !ctx.orders.canOrder(u) || !(ctx.economy.isFree(u) || ((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER)))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u.getGridX(), u.getGridY(), next.x, next.y);
                    if (d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
                if (best == null)
                {
                    return;
                }
                scout = best;
            }
            int laid = 0;
            com.oddlabs.tt.player.ultra.BuildManager.Task next_2 = null;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t_2 in java.lang.JIter.of(tasks_f))
            {
                if (!t_2.open())
                {
                    continue;
                }
                if (((object)t_2.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED) || (((object)t_2.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED) && t_2.building != null && t_2.building.getHitPoints() < 5))
                {
                    laid++;
                }
                if (((object)t_2.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED) && next_2 == null && !ctx.resources.threatened(t_2.x, t_2.y, 30))
                {
                    next_2 = t_2;
                }
            }
            com.oddlabs.tt.player.ultra.BuildManager.Task scout_task = ((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(scout));
            bool scout_busy = scout_task != null && ((object)scout_task.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED);
            if (!scout_busy && next_2 != null && laid < foundation_lead)
            {
                orderPlace(next_2, scout);
            }
        }
        // Java: 799
        public int builderCount(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) == (object)t))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 809
        public int wantedBuilders(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            if (t.desired < 0)
            {
                return t.desired;
            }
            if (dropEligible(t))
            {
                if (((object)t.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED))
                {
                    return 1;
                }
                return (dropFeasible(t) || t.drop_pending > 0 ? 1 : java.lang.Math.min(t.desired, (t.full_crew ? t.desired : drop_wait_builders)));
            }
            return t.desired;
        }
        // Java: 824
        public bool dropFeasible(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building a = ctx.economy.armory();
            if (a == null || a.isDead())
            {
                return false;
            }
            int need = woodNeed(t);
            int stock = a.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies() - com.oddlabs.tt.player.ultra.BuildManager.WOOD_KEEP;
            int inside = a.getUnitContainer().getNumSupplies() - 2;
            int n = java.lang.Math.min(need, java.lang.Math.min(stock, inside));
            return n > 0 && (n >= need || n >= 8);
        }
        // Java: 836
        public com.oddlabs.tt.player.ultra.BuildManager.Task neediest()
        {
            com.oddlabs.tt.player.ultra.BuildManager.Task fallback = null;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (!t.open() || t.building == null)
                {
                    continue;
                }
                if (ctx.resources.threatened(t.x, t.y, 14))
                {
                    continue;
                }
                if (t.desired < 0)
                {
                    int cap = java.lang.Math.min(com.oddlabs.tt.player.ultra.BuildManager.OPENING_BUILDER_CAP, woodNeed(t) + 6);
                    if (builderCount(t) >= cap)
                    {
                        continue;
                    }
                    if (((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED) && woodNeed(t) > 0)
                    {
                        return t;
                    }
                    if (fallback == null)
                    {
                        fallback = t;
                    }
                    continue;
                }
                if (((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED) && woodNeed(t) <= 0)
                {
                    continue;
                }
                if (unplaced_crew > 0 && ((object)t.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED) && builderCount(t) >= unplaced_crew)
                {
                    continue;
                }
                if (builderCount(t) < wantedBuilders(t))
                {
                    return t;
                }
            }
            return fallback;
        }
        // Java: 868
        public void assign(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            ctx.setRole(u, com.oddlabs.tt.player.ultra.Context.Role.BUILDER);
            assignment.put(u, t);
            issueBuild(u, t);
        }
        // Java: 874
        public void issueBuild(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building b = t.building;
            if (b == null || b.isDead())
            {
                return;
            }
            int d = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), t.x, t.y);
            if (d > com.oddlabs.tt.player.ultra.BuildManager.LONG_WALK && !com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u) && b.isPlaced())
            {
                int[] step = com.oddlabs.tt.player.ultra.BuildManager.towards(u.getGridX(), u.getGridY(), t.x, t.y, 6);
                ctx.orders.move(u, step[0], step[1]);
            }
            else
            {
                ctx.orders.build(u, b);
            }
        }
        // Java: 888
        public static bool carriesWood(com.oddlabs.tt.model.Unit u)
        {
            return u.getSupplyContainer() != null && u.getSupplyContainer().getNumSupplies() > 0 && ((object)u.getSupplyContainer().getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply));
        }
        // Java: 893
        public static bool carriesAnything(com.oddlabs.tt.model.Unit u)
        {
            return u.getSupplyContainer() != null && u.getSupplyContainer().getNumSupplies() > 0;
        }
        // Java: 898
        public int woodNeed(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building b = t.building;
            if (b == null || b.isDead())
            {
                return 0;
            }
            int hp = (b.isPlaced() ? b.getHitPoints() : 0);
            int need = (t.maxHp(ctx) - hp + 4) / 5;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) != (object)t))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u) || (u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.RepairBehaviour))
                {
                    need--;
                }
            }
            return need;
        }
        public java.util.Map builder_probe;
        public float last_builder_probe;
        // Java: 921
        public void unstickBuilders()
        {
            if ((float)(ctx.now - last_builder_probe) < 5.0f)
            {
                return;
            }
            last_builder_probe = ctx.now;
            builder_probe.keySet().removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || ((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER);
            }));
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)ctx.roleOf(u_2) != (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER) || ((object)u_2 == (object)scout))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.BuildManager.Task t = ((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u_2));
                if (t == null || ((object)t.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED))
                {
                    continue;
                }
                int carry = u_2.getSupplyContainer().getNumSupplies();
                float[] p = ((float[])builder_probe.get(u_2));
                if (p == null || p[0] != carry || (u_2.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.RepairBehaviour))
                {
                    builder_probe.put(u_2, new float[] { carry, ctx.now });
                    continue;
                }
                if ((float)(ctx.now - p[1]) < com.oddlabs.tt.player.ultra.EconomyManager.STUCK_TIME)
                {
                    continue;
                }
                builder_probe.remove(u_2);
                ctx.resources.markBad(u_2.getGridX(), u_2.getGridY());
                com.oddlabs.tt.landscape.TreeSupply tree = ctx.resources.bestTree(t.x, t.y, 3, 24, com.oddlabs.tt.player.ultra.ResourceTracker.TREE_CAP);
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "stuck builder at " + (u_2.getGridX()) + "," + (u_2.getGridY()) + " for " + java.lang.JStr.valueOf(t);
                }));
                if (tree != null)
                {
                    ctx.orders.gather(u_2, tree);
                    ctx.resources.noteAssigned(tree);
                }
                else
                {
                    release(u_2);
                    ctx.orders.stop(u_2);
                }
            }
        }
        // Java: 954
        public void builderTick()
        {
            unstickBuilders();
            java.util.List peons = ctx.model.me.peons;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(peons))
            {
                if (((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.BuildManager.Task t = ((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u));
                if (t == null || !t.open() || t.building == null || t.building.isDead())
                {
                    if (t != null && ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED) && ((object)u == (object)scout))
                    {
                        continue;
                    }
                    release(u);
                    continue;
                }
                if (((object)u == (object)scout) && ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.ORDERED))
                {
                    continue;
                }
                com.oddlabs.tt.model.Building target = com.oddlabs.tt.player.ultra.WorldModel.buildTarget(u);
                if (((object)target == (object)t.building))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.WorldModel.Activity a = com.oddlabs.tt.player.ultra.WorldModel.activityOf(u);
                if (((object)a == (object)com.oddlabs.tt.player.ultra.WorldModel.Activity.WALK) && ctx.orders.recently(u, 8.0f))
                {
                    if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(u.getGridX(), u.getGridY(), t.x, t.y) > 8)
                    {
                        continue;
                    }
                }
                if (ctx.orders.recently(u, com.oddlabs.tt.player.ultra.Orders.Kind.BUILD, t.building, t.x, t.y, 1.5f))
                {
                    continue;
                }
                if (!t.building.isPlaced() && ctx.orders.recently(u, 4.0f))
                {
                    continue;
                }
                com.oddlabs.tt.player.ultra.Orders.Last last = ctx.orders.lastOrder(u);
                if (last != null && ((object)last.kind() == (object)com.oddlabs.tt.player.ultra.Orders.Kind.GATHER) && (float)(ctx.now - last.time()) < 60.0f && !com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u) && (com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController))
                {
                    continue;
                }
                issueBuild(u, t);
            }
            handoff();
        }
        // Java: 995
        public void handoff()
        {
            com.oddlabs.tt.player.ultra.BuildManager.Task current = null;
            com.oddlabs.tt.player.ultra.BuildManager.Task next = null;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(tasks_f))
            {
                if (!t.open() || t.building == null || t.manual)
                {
                    continue;
                }
                if (current == null && ((object)t.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED))
                {
                    current = t;
                }
                else if (current != null && next == null)
                {
                    next = t;
                }
            }
            if (current == null)
            {
                return;
            }
            if (next == null)
            {
                foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t_2 in java.lang.JIter.of(tasks_f))
                {
                    if (((object)t_2 != (object)current) && t_2.open() && ((object)t_2.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLANNED))
                    {
                        next = t_2;
                        break;
                    }
                }
            }
            int need = woodNeed(current);
            if (need > 0)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) != (object)current) || com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u) || (u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.RepairBehaviour) || ((object)u == (object)scout))
                {
                    continue;
                }
                if (next != null && next.building != null && next.desired < 0)
                {
                    assignment.put(u, next);
                    issueBuild(u, next);
                }
                else if (current.desired >= 0 || next == null)
                {
                    release(u);
                    com.oddlabs.tt.model.Building a = ctx.economy.armory();
                    if (a != null)
                    {
                        ctx.orders.enter(u, a);
                    }
                    else
                    {
                        ctx.orders.stop(u);
                    }
                }
            }
        }
        // Java: 1038
        public void onComplete(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            com.oddlabs.tt.model.Building b = t.building;
            java.util.List builders = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) == (object)t))
                {
                    builders.add(u);
                }
            }
            int keep = 0;
            if (b != null && t.type == com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
            {
                keep = ctx.economy.nKeep();
            }
            builders.sort(new java.util.Comparator_L((object a_l, object c_l) =>
            {
                com.oddlabs.tt.model.Unit a = (com.oddlabs.tt.model.Unit)a_l;
                com.oddlabs.tt.model.Unit c = (com.oddlabs.tt.model.Unit)c_l;
                int da = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(a.getGridX(), a.getGridY(), t.x, t.y);
                int dc = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(c.getGridX(), c.getGridY(), t.x, t.y);
                return java.lang.Integer.compare(da, dc);
            }));
            com.oddlabs.tt.player.ultra.BuildManager.Task next = null;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task o in java.lang.JIter.of(tasks_f))
            {
                if (((object)o != (object)t) && o.open() && o.desired < 0)
                {
                    next = o;
                    break;
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(builders))
            {
                if (keep > 0 && !com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u_2) && b != null)
                {
                    release(u_2);
                    ctx.orders.enter(u_2, b);
                    keep--;
                }
                else if (next != null && next.building != null)
                {
                    assignment.put(u_2, next);
                    issueBuild(u_2, next);
                }
                else
                {
                    release(u_2);
                }
            }
        }
        // Java: 1075
        public void release(com.oddlabs.tt.model.Unit u)
        {
            assignment.remove(u);
            if (((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER))
            {
                ctx.clearRole(u);
            }
            if (((object)u == (object)scout) && !opening)
            {
                scout = null;
            }
        }
        // Java: 1083
        public void releaseBuildersOf(com.oddlabs.tt.player.ultra.BuildManager.Task t)
        {
            java.util.List list = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)((com.oddlabs.tt.player.ultra.BuildManager.Task)assignment.get(u)) == (object)t))
                {
                    list.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(list))
            {
                release(u_2);
            }
        }
        // Java: 1093
        public void prune()
        {
            assignment.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
        }
        // Java: 1100
        public com.oddlabs.tt.model.BuildingTemplate template(int type)
        {
            return ctx.race.getBuildingTemplate(type);
        }
        // Java: 1106
        public float armoryCost(int x, int y)
        {
            int home = ctx.map.homeDist(x, y);
            if (home == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            float cw = (float)(10.2f + (float)(0.45f * ctx.resources.treeWalk(x, y, 20, 5)));
            float ci = nodeCost(ctx.resources.iron(), x, y, armory_iron_units, null);
            float cr = nodeCost(ctx.resources.rock(), x, y, 20, null);
            float danger = (float)(400.0f * java.lang.Math.max(0.0f, (float)(ctx.map.frontness(x, y) - j_danger_r)));
            float build = (float)((float)(2.0f * ((float)(15.3f + (float)(0.45f * ctx.resources.treeWalk(x, y, 8, 5))))) / 60.0f);
            return (float)((float)((float)((float)((float)((float)(80.0f + (float)(2.0f * cw)) + ci) + (float)(0.1f * cr)) + (float)(j_walk * home)) + danger) + build);
        }
        // Java: 1122
        public float nodeCost(java.util.List nodes, int x, int y, int units, int[] field)
        {
            int k = java.lang.Math.max(10, units / 5);
            int[] d = new int[k];
            int[] amount = new int[k];
            int n = 0;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(nodes))
            {
                if (s.isDead())
                {
                    continue;
                }
                int dist = (field == null ? com.oddlabs.tt.player.ultra.ResourceTracker.dropDistance(x, y, s.getGridX(), s.getGridY()) : ctx.resources.walkFrom(field, com.oddlabs.tt.player.ultra.BuildManager.PROBE_MAX, x, y, s.getGridX(), s.getGridY()));
                if (n == k && dist >= d[k - 1])
                {
                    continue;
                }
                int pos = (n < k ? n : k - 1);
                while (pos > 0 && d[pos - 1] > dist)
                {
                    if (pos < k)
                    {
                        d[pos] = d[pos - 1];
                        amount[pos] = amount[pos - 1];
                    }
                    pos--;
                }
                d[pos] = dist;
                amount[pos] = com.oddlabs.tt.player.ultra.ResourceTracker.remaining(s);
                if (n < k)
                {
                    n++;
                }
            }
            float sum = 0.0f;
            int got = 0;
            for (int i = 0; i < n && got < units; i++)
            {
                int take = java.lang.Math.min(amount[i], units - got);
                sum = (float)(sum + (float)(take * ((float)(10.2f + (float)(0.45f * d[i])))));
                got += take;
            }
            if (got < units)
            {
                sum = (float)(sum + (float)((units - got) * ((float)(10.2f + (float)(0.45f * 300.0f)))));
            }
            return (float)(sum / units);
        }
        // Java: 1159
        public int[] planArmorySite(int cx, int cy, int radius)
        {
            ctx.map.refreshTrees(ctx.now, 10.0f);
            com.oddlabs.tt.player.ultra.SiteFinder.Scorer scorer = new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x, int y) =>
            {
                return (float)(-armoryCost(x, y));
            });
            com.oddlabs.tt.player.ultra.SiteFinder.Scorer refine = (ctx.strategy.b("pathAware") ? new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x_2, int y_2) =>
            {
                return (float)(-armoryCostPath(x_2, y_2));
            }) : scorer);
            int[] site = (radius > 30 ? ctx.sites.bestCoarse(template(com.oddlabs.tt.model.Race.BUILDING_ARMORY), cx, cy, radius, 3, scorer, refine) : ctx.sites.bestRefined(template(com.oddlabs.tt.model.Race.BUILDING_ARMORY), cx, cy, radius, scorer, refine, 8));
            if (site != null && com.oddlabs.tt.player.ultra.UltraLog.enabled())
            {
                int[] s = site;
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "armory site " + (s[0]) + "," + (s[1]) + " " + java.lang.JStr.valueOf(explainArmory(s[0], s[1])) + " | home " + java.lang.JStr.valueOf(explainArmory(ctx.map.home_x, ctx.map.home_y));
                }));
            }
            return site;
        }
        // Java: 1175
        public string explainArmory(int x, int y)
        {
            float cw = (float)(10.2f + (float)(0.45f * ctx.resources.treeWalk(x, y, 20, 5)));
            float ci = nodeCost(ctx.resources.iron(), x, y, armory_iron_units, null);
            float cr = nodeCost(ctx.resources.rock(), x, y, 20, null);
            return java.lang.JString.format("J=%.0f Jpath=%.0f wood=%.0f iron=%.0f rock=%.0f walk=%d R=%.2f", armoryCost(x, y), armoryCostPath(x, y), cw, ci, cr, ctx.map.homeDist(x, y), ctx.map.frontness(x, y));
        }
        public const int PROBE_MAX = 240;
        public int[] probe_field;
        // Java: 1188
        public float armoryIronCost(int x, int y)
        {
            if (ctx.map.homeDist(x, y) == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            int[] f = ctx.map.distanceField(x, y, com.oddlabs.tt.player.ultra.BuildManager.PROBE_MAX, probe_field);
            probe_field = f;
            return nodeCost(ctx.resources.iron(), x, y, armory_iron_units, f);
        }
        // Java: 1196
        public float armoryCostPath(int x, int y)
        {
            int home = ctx.map.homeDist(x, y);
            if (home == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            int[] f = ctx.map.distanceField(x, y, com.oddlabs.tt.player.ultra.BuildManager.PROBE_MAX, probe_field);
            probe_field = f;
            float cw = (float)(10.2f + (float)(0.45f * ctx.resources.treeWalkPath(f, x, y, 20, 40)));
            float ci = nodeCost(ctx.resources.iron(), x, y, armory_iron_units, f);
            float cr = nodeCost(ctx.resources.rock(), x, y, 20, f);
            float danger = (float)(400.0f * java.lang.Math.max(0.0f, (float)(ctx.map.frontness(x, y) - j_danger_r)));
            float build = (float)((float)(2.0f * ((float)(15.3f + (float)(0.45f * ctx.resources.treeWalkPath(f, x, y, 8, 40))))) / 60.0f);
            return (float)((float)((float)((float)((float)((float)(80.0f + (float)(2.0f * cw)) + ci) + (float)(0.1f * cr)) + (float)(j_walk * home)) + danger) + build);
        }
        // Java: 1214
        public float quartersCost(int x, int y, int fx, int fy)
        {
            return quartersCost(x, y, fx, fy, ctx.resources.treeWalk(x, y, 12, 5));
        }
        public const int QUARTERS_PROBE_MAX = 90;
        // Java: 1221
        public float quartersCostPath(int x, int y, int fx, int fy)
        {
            if (!ctx.strategy.b("pathAware"))
            {
                return quartersCost(x, y, fx, fy);
            }
            int[] f = ctx.map.distanceField(x, y, com.oddlabs.tt.player.ultra.BuildManager.QUARTERS_PROBE_MAX, probe_field);
            probe_field = f;
            return quartersCost(x, y, fx, fy, ctx.resources.treeWalkPath(f, x, y, 12, 20));
        }
        // Java: 1229
        public float quartersCost(int x, int y, int fx, int fy, float tree_walk)
        {
            if (ctx.map.homeDist(x, y) == com.oddlabs.tt.player.ultra.MapAnalysis.UNREACHABLE)
            {
                return java.lang.Float.POSITIVE_INFINITY;
            }
            float walk = (float)(com.oddlabs.tt.player.ultra.MapAnalysis.octile(x - fx, y - fy) / 5.0f);
            float build = (float)((float)(1.5f * ((float)(40.0f / 18.0f))) * ((float)(15.3f + (float)(0.45f * tree_walk))));
            int hx = (hub_x >= 0 ? hub_x : ctx.map.home_x);
            int hy = (hub_y >= 0 ? hub_y : ctx.map.home_y);
            int to_hub = com.oddlabs.tt.player.ultra.MapAnalysis.octile(x - hx, y - hy);
            int prev_to_hub = com.oddlabs.tt.player.ultra.MapAnalysis.octile(fx - hx, fy - hy);
            float hub = (float)((float)((float)(2.0f * to_hub) / 5.0f) + (float)(java.lang.Math.max(0, to_hub - prev_to_hub) / 5.0f));
            float danger = (float)(300.0f * java.lang.Math.max(0.0f, (float)(ctx.map.frontness(x, y) - 0.35f)));
            int nodes = 0;
            foreach (com.oddlabs.tt.model.IronSupply s in java.lang.JIter.of(ctx.resources.iron()))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(s.getGridX(), s.getGridY(), x, y) <= 6)
                {
                    nodes++;
                }
            }
            foreach (com.oddlabs.tt.model.RockSupply s_2 in java.lang.JIter.of(ctx.resources.rock()))
            {
                if (com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(s_2.getGridX(), s_2.getGridY(), x, y) <= 6)
                {
                    nodes++;
                }
            }
            int hub_gap = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, hx, hy);
            float crowd = (hub_gap < 11 ? 40.0f : 0.0f);
            return (float)((float)((float)((float)((float)(walk + build) + hub) + danger) + (float)(15.0f * nodes)) + crowd);
        }
        // Java: 1256
        public int[] planQuartersSite(int cx, int cy, int fx, int fy, int radius)
        {
            ctx.map.refreshTrees(ctx.now, 10.0f);
            return ctx.sites.bestRefined(template(com.oddlabs.tt.model.Race.BUILDING_QUARTERS), cx, cy, radius, new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x, int y) =>
            {
                return (float)(-quartersCost(x, y, fx, fy));
            }), new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x_2, int y_2) =>
            {
                return (float)(-quartersCostPath(x_2, y_2, fx, fy));
            }), 8);
        }
        // Java: 1263
        public int[] planBoomQuartersSite(com.oddlabs.tt.model.Building armory)
        {
            ctx.map.refreshTrees(ctx.now, 10.0f);
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            float front = ctx.map.frontness(ax, ay);
            return ctx.sites.bestRefined(template(com.oddlabs.tt.model.Race.BUILDING_QUARTERS), ax, ay, 34, new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x, int y) =>
            {
                int gap = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(x, y, ax, ay);
                if (gap < 10)
                {
                    return java.lang.Float.NEGATIVE_INFINITY;
                }
                float c = quartersCost(x, y, ax, ay);
                c = (float)(c + (float)(200.0f * java.lang.Math.max(0.0f, (float)(ctx.map.frontness(x, y) - front))));
                return (float)(-c);
            }), new com.oddlabs.tt.player.ultra.SiteFinder.Scorer_L((int x_2, int y_2) =>
            {
                return (float)((float)(-quartersCostPath(x_2, y_2, ax, ay)) - (float)(200.0f * java.lang.Math.max(0.0f, (float)(ctx.map.frontness(x_2, y_2) - front))));
            }), 8);
        }
        // Java: 1278
        public static int[] towards(int x, int y, int tx, int ty, int stop_short)
        {
            int dx = tx - x;
            int dy = ty - y;
            float len = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
            if (len <= stop_short)
            {
                return new int[] { x, y };
            }
            float f = (float)(((float)(len - stop_short)) / len);
            return new int[] { x + java.lang.Math.round((float)(dx * f)), y + java.lang.Math.round((float)(dy * f)) };
        }
        // Java: 1288
        public bool isLegalStatic(int type, int x, int y)
        {
            return com.oddlabs.tt.model.LandBuilding.isPlacingLegal(ctx.grid, template(type), x, y);
        }
        private void init_BuildManager()
        {
            this.tasks_f = new java.util.ArrayList();
            this.assignment = new java.util.LinkedHashMap();
            this.opening = true;
            this.hub_x = (-1);
            this.hub_y = (-1);
            this.builder_probe = new java.util.LinkedHashMap();
            this.last_builder_probe = java.lang.Float.NEGATIVE_INFINITY;
        }
        public BuildManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_BuildManager();
            this.ctx = ctx;
            this.foundation_lead = ctx.strategy.i("foundationLead");
            this.j_walk = ctx.strategy.f("jWalk");
            this.j_danger_r = ctx.strategy.f("jDangerR");
            this.hub_radius = ctx.strategy.i("hubRadius");
            this.q1_radius = ctx.strategy.i("q1Radius");
            this.wood_drop = ctx.strategy.b("woodDrop");
            this.drop_wait_builders = ctx.strategy.i("dropWaitBuilders");
            this.unplaced_crew = ctx.strategy.i("unplacedCrew");
            this.armory_iron_units = ctx.strategy.i("armoryIronUnits");
        }
    }
}

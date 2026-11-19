// Translated from EconomyManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ultra
{
    // Java: 28
    public class EconomyManager : java.lang.Object
    {
        // Java: 29
        public sealed class Phase : java.lang.Enum
        {
            public static Phase[] values() { return (Phase[])VALUES_.Clone(); }
            public static Phase valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Phase OPENING = new Phase("OPENING", 0);
            public static readonly Phase BOOM = new Phase("BOOM", 1);
            public static readonly Phase PRODUCE = new Phase("PRODUCE", 2);
            public static readonly Phase CAP = new Phase("CAP", 3);
            public static readonly Phase RECOVER = new Phase("RECOVER", 4);
            public static readonly Phase[] VALUES_ = new Phase[] { OPENING, BOOM, PRODUCE, CAP, RECOVER };
            public Phase(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public static System.Type[] GATHER_TYPES;
        public static com.oddlabs.tt.model.DeployType[] HARVEST;
        public com.oddlabs.tt.player.ultra.Context ctx;
        public com.oddlabs.tt.model.Building armory_f;
        public com.oddlabs.tt.player.ultra.EconomyManager.Phase phase_f;
        public bool armory_ever;
        public com.oddlabs.tt.player.ultra.BuildManager.Task recover_task;
        public bool expansion;
        public bool surplus_out;
        public bool unfreeze;
        public bool deploy_gate;
        public int deploy_threat_r;
        public float expand_front;
        public bool surplus_rock;
        public int iron_min_nodes;
        public int rock_min_nodes;
        public com.oddlabs.tt.player.ultra.BuildManager.Task expand_task;
        public float last_expand_check;
        public java.util.List draining;
        public float armory_missing_since;
        public java.util.Map gather_probe;
        public float last_probe;
        public const float STUCK_TIME = 120.0f;
        public com.oddlabs.tt.player.ultra.BuildManager.Task boom_task;
        public float last_recover;
        public float surplus_until;
        public const int SURPLUS_KEEP = 10;
        public int[] gather_target;
        public int[] gather_now;
        public int workers_target;
        public float last_chickens;
        public bool emergency;
        public bool rock_emergency;
        public bool rock_weapons;
        public bool stocks_ok;
        public int worker_deficit;
        public int quarters_max;
        public int n_keep_opening;
        public int n_keep_boom;
        public int n_keep_cap;
        public int cap_margin;
        public float builder_frac_boom;
        public float stock_wood;
        public float stock_iron;
        public float stock_rock;
        public float stock_tau;
        public int hunter_max;
        public int chicken_dist;
        public int chicken_dist_far;
        public float workers_keep_frac;
        public int builders_boom_site;
        public int wood_reserve;
        public float wood_reserve_time;
        public int iron_reach;
        public float rock_mix;
        public float rock_eff;
        public float expand_iron;
        public bool siege_evac;
        public int evac_keep;
        public bool rock_fallback;
        public bool drain_rally;
        public int last_s_wood;
        public int last_s_iron;
        public int last_s_rock;
        public bool qb_fixed;
        public int qb_prio;
        public bool iron_dry_rock;
        public float iron_dry_delay;
        public float iron_dry_since;
        public float iron_wet_since;
        public bool iron_dry_active;
        public float builder_cap;
        // Java: 175
        public com.oddlabs.tt.model.Building armory()
        {
            if (armory_f != null && (armory_f.isDead() || !armory_f.isComplete()))
            {
                armory_f = null;
            }
            return armory_f;
        }
        // Java: 182
        public com.oddlabs.tt.player.ultra.EconomyManager.Phase phase()
        {
            return phase_f;
        }
        // Java: 187
        public int nKeep()
        {
            if (ctx.owner.getUnitCountContainer().getNumSupplies() >= ctx.world.getMaxUnitCount() - 3)
            {
                return java.lang.Math.min(1, n_keep_cap);
            }
            return ((phase_f).ordinal() switch { 0 /*OPENING*/ or 4 /*RECOVER*/ => (int)(n_keep_opening), 1 /*BOOM*/ or 2 /*PRODUCE*/ => (int)(n_keep_boom), 3 /*CAP*/ => (int)(n_keep_cap), _ => throw new System.InvalidOperationException("MatchException") });
        }
        // Java: 199
        public int econPeons()
        {
            int n = (armory_f != null ? armory_f.getUnitContainer().getNumSupplies() : 0);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (!ctx.hasRole(u))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 209
        public bool isFree(com.oddlabs.tt.model.Unit u)
        {
            if (ctx.hasRole(u))
            {
                return false;
            }
            com.oddlabs.tt.player.ultra.WorldModel.Activity a = com.oddlabs.tt.player.ultra.WorldModel.activityOf(u);
            {
                switch ((a).ordinal())
                {
                    case 0 /*IDLE*/:
                    case 6 /*HUNT*/:
                    {
                        return true;
                    }
                    case 7 /*ENTER*/:
                    {
                        return (com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.TransferUnitController);
                    }
                    case 2 /*BUILD*/:
                    case 3 /*PLACE*/:
                    {
                        com.oddlabs.tt.model.Building b = com.oddlabs.tt.player.ultra.WorldModel.buildTarget(u);
                        return b == null || b.isDead() || (b.isComplete() && !b.isDamaged());
                    }
                    default:
                    {
                        return false;
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 226
        public void tick()
        {
            updateArmory();
            if (armory_f != null)
            {
                armory_missing_since = (float)(-1.0f);
            }
            else if (armory_missing_since < 0.0f)
            {
                armory_missing_since = ctx.now;
            }
            updatePhase();
            quartersFlow();
            if (armory_f != null)
            {
                gathererControl();
                weapons();
                deployWarriors();
                armoryDrain();
                if ((float)(ctx.now - last_chickens) >= 2.0f)
                {
                    last_chickens = ctx.now;
                    chickens();
                }
            }
            cleanupHunters();
            expandTick();
            drainOld();
            unstickGatherers();
            planQuarters();
            dispatch();
        }
        // Java: 256
        public void unstickGatherers()
        {
            if ((float)(ctx.now - last_probe) < 5.0f)
            {
                return;
            }
            last_probe = ctx.now;
            gather_probe.keySet().removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || ctx.hasRole(u) || !((com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController));
            }));
            com.oddlabs.tt.model.Building a = armory_f;
            int respread = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (ctx.hasRole(u_2) || !((com.oddlabs.tt.player.ultra.WorldModel.primary(u_2) is com.oddlabs.tt.model.behaviour.GatherController gc)))
                {
                    continue;
                }
                int carry = u_2.getSupplyContainer().getNumSupplies();
                com.oddlabs.tt.model.Supply cur = gc.getSupply();
                com.oddlabs.tt.model.Building drop = gc.getAssignedBuilding();
                bool old_drop = drop != null && draining.contains(drop);
                if (a != null && carry == 0 && respread < 6 && (cur == null || cur.isDead() || cur.isEmpty() || old_drop) && !ctx.orders.recently(u_2, 4.0f))
                {
                    int type = typeIndex(gc.getSupplyType());
                    if (type >= 0 && sendGather(u_2, type, false))
                    {
                        respread++;
                        gather_probe.put(u_2, new float[] { carry, ctx.now });
                        continue;
                    }
                }
                float[] p = ((float[])gather_probe.get(u_2));
                if (p == null || p[0] != carry)
                {
                    gather_probe.put(u_2, new float[] { carry, ctx.now });
                    continue;
                }
                if ((float)(ctx.now - p[1]) < com.oddlabs.tt.player.ultra.EconomyManager.STUCK_TIME)
                {
                    continue;
                }
                gather_probe.remove(u_2);
                ctx.resources.markBad(u_2.getGridX(), u_2.getGridY());
                com.oddlabs.tt.model.Supply s = gc.getSupply();
                if (s != null && !s.isDead())
                {
                    ctx.resources.markBad(s.getGridX(), s.getGridY());
                }
                int type_2 = typeIndex(gc.getSupplyType());
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "stuck gatherer at " + (u_2.getGridX()) + "," + (u_2.getGridY());
                }));
                if (a == null)
                {
                    ctx.orders.stop(u_2);
                }
                else if (carry > 0 || type_2 < 0 || !sendGather(u_2, type_2, false))
                {
                    ctx.orders.enter(u_2, a);
                }
            }
        }
        // Java: 310
        public void cleanupHunters()
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (u.isDead() || ((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.HUNTER))
                {
                    continue;
                }
                com.oddlabs.tt.model.Supply target = null;
                bool gathering = (com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc) && ((object)gc.getSupplyType() == (object)typeof(com.oddlabs.tt.model.RubberSupply));
                if (gathering)
                {
                    target = (((com.oddlabs.tt.model.behaviour.GatherController)com.oddlabs.tt.player.ultra.WorldModel.primary(u))).getSupply();
                }
                bool carrying = com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u);
                if (gathering && (carrying || (target != null && !target.isDead())))
                {
                    continue;
                }
                ctx.clearRole(u);
                if (armory() != null && !carrying)
                {
                    ctx.orders.enter(u, armory_f);
                }
                else if (armory() == null)
                {
                    ctx.orders.stop(u);
                }
            }
        }
        // Java: 330
        public void updateArmory()
        {
            if (armory_f != null && (armory_f.isDead() || !armory_f.isComplete()))
            {
                armory_f = null;
            }
            com.oddlabs.tt.player.ultra.BuildManager.Task et = expand_task;
            if (et != null && ((object)et.state == (object)com.oddlabs.tt.player.ultra.BuildManager.State.DONE))
            {
                com.oddlabs.tt.model.Building nb = et.building;
                expand_task = null;
                if (nb != null && !nb.isDead() && nb.isComplete() && ((object)nb != (object)armory_f))
                {
                    com.oddlabs.tt.model.Building old = armory_f;
                    if (old != null)
                    {
                        draining.add(old);
                    }
                    armory_f = nb;
                    armory_ever = true;
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "armory switched to " + (nb.getGridX()) + "," + (nb.getGridY());
                    }));
                }
            }
            if (armory_f == null)
            {
                com.oddlabs.tt.model.Building found = null;
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ctx.model.me.armories))
                {
                    if (!b.isDead() && b.isComplete())
                    {
                        found = b;
                        break;
                    }
                }
                if (found == null)
                {
                    found = ctx.build.completedBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                }
                if (found != null)
                {
                    com.oddlabs.tt.model.Building b_2 = found;
                    armory_f = b_2;
                    armory_ever = true;
                    ctx.log(new java.util.function.Supplier_L(() =>
                    {
                        return "armory ready at " + (b_2.getGridX()) + "," + (b_2.getGridY());
                    }));
                }
            }
        }
        // Java: 367
        public void updatePhase()
        {
            com.oddlabs.tt.player.ultra.EconomyManager.Phase old = phase_f;
            if (armory_f == null)
            {
                phase_f = (armory_ever ? com.oddlabs.tt.player.ultra.EconomyManager.Phase.RECOVER : com.oddlabs.tt.player.ultra.EconomyManager.Phase.OPENING);
            }
            else
            {
                int pop = ctx.owner.getUnitCountContainer().getNumSupplies();
                int cap = ctx.world.getMaxUnitCount();
                if (pop >= cap - cap_margin)
                {
                    phase_f = com.oddlabs.tt.player.ultra.EconomyManager.Phase.CAP;
                }
                else if (ctx.model.me.quarters.size() + ctx.build.openCount(com.oddlabs.tt.model.Race.BUILDING_QUARTERS) < quarters_max)
                {
                    phase_f = com.oddlabs.tt.player.ultra.EconomyManager.Phase.BOOM;
                }
                else
                {
                    phase_f = com.oddlabs.tt.player.ultra.EconomyManager.Phase.PRODUCE;
                }
            }
            if (((object)phase_f != (object)old))
            {
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return "econ phase " + java.lang.JStr.valueOf(old) + " -> " + java.lang.JStr.valueOf(phase_f);
                }));
            }
            if (armory_f == null && ctx.build.openCount(com.oddlabs.tt.model.Race.BUILDING_ARMORY) == 0 && !ctx.build.inOpeningPlan())
            {
                recover();
            }
            com.oddlabs.tt.player.ultra.BuildManager.Task rt = recover_task;
            if (armory_f != null && rt != null && rt.open() && ((object)rt.state != (object)com.oddlabs.tt.player.ultra.BuildManager.State.PLACED))
            {
                ctx.build.cancel(rt);
                recover_task = null;
            }
        }
        // Java: 397
        public void quartersFlow()
        {
            com.oddlabs.tt.model.Building chief_q = ctx.chief.trainingQuarters();
            int n_keep = nKeep();
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ctx.model.me.quarters))
            {
                if (q.isDead())
                {
                    continue;
                }
                int inside = q.getUnitContainer().getNumSupplies();
                int keep = (((object)q == (object)chief_q) ? ctx.chief.keepInside() : n_keep);
                if (armory_f == null && ctx.now < surplus_until)
                {
                    keep = java.lang.Math.max(keep, com.oddlabs.tt.player.ultra.EconomyManager.SURPLUS_KEEP);
                }
                if (ctx.military.quartersThreatened(q))
                {
                    keep = 0;
                }
                if (inside > keep)
                {
                    ctx.orders.deploy(q, com.oddlabs.tt.model.DeployType.PEON, inside - keep);
                }
            }
        }
        // Java: 414
        public void planQuarters()
        {
            if (boom_task != null && !boom_task.open())
            {
                boom_task = null;
            }
            if (boom_task != null && boom_task.priority > 500 && (emergency || ctx.towers.hot() || ctx.model.me.quarters.size() >= 4))
            {
                ctx.build.reprioritize(boom_task, 500);
            }
            if (armory_f == null || boom_task != null)
            {
                return;
            }
            if (((object)phase_f != (object)com.oddlabs.tt.player.ultra.EconomyManager.Phase.BOOM))
            {
                return;
            }
            if (ctx.build.openCount(com.oddlabs.tt.model.Race.BUILDING_QUARTERS) > 0)
            {
                return;
            }
            if (!ctx.owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_QUARTERS) || !ctx.towers.slotAvailableForQuarters())
            {
                return;
            }
            int[] site = ctx.build.planBoomQuartersSite(armory_f);
            if (site == null)
            {
                return;
            }
            int builders = (qb_fixed ? builders_boom_site : java.lang.Math.max(4, java.lang.Math.min(builders_boom_site, java.lang.Math.round((float)(builder_frac_boom * econPeons())))));
            int prio = (ctx.model.me.quarters.size() < 4 && !emergency ? qb_prio : 500);
            boom_task = ctx.build.request(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, site[0], site[1], prio, builders, "Qb");
            boom_task.full_crew = qb_fixed;
        }
        // Java: 443
        public void expandTick()
        {
            if (!expansion || armory_f == null || ctx.now < 360.0f || (float)(ctx.now - last_expand_check) < 30.0f)
            {
                return;
            }
            last_expand_check = ctx.now;
            com.oddlabs.tt.player.ultra.BuildManager.Task et = expand_task;
            if (et != null && et.open())
            {
                return;
            }
            if (ctx.model.me.armories.size() != 1 || ctx.build.openCount(com.oddlabs.tt.model.Race.BUILDING_ARMORY) > 0 || ctx.military.isDefending() || !ctx.owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY) || ctx.owner.getBuildingCountContainer().getNumSupplies() + ctx.build.pendingSlots() >= ctx.world.getMaxBuildingCount() - 1)
            {
                return;
            }
            com.oddlabs.tt.model.Building a = armory_f;
            float ti = ctx.resources.tripTime(typeof(com.oddlabs.tt.model.IronSupply), a, 12);
            float tw = ctx.resources.tripTime(typeof(com.oddlabs.tt.landscape.TreeSupply), a, 20);
            if (ti < 70.0f && tw < 45.0f)
            {
                return;
            }
            float cur = ctx.build.armoryCostPath(a.getGridX(), a.getGridY());
            int[] site = ctx.build.planArmorySite(a.getGridX(), a.getGridY(), 70);
            if (site == null)
            {
                last_expand_check = (float)(ctx.now + 60.0f);
                return;
            }
            float cost = ctx.build.armoryCostPath(site[0], site[1]);
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("expansion check: trips iron %.0f wood %.0f, cost here %.0f, best %.0f at %d,%d", ti, tw, cur, cost, site[0], site[1]);
            }));
            bool iron_case = false;
            if (expand_iron > 0.0f && ti >= 100.0f && cost <= cur)
            {
                float ci_here = ctx.build.armoryIronCost(a.getGridX(), a.getGridY());
                float ci_site = ctx.build.armoryIronCost(site[0], site[1]);
                iron_case = ci_site <= (float)(((float)(1.0f - expand_iron)) * ci_here);
                ctx.log(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("expansion iron check: iron cost here %.0f, there %.0f", ci_here, ci_site);
                }));
            }
            if ((cost > (float)(0.75f * cur) && !iron_case) || com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(site[0], site[1], a.getGridX(), a.getGridY()) < 20 || ctx.map.frontness(site[0], site[1]) > java.lang.Math.min(0.47f, (float)(ctx.map.frontness(a.getGridX(), a.getGridY()) + expand_front)))
            {
                last_expand_check = (float)(ctx.now + 60.0f);
                return;
            }
            expand_task = ctx.build.request(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site[0], site[1], 700, 14, "A2");
        }
        // Java: 490
        public void readMultiParams()
        {
            siege_evac = ctx.strategy.b("siegeEvac");
            rock_fallback = ctx.strategy.b("rockFallback");
        }
        // Java: 496
        public bool shelterBlocked(com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Building b)
        {
            return siege_evac && ((object)b == (object)armory_f) && ((object)com.oddlabs.tt.player.ultra.WorldModel.kindOf(u) == (object)com.oddlabs.tt.player.ultra.WorldModel.Kind.PEON) && ctx.military.armorySieged();
        }
        // Java: 504
        public void armoryDrain()
        {
            com.oddlabs.tt.model.Building a = armory_f;
            if (!siege_evac || a == null)
            {
                return;
            }
            bool sieged = ctx.military.armorySieged();
            if (!sieged)
            {
                if (drain_rally)
                {
                    drain_rally = false;
                    int[] rally = ctx.military.rallyPoint();
                    if (rally != null)
                    {
                        ctx.orders.rally(a, rally[0], rally[1]);
                    }
                    else
                    {
                        ctx.orders.clearRally(a);
                    }
                }
                return;
            }
            if ((float)(ctx.now - ctx.military.armorySiegedSince()) < 3.0f)
            {
                return;
            }
            int[] rear = ctx.military.rearPoint();
            if (rear == null)
            {
                return;
            }
            int queued = 0;
            foreach (com.oddlabs.tt.model.DeployType t in com.oddlabs.tt.model.DeployType.values())
            {
                com.oddlabs.tt.model.DeployContainer c = a.getDeployContainer(t);
                if (c != null)
                {
                    queued += c.getNumSupplies();
                }
            }
            if (queued > 0)
            {
                return;
            }
            int weapons_v = a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies() + a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() + a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            int inside = a.getUnitContainer().getNumSupplies();
            if (weapons_v > 0 && inside > 0)
            {
                return;
            }
            int n = inside - evac_keep;
            if (n <= 0)
            {
                return;
            }
            ctx.orders.rally(a, rear[0], rear[1]);
            drain_rally = true;
            ctx.orders.deploy(a, com.oddlabs.tt.model.DeployType.PEON, n);
            int k = n;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "armory drain: " + (k) + " peons out to " + (rear[0]) + "," + (rear[1]);
            }));
        }
        // Java: 548
        public void drainOld()
        {
            draining.removeIf(new java.util.function.Predicate_L((object b_l) =>
            {
                com.oddlabs.tt.model.Building b = (com.oddlabs.tt.model.Building)b_l;
                return b.isDead() || ((object)b == (object)armory_f);
            }));
            ctx.resources.setAvoidDrops((draining.isEmpty() ? java.util.List_.of() : java.util.List_.copyOf(draining)));
            int[] rally = ctx.military.rallyPoint();
            foreach (com.oddlabs.tt.model.Building old in java.lang.JIter.of(draining))
            {
                if (!old.isComplete())
                {
                    continue;
                }
                ctx.orders.weapons(old, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), false);
                ctx.orders.weapons(old, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), false);
                ctx.orders.weapons(old, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), false);
                if (rally != null)
                {
                    ctx.orders.rally(old, rally[0], rally[1]);
                }
                int inside = old.getUnitContainer().getNumSupplies();
                int queued = 0;
                foreach (com.oddlabs.tt.model.DeployType t in com.oddlabs.tt.model.DeployType.values())
                {
                    com.oddlabs.tt.model.DeployContainer c = old.getDeployContainer(t);
                    if (c != null)
                    {
                        queued += c.getNumSupplies();
                    }
                }
                if (inside <= 0 || queued > 0)
                {
                    continue;
                }
                int rub = old.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
                int iron = old.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
                int rock = old.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
                if (rub > 0)
                {
                    ctx.orders.deploy(old, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, java.lang.Math.min(rub, inside));
                }
                else if (iron > 0)
                {
                    ctx.orders.deploy(old, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, java.lang.Math.min(iron, inside));
                }
                else if (rock > 0)
                {
                    ctx.orders.deploy(old, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, java.lang.Math.min(rock, inside));
                }
                else
                {
                    ctx.orders.clearRally(old);
                    ctx.orders.deploy(old, com.oddlabs.tt.model.DeployType.PEON, inside);
                }
            }
        }
        // Java: 586
        public void recover()
        {
            if (armory_missing_since < 0.0f || (float)(ctx.now - armory_missing_since) < 2.0f)
            {
                return;
            }
            if ((recover_task != null && recover_task.open()) || (float)(ctx.now - last_recover) < 5.0f)
            {
                return;
            }
            last_recover = ctx.now;
            int[] site = null;
            com.oddlabs.tt.model.Building q = (ctx.model.me.quarters.isEmpty() ? (com.oddlabs.tt.model.Building)(null) : ((com.oddlabs.tt.model.Building)ctx.model.me.quarters.get(0)));
            int cx = (q != null ? q.getGridX() : ctx.map.home_x);
            int cy = (q != null ? q.getGridY() : ctx.map.home_y);
            site = ctx.build.planArmorySite(cx, cy, 40);
            if (site == null)
            {
                site = ctx.build.planArmorySite(ctx.map.home_x, ctx.map.home_y, 80);
            }
            if (site == null)
            {
                return;
            }
            recover_task = ctx.build.request(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site[0], site[1], 2000, (-1), "A-rebuild");
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return "no armory: (re)building";
            }));
        }
        // Java: 609
        public int typeIndex(System.Type type)
        {
            if (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
            {
                return 0;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
            {
                return 1;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
            {
                return 2;
            }
            return (-1);
        }
        // Java: 619
        public void gathererControl()
        {
            com.oddlabs.tt.model.Building a = armory_f;
            java.util.Arrays.fill(gather_now, 0);
            int free = 0;
            int hunters = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                com.oddlabs.tt.player.ultra.Context.Role role = ctx.roleOf(u);
                if (((object)role == (object)com.oddlabs.tt.player.ultra.Context.Role.HUNTER))
                {
                    hunters++;
                    continue;
                }
                if (role != null)
                {
                    continue;
                }
                if ((com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc))
                {
                    int i = typeIndex(gc.getSupplyType());
                    if (i >= 0)
                    {
                        gather_now[i]++;
                    }
                }
                else if (isFree(u))
                {
                    free++;
                }
            }
            int inside = a.getUnitContainer().getNumSupplies();
            int e = inside + free + gather_now[0] + gather_now[1] + gather_now[2];
            int rock_slots = ctx.resources.slotsWithin(typeof(com.oddlabs.tt.model.RockSupply), a, iron_reach, rock_min_nodes);
            int iron_slots = ctx.resources.slotsWithin(typeof(com.oddlabs.tt.model.IronSupply), a, iron_reach, iron_min_nodes);
            float tw = ctx.resources.tripTime(typeof(com.oddlabs.tt.landscape.TreeSupply), a, java.lang.Math.max(4, gather_now[0]));
            float ti = ctx.resources.tripTime(typeof(com.oddlabs.tt.model.IronSupply), a, java.lang.Math.max(2, java.lang.Math.min(gather_now[1], iron_slots)));
            float tr = ctx.resources.tripTime(typeof(com.oddlabs.tt.model.RockSupply), a, java.lang.Math.max(1, java.lang.Math.min(gather_now[2], rock_slots)));
            int s_wood = a.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies();
            int s_iron = a.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies();
            int s_rock = a.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies();
            int s_rub = a.getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies();
            float lam_i = (float)((float)(80.0f + (float)(2.0f * tw)) + ti);
            float lam_u = (float)((float)((float)(120.0f + (float)(2.0f * tw)) + ti) + tr);
            float lam_r = (float)((float)(40.0f + (float)(2.0f * tw)) + tr);
            float w_u = java.lang.Math.min((float)(e / lam_u), (float)(((float)(s_rub + (float)(0.5f * hunters))) / 40.0f));
            float w_i_labor = (float)(java.lang.Math.max(0.0f, (float)(e - (float)(w_u * lam_u))) / lam_i);
            float w_i = java.lang.Math.min(w_i_labor, (float)(((float)(iron_slots + (float)(s_iron / 20.0f))) / ti));
            if (rock_emergency)
            {
                w_i = 0.0f;
            }
            else if (rock_eff > 0.0f && (float)(com.oddlabs.tt.player.ultra.CombatModel.weight(com.oddlabs.tt.player.ultra.WorldModel.Kind.ROCK) / lam_r) > (float)(1.0f / lam_i) && (float)(rock_slots / tr) > w_u)
            {
                w_i = (float)(w_i * (float)(1.0f - rock_eff));
            }
            float labor_left = java.lang.Math.max(0.0f, (float)((float)(e - (float)(w_u * lam_u)) - (float)(w_i * lam_i)));
            float w_r = 0.0f;
            if (rock_mix > 0.0f && (w_i < (float)(0.8f * w_i_labor) || rock_emergency))
            {
                w_r = java.lang.Math.min((float)((float)(rock_mix * labor_left) / lam_r), java.lang.Math.max(0.0f, (float)((float)(rock_slots / tr) - w_u)));
            }
            if (iron_dry_rock)
            {
                bool dry = ((object)phase_f == (object)com.oddlabs.tt.player.ultra.EconomyManager.Phase.CAP) && s_iron <= 1;
                if (dry)
                {
                    iron_wet_since = (float)(-1.0f);
                    if (iron_dry_since < 0.0f)
                    {
                        iron_dry_since = ctx.now;
                    }
                    if ((float)(ctx.now - iron_dry_since) > iron_dry_delay)
                    {
                        iron_dry_active = true;
                    }
                }
                else
                {
                    if (iron_wet_since < 0.0f)
                    {
                        iron_wet_since = ctx.now;
                    }
                    if ((float)(ctx.now - iron_wet_since) > 5.0f)
                    {
                        iron_dry_since = (float)(-1.0f);
                    }
                    if ((float)(ctx.now - iron_wet_since) > 10.0f)
                    {
                        iron_dry_active = false;
                    }
                }
                if (iron_dry_active)
                {
                    float idle = (float)((float)(inside - ((float)((float)(80.0f * w_i) + (float)(120.0f * w_u)))) + java.lang.Math.max(0.0f, (float)(gather_now[2] - (float)(w_u * tr))));
                    w_r = java.lang.Math.max(w_r, java.lang.Math.min(java.lang.Math.max(0.0f, (float)((float)(rock_slots / tr) - w_u)), (float)(java.lang.Math.max(0.0f, idle) / lam_r)));
                }
            }
            rock_weapons = w_r > 0.01f;
            float wood_target = (float)((float)(stock_wood + ((ctx.now >= wood_reserve_time ? wood_reserve : 0))) + ctx.build.woodDemand());
            float g_w = (float)((float)((float)(2.0f * ((float)((float)(w_i + w_u) + w_r))) * tw) + refill((float)(wood_target - s_wood), tw));
            float g_i = (float)((float)(((float)(w_i + w_u)) * ti) + refill((float)(stock_iron - s_iron), ti));
            float g_r = (float)((float)(((float)(w_u + w_r)) * tr) + refill((float)(stock_rock - s_rock), tr));
            stocks_ok = s_wood >= 4 && (s_iron >= 2 || (s_rock >= 2 && rock_weapons));
            last_s_wood = s_wood;
            last_s_iron = s_iron;
            last_s_rock = s_rock;
            worker_deficit = workers_target - inside;
            gather_target[0] = com.oddlabs.tt.player.ultra.EconomyManager.clampTarget(g_w, s_wood);
            gather_target[1] = java.lang.Math.min(iron_slots, com.oddlabs.tt.player.ultra.EconomyManager.clampTarget(g_i, s_iron));
            gather_target[2] = java.lang.Math.min(rock_slots, com.oddlabs.tt.player.ultra.EconomyManager.clampTarget(g_r, s_rock));
            workers_target = java.lang.Math.round((float)((float)((float)(80.0f * w_i) + (float)(120.0f * w_u)) + (float)(40.0f * w_r)));
            surplus_rock = false;
            if (surplus_out && !emergency && inside - workers_target > 20)
            {
                int extra = inside - workers_target - 10;
                if (unfreeze)
                {
                    if (s_iron < 3 && gather_target[1] < iron_slots)
                    {
                        int add = java.lang.Math.min(extra, iron_slots - gather_target[1]);
                        gather_target[1] += add;
                        extra -= add;
                    }
                    int hyst = java.lang.Math.max(2, java.lang.Math.round((float)(0.15f * gather_target[0])));
                    if (extra > 0 && s_wood < 60 && gather_now[0] >= gather_target[0] - hyst)
                    {
                        gather_target[0] += extra;
                        extra = 0;
                    }
                    if (extra > 0 && s_iron < 3 && rock_slots > gather_target[2])
                    {
                        gather_target[2] = java.lang.Math.min(rock_slots, gather_target[2] + extra);
                        surplus_rock = true;
                    }
                }
                else if (s_wood < 60)
                {
                    gather_target[0] += extra;
                }
                else if (s_iron < 3 && rock_slots > gather_target[2])
                {
                    gather_target[2] = java.lang.Math.min(rock_slots, gather_target[2] + extra);
                    surplus_rock = true;
                }
            }
            int spare_inside = inside - workers_target;
            char gate = '-';
            bool raid_near = ctx.resources.threatened(a.getGridX(), a.getGridY(), (unfreeze ? deploy_threat_r : 30));
            for (int i_2 = 0; i_2 < 3; i_2++)
            {
                int diff = gather_target[i_2] - gather_now[i_2];
                int hysteresis = java.lang.Math.max(2, java.lang.Math.round((float)(0.15f * gather_target[i_2])));
                if (diff <= -hysteresis)
                {
                    int k = java.lang.Math.min(3, -diff);
                    ctx.orders.recallGatherers(a, com.oddlabs.tt.player.ultra.EconomyManager.GATHER_TYPES[i_2], k);
                }
                else if (diff >= hysteresis)
                {
                    bool free_ok = (unfreeze || deploy_gate ? free < java.lang.Math.max(1, diff / 3) : free == 0);
                    if (!free_ok)
                    {
                        gate = 'F';
                    }
                    else if (spare_inside <= 0)
                    {
                        gate = (gate == '-' ? 'W' : gate);
                    }
                    else if (emergency)
                    {
                        gate = 'E';
                    }
                    else if (raid_near)
                    {
                        gate = 'T';
                    }
                    else
                    {
                        int k_2 = java.lang.Math.min(java.lang.Math.min(3, diff), spare_inside);
                        com.oddlabs.tt.model.DeployContainer c = a.getDeployContainer(com.oddlabs.tt.player.ultra.EconomyManager.HARVEST[i_2]);
                        if (c != null && c.getNumSupplies() == 0)
                        {
                            ctx.orders.deploy(a, com.oddlabs.tt.player.ultra.EconomyManager.HARVEST[i_2], k_2);
                            spare_inside -= k_2;
                        }
                        else
                        {
                            gate = 'Q';
                        }
                    }
                }
            }
            int ew = e;
            int fr = free;
            char g = gate;
            float wr = w_r;
            ctx.log(new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("econ E=%d in=%d/%d free=%d G=%d/%d %d/%d %d/%d T=%.0f/%.0f/%.0f S=%d/%d/%d/%d gate=%c slots i/r=%d/%d w_r=%.3f", ew, inside, workers_target, fr, gather_now[0], gather_target[0], gather_now[1], gather_target[1], gather_now[2], gather_target[2], tw, ti, tr, s_wood, s_iron, s_rock, s_rub, g, iron_slots, rock_slots, wr);
            }));
        }
        // Java: 783
        public float refill(float missing, float trip)
        {
            return java.lang.Math.max((float)(-6.0f), java.lang.Math.min(6.0f, (float)((float)(missing * trip) / stock_tau)));
        }
        // Java: 787
        public static int clampTarget(float g, int stock)
        {
            if (stock >= 150)
            {
                return 0;
            }
            return java.lang.Math.max(0, java.lang.Math.round(g));
        }
        // Java: 794
        public bool sendOtherGather(com.oddlabs.tt.model.Unit u, int skip)
        {
            bool[] tried = new bool[3];
            if (skip >= 0)
            {
                tried[skip] = true;
            }
            for (int round = 0; round < 2; round++)
            {
                int best = (-1);
                float best_short = 0.0f;
                for (int i = 0; i < 3; i++)
                {
                    int shortfall = gather_target[i] - gather_now[i];
                    if (tried[i] || shortfall <= 0)
                    {
                        continue;
                    }
                    float rel = (float)(shortfall / ((float)(java.lang.Math.max(1, gather_target[i]))));
                    if (rel > best_short)
                    {
                        best_short = rel;
                        best = i;
                    }
                }
                if (best < 0)
                {
                    return false;
                }
                tried[best] = true;
                if (sendGather(u, best))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 821
        public int neediestType()
        {
            int best = (-1);
            float best_short = 0.0f;
            for (int i = 0; i < 3; i++)
            {
                int shortfall = gather_target[i] - gather_now[i];
                if (shortfall <= 0)
                {
                    continue;
                }
                float rel = (float)(shortfall / ((float)(java.lang.Math.max(1, gather_target[i]))));
                if (rel > best_short)
                {
                    best_short = rel;
                    best = i;
                }
            }
            return best;
        }
        // Java: 837
        public bool sendGather(com.oddlabs.tt.model.Unit u, int type)
        {
            return sendGather(u, type, true);
        }
        // Java: 842
        public bool sendGather(com.oddlabs.tt.model.Unit u, int type, bool count)
        {
            com.oddlabs.tt.model.Building a = armory_f;
            if (a == null)
            {
                return false;
            }
            com.oddlabs.tt.model.Supply s = default;
            if (type == 0)
            {
                s = ctx.resources.bestTreeFrom(a, 4, 30, com.oddlabs.tt.player.ultra.ResourceTracker.TREE_CAP);
            }
            else
            {
                s = ctx.resources.bestNode(com.oddlabs.tt.player.ultra.EconomyManager.GATHER_TYPES[type], a);
            }
            if (s == null)
            {
                return false;
            }
            ctx.orders.gather(u, s);
            ctx.resources.noteAssigned(s);
            if (count)
            {
                gather_now[type]++;
            }
            return true;
        }
        // Java: 863
        public void weapons()
        {
            com.oddlabs.tt.model.Building a = armory_f;
            bool iron = !ctx.resources.iron().isEmpty() && !rock_emergency;
            ctx.orders.weapons(a, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), iron);
            ctx.orders.weapons(a, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), true);
            bool rock_now = rock_fallback && emergency && last_s_iron < 2 && last_s_rock >= 1 && last_s_wood >= 2;
            ctx.orders.weapons(a, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), rock_emergency || rock_weapons || surplus_rock || rock_now);
        }
        // Java: 874
        public void deployWarriors()
        {
            com.oddlabs.tt.model.Building a = armory_f;
            int inside = a.getUnitContainer().getNumSupplies();
            int keep = (emergency ? 0 : java.lang.Math.max(2, java.lang.Math.round((float)(workers_keep_frac * workers_target))));
            int avail = inside - keep;
            if (avail <= 0)
            {
                return;
            }
            com.oddlabs.tt.model.DeployContainer pc = a.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON);
            if (drain_rally && pc != null && pc.getNumSupplies() > 0)
            {
                return;
            }
            int reserve = ctx.military.latentReserve();
            int queued = 0;
            foreach (com.oddlabs.tt.model.DeployType t in new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR })
            {
                com.oddlabs.tt.model.DeployContainer c = a.getDeployContainer(t);
                if (c != null)
                {
                    queued += c.getNumSupplies();
                }
            }
            if (queued >= 6)
            {
                return;
            }
            int[] rally = ctx.military.rallyPoint();
            if (rally != null)
            {
                ctx.orders.rally(a, rally[0], rally[1]);
            }
            int rub = a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int n = java.lang.Math.min(rub, avail);
            if (n > 0)
            {
                ctx.orders.deploy(a, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, n);
                avail -= n;
            }
            int iron = a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() - reserve;
            n = java.lang.Math.min(iron, avail);
            if (n > 0)
            {
                ctx.orders.deploy(a, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, n);
                avail -= n;
            }
            int rock = a.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            n = java.lang.Math.min(rock, avail);
            if (n > 0)
            {
                ctx.orders.deploy(a, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, n);
            }
        }
        // Java: 920
        public void chickens()
        {
            com.oddlabs.tt.model.Building a = armory_f;
            java.util.List hunters = new java.util.ArrayList();
            java.util.List hunted = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)ctx.roleOf(u) != (object)com.oddlabs.tt.player.ultra.Context.Role.HUNTER))
                {
                    continue;
                }
                com.oddlabs.tt.model.Supply target = null;
                if ((com.oddlabs.tt.player.ultra.WorldModel.primary(u) is com.oddlabs.tt.model.behaviour.GatherController gc) && ((object)gc.getSupplyType() == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
                {
                    target = gc.getSupply();
                }
                hunters.add(u);
                if (target != null)
                {
                    hunted.add(target);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (!ctx.hasRole(u_2) && (com.oddlabs.tt.player.ultra.WorldModel.primary(u_2) is com.oddlabs.tt.model.behaviour.GatherController gc_2) && ((object)gc_2.getSupplyType() == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
                {
                    hunters.add(u_2);
                    if (gc_2.getSupply() != null)
                    {
                        hunted.add(gc_2.getSupply());
                    }
                }
            }
            if (hunters.size() >= hunter_max)
            {
                return;
            }
            int pop = ctx.owner.getUnitCountContainer().getNumSupplies();
            int dmax = (pop > (float)(ctx.world.getMaxUnitCount() * 0.7f) ? chicken_dist_far : chicken_dist);
            java.util.List chickens_v = ctx.map.freeChickens();
            java.util.List candidates = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.RubberSupply c in java.lang.JIter.of(chickens_v))
            {
                if (hunted.contains(c))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ultra.MapAnalysis.chebyshev(c.getGridX(), c.getGridY(), a.getGridX(), a.getGridY());
                if (d > dmax || ctx.resources.threatened(c.getGridX(), c.getGridY(), 20))
                {
                    continue;
                }
                if (ctx.map.frontness(c.getGridX(), c.getGridY()) > 0.55f)
                {
                    continue;
                }
                if (ctx.resources.nearerToAvoided(a.getGridX(), a.getGridY(), c.getGridX(), c.getGridY()))
                {
                    continue;
                }
                candidates.add(c);
            }
            candidates.sort(new java.util.Comparator_L((object x_l, object y_l) =>
            {
                com.oddlabs.tt.model.RubberSupply x = (com.oddlabs.tt.model.RubberSupply)x_l;
                com.oddlabs.tt.model.RubberSupply y = (com.oddlabs.tt.model.RubberSupply)y_l;
                int dx = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(x.getGridX(), x.getGridY(), a.getGridX(), a.getGridY());
                int dy = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(y.getGridX(), y.getGridY(), a.getGridX(), a.getGridY());
                if (dx != dy)
                {
                    return java.lang.Integer.compare(dx, dy);
                }
                if (x.getGridX() != y.getGridX())
                {
                    return java.lang.Integer.compare(x.getGridX(), y.getGridX());
                }
                return java.lang.Integer.compare(x.getGridY(), y.getGridY());
            }));
            int slots = hunter_max - hunters.size();
            foreach (com.oddlabs.tt.model.RubberSupply c_2 in java.lang.JIter.of(candidates))
            {
                if (slots <= 0)
                {
                    break;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(ctx.model.me.peons))
                {
                    if (com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u_3))
                    {
                        continue;
                    }
                    bool ok = isFree(u_3) || (!ctx.hasRole(u_3) && (com.oddlabs.tt.player.ultra.WorldModel.primary(u_3) is com.oddlabs.tt.model.behaviour.GatherController gc_3) && ((object)gc_3.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)));
                    if (!ok)
                    {
                        continue;
                    }
                    int d_2 = com.oddlabs.tt.player.ultra.MapAnalysis.dist2(u_3.getGridX(), u_3.getGridY(), c_2.getGridX(), c_2.getGridY());
                    if (d_2 < best_d)
                    {
                        best_d = d_2;
                        best = u_3;
                    }
                }
                if (best == null)
                {
                    break;
                }
                ctx.orders.gather(best, c_2);
                ctx.setRole(best, com.oddlabs.tt.player.ultra.Context.Role.HUNTER);
                slots--;
            }
        }
        // Java: 999
        public void dispatch()
        {
            java.util.List free = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (isFree(u))
                {
                    free.add(u);
                }
            }
            if (free.isEmpty())
            {
                return;
            }
            int builders_wanted = buildersWanted();
            com.oddlabs.tt.model.Building chief_q = ctx.chief.trainingQuarters();
            int chief_wanted = (chief_q != null && !ctx.military.quartersThreatened(chief_q) && !ctx.resources.threatened(chief_q.getGridX(), chief_q.getGridY(), 14) ? ctx.chief.peonsWanted() : 0);
            if (chief_wanted > 0)
            {
                foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(ctx.model.me.peons))
                {
                    com.oddlabs.tt.player.ultra.Orders.Last l = ctx.orders.lastOrder(p);
                    if (!ctx.hasRole(p) && l != null && ((object)l.kind() == (object)com.oddlabs.tt.player.ultra.Orders.Kind.ENTER) && ((object)l.target() == (object)chief_q) && (com.oddlabs.tt.player.ultra.WorldModel.primary(p) is com.oddlabs.tt.model.behaviour.EnterController) && (float)(ctx.now - l.time()) < 30.0f)
                    {
                        chief_wanted--;
                    }
                }
                chief_wanted = java.lang.Math.max(0, chief_wanted);
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(free))
            {
                if (ctx.military.inDanger(u_2.getGridX(), u_2.getGridY()))
                {
                    refuge(u_2);
                    continue;
                }
                com.oddlabs.tt.player.ultra.BuildManager.Task site = (builders_wanted > 0 ? ctx.build.neediest() : (com.oddlabs.tt.player.ultra.BuildManager.Task)(null));
                if (site != null)
                {
                    ctx.build.assign(u_2, site);
                    builders_wanted--;
                    continue;
                }
                if (armory_f == null)
                {
                    com.oddlabs.tt.model.Building q = surplusQuarters(u_2);
                    if (q != null && !com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u_2))
                    {
                        surplus_until = (float)(ctx.now + 10.0f);
                        ctx.orders.enter(u_2, q);
                    }
                    else
                    {
                        prechop(u_2);
                    }
                    continue;
                }
                if (com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u_2))
                {
                    ctx.orders.enter(u_2, armory_f);
                    continue;
                }
                if (chief_wanted > 0 && !ctx.orders.recently(u_2, 3.0f))
                {
                    ctx.orders.enter(u_2, chief_q);
                    chief_wanted--;
                    continue;
                }
                if (stocks_ok && worker_deficit > 0 && !ctx.military.inDanger(armory_f.getGridX(), armory_f.getGridY()))
                {
                    ctx.orders.enter(u_2, armory_f);
                    worker_deficit--;
                    continue;
                }
                int type = neediestType();
                if (type >= 0 && sendGather(u_2, type))
                {
                    continue;
                }
                if (unfreeze && sendOtherGather(u_2, type))
                {
                    continue;
                }
                if ((com.oddlabs.tt.player.ultra.WorldModel.primary(u_2) is com.oddlabs.tt.model.behaviour.TransferUnitController) && !shelterBlocked(u_2, armory_f))
                {
                    continue;
                }
                ctx.orders.enter(u_2, armory_f);
            }
        }
        // Java: 1071
        public com.oddlabs.tt.model.Building surplusQuarters(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Building best = null;
            int best_inside = com.oddlabs.tt.player.ultra.EconomyManager.SURPLUS_KEEP;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ctx.model.me.quarters))
            {
                if (q.isDead() || ctx.resources.threatened(q.getGridX(), q.getGridY(), 14))
                {
                    continue;
                }
                int inside = q.getUnitContainer().getNumSupplies();
                if (inside < best_inside)
                {
                    best_inside = inside;
                    best = q;
                }
            }
            return best;
        }
        // Java: 1087
        public int buildersWanted()
        {
            if (((object)phase_f == (object)com.oddlabs.tt.player.ultra.EconomyManager.Phase.OPENING) || ((object)phase_f == (object)com.oddlabs.tt.player.ultra.EconomyManager.Phase.RECOVER))
            {
                return java.lang.Integer.MAX_VALUE;
            }
            int builders = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(ctx.model.me.peons))
            {
                if (((object)ctx.roleOf(u) == (object)com.oddlabs.tt.player.ultra.Context.Role.BUILDER))
                {
                    builders++;
                }
            }
            int cap = java.lang.Math.round((float)(builder_cap * (econPeons() + builders)));
            return java.lang.Math.max(0, cap - builders);
        }
        // Java: 1100
        public void prechop(com.oddlabs.tt.model.Unit u)
        {
            if (com.oddlabs.tt.player.ultra.BuildManager.carriesWood(u))
            {
                com.oddlabs.tt.player.ultra.BuildManager.Task site = ctx.build.neediest();
                if (site != null)
                {
                    ctx.build.assign(u, site);
                }
                return;
            }
            int cx = ctx.map.home_x;
            int cy = ctx.map.home_y;
            foreach (com.oddlabs.tt.player.ultra.BuildManager.Task t in java.lang.JIter.of(ctx.build.tasks()))
            {
                if (t.open())
                {
                    cx = t.x;
                    cy = t.y;
                    break;
                }
            }
            if (ctx.orders.recently(u, 4.0f))
            {
                return;
            }
            com.oddlabs.tt.landscape.TreeSupply tree = ctx.resources.bestTree(cx, cy, 5, 16, ctx.strategy.i("prechopPerTree"));
            if (tree != null)
            {
                ctx.orders.gather(u, tree);
                ctx.resources.noteAssigned(tree);
            }
        }
        // Java: 1125
        public void refuge(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Building a = armory_f;
            if (a != null)
            {
                ctx.orders.enter(u, a);
                return;
            }
            if (!com.oddlabs.tt.player.ultra.BuildManager.carriesAnything(u))
            {
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ctx.model.me.quarters))
                {
                    if (!q.isDead() && !ctx.military.quartersThreatened(q) && !ctx.resources.threatened(q.getGridX(), q.getGridY(), 14))
                    {
                        ctx.orders.enter(u, q);
                        return;
                    }
                }
            }
            if (!ctx.orders.recently(u, 2.5f))
            {
                int[] away = ctx.military.awayFromEnemy(u.getGridX(), u.getGridY(), 14);
                ctx.orders.move(u, away[0], away[1]);
            }
        }
        // Java: 1146
        public int workersTarget()
        {
            return workers_target;
        }
        static EconomyManager()
        {
            EconomyManager.GATHER_TYPES = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RockSupply) };
            EconomyManager.HARVEST = new com.oddlabs.tt.model.DeployType[] { com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK };
        }
        private void init_EconomyManager()
        {
            this.phase_f = com.oddlabs.tt.player.ultra.EconomyManager.Phase.OPENING;
            this.last_expand_check = java.lang.Float.NEGATIVE_INFINITY;
            this.draining = new java.util.ArrayList();
            this.armory_missing_since = (float)(-1.0f);
            this.gather_probe = new java.util.LinkedHashMap();
            this.last_probe = java.lang.Float.NEGATIVE_INFINITY;
            this.last_recover = java.lang.Float.NEGATIVE_INFINITY;
            this.surplus_until = java.lang.Float.NEGATIVE_INFINITY;
            this.gather_target = new int[3];
            this.gather_now = new int[3];
            this.last_chickens = java.lang.Float.NEGATIVE_INFINITY;
            this.iron_dry_since = (float)(-1.0f);
            this.iron_wet_since = (float)(-1.0f);
        }
        public EconomyManager(com.oddlabs.tt.player.ultra.Context ctx) : base()
        {
            init_EconomyManager();
            this.ctx = ctx;
            com.oddlabs.tt.player.ultra.Strategy s = ctx.strategy;
            quarters_max = s.i("quartersMax");
            n_keep_opening = s.i("nKeepOpening");
            n_keep_boom = s.i("nKeepBoom");
            n_keep_cap = s.i("nKeepCap");
            cap_margin = s.i("capMargin");
            builder_frac_boom = s.f("builderFracBoom");
            stock_wood = s.f("stockWood");
            stock_iron = s.f("stockIron");
            stock_rock = s.f("stockRock");
            stock_tau = s.f("stockTau");
            hunter_max = s.i("hunterMax");
            chicken_dist = s.i("chickenDist");
            chicken_dist_far = s.i("chickenDistFar");
            workers_keep_frac = s.f("workersKeepFrac");
            builders_boom_site = s.i("buildersBoomSite");
            wood_reserve = s.i("woodReserve");
            expansion = s.b("expansion");
            surplus_out = s.b("surplusOut");
            unfreeze = s.b("unfreeze");
            deploy_gate = s.b("deployGate");
            deploy_threat_r = s.i("deployThreatR");
            expand_front = s.f("expandFront");
            iron_min_nodes = s.i("ironMinNodes");
            rock_min_nodes = s.i("rockMinNodes");
            wood_reserve_time = s.f("woodReserveTime");
            iron_reach = s.i("ironReach");
            rock_mix = s.f("rockMix");
            rock_eff = s.f("rockEff");
            expand_iron = s.f("expandIron");
            siege_evac = s.b("siegeEvac");
            evac_keep = s.i("evacKeep");
            rock_fallback = s.b("rockFallback");
            qb_fixed = s.b("qbFixed");
            qb_prio = s.i("qbPrio");
            iron_dry_rock = s.b("ironDryRock");
            iron_dry_delay = s.f("ironDryDelay");
            builder_cap = s.f("builderCap");
            ctx.resources.reach_limit = iron_reach + 40;
        }
    }
}

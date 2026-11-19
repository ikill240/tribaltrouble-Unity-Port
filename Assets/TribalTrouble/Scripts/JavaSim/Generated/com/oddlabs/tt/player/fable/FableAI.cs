// Translated from FableAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 24
    public class FableAI : com.oddlabs.tt.player.AI, com.oddlabs.tt.player.fable.AiStatus
    {
        public static bool strict;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public com.oddlabs.tt.player.fable.Strategy strategy;
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Jobs jobs;
        public com.oddlabs.tt.player.fable.ResourceMap resources;
        public com.oddlabs.tt.player.fable.SiteFinder sites;
        public com.oddlabs.tt.player.fable.BasePlan plan;
        public com.oddlabs.tt.player.fable.Roster roster;
        public com.oddlabs.tt.player.fable.Intel intel;
        public com.oddlabs.tt.player.fable.Threat threat;
        public com.oddlabs.tt.player.fable.Construction construction;
        public com.oddlabs.tt.player.fable.Economy economy;
        public com.oddlabs.tt.player.fable.BuildPlanner planner;
        public com.oddlabs.tt.player.fable.Chieftain chieftain;
        public com.oddlabs.tt.player.fable.Military military;
        public float clock;
        public float fast_acc;
        public float slow_acc;
        public float econ_acc;
        public float last_status;
        public bool initialised;
        public bool dead;
        public int errors;
        public com.oddlabs.tt.model.LandBuilding training_quarters;
        public float training_started;
        // Java: 76
        public override bool isHardDifficulty()
        {
            return true;
        }
        // Java: 81
        public override void lateStart(float match_seconds)
        {
            clock = match_seconds;
        }
        // Java: 86
        public override void animate(float t)
        {
            if (!com.oddlabs.tt.global.Globals.run_ai || dead)
            {
                return;
            }
            clock += t;
            try
            {
                if (!initialised)
                {
                    init();
                    return;
                }
                if (!getOwner().isAlive())
                {
                    dead = true;
                    log.info("player dead, AI stops");
                    return;
                }
                fast_acc += t;
                slow_acc += t;
                econ_acc += t;
                if (slow_acc >= @params.slow_period)
                {
                    float dt = slow_acc;
                    slow_acc = 0.0f;
                    slowLane(dt);
                }
                if (fast_acc >= @params.fast_period)
                {
                    fast_acc = 0.0f;
                    fastLane();
                }
                if (econ_acc >= @params.econ_period)
                {
                    econ_acc = 0.0f;
                    economyLane();
                }
            }
            catch (System.Exception e) when (e is System.Exception || e is System.Exception)
            {
                errors++;
                java.lang.IO.println("FableAI(" + java.lang.JStr.valueOf(getOwner()) + ") error #" + (errors) + " at t=" + java.lang.JStr.valueOf(clock) + ": " + java.lang.JStr.valueOf(e));
                if (com.oddlabs.tt.player.fable.FableAI.strict || errors <= 3)
                {
                    e.printStackTrace();
                }
                if (com.oddlabs.tt.player.fable.FableAI.strict)
                {
                    throw e;
                }
                if (errors > 200)
                {
                    dead = true;
                    java.lang.IO.println("FableAI(" + java.lang.JStr.valueOf(getOwner()) + ") disabled after repeated errors");
                }
            }
        }
        // Java: 131
        public void init()
        {
            com.oddlabs.tt.player.Player owner = getOwner();
            int sx = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartX());
            int sy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartY());
            int island = owner.getWorld().getHeightMap().getIslandId(sx, sy);
            resources = new com.oddlabs.tt.player.fable.ResourceMap(owner.getWorld(), island);
            sites = new com.oddlabs.tt.player.fable.SiteFinder(owner);
            orders = new com.oddlabs.tt.player.fable.Orders(owner, log);
            jobs = new com.oddlabs.tt.player.fable.Jobs();
            plan = new com.oddlabs.tt.player.fable.BasePlan(owner, resources, sites, @params, log);
            plan.init();
            roster = new com.oddlabs.tt.player.fable.Roster(owner);
            intel = new com.oddlabs.tt.player.fable.Intel(owner);
            threat = new com.oddlabs.tt.player.fable.Threat();
            construction = new com.oddlabs.tt.player.fable.Construction(orders, jobs, sites, @params, log);
            economy = new com.oddlabs.tt.player.fable.Economy(owner, orders, jobs, resources, plan, @params, log);
            planner = new com.oddlabs.tt.player.fable.BuildPlanner(orders, jobs, construction, plan, economy, @params, log);
            planner.setResources(resources);
            planner.setSiteFinder(sites);
            chieftain = new com.oddlabs.tt.player.fable.Chieftain(orders, @params, log);
            military = new com.oddlabs.tt.player.fable.Military(owner, orders, jobs, @params, log, plan, chieftain);
            initialised = true;
            roster.refresh();
            intel.refresh();
            log.info(new java.util.function.Supplier_L(() =>
            {
                return "init: trees " + (resources.countTotal(com.oddlabs.tt.player.fable.ResourceMap.Kind.TREE)) + " iron " + (resources.countTotal(com.oddlabs.tt.player.fable.ResourceMap.Kind.IRON)) + " rock " + (resources.countTotal(com.oddlabs.tt.player.fable.ResourceMap.Kind.ROCK)) + " peons " + (roster.peons.size());
            }));
            slow_acc = @params.slow_period;
        }
        // Java: 162
        public void slowLane(float dt)
        {
            roster.refresh();
            intel.refresh();
            jobs.prune();
            threat.update(clock, dt, roster, intel, plan, @params);
            military.tick(clock, dt, roster, intel, threat, economy);
            planner.tick(clock, roster, intel, threat, military);
            manageChieftainTraining();
            if ((float)(clock - last_status) >= 30.0f)
            {
                last_status = clock;
                log.info(new java.util.function.Supplier_L(() => { return this.debugStatus(); }));
            }
        }
        // Java: 176
        public void fastLane()
        {
            com.oddlabs.tt.model.Unit chief = roster.chieftain;
            if (chief != null && !chief.isDead() && !chief.isMounted())
            {
                intel.refresh();
                int cx = chief.getGridX();
                int cy = chief.getGridY();
                bool fighting = military.fightingNear(cx, cy, 25) || intel.warriorsNear(cx, cy, 12) > 0;
                int own_near = com.oddlabs.tt.player.fable.Chieftain.ownWarriorsNear(roster.warriors, cx, cy, 15);
                int own18 = com.oddlabs.tt.player.fable.Chieftain.ownUnitsNear(roster, cx, cy, 18);
                bool defending = ((object)military.posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND);
                chieftain.decideCast(clock, chief, intel, threat, fighting, own_near, own18, defending, roster.warriors);
            }
            military.recoverStunned(clock, roster);
            economy.deployWarriors(roster);
        }
        // Java: 193
        public void economyLane()
        {
            roster.refresh();
            economy.tick(clock, roster, intel, training_quarters, roster.totalPeons());
            if (roster.mainArmory() == null && !planner.openingDone())
            {
                economy.parkIdlePeons(clock, roster);
            }
        }
        // Java: 203
        public void manageChieftainTraining()
        {
            com.oddlabs.tt.player.Player owner = getOwner();
            if (training_quarters != null && (training_quarters.isDead() || !owner.isTrainingChieftain()))
            {
                training_quarters = null;
            }
            if (owner.hasActiveChieftain() || owner.isTrainingChieftain() || roster.quarters.isEmpty())
            {
                return;
            }
            if (roster.quarters.size() < @params.chieftain_min_quarters || clock < @params.chieftain_min_time || roster.totalPeons() < @params.chieftain_min_peons)
            {
                return;
            }
            com.oddlabs.tt.model.LandBuilding best = null;
            int best_inside = (-1);
            foreach (com.oddlabs.tt.model.LandBuilding q in java.lang.JIter.of(roster.quarters))
            {
                int inside = com.oddlabs.tt.player.fable.Roster.unitsInside(q);
                if (inside > best_inside)
                {
                    best_inside = inside;
                    best = q;
                }
            }
            if (best == null)
            {
                return;
            }
            orders.trainChieftain(best, true);
            if (owner.isTrainingChieftain())
            {
                training_quarters = best;
                training_started = clock;
                com.oddlabs.tt.model.LandBuilding q_2 = best;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "chieftain training started in quarters " + (q_2.getGridX()) + "," + (q_2.getGridY());
                }));
            }
        }
        // Java: 234
        public string debugStatus()
        {
            if (!initialised)
            {
                return "init";
            }
            return java.lang.JString.format("t=%.0f %s | %s | %s | %s | peons=%d(q%d a%d) warriors=%d(t%d) chief=%s", clock, military.status(), threat, economy.status(roster), planner.status(), roster.totalPeons(), roster.peons_in_quarters, roster.peons_in_armory, roster.totalWarriors(), roster.warriors_in_towers, (roster.chieftain == null ? ((getOwner().isTrainingChieftain() ? (object)("training") : (object)("none"))) : (object)((roster.chieftain.getHitPoints()) + "hp/" + (java.lang.JC.f2i(((float)(100 * com.oddlabs.tt.player.fable.Chieftain.stunProgress(roster.chieftain))))) + "%")));
        }
        static FableAI()
        {
            FableAI.strict = false;
        }
        private void init_FableAI()
        {
            this.last_status = (float)(-100.0f);
            this.training_started = (float)(-1.0f);
        }
        public FableAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info) : this(owner, unit_info, "")
        {
        }
        public FableAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, string overrides) : base(owner, unit_info)
        {
            init_FableAI();
            com.oddlabs.tt.player.NavalCommander.attach(owner);
            com.oddlabs.tt.landscape.World world = owner.getWorld();
            world.getAnimationManagerRealTime().removeAnimation(this);
            world.getAnimationManagerGameTime().registerAnimation(this);
            this.strategy = com.oddlabs.tt.player.fable.Strategy.forWorld(world, owner);
            this.@params = strategy.@params();
            if (!overrides.isBlank())
            {
                @params.@override(overrides);
            }
            this.log = new com.oddlabs.tt.player.fable.AiLog(owner.getPlayerInfo().getName(), new java.util.function.Supplier_L(() =>
            {
                return clock;
            }));
            log.info("FableAI created, strategy " + java.lang.JStr.valueOf(strategy.name()) + java.lang.JStr.valueOf(((overrides.isBlank() ? "" : " overrides " + java.lang.JStr.valueOf(overrides)))));
        }
    }
}

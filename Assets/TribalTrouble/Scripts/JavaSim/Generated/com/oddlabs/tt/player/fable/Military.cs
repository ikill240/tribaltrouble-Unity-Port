// Translated from Military.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 29
    public class Military : java.lang.Object
    {
        // Java: 30
        public sealed class Posture : java.lang.Enum
        {
            public static Posture[] values() { return (Posture[])VALUES_.Clone(); }
            public static Posture valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Posture BUILD_UP = new Posture("BUILD_UP", 0);
            public static readonly Posture DEFEND = new Posture("DEFEND", 1);
            public static readonly Posture ATTACK = new Posture("ATTACK", 2);
            public static readonly Posture ALL_IN = new Posture("ALL_IN", 3);
            public static readonly Posture[] VALUES_ = new Posture[] { BUILD_UP, DEFEND, ATTACK, ALL_IN };
            public Posture(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 37
        public sealed class ArmyState : java.lang.Enum
        {
            public static ArmyState[] values() { return (ArmyState[])VALUES_.Clone(); }
            public static ArmyState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly ArmyState STAGING = new ArmyState("STAGING", 0);
            public static readonly ArmyState MARCH = new ArmyState("MARCH", 1);
            public static readonly ArmyState ENGAGE = new ArmyState("ENGAGE", 2);
            public static readonly ArmyState ASSAULT = new ArmyState("ASSAULT", 3);
            public static readonly ArmyState SIEGE_PREP = new ArmyState("SIEGE_PREP", 4);
            public static readonly ArmyState SIEGE_CYCLE = new ArmyState("SIEGE_CYCLE", 5);
            public static readonly ArmyState RETREAT = new ArmyState("RETREAT", 6);
            public static readonly ArmyState[] VALUES_ = new ArmyState[] { STAGING, MARCH, ENGAGE, ASSAULT, SIEGE_PREP, SIEGE_CYCLE, RETREAT };
            public ArmyState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public const int TAG_HOME = 1;
        public const int TAG_ARMY = 2;
        public const int TAG_REINF = 3;
        public const int TAG_GARRISON = 4;
        public const int TAG_FOCUS = 5;
        public const int TAG_CORPS = 6;
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.player.fable.Orders orders;
        public com.oddlabs.tt.player.fable.Jobs jobs;
        public com.oddlabs.tt.player.fable.Params @params;
        public com.oddlabs.tt.player.fable.AiLog log;
        public com.oddlabs.tt.player.fable.BasePlan plan;
        public com.oddlabs.tt.player.fable.Chieftain chief_brain;
        public com.oddlabs.tt.pathfinder.UnitGrid grid;
        public com.oddlabs.tt.player.fable.Micro micro;
        public com.oddlabs.tt.player.fable.Military.Posture posture;
        public com.oddlabs.tt.player.fable.Military.ArmyState army_state;
        public float posture_since;
        public float army_state_since;
        public java.util.Set army;
        public java.util.Set reinforcements;
        public java.util.Set home_guard;
        public int objective_x;
        public int objective_y;
        public com.oddlabs.tt.model.Selectable objective;
        public int leg_x;
        public int leg_y;
        public int prev_leg_x;
        public int prev_leg_y;
        public float leg_ordered_at;
        public int leg_skips;
        public float last_engage_order;
        public int last_engage_x;
        public int last_engage_y;
        public float engage_start_value;
        public float hold_since;
        public com.oddlabs.tt.model.LandBuilding siege_tower;
        public int stand_x;
        public int stand_y;
        public int army_pos_x;
        public int army_pos_y;
        public float siege_cast_at;
        public float siege_prep_since;
        public int staging_x;
        public int regroup_x;
        public int regroup_y;
        public int hold_x;
        public int hold_y;
        public int staging_y;
        public int rally_x;
        public int rally_armories;
        public bool economy_rally_dirty;
        public int rally_y;
        public float last_defend_order;
        public int last_defend_x;
        public int last_defend_y;
        public float last_garrison_swap;
        public float last_focus_at;
        public float last_leash;
        public java.util.List scratch_targets;
        // Java: 133
        public float unitValue(com.oddlabs.tt.model.Unit u)
        {
            System.Type type = u.getWeaponFactory().getType();
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
            {
                return @params.value_rock;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)))
            {
                return @params.value_rubber;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)))
            {
                return @params.value_iron;
            }
            return 0.1f;
        }
        // Java: 144
        public float towerValue(com.oddlabs.tt.model.LandBuilding t)
        {
            if (t.isDead() || !t.isComplete() || t.getUnitContainer() == null)
            {
                return 0.0f;
            }
            if (!((t.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m)) || m.getUnit() == null)
            {
                return 0.0f;
            }
            com.oddlabs.tt.model.Unit u = m.getUnit();
            if (com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
            {
                return 0.0f;
            }
            System.Type type = u.getWeaponFactory().getType();
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)))
            {
                return @params.value_tower_rubber;
            }
            if (((object)type == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)))
            {
                return @params.value_tower_rock;
            }
            return @params.value_tower_iron;
        }
        // Java: 161
        public float ownValueNear(com.oddlabs.tt.player.fable.Roster roster, int gx, int gy, int radius)
        {
            int r2 = radius * radius;
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
            {
                if (!com.oddlabs.tt.player.fable.Chieftain.isFrozen(u) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), gx, gy) <= r2)
                {
                    v = (float)(v + unitValue(u));
                }
            }
            if (roster.chieftain != null && !roster.chieftain.isDead() && com.oddlabs.tt.player.fable.BasePlan.dist2(roster.chieftain.getGridX(), roster.chieftain.getGridY(), gx, gy) <= r2)
            {
                v = (float)(v + @params.value_chief);
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(roster.towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), gx, gy) <= @params.tower_reach2)
                {
                    v = (float)(v + towerValue(t));
                }
            }
            return v;
        }
        // Java: 177
        public float enemyValueNear(com.oddlabs.tt.player.fable.Intel intel, int gx, int gy, int radius)
        {
            int r2 = radius * radius;
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                if (!com.oddlabs.tt.player.fable.Chieftain.isFrozen(u) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), gx, gy) <= r2)
                {
                    v = (float)(v + unitValue(u));
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.chieftains))
            {
                if (!com.oddlabs.tt.player.fable.Chieftain.isFrozen(u_2) && com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), gx, gy) <= r2)
                {
                    v = (float)(v + @params.value_chief);
                }
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), gx, gy) <= @params.tower_reach2)
                {
                    v = (float)(v + towerValue(t));
                }
            }
            return v;
        }
        // Java: 192
        public float enemyTotalValue(com.oddlabs.tt.player.fable.Intel intel)
        {
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.warriors))
            {
                v = (float)(v + unitValue(u));
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(intel.chieftains))
            {
                v = (float)(v + @params.value_chief);
            }
            return v;
        }
        // Java: 201
        public float groupValue(java.util.Set group)
        {
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
            {
                if (orders.usable(u))
                {
                    v = (float)(v + unitValue(u));
                }
            }
            return v;
        }
        // Java: 209
        public int[] centroid(java.util.Set group)
        {
            long sx = 0;
            long sy = 0;
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(group))
            {
                if (!orders.usable(u))
                {
                    continue;
                }
                sx += u.getGridX();
                sy += u.getGridY();
                n++;
            }
            return (n == 0 ? (int[])(null) : new int[] { unchecked((int)((sx / n))), unchecked((int)((sy / n))) });
        }
        // Java: 224
        public void tick(float now, float dt, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, com.oddlabs.tt.player.fable.Economy economy)
        {
            prune(roster);
            com.oddlabs.tt.model.LandBuilding armory = roster.mainArmory();
            int[] home = homeCell(roster);
            if (economy.rally_dirty)
            {
                economy.rally_dirty = false;
                economy_rally_dirty = true;
            }
            updateStaging(now, roster, armory, home);
            adoptNewWarriors(roster, home);
            garrisonTowers(now, roster, intel, threat, home);
            updatePosture(now, roster, intel, threat, economy, home);
            economy.demands.emergency = ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND) && (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED) || (threat.approaching && threat.eta < 30.0f));
            economy.demands.armory_threat = armory != null && intel.warriorsNear(armory.getGridX(), armory.getGridY(), 20) >= 3;
            economy.demands.want_rock_warriors = wantRockWarriors(now, roster, intel, threat, armory);
            threat.hotCells(intel, economy.demands.hot_cells);
            economy.demands.hot_radius = @params.hot_node_radius;
            economy.demands.cap_assault = ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN);
            switch ((posture).ordinal())
            {
                case 1 /*DEFEND*/:
                {
                    float field = (float)(groupValue(army) + groupValue(home_guard));
                    bool camping = (float)(now - posture_since) > @params.defend_sweep_after && threat.approach_value > 0.0f && field >= (float)(@params.engage_ratio * threat.approach_value);
                    if (!(camping && fieldSweep(now, roster, intel, home)))
                    {
                        defend(now, roster, intel, threat, home, allFieldUnits());
                    }
                    break;
                }
                case 0 /*BUILD_UP*/:
                {
                    holdHome(now, roster, intel, home);
                    break;
                }
                case 2 /*ATTACK*/:
                case 3 /*ALL_IN*/:
                {
                    if (threat_serious)
                    {
                        java.util.List defenders = new java.util.ArrayList(home_guard);
                        defenders.addAll(reinforcements);
                        defend(now, roster, intel, threat, home, defenders);
                    }
                    else
                    {
                        homeGuardStep(now, roster, intel, home);
                    }
                    armyStep(now, dt, roster, intel, threat, home);
                    break;
                }
            }
            reinforcementsStep(now, roster, home);
            corpsStep(now, roster, intel, economy, home);
            focusStunned(now, roster, intel);
            leash(now, roster, intel);
            positionChieftain(now, roster, intel, threat, home);
            chief_brain.hold_for_siege = ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP) && siege_tower != null;
        }
        public java.util.Set corps;
        public float last_corps_order;
        public float last_corps_recruit;
        public com.oddlabs.tt.model.LandBuilding corps_target;
        // Java: 287
        public void corpsStep(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Economy economy, int[] home)
        {
            corps.removeIf(new java.util.function.Predicate_L((object p_l) =>
            {
                com.oddlabs.tt.model.Unit p = (com.oddlabs.tt.model.Unit)p_l;
                return !orders.usable(p);
            }));
            economy.reserved.removeIf(new java.util.function.Predicate_L((object p_2_l) =>
            {
                com.oddlabs.tt.model.Unit p_2 = (com.oddlabs.tt.model.Unit)p_2_l;
                return !corps.contains(p_2);
            }));
            bool attacking = (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN)) && army.size() >= @params.corps_min_army;
            if (!attacking || !@params.corps)
            {
                if (!corps.isEmpty())
                {
                    foreach (com.oddlabs.tt.model.Unit p_3 in java.lang.JIter.of(corps))
                    {
                        jobs.clear(p_3);
                    }
                    log.info("corps released (" + (corps.size()) + ")");
                    corps.clear();
                    economy.reserved.clear();
                }
                corps_target = null;
                return;
            }
            int[] ac0 = centroid(army);
            bool army_near_home = ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING) || (ac0 != null && com.oddlabs.tt.player.fable.BasePlan.dist2(ac0[0], ac0[1], staging_x, staging_y) <= 60 * 60);
            if (corps.size() < @params.corps_size && (float)(now - last_corps_recruit) > 10.0f && army_near_home)
            {
                last_corps_recruit = now;
                int[] at = (((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING) ? new int[] { staging_x, staging_y } : ac0);
                if (at != null)
                {
                    java.util.List crew = economy.requestCrew(@params.corps_size - corps.size(), at[0], at[1], roster, now, false);
                    foreach (com.oddlabs.tt.model.Unit p_4 in java.lang.JIter.of(crew))
                    {
                        corps.add(p_4);
                        economy.reserved.add(p_4);
                        orders.move(p_4, at[0], at[1]);
                        jobs.set(p_4, com.oddlabs.tt.player.fable.Jobs.Kind.HOLD, at[0], at[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_CORPS);
                    }
                    if (!crew.isEmpty())
                    {
                        log.info(new java.util.function.Supplier_L(() =>
                        {
                            return "corps recruited " + (crew.size()) + " peons (" + (corps.size()) + ")";
                        }));
                    }
                }
            }
            if (corps.isEmpty())
            {
                return;
            }
            if (corps_target != null && (corps_target.isDead() || (!intel.towers.contains(corps_target) && !intel.construction_sites.contains(corps_target))))
            {
                corps_target = null;
            }
            if (corps_target != null)
            {
                return;
            }
            int[] c = centroid(army);
            if (c == null)
            {
                return;
            }
            int[] p_5 = (objective_x >= 0 ? plan.toward(c[0], c[1], objective_x, objective_y, -@params.corps_follow_dist) : c);
            int[] cc = centroid(corps);
            bool threatened = cc != null && intel.warriorsNear(cc[0], cc[1], 8) >= 2;
            if (threatened)
            {
                p_5 = plan.toward(c[0], c[1], (objective_x >= 0 ? objective_x : cc[0]), (objective_y >= 0 ? objective_y : cc[1]), (-3));
            }
            if ((float)(now - last_corps_order) < 3.0f && !threatened)
            {
                return;
            }
            last_corps_order = now;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(corps))
            {
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                bool there = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), p_5[0], p_5[1]) <= 36;
                if (there && !threatened)
                {
                    continue;
                }
                if (j != null && ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.MOVE) && com.oddlabs.tt.player.fable.BasePlan.dist2(j.gx, j.gy, p_5[0], p_5[1]) <= 16 && !java.lang.JO.equals(com.oddlabs.tt.player.fable.Roster.activity(u), com.oddlabs.tt.player.fable.Roster.Activity.IDLE))
                {
                    continue;
                }
                orders.move(u, p_5[0], p_5[1]);
                jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, p_5[0], p_5[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_CORPS);
            }
        }
        // Java: 361
        public bool corpsDemolish(float now, com.oddlabs.tt.model.LandBuilding tower, int[] c)
        {
            if (corps.size() < @params.corps_min_demolish || tower.isDead())
            {
                return false;
            }
            int tx = tower.getGridX();
            int ty = tower.getGridY();
            int near = 0;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(corps))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(p.getGridX(), p.getGridY(), tx, ty) <= 45 * 45)
                {
                    near++;
                }
            }
            if (near < @params.corps_min_demolish)
            {
                return false;
            }
            if (((object)corps_target != (object)tower))
            {
                corps_target = tower;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "corps: demolishing tower at " + (tx) + "," + (ty) + " with " + (corps.size()) + " peons";
                }));
            }
            if ((float)(now - last_corps_order) < 2.0f)
            {
                return true;
            }
            last_corps_order = now;
            foreach (com.oddlabs.tt.model.Unit p_2 in java.lang.JIter.of(corps))
            {
                if (com.oddlabs.tt.player.fable.Chieftain.isFrozen(p_2))
                {
                    continue;
                }
                if ((p_2.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController hc) && ((object)hc.getTarget() == (object)tower))
                {
                    continue;
                }
                orders.attack(p_2, tower);
                jobs.set(p_2, com.oddlabs.tt.player.fable.Jobs.Kind.HUNT, tx, ty, tower, null, now, com.oddlabs.tt.player.fable.Military.TAG_CORPS);
            }
            return true;
        }
        // Java: 390
        public void prune(com.oddlabs.tt.player.fable.Roster roster)
        {
            army.removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return !orders.usable(u);
            }));
            reinforcements.removeIf(new java.util.function.Predicate_L((object u_2_l) =>
            {
                com.oddlabs.tt.model.Unit u_2 = (com.oddlabs.tt.model.Unit)u_2_l;
                return !orders.usable(u_2);
            }));
            home_guard.removeIf(new java.util.function.Predicate_L((object u_3_l) =>
            {
                com.oddlabs.tt.model.Unit u_3 = (com.oddlabs.tt.model.Unit)u_3_l;
                return !orders.usable(u_3);
            }));
            java.util.Set alive = new java.util.LinkedHashSet(roster.warriors);
            army.retainAll(alive);
            reinforcements.retainAll(alive);
            home_guard.retainAll(alive);
        }
        // Java: 400
        public int[] homeCell(com.oddlabs.tt.player.fable.Roster roster)
        {
            com.oddlabs.tt.model.LandBuilding a = roster.mainArmory();
            if (a != null)
            {
                return new int[] { a.getGridX(), a.getGridY() };
            }
            if (!roster.quarters.isEmpty())
            {
                return new int[] { ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst()).getGridX(), ((com.oddlabs.tt.model.LandBuilding)roster.quarters.getFirst()).getGridY() };
            }
            return new int[] { plan.start_x, plan.start_y };
        }
        // Java: 409
        public void updateStaging(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.model.LandBuilding armory, int[] home)
        {
            int[] s = plan.along(home[0], home[1], @params.staging_dist);
            if (!plan.sameIsland(s[0], s[1]) || grid.getRegion(s[0], s[1]) == null)
            {
                s = plan.along(home[0], home[1], 8);
            }
            if (armory != null && (rally_x != s[0] || rally_y != s[1] || rally_armories != roster.armories.size() || economy_rally_dirty))
            {
                foreach (com.oddlabs.tt.model.LandBuilding a in java.lang.JIter.of(roster.armories))
                {
                    if (orders.complete(a))
                    {
                        orders.setRally(a, s[0], s[1]);
                    }
                }
                rally_x = s[0];
                rally_y = s[1];
                rally_armories = roster.armories.size();
                economy_rally_dirty = false;
            }
            bool regrouping = regroup_x >= 0 && ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING) && (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN));
            if (regrouping)
            {
                staging_x = regroup_x;
                staging_y = regroup_y;
            }
            else
            {
                regroup_x = (-1);
                regroup_y = (-1);
                staging_x = s[0];
                staging_y = s[1];
            }
        }
        // Java: 440
        public void adoptNewWarriors(com.oddlabs.tt.player.fable.Roster roster, int[] home)
        {
            int guard_target = guardTarget();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
            {
                if (army.contains(u) || reinforcements.contains(u) || home_guard.contains(u))
                {
                    continue;
                }
                if (((object)jobs.kindOf(u) == (object)com.oddlabs.tt.player.fable.Jobs.Kind.GARRISON))
                {
                    continue;
                }
                if (home_guard.size() < guard_target)
                {
                    home_guard.add(u);
                }
                else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
                {
                    if (((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING))
                    {
                        army.add(u);
                    }
                    else
                    {
                        reinforcements.add(u);
                    }
                }
                else
                {
                    army.add(u);
                }
            }
            while (home_guard.size() > guard_target + 2)
            {
                com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)home_guard.iterator().next());
                home_guard.remove(u_2);
                army.add(u_2);
            }
        }
        // Java: 466
        public int guardTarget()
        {
            float t = posture_since;
            return guard_target;
        }
        public int guard_target;
        // Java: 473
        public void setGameTime(float now)
        {
            int g = (now < @params.guard_mid_time ? @params.home_guard_early : (now < @params.guard_late_time ? @params.home_guard_mid : @params.home_guard_late));
            if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND))
            {
                g += @params.home_guard_defend_bonus;
            }
            if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
            {
                g = 2;
            }
            guard_target = g;
        }
        // Java: 484
        public void garrisonTowers(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, int[] home)
        {
            java.util.List stale = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(jobs.entries()))
            {
                if (((object)((com.oddlabs.tt.player.fable.Jobs.Job)e.getValue()).kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.GARRISON) && orders.usable(((com.oddlabs.tt.model.Unit)e.getKey())) && !((((com.oddlabs.tt.model.Unit)e.getKey()).getPrimaryController() is com.oddlabs.tt.model.behaviour.EnterController)))
                {
                    stale.add(((com.oddlabs.tt.model.Unit)e.getKey()));
                }
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(stale))
            {
                jobs.clear(u);
            }
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(roster.towers))
            {
                if (t.getUnitContainer() == null || t.getUnitContainer().getNumSupplies() > 0)
                {
                    continue;
                }
                java.util.List on_way = jobs.unitsOn(t);
                bool coming = false;
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(on_way))
                {
                    if (orders.usable(u_2) && ((object)jobs.kindOf(u_2) == (object)com.oddlabs.tt.player.fable.Jobs.Kind.GARRISON))
                    {
                        coming = true;
                        break;
                    }
                }
                if (coming)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit best = null;
                int best_score = java.lang.Integer.MIN_VALUE;
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(roster.warriors))
                {
                    if (!orders.usable(u_3) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u_3))
                    {
                        continue;
                    }
                    if (army.contains(u_3) && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING))
                    {
                        continue;
                    }
                    if (((object)jobs.kindOf(u_3) == (object)com.oddlabs.tt.player.fable.Jobs.Kind.GARRISON))
                    {
                        continue;
                    }
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(u_3.getGridX(), u_3.getGridY(), t.getGridX(), t.getGridY());
                    if (d2 > 60 * 60)
                    {
                        continue;
                    }
                    int material = (com.oddlabs.tt.player.fable.Roster.isRubber(u_3) ? 3 : (((object)u_3.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) ? 2 : 1));
                    int score = material * 100000 - d2;
                    if (score > best_score)
                    {
                        best_score = score;
                        best = u_3;
                    }
                }
                if (best != null)
                {
                    orders.enter(best, t);
                    jobs.set(best, com.oddlabs.tt.player.fable.Jobs.Kind.GARRISON, t.getGridX(), t.getGridY(), t, null, now, com.oddlabs.tt.player.fable.Military.TAG_GARRISON);
                    army.remove(best);
                    reinforcements.remove(best);
                    home_guard.remove(best);
                    com.oddlabs.tt.model.Unit chosen = best;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "garrison tower " + (t.getGridX()) + "," + (t.getGridY()) + " with " + java.lang.JStr.valueOf(((com.oddlabs.tt.player.fable.Roster.isRubber(chosen) ? "chicken" : "iron/rock"))) + " warrior";
                    }));
                }
            }
            if (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.NONE) && (float)(now - last_garrison_swap) > 10.0f && roster.rubber_warriors > 0)
            {
                foreach (com.oddlabs.tt.model.LandBuilding t_2 in java.lang.JIter.of(roster.towers))
                {
                    if ((t_2.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer m) && m.getUnit() != null && !com.oddlabs.tt.player.fable.Roster.isRubber(m.getUnit()) && t_2.canExitTower())
                    {
                        com.oddlabs.tt.model.Unit chicken = null;
                        foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(roster.warriors))
                        {
                            if (orders.usable(u_4) && com.oddlabs.tt.player.fable.Roster.isRubber(u_4) && ((object)jobs.kindOf(u_4) != (object)com.oddlabs.tt.player.fable.Jobs.Kind.GARRISON) && !(army.contains(u_4) && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING)) && com.oddlabs.tt.player.fable.BasePlan.dist2(u_4.getGridX(), u_4.getGridY(), t_2.getGridX(), t_2.getGridY()) <= 25 * 25)
                            {
                                chicken = u_4;
                                break;
                            }
                        }
                        if (chicken != null && intel.warriorsNear(t_2.getGridX(), t_2.getGridY(), 30) == 0)
                        {
                            orders.exitTower(t_2);
                            last_garrison_swap = now;
                            log.info("garrison swap: chicken warrior takes tower " + (t_2.getGridX()) + "," + (t_2.getGridY()));
                            break;
                        }
                    }
                }
            }
        }
        // Java: 564
        public void updatePosture(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, com.oddlabs.tt.player.fable.Economy economy, int[] home)
        {
            posture_since += 0.0f;
            com.oddlabs.tt.player.fable.Military.Posture next = posture;
            float dwell = (float)(now - posture_since);
            bool threatened = ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ALERT) || ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED);
            float A = (float)(groupValue(army) + groupValue(reinforcements));
            float H = groupValue(home_guard);
            float E_total = enemyTotalValue(intel);
            bool chief_ready = roster.chieftain != null && com.oddlabs.tt.player.fable.Chieftain.stunReady(roster.chieftain);
            bool enemy_chief = !intel.chieftains.isEmpty();
            com.oddlabs.tt.model.LandBuilding e_armory = (intel.armories.isEmpty() ? (com.oddlabs.tt.model.LandBuilding)(null) : ((com.oddlabs.tt.model.LandBuilding)intel.armories.getFirst()));
            int ex = (e_armory != null ? e_armory.getGridX() : plan.enemy_x);
            int ey = (e_armory != null ? e_armory.getGridY() : plan.enemy_y);
            float E_home = enemyValueNear(intel, ex, ey, 40);
            float k = (roster.chieftain == null ? @params.attack_ratio_no_chief : (!enemy_chief ? (float)(@params.attack_ratio * 0.8f) : @params.attack_ratio));
            if (chief_ready)
            {
                k = java.lang.Math.min(k, (float)(@params.attack_ratio_stun + 0.2f));
            }
            float march_s = (float)(com.oddlabs.tt.player.fable.BasePlan.dist((staging_x >= 0 ? staging_x : home[0]), (staging_y >= 0 ? staging_y : home[1]), ex, ey) / 2.0f);
            float margin = (float)(@params.attack_margin + (float)(@params.attack_margin_per_march_s * march_s));
            bool at_cap = roster.unitCount() >= @params.all_in_units;
            if (at_cap && cap_since < 0.0f)
            {
                cap_since = now;
            }
            else if (!at_cap)
            {
                cap_since = (float)(-1.0f);
            }
            float total_ratio = (at_cap && (float)(now - cap_since) > @params.cap_relax_time ? @params.capped_total_ratio_relaxed : @params.capped_total_ratio);
            float home_defence = (float)((float)(H + towersValue(roster)) + ((roster.chieftain != null && com.oddlabs.tt.player.fable.Chieftain.stunReady(roster.chieftain) && (((object)posture != (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING)) ? 6.0f : 0.0f)));
            bool serious = threatened && threat.approach_value >= 2.0f && (threat.approach_value >= (float)(0.7f * home_defence) || (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED) && threat.enemy_units_near_buildings >= 3 && threat.approach_value >= (float)(0.5f * home_defence)) || threat.enemy_units_near_buildings >= 6);
            threat_serious = serious;
            if (serious && ((object)posture != (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
            {
                int[] ac = centroid(army);
                bool far_out = ac != null && ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING) && com.oddlabs.tt.player.fable.BasePlan.dist(ac[0], ac[1], ex, ey) < com.oddlabs.tt.player.fable.BasePlan.dist(ac[0], ac[1], home[0], home[1]) && A >= (float)(1.3f * E_home) && threat.approach_value < A;
                bool far_fight = ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) && (((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT) || ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE) || ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_CYCLE)) && A >= (float)(1.5f * E_home);
                if (!far_fight && !far_out)
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.DEFEND;
                }
            }
            else if (serious && ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN) && threat.approach_value >= (float)(1.5f * home_defence) && roster.unitCount() < @params.all_in_units - 40)
            {
                next = com.oddlabs.tt.player.fable.Military.Posture.DEFEND;
            }
            else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND))
            {
                if (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.NONE) || (((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.WATCH) && threat.since_level_change > 20.0f))
                {
                    if (threat.recent_enemy_losses_near_base >= 6 && E_total <= (float)(0.7f * ((float)(A + H))) && (float)(A + H) >= @params.attack_min_army)
                    {
                        next = com.oddlabs.tt.player.fable.Military.Posture.ATTACK;
                    }
                    else
                    {
                        next = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                    }
                }
            }
            else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP))
            {
                bool cap = at_cap && A >= @params.attack_min_army && A >= (float)((float)(@params.capped_ratio * E_home) + margin) && A >= (float)(total_ratio * E_total);
                bool enemy_dead_armory = e_armory == null && intel.hasBuildings() && now > 300.0f;
                bool rested = (float)(now - last_retreat_at) >= @params.retreat_cooldown;
                bool all_in_odds = cap || (E_total <= (float)(0.5f * ((float)(A + H))) && now > 600.0f && A >= @params.attack_min_army) || (enemy_dead_armory && A >= 8.0f);
                bool attack_odds = A >= @params.attack_min_army && ((A >= @params.attack_max_army && A >= (float)(total_ratio * E_total) && A >= (float)(E_home + margin)) || (A >= (float)((float)(k * java.lang.Math.max(E_home, (float)(0.8f * E_total))) + margin) && ((float)(H + towersValue(roster)) >= (float)(0.8f * ((float)(E_total - E_home))) || (float)(E_total - E_home) < 4.0f)));
                attack_odds_ok = all_in_odds || attack_odds;
                if (rested && all_in_odds)
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.ALL_IN;
                }
                else if (rested && dwell >= @params.posture_dwell && now >= @params.attack_min_time && attack_odds)
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.ATTACK;
                }
            }
            else if ((((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN)) && ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING) && (float)(now - army_state_since) > 5.0f && (float)(now - posture_since) > 15.0f)
            {
                bool odds = (at_cap ? (A >= (float)((float)(@params.capped_ratio * E_home) + margin) && A >= (float)(total_ratio * E_total)) : A >= (float)((float)(k * java.lang.Math.max(E_home, (float)(0.8f * E_total))) + margin));
                bool weak_enemy = E_total <= (float)(0.5f * ((float)(A + H))) || (e_armory == null && intel.hasBuildings());
                if (!odds && !weak_enemy && A >= 4.0f)
                {
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("attack called off at staging: A=%.0f E_home=%.0f E_total=%.0f", A, E_home, E_total);
                    }));
                    next = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                }
                else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK))
                {
                    if ((roster.unitCount() >= @params.all_in_units && A >= @params.attack_min_army) || (e_armory == null && intel.hasBuildings() && now > 300.0f))
                    {
                        next = com.oddlabs.tt.player.fable.Military.Posture.ALL_IN;
                    }
                }
                else if (A < (float)(@params.attack_min_army * 0.5f))
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                }
            }
            else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK))
            {
                if ((roster.unitCount() >= @params.all_in_units && A >= @params.attack_min_army) || (e_armory == null && intel.hasBuildings() && now > 300.0f))
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.ALL_IN;
                }
                else if (A < 4.0f && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING))
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                }
            }
            else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
            {
                if (A < (float)(@params.attack_min_army * 0.5f) && ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING))
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                }
                else if (roster.unitCount() < 200 && e_armory != null && A < @params.attack_min_army)
                {
                    next = com.oddlabs.tt.player.fable.Military.Posture.ATTACK;
                }
            }
            if (((object)next != (object)posture))
            {
                com.oddlabs.tt.player.fable.Military.Posture from = posture;
                com.oddlabs.tt.player.fable.Military.Posture to = next;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "posture " + java.lang.JStr.valueOf(from) + " -> " + java.lang.JStr.valueOf(to) + " A=" + (java.lang.JC.f2i(A)) + " H=" + (java.lang.JC.f2i(H)) + " E_home=" + (java.lang.JC.f2i(E_home)) + " E_total=" + (java.lang.JC.f2i(E_total)) + " threat=" + java.lang.JStr.valueOf(threat);
                }));
                if (((object)next == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)next == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
                {
                    if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP) || ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND))
                    {
                        army.addAll(reinforcements);
                        reinforcements.clear();
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                    }
                }
                else if (((object)next == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND) || ((object)next == (object)com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP))
                {
                    army.addAll(reinforcements);
                    reinforcements.clear();
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                    if (((object)next == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND))
                    {
                        recallArmy(now, home);
                    }
                }
                posture = next;
                posture_since = now;
            }
            setGameTime(now);
        }
        // Java: 706
        public float towersValue(com.oddlabs.tt.player.fable.Roster roster)
        {
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(roster.towers))
            {
                v = (float)(v + towerValue(t));
            }
            return v;
        }
        // Java: 713
        public bool wantRockWarriors(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, com.oddlabs.tt.model.LandBuilding armory)
        {
            if (armory == null)
            {
                return false;
            }
            if (now < 420.0f && threat.enemy_peons_near_buildings >= 4 && com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) == 0)
            {
                return true;
            }
            return ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND) && ((object)threat.level == (object)com.oddlabs.tt.player.fable.Threat.Level.ENGAGED) && com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) == 0 && com.oddlabs.tt.player.fable.Economy.stock(armory, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) == 0;
        }
        // Java: 724
        public void setArmyState(float now, com.oddlabs.tt.player.fable.Military.ArmyState s)
        {
            if (((object)army_state != (object)s))
            {
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "army " + java.lang.JStr.valueOf(army_state) + " -> " + java.lang.JStr.valueOf(s) + " (" + (army.size()) + " units)";
                }));
                hold_since = (float)(-1.0f);
                hold_x = (-1);
                hold_y = (-1);
            }
            army_state = s;
            army_state_since = now;
        }
        // Java: 737
        public void recallArmy(float now, int[] home)
        {
            java.util.List all = new java.util.ArrayList(army);
            all.addAll(reinforcements);
            int[] p = defendCell(home);
            orders.move(all, p[0], p[1]);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(all))
            {
                jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, p[0], p[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
            }
            last_defend_x = (-1);
        }
        // Java: 747
        public int[] defendCell(int[] home)
        {
            return plan.along(home[0], home[1], @params.tower_ring_radius - 4);
        }
        public bool threat_serious;
        // Java: 753
        public java.util.List allFieldUnits()
        {
            java.util.List all = new java.util.ArrayList(home_guard);
            all.addAll(army);
            all.addAll(reinforcements);
            return all;
        }
        // Java: 760
        public void defend(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, int[] home, java.util.List defenders)
        {
            int px = default;
            int py = default;
            if (threat.approach_centroid != null)
            {
                int[] p = plan.toward(home[0], home[1], threat.approach_centroid[0], threat.approach_centroid[1], @params.tower_ring_radius - 4);
                px = p[0];
                py = p[1];
            }
            else
            {
                int[] p_2 = defendCell(home);
                px = p_2[0];
                py = p_2[1];
            }
            int[] ec = threat.approach_centroid;
            float ours = (ec == null ? ownValueNear(roster, px, py, 25) : ownValueNear(roster, ec[0], ec[1], 25));
            float theirs = (ec == null ? 0.0f : enemyValueNear(intel, ec[0], ec[1], 20));
            bool stunned_enemy = (float)(now - chief_brain.last_stun_at) < @params.stun_window && chief_brain.last_stun_cell != null && ec != null && com.oddlabs.tt.player.fable.BasePlan.dist2(chief_brain.last_stun_cell[0], chief_brain.last_stun_cell[1], ec[0], ec[1]) < 20 * 20;
            int[] target = new int[] { px, py };
            if (ec != null && (ours >= (float)(@params.engage_ratio * java.lang.Math.max(0.5f, theirs)) || stunned_enemy))
            {
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(home[0], home[1], ec[0], ec[1]);
                if (d <= (float)(@params.tower_ring_radius + @params.defend_pursuit))
                {
                    target = ec;
                }
            }
            if (((object)target == (object)ec))
            {
                micro.fight(now, defenders, intel, ec[0], ec[1], false, true, null, com.oddlabs.tt.player.fable.Military.TAG_HOME);
                last_defend_order = now;
                last_defend_x = ec[0];
                last_defend_y = ec[1];
                return;
            }
            micro.fight(now, defenders, intel, px, py, false, false, null, com.oddlabs.tt.player.fable.Military.TAG_HOME);
            bool changed = com.oddlabs.tt.player.fable.BasePlan.dist2(target[0], target[1], last_defend_x, last_defend_y) > 36 || (float)(now - last_defend_order) > 8.0f;
            if (!changed)
            {
                return;
            }
            java.util.List to_order = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(defenders))
            {
                if (!orders.usable(u) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                {
                    continue;
                }
                if ((u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour))
                {
                    continue;
                }
                to_order.add(u);
            }
            if (!to_order.isEmpty())
            {
                orders.defend(to_order, target[0], target[1]);
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(to_order))
                {
                    jobs.set(u_2, com.oddlabs.tt.player.fable.Jobs.Kind.DEFEND, target[0], target[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_HOME);
                }
            }
            last_defend_order = now;
            last_defend_x = target[0];
            last_defend_y = target[1];
        }
        // Java: 821
        public void homeGuardStep(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] home)
        {
            int[] g = defendCell(home);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(home_guard))
            {
                if (!orders.usable(u) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                {
                    continue;
                }
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                bool idle = ((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE);
                if (j == null || ((object)j.kind != (object)com.oddlabs.tt.player.fable.Jobs.Kind.DEFEND) || (idle && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), g[0], g[1]) > 64))
                {
                    orders.defend(java.util.List_.of(u), g[0], g[1]);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.DEFEND, g[0], g[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_HOME);
                }
            }
            int[] c = intel.armyCentroid(home[0], home[1], 45);
            if (c != null && !home_guard.isEmpty())
            {
                float theirs = enemyValueNear(intel, c[0], c[1], 15);
                float ours = (float)(groupValue(home_guard) + ownTowerCover(roster, c));
                if (theirs > 0.0f && ours >= (float)(@params.engage_ratio * theirs))
                {
                    micro.fight(now, home_guard, intel, c[0], c[1], false, true, null, com.oddlabs.tt.player.fable.Military.TAG_HOME);
                    last_defend_order = now;
                }
            }
        }
        // Java: 850
        public bool fieldSweep(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] home)
        {
            if (army.size() < 6 || !@params.field_sweep)
            {
                return false;
            }
            int r = @params.sweep_radius;
            int[] target = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            bool ongoing = sweep_x >= 0;
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.warriors))
            {
                if (e.isDead() || com.oddlabs.tt.player.fable.Chieftain.isFrozen(e))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(e.getGridX(), e.getGridY(), home[0], home[1]);
                if (d2 > r * r)
                {
                    continue;
                }
                if (ongoing)
                {
                    int ds = com.oddlabs.tt.player.fable.BasePlan.dist2(e.getGridX(), e.getGridY(), sweep_x, sweep_y);
                    if (ds <= 25 * 25 && ds < best_d2)
                    {
                        best_d2 = ds;
                        target = new int[] { e.getGridX(), e.getGridY() };
                    }
                    continue;
                }
                bool near_peon = false;
                foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
                {
                    com.oddlabs.tt.player.fable.Roster.Activity a = com.oddlabs.tt.player.fable.Roster.activity(p);
                    if ((((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_TREE) || ((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_IRON) || ((object)a == (object)com.oddlabs.tt.player.fable.Roster.Activity.GATHER_ROCK)) && com.oddlabs.tt.player.fable.BasePlan.dist2(p.getGridX(), p.getGridY(), e.getGridX(), e.getGridY()) <= 20 * 20)
                    {
                        near_peon = true;
                        break;
                    }
                }
                if (!near_peon && d2 > 45 * 45)
                {
                    continue;
                }
                if (d2 > 45 * 45 && now < sweep_cooldown_until)
                {
                    continue;
                }
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    target = new int[] { e.getGridX(), e.getGridY() };
                }
            }
            com.oddlabs.tt.model.LandBuilding building = null;
            if (target == null)
            {
                int best_b = java.lang.Integer.MAX_VALUE;
                java.util.List cands = new java.util.ArrayList(intel.towers);
                cands.addAll(intel.construction_sites);
                foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(cands))
                {
                    if (t.isDead())
                    {
                        continue;
                    }
                    int d2_2 = com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), home[0], home[1]);
                    if (d2_2 > r * r)
                    {
                        continue;
                    }
                    bool ours_side = com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), plan.start_x, plan.start_y) < com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), plan.enemy_x, plan.enemy_y);
                    if (!ours_side)
                    {
                        continue;
                    }
                    if (d2_2 < best_b)
                    {
                        best_b = d2_2;
                        building = t;
                    }
                }
                if (building != null)
                {
                    target = new int[] { building.getGridX(), building.getGridY() };
                }
            }
            if (target == null)
            {
                if (sweep_x >= 0)
                {
                    log.info("field sweep done");
                    sweep_x = (-1);
                    sweep_y = (-1);
                    sweep_cooldown_until = (float)(now + 12.0f);
                }
                return false;
            }
            int[] ec = (building != null ? target : intel.armyCentroid(target[0], target[1], 15));
            if (ec == null)
            {
                return false;
            }
            float theirs = enemyValueNear(intel, ec[0], ec[1], 15);
            float ours = groupValue(army);
            bool covered = building == null && coveringTower(intel, ec[0], ec[1], ec) != null;
            if ((theirs <= 0.0f && building == null) || ours < (float)(@params.engage_ratio * theirs) || covered)
            {
                if (sweep_x >= 0)
                {
                    log.info("field sweep called off");
                    sweep_x = (-1);
                    sweep_y = (-1);
                    sweep_cooldown_until = (float)(now + 12.0f);
                }
                return false;
            }
            if (sweep_x < 0)
            {
                float fo = ours;
                float ft = theirs;
                string what = (building != null ? "tower/foundation" : "enemy");
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("field sweep: %s %.0f at %d,%d vs army %.0f", what, ft, ec[0], ec[1], fo);
                }));
                sweep_since = now;
            }
            sweep_x = ec[0];
            sweep_y = ec[1];
            micro.fight(now, army, intel, ec[0], ec[1], building != null, true, building, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
            return true;
        }
        // Java: 955
        public float ownTowerCover(com.oddlabs.tt.player.fable.Roster roster, int[] c)
        {
            float v = 0.0f;
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(roster.towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), c[0], c[1]) <= @params.tower_reach2)
                {
                    v = (float)(v + towerValue(t));
                }
            }
            return v;
        }
        public int sweep_x;
        public int sweep_y;
        public float sweep_since;
        public float sweep_cooldown_until;
        // Java: 969
        public void holdHome(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] home)
        {
            homeGuardStep(now, roster, intel, home);
            if (fieldSweep(now, roster, intel, home))
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (!orders.usable(u) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                {
                    continue;
                }
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                bool idle = ((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE);
                bool far = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) > 100;
                if (j == null || ((object)j.kind != (object)com.oddlabs.tt.player.fable.Jobs.Kind.HOLD) || (idle && far))
                {
                    orders.attackMove(u, staging_x, staging_y);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.HOLD, staging_x, staging_y, null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                }
            }
            int[] c = intel.armyCentroid(home[0], home[1], 45);
            if (c != null)
            {
                float theirs = enemyValueNear(intel, c[0], c[1], 15);
                float ours = (float)((float)(groupValue(home_guard) + groupValue(army)) + ownTowerCover(roster, c));
                if (theirs > 0.0f && ours >= (float)(@params.engage_ratio * theirs))
                {
                    java.util.List to = new java.util.ArrayList(home_guard);
                    to.addAll(army);
                    micro.fight(now, to, intel, c[0], c[1], false, true, null, com.oddlabs.tt.player.fable.Military.TAG_HOME);
                    last_defend_order = now;
                }
            }
        }
        // Java: 1002
        public void armyStep(float now, float dt, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, int[] home)
        {
            int[] c = centroid(army);
            if (c == null)
            {
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                return;
            }
            chooseObjective(now, intel, roster, c);
            if (objective_x < 0)
            {
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                return;
            }
            switch ((army_state).ordinal())
            {
                case 0 /*STAGING*/:
                {
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
                    {
                        if (!orders.usable(u))
                        {
                            continue;
                        }
                        com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                        if (j == null || ((object)j.kind != (object)com.oddlabs.tt.player.fable.Jobs.Kind.HOLD) || (((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), staging_x, staging_y) > 144))
                        {
                            orders.attackMove(u, staging_x, staging_y);
                            jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.HOLD, staging_x, staging_y, null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                        }
                    }
                    int near = 0;
                    foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
                    {
                        if (com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), staging_x, staging_y) <= 144)
                        {
                            near++;
                        }
                    }
                    bool chief_ok = roster.chieftain == null || com.oddlabs.tt.player.fable.BasePlan.dist2(roster.chieftain.getGridX(), roster.chieftain.getGridY(), staging_x, staging_y) <= 400 || (float)(now - army_state_since) > 40.0f;
                    if (army.size() >= 3 && (near >= (float)(@params.leg_arrival_fraction * army.size()) || (float)(now - army_state_since) > 30.0f) && chief_ok)
                    {
                        leg_x = (-1);
                        leg_y = (-1);
                        leg_skips = 0;
                        nextLeg(now, c);
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.MARCH);
                    }
                    break;
                }
                case 1 /*MARCH*/:
                {
                    march(now, roster, intel, c);
                    break;
                }
                case 2 /*ENGAGE*/:
                {
                    engage(now, roster, intel, c);
                    break;
                }
                case 3 /*ASSAULT*/:
                {
                    assault(now, roster, intel, c);
                    break;
                }
                case 4 /*SIEGE_PREP*/:
                {
                    siegePrep(now, roster, intel, c);
                    break;
                }
                case 5 /*SIEGE_CYCLE*/:
                {
                    siegeCycle(now, roster, intel, c);
                    break;
                }
                case 6 /*RETREAT*/:
                {
                    retreat(now, roster, intel, c, home);
                    break;
                }
            }
        }
        // Java: 1053
        public void chooseObjective(float now, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Roster roster, int[] c)
        {
            com.oddlabs.tt.model.Selectable target = null;
            foreach (com.oddlabs.tt.model.LandBuilding s in java.lang.JIter.of(intel.construction_sites))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(s.getGridX(), s.getGridY(), c[0], c[1]) <= 30 * 30)
                {
                    target = s;
                    break;
                }
            }
            if (target == null && !intel.armories.isEmpty())
            {
                target = ((com.oddlabs.tt.model.LandBuilding)com.oddlabs.tt.player.fable.Intel.nearest(intel.armories, c[0], c[1]));
            }
            if (target == null && !intel.quarters.isEmpty())
            {
                target = ((com.oddlabs.tt.model.LandBuilding)com.oddlabs.tt.player.fable.Intel.nearest(intel.quarters, c[0], c[1]));
            }
            if (target == null && !intel.towers.isEmpty())
            {
                target = ((com.oddlabs.tt.model.LandBuilding)com.oddlabs.tt.player.fable.Intel.nearest(intel.towers, c[0], c[1]));
            }
            if (target == null)
            {
                if ((objective is com.oddlabs.tt.model.Unit ou) && !ou.isDead() && !ou.isMounted() && com.oddlabs.tt.player.fable.BasePlan.dist2(ou.getGridX(), ou.getGridY(), objective_x, objective_y) <= 400)
                {
                    objective_x = ou.getGridX();
                    objective_y = ou.getGridY();
                    return;
                }
                if (!intel.warriors.isEmpty())
                {
                    target = ((com.oddlabs.tt.model.Unit)com.oddlabs.tt.player.fable.Intel.nearest(intel.warriors, c[0], c[1]));
                }
                else if (!intel.peons.isEmpty())
                {
                    target = ((com.oddlabs.tt.model.Unit)com.oddlabs.tt.player.fable.Intel.nearest(intel.peons, c[0], c[1]));
                }
                else if (!intel.chieftains.isEmpty())
                {
                    target = ((com.oddlabs.tt.model.Unit)intel.chieftains.getFirst());
                }
            }
            if (target == null)
            {
                objective = null;
                objective_x = (-1);
                objective_y = (-1);
                return;
            }
            if (((object)objective != (object)target))
            {
                com.oddlabs.tt.model.Selectable t = target;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "objective: " + java.lang.JStr.valueOf((((t is com.oddlabs.tt.model.LandBuilding) ? java.lang.JO.toString(t) : "unit"))) + " at " + (t.getGridX()) + "," + (t.getGridY());
                }));
            }
            objective = target;
            objective_x = target.getGridX();
            objective_y = target.getGridY();
        }
        // Java: 1102
        public void nextLeg(float now, int[] c)
        {
            int from_x = (leg_x >= 0 ? leg_x : c[0]);
            int from_y = (leg_y >= 0 ? leg_y : c[1]);
            if (leg_x >= 0 && com.oddlabs.tt.player.fable.BasePlan.dist(c[0], c[1], leg_x, leg_y) > @params.leg_length + 15)
            {
                from_x = c[0];
                from_y = c[1];
            }
            float d = com.oddlabs.tt.player.fable.BasePlan.dist(from_x, from_y, objective_x, objective_y);
            int[] p = (d <= @params.leg_length + 4 ? new int[] { objective_x, objective_y } : plan.toward(from_x, from_y, objective_x, objective_y, @params.leg_length));
            if (!plan.sameIsland(p[0], p[1]) || grid.getRegion(p[0], p[1]) == null)
            {
                p = plan.toward(from_x, from_y, objective_x, objective_y, (float)(@params.leg_length / 2.0f));
                if (!plan.sameIsland(p[0], p[1]) || grid.getRegion(p[0], p[1]) == null)
                {
                    p = new int[] { objective_x, objective_y };
                }
            }
            prev_leg_x = (leg_x >= 0 ? leg_x : c[0]);
            prev_leg_y = (leg_y >= 0 ? leg_y : c[1]);
            leg_x = p[0];
            leg_y = p[1];
            leg_ordered_at = now;
            orderLeg(now);
        }
        // Java: 1127
        public void orderLeg(float now)
        {
            java.util.List to = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (orders.usable(u) && !com.oddlabs.tt.player.fable.Chieftain.isStunned(u) && !((u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour)) && !((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.HuntController)))
                {
                    to.add(u);
                }
            }
            orders.attackMove(to, leg_x, leg_y);
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(to))
            {
                jobs.set(u_2, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, leg_x, leg_y, null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
            }
        }
        // Java: 1139
        public int arrivalRadius()
        {
            return java.lang.Math.max(@params.leg_arrival_radius, java.lang.JC.d2i((1.6d * java.lang.Math.sqrt(army.size()))) + 4);
        }
        // Java: 1147
        public void dropStragglers(float now, int[] c)
        {
            if (leg_x < 0 || army.size() < 8)
            {
                return;
            }
            java.util.List behind = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (!orders.usable(u))
                {
                    continue;
                }
                float d = com.oddlabs.tt.player.fable.BasePlan.dist(u.getGridX(), u.getGridY(), c[0], c[1]);
                if (d > 50.0f)
                {
                    behind.add(u);
                }
            }
            int max = java.lang.Math.max(1, army.size() / 5);
            if (behind.size() > max)
            {
                behind = behind.subList(0, max);
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(behind))
            {
                army.remove(u_2);
                reinforcements.add(u_2);
                orders.attackMove(u_2, c[0], c[1]);
                jobs.set(u_2, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, c[0], c[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_REINF);
            }
        }
        // Java: 1175
        public bool contact(com.oddlabs.tt.player.fable.Intel intel, int[] c, float ours_all)
        {
            float theirs = enemyValueNear(intel, c[0], c[1], 20);
            int near_enemies = intel.warriorsNear(c[0], c[1], 12);
            bool trickle = theirs < 8.0f && theirs < (float)(0.12f * ours_all) && near_enemies < 4;
            return (near_enemies > 0 || theirs >= 3.0f) && !trickle;
        }
        // Java: 1182
        public void march(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c)
        {
            int[] ec0 = intel.armyCentroid(c[0], c[1], 30);
            if (ec0 != null && enemyChiefResponse(now, roster, intel, c, ec0))
            {
                return;
            }
            float theirs = enemyValueNear(intel, c[0], c[1], 20);
            float ours_all = groupValue(army);
            if (contact(intel, c, ours_all))
            {
                engage_start_value = theirs;
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE);
                return;
            }
            if (theirs > 0.0f && leg_x >= 0)
            {
                micro.fight(now, army, intel, leg_x, leg_y, false, false, null, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
            }
            com.oddlabs.tt.model.Unit echief = com.oddlabs.tt.player.fable.Chieftain.nearestEnemyChief(intel, c[0], c[1]);
            com.oddlabs.tt.model.Unit chief = roster.chieftain;
            if (echief != null && chief != null && orders.usable(chief) && com.oddlabs.tt.player.fable.Chieftain.stunReady(chief) && !com.oddlabs.tt.player.fable.Chieftain.isFrozen(echief) && com.oddlabs.tt.player.fable.Chieftain.stunProgress(echief) >= 0.6f && com.oddlabs.tt.player.fable.BasePlan.dist2(echief.getGridX(), echief.getGridY(), c[0], c[1]) <= 32 * 32 && com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), c[0], c[1]) > 12 * 12)
            {
                if (chief_wait_since < 0.0f)
                {
                    chief_wait_since = now;
                    log.info("march: armed enemy chieftain ahead, waiting for ours to close up");
                }
                if ((float)(now - chief_wait_since) < 12.0f)
                {
                    leg_ordered_at = now;
                    return;
                }
            }
            else
            {
                chief_wait_since = (float)(-1.0f);
            }
            com.oddlabs.tt.model.LandBuilding creep = coveringTower(intel, c[0], c[1], c);
            float d_obj = com.oddlabs.tt.player.fable.BasePlan.dist(c[0], c[1], objective_x, objective_y);
            if (d_obj <= 26.0f || creep != null)
            {
                com.oddlabs.tt.model.LandBuilding covering = (d_obj <= 26.0f ? coveringTower(intel, objective_x, objective_y, c) : creep);
                int tx = (covering != null ? covering.getGridX() : objective_x);
                int ty = (covering != null ? covering.getGridY() : objective_y);
                float t_val = enemyValueNear(intel, tx, ty, 20);
                float ours = ours_all;
                float crush = (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN) ? 1.6f : 2.5f);
                if (covering != null && d_obj > 26.0f)
                {
                    objective = covering;
                    objective_x = tx;
                    objective_y = ty;
                }
                int corps_near = corpsNear(tx, ty, 45);
                bool crushing = ours >= (float)(crush * java.lang.Math.max(1.0f, t_val)) && corps_near >= @params.corps_min_demolish && ours >= (float)(3.0f * java.lang.Math.max(1.0f, t_val));
                if (covering != null && !crushing)
                {
                    bool chief_ok = roster.chieftain != null && orders.usable(roster.chieftain) && com.oddlabs.tt.player.fable.Chieftain.stunProgress(roster.chieftain) >= 0.5f;
                    if (chief_ok || corps_near >= @params.corps_min_demolish)
                    {
                        siege_tower = covering;
                        siege_prep_since = now;
                        stand_x = (-1);
                        stand_fails = 0;
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP);
                        return;
                    }
                    if (ours < (float)(2.5f * java.lang.Math.max(1.0f, t_val)))
                    {
                        holdAt(now, c, "waiting for strength vs towers");
                        return;
                    }
                }
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT);
                return;
            }
            dropStragglers(now, c);
            int r = arrivalRadius();
            int arrived = 0;
            int within_leg = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), leg_x, leg_y);
                if (d2 <= r * r)
                {
                    arrived++;
                }
                if (d2 <= (r + @params.leg_length) * (r + @params.leg_length))
                {
                    within_leg++;
                }
            }
            bool timeout = (float)(now - leg_ordered_at) > @params.leg_timeout;
            bool gate = arrived >= (float)(@params.leg_arrival_fraction * army.size()) && within_leg >= (float)(0.95f * army.size());
            int en_route = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(reinforcements))
            {
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u_2);
                if (j != null && ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) && j.tag == com.oddlabs.tt.player.fable.Military.TAG_REINF && com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), c[0], c[1]) <= 60 * 60)
                {
                    en_route++;
                }
            }
            if (en_route >= java.lang.Math.max(4, java.lang.JC.f2i(((float)(0.25f * army.size())))) && gate)
            {
                if (reinf_wait_since < 0.0f)
                {
                    reinf_wait_since = now;
                    int n = en_route;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "march: waiting for " + (n) + " reinforcements";
                    }));
                }
                if ((float)(now - reinf_wait_since) < 30.0f)
                {
                    leg_ordered_at = now;
                    return;
                }
            }
            else
            {
                reinf_wait_since = (float)(-1.0f);
            }
            if (gate || timeout)
            {
                if (timeout && arrived < (float)(0.3f * army.size()))
                {
                    leg_skips++;
                    if (leg_skips >= 3 && arrived < (float)(0.1f * army.size()))
                    {
                        leg_x = objective_x;
                        leg_y = objective_y;
                        leg_ordered_at = now;
                        orderLeg(now);
                        return;
                    }
                    leg_x = (-1);
                    leg_y = (-1);
                }
                else
                {
                    leg_skips = 0;
                }
                nextLeg(now, c);
            }
            else if ((float)(now - leg_ordered_at) > 6.0f)
            {
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(army))
                {
                    if (!orders.usable(u_3) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u_3))
                    {
                        continue;
                    }
                    if (((object)com.oddlabs.tt.player.fable.Roster.activity(u_3) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) && com.oddlabs.tt.player.fable.BasePlan.dist2(u_3.getGridX(), u_3.getGridY(), leg_x, leg_y) > r * r)
                    {
                        orders.attackMove(u_3, leg_x, leg_y);
                        jobs.set(u_3, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, leg_x, leg_y, null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                    }
                }
            }
        }
        // Java: 1326
        public void holdAt(float now, int[] c, string why)
        {
            if (hold_since < 0.0f)
            {
                hold_since = now;
                log.info("army holds: " + java.lang.JStr.valueOf(why));
            }
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (!orders.usable(u) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                {
                    continue;
                }
                if (((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), c[0], c[1]) > 100)
                {
                    orders.attackMove(u, c[0], c[1]);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.HOLD, c[0], c[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                }
            }
            if ((float)(now - hold_since) > 90.0f)
            {
                hold_since = (float)(-1.0f);
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
            }
        }
        // Java: 1348
        public com.oddlabs.tt.model.LandBuilding coveringTower(com.oddlabs.tt.player.fable.Intel intel, int gx, int gy, int[] c)
        {
            com.oddlabs.tt.model.LandBuilding best = null;
            int best_d2 = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
            {
                if (com.oddlabs.tt.player.fable.Chieftain.towerStunned(t))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), gx, gy) <= (@params.tower_reach2 + 100))
                {
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), c[0], c[1]);
                    if (d2 < best_d2)
                    {
                        best_d2 = d2;
                        best = t;
                    }
                }
            }
            return best;
        }
        public float last_engage_log;
        public float last_fight_theirs;
        public float last_retreat_at;
        public float cap_since;
        public float reinf_wait_since;
        public float chief_wait_since;
        public bool attack_odds_ok;
        public float last_siege_log;
        public int fight_x;
        public int fight_y;
        public float fight_at;
        // Java: 1384
        public float[] sidesAt(com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c, int[] ec)
        {
            int mx = (c[0] + ec[0]) / 2;
            int my = (c[1] + ec[1]) / 2;
            float ours = ownValueNear(roster, mx, my, 25);
            float theirs = java.lang.Math.max(enemyValueNear(intel, ec[0], ec[1], 20), enemyValueNear(intel, mx, my, 25));
            return new float[] { ours, theirs };
        }
        // Java: 1393
        public void engage(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c)
        {
            int[] ec = intel.armyCentroid(c[0], c[1], 25);
            int present = (ec == null ? 0 : intel.warriorsNear(ec[0], ec[1], 20) + intel.unitsNear(ec[0], ec[1], 12));
            float ours = 0.0f;
            float theirs = 0.0f;
            if (ec != null)
            {
                float[] s = sidesAt(roster, intel, c, ec);
                ours = s[0];
                theirs = s[1];
                last_fight_theirs = theirs;
                fight_x = ec[0];
                fight_y = ec[1];
                fight_at = now;
            }
            bool stun_landing = (float)(now - chief_brain.last_stun_at) < @params.stun_window;
            if (stun_landing)
            {
                theirs = (float)(theirs * 0.35f);
            }
            if (ec != null && enemyChiefResponse(now, roster, intel, c, ec))
            {
                return;
            }
            if ((float)(now - last_engage_log) >= 4.0f && ec != null)
            {
                last_engage_log = now;
                int stunned_ours = 0;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
                {
                    if (com.oddlabs.tt.player.fable.Chieftain.isFrozen(u))
                    {
                        stunned_ours++;
                    }
                }
                com.oddlabs.tt.model.Unit echief = com.oddlabs.tt.player.fable.Chieftain.nearestEnemyChief(intel, ec[0], ec[1]);
                com.oddlabs.tt.model.Unit chief = roster.chieftain;
                float fo = ours;
                float ft = theirs;
                int so = stunned_ours;
                int[] fec = ec;
                string ech = (echief == null ? "none" : (java.lang.JC.f2i(com.oddlabs.tt.player.fable.BasePlan.dist(echief.getGridX(), echief.getGridY(), ec[0], ec[1]))) + "c/" + (java.lang.JC.f2i(((float)(100 * com.oddlabs.tt.player.fable.Chieftain.stunProgress(echief))))) + "%" + java.lang.JStr.valueOf(((com.oddlabs.tt.player.fable.Chieftain.isCasting(echief) ? " CASTING" : ""))));
                string och = (chief == null || chief.isDead() ? "none" : (java.lang.JC.f2i(com.oddlabs.tt.player.fable.BasePlan.dist(chief.getGridX(), chief.getGridY(), ec[0], ec[1]))) + "c/" + (java.lang.JC.f2i(((float)(100 * com.oddlabs.tt.player.fable.Chieftain.stunProgress(chief))))) + "%");
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("engage: ours=%.1f theirs=%.1f R=%.2f army=%d stunned=%d echief=%s chief=%s ours@fp=%d theirs@fp=%d %s", fo, ft, (float)(fo / java.lang.Math.max(0.1f, ft)), army.size(), so, ech, och, com.oddlabs.tt.player.fable.Chieftain.ownWarriorsNear(roster.warriors, fec[0], fec[1], 20), intel.warriorsNear(fec[0], fec[1], 20), micro);
                }));
            }
            if ((ec == null || present == 0 || !contact(intel, c, groupValue(army))) && (float)(now - army_state_since) > 3.0f)
            {
                hold_since = (float)(-1.0f);
                leg_x = (-1);
                leg_y = (-1);
                nextLeg(now, c);
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.MARCH);
                return;
            }
            if (ec == null)
            {
                return;
            }
            float R = (float)(ours / java.lang.Math.max(0.1f, theirs));
            if (R < @params.retreat_ratio && !stun_landing)
            {
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                return;
            }
            com.oddlabs.tt.model.LandBuilding cover = coveringTower(intel, ec[0], ec[1], c);
            if (cover != null && !stun_landing && !com.oddlabs.tt.player.fable.Chieftain.towerStunned(cover))
            {
                float t_val = enemyValueNear(intel, cover.getGridX(), cover.getGridY(), 20);
                if (ours >= (float)(2.5f * java.lang.Math.max(1.0f, t_val)))
                {
                    objective = cover;
                    objective_x = cover.getGridX();
                    objective_y = cover.getGridY();
                    log.info("engage: under a tower but crushing: assault it");
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT);
                }
                else
                {
                    siege_tower = cover;
                    siege_prep_since = now;
                    stand_x = (-1);
                    stand_fails = 0;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "engage: under tower cover at " + (cover.getGridX()) + "," + (cover.getGridY()) + ": pulling out for the stun cycle";
                    }));
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP);
                }
                return;
            }
            if (R >= @params.engage_ratio || stun_landing)
            {
                hold_since = (float)(-1.0f);
                micro.fight(now, army, intel, ec[0], ec[1], false, true, objective, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                last_engage_order = now;
                last_engage_x = ec[0];
                last_engage_y = ec[1];
            }
            else
            {
                if (hold_since < 0.0f)
                {
                    hold_since = now;
                }
                micro.fight(now, army, intel, c[0], c[1], false, false, null, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                if ((float)(now - hold_since) > 45.0f && R < 1.0f)
                {
                    hold_since = (float)(-1.0f);
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                }
            }
        }
        public float last_scatter;
        public float last_snipe;
        // Java: 1502
        public bool enemyChiefResponse(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c, int[] ec)
        {
            com.oddlabs.tt.model.Unit echief = com.oddlabs.tt.player.fable.Chieftain.nearestEnemyChief(intel, c[0], c[1]);
            if (echief == null || com.oddlabs.tt.player.fable.Chieftain.isStunned(echief))
            {
                return false;
            }
            int ex = echief.getGridX();
            int ey = echief.getGridY();
            float d_army = com.oddlabs.tt.player.fable.BasePlan.dist(ex, ey, c[0], c[1]);
            bool casting = com.oddlabs.tt.player.fable.Chieftain.isCasting(echief);
            bool armed = com.oddlabs.tt.player.fable.Chieftain.stunProgress(echief) >= @params.counter_toot_progress;
            if (casting && d_army <= 26.0f && (float)(now - last_scatter) > 5.0f)
            {
                last_scatter = now;
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
                {
                    if (!orders.usable(u) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                    {
                        continue;
                    }
                    int dx = u.getGridX() - ex;
                    int dy = u.getGridY() - ey;
                    float d = ((float)(java.lang.Math.sqrt(dx * dx + dy * dy)));
                    if (d > 19.5f || d < 14.0f)
                    {
                        continue;
                    }
                    int tx = java.lang.Math.round((float)(ex + (float)((float)(dx / d) * 23.0f)));
                    int ty = java.lang.Math.round((float)(ey + (float)((float)(dy / d) * 23.0f)));
                    orders.move(u, tx, ty);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, tx, ty, null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                    n++;
                }
                int fn = n;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "enemy chieftain casting: scattering " + (fn) + " outer units";
                }));
                return n > 0;
            }
            if (d_army > 26.0f)
            {
                return false;
            }
            int ours_near = com.oddlabs.tt.player.fable.Chieftain.ownWarriorsNear(roster.warriors, ex, ey, 12);
            float theirs_near = enemyValueNear(intel, ex, ey, 12);
            float ours_val = ownValueNear(roster, ex, ey, 14);
            bool covered = coveringTower(intel, ex, ey, c) != null;
            int need = (armed ? 8 : 10);
            float edge = (armed ? 1.3f : 1.6f);
            if (ours_near >= need && ours_val >= (float)(edge * java.lang.Math.max(1.0f, theirs_near)) && !covered && (float)(now - last_snipe) > 6.0f)
            {
                last_snipe = now;
                java.util.List hunters = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
                {
                    if (orders.usable(u_2) && !com.oddlabs.tt.player.fable.Chieftain.isStunned(u_2) && com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), ex, ey) <= 16 * 16)
                    {
                        hunters.add(u_2);
                    }
                }
                if (!hunters.isEmpty())
                {
                    orders.attack(hunters, echief);
                    foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(hunters))
                    {
                        jobs.set(u_3, com.oddlabs.tt.player.fable.Jobs.Kind.HUNT, ex, ey, echief, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                    }
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return "sniping the enemy chieftain with " + (hunters.size()) + " warriors";
                    }));
                    return true;
                }
            }
            return false;
        }
        // Java: 1561
        public int corpsNear(int gx, int gy, int radius)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(corps))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(p.getGridX(), p.getGridY(), gx, gy) <= radius * radius)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1569
        public void assault(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c)
        {
            if (objective == null || objective.isDead())
            {
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.MARCH);
                nextLeg(now, c);
                return;
            }
            com.oddlabs.tt.model.LandBuilding cover = coveringTower(intel, objective_x, objective_y, c);
            bool stun_on = (float)(now - chief_brain.last_stun_at) < @params.stun_window;
            if (cover != null && !stun_on && ((object)corps_target != (object)cover) && groupValue(army) < (float)(3.0f * java.lang.Math.max(1.0f, enemyValueNear(intel, cover.getGridX(), cover.getGridY(), 20))))
            {
                siege_tower = cover;
                siege_prep_since = now;
                stand_x = (-1);
                stand_fails = 0;
                log.info("assault: a manned tower covers the objective, back to the siege");
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP);
                return;
            }
            int[] ec = intel.armyCentroid(c[0], c[1], 25);
            float theirs = enemyValueNear(intel, c[0], c[1], 20);
            float ours = ownValueNear(roster, c[0], c[1], 20);
            if (ec != null)
            {
                float[] s = sidesAt(roster, intel, c, ec);
                ours = s[0];
                theirs = java.lang.Math.max(theirs, s[1]);
                last_fight_theirs = theirs;
            }
            fight_x = objective_x;
            fight_y = objective_y;
            fight_at = now;
            bool stun_landing = (float)(now - chief_brain.last_stun_at) < @params.stun_window;
            if (!stun_landing && theirs > 0.0f && (float)(ours / theirs) < @params.retreat_ratio)
            {
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                return;
            }
            int near_enemies = intel.warriorsNear(c[0], c[1], 12);
            if (near_enemies >= 3 && (float)(ours / java.lang.Math.max(0.1f, theirs)) < @params.engage_ratio && !stun_landing)
            {
                engage_start_value = theirs;
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE);
                return;
            }
            if (ec != null && enemyChiefResponse(now, roster, intel, c, ec))
            {
                return;
            }
            com.oddlabs.tt.model.LandBuilding demolish = null;
            int best_d2 = 24 * 24;
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.towers))
            {
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), objective_x, objective_y);
                if (d2 < best_d2)
                {
                    best_d2 = d2;
                    demolish = t;
                }
            }
            if (demolish == null && (objective is com.oddlabs.tt.model.LandBuilding ob) && !ob.isDead())
            {
                demolish = ob;
            }
            if (demolish != null)
            {
                corpsDemolish(now, demolish, c);
            }
            micro.fight(now, army, intel, objective_x, objective_y, true, true, objective, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
            last_engage_order = now;
            last_engage_x = objective_x;
            last_engage_y = objective_y;
        }
        public int stand_fails;
        public float stand_since;
        public int stand_best_d2;
        public bool stand_pre_reached;
        public java.util.List stand_blacklist;
        // Java: 1643
        public void siegePrep(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c)
        {
            com.oddlabs.tt.model.Unit chief = roster.chieftain;
            if (siege_tower == null || siege_tower.isDead() || !intel.manned_towers.contains(siege_tower))
            {
                siege_tower = null;
                stand_x = (-1);
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.MARCH);
                nextLeg(now, c);
                return;
            }
            float t_val = enemyValueNear(intel, siege_tower.getGridX(), siege_tower.getGridY(), 20);
            float ours = groupValue(army);
            if ((float)(now - chief_brain.last_stun_at) < 4.0f && chief_brain.last_stun_cell != null && com.oddlabs.tt.player.fable.BasePlan.dist2(chief_brain.last_stun_cell[0], chief_brain.last_stun_cell[1], siege_tower.getGridX(), siege_tower.getGridY()) <= 20 * 20)
            {
                siege_cast_at = chief_brain.last_stun_at;
                stand_blacklist.clear();
                stand_fails = 0;
                log.info("siege: using the stun that just landed");
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_CYCLE);
                return;
            }
            int[] ec = intel.armyCentroid(c[0], c[1], 25);
            if (ec != null && (float)(now - last_siege_log) >= 4.0f)
            {
                last_siege_log = now;
                float[] s = sidesAt(roster, intel, c, ec);
                int frozen = 0;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
                {
                    if (com.oddlabs.tt.player.fable.Chieftain.isFrozen(u))
                    {
                        frozen++;
                    }
                }
                int ff = frozen;
                int[] fec = ec;
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("siege line: ours=%.1f theirs=%.1f army=%d frozen=%d chief=%s ec=%d,%d tower=%d,%d", s[0], s[1], army.size(), ff, (chief == null ? (object)("none") : (object)((java.lang.JC.f2i(((float)(100 * com.oddlabs.tt.player.fable.Chieftain.stunProgress(chief))))) + "%")), fec[0], fec[1], siege_tower.getGridX(), siege_tower.getGridY());
                }));
            }
            if (ec != null)
            {
                float[] s_2 = sidesAt(roster, intel, c, ec);
                int nearest_tower_d2 = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
                {
                    if (!com.oddlabs.tt.player.fable.Chieftain.towerStunned(t))
                    {
                        nearest_tower_d2 = java.lang.Math.min(nearest_tower_d2, com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), ec[0], ec[1]));
                    }
                }
                bool @out = nearest_tower_d2 > 21 * 21 && (float)(now - army_state_since) > 4.0f;
                bool stun_landing = (float)(now - chief_brain.last_stun_at) < @params.stun_window;
                bool about_to_cast = chief != null && orders.usable(chief) && com.oddlabs.tt.player.fable.Chieftain.stunReady(chief) && com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), ec[0], ec[1]) <= 15 * 15;
                if (s_2[1] > 0.0f && (float)(s_2[0] / s_2[1]) < @params.retreat_ratio && !stun_landing && !about_to_cast)
                {
                    log.info("siege: outgunned at the line, retreating");
                    siege_tower = null;
                    stand_x = (-1);
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                    return;
                }
                if (@out && s_2[0] >= (float)(@params.engage_ratio * java.lang.Math.max(1.0f, s_2[1])))
                {
                    log.info("siege: their army is outside the towers, engaging it");
                    engage_start_value = s_2[1];
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE);
                    return;
                }
            }
            bool camping = t_val >= (float)(@params.siege_camp_ratio * java.lang.Math.max(1.0f, ours));
            if (camping)
            {
                if ((float)(now - siege_prep_since) > @params.siege_camp_patience)
                {
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("siege abandoned: their army (%.0f) camps under the towers vs ours %.0f", t_val, ours);
                    }));
                    siege_tower = null;
                    stand_x = (-1);
                    last_retreat_at = now;
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                    posture = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                    posture_since = now;
                    recallArmy(now, homeCell(roster));
                    return;
                }
                int[] far0 = plan.toward(siege_tower.getGridX(), siege_tower.getGridY(), c[0], c[1], 30);
                int[] far = stickyHold(safePoint(intel, far0[0], far0[1]));
                holdOutsideReach(now, intel, far);
                if (chief != null && orders.usable(chief) && com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), far[0], far[1]) > 64)
                {
                    chief_brain.moveTo(now, chief, far[0], far[1], false);
                }
                return;
            }
            if (corpsDemolish(now, siege_tower, c))
            {
                return;
            }
            if (chief == null || !orders.usable(chief) || (float)(now - siege_prep_since) > 150.0f)
            {
                if (ours >= (float)(2.0f * java.lang.Math.max(1.0f, t_val)))
                {
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT);
                }
                else
                {
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                }
                siege_tower = null;
                stand_x = (-1);
                return;
            }
            if (!com.oddlabs.tt.player.fable.Chieftain.stunReady(chief))
            {
                int[] wait0 = plan.toward(siege_tower.getGridX(), siege_tower.getGridY(), c[0], c[1], 24);
                int[] wait_v = stickyHold(safePoint(intel, wait0[0], wait0[1]));
                holdOutsideReach(now, intel, wait_v);
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), wait_v[0], wait_v[1]) > 100)
                {
                    chief_brain.moveTo(now, chief, wait_v[0], wait_v[1], false);
                }
                return;
            }
            if (stand_x < 0)
            {
                int[] stand = findStand(intel, siege_tower, c);
                if (stand == null)
                {
                    if (ours >= (float)(2.0f * java.lang.Math.max(1.0f, t_val)))
                    {
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT);
                    }
                    else
                    {
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                    }
                    siege_tower = null;
                    return;
                }
                stand_x = stand[0];
                stand_y = stand[1];
                stand_since = now;
                stand_pre_reached = false;
                stand_best_d2 = java.lang.Integer.MAX_VALUE;
                int[] ap0 = plan.toward(siege_tower.getGridX(), siege_tower.getGridY(), stand_x, stand_y, 21);
                int[] ap = safePoint(intel, ap0[0], ap0[1]);
                army_pos_x = ap[0];
                army_pos_y = ap[1];
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "siege: stand " + (stand_x) + "," + (stand_y) + " vs tower " + (siege_tower.getGridX()) + "," + (siege_tower.getGridY());
                }));
            }
            int[] pre = plan.toward(stand_x, stand_y, siege_tower.getGridX(), siege_tower.getGridY(), (float)(-2.5f));
            if (!stand_pre_reached && com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), pre[0], pre[1]) <= 2)
            {
                stand_pre_reached = true;
            }
            if (stand_pre_reached)
            {
                chief_brain.moveTo(now, chief, stand_x, stand_y, true);
            }
            else
            {
                chief_brain.moveTo(now, chief, pre[0], pre[1], false);
            }
            holdOutsideReach(now, intel, new int[] { army_pos_x, army_pos_y });
            int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), stand_x, stand_y);
            if (d2 < stand_best_d2)
            {
                stand_best_d2 = d2;
                stand_since = now;
            }
            bool chief_there = d2 <= 2;
            if (!chief_there && (float)(now - stand_since) > 15.0f)
            {
                stand_blacklist.add(new int[] { stand_x, stand_y });
                stand_fails++;
                stand_x = (-1);
                log.info("siege: stand unreachable, retrying (" + (stand_fails) + ")");
                if (stand_fails >= 3)
                {
                    if (ours >= (float)(1.8f * java.lang.Math.max(1.0f, t_val)))
                    {
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT);
                    }
                    else
                    {
                        setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.RETREAT);
                    }
                    siege_tower = null;
                    stand_fails = 0;
                    stand_blacklist.clear();
                }
                return;
            }
            int near = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), army_pos_x, army_pos_y) <= 100)
                {
                    near++;
                }
            }
            if (chief_there && (near >= (float)(0.7f * army.size()) || (float)(now - siege_prep_since) > 60.0f))
            {
                chief_brain.siege_cast_requested = true;
                siege_cast_at = now;
                stand_blacklist.clear();
                stand_fails = 0;
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_CYCLE);
            }
        }
        // Java: 1831
        public int[] safePoint(com.oddlabs.tt.player.fable.Intel intel, int gx, int gy)
        {
            int x = gx;
            int y = gy;
            for (int i = 0; i < 6; i++)
            {
                com.oddlabs.tt.model.LandBuilding offending = null;
                int best_d2 = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
                {
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), x, y);
                    if (d2 <= 18 * 18 && d2 < best_d2)
                    {
                        best_d2 = d2;
                        offending = t;
                    }
                }
                if (offending == null)
                {
                    break;
                }
                int[] p = plan.toward(x, y, offending.getGridX(), offending.getGridY(), (float)(-((float)(19.0f - ((float)(java.lang.Math.sqrt(best_d2)))))));
                if (!plan.sameIsland(p[0], p[1]) || grid.getRegion(p[0], p[1]) == null)
                {
                    break;
                }
                x = p[0];
                y = p[1];
            }
            return new int[] { x, y };
        }
        // Java: 1860
        public void holdOutsideReach(float now, com.oddlabs.tt.player.fable.Intel intel, int[] p)
        {
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
            {
                if (!orders.usable(u) || com.oddlabs.tt.player.fable.Chieftain.isStunned(u) || com.oddlabs.tt.player.fable.Chieftain.isFrozen(u))
                {
                    continue;
                }
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                bool covered = false;
                foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
                {
                    if (!com.oddlabs.tt.player.fable.Chieftain.towerStunned(t) && com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), u.getGridX(), u.getGridY()) <= 17 * 17)
                    {
                        covered = true;
                        break;
                    }
                }
                if (covered)
                {
                    if (j == null || ((object)j.kind != (object)com.oddlabs.tt.player.fable.Jobs.Kind.MOVE) || (float)(now - j.issued_at) > 3.0f)
                    {
                        orders.move(u, p[0], p[1]);
                        jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, p[0], p[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                    }
                    continue;
                }
                if ((u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour) || (u.getCurrentController() is com.oddlabs.tt.model.behaviour.HuntController))
                {
                    continue;
                }
                bool there = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), p[0], p[1]) <= 64;
                bool same = j != null && ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.HOLD) && com.oddlabs.tt.player.fable.BasePlan.dist2(j.gx, j.gy, p[0], p[1]) <= 36;
                bool idle_far = ((object)com.oddlabs.tt.player.fable.Roster.activity(u) == (object)com.oddlabs.tt.player.fable.Roster.Activity.IDLE) && !there && (j == null || (float)(now - j.issued_at) > 3.0f);
                if (!same || idle_far)
                {
                    orders.attackMove(u, p[0], p[1]);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.HOLD, p[0], p[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                }
            }
            micro.fight(now, army, intel, p[0], p[1], false, false, null, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
        }
        // Java: 1901
        public int[] stickyHold(int[] p)
        {
            if (hold_x >= 0 && com.oddlabs.tt.player.fable.BasePlan.dist2(p[0], p[1], hold_x, hold_y) <= 36)
            {
                return new int[] { hold_x, hold_y };
            }
            hold_x = p[0];
            hold_y = p[1];
            return p;
        }
        // Java: 1913
        public int[] findStand(com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.model.LandBuilding tower, int[] c)
        {
            int tx = tower.getGridX();
            int ty = tower.getGridY();
            int[] best = null;
            float best_score = java.lang.Float.NEGATIVE_INFINITY;
            for (int i = 0; i < 36; i++)
            {
                double a = java.lang.Math.toRadians(i * 10);
                int gx = unchecked((int)(java.lang.Math.round(tx + 16.25d * java.lang.StrictMath.cos(a))));
                int gy = unchecked((int)(java.lang.Math.round(ty + 16.25d * java.lang.StrictMath.sin(a))));
                if (!plan.sameIsland(gx, gy) || grid.getRegion(gx, gy) == null)
                {
                    continue;
                }
                bool listed = false;
                foreach (int[] b in java.lang.JIter.of(stand_blacklist))
                {
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(b[0], b[1], gx, gy) <= 16)
                    {
                        listed = true;
                        break;
                    }
                }
                if (listed)
                {
                    continue;
                }
                if (((object)grid.getRegion(gx, gy) != (object)grid.getRegion(c[0], c[1])) && grid.getRegion(c[0], c[1]) != null && !regionsAdjacent(gx, gy, c))
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(gx, gy, tx, ty);
                if (d2 < @params.stand_min2 || d2 > @params.stand_max2)
                {
                    continue;
                }
                bool safe = true;
                foreach (com.oddlabs.tt.model.LandBuilding other in java.lang.JIter.of(intel.manned_towers))
                {
                    if (((object)other == (object)tower))
                    {
                        continue;
                    }
                    if (com.oddlabs.tt.player.fable.BasePlan.dist2(gx, gy, other.getGridX(), other.getGridY()) < 256)
                    {
                        safe = false;
                        break;
                    }
                }
                if (!safe)
                {
                    continue;
                }
                float score = (float)((float)((float)((float)(-0.02f) * com.oddlabs.tt.player.fable.BasePlan.dist2(gx, gy, c[0], c[1])) - enemyValueNear(intel, gx, gy, 12)) + (float)(5.0f * countTowersWithin(intel, gx, gy, 17)));
                if (score > best_score)
                {
                    best_score = score;
                    best = new int[] { gx, gy };
                }
            }
            return best;
        }
        // Java: 1961
        public bool regionsAdjacent(int gx, int gy, int[] c)
        {
            com.oddlabs.tt.pathfinder.Region from = grid.getRegion(c[0], c[1]);
            com.oddlabs.tt.pathfinder.Region to = grid.getRegion(gx, gy);
            if (from == null || to == null)
            {
                return false;
            }
            return com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(grid, from, to) != null;
        }
        // Java: 1969
        public int countTowersWithin(com.oddlabs.tt.player.fable.Intel intel, int gx, int gy, int r)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.LandBuilding t in java.lang.JIter.of(intel.manned_towers))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(t.getGridX(), t.getGridY(), gx, gy) <= r * r)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1977
        public void siegeCycle(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c)
        {
            if (siege_tower == null || siege_tower.isDead())
            {
                log.info("siege: tower down");
                siege_tower = null;
                stand_x = (-1);
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.MARCH);
                nextLeg(now, c);
                return;
            }
            float since = (float)(now - siege_cast_at);
            if (since < 1.5f)
            {
                return;
            }
            if (since < (float)(@params.stun_window + 4.0f))
            {
                if (com.oddlabs.tt.player.fable.Chieftain.towerStunned(siege_tower) || since < 5.0f)
                {
                    corpsDemolish(now, siege_tower, c);
                    if ((float)(now - last_engage_order) > 3.0f)
                    {
                        java.util.List to = new java.util.ArrayList();
                        foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(army))
                        {
                            if (orders.usable(u) && !com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                            {
                                to.add(u);
                            }
                        }
                        orders.attack(to, siege_tower);
                        foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(to))
                        {
                            jobs.set(u_2, com.oddlabs.tt.player.fable.Jobs.Kind.HUNT, siege_tower.getGridX(), siege_tower.getGridY(), siege_tower, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                        }
                        last_engage_order = now;
                    }
                }
                else if (since > 6.0f)
                {
                    log.info("siege: stun missed the tower, pulling back");
                    int[] back0 = plan.toward(siege_tower.getGridX(), siege_tower.getGridY(), army_pos_x, army_pos_y, 21);
                    int[] back = safePoint(intel, back0[0], back0[1]);
                    orders.move(new java.util.ArrayList(army), back[0], back[1]);
                    foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(army))
                    {
                        jobs.set(u_3, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, back[0], back[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                    }
                    stand_x = (-1);
                    siege_prep_since = now;
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP);
                }
                return;
            }
            int[] back0_2 = plan.toward(siege_tower.getGridX(), siege_tower.getGridY(), army_pos_x, army_pos_y, 21);
            int[] back_2 = safePoint(intel, back0_2[0], back0_2[1]);
            orders.move(new java.util.ArrayList(army), back_2[0], back_2[1]);
            foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(army))
            {
                jobs.set(u_4, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, back_2[0], back_2[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
            }
            stand_x = (-1);
            siege_prep_since = now;
            setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP);
        }
        // Java: 2030
        public void retreat(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, int[] c, int[] home)
        {
            if ((float)(now - chief_brain.last_stun_at) < 3.0f && chief_brain.last_stun_cell != null && (float)(now - army_state_since) < 4.0f && com.oddlabs.tt.player.fable.BasePlan.dist2(chief_brain.last_stun_cell[0], chief_brain.last_stun_cell[1], c[0], c[1]) <= 25 * 25 && groupValue(army) >= (float)(0.5f * last_fight_theirs))
            {
                log.info("retreat cancelled: our stun landed");
                hold_since = (float)(-1.0f);
                setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE);
                return;
            }
            if ((float)(now - army_state_since) < 0.1f || ((leg_x != prev_leg_x || leg_y != prev_leg_y) && (float)(now - army_state_since) < 1.0f))
            {
                int[] back = (prev_leg_x >= 0 ? new int[] { prev_leg_x, prev_leg_y } : new int[] { staging_x, staging_y });
                java.util.List all = new java.util.ArrayList(army);
                orders.move(all, back[0], back[1]);
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(all))
                {
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, back[0], back[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                }
                log.info(new java.util.function.Supplier_L(() =>
                {
                    return "army retreats to " + (back[0]) + "," + (back[1]);
                }));
                leg_x = back[0];
                leg_y = back[1];
            }
            int arrived = 0;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(army))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u_2.getGridX(), u_2.getGridY(), leg_x, leg_y) <= 100)
                {
                    arrived++;
                }
            }
            float theirs = enemyValueNear(intel, c[0], c[1], 25);
            if (arrived >= (float)(0.7f * army.size()) || (float)(now - army_state_since) > 30.0f)
            {
                army.addAll(reinforcements);
                reinforcements.clear();
                float ours = groupValue(army);
                float @ref = java.lang.Math.max(theirs, (float)(0.8f * last_fight_theirs));
                last_retreat_at = now;
                if (@ref > 0.0f && (float)(ours / @ref) < @params.engage_ratio)
                {
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                    posture = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
                    posture_since = now;
                    recallArmy(now, home);
                    float fo = ours;
                    float fr = @ref;
                    log.info(new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("army retreat -> home (BUILD_UP) ours=%.1f vs %.1f", fo, fr);
                    }));
                }
                else
                {
                    setArmyState(now, com.oddlabs.tt.player.fable.Military.ArmyState.STAGING);
                    if (com.oddlabs.tt.player.fable.BasePlan.dist(c[0], c[1], staging_x, staging_y) > 60.0f)
                    {
                        regroup_x = leg_x;
                        regroup_y = leg_y;
                        staging_x = leg_x;
                        staging_y = leg_y;
                    }
                }
            }
        }
        // Java: 2088
        public void reinforcementsStep(float now, com.oddlabs.tt.player.fable.Roster roster, int[] home)
        {
            if (reinforcements.isEmpty())
            {
                return;
            }
            if (((object)posture != (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) && ((object)posture != (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
            {
                army.addAll(reinforcements);
                reinforcements.clear();
                return;
            }
            if (threat_serious)
            {
                return;
            }
            int[] ac = centroid(army);
            if (ac == null)
            {
                army.addAll(reinforcements);
                reinforcements.clear();
                return;
            }
            float d_army = com.oddlabs.tt.player.fable.BasePlan.dist(ac[0], ac[1], home[0], home[1]);
            int batch = (d_army < 60.0f ? @params.reinforce_batch_near : @params.reinforce_batch);
            if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN))
            {
                batch = java.lang.Math.min(batch, 4);
            }
            java.util.List merged = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(reinforcements))
            {
                if (com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), ac[0], ac[1]) <= 20 * 20)
                {
                    merged.add(u);
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(merged))
            {
                reinforcements.remove(u_2);
                army.add(u_2);
            }
            if (reinforcements.size() >= batch)
            {
                java.util.List batch_units = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(reinforcements))
                {
                    com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u_3);
                    if (j == null || ((object)j.kind != (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) || j.tag != com.oddlabs.tt.player.fable.Military.TAG_REINF)
                    {
                        batch_units.add(u_3);
                    }
                }
                if (!batch_units.isEmpty())
                {
                    orders.attackMove(batch_units, ac[0], ac[1]);
                    foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(batch_units))
                    {
                        jobs.set(u_4, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, ac[0], ac[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_REINF);
                    }
                }
            }
            else
            {
                foreach (com.oddlabs.tt.model.Unit u_5 in java.lang.JIter.of(reinforcements))
                {
                    com.oddlabs.tt.player.fable.Jobs.Job j_2 = jobs.get(u_5);
                    bool en_route = j_2 != null && ((object)j_2.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) && j_2.tag == com.oddlabs.tt.player.fable.Military.TAG_REINF;
                    if (j_2 == null || (((object)j_2.kind != (object)com.oddlabs.tt.player.fable.Jobs.Kind.HOLD) && !en_route))
                    {
                        orders.attackMove(u_5, staging_x, staging_y);
                        jobs.set(u_5, com.oddlabs.tt.player.fable.Jobs.Kind.HOLD, staging_x, staging_y, null, null, now, com.oddlabs.tt.player.fable.Military.TAG_REINF);
                    }
                }
            }
            foreach (com.oddlabs.tt.model.Unit u_6 in java.lang.JIter.of(reinforcements))
            {
                com.oddlabs.tt.player.fable.Jobs.Job j_3 = jobs.get(u_6);
                if (j_3 != null && ((object)j_3.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) && (float)(now - j_3.issued_at) > 8.0f && com.oddlabs.tt.player.fable.BasePlan.dist2(j_3.gx, j_3.gy, ac[0], ac[1]) > 100)
                {
                    orders.attackMove(u_6, ac[0], ac[1]);
                    jobs.set(u_6, com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE, ac[0], ac[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_REINF);
                }
            }
        }
        public java.util.Map recovery_orders;
        // Java: 2161
        public void recoverStunned(float now, com.oddlabs.tt.player.fable.Roster roster)
        {
            if (!@params.stun_recovery)
            {
                return;
            }
            recovery_orders.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || (float)(now - ((float)((float?)e.getValue()))) > 60.0f;
            }));
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
            {
                if (!orders.usable(u) || !com.oddlabs.tt.player.fable.Chieftain.isStunned(u))
                {
                    continue;
                }
                float? last = ((float?)recovery_orders.get(u));
                if (last != null && (float)(now - ((float)last)) < 20.0f)
                {
                    continue;
                }
                recovery_orders.put(u, now);
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                if (j != null && (((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.HUNT)) && j.target != null && !j.target.isDead())
                {
                    orders.attack(u, j.target);
                }
                else if (j != null && (((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.MOVE)))
                {
                    orders.move(u, j.gx, j.gy);
                }
                else if (j != null && j.gx >= 0 && (((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.ATTACK_MOVE) || ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.DEFEND) || ((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.HOLD)))
                {
                    orders.attackMove(u, j.gx, j.gy);
                }
                else
                {
                    orders.attackMove(u, u.getGridX(), u.getGridY());
                }
            }
            com.oddlabs.tt.model.Unit chief = roster.chieftain;
            if (chief != null && orders.usable(chief) && com.oddlabs.tt.player.fable.Chieftain.isStunned(chief))
            {
                float? last_2 = ((float?)recovery_orders.get(chief));
                if (last_2 == null || (float)(now - ((float)last_2)) >= 20.0f)
                {
                    recovery_orders.put(chief, now);
                    orders.move(chief, chief.getGridX(), chief.getGridY());
                }
            }
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(roster.peons))
            {
                if (!orders.usable(p) || !com.oddlabs.tt.player.fable.Chieftain.isStunned(p))
                {
                    continue;
                }
                float? last_3 = ((float?)recovery_orders.get(p));
                if (last_3 != null && (float)(now - ((float)last_3)) < 20.0f)
                {
                    continue;
                }
                recovery_orders.put(p, now);
                com.oddlabs.tt.player.fable.Jobs.Job j_2 = jobs.get(p);
                if (j_2 != null && ((object)j_2.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.BUILD) && (j_2.target is com.oddlabs.tt.model.LandBuilding b) && !b.isDead())
                {
                    orders.build(p, b);
                }
                else
                {
                    orders.move(p, p.getGridX(), p.getGridY());
                }
            }
        }
        // Java: 2210
        public void focusStunned(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel)
        {
            float since = (float)(now - chief_brain.last_stun_at);
            if (since < 4.0f || since > (float)(@params.stun_window + 2.0f) || chief_brain.last_stun_cell == null)
            {
                return;
            }
            if ((float)(now - last_focus_at) < 2.0f)
            {
                return;
            }
            int cx = chief_brain.last_stun_cell[0];
            int cy = chief_brain.last_stun_cell[1];
            com.oddlabs.tt.player.fable.Chieftain.stunnedEnemies(intel, cx, cy, 20, scratch_targets);
            if (scratch_targets.isEmpty())
            {
                return;
            }
            java.util.List shooters = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
            {
                if (orders.usable(u) && !com.oddlabs.tt.player.fable.Chieftain.isStunned(u) && !((u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour)) && com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), cx, cy) <= 22 * 22)
                {
                    shooters.add(u);
                }
            }
            if (shooters.isEmpty())
            {
                return;
            }
            int idx = 0;
            foreach (com.oddlabs.tt.model.Selectable t in java.lang.JIter.of(scratch_targets))
            {
                int per = ((t is com.oddlabs.tt.model.Unit tu) && tu.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) ? 8 : ((t is com.oddlabs.tt.model.LandBuilding) ? 6 : 2));
                java.util.List group = new java.util.ArrayList();
                for (int i = 0; i < per && idx < shooters.size(); i++, idx++)
                {
                    group.add(((com.oddlabs.tt.model.Unit)shooters.get(idx)));
                }
                if (group.isEmpty())
                {
                    break;
                }
                orders.attack(group, t);
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(group))
                {
                    jobs.set(u_2, com.oddlabs.tt.player.fable.Jobs.Kind.HUNT, t.getGridX(), t.getGridY(), t, null, now, com.oddlabs.tt.player.fable.Military.TAG_FOCUS);
                }
            }
            last_focus_at = now;
        }
        // Java: 2245
        public void leash(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel)
        {
            if ((float)(now - last_leash) < 2.0f)
            {
                return;
            }
            last_leash = now;
            int[] ac = centroid(army);
            int[] hc = centroid(home_guard);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(roster.warriors))
            {
                if (!orders.usable(u) || !((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.HuntController)) || (u.getCurrentBehaviour() is com.oddlabs.tt.model.behaviour.AttackBehaviour))
                {
                    continue;
                }
                com.oddlabs.tt.player.fable.Jobs.Job j = jobs.get(u);
                if (j != null && (((object)j.kind == (object)com.oddlabs.tt.player.fable.Jobs.Kind.HUNT)) && (float)(now - j.issued_at) < (float)(@params.stun_window + 4.0f))
                {
                    continue;
                }
                int[] c = (army.contains(u) ? ac : (home_guard.contains(u) ? hc : (int[])(null)));
                if (c == null)
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(u.getGridX(), u.getGridY(), c[0], c[1]);
                if (d2 > @params.leash_dist * @params.leash_dist)
                {
                    orders.move(u, c[0], c[1]);
                    jobs.set(u, com.oddlabs.tt.player.fable.Jobs.Kind.MOVE, c[0], c[1], null, null, now, com.oddlabs.tt.player.fable.Military.TAG_ARMY);
                }
            }
        }
        // Java: 2271
        public void positionChieftain(float now, com.oddlabs.tt.player.fable.Roster roster, com.oddlabs.tt.player.fable.Intel intel, com.oddlabs.tt.player.fable.Threat threat, int[] home)
        {
            com.oddlabs.tt.model.Unit chief = roster.chieftain;
            if (chief == null || !orders.usable(chief) || com.oddlabs.tt.player.fable.Chieftain.isCasting(chief) || com.oddlabs.tt.player.fable.Chieftain.isStunned(chief))
            {
                return;
            }
            if (chief_brain.shouldEscape(chief, intel))
            {
                int[] c = centroid(army);
                int[] to = (c != null && army.size() >= 6 && com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), c[0], c[1]) < com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), home[0], home[1]) ? c : plan.along(home[0], home[1], (-6)));
                chief_brain.moveTo(now, chief, to[0], to[1], true);
                return;
            }
            if (((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_PREP) || ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_CYCLE))
            {
                return;
            }
            bool with_army = (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) || ((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ALL_IN)) && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING) && !army.isEmpty();
            if (with_army)
            {
                int[] c_2 = centroid(army);
                if (c_2 == null)
                {
                    return;
                }
                bool fighting = (((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE) || ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT)) && fight_x >= 0 && (float)(now - fight_at) < 3.0f;
                int screen = (fighting ? com.oddlabs.tt.player.fable.Chieftain.ownWarriorsNear(roster.warriors, fight_x, fight_y, 20) : 0);
                if (fighting && screen >= 6 && com.oddlabs.tt.player.fable.Chieftain.stunReady(chief))
                {
                    int[] p = plan.toward(fight_x, fight_y, c_2[0], c_2[1], @params.stun_cast_dist - 7);
                    int d2 = com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), p[0], p[1]);
                    if (d2 > 9)
                    {
                        chief_brain.moveTo(now, chief, p[0], p[1], true);
                    }
                    return;
                }
                int follow = (((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.MARCH) ? 1 : @params.chief_follow_dist);
                int[] behind = (objective_x >= 0 ? plan.toward(c_2[0], c_2[1], objective_x, objective_y, -follow) : c_2);
                int d2_2 = com.oddlabs.tt.player.fable.BasePlan.dist2(chief.getGridX(), chief.getGridY(), behind[0], behind[1]);
                if (d2_2 > 144 || (d2_2 < 9 && ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE)))
                {
                    chief_brain.moveTo(now, chief, behind[0], behind[1], false);
                }
            }
            else if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.DEFEND) && threat.approach_centroid != null)
            {
                int[] p_2 = plan.toward(home[0], home[1], threat.approach_centroid[0], threat.approach_centroid[1], @params.tower_ring_radius - 2);
                chief_brain.moveTo(now, chief, p_2[0], p_2[1], false);
            }
            else
            {
                int[] p_3 = plan.along(home[0], home[1], 9);
                if (((object)posture == (object)com.oddlabs.tt.player.fable.Military.Posture.ATTACK) && ((object)army_state == (object)com.oddlabs.tt.player.fable.Military.ArmyState.STAGING))
                {
                    p_3 = new int[] { staging_x, staging_y };
                }
                chief_brain.moveTo(now, chief, p_3[0], p_3[1], false);
            }
        }
        // Java: 2327
        public int armySize()
        {
            return army.size();
        }
        // Java: 2331
        public int guardSize()
        {
            return home_guard.size();
        }
        // Java: 2335
        public int reinforcementSize()
        {
            return reinforcements.size();
        }
        // Java: 2339
        public bool fightingNear(int gx, int gy, int radius)
        {
            if (((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.ENGAGE) && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.ASSAULT) && ((object)army_state != (object)com.oddlabs.tt.player.fable.Military.ArmyState.SIEGE_CYCLE))
            {
                return false;
            }
            int[] c = centroid(army);
            return c != null && com.oddlabs.tt.player.fable.BasePlan.dist2(c[0], c[1], gx, gy) <= radius * radius;
        }
        // Java: 2346
        public string status()
        {
            return java.lang.JStr.valueOf(posture) + "/" + java.lang.JStr.valueOf(army_state) + " army=" + (army.size()) + " guard=" + (home_guard.size()) + " reinf=" + (reinforcements.size()) + java.lang.JStr.valueOf(((objective != null ? " obj=" + (objective_x) + "," + (objective_y) : "")));
        }
        private void init_Military()
        {
            this.posture = com.oddlabs.tt.player.fable.Military.Posture.BUILD_UP;
            this.army_state = com.oddlabs.tt.player.fable.Military.ArmyState.STAGING;
            this.army = new java.util.LinkedHashSet();
            this.reinforcements = new java.util.LinkedHashSet();
            this.home_guard = new java.util.LinkedHashSet();
            this.objective_x = (-1);
            this.objective_y = (-1);
            this.leg_x = (-1);
            this.leg_y = (-1);
            this.prev_leg_x = (-1);
            this.prev_leg_y = (-1);
            this.last_engage_order = (float)(-100.0f);
            this.last_engage_x = (-1);
            this.last_engage_y = (-1);
            this.hold_since = (float)(-1.0f);
            this.stand_x = (-1);
            this.stand_y = (-1);
            this.army_pos_x = (-1);
            this.army_pos_y = (-1);
            this.siege_cast_at = (float)(-100.0f);
            this.staging_x = (-1);
            this.regroup_x = (-1);
            this.regroup_y = (-1);
            this.hold_x = (-1);
            this.hold_y = (-1);
            this.staging_y = (-1);
            this.rally_x = (-1);
            this.rally_armories = (-1);
            this.rally_y = (-1);
            this.last_defend_order = (float)(-100.0f);
            this.last_defend_x = (-1);
            this.last_defend_y = (-1);
            this.last_garrison_swap = (float)(-100.0f);
            this.last_focus_at = (float)(-100.0f);
            this.last_leash = (float)(-100.0f);
            this.scratch_targets = new java.util.ArrayList();
            this.corps = new java.util.LinkedHashSet();
            this.last_corps_order = (float)(-100.0f);
            this.last_corps_recruit = (float)(-100.0f);
            this.guard_target = 3;
            this.sweep_x = (-1);
            this.sweep_y = (-1);
            this.sweep_since = (float)(-100.0f);
            this.sweep_cooldown_until = (float)(-100.0f);
            this.last_engage_log = (float)(-100.0f);
            this.last_retreat_at = (float)(-1000.0f);
            this.cap_since = (float)(-1.0f);
            this.reinf_wait_since = (float)(-1.0f);
            this.chief_wait_since = (float)(-1.0f);
            this.last_siege_log = (float)(-100.0f);
            this.fight_x = (-1);
            this.fight_y = (-1);
            this.fight_at = (float)(-100.0f);
            this.last_scatter = (float)(-100.0f);
            this.last_snipe = (float)(-100.0f);
            this.stand_blacklist = new java.util.ArrayList();
            this.recovery_orders = new java.util.LinkedHashMap();
        }
        public Military(com.oddlabs.tt.player.Player player, com.oddlabs.tt.player.fable.Orders orders, com.oddlabs.tt.player.fable.Jobs jobs, com.oddlabs.tt.player.fable.Params @params, com.oddlabs.tt.player.fable.AiLog log, com.oddlabs.tt.player.fable.BasePlan plan, com.oddlabs.tt.player.fable.Chieftain chief_brain) : base()
        {
            init_Military();
            this.player = player;
            this.orders = orders;
            this.jobs = jobs;
            this.@params = @params;
            this.log = log;
            this.plan = plan;
            this.chief_brain = chief_brain;
            this.grid = player.getWorld().getUnitGrid();
            this.micro = new com.oddlabs.tt.player.fable.Micro(player, orders, jobs, @params);
        }
    }
}

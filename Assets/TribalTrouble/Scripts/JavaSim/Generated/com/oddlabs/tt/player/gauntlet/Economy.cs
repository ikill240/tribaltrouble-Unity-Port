// Translated from Economy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.gauntlet
{
    // Java: 44
    public class Economy : java.lang.Object
    {
        public const float IRON_WORK = 80.0f;
        public const int MAX_BUILDERS = 20;
        public const int TREE_LOAD = 2;
        public com.oddlabs.tt.player.gauntlet.GauntletAI ai;
        public java.util.List projects;
        public java.util.Map gather_targets;
        public java.util.Map supply_load;
        public java.util.List chickens;
        public com.oddlabs.tt.model.Unit scout;
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site armory_site;
        public com.oddlabs.tt.player.gauntlet.DistanceField armory_field;
        public com.oddlabs.tt.model.Building armory_field_owner;
        public float armory_field_time;
        public float chickens_time;
        public int want_tree;
        public int want_iron;
        public int want_rock;
        public int want_chicken;
        public int want_workers;
        public float tree_cycle;
        public float iron_cycle;
        public com.oddlabs.tt.model.Building iron_cycle_armory;
        public bool rock_weapons;
        public bool rock_filler;
        public com.oddlabs.tt.player.gauntlet.Economy.Project expansion_project;
        public com.oddlabs.tt.model.Building expansion;
        public float last_expansion_check;
        public float last_old_recall;
        public int project_counter;
        public java.util.List forward_towers;
        public java.util.List sniper_towers;
        public java.util.Map evacuating;
        public float last_sniper;
        public java.util.Map gather_progress;
        public java.util.Map bad_supplies;
        public int unstuck;
        public java.util.Map builder_cells;
        public float last_unstuck_log;
        public bool rush_alert;
        public float rush_alert_time;
        public bool had_armory;
        public com.oddlabs.tt.player.gauntlet.Retire retire;
        // Java: 108
        public void openingPlan()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.SitePlanner planner = ai.planner();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            java.util.List reserved = new java.util.ArrayList();
            armory_site = planner.findArmorySite(reserved);
            int sx = planner.getStartX();
            int sy = planner.getStartY();
            int ax = (armory_site != null ? armory_site.x : sx);
            int ay = (armory_site != null ? armory_site.y : sy);
            if (armory_site != null)
            {
                reserved.add(armory_site.withHalf(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.ARMORY));
            }
            int first_builders = java.lang.Math.max(1, intel.peons.size() - strategy.scouts - intel.strikers.size());
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site q1 = planner.findQuartersSite(reserved, sx, sy, 110, planner.getStartField(), ax, ay, first_builders, 0.2f, 0.06f);
            if (q1 == null || (float)(-q1.score) > 110.0f)
            {
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site further = planner.findQuartersSite(reserved, sx, sy, 400, planner.getStartField(), ax, ay, first_builders, 0.2f, 0.06f);
                if (further != null && (q1 == null || further.score > q1.score))
                {
                    q1 = further;
                }
            }
            if (q1 == null)
            {
                q1 = planner.findQuartersSite(java.util.List_.of(), sx, sy, 400, planner.getStartField(), ax, ay, first_builders, 0.2f, 0.0f);
                if (q1 != null && armory_site != null && com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(java.util.List_.of(armory_site), q1.x, q1.y, com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.QUARTERS))
                {
                    reserved.remove(armory_site);
                    armory_site = null;
                }
            }
            if (q1 != null)
            {
                reserved.add(q1);
                com.oddlabs.tt.player.gauntlet.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, q1, 0);
                p.first = true;
            }
            int armory_priority = 1 + 2 * java.lang.Math.max(0, strategy.quarters_before_armory - 1);
            if (armory_site != null)
            {
                com.oddlabs.tt.player.gauntlet.Economy.Project p_2 = addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, armory_site, armory_priority);
                p_2.use_scout = true;
            }
            com.oddlabs.tt.player.gauntlet.DistanceField a_field = (armory_site != null ? ai.map().computeField(ax, ay, 240) : (com.oddlabs.tt.player.gauntlet.DistanceField)(null));
            for (int i = 1; i < strategy.initial_quarters; i++)
            {
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site q = default;
                if (strategy.opening_near_start && q1 != null)
                {
                    q = planner.findQuartersSite(reserved, q1.x, q1.y, 80, planner.getStartField(), ax, ay, strategy.quarters_builders, 0.25f, 0.02f);
                }
                else if (a_field != null)
                {
                    q = planner.findQuartersSite(reserved, ax, ay, 80, a_field, sx, sy, strategy.quarters_builders, 0.25f, 0.02f);
                }
                else
                {
                    q = planner.findQuartersSite(reserved, sx, sy, 110, planner.getStartField(), sx, sy, strategy.quarters_builders, 0.25f, 0.0f);
                }
                if (q == null)
                {
                    break;
                }
                reserved.add(q);
                com.oddlabs.tt.player.gauntlet.Economy.Project p_3 = addProject(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, q, 2 * i);
                p_3.use_scout = true;
            }
            com.oddlabs.tt.model.Unit best_scout = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                if (intel.strikers.contains(peon))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), ax, ay);
                if (d < best_d)
                {
                    best_d = d;
                    best_scout = peon;
                }
            }
            scout = (strategy.scouts > 0 ? best_scout : (com.oddlabs.tt.model.Unit)(null));
            com.oddlabs.tt.player.gauntlet.Economy.Project first = (projects.isEmpty() ? (com.oddlabs.tt.player.gauntlet.Economy.Project)(null) : ((com.oddlabs.tt.player.gauntlet.Economy.Project)projects.getFirst()));
            if (first != null && first.first)
            {
                java.util.List builders = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit peon_2 in java.lang.JIter.of(intel.peons))
                {
                    if (((object)peon_2 != (object)scout) && !intel.strikers.contains(peon_2))
                    {
                        builders.add(peon_2);
                    }
                }
                if (!builders.isEmpty())
                {
                    place(first, builders);
                }
            }
        }
        // Java: 203
        public com.oddlabs.tt.model.Building pushArmory()
        {
            if (!ai.intel().armories.isEmpty())
            {
                return null;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project armory = null;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    armory = p;
                    break;
                }
            }
            if (armory == null)
            {
                return null;
            }
            if (armory.priority > 1)
            {
                projects.remove(armory);
                armory.priority = 1;
                int i = 0;
                while (i < projects.size() && ((com.oddlabs.tt.player.gauntlet.Economy.Project)projects.get(i)).priority <= armory.priority)
                {
                    i++;
                }
                projects.add(i, armory);
                ai.aiLog().count("freeze_push_priority");
                ai.log("freeze push: " + java.lang.JStr.valueOf(armory.describe()) + " moves up to priority 1");
            }
            com.oddlabs.tt.model.Building site = armory.building;
            return (armory.isPlaced() && site != null && !site.isComplete() ? site : (com.oddlabs.tt.model.Building)(null));
        }
        // Java: 232
        public void tick()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            choosePrimaryArmory();
            refreshArmoryField();
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            if (st.tower_cooldown || st.reloc || st.rearm_reach > 0 || st.reloc_lock > 0 || st.retire)
            {
                trackRazings();
            }
            manageProjects();
            if (st.reloc || st.reloc_lock > 0)
            {
                watchReloc();
            }
            if (st.retire)
            {
                retire.tick();
            }
            escortForward();
            evacuate();
            measureYield();
            manageQuarters();
            manageArmory();
            guardBank();
            allocatePeons();
            manageRepairs();
        }
        // Java: 255
        public bool isEvacuating(com.oddlabs.tt.model.Building b)
        {
            return evacuating.containsKey(b);
        }
        // Java: 265
        public void evacuate()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!raid_evacs.isEmpty())
            {
                endRaidEvacs();
            }
            evacuating.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Building)e.getKey()).isDead() || (float)(ai.time() - ((float)((float?)e.getValue()))) > 60.0f;
            }));
            if (strategy.raid_evac)
            {
                raidEvacuate();
            }
            if (!strategy.evacuate)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List homes = intel.homes();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(homes))
            {
                if (b.isDead() || !b.isComplete() || b.getUnitContainer() == null)
                {
                    continue;
                }
                int inside = b.getUnitContainer().getNumSupplies();
                if (inside == 0 && !evacuating.containsKey(b))
                {
                    continue;
                }
                if (b.getHitPoints() > (float)(strategy.evac_hp * ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getMaxHitPoints()))
                {
                    continue;
                }
                int n = 0;
                long ex = 0;
                long ey = 0;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(intel.enemy_warriors))
                {
                    if (e_2.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(e_2.getGridX(), e_2.getGridY(), b.getGridX(), b.getGridY()) > 10 * 10)
                    {
                        continue;
                    }
                    n++;
                    ex += e_2.getGridX();
                    ey += e_2.getGridY();
                }
                if (n < strategy.evac_min)
                {
                    continue;
                }
                com.oddlabs.tt.player.Player owner = ai.owner();
                if (!evacuating.containsKey(b))
                {
                    int cx = unchecked((int)((ex / n)));
                    int cy = unchecked((int)((ey / n)));
                    bool quarters = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_QUARTERS;
                    com.oddlabs.tt.model.Building refuge_v = null;
                    int far = (-1);
                    if (quarters)
                    {
                        foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                        {
                            if (((object)q == (object)b) || q.isDead() || evacuating.containsKey(q) || retire.isDoomed(q))
                            {
                                continue;
                            }
                            int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), cx, cy);
                            if (d > far && !ai.military().threatNear(q.getGridX(), q.getGridY(), 16))
                            {
                                far = d;
                                refuge_v = q;
                            }
                        }
                    }
                    if (refuge_v != null)
                    {
                        owner.setRallyPoint(b, refuge_v);
                    }
                    else
                    {
                        float dx = b.getGridX() - cx;
                        float dy = b.getGridY() - cy;
                        float len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                        int size = ai.map().getSize();
                        int rx = java.lang.Math.max(3, java.lang.Math.min(size - 4, b.getGridX() + java.lang.Math.round((float)((float)(18 * dx) / len))));
                        int ry = java.lang.Math.max(3, java.lang.Math.min(size - 4, b.getGridY() + java.lang.Math.round((float)((float)(18 * dy) / len))));
                        owner.setRallyPoint(b, rx, ry);
                    }
                    evacuating.put(b, ai.time());
                    ai.aiLog().count((quarters ? "evac_quarters" : "evac_armory"));
                    ai.log("evacuating " + java.lang.JStr.valueOf(((quarters ? "quarters" : "armory"))) + " at " + (b.getGridX()) + "," + (b.getGridY()) + ": " + (inside) + " inside, hp " + (b.getHitPoints()) + ", " + (n) + " enemy warriors by it");
                }
                continueEvacuation(b, inside);
            }
        }
        // Java: 337
        public void continueEvacuation(com.oddlabs.tt.model.Building b, int inside)
        {
            if (inside == 0)
            {
                return;
            }
            int deployed = 0;
            if (((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
            {
                deployed = deployWarriors(b, inside, com.oddlabs.tt.player.gauntlet.Economy.stock(b, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)), com.oddlabs.tt.player.gauntlet.Economy.stock(b, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)), com.oddlabs.tt.player.gauntlet.Economy.stock(b, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)));
            }
            int pending = b.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            int peons = inside - deployed - pending;
            if (peons > 0)
            {
                ai.owner().deployUnits(b, com.oddlabs.tt.model.DeployType.PEON, peons);
            }
        }
        // Java: 354
        public class Raid : java.lang.Object
        {
            public com.oddlabs.tt.model.Building armory;
            public com.oddlabs.tt.player.Player copy;
            public java.util.List wave;
            public int tx;
            public int ty;
            public Raid(com.oddlabs.tt.model.Building armory, com.oddlabs.tt.player.Player copy, java.util.List wave, int tx, int ty) : base()
            {
                this.armory = armory;
                this.copy = copy;
                this.wave = wave;
                this.tx = tx;
                this.ty = ty;
            }
        }
        // Java: 371
        public class RaidEvac : java.lang.Object
        {
            public com.oddlabs.tt.model.Building refuge;
            public com.oddlabs.tt.model.Building shelter;
            public RaidEvac() : base()
            {
            }
        }
        public java.util.List raids;
        public java.util.Map raid_evacs;
        public java.util.Map raid_hold;
        public const float WAVE_SPEED = 2.5f;
        public const float RAID_LEAD = 45.0f;
        public const float RAID_LATE = 10.0f;
        public const int RAID_CELLS = 20;
        public const int RAID_WAVE_MIN = 12;
        // Java: 396
        public void waveLaunched(com.oddlabs.tt.player.Player copy, int tx, int ty)
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            if (!st.raid_evac)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building target = null;
            int best = com.oddlabs.tt.player.gauntlet.Economy.RAID_CELLS * com.oddlabs.tt.player.gauntlet.Economy.RAID_CELLS + 1;
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
            {
                if (a.isDead() || !a.isComplete())
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), tx, ty);
                if (d < best)
                {
                    best = d;
                    target = a;
                }
            }
            if (target == null)
            {
                return;
            }
            java.util.List wave = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (((object)e.getOwner() == (object)copy) && !e.isDead() && (e.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController w) && w.isAgressive() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(w.getTarget().getGridX(), w.getTarget().getGridY(), tx, ty) <= 12 * 12)
                {
                    wave.add(e);
                }
            }
            ai.aiLog().count("raid_wave_seen");
            if (wave.size() < com.oddlabs.tt.player.gauntlet.Economy.RAID_WAVE_MIN)
            {
                ai.aiLog().count("raid_wave_small");
                return;
            }
            if (evacuating.containsKey(target))
            {
                ai.aiLog().count("raid_wave_emptying");
                return;
            }
            com.oddlabs.tt.model.Building a_2 = target;
            raids.removeIf(new java.util.function.Predicate_L((object r_l) =>
            {
                com.oddlabs.tt.player.gauntlet.Economy.Raid r = (com.oddlabs.tt.player.gauntlet.Economy.Raid)r_l;
                return ((object)r.armory == (object)a_2) && ((object)r.copy == (object)copy);
            }));
            raids.add(new com.oddlabs.tt.player.gauntlet.Economy.Raid(a_2, copy, wave, tx, ty));
            if (ai.logging())
            {
                int inside = a_2.getUnitContainer().getNumSupplies();
                float strength = com.oddlabs.tt.player.gauntlet.Combat.total(wave);
                ai.aiLog().log("RAID", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("wave of %s (%d warriors, strength %.0f) to %d,%d goes for the armory at %d,%d (%d inside)", copy.getPlayerInfo().getName(), wave.size(), strength, tx, ty, a_2.getGridX(), a_2.getGridY(), inside);
                }));
            }
        }
        // Java: 450
        public void raidEvacuate()
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            for (java.util.Iterator it = raids.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.gauntlet.Economy.Raid r = ((com.oddlabs.tt.player.gauntlet.Economy.Raid)it.next());
                com.oddlabs.tt.model.Building a = r.armory;
                if (a.isDead() || evacuating.containsKey(a))
                {
                    it.remove();
                    continue;
                }
                java.util.List moving = new java.util.ArrayList();
                int front = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(r.wave))
                {
                    if (u.isDead() || !((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController w)))
                    {
                        continue;
                    }
                    int tx = w.getTarget().getGridX();
                    int ty = w.getTarget().getGridY();
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(tx, ty, r.tx, r.ty) > 12 * 12)
                    {
                        continue;
                    }
                    moving.add(u);
                    front = java.lang.Math.min(front, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), r.tx, r.ty));
                }
                if (moving.isEmpty() || 2 * moving.size() < com.oddlabs.tt.player.gauntlet.Economy.RAID_WAVE_MIN)
                {
                    it.remove();
                    ai.aiLog().count("raid_evac_gone");
                    continue;
                }
                float eta = (float)(((float)(java.lang.Math.sqrt(front))) / com.oddlabs.tt.player.gauntlet.Economy.WAVE_SPEED);
                if (eta > com.oddlabs.tt.player.gauntlet.Economy.RAID_LEAD)
                {
                    continue;
                }
                it.remove();
                if (eta < com.oddlabs.tt.player.gauntlet.Economy.RAID_LATE)
                {
                    ai.aiLog().count("raid_evac_late");
                    continue;
                }
                if (a.getUnitContainer().getNumSupplies() < st.raid_evac_min)
                {
                    ai.aiLog().count("raid_evac_empty");
                    continue;
                }
                if (ai.time() < st.raid_evac_time)
                {
                    ai.aiLog().count("raid_evac_early");
                    continue;
                }
                int ax = a.getGridX();
                int ay = a.getGridY();
                float defence = (float)(com.oddlabs.tt.player.gauntlet.Combat.strengthNear(intel.towers, ax, ay, 16) + com.oddlabs.tt.player.gauntlet.Combat.strengthNear(intel.warriors, ax, ay, 20));
                float strength = com.oddlabs.tt.player.gauntlet.Combat.total(moving);
                if (strength < (float)(st.raid_evac_ratio * defence))
                {
                    ai.aiLog().count("raid_evac_defended");
                    ai.aiLog().log("RAID", new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("armory at %d,%d stays: the wave of %s (%d, strength %.0f, %.0f s out) against a defence of %.0f", ax, ay, r.copy.getPlayerInfo().getName(), moving.size(), strength, eta, defence);
                    }));
                    continue;
                }
                startRaidEvac(a, r, moving, eta, strength, defence);
            }
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(raid_evacs.keySet()))
            {
                if (!b.isDead() && evacuating.containsKey(b))
                {
                    continueEvacuation(b, b.getUnitContainer().getNumSupplies());
                }
            }
        }
        // Java: 521
        public void startRaidEvac(com.oddlabs.tt.model.Building a, com.oddlabs.tt.player.gauntlet.Economy.Raid r, java.util.List wave, float eta, float strength, float defence)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            float now = ai.time();
            int inside = a.getUnitContainer().getNumSupplies();
            int weapons = com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) + com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) + com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            com.oddlabs.tt.player.gauntlet.Economy.RaidEvac ev = new com.oddlabs.tt.player.gauntlet.Economy.RaidEvac();
            com.oddlabs.tt.model.Building home = homeArmory(a);
            string to = default;
            if (((object)home != (object)a) && !evacuating.containsKey(home) && !retire.isDoomed(home) && !ai.military().threatNear(home.getGridX(), home.getGridY(), 16))
            {
                owner.setRallyPoint(a, home.getGridX(), home.getGridY());
                ev.refuge = home;
                to = "home";
            }
            else
            {
                com.oddlabs.tt.model.Building q = reserveQuarters();
                if (q != null)
                {
                    owner.setRallyPoint(a, q);
                    ev.shelter = q;
                    holdEvacuees(q, java.lang.Math.max(0, inside - weapons));
                    to = "quarters";
                }
                else
                {
                    long wx = 0;
                    long wy = 0;
                    foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(wave))
                    {
                        wx += u.getGridX();
                        wy += u.getGridY();
                    }
                    float dx = (float)(a.getGridX() - (float)(wx / ((float)(wave.size()))));
                    float dy = (float)(a.getGridY() - (float)(wy / ((float)(wave.size()))));
                    float len = java.lang.Math.max(1.0f, ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy))))));
                    int size = ai.map().getSize();
                    int rx = java.lang.Math.max(3, java.lang.Math.min(size - 4, a.getGridX() + java.lang.Math.round((float)((float)(18 * dx) / len))));
                    int ry = java.lang.Math.max(3, java.lang.Math.min(size - 4, a.getGridY() + java.lang.Math.round((float)((float)(18 * dy) / len))));
                    owner.setRallyPoint(a, rx, ry);
                    to = "away";
                }
            }
            evacuating.put(a, now);
            raid_evacs.put(a, ev);
            ai.aiLog().count("raid_evac_start");
            ai.aiLog().count("raid_evac_to_" + java.lang.JStr.valueOf(to));
            ai.aiLog().count((((object)home == (object)a) ? "raid_evac_homearmory" : "raid_evac_forward"));
            for (int i = 0; i < inside; i++)
            {
                ai.aiLog().count("raid_evac_units");
            }
            ai.aiLog().log("RAID", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("emptying the armory at %d,%d (%d inside, %d weapons) to %s: wave of %s (%d, strength %.0f) %.0f s out, defence %.0f", a.getGridX(), a.getGridY(), inside, weapons, to, r.copy.getPlayerInfo().getName(), wave.size(), strength, eta, defence);
            }));
        }
        // Java: 573
        public void holdEvacuees(com.oddlabs.tt.model.Building q, int n)
        {
            float[] h = ((float[])raid_hold.get(q));
            if (h == null)
            {
                raid_hold.put(q, new float[] { n, (float)(ai.time() + 60.0f) });
            }
            else
            {
                h[0] += n;
                h[1] = (float)(ai.time() + 60.0f);
            }
        }
        // Java: 584
        public int evacueesHeld(com.oddlabs.tt.model.Building q)
        {
            float[] h = ((float[])raid_hold.get(q));
            return (h == null || ai.time() >= h[1] ? 0 : java.lang.JC.f2i(h[0]));
        }
        // Java: 590
        public bool raidEvacuating(com.oddlabs.tt.model.Building a)
        {
            return a != null && raid_evacs.containsKey(a) && evacuating.containsKey(a);
        }
        // Java: 595
        public bool raidRefuge(com.oddlabs.tt.model.Building a)
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.RaidEvac ev in java.lang.JIter.of(raid_evacs.values()))
            {
                if (((object)ev.refuge == (object)a))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 606
        public void endRaidEvacs()
        {
            float now = ai.time();
            for (java.util.Iterator it = raid_evacs.entrySet().iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)((java.util.Map.Entry)it.next()).getKey());
                float? since = ((float?)evacuating.get(b));
                if (!b.isDead() && since != null && (float)(now - ((float)since)) <= 60.0f)
                {
                    continue;
                }
                it.remove();
                bool razed = b.isDead();
                ai.aiLog().count((razed ? "raid_evac_razed" : "raid_evac_stood"));
                if (!razed)
                {
                    ai.owner().setRallyPoint(b, b);
                }
                ai.aiLog().log("RAID", new java.util.function.Supplier_L(() =>
                {
                    return "the armory emptied at " + (b.getGridX()) + "," + (b.getGridY()) + java.lang.JStr.valueOf(((razed ? " fell" : " stood"))) + java.lang.JStr.valueOf(((since != null ? " (" + (java.lang.JC.f2i(((float)(now - ((float)since))))) + " s)" : "")));
                }));
            }
            raid_hold.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Building)e.getKey()).isDead() || now >= ((float[])e.getValue())[1];
            }));
        }
        // Java: 624
        public void plan()
        {
            checkRush();
            planBuildings();
            computeGatherTargets();
        }
        // Java: 633
        public class Project : java.lang.Object
        {
            public int type;
            public int id;
            public com.oddlabs.tt.player.gauntlet.SitePlanner.Site site;
            public com.oddlabs.tt.model.Building building;
            public int priority;
            public bool first;
            public bool use_scout;
            public bool forward;
            public bool sniper;
            public int failures;
            public float placed_time;
            public float veto_since;
            public int wood_sent;
            public float wood_last;
            public bool reloc;
            public float added;
            public bool @lock;
            public float open_since;
            public com.oddlabs.tt.model.Unit placer;
            public bool evac_skipped;
            public float noplacer_since;
            // Java: 678
            public bool isPlaced()
            {
                return building != null && !building.isDead() && building.isPlaced();
            }
            // Java: 682
            public string describe()
            {
                string name = ((type) switch { 0 => (string)("quarters"), 1 => (string)("armory"), _ => (string)((sniper ? "sniper tower" : (forward ? "forward tower" : "tower"))) });
                return java.lang.JStr.valueOf(name) + "#" + (id) + " at " + (site.x) + "," + (site.y);
            }
            private void init_Project()
            {
                this.placed_time = (float)(-1.0f);
                this.veto_since = (float)(-1.0f);
                this.wood_last = (float)(-100.0f);
                this.added = (float)(-1.0f);
                this.open_since = (float)(-1.0f);
                this.noplacer_since = (float)(-1.0f);
            }
            public Project(int type, int id, com.oddlabs.tt.player.gauntlet.SitePlanner.Site site, int priority) : base()
            {
                init_Project();
                this.type = type;
                this.id = id;
                this.site = site;
                this.priority = priority;
            }
        }
        // Java: 693
        public com.oddlabs.tt.player.gauntlet.Economy.Project addProject(int type, com.oddlabs.tt.player.gauntlet.SitePlanner.Site site, int priority)
        {
            site.withHalf(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.of(type));
            com.oddlabs.tt.player.gauntlet.Economy.Project p = new com.oddlabs.tt.player.gauntlet.Economy.Project(type, project_counter++, site, priority);
            ai.log("plan " + java.lang.JStr.valueOf(p.describe()) + " trees=" + (ai.map().treesAround(site.x, site.y, 7)) + " from start=" + (ai.planner().getStartField().get(site.x, site.y)) + "m");
            int i = 0;
            while (i < projects.size() && ((com.oddlabs.tt.player.gauntlet.Economy.Project)projects.get(i)).priority <= priority)
            {
                i++;
            }
            projects.add(i, p);
            return p;
        }
        // Java: 706
        public void place(com.oddlabs.tt.player.gauntlet.Economy.Project p, java.util.List builders)
        {
            p.building = ai.placeSite(builders, p.type, p.site.x, p.site.y);
        }
        // Java: 710
        public void order(java.util.List units, com.oddlabs.tt.model.Building target, com.oddlabs.tt.model.Action action)
        {
            if (target == null || units.isEmpty())
            {
                return;
            }
            ai.owner().setTarget(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), target, action, false);
        }
        // Java: 716
        public void order(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building target, com.oddlabs.tt.model.Action action)
        {
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(unit), target, action, false);
        }
        // Java: 720
        public int builderCount(com.oddlabs.tt.model.Building building)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(ai.intel().builder_sites.values()))
            {
                if (((object)b == (object)building))
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 728
        public com.oddlabs.tt.model.Unit placerOf(com.oddlabs.tt.model.Building building)
        {
            foreach (java.util.Map.Entry e in java.lang.JIter.of(ai.intel().builder_sites.entrySet()))
            {
                if (((object)((com.oddlabs.tt.model.Building)e.getValue()) == (object)building))
                {
                    return ((com.oddlabs.tt.model.Unit)e.getKey());
                }
            }
            return null;
        }
        // Java: 735
        public void manageProjects()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            for (java.util.Iterator it = projects.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.gauntlet.Economy.Project p = ((com.oddlabs.tt.player.gauntlet.Economy.Project)it.next());
                if (p.building != null && p.building.isDead())
                {
                    it.remove();
                    continue;
                }
                if (p.isPlaced())
                {
                    if (p.placed_time < 0)
                    {
                        p.placed_time = ai.time();
                        ai.log("placed " + java.lang.JStr.valueOf(p.describe()));
                        if (p.@lock)
                        {
                            ai.aiLog().count("lock_placed");
                        }
                        if ((p.reloc || p.@lock) && ai.strategy().retire)
                        {
                            retire.flaggedPlaced();
                        }
                    }
                    if (p.building.isComplete())
                    {
                        it.remove();
                        ai.log("completed " + java.lang.JStr.valueOf(p.describe()) + " after " + (java.lang.JC.f2i(((float)(ai.time() - p.placed_time)))) + "s");
                        if (p.wood_sent > 0)
                        {
                            com.oddlabs.tt.player.gauntlet.Economy.Project q = p;
                            ai.aiLog().log("WOOD", new java.util.function.Supplier_L(() =>
                            {
                                return "completed " + java.lang.JStr.valueOf(q.describe()) + " after " + (java.lang.JC.f2i(((float)(ai.time() - q.placed_time)))) + "s with " + (q.wood_sent) + " wood sent";
                            }));
                        }
                        onCompleted(p);
                    }
                    continue;
                }
                if (p.building != null && placerOf(p.building) != null)
                {
                    continue;
                }
                if (p.building != null)
                {
                    if (p.placer != null)
                    {
                        placerFailed(p);
                    }
                    p.failures++;
                    p.building = null;
                    if (p.failures > 6 || p.sniper)
                    {
                        it.remove();
                        continue;
                    }
                    com.oddlabs.tt.player.gauntlet.SitePlanner.Site moved = resite(p);
                    if (moved == null)
                    {
                        it.remove();
                        continue;
                    }
                    p.site = moved;
                }
                if (!projectMayStart(p))
                {
                    if (last_veto == com.oddlabs.tt.player.gauntlet.Economy.VETO_THREAT && resiteEligible(p))
                    {
                        vetoResite(p, it);
                    }
                    continue;
                }
                p.veto_since = (float)(-1.0f);
                com.oddlabs.tt.model.Unit placer = choosePlacer(p);
                if (placer == null)
                {
                    if (p.noplacer_since >= 0.0f && (float)(ai.time() - p.noplacer_since) > com.oddlabs.tt.player.gauntlet.Economy.REARM_WAIT && isRebuild(p))
                    {
                        it.remove();
                        ai.aiLog().count("rearm_placer_timeout");
                        ai.log("drop " + java.lang.JStr.valueOf(p.describe()) + ": no safe placer for " + (java.lang.JC.f2i(com.oddlabs.tt.player.gauntlet.Economy.REARM_WAIT)) + "s");
                    }
                    continue;
                }
                place(p, java.util.List_.of(placer));
                if (p.building != null)
                {
                    intel.builder_sites.put(placer, p.building);
                }
                intel.peon_states.put(placer, com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD);
                if ((ai.strategy().rearm_placer || p.@lock) && p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY && p.building != null)
                {
                    p.placer = placer;
                }
            }
        }
        // Java: 812
        public void placerFailed(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            com.oddlabs.tt.model.Unit u = p.placer;
            p.placer = null;
            if (u == null)
            {
                return;
            }
            if (u.isDead())
            {
                ai.aiLog().count("rearm_placer_died");
                if (p.evac_skipped)
                {
                    ai.aiLog().count("rearm_placer_died_exempt");
                }
            }
            else
            {
                ai.aiLog().count("rearm_placer_lost");
            }
            if (ai.logging())
            {
                ai.log("armory placer for " + java.lang.JStr.valueOf(p.describe()) + java.lang.JStr.valueOf(((u.isDead() ? " died" : " re-ordered: " + java.lang.JStr.valueOf(((com.oddlabs.tt.player.gauntlet.Intel.PeonState)ai.intel().peon_states.get(u)))))));
            }
        }
        // Java: 829
        public bool projectMayStart(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_NONE;
            if (p.use_scout)
            {
                return true;
            }
            if ((p.reloc || p.@lock) && !ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY))
            {
                ai.aiLog().count((p.@lock ? "lock_slot_wait" : "reloc_slot_wait"));
                last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_SITES;
                return false;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project waiter = (!p.reloc && !p.@lock ? slotWaiter() : (com.oddlabs.tt.player.gauntlet.Economy.Project)(null));
            if (waiter != null)
            {
                string what = (p.type == com.oddlabs.tt.model.Race.BUILDING_TOWER ? "tower" : "other");
                ai.aiLog().count(java.lang.JStr.valueOf(((waiter.@lock ? "lock_slot_" : "reloc_slot_"))) + java.lang.JStr.valueOf(what) + "_wait");
                last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_SITES;
                return false;
            }
            if (p.sniper)
            {
                return sniperSafe(p.site.x, p.site.y);
            }
            if (ai.military().threatNearEcon(p.site.x, p.site.y, 16))
            {
                last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_THREAT;
                return false;
            }
            if (ai.strategy().tower_cooldown && p.type == com.oddlabs.tt.model.Race.BUILDING_TOWER && recentlyRazedNear(p.site.x, p.site.y))
            {
                ai.aiLog().count("tower_cooldown_skip");
                last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_COOLDOWN;
                return false;
            }
            if (p.forward)
            {
                bool arrived = ai.military().escortArrived(p.site.x, p.site.y);
                if (!arrived)
                {
                    last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_ESCORT;
                }
                return arrived;
            }
            int placed_incomplete = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (((object)q != (object)p) && q.isPlaced() && q.type != com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    placed_incomplete++;
                }
            }
            int sites = (ai.time() >= ai.strategy().tower_parallel_late_time ? ai.strategy().sites_parallel_late : ai.strategy().sites_parallel);
            bool may = p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY || placed_incomplete < sites;
            if (may && ai.strategy().site_towers_first && p.type == com.oddlabs.tt.model.Race.BUILDING_QUARTERS && placed_incomplete == sites - 1 && ai.time() >= ai.strategy().tower_parallel_late_time && ai.intel().quarters.size() >= ai.strategy().tower_min_quarters)
            {
                com.oddlabs.tt.player.gauntlet.Economy.Project tower = waitingTower();
                if (tower != null)
                {
                    may = false;
                    ai.aiLog().count("site_towers_first_used");
                    ai.aiLog().log("SITES", new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JStr.valueOf(p.describe()) + " leaves the last site slot to " + java.lang.JStr.valueOf(tower.describe());
                    }));
                }
            }
            if (!may)
            {
                last_veto = com.oddlabs.tt.player.gauntlet.Economy.VETO_SITES;
            }
            return may;
        }
        // Java: 894
        public com.oddlabs.tt.player.gauntlet.Economy.Project waitingTower()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (q.type == com.oddlabs.tt.model.Race.BUILDING_TOWER && !q.isPlaced() && !q.forward && !q.sniper && !q.use_scout && !ai.military().threatNearEcon(q.site.x, q.site.y, 16))
                {
                    return q;
                }
            }
            return null;
        }
        public const int VETO_NONE = 0;
        public const int VETO_THREAT = 1;
        public const int VETO_COOLDOWN = 2;
        public const int VETO_ESCORT = 3;
        public const int VETO_SITES = 4;
        public int last_veto;
        public float tower_hold_until;
        public float quarters_hold_until;
        // Java: 916
        public bool resiteEligible(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            return st.veto_resite > 0.0f && ai.time() >= st.veto_resite_time && p.building == null && !p.forward && !p.sniper && !p.use_scout && (p.type == com.oddlabs.tt.model.Race.BUILDING_TOWER || (p.type == com.oddlabs.tt.model.Race.BUILDING_QUARTERS && st.veto_resite_quarters));
        }
        // Java: 929
        public void vetoResite(com.oddlabs.tt.player.gauntlet.Economy.Project p, java.util.Iterator it)
        {
            float now = ai.time();
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            bool tower = p.type == com.oddlabs.tt.model.Race.BUILDING_TOWER;
            if (p.veto_since < 0.0f)
            {
                p.veto_since = now;
            }
            ai.aiLog().count((tower ? "tower_veto_ticks" : "quarters_veto_ticks"));
            if ((float)(now - p.veto_since) < st.veto_resite)
            {
                return;
            }
            int vetoed = java.lang.JC.f2i(((float)(now - p.veto_since)));
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site s = clearSite(p.type, p.site.x, p.site.y, p);
            if (s != null)
            {
                ai.log("re-site " + java.lang.JStr.valueOf(p.describe()) + " to " + (s.x) + "," + (s.y) + " (vetoed " + (vetoed) + "s)");
                p.site = s.withHalf(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.of(p.type));
                p.veto_since = (float)(-1.0f);
                ai.aiLog().count((tower ? "veto_resite" : "veto_resite_quarters"));
            }
            else
            {
                ai.log("drop " + java.lang.JStr.valueOf(p.describe()) + ": vetoed, no clear site");
                it.remove();
                if (tower)
                {
                    tower_hold_until = (float)(now + st.veto_resite);
                }
                else
                {
                    quarters_hold_until = (float)(now + st.veto_resite);
                }
                ai.aiLog().count("veto_resite_drop");
            }
        }
        // Java: 961
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site clearSite(int type, int ox, int oy, com.oddlabs.tt.player.gauntlet.Economy.Project except)
        {
            if (!ai.owner().canBuild(type))
            {
                return null;
            }
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Military m = ai.military();
            com.oddlabs.tt.player.gauntlet.SitePlanner planner = ai.planner();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            bool tower = type == com.oddlabs.tt.model.Race.BUILDING_TOWER;
            com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok = new com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk_L((int x, int y) =>
            {
                return !m.threatNearEcon(x, y, st.veto_resite_clear) && !(st.tower_cooldown && tower && recentlyRazedNear(x, y));
            });
            java.util.List reserved = reservedSites(except);
            java.util.List anchors = new java.util.ArrayList();
            anchors.add(new int[] { ox, oy });
            com.oddlabs.tt.model.Building primary = intel.armory();
            if (primary != null)
            {
                com.oddlabs.tt.model.Building home = homeArmory(primary);
                anchors.add(new int[] { home.getGridX(), home.getGridY() });
                if (((object)home != (object)primary))
                {
                    anchors.add(new int[] { primary.getGridX(), primary.getGridY() });
                }
            }
            java.util.List quarters = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (!q.isDead() && q.isComplete())
                {
                    quarters.add(q);
                }
            }
            quarters.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object q_2_l) =>
            {
                com.oddlabs.tt.model.Building q_2 = (com.oddlabs.tt.model.Building)q_2_l;
                return com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q_2.getGridX(), q_2.getGridY(), ox, oy);
            })));
            foreach (com.oddlabs.tt.model.Building q_3 in java.lang.JIter.of(quarters))
            {
                anchors.add(new int[] { q_3.getGridX(), q_3.getGridY() });
            }
            bool threat = m.baseThreatLevel() > 0;
            int face_x = (threat ? m.threatX() : planner.getEnemyX());
            int face_y = (threat ? m.threatY() : planner.getEnemyY());
            java.util.List existing = (tower ? existingTowers() : java.util.List_.of());
            for (int k = 0; k < anchors.size(); k++)
            {
                int[] a = ((int[])anchors.get(k));
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site s = (tower ? planner.findTowerSite(reserved, a[0], a[1], (k == 0 ? 4 : 7), (k == 0 ? 16 : 15), existing, face_x, face_y, ok) : planner.findQuartersSiteLike(reserved, a[0], a[1], (k == 0 ? 14 : 20), type, ok));
                if (s != null)
                {
                    return s;
                }
            }
            return null;
        }
        public float fallback_next;
        // Java: 1012
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site fallbackTowerSite(int[] center, int[] face, java.util.List existing)
        {
            com.oddlabs.tt.player.gauntlet.SitePlanner planner = ai.planner();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List reserved = reservedSites(null);
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site s = planner.findTowerSite(reserved, center[0], center[1], 4, 20, existing, face[0], face[1]);
            if (s != null)
            {
                return s;
            }
            java.util.List anchors = new java.util.ArrayList();
            com.oddlabs.tt.model.Building primary = intel.armory();
            if (primary != null)
            {
                com.oddlabs.tt.model.Building home = homeArmory(primary);
                anchors.add(new int[] { home.getGridX(), home.getGridY() });
                if (((object)home != (object)primary))
                {
                    anchors.add(new int[] { primary.getGridX(), primary.getGridY() });
                }
            }
            java.util.List quarters = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (!q.isDead() && q.isComplete())
                {
                    quarters.add(q);
                }
            }
            quarters.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object q_2_l) =>
            {
                com.oddlabs.tt.model.Building q_2 = (com.oddlabs.tt.model.Building)q_2_l;
                return com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q_2.getGridX(), q_2.getGridY(), center[0], center[1]);
            })));
            foreach (com.oddlabs.tt.model.Building q_3 in java.lang.JIter.of(quarters))
            {
                anchors.add(new int[] { q_3.getGridX(), q_3.getGridY() });
            }
            for (int pass = 0; pass < 2; pass++)
            {
                foreach (int[] a in java.lang.JIter.of(anchors))
                {
                    s = planner.findTowerSite(reserved, a[0], a[1], (pass == 0 ? 7 : 4), (pass == 0 ? 15 : 20), existing, face[0], face[1]);
                    if (s != null)
                    {
                        return s;
                    }
                }
            }
            return null;
        }
        // Java: 1048
        public java.util.List existingTowers()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List existing = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
            {
                existing.add(new int[] { t.getGridX(), t.getGridY() });
            }
            foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(intel.tower_sites))
            {
                existing.add(new int[] { t_2.getGridX(), t_2.getGridY() });
            }
            return existing;
        }
        public java.util.Set standing;
        public java.util.List razings;
        public java.util.List razings_long;
        // Java: 1064
        public void trackRazings()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            for (java.util.Iterator it = standing.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)it.next());
                if (b.isDead())
                {
                    float[] r = new float[] { b.getGridX(), b.getGridY(), ai.time() };
                    razings.add(r);
                    razings_long.add(r);
                    it.remove();
                }
            }
            razings.removeIf(new java.util.function.Predicate_L((object r_2_l) =>
            {
                float[] r_2 = (float[])r_2_l;
                return (float)(ai.time() - r_2[2]) > 90.0f;
            }));
            razings_long.removeIf(new java.util.function.Predicate_L((object r_3_l) =>
            {
                float[] r_3 = (float[])r_3_l;
                return (float)(ai.time() - r_3[2]) > 180.0f;
            }));
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.quarters, intel.armories, intel.towers, intel.quarters_sites, intel.armory_sites, intel.tower_sites)))
            {
                foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(group))
                {
                    if (!b_2.isDead())
                    {
                        standing.add(b_2);
                    }
                }
            }
        }
        // Java: 1084
        public bool recentlyRazedNear(int x, int y)
        {
            foreach (float[] r in java.lang.JIter.of(razings))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, java.lang.JC.f2i(r[0]), java.lang.JC.f2i(r[1])) <= 25 * 25)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1096
        public bool quietOk(int x, int y)
        {
            com.oddlabs.tt.player.gauntlet.Military m = ai.military();
            return !m.threatNear(x, y, 30) && m.enemyStrengthNear(x, y, 30) == 0.0f && !razedNear(x, y);
        }
        // Java: 1102
        public bool razedNear(int x, int y)
        {
            foreach (float[] r in java.lang.JIter.of(razings_long))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, java.lang.JC.f2i(r[0]), java.lang.JC.f2i(r[1])) <= 25 * 25)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1110
        public bool awakeEnemyNear(int x, int y, int r)
        {
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                if (e.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e.getGridX(), e.getGridY()) > r * r)
                {
                    continue;
                }
                if (!com.oddlabs.tt.player.gauntlet.Intel.isParked(e))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1120
        public void onCompleted(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            if (((object)p == (object)expansion_project))
            {
                expansion = p.building;
            }
            if (p.forward && p.building != null)
            {
                forward_towers.add(p.building);
            }
            if (p.sniper && p.building != null)
            {
                sniper_towers.add(p.building);
            }
            if (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY && p.building != null)
            {
                had_armory = true;
                com.oddlabs.tt.model.Building armory = p.building;
                ai.owner().buildIronWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                if (ai.owner().canUseRubber())
                {
                    ai.owner().buildRubberWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                }
            }
        }
        // Java: 1136
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site resite(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            com.oddlabs.tt.player.gauntlet.SitePlanner planner = ai.planner();
            java.util.List reserved = reservedSites(p);
            {
                switch ((p.type))
                {
                    case 1:
                    {
                        com.oddlabs.tt.player.gauntlet.SitePlanner.Site s = planner.findQuartersSiteLike(reserved, p.site.x, p.site.y, 12, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                        return (s != null ? s : planner.findArmorySite(reserved));
                    }
                    case 0:
                    {
                        return planner.findQuartersSiteLike(reserved, p.site.x, p.site.y, 14, com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
                    }
                    default:
                    {
                        return planner.findQuartersSiteLike(reserved, p.site.x, p.site.y, 10, com.oddlabs.tt.model.Race.BUILDING_TOWER);
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 1150
        public java.util.List reservedSites(com.oddlabs.tt.player.gauntlet.Economy.Project except)
        {
            java.util.List reserved = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (((object)q != (object)except))
                {
                    reserved.add(q.site);
                }
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.quarters);
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.armories);
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.towers);
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.quarters_sites);
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.armory_sites);
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.tower_sites);
            com.oddlabs.tt.player.gauntlet.Economy.addBuildings(reserved, intel.decoy_sites);
            return reserved;
        }
        // Java: 1166
        public static void addBuildings(java.util.List reserved, java.util.List buildings)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                reserved.add(new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(b.getGridX(), b.getGridY(), 0).withHalf(com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.of(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID())));
            }
        }
        // Java: 1173
        public com.oddlabs.tt.model.Unit choosePlacer(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            if (p.use_scout && scout != null && !scout.isDead())
            {
                foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
                {
                    if (((object)q == (object)p))
                    {
                        break;
                    }
                    if (q.use_scout && !q.isPlaced() && q.building != null && ((object)placerOf(q.building) == (object)scout))
                    {
                        return null;
                    }
                }
                return scout;
            }
            if (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY && (ai.strategy().rearm_placer || p.@lock))
            {
                return safePlacer(p);
            }
            return plainPlacer(p);
        }
        // Java: 1194
        public com.oddlabs.tt.model.Unit plainPlacer(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon));
                if (((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), p.site.x, p.site.y);
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE))
                {
                    d += 40 * 40;
                }
                if (d < best_d)
                {
                    best_d = d;
                    best = peon;
                }
            }
            if (best == null && p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
            {
                foreach (com.oddlabs.tt.model.Unit peon_2 in java.lang.JIter.of(intel.peons))
                {
                    if (((object)((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon_2)) == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD))
                    {
                        best = peon_2;
                        break;
                    }
                }
            }
            return best;
        }
        public com.oddlabs.tt.model.Building reserve_quarters;
        public float reserve_until;
        public float rearm_deploy_next;
        public java.util.List reserve_known;
        public const float RESERVE_SECONDS = 3.0f;
        public const int RESERVE_CELLS = 10;
        // Java: 1245
        public com.oddlabs.tt.model.Unit safePlacer(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            float now = ai.time();
            com.oddlabs.tt.model.Unit u = safePlacerOf(p, now);
            bool timed = !p.reloc && !p.@lock;
            if (u != null || !timed || now < rearm_deploy_next)
            {
                p.noplacer_since = (float)(-1.0f);
            }
            else if (p.noplacer_since < 0.0f)
            {
                p.noplacer_since = now;
            }
            if (u == null && !isRebuild(p) && p.noplacer_since >= 0.0f && (float)(now - p.noplacer_since) > com.oddlabs.tt.player.gauntlet.Economy.REARM_WAIT)
            {
                u = plainPlacer(p);
                if (u != null)
                {
                    p.noplacer_since = (float)(-1.0f);
                    ai.aiLog().count("rearm_placer_fallback");
                }
            }
            return u;
        }
        // Java: 1268
        public bool isRebuild(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            return had_armory && ai.intel().armories.isEmpty() && !p.reloc && !p.@lock;
        }
        public const float REARM_WAIT = 30.0f;
        // Java: 1278
        public com.oddlabs.tt.model.Unit safePlacerOf(com.oddlabs.tt.player.gauntlet.Economy.Project p, float now)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int sx = p.site.x;
            int sy = p.site.y;
            com.oddlabs.tt.model.Building rq = reserve_quarters;
            if (rq != null && (rq.isDead() || now > reserve_until))
            {
                reserve_quarters = null;
                rq = null;
            }
            if (rq != null)
            {
                com.oddlabs.tt.model.Unit mine = null;
                int best_d = com.oddlabs.tt.player.gauntlet.Economy.RESERVE_CELLS * com.oddlabs.tt.player.gauntlet.Economy.RESERVE_CELLS + 1;
                foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
                {
                    com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon));
                    if ((((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT) && ((object)s != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE)) || reserve_known.contains(peon))
                    {
                        continue;
                    }
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), rq.getGridX(), rq.getGridY());
                    if (d < best_d && safeWay(peon, sx, sy, 12))
                    {
                        best_d = d;
                        mine = peon;
                    }
                }
                if (mine != null)
                {
                    reserve_quarters = null;
                    ai.aiLog().count("rearm_placer_reserved");
                    return mine;
                }
            }
            com.oddlabs.tt.model.Unit safe = nearestSafe(p, java.util.EnumSet.of(com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE), 12);
            if (safe == null)
            {
                safe = nearestSafe(p, java.util.EnumSet.of(com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT), 12);
            }
            if (safe != null)
            {
                bool transit = ((object)((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(safe)) == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT);
                ai.aiLog().count((transit ? "rearm_placer_transit" : "rearm_placer_safe"));
                return safe;
            }
            if (rq != null || now < rearm_deploy_next)
            {
                ai.aiLog().count("rearm_placer_wait");
                return null;
            }
            com.oddlabs.tt.model.Building from = deployQuarters(sx, sy);
            if (from != null)
            {
                ai.owner().deployUnits(from, com.oddlabs.tt.model.DeployType.PEON, 1);
                reserve_known.clear();
                foreach (com.oddlabs.tt.model.Unit peon_2 in java.lang.JIter.of(intel.peons))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(peon_2.getGridX(), peon_2.getGridY(), from.getGridX(), from.getGridY()) <= com.oddlabs.tt.player.gauntlet.Economy.RESERVE_CELLS * com.oddlabs.tt.player.gauntlet.Economy.RESERVE_CELLS)
                    {
                        reserve_known.add(peon_2);
                    }
                }
                reserve_quarters = from;
                reserve_until = (float)(now + com.oddlabs.tt.player.gauntlet.Economy.RESERVE_SECONDS);
                rearm_deploy_next = (float)(now + 5.0f);
                ai.aiLog().count("rearm_placer_deploy");
                if (ai.logging())
                {
                    ai.log("armory placer for " + java.lang.JStr.valueOf(p.describe()) + ": one peon out of the quarters at " + (from.getGridX()) + "," + (from.getGridY()));
                }
                return null;
            }
            safe = nearestSafe(p, java.util.EnumSet.of(com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD), 12);
            if (safe != null)
            {
                ai.aiLog().count("rearm_placer_build");
                return safe;
            }
            safe = nearestSafe(p, java.util.EnumSet.of(com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.MOVE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK, com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT, com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD), 8);
            if (safe != null)
            {
                ai.aiLog().count("rearm_placer_relaxed");
                return safe;
            }
            if (intel.armories.isEmpty() && !intel.quarters.isEmpty())
            {
                com.oddlabs.tt.model.Unit shepherd = null;
                int best_d_2 = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(intel.shepherds))
                {
                    if (u.isDead() || u.isMounted() || com.oddlabs.tt.player.gauntlet.Intel.isStunned(u))
                    {
                        continue;
                    }
                    int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), sx, sy);
                    if (d_2 < best_d_2 && safeWay(u, sx, sy, 12))
                    {
                        best_d_2 = d_2;
                        shepherd = u;
                    }
                }
                if (shepherd != null && ai.shepherd().giveUp(shepherd))
                {
                    ai.aiLog().count("rearm_placer_shepherd");
                    return shepherd;
                }
            }
            if (!intel.peons.isEmpty())
            {
                ai.aiLog().count("rearm_placer_none");
                if (ai.logging() && now >= rearm_none_log)
                {
                    rearm_none_log = (float)(now + 15.0f);
                    java.util.Map states = new java.util.EnumMap(typeof(com.oddlabs.tt.player.gauntlet.Intel.PeonState));
                    foreach (com.oddlabs.tt.player.gauntlet.Intel.PeonState s_2 in java.lang.JIter.of(intel.peon_states.values()))
                    {
                        states.merge(s_2, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                    }
                    java.lang.StringBuilder q = new java.lang.StringBuilder();
                    foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.quarters))
                    {
                        q.append(' ').append(b.getUnitContainer().getNumSupplies()).append((ai.military().threatNearEcon(b.getGridX(), b.getGridY(), 12) ? "t" : "")).append((pathClear(b.getGridX(), b.getGridY(), sx, sy, 12) ? "" : "p"));
                    }
                    ai.aiLog().log("REARM", new java.util.function.Supplier_L(() =>
                    {
                        return "no placer for " + java.lang.JStr.valueOf(p.describe()) + ": peons " + java.lang.JStr.valueOf(states) + ", quarters inside" + java.lang.JStr.valueOf(q) + ", shepherds " + (intel.shepherds.size());
                    }));
                }
            }
            return null;
        }
        public float rearm_none_log;
        // Java: 1392
        public com.oddlabs.tt.model.Unit nearestSafe(com.oddlabs.tt.player.gauntlet.Economy.Project p, java.util.EnumSet states, int way_clear)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int sx = p.site.x;
            int sy = p.site.y;
            java.util.List candidates = new java.util.ArrayList();
            java.util.List dists = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon));
                if (s == null || !states.contains(s))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), sx, sy);
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE))
                {
                    d += 40 * 40;
                }
                int i = 0;
                while (i < dists.size() && ((int)((int?)dists.get(i))) <= d)
                {
                    i++;
                }
                candidates.add(i, peon);
                dists.add(i, d);
            }
            foreach (com.oddlabs.tt.model.Unit peon_2 in java.lang.JIter.of(candidates))
            {
                if (safeWay(peon_2, sx, sy, way_clear))
                {
                    return peon_2;
                }
            }
            return null;
        }
        // Java: 1422
        public com.oddlabs.tt.model.Building deployQuarters(int sx, int sy)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building from = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (q.isDead() || !q.isComplete() || evacuating.containsKey(q) || retire.isDoomed(q) || q.getUnitContainer().getNumSupplies() == 0)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), sx, sy);
                if (d >= best_d || ai.military().threatNearEcon(q.getGridX(), q.getGridY(), 12) || !pathClear(q.getGridX(), q.getGridY(), sx, sy, 12))
                {
                    continue;
                }
                best_d = d;
                from = q;
            }
            return from;
        }
        // Java: 1441
        public bool safeWay(com.oddlabs.tt.model.Unit peon, int x, int y, int r)
        {
            return !ai.military().threatNear(peon.getGridX(), peon.getGridY(), 11) && pathClear(peon.getGridX(), peon.getGridY(), x, y, r);
        }
        // Java: 1451
        public bool reservedPlacer(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.Building q = reserve_quarters;
            return q != null && ai.time() <= reserve_until && !q.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), q.getGridX(), q.getGridY()) <= com.oddlabs.tt.player.gauntlet.Economy.RESERVE_CELLS * com.oddlabs.tt.player.gauntlet.Economy.RESERVE_CELLS && !reserve_known.contains(u);
        }
        // Java: 1463
        public bool evacExempt(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            if (!st.rearm_placer && st.reloc_lock <= 0)
            {
                return false;
            }
            com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)ai.intel().builder_sites.get(u));
            if (b == null || b.isDead() || b.isPlaced() || ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_ARMORY)
            {
                return false;
            }
            bool @lock = false;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                @lock |= ((object)p.building == (object)b) && p.@lock;
            }
            if (!st.rearm_placer && !@lock)
            {
                return false;
            }
            if (ai.military().threatNear(u.getGridX(), u.getGridY(), 6))
            {
                return false;
            }
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p_2 in java.lang.JIter.of(projects))
            {
                if (((object)p_2.building == (object)b))
                {
                    p_2.evac_skipped = true;
                }
            }
            ai.aiLog().count("rearm_evac_skip");
            return true;
        }
        // Java: 1488
        public bool pathClear(int x0, int y0, int x1, int y1, int r)
        {
            float len = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x0, y0, x1, y1))));
            int steps = java.lang.Math.max(1, java.lang.JC.f2i(((float)(len / 6.0f))));
            com.oddlabs.tt.player.gauntlet.EnemyIndex index = ai.intel().enemyIndex(ai.ticks());
            for (int i = steps; i >= 0; i--)
            {
                int px = x0 + java.lang.Math.round((float)((x1 - x0) * i / ((float)(steps))));
                int py = y0 + java.lang.Math.round((float)((y1 - y0) * i / ((float)(steps))));
                int[] found = index.queryUnordered(px, py, r * r);
                for (int k = 0; k < index.count(); k++)
                {
                    int e = found[k];
                    if (index.group(e) == com.oddlabs.tt.player.gauntlet.EnemyIndex.WARRIOR && !index.unit(e).isDead())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // Java: 1506
        public int buildersWanted(com.oddlabs.tt.player.gauntlet.Economy.Project p)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            bool have_quarters = !intel.quarters.isEmpty();
            bool have_armory = !intel.armories.isEmpty();
            if (p.first)
            {
                return com.oddlabs.tt.player.gauntlet.Economy.MAX_BUILDERS;
            }
            if (!have_quarters && hasFirstQuarters())
            {
                return 0;
            }
            int trees = ai.map().treesAround(p.site.x, p.site.y, 12);
            int feedable = java.lang.Math.max(6, 4 * trees);
            int cap = ((p.type) switch { 1 => (int)((have_armory ? 12 : com.oddlabs.tt.player.gauntlet.Economy.MAX_BUILDERS)), 0 => (int)((armsRace() ? 3 : (have_armory ? strategy.quarters_builders : com.oddlabs.tt.player.gauntlet.Economy.MAX_BUILDERS))), _ => (int)(strategy.tower_builders) });
            return java.lang.Math.min(cap, feedable);
        }
        // Java: 1526
        public bool hasFirstQuarters()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.first)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1536
        public void planBuildings()
        {
            if (!planArmory())
            {
                return;
            }
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            planQuarters(armory);
            planTowers(armory);
        }
        // Java: 1548
        public bool planArmory()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int armory_count = intel.armories.size() + intel.armory_sites.size() + countProjects(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            if (armory_count == 0)
            {
                com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = null;
                bool quiet = false;
                if (had_armory && st.rearm_reach > 0 && ai.time() >= rearm_next)
                {
                    rearm_next = (float)(ai.time() + 10.0f);
                    site = ai.planner().findArmorySite(reservedSites(null), st.rearm_reach, ai.planner().getStartField(), new com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk_L((int a0, int a1) => { return this.quietOk(a0, a1); }), rearm_search, ai.time());
                    quiet = site != null;
                    ai.aiLog().count((quiet ? "rearm_site_quiet" : "rearm_site_none"));
                    if (quiet && ai.logging())
                    {
                        ai.log(java.lang.JString.format("rearm: quiet armory site %d,%d, cost %.0f, %d m from the start", site.x, site.y, (float)(-site.score), ai.planner().getStartField().get(site.x, site.y)));
                    }
                }
                if (site == null)
                {
                    site = (had_armory ? safeArmorySite() : ai.planner().findArmorySite(reservedSites(null)));
                }
                if (!quiet && site != null && ai.military().threatNear(site.x, site.y, 25))
                {
                    com.oddlabs.tt.player.gauntlet.SitePlanner.Site alt = (had_armory && st.rearm_placer && ai.time() >= rearm_alt_next ? calmArmorySite() : (com.oddlabs.tt.player.gauntlet.SitePlanner.Site)(null));
                    if (alt == null)
                    {
                        return false;
                    }
                    ai.aiLog().count("rearm_site_alt");
                    site = alt;
                }
                if (site == null)
                {
                    site = ai.planner().findArmorySite(reservedSites(null));
                }
                if (site == null && !intel.peons.isEmpty())
                {
                    com.oddlabs.tt.model.Unit p = ((com.oddlabs.tt.model.Unit)intel.peons.getFirst());
                    site = ai.planner().findQuartersSiteLike(reservedSites(null), p.getGridX(), p.getGridY(), 40, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
                }
                if (site != null)
                {
                    armory_site = site;
                    addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site, 0);
                }
            }
            else if (lock_armed && lockGate())
            {
                lockRelocate();
            }
            else if (ai.strategy().reloc && ai.time() >= ai.strategy().reloc_time && relocGate())
            {
                considerRelocation();
            }
            else if (armory_count == 1 && intel.armories.size() == 1)
            {
                considerExpansion();
            }
            return true;
        }
        public float rearm_next;
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Search rearm_search;
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Search reloc_search;
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Search lock_search;
        public float rearm_alt_next;
        // Java: 1612
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site calmArmorySite()
        {
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            java.util.List quarters = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ai.intel().quarters))
            {
                if (!q.isDead() && q.isComplete())
                {
                    quarters.add(q);
                }
            }
            java.util.List danger = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building q_2 in java.lang.JIter.of(quarters))
            {
                danger.add(military.enemyStrengthNear(q_2.getGridX(), q_2.getGridY(), 40));
            }
            java.util.List reserved = reservedSites(null);
            com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok = new com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk_L((int x, int y) =>
            {
                return !military.threatNear(x, y, 25);
            });
            bool[] tried = new bool[quarters.size()];
            for (int n = 0; n < quarters.size(); n++)
            {
                int k = (-1);
                for (int i = 0; i < quarters.size(); i++)
                {
                    if (!tried[i] && (k < 0 || ((float)((float?)danger.get(i))) < ((float)((float?)danger.get(k)))))
                    {
                        k = i;
                    }
                }
                tried[k] = true;
                com.oddlabs.tt.model.Building q_3 = ((com.oddlabs.tt.model.Building)quarters.get(k));
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site s = ai.planner().findQuartersSiteLike(reserved, q_3.getGridX(), q_3.getGridY(), 24, com.oddlabs.tt.model.Race.BUILDING_ARMORY, ok);
                if (s != null)
                {
                    return s;
                }
            }
            rearm_alt_next = (float)(ai.time() + 10.0f);
            ai.aiLog().count("rearm_site_wait");
            return null;
        }
        // Java: 1643
        public void planQuarters(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            float time = ai.time();
            int quarters_count = intel.quarters.size() + intel.quarters_sites.size() + countProjects(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, false);
            int target_quarters = strategy.initial_quarters;
            if (armory != null && time >= strategy.expand_time)
            {
                target_quarters = strategy.max_quarters;
            }
            int pop = ai.owner().getUnitCountContainer().getNumSupplies();
            if (pop > ai.owner().getWorld().getMaxUnitCount() * 3 / 4)
            {
                target_quarters = java.lang.Math.min(target_quarters, intel.quarters.size());
            }
            if (quarters_count < target_quarters && countProjects(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, true) == 0 && (armory != null || intel.quarters.isEmpty()) && time >= quarters_hold_until)
            {
                int ax = (armory != null ? armory.getGridX() : ai.planner().getStartX());
                int ay = (armory != null ? armory.getGridY() : ai.planner().getStartY());
                com.oddlabs.tt.player.gauntlet.DistanceField field = (armory != null ? armoryField() : ai.planner().getStartField());
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = ai.planner().findQuartersSite(reservedSites(null), ax, ay, 90, field, ai.planner().getStartX(), ai.planner().getStartY(), ai.strategy().quarters_builders, 0.25f, 0.02f);
                if (site != null && strategy.veto_resite > 0.0f && strategy.veto_resite_quarters && time >= strategy.veto_resite_time)
                {
                    site = clearAtBirth(site, com.oddlabs.tt.model.Race.BUILDING_QUARTERS, "veto_resite_born_quarters");
                }
                if (site != null)
                {
                    addProject(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, site, 5);
                }
            }
        }
        // Java: 1674
        public void planTowers(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            float time = ai.time();
            if (armory == null)
            {
                return;
            }
            if (intel.quarters.size() < strategy.tower_min_quarters)
            {
                ai.aiLog().count("tower_gate_quarters");
                return;
            }
            int target_towers = 0;
            if (time >= strategy.towers_early_time)
            {
                target_towers = strategy.towers_early;
            }
            if (time >= strategy.towers_mid_time)
            {
                target_towers = strategy.towers_mid;
            }
            if (time >= strategy.towers_late_time)
            {
                target_towers = strategy.towers_late;
            }
            if (ai.military().baseThreatLevel() > 0 && target_towers < 2)
            {
                target_towers = java.lang.Math.max(target_towers, 1);
            }
            int enemies = ai.enemiesAlive();
            bool fronts = enemies > 1 && strategy.multi_front_towers;
            if (fronts && target_towers > 0)
            {
                target_towers += java.lang.Math.min(enemies - 1, strategy.front_tower_bonus_max);
            }
            if (strategy.tower_cap)
            {
                target_towers = java.lang.Math.min(target_towers, ai.owner().getWorld().getMaxBuildingCount() - strategy.max_quarters - 3);
            }
            forward_towers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            sniper_towers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            int tower_count = intel.towers.size() + intel.tower_sites.size() + countProjects(com.oddlabs.tt.model.Race.BUILDING_TOWER, false) - forward_towers.size() - countForward() - ai.military().creepTowerCount() - sniper_towers.size() - countSniper();
            int tower_parallel = (time >= strategy.tower_parallel_late_time ? strategy.tower_parallel_late : strategy.tower_parallel);
            bool may = tower_count < target_towers && countProjects(com.oddlabs.tt.model.Race.BUILDING_TOWER, true) < tower_parallel && ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER) && time >= tower_hold_until;
            com.oddlabs.tt.player.gauntlet.Economy.Project waiter = (may ? slotWaiter() : (com.oddlabs.tt.player.gauntlet.Economy.Project)(null));
            if (waiter != null)
            {
                may = false;
                ai.aiLog().count((waiter.@lock ? "lock_slot_held" : "reloc_slot_tower_held"));
            }
            if (may)
            {
                java.util.List existing = existingTowers();
                int[] center = towerAnchor(tower_count);
                int[] face = new int[] { ai.planner().getEnemyX(), ai.planner().getEnemyY() };
                float[] live = (strategy.tower_face_place ? ai.liveEnemyCenter() : (float[])(null));
                if (live != null)
                {
                    face = new int[] { java.lang.Math.round(live[0]), java.lang.Math.round(live[1]) };
                }
                int min_cells = 7;
                int max_cells = 15;
                if (fronts && tower_count % 2 == 1)
                {
                    int[][] front = enemyFront(tower_count / 2);
                    if (front != null)
                    {
                        center = front[0];
                        face = front[1];
                        live = null;
                        min_cells = ai.strategy().front_tower_min;
                        max_cells = ai.strategy().front_tower_max;
                    }
                }
                if (live != null)
                {
                    ai.aiLog().count("tower_face_place");
                }
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = ai.planner().findTowerSite(reservedSites(null), center[0], center[1], min_cells, max_cells, existing, face[0], face[1]);
                if (site == null)
                {
                    ai.aiLog().count("tower_nosite");
                    if (strategy.tower_site_fallback && time >= fallback_next)
                    {
                        site = fallbackTowerSite(center, face, existing);
                        if (site != null)
                        {
                            ai.aiLog().count("tower_site_fallback");
                        }
                        else
                        {
                            ai.aiLog().count("tower_site_fallback_none");
                            fallback_next = (float)(time + 15.0f);
                        }
                    }
                }
                if (site != null && strategy.veto_resite > 0.0f && time >= strategy.veto_resite_time)
                {
                    site = clearAtBirth(site, com.oddlabs.tt.model.Race.BUILDING_TOWER, "veto_resite_born");
                }
                if (site != null)
                {
                    addProject(com.oddlabs.tt.model.Race.BUILDING_TOWER, site, 8);
                }
            }
            planForwardTower();
            planSniper();
        }
        // Java: 1763
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site clearAtBirth(com.oddlabs.tt.player.gauntlet.SitePlanner.Site site, int type, string counter)
        {
            if (!ai.military().threatNearEcon(site.x, site.y, 16))
            {
                return site;
            }
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site clear = clearSite(type, site.x, site.y, null);
            ai.aiLog().count((clear != null ? counter : java.lang.JStr.valueOf(counter) + "_none"));
            return (clear != null ? clear : site);
        }
        // Java: 1772
        public int countSniper()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (q.sniper)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1786
        public void planSniper()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!strategy.snipers || (float)(ai.time() - last_sniper) < 2.0f || countSniper() > 0)
            {
                return;
            }
            last_sniper = ai.time();
            com.oddlabs.tt.player.Player owner = ai.owner();
            if (!owner.canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER) || owner.getBuildingCountContainer().getNumSupplies() + 2 >= owner.getWorld().getMaxBuildingCount())
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List own = intel.finishedBuildings();
            int range2 = strategy.snipe_range * strategy.snipe_range;
            java.util.List parked = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead() || !com.oddlabs.tt.player.gauntlet.Intel.isParked(e))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
                {
                    if (!b.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), e.getGridX(), e.getGridY()) <= range2)
                    {
                        parked.add(e);
                        break;
                    }
                }
            }
            com.oddlabs.tt.model.Unit seed = null;
            int best_n = 0;
            foreach (com.oddlabs.tt.model.Unit s in java.lang.JIter.of(parked))
            {
                int n = 0;
                foreach (com.oddlabs.tt.model.Unit e_2 in java.lang.JIter.of(parked))
                {
                    if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(s.getGridX(), s.getGridY(), e_2.getGridX(), e_2.getGridY()) <= 8 * 8)
                    {
                        n++;
                    }
                }
                if (n > best_n)
                {
                    best_n = n;
                    seed = s;
                }
            }
            if (seed == null || best_n < strategy.snipe_min)
            {
                return;
            }
            com.oddlabs.tt.model.BuildingTemplate template = owner.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            java.util.List reserved = reservedSites(null);
            com.oddlabs.tt.model.Building armory = intel.armory();
            int hx = (armory != null ? armory.getGridX() : ai.planner().getStartX());
            int hy = (armory != null ? armory.getGridY() : ai.planner().getStartY());
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site best = null;
            float best_score = (float)(-java.lang.Float.MAX_VALUE);
            int[] rejects = new int[4];
            for (int r = 11; r <= 15; r++)
            {
                for (int a = 0; a < 24; a++)
                {
                    double ang = a * java.lang.Math.PI / 12;
                    int x = seed.getGridX() + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.cos(ang))));
                    int y = seed.getGridY() + unchecked((int)(java.lang.Math.round(r * java.lang.StrictMath.sin(ang))));
                    if (!ai.map().inside(x, y) || !ai.planner().getStartField().reachable(x, y))
                    {
                        continue;
                    }
                    int reach = 0;
                    foreach (com.oddlabs.tt.model.Unit e_3 in java.lang.JIter.of(parked))
                    {
                        if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, e_3.getGridX(), e_3.getGridY()) <= 15 * 15)
                        {
                            reach++;
                        }
                    }
                    if (reach < strategy.snipe_min)
                    {
                        rejects[0]++;
                        continue;
                    }
                    if (!sniperSafe(x, y))
                    {
                        rejects[1]++;
                        continue;
                    }
                    if (!ai.map().canPlace(template, x, y))
                    {
                        rejects[2]++;
                        continue;
                    }
                    if (com.oddlabs.tt.player.gauntlet.SitePlanner.conflicts(reserved, x, y, com.oddlabs.tt.player.gauntlet.SitePlanner.RaceSizes.TOWER))
                    {
                        rejects[3]++;
                        continue;
                    }
                    float score = (float)((float)(reach * 10.0f) - (float)(0.05f * ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, hx, hy))))));
                    if (score > best_score)
                    {
                        best_score = score;
                        best = new com.oddlabs.tt.player.gauntlet.SitePlanner.Site(x, y, score);
                    }
                }
            }
            if (best == null)
            {
                int m = 0;
                for (int i = 1; i < 4; i++)
                {
                    if (rejects[i] > rejects[m])
                    {
                        m = i;
                    }
                }
                ai.aiLog().count("sniper_nospot_" + java.lang.JStr.valueOf(new string[] { "reach", "unsafe", "illegal", "taken" }[m]));
                return;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_TOWER, best, 1);
            p.sniper = true;
            ai.aiLog().count("sniper_planned");
            com.oddlabs.tt.model.Unit s_2 = seed;
            int n_2 = best_n;
            ai.log("sniper tower for " + (n_2) + " parked enemies at " + (s_2.getGridX()) + "," + (s_2.getGridY()));
        }
        // Java: 1882
        public bool sniperSafe(int x, int y)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.enemy_warriors, intel.enemy_chieftains, intel.enemy_peons)))
            {
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(group))
                {
                    if (e.isDead())
                    {
                        continue;
                    }
                    int dx = java.lang.Math.abs(e.getGridX() - x);
                    int dy = java.lang.Math.abs(e.getGridY() - y);
                    if ((com.oddlabs.tt.player.gauntlet.Intel.isParked(e) ? java.lang.Math.max(dx, dy) <= 9 : dx * dx + dy * dy <= 12 * 12))
                    {
                        return false;
                    }
                }
            }
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.enemy_towers))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(t.getGridX(), t.getGridY(), x, y) <= 20 * 20)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 1900
        public void escortForward()
        {
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.forward && military.canEscort())
                {
                    military.escort(p.site.x, p.site.y);
                    return;
                }
            }
        }
        // Java: 1911
        public int countForward()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.forward)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1920
        public void planForwardTower()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            for (java.util.Iterator it = projects.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.gauntlet.Economy.Project p = ((com.oddlabs.tt.player.gauntlet.Economy.Project)it.next());
                if (!p.forward || p.building != null || military.canEscort())
                {
                    continue;
                }
                ai.log("dropping " + java.lang.JStr.valueOf(p.describe()) + ": no cover");
                it.remove();
            }
            if (strategy.forward_towers <= 0 || ai.time() < strategy.forward_tower_time || countForward() > 0 || forward_towers.size() >= strategy.forward_towers || !military.canEscort() || !ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER))
            {
                return;
            }
            int[] spot = military.forwardTarget();
            if (spot == null)
            {
                return;
            }
            java.util.List existing = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(ai.intel().towers))
            {
                existing.add(new int[] { t.getGridX(), t.getGridY() });
            }
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = ai.planner().findTowerSite(reservedSites(null), spot[0], spot[1], 3, 6, existing, ai.planner().getStartX(), ai.planner().getStartY());
            if (site == null)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project p_2 = addProject(com.oddlabs.tt.model.Race.BUILDING_TOWER, site, 3);
            p_2.forward = true;
        }
        // Java: 1949
        public com.oddlabs.tt.player.gauntlet.SitePlanner.Site safeArmorySite()
        {
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            com.oddlabs.tt.model.Building safest = null;
            float least = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ai.intel().quarters))
            {
                float danger = military.enemyStrengthNear(q.getGridX(), q.getGridY(), 40);
                if (danger < least)
                {
                    least = danger;
                    safest = q;
                }
            }
            if (safest == null)
            {
                return null;
            }
            return ai.planner().findQuartersSiteLike(reservedSites(null), safest.getGridX(), safest.getGridY(), 24, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
        }
        // Java: 1970
        public void checkRush()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (rush_alert || !ai.strategy().rush_response || !intel.armories.isEmpty() || (ai.strategy().rush_opening_only && had_armory))
            {
                return;
            }
            int enemy_quarters = 0;
            bool enemy_armory = false;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.enemy_buildings))
            {
                int id = ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID();
                if (id == com.oddlabs.tt.model.Race.BUILDING_QUARTERS)
                {
                    enemy_quarters++;
                }
                else if (id == com.oddlabs.tt.model.Race.BUILDING_ARMORY && b.isComplete())
                {
                    enemy_armory = true;
                }
            }
            if (intel.enemy_warriors.size() < 6 && !(enemy_armory && enemy_quarters < ai.strategy().rush_quarters))
            {
                return;
            }
            rush_alert = true;
            rush_alert_time = ai.time();
            ai.log("enemy arming early (" + (intel.enemy_warriors.size()) + " warriors, armory " + java.lang.JStr.valueOf(enemy_armory) + ", " + (enemy_quarters) + " quarters): armory first");
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    p.priority = 1;
                }
            }
            projects.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object p_2_l) =>
            {
                com.oddlabs.tt.player.gauntlet.Economy.Project p_2 = (com.oddlabs.tt.player.gauntlet.Economy.Project)p_2_l;
                return p_2.priority;
            })));
        }
        // Java: 2000
        public bool armsRace()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (!rush_alert || ai.time() > (float)(rush_alert_time + strategy.rush_seconds))
            {
                return false;
            }
            return com.oddlabs.tt.player.gauntlet.Combat.total(ai.intel().warriors) < (float)((float)(1.2f * com.oddlabs.tt.player.gauntlet.Combat.total(ai.intel().enemy_warriors)) + 4.0f);
        }
        // Java: 2008
        public bool underPressure()
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            if (!strategy.pressure_response || ai.time() >= strategy.pressure_time || military.baseThreatLevel() == 0)
            {
                return false;
            }
            return com.oddlabs.tt.player.gauntlet.Combat.total(ai.intel().warriors) < (float)((float)(1.2f * military.threatStrength()) + 4.0f);
        }
        // Java: 2020
        public int[] towerAnchor(int tower_count)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (tower_count % 3 == 2 && !intel.quarters.isEmpty() && !strategy.tower_q_anchor)
            {
                com.oddlabs.tt.model.Building home = homeArmory(armory);
                ai.aiLog().count("tower_q_home");
                return new int[] { home.getGridX(), home.getGridY() };
            }
            if (tower_count % 3 == 2 && !intel.quarters.isEmpty())
            {
                com.oddlabs.tt.model.Building exposed = null;
                float best = (float)(-1.0f);
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    float e = ai.planner().exposure(q.getGridX(), q.getGridY());
                    if (e > best && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), armory.getGridX(), armory.getGridY()) > 20 * 20)
                    {
                        best = e;
                        exposed = q;
                    }
                }
                if (exposed != null)
                {
                    return new int[] { exposed.getGridX(), exposed.getGridY() };
                }
            }
            if (strategy.tower_home_anchor)
            {
                com.oddlabs.tt.model.Building home_2 = homeArmory(armory);
                if (((object)home_2 != (object)armory))
                {
                    ai.aiLog().count("tower_home_anchor");
                    return new int[] { home_2.getGridX(), home_2.getGridY() };
                }
            }
            return new int[] { armory.getGridX(), armory.getGridY() };
        }
        // Java: 2058
        public com.oddlabs.tt.model.Building homeArmory(com.oddlabs.tt.model.Building primary)
        {
            com.oddlabs.tt.model.Building home = primary;
            int best = java.lang.Integer.MAX_VALUE;
            int sx = ai.planner().getStartX();
            int sy = ai.planner().getStartY();
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(ai.intel().armories))
            {
                if (a.isDead())
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, a.getGridX(), a.getGridY());
                if (d < best)
                {
                    best = d;
                    home = a;
                }
            }
            return home;
        }
        // Java: 2079
        public int[][] enemyFront(int k)
        {
            java.util.List enemies = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.Player p in ai.owner().getWorld().getPlayers())
            {
                if (ai.owner().isEnemy(p) && p.isAlive())
                {
                    enemies.add(p);
                }
            }
            if (enemies.isEmpty())
            {
                return null;
            }
            com.oddlabs.tt.player.Player enemy = ((com.oddlabs.tt.player.Player)enemies.get(k % enemies.size()));
            int order_v = ai.strategy().front_order;
            if (order_v != 0 && enemies.size() > 1)
            {
                int sx = ai.planner().getStartX();
                int sy = ai.planner().getStartY();
                java.util.Comparator farthest = java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object p_2_l) =>
                {
                    com.oddlabs.tt.player.Player p_2 = (com.oddlabs.tt.player.Player)p_2_l;
                    return -com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(sx, sy, com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p_2.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p_2.getStartY()));
                }));
                com.oddlabs.tt.player.gauntlet.Shepherd shepherd = ai.shepherd();
                java.util.List sorted = new java.util.ArrayList(enemies);
                sorted.sort((order_v == 2 ? java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object p_3_l) =>
                {
                    com.oddlabs.tt.player.Player p_3 = (com.oddlabs.tt.player.Player)p_3_l;
                    return -shepherd.baseWaves(p_3);
                })).thenComparing(farthest) : farthest));
                com.oddlabs.tt.player.Player chosen = ((com.oddlabs.tt.player.Player)sorted.get(k % sorted.size()));
                if (((object)chosen != (object)enemy))
                {
                    ai.aiLog().count("front_order");
                    if (ai.logging())
                    {
                        ai.log("front tower " + (k) + " faces " + java.lang.JStr.valueOf(chosen.getPlayerInfo().getName()) + " (" + (shepherd.baseWaves(chosen)) + " base waves), not " + java.lang.JStr.valueOf(enemy.getPlayerInfo().getName()) + " (" + (shepherd.baseWaves(enemy)) + ")");
                    }
                }
                enemy = chosen;
            }
            int ex = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartX());
            int ey = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartY());
            java.util.List own = ai.intel().homes();
            com.oddlabs.tt.model.Building nearest = com.oddlabs.tt.player.gauntlet.MapAnalysis.nearest(own, ex, ey);
            if (nearest == null)
            {
                return null;
            }
            return new int[][] { new int[] { nearest.getGridX(), nearest.getGridY() }, new int[] { ex, ey } };
        }
        // Java: 2119
        public int countProjects(int type, bool unplaced_only)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.type != type)
                {
                    continue;
                }
                if (unplaced_only && p.isPlaced())
                {
                    continue;
                }
                if (!unplaced_only && p.isPlaced())
                {
                    continue;
                }
                n++;
            }
            return n;
        }
        // Java: 2137
        public int holdFor(com.oddlabs.tt.model.Building quarters)
        {
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.Player owner = ai.owner();
            int pop = owner.getUnitCountContainer().getNumSupplies();
            int max = owner.getWorld().getMaxUnitCount();
            bool training = quarters.getChieftainContainer() != null && quarters.getChieftainContainer().isTraining();
            if (training && strategy.chief_topup_any)
            {
                return strategy.hold_chieftain;
            }
            if (pop >= max - 2)
            {
                return 0;
            }
            if (training)
            {
                return strategy.hold_chieftain;
            }
            if (ai.intel().armories.isEmpty() && !ai.intel().quarters.isEmpty() && needsBuilders())
            {
                return java.lang.Math.min(2, strategy.hold_early);
            }
            int hold = default;
            if (pop > max * 7 / 10)
            {
                hold = strategy.hold_late;
            }
            else
            {
                hold = (ai.time() < strategy.hold_mid_time ? strategy.hold_early : strategy.hold_mid);
                if (armsRace())
                {
                    hold = java.lang.Math.min(2, hold);
                }
            }
            if (strategy.hold_backlog > 0 && backlog_on && hold > strategy.hold_early)
            {
                hold = strategy.hold_early;
                ai.aiLog().count("hold_backlog_ticks");
            }
            return hold;
        }
        public bool backlog_on;
        public float backlog_since;
        // Java: 2171
        public bool needsBuilders()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.isPlaced() && builderCount(p.building) < buildersWanted(p))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 2179
        public bool gatherUnderThreat()
        {
            return ai.strategy().gather_under_threat && (ai.enemiesAlive() > 1 || armsRace() || underPressure() || ai.strategy().gather_threat_1v1);
        }
        // Java: 2184
        public void manageQuarters()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            bool threatened = ai.military().baseThreatLevel() > 1;
            bool keep_reserve = !bank_active && !bank_reserve.isEmpty() && raidEvacuating(intel.armory());
            if (!bank_active && !bank_reserve.isEmpty() && !keep_reserve)
            {
                bank_reserve.clear();
            }
            int builders_short = 0;
            if (bank_active)
            {
                foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
                {
                    if (p.isPlaced())
                    {
                        builders_short += java.lang.Math.max(0, buildersWanted(p) - builderCount(p.building));
                    }
                }
            }
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (evacuating.containsKey(q) || retire.isDoomed(q))
                {
                    continue;
                }
                int inside = q.getUnitContainer().getNumSupplies();
                int hold = holdFor(q);
                if (threatened && ai.military().threatNearEcon(q.getGridX(), q.getGridY(), (gatherUnderThreat() ? 12 : 20)))
                {
                    continue;
                }
                int r = (bank_active || keep_reserve ? ((int)((int?)bank_reserve.getOrDefault(q, 0))) : 0);
                if (r > 0 && bank_active)
                {
                    int rel = java.lang.Math.min(r, java.lang.Math.max(0, bank_room - 4));
                    r -= rel;
                    bank_room -= rel;
                    for (int i = 0; i < rel; i++)
                    {
                        ai.aiLog().count("bank_release_room");
                    }
                    int relb = java.lang.Math.min(r, builders_short);
                    r -= relb;
                    builders_short -= relb;
                    for (int i_2 = 0; i_2 < relb; i_2++)
                    {
                        ai.aiLog().count("bank_release_builders");
                    }
                    bank_reserve.put(q, r);
                }
                hold += r;
                int held = (raid_hold.isEmpty() ? 0 : evacueesHeld(q));
                if (held > 0)
                {
                    hold += held;
                    if (inside > hold - held)
                    {
                        ai.aiLog().count("raid_evac_held");
                    }
                }
                if (danger_idle && inside > hold && !needsBuilders())
                {
                    ai.aiLog().count("hold_danger");
                    continue;
                }
                if (inside > hold)
                {
                    ai.owner().deployUnits(q, com.oddlabs.tt.model.DeployType.PEON, inside - hold);
                    if (backlog_on)
                    {
                        for (int i_3 = 0; i_3 < inside - hold; i_3++)
                        {
                            ai.aiLog().count("hold_backlog_deployed");
                        }
                    }
                }
            }
        }
        // Java: 2246
        public void refreshArmoryField()
        {
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            if (armory == null)
            {
                return;
            }
            if (armory_field == null || ((object)armory_field_owner != (object)armory) || (float)(ai.time() - armory_field_time) > 60.0f)
            {
                armory_field = ai.map().computeField(armory.getGridX(), armory.getGridY(), 400);
                armory_field_owner = armory;
                armory_field_time = ai.time();
            }
        }
        // Java: 2261
        public void choosePrimaryArmory()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building primary = null;
            if (expansion != null && !expansion.isDead() && expansion.isComplete())
            {
                primary = expansion;
                reloc_primary = null;
            }
            else if (ai.strategy().reloc && ai.time() >= ai.strategy().reloc_time)
            {
                primary = relocPrimary();
            }
            if (primary == null && ai.strategy().retire && !intel.armories.isEmpty() && retire.isDoomed(((com.oddlabs.tt.model.Building)intel.armories.getFirst())))
            {
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
                {
                    if (!a.isDead() && !retire.isDoomed(a))
                    {
                        primary = a;
                        break;
                    }
                }
            }
            intel.setPrimaryArmory(primary);
            com.oddlabs.tt.model.Building armory = intel.armory();
            if (ai.strategy().quarters_rally && armory != null && ((object)armory != (object)rally_armory))
            {
                rally_armory = armory;
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    if (!q.isDead() && q.isComplete() && !evacuating.containsKey(q))
                    {
                        ai.owner().setRallyPoint(q, armory);
                    }
                }
            }
        }
        public com.oddlabs.tt.model.Building rally_armory;
        public com.oddlabs.tt.model.Building reloc_primary;
        public float reloc_besieged_since;
        public float reloc_switch_time;
        public java.util.Map armory_costs;
        public const float RELOC_SWITCH_SIEGE = 20.0f;
        public const float RELOC_SWITCH_GAP = 60.0f;
        // Java: 2309
        public com.oddlabs.tt.model.Building relocPrimary()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            float now = ai.time();
            armory_costs.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            if (intel.armories.isEmpty())
            {
                reloc_primary = null;
                return null;
            }
            com.oddlabs.tt.model.Building cur = reloc_primary;
            if (cur == null || cur.isDead() || !intel.armories.contains(cur))
            {
                reloc_primary = pickPrimary(false);
                reloc_besieged_since = (float)(-1.0f);
                reloc_switch_time = now;
                if (intel.armories.size() > 1)
                {
                    ai.aiLog().count("reloc_primary_pick");
                    com.oddlabs.tt.model.Building b = reloc_primary;
                    if (b != null && ai.logging())
                    {
                        ai.log("reloc: primary armory now " + (b.getGridX()) + "," + (b.getGridY()) + " of " + (intel.armories.size()));
                    }
                }
                return reloc_primary;
            }
            com.oddlabs.tt.player.gauntlet.Military m = ai.military();
            bool besieged = m.threatNear(cur.getGridX(), cur.getGridY(), 16);
            reloc_besieged_since = (!besieged ? (float)(-1.0f) : (reloc_besieged_since < 0.0f ? now : reloc_besieged_since));
            if (besieged && intel.armories.size() > 1 && (float)(now - reloc_besieged_since) >= com.oddlabs.tt.player.gauntlet.Economy.RELOC_SWITCH_SIEGE && (float)(now - reloc_switch_time) >= com.oddlabs.tt.player.gauntlet.Economy.RELOC_SWITCH_GAP)
            {
                com.oddlabs.tt.model.Building alt = pickPrimary(true);
                if (alt != null && ((object)alt != (object)cur))
                {
                    reloc_primary = alt;
                    reloc_besieged_since = (float)(-1.0f);
                    reloc_switch_time = now;
                    ai.aiLog().count("reloc_primary_switch");
                    if (ai.logging())
                    {
                        ai.log("reloc: primary armory switches from " + (cur.getGridX()) + "," + (cur.getGridY()) + " (besieged) to " + (alt.getGridX()) + "," + (alt.getGridY()));
                    }
                }
            }
            return reloc_primary;
        }
        // Java: 2352
        public com.oddlabs.tt.model.Building pickPrimary(bool quiet_only)
        {
            com.oddlabs.tt.player.gauntlet.Military m = ai.military();
            if (ai.intel().armories.size() == 1 && !quiet_only)
            {
                return ((com.oddlabs.tt.model.Building)ai.intel().armories.getFirst());
            }
            com.oddlabs.tt.model.Building best = null;
            float best_cost = java.lang.Float.MAX_VALUE;
            com.oddlabs.tt.model.Building any = null;
            float any_cost = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(ai.intel().armories))
            {
                if (a.isDead() || retire.isDoomed(a))
                {
                    continue;
                }
                float cost = armoryCost(a);
                if (cost <= any_cost)
                {
                    any_cost = cost;
                    any = a;
                }
                if (m.threatNear(a.getGridX(), a.getGridY(), 16))
                {
                    continue;
                }
                if (cost <= best_cost)
                {
                    best_cost = cost;
                    best = a;
                }
            }
            return (best != null || quiet_only ? best : any);
        }
        // Java: 2379
        public float armoryCost(com.oddlabs.tt.model.Building a)
        {
            float[] c = ((float[])armory_costs.get(a));
            if (c == null || (float)(ai.time() - c[1]) > 60.0f)
            {
                c = new float[] { ai.planner().warriorGatherCost(ai.map().computeField(a.getGridX(), a.getGridY(), 220)), ai.time() };
                armory_costs.put(a, c);
            }
            return c[0];
        }
        public float meas_start;
        public int meas_iron0;
        public int meas_rock0;
        public float meas_iron_gs;
        public float meas_rock_gs;
        public float iron_s;
        public float rock_s;
        public bool rock_stream_on;
        // Java: 2400
        public void measureYield()
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            if (meas_start < 0.0f)
            {
                meas_start = ai.time();
                meas_iron0 = owner.getIronHarvested();
                meas_rock0 = owner.getRockHarvested();
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            meas_iron_gs += intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON);
            meas_rock_gs += intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK);
            if ((float)(ai.time() - meas_start) < 30.0f)
            {
                return;
            }
            int di = owner.getIronHarvested() - meas_iron0;
            iron_rate = (float)((float)(0.5f * iron_rate) + (float)((float)(0.5f * di) / java.lang.Math.max(1.0f, (float)(ai.time() - meas_start))));
            int dr = owner.getRockHarvested() - meas_rock0;
            if (meas_iron_gs >= 60.0f)
            {
                iron_s = (float)(meas_iron_gs / java.lang.Math.max(0.5f, di));
            }
            if (meas_rock_gs >= 60.0f)
            {
                rock_s = (float)(meas_rock_gs / java.lang.Math.max(0.5f, dr));
            }
            meas_start = ai.time();
            meas_iron0 = owner.getIronHarvested();
            meas_rock0 = owner.getRockHarvested();
            meas_iron_gs = 0.0f;
            meas_rock_gs = 0.0f;
        }
        // Java: 2431
        public void considerExpansion()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            if (armory == null || armory_field == null || ai.time() < 300.0f || (float)(ai.time() - last_expansion_check) < 30.0f || !ai.strategy().expansion)
            {
                return;
            }
            bool under_threat = ai.strategy().expand_under_threat;
            if (under_threat)
            {
                last_expansion_check = ai.time();
            }
            if ((!under_threat && ai.military().baseThreatLevel() > 0) || countProjects(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false) > 0)
            {
                return;
            }
            if (!ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY))
            {
                return;
            }
            last_expansion_check = ai.time();
            float current = ai.planner().warriorGatherCost(armory_field);
            if (iron_cycle < 70.0f && current < 110.0f)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = ai.planner().findExpansionSite(reservedSites(null), armory_field);
            float better = java.lang.Float.MAX_VALUE;
            if (site != null)
            {
                better = ai.planner().warriorGatherCost(ai.map().computeField(site.x, site.y, 220));
            }
            if (ai.strategy().far_expansion && better > (float)(0.75f * current))
            {
                com.oddlabs.tt.player.gauntlet.SitePlanner.Site far = ai.planner().findFarExpansionSite(reservedSites(null));
                if (far != null && (float)(-far.score) < java.lang.Math.min(better, (float)(0.75f * current)))
                {
                    site = far;
                    better = (float)(-far.score);
                }
            }
            if (site == null)
            {
                return;
            }
            if (retire.retiredNear(site.x, site.y))
            {
                ai.aiLog().count("retire_site_skip");
                return;
            }
            ai.log(java.lang.JString.format("expansion check: current armory %.0f (iron %.0fs), best site %d,%d %.0f", current, iron_cycle, site.x, site.y, better));
            float gain = (iron_cycle >= ai.strategy().desperate_iron_cycle ? ai.strategy().desperate_expansion : 0.75f);
            if (better > (float)(gain * current))
            {
                return;
            }
            if (under_threat && ai.military().baseThreatLevel() > 0)
            {
                if (!quietSite(armory, site.x, site.y))
                {
                    ai.aiLog().count("exp_blocked_site");
                    return;
                }
                ai.aiLog().count("exp_under_threat");
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site, 1);
            expansion_project = p;
        }
        // Java: 2484
        public bool quietSite(com.oddlabs.tt.model.Building armory, int x, int y)
        {
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            if (military.threatNear(x, y, 30) || military.enemyStrengthNear(x, y, 30) > 0.0f)
            {
                return false;
            }
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            float len = ((float)(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ax, ay, x, y))));
            int steps = java.lang.Math.max(1, java.lang.JC.f2i(((float)(len / 6.0f))));
            for (int i = 0; i <= steps; i++)
            {
                int px = ax + java.lang.Math.round((float)((x - ax) * i / ((float)(steps))));
                int py = ay + java.lang.Math.round((float)((y - ay) * i / ((float)(steps))));
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
                {
                    if (!e.isDead() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(px, py, e.getGridX(), e.getGridY()) <= 12 * 12)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public float reloc_next;
        public float last_reloc_end;
        public com.oddlabs.tt.player.gauntlet.Economy.Project reloc_ended;
        public com.oddlabs.tt.model.Building reloc_armory;
        public float reloc_armory_done;
        public int reloc_armory_iron;
        public java.util.Map was_drained;
        public const float RELOC_WAIT = 90.0f;
        // Java: 2524
        public bool relocGate()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building primary = intel.armory();
            if (primary == null || !intel.armory_sites.isEmpty() || hasArmoryProject())
            {
                return false;
            }
            was_drained.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            bool all = true;
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
            {
                if (((object)a == (object)primary) || a.isDead())
                {
                    continue;
                }
                bool d = drained(a);
                bool? before = ((bool?)was_drained.put(a, d));
                if (before != null && ((bool)before) && !d)
                {
                    ai.aiLog().count("reloc_drained_flicker");
                }
                all &= d;
            }
            if (!all)
            {
                ai.aiLog().count("reloc_gate_undrained");
            }
            return all;
        }
        // Java: 2549
        public bool drained(com.oddlabs.tt.model.Building a)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (((object)a == (object)intel.armory()) || evacuating.containsKey(a) || a.getUnitContainer().getNumSupplies() > 0 || com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.IronSupply)) + com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.RockSupply)) > 1)
            {
                return false;
            }
            foreach (com.oddlabs.tt.player.gauntlet.Intel.PeonState s in new com.oddlabs.tt.player.gauntlet.Intel.PeonState[] { com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN })
            {
                if (intel.countLinkedGatherers(s, a) > 0)
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 2561
        public int liveIronNear(int x, int y, int r)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.IronSupply s in java.lang.JIter.of(ai.map().getIron()))
            {
                if (!s.isEmpty() && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, s.getGridX(), s.getGridY()) <= r * r)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 2581
        public void considerRelocation()
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            float now = ai.time();
            com.oddlabs.tt.model.Building primary = ai.intel().armory();
            if (primary == null || armory_field == null || now < reloc_next || (float)(now - last_reloc_end) < st.reloc_gap)
            {
                return;
            }
            bool capped = !ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            if (capped && !st.reloc_slot)
            {
                ai.aiLog().count("reloc_cap_blocked");
                return;
            }
            if (((object)iron_cycle_armory != (object)primary))
            {
                ai.aiLog().count("reloc_cycle_stale");
                return;
            }
            reloc_next = (float)(now + 30.0f);
            ai.aiLog().count("reloc_check");
            int ax = primary.getGridX();
            int ay = primary.getGridY();
            float current = ai.planner().warriorGatherCost(armory_field);
            int nodes = liveIronNear(ax, ay, 30);
            bool econ = iron_cycle >= 70.0f || current >= 110.0f;
            bool few = st.reloc_nodes > 0 && nodes < st.reloc_nodes;
            if (!econ && !few)
            {
                ai.aiLog().count("reloc_not_bad");
                return;
            }
            com.oddlabs.tt.player.gauntlet.DistanceField from = (st.reloc_reach <= 400 ? armory_field : ai.map().computeField(ax, ay, st.reloc_reach));
            com.oddlabs.tt.player.gauntlet.Military m = ai.military();
            java.util.List origins = drawOrigins();
            int[] why = new int[5];
            com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok = new com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk_L((int x, int y) =>
            {
                if (m.threatNear(x, y, 30) || m.enemyStrengthNear(x, y, 30) > 0.0f)
                {
                    why[0]++;
                    return false;
                }
                if (razedNear(x, y) || retire.retiredNear(x, y))
                {
                    why[1]++;
                    return false;
                }
                if (st.reloc_nodes > 0 && liveIronNear(x, y, 30) < st.reloc_nodes)
                {
                    why[2]++;
                    return false;
                }
                if (st.reloc_draw > 0 && com.oddlabs.tt.player.gauntlet.Economy.drawOf(origins, x, y) >= st.reloc_draw)
                {
                    why[4]++;
                    return false;
                }
                if (!pathClear(ax, ay, x, y, 12))
                {
                    why[3]++;
                    return false;
                }
                return true;
            });
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = ai.planner().findArmorySite(reservedSites(null), st.reloc_reach, from, 40, ok, 0.0f, reloc_search, now);
            if (ai.planner().lastSearchGaveUp())
            {
                ai.aiLog().count("reloc_search_gaveup");
            }
            if (site == null)
            {
                int k = 0;
                for (int i = 1; i < why.Length; i++)
                {
                    if (why[i] > why[k])
                    {
                        k = i;
                    }
                }
                ai.aiLog().count("reloc_nosite_" + java.lang.JStr.valueOf(new string[] { "enemy", "razed", "nodes", "path", "draw" }[k]));
                if (why[4] > 0)
                {
                    ai.aiLog().count("reloc_draw_nosite");
                }
                if (ai.logging())
                {
                    ai.log(java.lang.JString.format("reloc check: armory %d,%d, no quiet site (turned down: enemy %d, razed %d, nodes %d, path %d, draw %d; %d copies with an idle warrior)", ax, ay, why[0], why[1], why[2], why[3], why[4], origins.size()));
                }
                return;
            }
            if (why[4] > 0)
            {
                ai.aiLog().count("reloc_draw_filtered");
            }
            float[] known = ai.planner().searched(reloc_search, site.x, site.y);
            float better = (known != null ? known[0] : ai.planner().warriorGatherCost(ai.map().computeField(site.x, site.y, 220)));
            float gain = (iron_cycle >= st.desperate_iron_cycle ? st.desperate_expansion : 0.75f);
            int dist = java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ax, ay, site.x, site.y)));
            int site_nodes = liveIronNear(site.x, site.y, 30);
            int draw = com.oddlabs.tt.player.gauntlet.Economy.drawOf(origins, site.x, site.y);
            if (ai.logging())
            {
                ai.log(java.lang.JString.format("reloc check: armory %d,%d cost %.0f (iron %.0fs, %d nodes within 30), quiet site %d,%d cost %.0f (%d nodes, %d cells, draw %d of %d copies), gain %.2f%s", ax, ay, current, iron_cycle, nodes, site.x, site.y, better, site_nodes, dist, draw, origins.size(), gain, (capped ? (object)(", capped") : (object)(""))));
            }
            if (better > (float)(gain * current))
            {
                ai.aiLog().count("reloc_not_better");
                return;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site, 1);
            p.reloc = true;
            p.added = now;
            expansion_project = p;
            ai.aiLog().count("reloc_placed");
            if (econ)
            {
                ai.aiLog().count("reloc_trig_econ");
            }
            if (few)
            {
                ai.aiLog().count("reloc_trig_nodes");
            }
            if (capped)
            {
                ai.aiLog().count("reloc_slot_project");
            }
            for (int i_2 = 0; i_2 < draw; i_2++)
            {
                ai.aiLog().count("reloc_placed_draw");
            }
            if (draw == 0)
            {
                ai.aiLog().count("reloc_placed_draw0");
            }
            ai.aiLog().log("RELOC", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("hop to %d,%d: cost %.0f vs %.0f, %d vs %d nodes within 30, %d cells, draw %d of %d copies%s", site.x, site.y, better, current, site_nodes, nodes, dist, draw, origins.size(), (capped ? (object)(", waiting for a slot") : (object)("")));
            }));
        }
        // Java: 2703
        public java.util.List drawOrigins()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            java.util.List @out = new java.util.ArrayList();
            java.util.List seen = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(intel.enemy_warriors))
            {
                if (e.isDead() || !((e.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)) || seen.contains(e.getOwner()))
                {
                    continue;
                }
                seen.add(e.getOwner());
                int x = e.getGridX();
                int y = e.getGridY();
                int best = java.lang.Integer.MAX_VALUE;
                foreach (java.util.List group in java.lang.JIter.of(java.util.List_.of(intel.quarters, intel.armories, intel.towers, intel.quarters_sites, intel.armory_sites, intel.tower_sites, intel.decoy_sites)))
                {
                    foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(group))
                    {
                        if (!b.isDead())
                        {
                            best = java.lang.Math.min(best, com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y));
                        }
                    }
                }
                @out.add(new int[] { x, y, best });
            }
            return @out;
        }
        // Java: 2725
        public static int drawOf(java.util.List origins, int x, int y)
        {
            int n = 0;
            foreach (int[] o in java.lang.JIter.of(origins))
            {
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(x, y, o[0], o[1]) < o[2])
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 2740
        public void watchReloc()
        {
            float now = ai.time();
            com.oddlabs.tt.player.gauntlet.Economy.Project p = expansion_project;
            if (p != null && p.reloc && !ai.strategy().reloc_slot && projects.contains(p) && p.building == null && (float)(now - p.added) > com.oddlabs.tt.player.gauntlet.Economy.RELOC_WAIT)
            {
                projects.remove(p);
                ai.aiLog().count("reloc_stale_drop");
                ai.log("reloc: drop " + java.lang.JStr.valueOf(p.describe()) + ", unplaced for " + (java.lang.JC.f2i(((float)(now - p.added)))) + "s");
            }
            if (p != null && (p.@lock || (p.reloc && ai.strategy().reloc_slot)) && projects.contains(p) && p.building == null)
            {
                p.open_since = (!ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY) ? (float)(-1.0f) : (p.open_since < 0.0f ? now : p.open_since));
                if (p.open_since >= 0.0f && (float)(now - p.open_since) > com.oddlabs.tt.player.gauntlet.Economy.RELOC_WAIT)
                {
                    projects.remove(p);
                    ai.aiLog().count((p.@lock ? "lock_stale_drop" : "reloc_stale_drop"));
                    ai.aiLog().log((p.@lock ? "LOCK" : "RELOC"), new java.util.function.Supplier_L(() =>
                    {
                        return "drop " + java.lang.JStr.valueOf(p.describe()) + ": no placer out with a slot open for " + (java.lang.JC.f2i(((float)(now - p.open_since)))) + "s, " + (java.lang.JC.f2i(((float)(now - p.added)))) + "s after planning";
                    }));
                }
            }
            if (p != null && ((object)p != (object)reloc_ended) && !projects.contains(p) && p.@lock)
            {
                reloc_ended = p;
                last_reloc_end = now;
                com.oddlabs.tt.model.Building b = p.building;
                if (b != null && !b.isDead() && b.isComplete())
                {
                    lock_armed = false;
                    lock_since = (float)(-1.0f);
                    lock_at = null;
                    lock_new = b;
                    lock_armory = b;
                    lock_armory_done = now;
                    lock_armory_iron = ai.owner().getIronHarvested();
                    ai.aiLog().count("lock_completed");
                    com.oddlabs.tt.model.Building old = lock_old;
                    ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                    {
                        return "lock armory at " + (b.getGridX()) + "," + (b.getGridY()) + " completed " + (java.lang.JC.f2i(((float)(now - p.added)))) + "s after planning" + java.lang.JStr.valueOf(((old != null && !old.isDead() ? ", " + (old.getUnitContainer().getNumSupplies()) + " workers in the locked armory" : "")));
                    }));
                }
                else
                {
                    string why = (b != null && b.isDead() ? "razed" : (p.failures > 6 ? "placer" : "other"));
                    lock_old = null;
                    ai.aiLog().count("lock_dropped");
                    ai.aiLog().count("lock_dropped_" + java.lang.JStr.valueOf(why));
                    ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                    {
                        return "lock project " + java.lang.JStr.valueOf(p.describe()) + " dropped (" + java.lang.JStr.valueOf(why) + ", " + (p.failures) + " placer failures, " + (java.lang.JC.f2i(((float)(now - p.added)))) + "s after planning)";
                    }));
                }
            }
            com.oddlabs.tt.model.Building la = lock_armory;
            if (la != null && la.isDead())
            {
                lock_armory = null;
                int life = java.lang.JC.f2i(((float)(now - lock_armory_done)));
                int iron = ai.owner().getIronHarvested() - lock_armory_iron;
                ai.aiLog().count("lock_lost");
                if (life < 300)
                {
                    ai.aiLog().count("lock_lost_early");
                }
                ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                {
                    return "lock armory at " + (la.getGridX()) + "," + (la.getGridY()) + " razed after " + (life) + "s, iron +" + (iron) + " meanwhile";
                }));
            }
            if (p != null && ((object)p != (object)reloc_ended) && !projects.contains(p))
            {
                reloc_ended = p;
                last_reloc_end = now;
                com.oddlabs.tt.model.Building b_2 = p.building;
                bool done = b_2 != null && !b_2.isDead() && b_2.isComplete();
                if (p.reloc && done)
                {
                    reloc_armory = b_2;
                    reloc_armory_done = now;
                    reloc_armory_iron = ai.owner().getIronHarvested();
                    ai.aiLog().count("reloc_completed");
                    ai.log("reloc: hop armory at " + (b_2.getGridX()) + "," + (b_2.getGridY()) + " completed");
                }
                else if (p.reloc)
                {
                    string why_2 = (b_2 != null && b_2.isDead() ? "razed" : (p.failures > 6 ? "placer" : "other"));
                    ai.aiLog().count("reloc_dropped");
                    ai.aiLog().count("reloc_dropped_" + java.lang.JStr.valueOf(why_2));
                    ai.aiLog().log("RELOC", new java.util.function.Supplier_L(() =>
                    {
                        return "hop project " + java.lang.JStr.valueOf(p.describe()) + " dropped (" + java.lang.JStr.valueOf(why_2) + ", " + (p.failures) + " placer failures, " + (java.lang.JC.f2i(((float)(now - p.added)))) + "s after planning)";
                    }));
                }
            }
            com.oddlabs.tt.model.Building a = reloc_armory;
            if (a != null && a.isDead())
            {
                reloc_armory = null;
                int life_2 = java.lang.JC.f2i(((float)(now - reloc_armory_done)));
                int iron_2 = ai.owner().getIronHarvested() - reloc_armory_iron;
                ai.aiLog().count("reloc_lost");
                if (life_2 < 180)
                {
                    ai.aiLog().count("reloc_lost_early");
                }
                ai.aiLog().log("RELOC", new java.util.function.Supplier_L(() =>
                {
                    return "hop armory at " + (a.getGridX()) + "," + (a.getGridY()) + " razed after " + (life_2) + "s, iron +" + (iron_2) + " meanwhile";
                }));
            }
        }
        public float lock_since;
        public com.oddlabs.tt.model.Building lock_at;
        public bool lock_armed;
        public float lock_next;
        public com.oddlabs.tt.model.Building lock_old;
        public com.oddlabs.tt.model.Building lock_new;
        public com.oddlabs.tt.model.Building lock_armory;
        public float lock_armory_done;
        public int lock_armory_iron;
        public const float LOCK_SITE_TREE = 60.0f;
        public const float LOCK_TREE = 90.0f;
        public const float LOCK_HOLD = 120.0f;
        public const float LOCK_RETRY = 10.0f;
        // Java: 2857
        public void trackLock(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            float now = ai.time();
            bool locked = armory != null && now >= st.reloc_lock_time && armory.isComplete() && tree_cycle >= com.oddlabs.tt.player.gauntlet.Economy.LOCK_TREE && com.oddlabs.tt.player.gauntlet.Economy.stock(armory, typeof(com.oddlabs.tt.landscape.TreeSupply)) < 2 && armory.getUnitContainer().getNumSupplies() >= want_workers + 20;
            if (!locked || ((object)armory != (object)lock_at))
            {
                if (lock_armed)
                {
                    float held = (float)(now - lock_since);
                    ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                    {
                        return java.lang.JString.format("wood lock over after %.0f s", held);
                    }));
                }
                lock_armed = false;
                lock_since = (locked ? now : (float)(-1.0f));
                lock_at = (locked ? armory : (com.oddlabs.tt.model.Building)(null));
                return;
            }
            if (!lock_armed && (float)(now - lock_since) >= st.reloc_lock)
            {
                lock_armed = true;
                ai.aiLog().count("lock_armed");
                int workers = armory.getUnitContainer().getNumSupplies();
                int wood = com.oddlabs.tt.player.gauntlet.Economy.stock(armory, typeof(com.oddlabs.tt.landscape.TreeSupply));
                int iron = com.oddlabs.tt.player.gauntlet.Economy.stock(armory, typeof(com.oddlabs.tt.model.IronSupply));
                ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("wood lock at the armory at %d,%d for %.0f s: tree cycle %.0f s, wood %d, iron %d, workers %d (want %d)", armory.getGridX(), armory.getGridY(), (float)(now - lock_since), tree_cycle, wood, iron, workers, want_workers);
                }));
            }
            if (lock_armed)
            {
                ai.aiLog().count("lock_ticks");
            }
        }
        // Java: 2892
        public bool lockGate()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            return intel.armory() != null && ((object)intel.armory() == (object)lock_at) && intel.armory_sites.isEmpty() && !hasArmoryProject();
        }
        // Java: 2903
        public bool hasArmoryProject()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (q.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 2921
        public void lockRelocate()
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            float now = ai.time();
            com.oddlabs.tt.model.Building primary = ai.intel().armory();
            if (primary == null || armory_field == null || now < lock_next)
            {
                return;
            }
            lock_next = (float)(now + com.oddlabs.tt.player.gauntlet.Economy.LOCK_RETRY);
            ai.aiLog().count("lock_check");
            int ax = primary.getGridX();
            int ay = primary.getGridY();
            com.oddlabs.tt.player.gauntlet.DistanceField from = (st.reloc_reach <= 400 ? armory_field : ai.map().computeField(ax, ay, st.reloc_reach));
            int[] why = new int[2];
            com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk ok = new com.oddlabs.tt.player.gauntlet.SitePlanner.CellOk_L((int x, int y) =>
            {
                if (!quietOk(x, y) || retire.retiredNear(x, y))
                {
                    why[0]++;
                    return false;
                }
                if (!pathClear(ax, ay, x, y, 12))
                {
                    why[1]++;
                    return false;
                }
                return true;
            });
            com.oddlabs.tt.player.gauntlet.SitePlanner planner = ai.planner();
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site site = planner.findArmorySite(reservedSites(null), st.reloc_reach, from, 0, ok, com.oddlabs.tt.player.gauntlet.Economy.LOCK_SITE_TREE, lock_search, now);
            if (planner.lastSearchGaveUp())
            {
                ai.aiLog().count("lock_search_gaveup");
            }
            if (site == null)
            {
                int trees = planner.lastTreeFailed();
                ai.aiLog().count("lock_nosite");
                int k = (trees > why[0] && trees > why[1] ? 2 : (why[0] >= why[1] ? 0 : 1));
                ai.aiLog().count("lock_nosite_" + java.lang.JStr.valueOf(new string[] { "quiet", "path", "trees" }[k]));
                ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("no lock site for the armory at %d,%d: %d candidates not quiet, %d with enemies on the way, %d quiet ones short of trees", ax, ay, why[0], why[1], trees);
                }));
                return;
            }
            float[] known = planner.searched(lock_search, site.x, site.y);
            float site_tree = (known != null ? known[2] : (float)(-1.0f));
            bool capped = !ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            com.oddlabs.tt.player.gauntlet.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site, 1);
            p.@lock = true;
            p.added = now;
            expansion_project = p;
            lock_old = primary;
            lock_new = null;
            ai.aiLog().count("lock_project");
            if (capped)
            {
                ai.aiLog().count("lock_project_capped");
            }
            com.oddlabs.tt.player.gauntlet.SitePlanner.Site s = site;
            float tree = site_tree;
            int dist = java.lang.JC.d2i(java.lang.Math.sqrt(com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ax, ay, s.x, s.y)));
            int workers = primary.getUnitContainer().getNumSupplies();
            ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("lock move to %d,%d: cost %.0f, tree cycle %.0f s against %.0f, %d cells from the armory at %d,%d (%d workers)%s", s.x, s.y, (float)(-s.score), tree, tree_cycle, dist, ax, ay, workers, (capped ? (object)(", waiting for a slot") : (object)("")));
            }));
        }
        // Java: 2987
        public bool lockHold(com.oddlabs.tt.model.Building old)
        {
            com.oddlabs.tt.model.Building nu = lock_new;
            bool building = false;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                building |= q.@lock;
            }
            if (old.isDead() || (nu == null && !building) || (nu != null && nu.isDead()))
            {
                lock_old = null;
                lock_new = null;
                return false;
            }
            bool timeout = nu != null && (float)(ai.time() - lock_armory_done) > com.oddlabs.tt.player.gauntlet.Economy.LOCK_HOLD;
            if (nu != null && (timeout || (com.oddlabs.tt.player.gauntlet.Economy.stock(nu, typeof(com.oddlabs.tt.landscape.TreeSupply)) >= 2 && !ai.military().threatNear(nu.getGridX(), nu.getGridY(), 16))))
            {
                lock_old = null;
                lock_new = null;
                ai.aiLog().count((timeout ? "lock_bank_timeout" : "lock_bank_released"));
                int workers = old.getUnitContainer().getNumSupplies();
                ai.aiLog().log("LOCK", new java.util.function.Supplier_L(() =>
                {
                    return "the locked armory at " + (old.getGridX()) + "," + (old.getGridY()) + " lets its " + (workers) + " workers go to the new one";
                }));
                return false;
            }
            ai.aiLog().count("lock_bank_held");
            return true;
        }
        // Java: 3019
        public com.oddlabs.tt.player.gauntlet.Economy.Project slotWaiter()
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            if (!st.reloc_slot && st.reloc_lock <= 0)
            {
                return null;
            }
            com.oddlabs.tt.player.gauntlet.Economy.Project waiting = null;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (((st.reloc_slot && q.reloc) || q.@lock) && !q.isPlaced())
                {
                    waiting = q;
                    break;
                }
            }
            if (waiting == null)
            {
                return null;
            }
            int carried = 0;
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q_2 in java.lang.JIter.of(projects))
            {
                if (((object)q_2 != (object)waiting) && q_2.building != null && !q_2.building.isDead() && !q_2.isPlaced())
                {
                    carried++;
                }
            }
            com.oddlabs.tt.player.Player owner = ai.owner();
            return (owner.getBuildingCountContainer().getNumSupplies() + carried >= owner.getWorld().getMaxBuildingCount() - 1 ? waiting : (com.oddlabs.tt.player.gauntlet.Economy.Project)(null));
        }
        // Java: 3040
        public com.oddlabs.tt.player.gauntlet.Economy.Project flaggedWaiting()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project q in java.lang.JIter.of(projects))
            {
                if ((q.reloc || q.@lock) && !q.isPlaced())
                {
                    return q;
                }
            }
            return null;
        }
        // Java: 3049
        public int buildersOn(com.oddlabs.tt.model.Building b)
        {
            return builderCount(b);
        }
        // Java: 3054
        public bool isDrained(com.oddlabs.tt.model.Building a)
        {
            return drained(a);
        }
        // Java: 3059
        public bool isDoomed(com.oddlabs.tt.model.Building b)
        {
            return retire.isDoomed(b);
        }
        // Java: 3063
        public com.oddlabs.tt.player.gauntlet.DistanceField armoryField()
        {
            return (ai.intel().armory() != null ? armory_field : (com.oddlabs.tt.player.gauntlet.DistanceField)(null));
        }
        // Java: 3068
        public void manageArmory()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building primary = intel.armory();
            if (primary == null)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Building armory in java.lang.JIter.of(intel.armories))
            {
                orderWeapons(armory);
                if (((object)armory == (object)primary))
                {
                    deployFromPrimary(armory);
                }
                else
                {
                    drainSecondary(armory);
                }
            }
        }
        public float iron_rate;
        public bool bank_active;
        public bool bank_raid;
        public int bank_cap;
        public int bank_room;
        public float noforge_since;
        public float last_bank_unload;
        public java.util.Map bank_reserve;
        // Java: 3095
        public static int stock(com.oddlabs.tt.model.Building armory, System.Type type)
        {
            return armory.getSupplyContainer(type).getNumSupplies();
        }
        // Java: 3108
        public void guardBank()
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            float now = ai.time();
            com.oddlabs.tt.model.Building a = ai.intel().armory();
            bank_reserve.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
            bool raid = st.raid_bank && now >= st.raid_bank_time && a != null && !a.isDead() && a.isComplete() && ((object)homeArmory(a) != (object)a);
            bank_active = ((st.bank_guard && now >= st.bank_guard_time) || raid) && a != null && !a.isDead() && a.isComplete() && !evacuating.containsKey(a);
            bank_raid = bank_active && raid;
            if (!bank_active || a == null)
            {
                bank_cap = java.lang.Integer.MAX_VALUE;
                noforge_since = (float)(-1.0f);
                return;
            }
            if (raid)
            {
                ai.aiLog().count("raid_bank_ticks");
            }
            int iron = com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.IronSupply));
            int rock = com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.RockSupply));
            bool rockw = rock_weapons || rock_filler;
            bool can_make = canForge(a);
            noforge_since = (can_make ? (float)(-1.0f) : (noforge_since < 0.0f ? now : noforge_since));
            if (noforge_since >= 0.0f && (float)(now - noforge_since) >= st.bank_noforge_s)
            {
                bank_cap = st.bank_min;
                ai.aiLog().count("bank_noforge");
            }
            else
            {
                int flow = java.lang.JC.d2i(java.lang.Math.ceil((float)((float)(st.bank_margin * iron_rate) * com.oddlabs.tt.player.gauntlet.Economy.IRON_WORK)));
                int backlog = java.lang.Math.min((raid ? st.raid_bank_extra : 12), iron + ((rockw ? rock / 2 : 0)));
                bank_cap = java.lang.Math.max(st.bank_min, flow + backlog);
            }
            int workers = a.getUnitContainer().getNumSupplies();
            bank_room = bank_cap - workers - countHeadingTo(a);
            if ((float)(now - last_bank_unload) < 10.0f)
            {
                return;
            }
            int weapons = com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) + com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) + com.oddlabs.tt.player.gauntlet.Economy.stock(a, typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            int pending = a.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            int surplus = workers - weapons - bank_cap;
            if (surplus <= 4 || pending > 0 || reserveQuarters() == null)
            {
                return;
            }
            last_bank_unload = now;
            ai.owner().deployUnits(a, com.oddlabs.tt.model.DeployType.PEON, surplus);
            for (int i = 0; i < surplus; i++)
            {
                ai.aiLog().count("bank_unload");
                if (raid)
                {
                    ai.aiLog().count("raid_bank_unload");
                }
            }
            if (ai.logging())
            {
                ai.log(java.lang.JString.format("%s: unloading %d of %d workers at %d,%d (cap %d, iron %.1f/min)", (raid ? (object)("raid bank") : (object)("bank guard")), surplus, workers, a.getGridX(), a.getGridY(), bank_cap, (float)(60 * iron_rate)));
            }
        }
        // Java: 3165
        public com.oddlabs.tt.model.Building reserveQuarters()
        {
            com.oddlabs.tt.player.gauntlet.Military m = ai.military();
            bool thr = m.baseThreatLevel() > 0;
            int sx = ai.planner().getStartX();
            int sy = ai.planner().getStartY();
            com.oddlabs.tt.model.Building best = null;
            int best_score = java.lang.Integer.MIN_VALUE;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ai.intel().quarters))
            {
                if (q.isDead() || !q.isComplete() || evacuating.containsKey(q) || m.threatNearEcon(q.getGridX(), q.getGridY(), 16) || retire.isDoomed(q))
                {
                    continue;
                }
                if (((int)((int?)bank_reserve.getOrDefault(q, 0))) >= ai.strategy().bank_reserve_max)
                {
                    continue;
                }
                int score = (thr ? com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), m.threatX(), m.threatY()) : -com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(q.getGridX(), q.getGridY(), sx, sy));
                if (best == null || score > best_score)
                {
                    best_score = score;
                    best = q;
                }
            }
            return best;
        }
        // Java: 3189
        public bool bankFull(com.oddlabs.tt.model.Building armory)
        {
            return bank_active && armory != null && ((object)armory == (object)ai.intel().armory()) && !armory.isDead() && armory.getUnitContainer().getNumSupplies() + countHeadingTo(armory) >= bank_cap;
        }
        // Java: 3195
        public bool hasUnplacedProject()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (!p.isPlaced() && p.building == null)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 3206
        public void drainSecondary(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            if (ai.strategy().recall_old_gatherers && (float)(ai.time() - last_old_recall) >= 10.0f)
            {
                last_old_recall = ai.time();
                com.oddlabs.tt.player.gauntlet.Intel.PeonState[] states = new com.oddlabs.tt.player.gauntlet.Intel.PeonState[] { com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK, com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN };
                System.Type[] types = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.RubberSupply) };
                int recalled = 0;
                for (int t = 0; t < states.Length; t++)
                {
                    int n = ai.intel().countLinkedGatherers(states[t], armory);
                    if (n > 0)
                    {
                        owner.recallGatherers(armory, com.oddlabs.tt.player.gauntlet.Economy.supplyClass(types[t]), n);
                        recalled += n;
                    }
                }
                if (recalled > 0)
                {
                    ai.log("recalling " + (recalled) + " gatherers of the old armory at " + (armory.getGridX()) + "," + (armory.getGridY()));
                }
            }
            int workers = armory.getUnitContainer().getNumSupplies();
            if (workers == 0)
            {
                return;
            }
            int iron = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
            int chicken = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int rock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            int left = workers - deployWarriors(armory, workers, chicken, iron, rock);
            bool can_make = canForge(armory);
            int pending = armory.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            bool refuge_v = ((object)armory == (object)refuge_armory) && ai.time() < refuge_until;
            if (!raid_evacs.isEmpty() && raidRefuge(armory))
            {
                refuge_v = true;
            }
            if (((object)lock_old == (object)armory) && lockHold(armory))
            {
                refuge_v = true;
            }
            if (!can_make && left > 0 && pending == 0 && !refuge_v)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON, left);
            }
        }
        // Java: 3244
        public bool canForge(com.oddlabs.tt.model.Building armory)
        {
            return com.oddlabs.tt.player.gauntlet.Economy.stock(armory, typeof(com.oddlabs.tt.landscape.TreeSupply)) >= 2 && (com.oddlabs.tt.player.gauntlet.Economy.stock(armory, typeof(com.oddlabs.tt.model.IronSupply)) >= 1 || ((rock_weapons || rock_filler) && com.oddlabs.tt.player.gauntlet.Economy.stock(armory, typeof(com.oddlabs.tt.model.RockSupply)) >= 1));
        }
        // Java: 3249
        public void orderWeapons(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            bool synced = ((object)armory == (object)sync_armory);
            if (!(synced && sync_paused[com.oddlabs.tt.player.gauntlet.Economy.SYNC_IRON]) && armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() == 0)
            {
                owner.buildIronWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            if (!(synced && sync_paused[com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER]) && owner.canUseRubber() && armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies() == 0)
            {
                owner.buildRubberWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            int rock_orders = armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            bool make_rock = wantsRockWeapons();
            if (make_rock && rock_orders == 0 && !(synced && sync_paused[com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK]))
            {
                owner.buildRockWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            else if (!make_rock && rock_orders > 0 && !(synced && (sync_helping & 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK) != 0))
            {
                owner.buildRockWeapons(armory, -rock_orders, false);
            }
        }
        // Java: 3267
        public bool wantsRockWeapons()
        {
            return rock_weapons || rock_filler || ai.strategy().rock_share > 0.0f || rock_stream_on;
        }
        // Java: 3271
        public void deployFromPrimary(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            int workers = armory.getUnitContainer().getNumSupplies();
            int iron = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
            int chicken = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int rock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            int stock_v = iron + chicken + rock;
            if (stock_v == 0 || workers == 0)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            int pop = owner.getUnitCountContainer().getNumSupplies();
            bool capped = pop >= owner.getWorld().getMaxUnitCount() - 3;
            int deploy = default;
            if (military.wantsEverything() || capped)
            {
                deploy = stock_v;
            }
            else
            {
                deploy = 0;
                deploy = java.lang.Math.max(deploy, java.lang.Math.min(chicken, military.towerSeatsFree()));
                float deficit = (float)(military.armyStrengthWanted() - military.armyStrength());
                if (deficit > 0)
                {
                    deploy = java.lang.Math.max(deploy, java.lang.JC.d2i(java.lang.Math.ceil(deficit)));
                }
                if (workers > want_workers + 2)
                {
                    deploy = java.lang.Math.max(deploy, workers - want_workers);
                }
                if (stock_v > 12)
                {
                    deploy = java.lang.Math.max(deploy, stock_v - 12);
                }
            }
            int keep = (military.wantsEverything() ? 0 : java.lang.Math.min(2, workers));
            deploy = java.lang.Math.min(deploy, java.lang.Math.min(stock_v, workers - keep));
            if (deploy <= 0)
            {
                return;
            }
            deployWarriors(armory, deploy, chicken, iron, rock);
        }
        // Java: 3312
        public int deployWarriors(com.oddlabs.tt.model.Building armory, int n, int chicken, int iron, int rock)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            int c = java.lang.Math.min(chicken, n);
            if (c > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, c);
            }
            int i = java.lang.Math.min(iron, n - c);
            if (i > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, i);
            }
            int r = java.lang.Math.min(rock, n - c - i);
            if (r > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, r);
            }
            return c + i + r;
        }
        public const int SYNC_ROCK = 0;
        public const int SYNC_IRON = 1;
        public const int SYNC_RUBBER = 2;
        public static string[] SYNC_NAMES;
        public static System.Type[] SYNC_WEAPONS;
        public static float[] SYNC_WORK;
        public static System.Type[] SYNC_SUPPLIES;
        public static string[] SYNC_SUPPLY_NAMES;
        public static int[][] SYNC_COST;
        public const int SYNC_HORIZON = 250;
        public const int SYNC_DEPTH = 5;
        public const int SYNC_REACH = 12;
        public com.oddlabs.tt.model.Building sync_armory;
        public bool[] sync_paused;
        public int sync_tick;
        public float[] sync_progress;
        public float[] sync_ms;
        public int[] sync_weapons;
        public int[] sync_stock;
        public float[] sync_expect;
        public java.util.Map sync_carried;
        public int sync_targets;
        public int sync_held;
        public int sync_helping;
        public int sync_attempts;
        public int sync_due;
        public long sync_failed;
        public int sync_expect_ok;
        public int sync_expect_off;
        public int sync_rubber;
        // Java: 3398
        public void weaponSyncObserve()
        {
            com.oddlabs.tt.model.Building armory = sync_armory;
            if (armory == null || armory.isDead() || sync_tick != ai.ticks() - 1)
            {
                return;
            }
            int done = 0;
            for (int c = 0; c < 3; c++)
            {
                float progress = com.oddlabs.tt.player.gauntlet.Economy.syncQueue(armory, c).getBuildProgress();
                int weapons = armory.getSupplyContainer(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WEAPONS[c]).getNumSupplies();
                if (sync_progress[c] > 0.0f && progress == 0.0f && weapons > sync_weapons[c])
                {
                    done |= 1 << c;
                }
            }
            if (done == 0)
            {
                return;
            }
            int[] stock_v = com.oddlabs.tt.player.gauntlet.Economy.syncStock(armory);
            int[] delivered = new int[4];
            foreach (java.util.Map.Entry e in java.lang.JIter.of(sync_carried.entrySet()))
            {
                com.oddlabs.tt.model.UnitSupplyContainer load = ((com.oddlabs.tt.model.Unit)e.getKey()).getSupplyContainer();
                int[] was = ((int[])e.getValue());
                int now = (load != null && com.oddlabs.tt.player.gauntlet.Economy.syncSupplyIndex(load.getSupplyType()) == was[0] ? load.getNumSupplies() : 0);
                delivered[was[0]] += java.lang.Math.max(0, was[1] - now);
            }
            bool dup = false;
            if (java.lang.Integer.bitCount(done) >= 2)
            {
                for (int s = 0; s < 4; s++)
                {
                    int users = 0;
                    int cost = 0;
                    for (int c_2 = 0; c_2 < 3; c_2++)
                    {
                        if ((done & 1 << c_2) != 0 && com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[c_2][s] > 0)
                        {
                            users++;
                            cost += com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[c_2][s];
                        }
                    }
                    if (users >= 2 && sync_stock[s] - stock_v[s] < cost && sync_stock[s] + delivered[s] < cost)
                    {
                        dup = true;
                    }
                }
            }
            if ((done & 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER) != 0)
            {
                sync_rubber++;
            }
            if (dup)
            {
                ai.aiLog().count("weapon_sync_dup");
            }
            int targets = sync_targets;
            string outcome = "";
            if (targets != 0 && (done & targets) != 0)
            {
                bool hit = (done & targets) == targets;
                if (!hit)
                {
                    ai.aiLog().count("weapon_sync_missed");
                }
                outcome = java.lang.JString.format("#%d %s (%s, due %s)", sync_attempts, (hit ? (object)("hit") : (object)("missed")), com.oddlabs.tt.player.gauntlet.Economy.syncNames(targets), (sync_due > 0 ? (object)(java.lang.Integer.toString(sync_due - ai.ticks())) : (object)("-")));
                sync_targets = 0;
                sync_due = 0;
                sync_held = 0;
            }
            if (targets != 0 || dup || java.lang.Integer.bitCount(done) >= 2 || (done & 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER) != 0)
            {
                string what = outcome;
                bool duplicated = dup;
                int finished = done;
                ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                {
                    java.lang.StringBuilder sb = new java.lang.StringBuilder();
                    sb.append((what.isEmpty() ? "done" : what)).append(": ").append(com.oddlabs.tt.player.gauntlet.Economy.syncNames(finished));
                    if ((finished & 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER) != 0)
                    {
                        sb.append(" (rubber axe ").append(sync_rubber).append(')');
                    }
                    sb.append((duplicated ? ", dup" : "")).append(", stock");
                    for (int s_2 = 0; s_2 < 4; s_2++)
                    {
                        sb.append(' ').append(com.oddlabs.tt.player.gauntlet.Economy.SYNC_SUPPLY_NAMES[s_2]).append(' ').append(sync_stock[s_2]).append("->").append(stock_v[s_2]);
                        if (delivered[s_2] > 0)
                        {
                            sb.append(" (<=").append(delivered[s_2]).append(" in)");
                        }
                    }
                    sb.append(", predicted ").append(sync_expect_ok).append('/').append(sync_expect_ok + sync_expect_off);
                    return java.lang.JO.toString(sb);
                }));
            }
        }
        // Java: 3476
        public void weaponSync(float t)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            com.oddlabs.tt.model.Building armory = ai.intel().armory();
            if (armory != null && (armory.isDead() || !armory.isComplete()))
            {
                armory = null;
            }
            if (((object)armory != (object)sync_armory))
            {
                syncRelease("main armory changed");
                sync_armory = armory;
                sync_tick = (-100);
            }
            if (armory == null || !owner.isAlive())
            {
                return;
            }
            bool @checked = sync_tick == ai.ticks() - 1;
            com.oddlabs.tt.model.BuildProductionContainer[] queues = new com.oddlabs.tt.model.BuildProductionContainer[3];
            float[] ms = new float[3];
            for (int c = 0; c < 3; c++)
            {
                queues[c] = com.oddlabs.tt.player.gauntlet.Economy.syncQueue(armory, c);
                ms[c] = com.oddlabs.tt.player.gauntlet.Economy.syncManSeconds(queues[c].getBuildProgress(), com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c], (@checked ? sync_expect[c] : java.lang.Float.NaN), (@checked ? sync_ms[c] : java.lang.Float.NaN));
                if (@checked && ms[c] == sync_expect[c])
                {
                    sync_expect_ok++;
                }
                else if (@checked)
                {
                    sync_expect_off++;
                }
            }
            int workers = armory.getUnitContainer().getNumSupplies();
            int[] stock_v = com.oddlabs.tt.player.gauntlet.Economy.syncStock(armory);
            float step = (float)((float)(owner.getWorld().getSecondsPerTick() * t) / com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK);
            bool three = ai.strategy().weapon_sync_three;
            bool[] wanted = new bool[] { wantsRockWeapons(), true, owner.canUseRubber() };
            int avail = 0;
            int forced = 0;
            int helpers = 0;
            for (int c_2 = 0; c_2 < 3; c_2++)
            {
                bool pausable = c_2 != com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK || three;
                if (wanted[c_2] || !pausable)
                {
                    sync_helping &= ~(1 << c_2);
                }
                if (!com.oddlabs.tt.player.gauntlet.Economy.syncAffordable(stock_v, c_2))
                {
                    continue;
                }
                if (wanted[c_2] && pausable)
                {
                    avail |= 1 << c_2;
                }
                else if ((sync_helping & 1 << c_2) != 0 || (pausable && queues[c_2].getNumSupplies() == 0))
                {
                    helpers |= 1 << c_2;
                }
                else if (queues[c_2].getNumSupplies() > 0)
                {
                    avail |= 1 << c_2;
                    forced |= 1 << c_2;
                }
            }
            int free = avail & ~forced;
            int targets = 0;
            if ((free & 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER) != 0)
            {
                for (int c_3 = com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK; c_3 <= com.oddlabs.tt.player.gauntlet.Economy.SYNC_IRON; c_3++)
                {
                    if ((free & 1 << c_3) != 0 && com.oddlabs.tt.player.gauntlet.Economy.syncShortShared(stock_v, c_3))
                    {
                        targets |= 1 << c_3;
                    }
                }
                if (targets != 0)
                {
                    targets |= 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER;
                }
            }
            if (targets == 0)
            {
                if (sync_targets != 0)
                {
                    syncRelease(((free & 1 << com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER) == 0 ? "no rubber axe to forge" : "nothing short"));
                }
                else
                {
                    syncRelease("");
                }
                syncSnapshot(armory, queues, ms, stock_v, workers, step);
                return;
            }
            int held = sync_held;
            if (sync_targets != 0 && held != 0 && (targets & held) == held && java.lang.Integer.bitCount(targets & ~held) >= 2)
            {
                targets &= ~held;
                avail &= ~held;
            }
            else
            {
                sync_held = 0;
            }
            if (sync_targets == 0)
            {
                sync_attempts++;
                sync_failed = (-1);
                int shown = targets;
                ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("#%d start %s: %s, %d peons", sync_attempts, com.oddlabs.tt.player.gauntlet.Economy.syncNames(shown), com.oddlabs.tt.player.gauntlet.Economy.syncState(ms, stock_v), workers);
                }));
            }
            sync_targets = targets;
            if (workers > 0)
            {
                int run = syncPlan(ms, avail, forced, helpers, workers, step);
                for (int c_4 = 0; c_4 < 3; c_4++)
                {
                    if ((forced & 1 << c_4) != 0 || (c_4 == com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK && !three))
                    {
                        continue;
                    }
                    int orders = queues[c_4].getNumSupplies();
                    bool helper = (helpers & 1 << c_4) != 0;
                    int queue = c_4;
                    if ((run & 1 << c_4) != 0 && orders == 0)
                    {
                        syncOrder(armory, c_4, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                        sync_paused[c_4] = false;
                        if (helper)
                        {
                            sync_helping |= 1 << c_4;
                        }
                        ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                        {
                            return java.lang.JString.format("#%d %s %s: %s", sync_attempts, (helper ? (object)("helper on") : (object)("resume")), com.oddlabs.tt.player.gauntlet.Economy.SYNC_NAMES[queue], com.oddlabs.tt.player.gauntlet.Economy.syncState(ms, stock_v));
                        }));
                    }
                    else if ((run & 1 << c_4) == 0 && ((avail | sync_held | helpers) & 1 << c_4) != 0 && orders > 0)
                    {
                        syncOrder(armory, c_4, -orders, false);
                        if (helper)
                        {
                            sync_helping &= ~(1 << c_4);
                            ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                            {
                                return java.lang.JString.format("#%d helper off %s: %s", sync_attempts, com.oddlabs.tt.player.gauntlet.Economy.SYNC_NAMES[queue], com.oddlabs.tt.player.gauntlet.Economy.syncState(ms, stock_v));
                            }));
                        }
                        else
                        {
                            sync_paused[c_4] = true;
                            ai.aiLog().count("weapon_sync_pause");
                            ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                            {
                                return java.lang.JString.format("#%d pause %s: %s", sync_attempts, com.oddlabs.tt.player.gauntlet.Economy.SYNC_NAMES[queue], com.oddlabs.tt.player.gauntlet.Economy.syncState(ms, stock_v));
                            }));
                        }
                    }
                }
            }
            syncSnapshot(armory, queues, ms, stock_v, workers, step);
        }
        // Java: 3599
        public int syncPlan(float[] ms, int avail, int forced, int helpers, int workers, float step)
        {
            int targets = sync_targets;
            int k = com.oddlabs.tt.player.gauntlet.Economy.syncAlignedIn(ms, avail, targets, workers, step);
            if (k > 0)
            {
                return syncAligned(avail, targets, k);
            }
            sync_due = 0;
            int catch_up = com.oddlabs.tt.player.gauntlet.Economy.syncCatchUp(ms, avail, targets, workers, step);
            bool close = k == 0 && com.oddlabs.tt.player.gauntlet.Economy.syncSpread(ms, targets) <= (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_DEPTH * ((float)(workers * step)));
            long key = (((long)(workers)) << 16) | (((long)(helpers)) << 12) | (((long)(avail)) << 8) | (((long)(forced)) << 4) | targets;
            if (close && key != sync_failed)
            {
                int run = com.oddlabs.tt.player.gauntlet.Economy.syncSearch(ms, avail, forced, helpers, targets, workers, step);
                if (run != 0)
                {
                    return run;
                }
                if (catch_up == avail && java.lang.Integer.bitCount(targets) == 3)
                {
                    for (int drop = com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK; drop <= com.oddlabs.tt.player.gauntlet.Economy.SYNC_IRON; drop++)
                    {
                        int pair = targets & ~(1 << drop);
                        int rest = avail & ~(1 << drop);
                        int kp = com.oddlabs.tt.player.gauntlet.Economy.syncAlignedIn(ms, rest, pair, workers, step);
                        if (kp > 0)
                        {
                            syncHold(drop, pair);
                            return syncAligned(rest, pair, kp);
                        }
                        if (kp == 0)
                        {
                            run = com.oddlabs.tt.player.gauntlet.Economy.syncSearch(ms, rest, forced, helpers, pair, workers, step);
                            if (run != 0)
                            {
                                syncHold(drop, pair);
                                return run;
                            }
                        }
                    }
                }
            }
            sync_failed = (close && catch_up == avail ? key : (-1));
            return catch_up;
        }
        // Java: 3638
        public static float syncSpread(float[] ms, int targets)
        {
            float lo = java.lang.Float.MAX_VALUE;
            float hi = 0.0f;
            for (int c = 0; c < 3; c++)
            {
                if ((targets & 1 << c) != 0)
                {
                    lo = java.lang.Math.min(lo, (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c] - ms[c]));
                    hi = java.lang.Math.max(hi, (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c] - ms[c]));
                }
            }
            return (float)(hi - lo);
        }
        // Java: 3651
        public int syncAligned(int run, int targets, int k)
        {
            int due = ai.ticks() + k;
            if (due != sync_due)
            {
                ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("#%d aligned %s: due in %d ticks", sync_attempts, com.oddlabs.tt.player.gauntlet.Economy.syncNames(targets), k);
                }));
            }
            sync_due = due;
            return run;
        }
        // Java: 3661
        public void syncHold(int drop, int pair)
        {
            sync_targets = pair;
            sync_held = 1 << drop;
            ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
            {
                return java.lang.JString.format("#%d three cannot meet: %s, holding %s", sync_attempts, com.oddlabs.tt.player.gauntlet.Economy.syncNames(pair), com.oddlabs.tt.player.gauntlet.Economy.SYNC_NAMES[drop]);
            }));
        }
        // Java: 3672
        public static int syncAlignedIn(float[] ms, int run, int targets, int workers, float step)
        {
            float share = (float)((float)(workers * step) / java.lang.Integer.bitCount(run));
            int k = 0;
            bool apart = false;
            for (int c = 0; c < 3; c++)
            {
                if ((targets & 1 << c) == 0)
                {
                    continue;
                }
                int kc = com.oddlabs.tt.player.gauntlet.Economy.syncTicksToFinish(ms[c], com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c], share);
                if (kc > com.oddlabs.tt.player.gauntlet.Economy.SYNC_HORIZON)
                {
                    return (-1);
                }
                if (k == 0)
                {
                    k = kc;
                }
                else if (kc != k)
                {
                    apart = true;
                }
            }
            return (apart ? 0 : k);
        }
        // Java: 3693
        public static int syncTicksToFinish(float ms, float work, float share)
        {
            if (share <= 0.0f)
            {
                return com.oddlabs.tt.player.gauntlet.Economy.SYNC_HORIZON + 1;
            }
            float m = ms;
            for (int k = 1; k <= com.oddlabs.tt.player.gauntlet.Economy.SYNC_HORIZON; k++)
            {
                m = (float)(m + share);
                if (m >= work)
                {
                    return k;
                }
            }
            return com.oddlabs.tt.player.gauntlet.Economy.SYNC_HORIZON + 1;
        }
        // Java: 3711
        public static int syncSearch(float[] ms, int avail, int forced, int helpers, int targets, int workers, float step)
        {
            int[] moves = new int[7];
            int m = 0;
            for (int s = 1; s < 8; s++)
            {
                if ((s & ~(avail | helpers)) == 0 && (s & forced) == forced && s != avail)
                {
                    moves[m++] = s;
                }
            }
            if (m == 0)
            {
                return 0;
            }
            int[] seq = new int[com.oddlabs.tt.player.gauntlet.Economy.SYNC_DEPTH];
            float[] sim = new float[3];
            for (int depth = 1; depth <= com.oddlabs.tt.player.gauntlet.Economy.SYNC_DEPTH; depth++)
            {
                java.util.Arrays.fill(seq, 0);
                int best = 0;
                int best_pauses = java.lang.Integer.MAX_VALUE;
                while (true)
                {
                    int pauses = 0;
                    for (int i = 0; i < depth; i++)
                    {
                        pauses += java.lang.Integer.bitCount(avail & ~moves[seq[i]]) + java.lang.Integer.bitCount(helpers & moves[seq[i]]);
                    }
                    if (pauses < best_pauses && com.oddlabs.tt.player.gauntlet.Economy.syncPlanWorks(ms, sim, moves, seq, depth, avail, helpers, targets, workers, step))
                    {
                        best = moves[seq[0]];
                        best_pauses = pauses;
                    }
                    int i_2 = depth - 1;
                    while (i_2 >= 0 && seq[i_2] == m - 1)
                    {
                        i_2--;
                    }
                    if (i_2 < 0)
                    {
                        break;
                    }
                    seq[i_2]++;
                    for (int j = i_2 + 1; j < depth; j++)
                    {
                        seq[j] = seq[i_2];
                    }
                }
                if (best != 0)
                {
                    return best;
                }
            }
            return 0;
        }
        // Java: 3750
        public static bool syncPlanWorks(float[] ms, float[] sim, int[] moves, int[] seq, int depth, int avail, int helpers, int targets, int workers, float step)
        {
            java.lang.JSystem.arraycopy(ms, 0, sim, 0, 3);
            for (int i = 0; i < depth; i++)
            {
                int run = moves[seq[i]];
                float share = (float)((float)(workers * step) / java.lang.Integer.bitCount(run));
                int finished = 0;
                for (int c = 0; c < 3; c++)
                {
                    if ((run & 1 << c) == 0)
                    {
                        continue;
                    }
                    float v = (float)(sim[c] + share);
                    if (v >= com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c])
                    {
                        finished |= 1 << c;
                        v = 0.0f;
                    }
                    sim[c] = v;
                }
                if ((finished & helpers) != 0)
                {
                    return false;
                }
                if ((finished & targets) != 0)
                {
                    return (finished & targets) == targets;
                }
            }
            return com.oddlabs.tt.player.gauntlet.Economy.syncAlignedIn(sim, avail, targets, workers, step) > 0;
        }
        // Java: 3777
        public static int syncCatchUp(float[] ms, int avail, int targets, int workers, float step)
        {
            float top = 0.0f;
            for (int c = 0; c < 3; c++)
            {
                if ((targets & 1 << c) != 0)
                {
                    top = java.lang.Math.max(top, (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c] - ms[c]));
                }
            }
            float slack = (float)(workers * step);
            int run = avail;
            for (int c_2 = 0; c_2 < 3; c_2++)
            {
                if ((targets & 1 << c_2) != 0 && (float)(top - ((float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c_2] - ms[c_2]))) > slack)
                {
                    run &= ~(1 << c_2);
                }
            }
            return run;
        }
        // Java: 3791
        public void syncRelease(string why)
        {
            if (sync_targets != 0)
            {
                int n = sync_attempts;
                ai.aiLog().log("WSYNC", new java.util.function.Supplier_L(() =>
                {
                    return java.lang.JString.format("#%d released: %s", n, why);
                }));
                sync_targets = 0;
                sync_due = 0;
            }
            sync_held = 0;
            com.oddlabs.tt.model.Building armory = sync_armory;
            bool alive = armory != null && !armory.isDead() && armory.isComplete();
            if (sync_helping != 0 && alive && !wantsRockWeapons())
            {
                int orders = com.oddlabs.tt.player.gauntlet.Economy.syncQueue(armory, com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK).getNumSupplies();
                if (orders > 0)
                {
                    syncOrder(armory, com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK, -orders, false);
                }
            }
            sync_helping = 0;
            for (int c = 0; c < 3; c++)
            {
                if (!sync_paused[c])
                {
                    continue;
                }
                sync_paused[c] = false;
                bool wanted = (c == com.oddlabs.tt.player.gauntlet.Economy.SYNC_ROCK ? wantsRockWeapons() : c == com.oddlabs.tt.player.gauntlet.Economy.SYNC_IRON || ai.owner().canUseRubber());
                if (alive && wanted && com.oddlabs.tt.player.gauntlet.Economy.syncQueue(armory, c).getNumSupplies() == 0)
                {
                    syncOrder(armory, c, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                }
            }
        }
        // Java: 3818
        public void syncSnapshot(com.oddlabs.tt.model.Building armory, com.oddlabs.tt.model.BuildProductionContainer[] queues, float[] ms, int[] stock_v, int workers, float step)
        {
            int run = 0;
            for (int c = 0; c < 3; c++)
            {
                if (queues[c].getNumSupplies() > 0 && com.oddlabs.tt.player.gauntlet.Economy.syncAffordable(stock_v, c))
                {
                    run |= 1 << c;
                }
            }
            float share = (run != 0 ? (float)((float)(workers * step) / java.lang.Integer.bitCount(run)) : 0.0f);
            for (int c_2 = 0; c_2 < 3; c_2++)
            {
                float next = ms[c_2];
                if ((run & 1 << c_2) != 0)
                {
                    next = (float)(next + share);
                    if (next >= com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[c_2])
                    {
                        next = 0.0f;
                    }
                }
                sync_expect[c_2] = next;
                sync_ms[c_2] = ms[c_2];
                sync_progress[c_2] = queues[c_2].getBuildProgress();
                sync_weapons[c_2] = armory.getSupplyContainer(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WEAPONS[c_2]).getNumSupplies();
            }
            java.lang.JSystem.arraycopy(stock_v, 0, sync_stock, 0, 4);
            sync_carried.clear();
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if (!((s is com.oddlabs.tt.model.Unit u)) || u.isDead() || java.lang.Math.abs(u.getGridX() - ax) > com.oddlabs.tt.player.gauntlet.Economy.SYNC_REACH || java.lang.Math.abs(u.getGridY() - ay) > com.oddlabs.tt.player.gauntlet.Economy.SYNC_REACH)
                {
                    continue;
                }
                com.oddlabs.tt.model.UnitSupplyContainer load = u.getSupplyContainer();
                if (load == null || load.getNumSupplies() == 0)
                {
                    continue;
                }
                int supply = com.oddlabs.tt.player.gauntlet.Economy.syncSupplyIndex(load.getSupplyType());
                if (supply >= 0)
                {
                    sync_carried.put(u, new int[] { supply, load.getNumSupplies() });
                }
            }
            sync_tick = ai.ticks();
        }
        // Java: 3859
        public static float syncManSeconds(float progress, float work, float planned, float unchanged)
        {
            if (progress == 0.0f)
            {
                return 0.0f;
            }
            if (!java.lang.Float.isNaN(planned) && (float)(planned / work) == progress)
            {
                return planned;
            }
            if (!java.lang.Float.isNaN(unchanged) && (float)(unchanged / work) == progress)
            {
                return unchanged;
            }
            float guess = (float)(progress * work);
            float best = guess;
            float best_d = java.lang.Float.MAX_VALUE;
            float f = java.lang.Math.nextDown(java.lang.Math.nextDown(java.lang.Math.nextDown(guess)));
            for (int i = 0; i < 7; i++, f = java.lang.Math.nextUp(f))
            {
                if ((float)(f / work) == progress && java.lang.Math.abs((float)(f - guess)) < best_d)
                {
                    best = f;
                    best_d = java.lang.Math.abs((float)(f - guess));
                }
            }
            return best;
        }
        // Java: 3880
        public static bool syncAffordable(int[] stock_v, int c)
        {
            for (int s = 0; s < 4; s++)
            {
                if (stock_v[s] < com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[c][s])
                {
                    return false;
                }
            }
            return true;
        }
        // Java: 3888
        public static bool syncShortShared(int[] stock_v, int c)
        {
            for (int s = 0; s < 4; s++)
            {
                if (com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[c][s] > 0 && com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER][s] > 0 && stock_v[s] < com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[c][s] + com.oddlabs.tt.player.gauntlet.Economy.SYNC_COST[com.oddlabs.tt.player.gauntlet.Economy.SYNC_RUBBER][s])
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 3896
        public static int syncSupplyIndex(System.Type type)
        {
            for (int s = 0; s < 4; s++)
            {
                if (((object)com.oddlabs.tt.player.gauntlet.Economy.SYNC_SUPPLIES[s] == (object)type))
                {
                    return s;
                }
            }
            return (-1);
        }
        // Java: 3903
        public static com.oddlabs.tt.model.BuildProductionContainer syncQueue(com.oddlabs.tt.model.Building armory, int c)
        {
            return ((com.oddlabs.tt.model.BuildProductionContainer)armory.getBuildSupplyContainer(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WEAPONS[c]));
        }
        // Java: 3907
        public static int[] syncStock(com.oddlabs.tt.model.Building armory)
        {
            int[] stock_v = new int[4];
            for (int s = 0; s < 4; s++)
            {
                stock_v[s] = armory.getSupplyContainer(com.oddlabs.tt.player.gauntlet.Economy.SYNC_SUPPLIES[s]).getNumSupplies();
            }
            return stock_v;
        }
        // Java: 3914
        public void syncOrder(com.oddlabs.tt.model.Building armory, int c, int count, bool infinite)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            switch ((c))
            {
                case 0:
                {
                    owner.buildRockWeapons(armory, count, infinite);
                    break;
                }
                case 1:
                {
                    owner.buildIronWeapons(armory, count, infinite);
                    break;
                }
                default:
                {
                    owner.buildRubberWeapons(armory, count, infinite);
                    break;
                }
            }
        }
        // Java: 3923
        public static string syncNames(int queues)
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            for (int c = 0; c < 3; c++)
            {
                if ((queues & 1 << c) != 0)
                {
                    sb.append((sb.isEmpty() ? "" : "+")).append(com.oddlabs.tt.player.gauntlet.Economy.SYNC_NAMES[c]);
                }
            }
            return java.lang.JO.toString(sb);
        }
        // Java: 3931
        public static string syncState(float[] ms, int[] stock_v)
        {
            return java.lang.JString.format("left rock %.2f iron %.2f rubber %.2f, stock %d/%d/%d/%d", (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[0] - ms[0]), (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[1] - ms[1]), (float)(com.oddlabs.tt.player.gauntlet.Economy.SYNC_WORK[2] - ms[2]), stock_v[0], stock_v[1], stock_v[2], stock_v[3]);
        }
        // Java: 3936
        public void computeGatherTargets()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            if (armory == null || armory_field == null)
            {
                want_tree = want_iron = want_rock = want_chicken = want_workers = 0;
                if (ai.strategy().reloc_lock > 0)
                {
                    trackLock(null);
                }
                return;
            }
            com.oddlabs.tt.player.gauntlet.MapAnalysis map = ai.map();
            float harvest = ai.strategy().harvest_seconds;
            tree_cycle = com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(armory_field, map.getTrees(), 60, 10, 120, harvest);
            int reach = ai.strategy().wood_reach;
            wood_starved = reach > 0 && ai.time() >= ai.strategy().wood_reach_time && (tree_cycle >= 90.0f || ai.time() < tree_null_until);
            if (wood_starved)
            {
                tree_cycle = com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(armory_field, map.getTrees(), 60, 10, 2 * reach, harvest);
                ai.aiLog().count("wood_starved");
            }
            iron_cycle = com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(armory_field, map.getIron(), 30, 10, 400, harvest);
            iron_cycle_armory = armory;
            int iron_left = countReachable(map.getIron(), 400);
            rock_weapons = (iron_left == 0 || iron_cycle > 200.0f) && armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies() < 2;
            int iron_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies();
            int armory_workers = armory.getUnitContainer().getNumSupplies();
            if (ai.strategy().hold_backlog > 0 && armory.isComplete())
            {
                int forge = java.lang.Math.min(iron_stock, armory.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies() / 2);
                float until = ai.strategy().hold_backlog_until;
                if (!backlog_on && forge >= ai.strategy().hold_backlog && (until <= 0.0f || ai.time() < until))
                {
                    backlog_on = true;
                    backlog_since = ai.time();
                    ai.aiLog().count("backlog_on");
                }
                else if (backlog_on && ((forge <= 1 && (float)(ai.time() - backlog_since) >= 30.0f) || (until > 0.0f && ai.time() >= until)))
                {
                    backlog_on = false;
                }
            }
            if (!rock_filler && iron_stock <= 1 && armory_workers >= ai.strategy().rock_filler_min_workers && iron_cycle > 45.0f)
            {
                rock_filler = true;
            }
            else if (rock_filler && (iron_stock >= 5 || armory_workers < 8))
            {
                rock_filler = false;
            }
            float ore_cycle = (rock_weapons ? com.oddlabs.tt.player.gauntlet.SitePlanner.gatherSeconds(armory_field, map.getRocks(), 30, 10, 240, harvest) : iron_cycle);
            float work = (rock_weapons ? (float)(com.oddlabs.tt.player.gauntlet.Economy.IRON_WORK / 2) : com.oddlabs.tt.player.gauntlet.Economy.IRON_WORK);
            int workers = armory.getUnitContainer().getNumSupplies();
            int g_tree = intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE, armory);
            int g_iron = intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON, armory);
            int g_rock = intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK, armory);
            int g_chicken = intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN, armory);
            int transit = intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT);
            int pool = workers + g_tree + g_iron + g_rock + g_chicken + transit;
            want_chicken = 0;
            int chickens_left = countChickens();
            com.oddlabs.tt.player.gauntlet.Strategy strategy = ai.strategy();
            if (ai.owner().canUseRubber() && ai.time() > strategy.chicken_time && pool > 10 && chickens_left > 0)
            {
                want_chicken = java.lang.Math.min(java.lang.Math.min(strategy.chicken_hunters, chickens_left), 2 + pool / strategy.chicken_pool_div);
            }
            int rock_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies();
            int chicken_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies();
            want_rock = ((want_chicken > 0 || chicken_stock > 0) && rock_stock < 6 ? 1 + want_chicken / 4 : 0);
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            if (!st.rock_surge && rock_filler && !rock_weapons && rock_stock < st.rock_filler_stock)
            {
                want_rock += java.lang.Math.max(2, armory_workers / st.rock_filler_div);
            }
            pool -= want_chicken + want_rock;
            float per_weapon = (float)((float)(work + (float)(2 * tree_cycle)) + ore_cycle);
            float x = (float)(java.lang.Math.max(0, pool) / per_weapon);
            int tree_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies();
            int ore_stock = armory.getSupplyContainer((rock_weapons ? typeof(com.oddlabs.tt.model.RockSupply) : typeof(com.oddlabs.tt.model.IronSupply))).getNumSupplies();
            float tree_adj = (tree_stock > 40 ? 0.35f : (tree_stock > 20 ? 0.7f : (tree_stock < 6 ? 1.15f : 1.0f)));
            float ore_adj = (ore_stock > 20 ? 0.35f : (ore_stock > 10 ? 0.7f : (ore_stock < 3 ? 1.15f : 1.0f)));
            want_tree = java.lang.Math.round((float)((float)((float)(2 * tree_cycle) * x) * tree_adj));
            int want_ore = java.lang.Math.round((float)((float)(ore_cycle * x) * ore_adj));
            if (pool >= 3)
            {
                want_tree = java.lang.Math.max(1, want_tree);
                want_ore = java.lang.Math.max(1, want_ore);
            }
            float rock_share = ai.strategy().rock_share;
            if (rock_weapons)
            {
                want_rock += want_ore;
                want_iron = 0;
            }
            else if (rock_share > 0.0f && want_ore > 0)
            {
                int rock = java.lang.Math.round((float)(want_ore * rock_share));
                want_rock += rock;
                want_iron = want_ore - rock;
            }
            else
            {
                want_iron = want_ore;
            }
            want_workers = java.lang.Math.max(2, pool - want_tree - want_ore);
            rock_stream_on = st.rock_stream && ai.time() >= st.rock_stream_time && iron_s > st.rock_stream_iron_s && !rock_weapons;
            if (rock_stream_on)
            {
                int add = java.lang.Math.min(st.rock_stream_max, java.lang.Math.max(0, java.lang.Math.min(want_workers - 4, armory_workers / 2)));
                if (iron_s > 150.0f && want_iron > 6)
                {
                    add += want_iron - 6;
                    want_iron = 6;
                }
                int wood = add / 3;
                want_rock += add - wood;
                want_tree += wood;
                want_workers = java.lang.Math.max(2, want_workers - add);
                ai.aiLog().count("rock_stream");
            }
            if (st.rock_surge && rock_filler && !rock_weapons && rock_stock < st.rock_filler_stock)
            {
                int filler = java.lang.Math.min(want_workers - 2, java.lang.Math.max(2, armory_workers / st.rock_filler_div));
                if (filler > 0)
                {
                    want_rock += filler;
                    want_workers -= filler;
                }
            }
            if (st.reloc_lock > 0)
            {
                trackLock(armory);
            }
        }
        // Java: 4069
        public int countReachable(java.util.List supplies, int max_meters)
        {
            if (armory_field == null)
            {
                return 0;
            }
            int n = 0;
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                if (armory_field.getAround(s.getGridX(), s.getGridY(), 1) <= max_meters)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 4085
        public void allocatePeons()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            java.util.List free = new java.util.ArrayList();
            java.util.List transit = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE))
                {
                    free.add(peon);
                }
                else if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT))
                {
                    transit.add(peon);
                }
            }
            if (scout != null && (scout.isDead() || !scoutHasWork()))
            {
                scout = (scout.isDead() ? (com.oddlabs.tt.model.Unit)(null) : scout);
            }
            bool wood_drop = ai.strategy().tower_wood_drop && ai.time() >= ai.strategy().tower_wood_time;
            java.util.List carriers = new java.util.ArrayList();
            if (wood_drop)
            {
                for (java.util.Iterator it = free.iterator(); it.hasNext(); )
                {
                    com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)it.next());
                    if (com.oddlabs.tt.player.gauntlet.Economy.carriesWood(u))
                    {
                        it.remove();
                        carriers.add(u);
                    }
                }
            }
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (!p.isPlaced() || retire.isDoomed(p.building))
                {
                    continue;
                }
                if (wood_drop)
                {
                    dropWood(p, carriers);
                }
                int need = buildersWanted(p) - builderCount(p.building);
                if (need <= 0)
                {
                    continue;
                }
                if (ai.strategy().tower_cooldown && p.type == com.oddlabs.tt.model.Race.BUILDING_TOWER && awakeEnemyNear(p.site.x, p.site.y, 12))
                {
                    ai.aiLog().count("tower_builders_held");
                    continue;
                }
                java.util.List chosen = new java.util.ArrayList();
                takeNearest(free, chosen, need, p.site.x, p.site.y);
                takeNearest(transit, chosen, need - chosen.size(), p.site.x, p.site.y);
                if (chosen.size() < need && (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY || p.first))
                {
                    takeGatherers(chosen, need - chosen.size(), p.site.x, p.site.y);
                }
                if ((ai.strategy().expand_under_threat || p.reloc || p.@lock) && ((object)p == (object)expansion_project) && chosen.size() < need && armory != null && armory.isComplete() && ((object)armory != (object)p.building))
                {
                    int workers = armory.getUnitContainer().getNumSupplies();
                    int pending = armory.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
                    if (workers > want_workers + 5 && pending == 0)
                    {
                        ai.owner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON, java.lang.Math.min(need - chosen.size(), workers - want_workers));
                        ai.aiLog().count((p.@lock ? "lock_builders_deployed" : (p.reloc ? "reloc_builders_deployed" : "exp_builders_deployed")));
                    }
                }
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(chosen))
                {
                    if (((object)u_2 == (object)scout) && scoutHasWork())
                    {
                        continue;
                    }
                    order(u_2, p.building, com.oddlabs.tt.model.Action.DEFAULT);
                    intel.peon_states.put(u_2, com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD);
                    intel.builder_sites.put(u_2, p.building);
                }
            }
            free.addAll(carriers);
            com.oddlabs.tt.model.Building trainer = ai.chieftain().trainingQuarters();
            bool topup_ok = ai.military().baseThreatLevel() == 0 || (ai.strategy().chief_topup_any && trainer != null && !ai.military().threatNear(trainer.getGridX(), trainer.getGridY(), 20));
            if (trainer != null && topup_ok && !evacuating.containsKey(trainer))
            {
                bool near = ai.strategy().chief_trainer_near;
                int heading = (near ? countSentTo(trainer) : countHeadingTo(trainer));
                int need_2 = ai.strategy().hold_chieftain - trainer.getUnitContainer().getNumSupplies() - heading;
                if (need_2 > 0)
                {
                    java.util.List chosen_2 = new java.util.ArrayList();
                    takeNearest(free, chosen_2, need_2, trainer.getGridX(), trainer.getGridY());
                    java.util.List walkers = transit;
                    if (near)
                    {
                        walkers = new java.util.ArrayList();
                        foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(transit))
                        {
                            if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_3.getGridX(), u_3.getGridY(), trainer.getGridX(), trainer.getGridY()) <= 40 * 40)
                            {
                                walkers.add(u_3);
                            }
                        }
                    }
                    takeNearest(walkers, chosen_2, need_2 - chosen_2.size(), trainer.getGridX(), trainer.getGridY());
                    if (((object)walkers != (object)transit))
                    {
                        transit.removeAll(chosen_2);
                    }
                    order(chosen_2, trainer, com.oddlabs.tt.model.Action.DEFAULT);
                    if (near)
                    {
                        foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(chosen_2))
                        {
                            topup_sent.put(u_4, trainer);
                            ai.aiLog().count("topup_sent");
                        }
                    }
                }
            }
            if (ai.strategy().seed_quarters > 0.0f)
            {
                seedQuarters(free, armory != null);
            }
            if (armory == null)
            {
                foreach (com.oddlabs.tt.model.Unit u_5 in java.lang.JIter.of(free))
                {
                    com.oddlabs.tt.model.Building q = com.oddlabs.tt.player.gauntlet.MapAnalysis.nearest(intel.quarters, u_5.getGridX(), u_5.getGridY());
                    if (q != null && !evacuating.containsKey(q) && !retire.isDoomed(q) && q.getUnitContainer().getNumSupplies() + countHeadingTo(q) < holdFor(q))
                    {
                        order(u_5, q, com.oddlabs.tt.model.Action.DEFAULT);
                    }
                }
                return;
            }
            bool danger = ai.military().baseThreatLevel() > 1 && (!gatherUnderThreat() || ai.military().threatNearEcon(armory.getGridX(), armory.getGridY(), 16));
            danger_idle = ai.strategy().danger_refuge && danger && armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies() + armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies() < 2;
            int[] have = new int[] { intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE, armory), intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON, armory), intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK, armory), intel.countGatherers(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN, armory) };
            int[] want = new int[] { want_tree, want_iron, want_rock, want_chicken };
            System.Type[] types = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.RubberSupply) };
            rebuildSupplyLoad();
            int deploy_for_gathering = 0;
            bool raid_out = raidEvacuating(armory);
            for (int t = 0; t < 4; t++)
            {
                int need_3 = (danger || raid_out ? 0 : want[t] - have[t]);
                while (need_3 > 0)
                {
                    com.oddlabs.tt.model.Unit u_6 = (free.isEmpty() ? (com.oddlabs.tt.model.Unit)(null) : ((com.oddlabs.tt.model.Unit)free.removeFirst()));
                    if (u_6 == null && !transit.isEmpty())
                    {
                        u_6 = ((com.oddlabs.tt.model.Unit)transit.removeFirst());
                    }
                    if (u_6 == null)
                    {
                        deploy_for_gathering += need_3;
                        break;
                    }
                    if (!sendGatherer(u_6, types[t], armory))
                    {
                        break;
                    }
                    need_3--;
                }
                if (!danger && have[t] > want[t] + 1)
                {
                    ai.owner().recallGatherers(armory, com.oddlabs.tt.player.gauntlet.Economy.supplyClass(types[t]), have[t] - want[t]);
                }
            }
            retargetGatherers(armory);
            unstickGatherers(armory);
            if (ai.strategy().unstick_builders > 0.0f)
            {
                unstickBuilders();
            }
            int workers_2 = armory.getUnitContainer().getNumSupplies();
            int pending_2 = armory.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            if (deploy_for_gathering > 0 && workers_2 > 3 && pending_2 == 0)
            {
                ai.owner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON, java.lang.Math.min(deploy_for_gathering, workers_2 - 3));
            }
            com.oddlabs.tt.model.Building work = armory;
            if (danger_idle && !free.isEmpty())
            {
                refuge(free, armory);
                return;
            }
            if (evacuating.containsKey(armory))
            {
                work = null;
                foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
                {
                    if (((object)a != (object)armory) && !a.isDead() && a.isComplete() && !evacuating.containsKey(a) && !retire.isDoomed(a))
                    {
                        work = a;
                    }
                }
                com.oddlabs.tt.player.gauntlet.Economy.RaidEvac ev = (raid_out ? ((com.oddlabs.tt.player.gauntlet.Economy.RaidEvac)raid_evacs.get(armory)) : (com.oddlabs.tt.player.gauntlet.Economy.RaidEvac)(null));
                com.oddlabs.tt.model.Building refuge_v = (ev != null ? ev.refuge : (com.oddlabs.tt.model.Building)(null));
                com.oddlabs.tt.model.Building shelter = (ev != null ? ev.shelter : (com.oddlabs.tt.model.Building)(null));
                bool sheltered = shelter != null && !shelter.isDead() && !evacuating.containsKey(shelter);
                if (refuge_v != null && !refuge_v.isDead() && !evacuating.containsKey(refuge_v))
                {
                    work = refuge_v;
                    for (int i = 0; i < free.size(); i++)
                    {
                        ai.aiLog().count("raid_evac_refuged");
                    }
                }
                else if (shelter != null && sheltered && !free.isEmpty())
                {
                    order(free, shelter, com.oddlabs.tt.model.Action.DEFAULT);
                    holdEvacuees(shelter, free.size());
                    for (int i_2 = 0; i_2 < free.size(); i_2++)
                    {
                        ai.aiLog().count("raid_evac_sheltered");
                    }
                    return;
                }
            }
            if (bank_active && ((object)work == (object)armory) && !free.isEmpty())
            {
                java.util.List @in = new java.util.ArrayList();
                takeNearest(free, @in, java.lang.Math.max(0, bank_room), armory.getGridX(), armory.getGridY());
                order(@in, armory, com.oddlabs.tt.model.Action.DEFAULT);
                bank_room -= @in.size();
                if (hasUnplacedProject())
                {
                    for (int k = 0; k < 2 && !free.isEmpty(); k++)
                    {
                        free.removeFirst();
                    }
                }
                com.oddlabs.tt.model.Building q_2 = reserveQuarters();
                if (q_2 == null)
                {
                    order(free, armory, com.oddlabs.tt.model.Action.DEFAULT);
                    for (int i_3 = 0; i_3 < free.size(); i_3++)
                    {
                        ai.aiLog().count("bank_reserve_none");
                    }
                }
                else
                {
                    order(free, q_2, com.oddlabs.tt.model.Action.DEFAULT);
                    bank_reserve.merge(q_2, free.size(), new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                    for (int i_4 = 0; i_4 < free.size(); i_4++)
                    {
                        ai.aiLog().count("bank_reserve");
                        if (bank_raid)
                        {
                            ai.aiLog().count("raid_bank_reserve");
                        }
                    }
                }
                return;
            }
            if (work != null)
            {
                foreach (com.oddlabs.tt.model.Unit u_7 in java.lang.JIter.of(free))
                {
                    order(u_7, work, com.oddlabs.tt.model.Action.DEFAULT);
                }
            }
        }
        // Java: 4301
        public static bool carriesWood(com.oddlabs.tt.model.Unit u)
        {
            com.oddlabs.tt.model.UnitSupplyContainer c = u.getSupplyContainer();
            return c != null && ((object)c.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) && c.getNumSupplies() > 0;
        }
        // Java: 4316
        public void dropWood(com.oddlabs.tt.player.gauntlet.Economy.Project p, java.util.List carriers)
        {
            com.oddlabs.tt.model.Building site = p.building;
            if (site == null || p.type != com.oddlabs.tt.model.Race.BUILDING_TOWER || p.forward || p.sniper)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            if (ai.map().treesAround(p.site.x, p.site.y, 7) > st.tower_wood_trees)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int carried = 0;
            foreach (java.util.Map.Entry e in java.lang.JIter.of(intel.builder_sites.entrySet()))
            {
                if (((object)((com.oddlabs.tt.model.Building)e.getValue()) == (object)site) && com.oddlabs.tt.player.gauntlet.Economy.carriesWood(((com.oddlabs.tt.model.Unit)e.getKey())))
                {
                    carried++;
                }
            }
            int missing = (((com.oddlabs.tt.model.BuildingTemplate)site.getTemplate()).getMaxHitPoints() - site.getHitPoints() + 4) / 5 - carried;
            if (missing <= 0)
            {
                return;
            }
            java.util.List chosen = new java.util.ArrayList();
            takeNearest(carriers, chosen, missing, p.site.x, p.site.y);
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(chosen))
            {
                order(u, site, com.oddlabs.tt.model.Action.DEFAULT);
                intel.peon_states.put(u, com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD);
                intel.builder_sites.put(u, site);
                ai.aiLog().count("tower_wood_taken");
            }
            missing -= chosen.size();
            if (!chosen.isEmpty())
            {
                int n = chosen.size();
                int left = missing;
                ai.aiLog().log("WOOD", new java.util.function.Supplier_L(() =>
                {
                    return (n) + " carriers to " + java.lang.JStr.valueOf(p.describe()) + ", " + (left) + " pieces still missing";
                }));
            }
            if (missing <= 0 || !carriers.isEmpty() || p.wood_sent >= st.tower_wood_max || (float)(ai.time() - p.wood_last) < 6.0f || ai.military().threatNearEcon(p.site.x, p.site.y, 16))
            {
                return;
            }
            orderWood(p, missing);
        }
        // Java: 4358
        public void orderWood(com.oddlabs.tt.player.gauntlet.Economy.Project p, int missing)
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            com.oddlabs.tt.model.Building armory = null;
            int reach2 = st.tower_wood_reach * st.tower_wood_reach;
            int best = java.lang.Integer.MAX_VALUE;
            int k = 0;
            int wood = 0;
            int workers = 0;
            string why = "tower_wood_noarmory";
            int why_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(ai.intel().armories))
            {
                if (a.isDead() || !a.isComplete() || evacuating.containsKey(a))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), p.site.x, p.site.y);
                if (d > reach2 || d >= best)
                {
                    continue;
                }
                string no = null;
                int a_workers = a.getUnitContainer().getNumSupplies();
                int a_wood = a.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies();
                int n = java.lang.Math.min(java.lang.Math.min(missing, st.tower_wood_max - p.wood_sent), java.lang.Math.min(a_wood - st.tower_wood_reserve, a_workers - java.lang.Math.max(4, a_workers / 2)));
                if (a.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies() + a.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE).getNumSupplies() > 0)
                {
                    no = "tower_wood_pending";
                }
                else if (a_wood <= st.tower_wood_reserve)
                {
                    no = "tower_wood_short_wood";
                }
                else if (n <= 0)
                {
                    no = "tower_wood_short_workers";
                }
                if (no != null)
                {
                    if (d < why_d)
                    {
                        why_d = d;
                        why = no;
                    }
                    continue;
                }
                best = d;
                armory = a;
                k = n;
                wood = a_wood;
                workers = a_workers;
            }
            if (armory == null)
            {
                ai.aiLog().count(why);
                return;
            }
            ai.owner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON_TRANSPORT_TREE, k);
            p.wood_sent += k;
            p.wood_last = ai.time();
            ai.aiLog().count("tower_wood_drop");
            for (int i = 0; i < k; i++)
            {
                ai.aiLog().count("tower_wood_units");
            }
            int d_2 = java.lang.JC.d2i(java.lang.Math.sqrt(best));
            int m = missing;
            com.oddlabs.tt.model.Building from = armory;
            int @out = k;
            int had_wood = wood;
            int had_workers = workers;
            ai.aiLog().log("WOOD", new java.util.function.Supplier_L(() =>
            {
                return (@out) + " wood out of the armory at " + (from.getGridX()) + "," + (from.getGridY()) + " (" + (d_2) + " cells, " + (had_wood) + " wood, " + (had_workers) + " workers) for " + java.lang.JStr.valueOf(p.describe()) + ", " + (m) + " missing, " + (p.wood_sent) + " sent";
            }));
        }
        public bool danger_idle;
        public com.oddlabs.tt.model.Building refuge_armory;
        public float refuge_until;
        // Java: 4431
        public void refuge(java.util.List free, com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.player.gauntlet.Military military = ai.military();
            int tx = military.threatX();
            int ty = military.threatY();
            com.oddlabs.tt.model.Building work = null;
            int best = (-1);
            foreach (com.oddlabs.tt.model.Building a in java.lang.JIter.of(intel.armories))
            {
                if (((object)a == (object)armory) || a.isDead() || !a.isComplete() || evacuating.containsKey(a) || military.threatNear(a.getGridX(), a.getGridY(), 25) || retire.isDoomed(a))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(a.getGridX(), a.getGridY(), tx, ty);
                if (d > best)
                {
                    best = d;
                    work = a;
                }
            }
            if (work != null)
            {
                refuge_armory = work;
                refuge_until = (float)(ai.time() + 30.0f);
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(free))
                {
                    order(u, work, com.oddlabs.tt.model.Action.DEFAULT);
                    ai.aiLog().count("refuge_armory");
                }
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(free))
            {
                com.oddlabs.tt.model.Building shelter = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    if (q.isDead() || evacuating.containsKey(q) || military.threatNearEcon(q.getGridX(), q.getGridY(), 16) || retire.isDoomed(q))
                    {
                        continue;
                    }
                    int d_2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), q.getGridX(), q.getGridY());
                    if (d_2 < best_d)
                    {
                        best_d = d_2;
                        shelter = q;
                    }
                }
                if (shelter != null)
                {
                    order(u_2, shelter, com.oddlabs.tt.model.Action.DEFAULT);
                    ai.aiLog().count("refuge_quarters");
                }
            }
        }
        // Java: 4477
        public bool scoutHasWork()
        {
            foreach (com.oddlabs.tt.player.gauntlet.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.use_scout && !p.isPlaced())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4484
        public static System.Type supplyClass(System.Type c)
        {
            return ((System.Type)c);
        }
        public java.util.Map topup_sent;
        public java.util.Map quarters_done;
        public java.util.Map seed_sent;
        public java.util.Map seed_sent_time;
        // Java: 4502
        public void seedQuarters(java.util.List free, bool seed)
        {
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            float now = ai.time();
            seed_sent.keySet().removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || (float)(now - ((float)((float?)seed_sent_time.getOrDefault(u, now)))) > 30.0f;
            }));
            seed_sent_time.keySet().retainAll(seed_sent.keySet());
            int reach2 = st.seed_quarters_reach * st.seed_quarters_reach;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(ai.intel().quarters))
            {
                if (q.isDead() || !q.isComplete() || evacuating.containsKey(q))
                {
                    continue;
                }
                float done = ((float)((float?)quarters_done.computeIfAbsent(q, new java.util.function.Function_L((object b_l) =>
                {
                    com.oddlabs.tt.model.Building b = (com.oddlabs.tt.model.Building)b_l;
                    return now;
                }))));
                if (!seed || (float)(now - done) > st.seed_quarters || retire.isDoomed(q))
                {
                    continue;
                }
                int sent = 0;
                foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(seed_sent.values()))
                {
                    if (((object)b_2 == (object)q))
                    {
                        sent++;
                    }
                }
                int need = holdFor(q) - q.getUnitContainer().getNumSupplies() - sent;
                if (need <= 0)
                {
                    continue;
                }
                java.util.List near = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(free))
                {
                    if (((object)u_2 != (object)scout) && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), q.getGridX(), q.getGridY()) <= reach2)
                    {
                        near.add(u_2);
                    }
                }
                java.util.List chosen = new java.util.ArrayList();
                takeNearest(near, chosen, need, q.getGridX(), q.getGridY());
                if (chosen.isEmpty())
                {
                    continue;
                }
                free.removeAll(chosen);
                order(chosen, q, com.oddlabs.tt.model.Action.DEFAULT);
                foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(chosen))
                {
                    seed_sent.put(u_3, q);
                    seed_sent_time.put(u_3, now);
                    ai.aiLog().count("seed_quarters_sent");
                }
            }
        }
        // Java: 4540
        public int countSentTo(com.oddlabs.tt.model.Building trainer)
        {
            topup_sent.entrySet().removeIf(new java.util.function.Predicate_L((object e_l) =>
            {
                java.util.Map.Entry e = (java.util.Map.Entry)e_l;
                return ((com.oddlabs.tt.model.Unit)e.getKey()).isDead() || ((object)((com.oddlabs.tt.model.Building)e.getValue()) != (object)trainer) || ((object)((com.oddlabs.tt.player.gauntlet.Intel.PeonState)ai.intel().peon_states.get(((com.oddlabs.tt.model.Unit)e.getKey()))) != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT);
            }));
            return topup_sent.size();
        }
        // Java: 4546
        public int countHeadingTo(com.oddlabs.tt.model.Building building)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(ai.intel().peons))
            {
                if (((object)((com.oddlabs.tt.player.gauntlet.Intel.PeonState)ai.intel().peon_states.get(peon)) != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), building.getGridX(), building.getGridY()) < 900)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 4557
        public void takeNearest(java.util.List from, java.util.List into, int n, int x, int y)
        {
            for (int k = 0; k < n && !from.isEmpty(); k++)
            {
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(from))
                {
                    int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y);
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
                from.remove(best);
                into.add(best);
            }
        }
        // Java: 4576
        public void takeGatherers(java.util.List into, int n, int x, int y)
        {
            java.util.List gatherers = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(ai.intel().peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)ai.intel().peon_states.get(peon));
                if (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK))
                {
                    gatherers.add(peon);
                }
            }
            if (ai.strategy().walk_select > 0 && n > 0 && !gatherers.isEmpty())
            {
                takeWalkNearest(gatherers, into, n, x, y, ai.strategy().walk_select);
                return;
            }
            takeNearest(gatherers, into, n, x, y);
        }
        // Java: 4594
        public void takeWalkNearest(java.util.List from, java.util.List into, int n, int x, int y, int max_walk)
        {
            com.oddlabs.tt.player.gauntlet.DistanceField field = ai.map().computeField(x, y, max_walk);
            for (int k = 0; k < n && !from.isEmpty(); k++)
            {
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(from))
                {
                    int d = field.getAround(u.getGridX(), u.getGridY(), 1);
                    if (d != com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE && d < best_d)
                    {
                        best_d = d;
                        best = u;
                    }
                }
                if (best == null)
                {
                    return;
                }
                from.remove(best);
                into.add(best);
            }
        }
        // Java: 4614
        public void rebuildSupplyLoad()
        {
            supply_load.clear();
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            for (java.util.Iterator it = gather_targets.entrySet().iterator(); it.hasNext(); )
            {
                java.util.Map.Entry e = ((java.util.Map.Entry)it.next());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(u));
                if (u.isDead() || s == null || !com.oddlabs.tt.player.gauntlet.Economy.isGathering(s))
                {
                    it.remove();
                    continue;
                }
                com.oddlabs.tt.model.Supply supply = ((com.oddlabs.tt.model.Supply)e.getValue());
                if (!supply.isEmpty())
                {
                    supply_load.merge(supply, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                }
            }
        }
        // Java: 4631
        public static bool isGathering(com.oddlabs.tt.player.gauntlet.Intel.PeonState s)
        {
            return ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN);
        }
        // Java: 4636
        public bool sendGatherer(com.oddlabs.tt.model.Unit peon, System.Type type, com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.model.Supply supply = pickSupply(type, armory, peon);
            if (supply == null)
            {
                return false;
            }
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(peon), supply, com.oddlabs.tt.model.Action.DEFAULT, false);
            gather_targets.put(peon, supply);
            supply_load.merge(supply, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
            return true;
        }
        // Java: 4650
        public void unstickGatherers(com.oddlabs.tt.model.Building armory)
        {
            if (!ai.strategy().unstick)
            {
                return;
            }
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            float now = ai.time();
            gather_progress.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            bad_supplies.values().removeIf(new java.util.function.Predicate_L((object until_l) =>
            {
                float? until = (float?)until_l;
                return ((float)until) < now;
            }));
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon));
                System.Type type = (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE) ? typeof(com.oddlabs.tt.landscape.TreeSupply) : (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON) ? typeof(com.oddlabs.tt.model.IronSupply) : (((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK) ? typeof(com.oddlabs.tt.model.RockSupply) : (System.Type)(null))));
                if (type == null)
                {
                    gather_progress.remove(peon);
                    continue;
                }
                int amount = (peon.getSupplyContainer() != null ? peon.getSupplyContainer().getNumSupplies() : 0);
                float[] seen = ((float[])gather_progress.get(peon));
                if (seen == null || seen[0] != amount)
                {
                    gather_progress.put(peon, new float[] { amount, now });
                    continue;
                }
                float trip = (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? iron_cycle : (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? tree_cycle : 0.0f));
                if ((float)(now - seen[1]) < java.lang.Math.max(70.0f, (float)(ai.strategy().stuck_trip_factor * trip)))
                {
                    continue;
                }
                com.oddlabs.tt.model.Supply old = ((com.oddlabs.tt.model.Supply)gather_targets.get(peon));
                if (old != null)
                {
                    bad_supplies.put(old, (float)(now + 120.0f));
                }
                gather_progress.put(peon, new float[] { amount, now });
                unstuck++;
                sendGatherer(peon, type, armory);
            }
            if (unstuck > 0 && (float)(now - last_unstuck_log) > 60.0f)
            {
                last_unstuck_log = now;
                ai.log((unstuck) + " stuck gatherers re-sent so far");
            }
        }
        // Java: 4688
        public void unstickBuilders()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            float now = ai.time();
            builder_cells.keySet().removeIf(new java.util.function.Predicate_L((object u_l) =>
            {
                com.oddlabs.tt.model.Unit u = (com.oddlabs.tt.model.Unit)u_l;
                return u.isDead() || !intel.builder_sites.containsKey(u);
            }));
            java.util.List wedged = new java.util.ArrayList();
            foreach (java.util.Map.Entry e in java.lang.JIter.of(intel.builder_sites.entrySet()))
            {
                com.oddlabs.tt.model.Unit u_2 = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)e.getValue());
                float[] seen = ((float[])builder_cells.get(u_2));
                if (seen == null || seen[0] != u_2.getGridX() || seen[1] != u_2.getGridY())
                {
                    builder_cells.put(u_2, new float[] { u_2.getGridX(), u_2.getGridY(), now });
                    continue;
                }
                if ((float)(now - seen[2]) < ai.strategy().unstick_builders || b.isDead() || com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(u_2.getGridX(), u_2.getGridY(), b.getGridX(), b.getGridY()) <= 3 * 3)
                {
                    continue;
                }
                wedged.add(u_2);
                builder_cells.remove(u_2);
            }
            if (wedged.isEmpty())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(wedged))
            {
                com.oddlabs.tt.model.Building home = com.oddlabs.tt.player.gauntlet.MapAnalysis.nearest(intel.armories, u_3.getGridX(), u_3.getGridY());
                if (home != null)
                {
                    order(u_3, home, com.oddlabs.tt.model.Action.DEFAULT);
                }
                ai.aiLog().count("builder_unstuck");
            }
            com.oddlabs.tt.model.Unit first = ((com.oddlabs.tt.model.Unit)wedged.getFirst());
            ai.log(java.lang.JString.format("%d wedged builders around %d,%d sent into the armory", wedged.size(), first.getGridX(), first.getGridY()));
        }
        // Java: 4724
        public void retargetGatherers(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            int moved = 0;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                if (moved >= 4)
                {
                    return;
                }
                com.oddlabs.tt.player.gauntlet.Intel.PeonState s = ((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(peon));
                if (s == null || !com.oddlabs.tt.player.gauntlet.Economy.isGathering(s) || ((object)s == (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN))
                {
                    continue;
                }
                com.oddlabs.tt.model.Building works_for = ((com.oddlabs.tt.model.Building)intel.gather_buildings.get(peon));
                bool moving_over = works_for != null && ((object)works_for != (object)armory) && !works_for.isDead() && intel.armories.contains(works_for);
                if (((object)works_for != (object)armory) && works_for != null && !moving_over)
                {
                    continue;
                }
                com.oddlabs.tt.model.Supply current = ((com.oddlabs.tt.model.Supply)gather_targets.get(peon));
                if (!moving_over && current != null && !current.isEmpty())
                {
                    continue;
                }
                if (peon.getSupplyContainer() != null && peon.getSupplyContainer().getNumSupplies() > 0)
                {
                    continue;
                }
                System.Type type = ((s).ordinal() switch { 1 /*GATHER_TREE*/ => (System.Type)(typeof(com.oddlabs.tt.landscape.TreeSupply)), 3 /*GATHER_IRON*/ => (System.Type)(typeof(com.oddlabs.tt.model.IronSupply)), _ => (System.Type)(typeof(com.oddlabs.tt.model.RockSupply)) });
                if (sendGatherer(peon, type, armory))
                {
                    moved++;
                }
            }
        }
        public bool wood_starved;
        public float tree_null_until;
        // Java: 4760
        public com.oddlabs.tt.model.Supply pickSupply(System.Type type, com.oddlabs.tt.model.Building armory, com.oddlabs.tt.model.Unit peon)
        {
            if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
            {
                return pickChicken(armory);
            }
            com.oddlabs.tt.player.gauntlet.Strategy st = ai.strategy();
            bool tree = ((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply));
            int radius = (tree ? ((wood_starved ? st.wood_reach : 60)) : 200);
            com.oddlabs.tt.model.Supply best = scanSupplies(type, armory, radius);
            if (best == null && tree && !wood_starved && st.wood_reach > 0 && ai.time() >= st.wood_reach_time)
            {
                tree_null_until = (float)(ai.time() + 30.0f);
                best = scanSupplies(type, armory, st.wood_reach);
            }
            if (best != null && tree && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), best.getGridX(), best.getGridY()) > 60 * 60)
            {
                ai.aiLog().count("wood_far");
            }
            return best;
        }
        // Java: 4779
        public com.oddlabs.tt.model.Supply scanSupplies(System.Type type, com.oddlabs.tt.model.Building armory, int radius)
        {
            com.oddlabs.tt.player.gauntlet.DistanceField field = armory_field;
            java.util.List supplies = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? ai.map().getTrees() : (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? ai.map().getIron() : ai.map().getRocks()));
            int max_load = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? com.oddlabs.tt.player.gauntlet.Economy.TREE_LOAD : ai.strategy().ore_load);
            float load_penalty = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? 9.0f : ai.strategy().ore_load_penalty);
            com.oddlabs.tt.model.Supply best = null;
            float best_cost = java.lang.Float.MAX_VALUE;
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(ax, ay, s.getGridX(), s.getGridY());
                if (d2 > radius * radius)
                {
                    continue;
                }
                int d = (field != null ? field.getAround(s.getGridX(), s.getGridY(), 1) : java.lang.JC.d2i((java.lang.Math.sqrt(d2) * 2)));
                if (d == com.oddlabs.tt.player.gauntlet.DistanceField.UNREACHABLE)
                {
                    continue;
                }
                if (ai.military().threatNearEcon(s.getGridX(), s.getGridY(), 14))
                {
                    continue;
                }
                if (ai.strategy().gather_avoid_parked && seenByParked(s.getGridX(), s.getGridY()))
                {
                    continue;
                }
                float? bad = ((float?)bad_supplies.get(s));
                if (bad != null && ((float)bad) > ai.time())
                {
                    continue;
                }
                int load = ((int)((int?)supply_load.getOrDefault(s, 0)));
                float cost = (float)((float)(d + (float)(load * load_penalty)) + ((load >= max_load ? 60.0f : 0.0f)));
                if (cost < best_cost)
                {
                    best_cost = cost;
                    best = s;
                }
            }
            return best;
        }
        public java.util.List parked_cells;
        public float parked_time;
        // Java: 4821
        public bool seenByParked(int x, int y)
        {
            if ((float)(ai.time() - parked_time) >= 1.0f)
            {
                parked_time = ai.time();
                parked_cells.clear();
                foreach (com.oddlabs.tt.model.Unit e in java.lang.JIter.of(ai.intel().enemy_warriors))
                {
                    if (!e.isDead() && com.oddlabs.tt.player.gauntlet.Intel.isParked(e))
                    {
                        parked_cells.add(new int[] { e.getGridX(), e.getGridY() });
                    }
                }
            }
            foreach (int[] c in java.lang.JIter.of(parked_cells))
            {
                if (java.lang.Math.abs(c[0] - x) <= 10 && java.lang.Math.abs(c[1] - y) <= 10)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4835
        public int countChickens()
        {
            if ((float)(ai.time() - chickens_time) > 10.0f)
            {
                chickens_time = ai.time();
                chickens.clear();
                com.oddlabs.tt.pathfinder.UnitGrid grid = ai.map().getGrid();
                int size = grid.getGridSize();
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        com.oddlabs.tt.pathfinder.Occupant occ = grid.getOccupant(x, y);
                        if ((occ is com.oddlabs.tt.model.RubberSupply chicken) && !chicken.isEmpty() && !chicken.isHit() && !chickens.contains(chicken))
                        {
                            chickens.add(chicken);
                        }
                    }
                }
            }
            int n = 0;
            foreach (com.oddlabs.tt.model.RubberSupply c in java.lang.JIter.of(chickens))
            {
                if (!c.isEmpty())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 4857
        public com.oddlabs.tt.model.Supply pickChicken(com.oddlabs.tt.model.Building armory)
        {
            countChickens();
            com.oddlabs.tt.model.RubberSupply best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            int left = 0;
            int free = 0;
            foreach (com.oddlabs.tt.model.RubberSupply c in java.lang.JIter.of(chickens))
            {
                if (c.isEmpty() || c.isHit())
                {
                    continue;
                }
                left++;
                if (((int)((int?)supply_load.getOrDefault(c, 0))) > 0)
                {
                    continue;
                }
                free++;
                if (ai.military().enemyStrengthNear(c.getGridX(), c.getGridY(), 20) > 0)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), c.getGridX(), c.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = c;
                }
            }
            if (best == null)
            {
                ai.aiLog().count((left == 0 ? "chicken_null_left" : (free == 0 ? "chicken_null_taken" : "chicken_null_enemy20")));
            }
            if (best != null && best_d > 150 * 150)
            {
                ai.aiLog().count("chicken_null_range");
                return null;
            }
            return best;
        }
        // Java: 4892
        public void manageRepairs()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            if (intel.armory() == null)
            {
                return;
            }
            java.util.List damaged = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(intel.armories))
            {
                if (b.isDamaged())
                {
                    damaged.add(b);
                }
            }
            foreach (com.oddlabs.tt.model.Building b_2 in java.lang.JIter.of(intel.towers))
            {
                if (b_2.isDamaged())
                {
                    damaged.add(b_2);
                }
            }
            foreach (com.oddlabs.tt.model.Building b_3 in java.lang.JIter.of(intel.quarters))
            {
                if (b_3.isDamaged())
                {
                    damaged.add(b_3);
                }
            }
            foreach (com.oddlabs.tt.model.Building b_4 in java.lang.JIter.of(damaged))
            {
                if (ai.military().threatNearEcon(b_4.getGridX(), b_4.getGridY(), 12) || retire.isDoomed(b_4))
                {
                    continue;
                }
                int missing = ((com.oddlabs.tt.model.BuildingTemplate)b_4.getTemplate()).getMaxHitPoints() - b_4.getHitPoints();
                int want = java.lang.Math.min(4, 1 + missing / 40);
                int have = builderCount(b_4);
                if (have >= want)
                {
                    continue;
                }
                java.util.List chosen = new java.util.ArrayList();
                takeGatherers(chosen, want - have, b_4.getGridX(), b_4.getGridY());
                order(chosen, b_4, com.oddlabs.tt.model.Action.GATHER_REPAIR);
            }
        }
        // Java: 4923
        public string debugStatus()
        {
            com.oddlabs.tt.player.gauntlet.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            sb.append("Q").append(intel.quarters.size()).append('+').append(intel.quarters_sites.size());
            sb.append(" A").append(intel.armories.size()).append('+').append(intel.armory_sites.size());
            sb.append(" T").append(intel.towers.size()).append('+').append(intel.tower_sites.size());
            sb.append(" proj=").append(projects.size());
            sb.append(java.lang.JString.format(" Is=%.0f Rs=%.0f%s", iron_s, rock_s, (rock_stream_on ? (object)(" RS") : (object)(""))));
            int held = 0;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                if (!q.isDead())
                {
                    held += q.getUnitContainer().getNumSupplies();
                }
            }
            sb.append(" held=").append(held);
            sb.append(" peons=").append(intel.peons.size());
            sb.append(" (idle ").append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.IDLE));
            sb.append(" bld ").append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.BUILD));
            sb.append(" tr ").append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT));
            sb.append(" g ").append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_TREE)).append('/').append(want_tree);
            sb.append(',').append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_IRON)).append('/').append(want_iron);
            sb.append(',').append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_ROCK)).append('/').append(want_rock);
            sb.append(',').append(intel.countPeons(com.oddlabs.tt.player.gauntlet.Intel.PeonState.GATHER_CHICKEN)).append('/').append(want_chicken);
            sb.append(')');
            if (armory != null && !armory.isDead())
            {
                sb.append(" W=").append(armory.getUnitContainer().getNumSupplies()).append('/').append(want_workers);
                sb.append(" res=").append(armory.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies());
                sb.append(',').append(armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies());
                sb.append(',').append(armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies());
                sb.append(',').append(armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies());
                sb.append(" wpn=").append(armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies());
                sb.append(',').append(armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies());
                sb.append(java.lang.JString.format(" cyc=%.0f/%.0f", tree_cycle, iron_cycle));
            }
            int near = 0;
            int far = 0;
            int transfer = 0;
            foreach (com.oddlabs.tt.model.Unit p in java.lang.JIter.of(intel.peons))
            {
                if (p.isDead() || ((object)((com.oddlabs.tt.player.gauntlet.Intel.PeonState)intel.peon_states.get(p)) != (object)com.oddlabs.tt.player.gauntlet.Intel.PeonState.TRANSIT))
                {
                    continue;
                }
                if ((p.getPrimaryController() is com.oddlabs.tt.model.behaviour.TransferUnitController))
                {
                    transfer++;
                }
                else if (armory != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), p.getGridX(), p.getGridY()) <= 8 * 8)
                {
                    near++;
                }
                else if (armory != null && com.oddlabs.tt.player.gauntlet.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), p.getGridX(), p.getGridY()) > 60 * 60)
                {
                    far++;
                }
            }
            sb.append(" trx=").append(near).append('/').append(far).append('/').append(transfer);
            return java.lang.JO.toString(sb);
        }
        static Economy()
        {
            Economy.SYNC_NAMES = new string[] { "rock", "iron", "rubber" };
            Economy.SYNC_WEAPONS = new System.Type[] { typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon) };
            Economy.SYNC_WORK = new float[] { 40.0f, 80.0f, 120.0f };
            Economy.SYNC_SUPPLIES = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RubberSupply) };
            Economy.SYNC_SUPPLY_NAMES = new string[] { "tree", "rock", "iron", "rubber" };
            Economy.SYNC_COST = new int[][] { new int[] { 2, 1, 0, 0 }, new int[] { 2, 0, 1, 0 }, new int[] { 2, 1, 1, 1 } };
        }
        private void init_Economy()
        {
            this.projects = new java.util.ArrayList();
            this.gather_targets = new java.util.LinkedHashMap();
            this.supply_load = new java.util.LinkedHashMap();
            this.chickens = new java.util.ArrayList();
            this.chickens_time = (float)(-100.0f);
            this.tree_cycle = 14.0f;
            this.iron_cycle = 22.0f;
            this.last_expansion_check = (float)(-100.0f);
            this.last_old_recall = (float)(-100.0f);
            this.forward_towers = new java.util.ArrayList();
            this.sniper_towers = new java.util.ArrayList();
            this.evacuating = new java.util.LinkedHashMap();
            this.last_sniper = (float)(-100.0f);
            this.gather_progress = new java.util.LinkedHashMap();
            this.bad_supplies = new java.util.LinkedHashMap();
            this.builder_cells = new java.util.LinkedHashMap();
            this.raids = new java.util.ArrayList();
            this.raid_evacs = new java.util.LinkedHashMap();
            this.raid_hold = new java.util.LinkedHashMap();
            this.tower_hold_until = (float)(-1.0f);
            this.quarters_hold_until = (float)(-1.0f);
            this.standing = new java.util.LinkedHashSet();
            this.razings = new java.util.ArrayList();
            this.razings_long = new java.util.ArrayList();
            this.reserve_until = (float)(-1.0f);
            this.rearm_deploy_next = (float)(-1.0f);
            this.reserve_known = new java.util.ArrayList();
            this.rearm_next = (float)(-1.0f);
            this.rearm_search = new com.oddlabs.tt.player.gauntlet.SitePlanner.Search();
            this.reloc_search = new com.oddlabs.tt.player.gauntlet.SitePlanner.Search();
            this.lock_search = new com.oddlabs.tt.player.gauntlet.SitePlanner.Search();
            this.rearm_alt_next = (float)(-1.0f);
            this.reloc_besieged_since = (float)(-1.0f);
            this.reloc_switch_time = (float)(-100.0f);
            this.armory_costs = new java.util.LinkedHashMap();
            this.meas_start = (float)(-1.0f);
            this.iron_s = 0.0f;
            this.rock_s = 0.0f;
            this.reloc_next = (float)(-1.0f);
            this.last_reloc_end = (float)(-1000.0f);
            this.was_drained = new java.util.LinkedHashMap();
            this.lock_since = (float)(-1.0f);
            this.lock_next = (float)(-1.0f);
            this.bank_cap = java.lang.Integer.MAX_VALUE;
            this.noforge_since = (float)(-1.0f);
            this.last_bank_unload = (float)(-100.0f);
            this.bank_reserve = new java.util.LinkedHashMap();
            this.sync_paused = new bool[3];
            this.sync_tick = (-100);
            this.sync_progress = new float[3];
            this.sync_ms = new float[3];
            this.sync_weapons = new int[3];
            this.sync_stock = new int[4];
            this.sync_expect = new float[3];
            this.sync_carried = new java.util.LinkedHashMap();
            this.sync_failed = (-1);
            this.refuge_until = (float)(-1.0f);
            this.topup_sent = new java.util.LinkedHashMap();
            this.quarters_done = new java.util.LinkedHashMap();
            this.seed_sent = new java.util.LinkedHashMap();
            this.seed_sent_time = new java.util.LinkedHashMap();
            this.tree_null_until = (float)(-1.0f);
            this.parked_cells = new java.util.ArrayList();
            this.parked_time = (float)(-100.0f);
        }
        public Economy(com.oddlabs.tt.player.gauntlet.GauntletAI ai) : base()
        {
            init_Economy();
            this.ai = ai;
            retire = new com.oddlabs.tt.player.gauntlet.Retire(ai, this);
            openingPlan();
        }
    }
}

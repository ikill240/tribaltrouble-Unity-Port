// Translated from Economy.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.ai
{
    // Java: 38
    public class Economy : java.lang.Object
    {
        public const float IRON_WORK = 80.0f;
        public const int MAX_BUILDERS = 20;
        public const int TREE_LOAD = 2;
        public const int ORE_LOAD = 3;
        public com.oddlabs.tt.player.ai.ExpertAI ai;
        public java.util.List projects;
        public java.util.Map gather_targets;
        public java.util.Map supply_load;
        public java.util.List chickens;
        public com.oddlabs.tt.model.Unit scout;
        public com.oddlabs.tt.player.ai.SitePlanner.Site armory_site;
        public com.oddlabs.tt.player.ai.DistanceField armory_field;
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
        public bool rock_weapons;
        public bool rock_filler;
        public com.oddlabs.tt.player.ai.Economy.Project expansion_project;
        public com.oddlabs.tt.model.Building expansion;
        public float last_expansion_check;
        public float last_old_recall;
        public bool chieftain_topup;
        public int project_counter;
        public java.util.List forward_towers;
        public java.util.Map gather_progress;
        public java.util.Map bad_supplies;
        public int unstuck;
        public float last_unstuck_log;
        public bool rush_alert;
        public float rush_alert_time;
        public bool had_armory;
        // Java: 93
        public void openingPlan()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.player.ai.SitePlanner planner = ai.planner();
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            java.util.List reserved = new java.util.ArrayList();
            armory_site = planner.findArmorySite(reserved);
            int sx = planner.getStartX();
            int sy = planner.getStartY();
            int ax = (armory_site != null ? armory_site.x : sx);
            int ay = (armory_site != null ? armory_site.y : sy);
            if (armory_site != null)
            {
                reserved.add(armory_site.withHalf(com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.ARMORY));
            }
            int first_builders = java.lang.Math.max(1, intel.peons.size() - strategy.scouts);
            com.oddlabs.tt.player.ai.SitePlanner.Site q1 = planner.findQuartersSite(reserved, sx, sy, 110, planner.getStartField(), ax, ay, first_builders, 0.2f, 0.06f);
            if (q1 == null || (float)(-q1.score) > 110.0f)
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site further = planner.findQuartersSite(reserved, sx, sy, 400, planner.getStartField(), ax, ay, first_builders, 0.2f, 0.06f);
                if (further != null && (q1 == null || further.score > q1.score))
                {
                    q1 = further;
                }
            }
            if (q1 == null)
            {
                q1 = planner.findQuartersSite(java.util.List_.of(), sx, sy, 400, planner.getStartField(), ax, ay, first_builders, 0.2f, 0.0f);
                if (q1 != null && armory_site != null && com.oddlabs.tt.player.ai.SitePlanner.conflicts(java.util.List_.of(armory_site), q1.x, q1.y, com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.QUARTERS))
                {
                    reserved.remove(armory_site);
                    armory_site = null;
                }
            }
            if (q1 != null)
            {
                reserved.add(q1);
                com.oddlabs.tt.player.ai.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, q1, 0);
                p.first = true;
            }
            int armory_priority = 1 + 2 * java.lang.Math.max(0, strategy.quarters_before_armory - 1);
            if (armory_site != null)
            {
                com.oddlabs.tt.player.ai.Economy.Project p_2 = addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, armory_site, armory_priority);
                p_2.use_scout = true;
            }
            com.oddlabs.tt.player.ai.DistanceField a_field = (armory_site != null ? ai.map().computeField(ax, ay, 240) : (com.oddlabs.tt.player.ai.DistanceField)(null));
            for (int i = 1; i < strategy.initial_quarters; i++)
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site q = default;
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
                com.oddlabs.tt.player.ai.Economy.Project p_3 = addProject(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, q, 2 * i);
                p_3.use_scout = true;
            }
            com.oddlabs.tt.model.Unit best_scout = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), ax, ay);
                if (d < best_d)
                {
                    best_d = d;
                    best_scout = peon;
                }
            }
            scout = (strategy.scouts > 0 ? best_scout : (com.oddlabs.tt.model.Unit)(null));
            com.oddlabs.tt.player.ai.Economy.Project first = (projects.isEmpty() ? (com.oddlabs.tt.player.ai.Economy.Project)(null) : ((com.oddlabs.tt.player.ai.Economy.Project)projects.getFirst()));
            if (first != null && first.first)
            {
                java.util.List builders = new java.util.ArrayList();
                foreach (com.oddlabs.tt.model.Unit peon_2 in java.lang.JIter.of(intel.peons))
                {
                    if (((object)peon_2 != (object)scout))
                    {
                        builders.add(peon_2);
                    }
                }
                if (!builders.isEmpty())
                {
                    createBuilding(first);
                    order(builders, first.building, com.oddlabs.tt.model.Action.DEFAULT);
                }
            }
        }
        // Java: 184
        public void tick()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            choosePrimaryArmory();
            refreshArmoryField();
            manageProjects();
            escortForward();
            manageQuarters();
            manageArmory();
            allocatePeons();
            manageRepairs();
        }
        // Java: 196
        public void plan()
        {
            checkRush();
            planBuildings();
            computeGatherTargets();
        }
        // Java: 205
        public class Project : java.lang.Object
        {
            public int type;
            public int id;
            public com.oddlabs.tt.player.ai.SitePlanner.Site site;
            public com.oddlabs.tt.model.Building building;
            public int priority;
            public bool first;
            public bool use_scout;
            public bool forward;
            public int failures;
            public float placed_time;
            // Java: 227
            public bool isPlaced()
            {
                return building != null && !building.isDead() && building.isPlaced();
            }
            // Java: 231
            public string describe()
            {
                string name = ((type) switch { 0 => (string)("quarters"), 1 => (string)("armory"), 6 => (string)("race hall"), _ => (string)((forward ? "forward tower" : "tower")) });
                return java.lang.JStr.valueOf(name) + "#" + (id) + " at " + (site.x) + "," + (site.y);
            }
            private void init_Project()
            {
                this.placed_time = (float)(-1.0f);
            }
            public Project(int type, int id, com.oddlabs.tt.player.ai.SitePlanner.Site site, int priority) : base()
            {
                init_Project();
                this.type = type;
                this.id = id;
                this.site = site;
                this.priority = priority;
            }
        }
        // Java: 243
        public com.oddlabs.tt.player.ai.Economy.Project addProject(int type, com.oddlabs.tt.player.ai.SitePlanner.Site site, int priority)
        {
            site.withHalf(com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.of(type));
            com.oddlabs.tt.player.ai.Economy.Project p = new com.oddlabs.tt.player.ai.Economy.Project(type, project_counter++, site, priority);
            ai.log("plan " + java.lang.JStr.valueOf(p.describe()) + " trees=" + (ai.map().treesAround(site.x, site.y, 7)) + " from start=" + (ai.planner().getStartField().get(site.x, site.y)) + "m");
            int i = 0;
            while (i < projects.size() && ((com.oddlabs.tt.player.ai.Economy.Project)projects.get(i)).priority <= priority)
            {
                i++;
            }
            projects.add(i, p);
            return p;
        }
        // Java: 255
        public void createBuilding(com.oddlabs.tt.player.ai.Economy.Project p)
        {
            com.oddlabs.tt.model.BuildingTemplate template = ai.owner().getRace().getBuildingTemplate(p.type);
            p.building = template.create(ai.owner(), p.site.x, p.site.y);
        }
        // Java: 260
        public void order(java.util.List units, com.oddlabs.tt.model.Building target, com.oddlabs.tt.model.Action action)
        {
            if (target == null || units.isEmpty())
            {
                return;
            }
            ai.owner().setTarget(((com.oddlabs.tt.model.Selectable[])units.toArray(new com.oddlabs.tt.model.Selectable[0])), target, action, false);
        }
        // Java: 266
        public void order(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Building target, com.oddlabs.tt.model.Action action)
        {
            ai.owner().setTarget(com.oddlabs.tt.model.Selectable.newArray(unit), target, action, false);
        }
        // Java: 270
        public bool hasBuilding(int template_id)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ai.owner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == template_id)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 277
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
        // Java: 285
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
        // Java: 292
        public void manageProjects()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            for (java.util.Iterator it = projects.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.ai.Economy.Project p = ((com.oddlabs.tt.player.ai.Economy.Project)it.next());
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
                    }
                    if (p.building.isComplete())
                    {
                        it.remove();
                        ai.log("completed " + java.lang.JStr.valueOf(p.describe()) + " after " + (java.lang.JC.f2i(((float)(ai.time() - p.placed_time)))) + "s");
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
                    p.failures++;
                    p.building = null;
                    if (p.failures > 6)
                    {
                        it.remove();
                        continue;
                    }
                    com.oddlabs.tt.player.ai.SitePlanner.Site moved = resite(p);
                    if (moved == null)
                    {
                        it.remove();
                        continue;
                    }
                    p.site = moved;
                }
                if (!projectMayStart(p))
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit placer = choosePlacer(p);
                if (placer == null)
                {
                    continue;
                }
                createBuilding(p);
                order(placer, p.building, com.oddlabs.tt.model.Action.DEFAULT);
                intel.builder_sites.put(placer, p.building);
                intel.peon_states.put(placer, com.oddlabs.tt.player.ai.Intel.PeonState.BUILD);
            }
        }
        // Java: 341
        public bool projectMayStart(com.oddlabs.tt.player.ai.Economy.Project p)
        {
            if (p.use_scout)
            {
                return true;
            }
            if (ai.military().threatNear(p.site.x, p.site.y, 16))
            {
                return false;
            }
            if (p.forward)
            {
                return ai.military().escortArrived(p.site.x, p.site.y);
            }
            int placed_incomplete = 0;
            foreach (com.oddlabs.tt.player.ai.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (((object)q != (object)p) && q.isPlaced() && q.type != com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    placed_incomplete++;
                }
            }
            return p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY || placed_incomplete < 2;
        }
        // Java: 358
        public void onCompleted(com.oddlabs.tt.player.ai.Economy.Project p)
        {
            if (((object)p == (object)expansion_project))
            {
                expansion = p.building;
            }
            if (p.forward && p.building != null)
            {
                forward_towers.add(p.building);
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
        // Java: 372
        public com.oddlabs.tt.player.ai.SitePlanner.Site resite(com.oddlabs.tt.player.ai.Economy.Project p)
        {
            com.oddlabs.tt.player.ai.SitePlanner planner = ai.planner();
            java.util.List reserved = reservedSites(p);
            {
                switch ((p.type))
                {
                    case 1:
                    {
                        com.oddlabs.tt.player.ai.SitePlanner.Site s = planner.findQuartersSiteLike(reserved, p.site.x, p.site.y, 12, com.oddlabs.tt.model.Race.BUILDING_ARMORY);
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
        // Java: 386
        public java.util.List reservedSites(com.oddlabs.tt.player.ai.Economy.Project except)
        {
            java.util.List reserved = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.ai.Economy.Project q in java.lang.JIter.of(projects))
            {
                if (((object)q != (object)except))
                {
                    reserved.add(q.site);
                }
            }
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.player.ai.Economy.addBuildings(reserved, intel.quarters);
            com.oddlabs.tt.player.ai.Economy.addBuildings(reserved, intel.armories);
            com.oddlabs.tt.player.ai.Economy.addBuildings(reserved, intel.towers);
            com.oddlabs.tt.player.ai.Economy.addBuildings(reserved, intel.quarters_sites);
            com.oddlabs.tt.player.ai.Economy.addBuildings(reserved, intel.armory_sites);
            com.oddlabs.tt.player.ai.Economy.addBuildings(reserved, intel.tower_sites);
            return reserved;
        }
        // Java: 401
        public static void addBuildings(java.util.List reserved, java.util.List buildings)
        {
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                reserved.add(new com.oddlabs.tt.player.ai.SitePlanner.Site(b.getGridX(), b.getGridY(), 0).withHalf(com.oddlabs.tt.player.ai.SitePlanner.RaceSizes.of(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID())));
            }
        }
        // Java: 408
        public com.oddlabs.tt.model.Unit choosePlacer(com.oddlabs.tt.player.ai.Economy.Project p)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            if (p.use_scout && scout != null && !scout.isDead())
            {
                foreach (com.oddlabs.tt.player.ai.Economy.Project q in java.lang.JIter.of(projects))
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
            com.oddlabs.tt.model.Unit best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(peon));
                if (((object)s != (object)com.oddlabs.tt.player.ai.Intel.PeonState.IDLE) && ((object)s != (object)com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT) && ((object)s != (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE) && ((object)s != (object)com.oddlabs.tt.player.ai.Intel.PeonState.MOVE))
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), p.site.x, p.site.y);
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE))
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
                    if (((object)((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(peon_2)) == (object)com.oddlabs.tt.player.ai.Intel.PeonState.BUILD))
                    {
                        best = peon_2;
                        break;
                    }
                }
            }
            return best;
        }
        // Java: 446
        public int buildersWanted(com.oddlabs.tt.player.ai.Economy.Project p)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            bool have_quarters = !intel.quarters.isEmpty();
            bool have_armory = !intel.armories.isEmpty();
            if (p.first)
            {
                return com.oddlabs.tt.player.ai.Economy.MAX_BUILDERS;
            }
            if (!have_quarters && hasFirstQuarters())
            {
                return 0;
            }
            int trees = ai.map().treesAround(p.site.x, p.site.y, 12);
            int feedable = java.lang.Math.max(6, 4 * trees);
            int cap = ((p.type) switch { 1 => (int)((have_armory ? 12 : com.oddlabs.tt.player.ai.Economy.MAX_BUILDERS)), 0 => (int)((armsRace() ? 3 : (have_armory ? strategy.quarters_builders : com.oddlabs.tt.player.ai.Economy.MAX_BUILDERS))), _ => (int)(strategy.tower_builders) });
            return java.lang.Math.min(cap, feedable);
        }
        // Java: 466
        public bool hasFirstQuarters()
        {
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.first)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 476
        public void planBuildings()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            float time = ai.time();
            int armory_count = intel.armories.size() + intel.armory_sites.size() + countProjects(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false);
            if (armory_count == 0)
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site site = (had_armory ? safeArmorySite() : ai.planner().findArmorySite(reservedSites(null)));
                if (site != null && ai.military().threatNear(site.x, site.y, 25))
                {
                    return;
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
            else if (armory_count == 1 && intel.armories.size() == 1)
            {
                considerExpansion();
            }
            com.oddlabs.tt.model.Building armory = intel.armory();
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
            if (quarters_count < target_quarters && countProjects(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, true) == 0 && (armory != null || intel.quarters.isEmpty()))
            {
                int ax = (armory != null ? armory.getGridX() : ai.planner().getStartX());
                int ay = (armory != null ? armory.getGridY() : ai.planner().getStartY());
                com.oddlabs.tt.player.ai.DistanceField field = (armory != null ? armoryField() : ai.planner().getStartField());
                com.oddlabs.tt.player.ai.SitePlanner.Site site_2 = ai.planner().findQuartersSite(reservedSites(null), ax, ay, 90, field, ai.planner().getStartX(), ai.planner().getStartY(), ai.strategy().quarters_builders, 0.25f, 0.02f);
                if (site_2 != null)
                {
                    addProject(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, site_2, 5);
                }
            }
            if (ai.owner().getWorld().isExtended() && armory != null && time >= java.lang.Math.max(strategy.expand_time, 480.0f) && intel.quarters.size() >= 2 && countProjects(com.oddlabs.tt.model.Race.BUILDING_HALL, true) == 0 && !hasBuilding(com.oddlabs.tt.model.Race.BUILDING_HALL) && ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_HALL))
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site site_3 = ai.planner().findQuartersSiteLike(reservedSites(null), armory.getGridX(), armory.getGridY(), 16, com.oddlabs.tt.model.Race.BUILDING_HALL);
                if (site_3 != null)
                {
                    addProject(com.oddlabs.tt.model.Race.BUILDING_HALL, site_3, 6);
                }
            }
            if (armory != null && intel.quarters.size() >= 2)
            {
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
                    target_towers += enemies - 1;
                }
                forward_towers.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Building)a0).isDead(); }));
                int tower_count = intel.towers.size() + intel.tower_sites.size() + countProjects(com.oddlabs.tt.model.Race.BUILDING_TOWER, false) - forward_towers.size() - countForward() - ai.military().creepTowerCount();
                if (tower_count < target_towers && countProjects(com.oddlabs.tt.model.Race.BUILDING_TOWER, true) == 0 && ai.owner().canBuild(com.oddlabs.tt.model.Race.BUILDING_TOWER))
                {
                    java.util.List existing = new java.util.ArrayList();
                    foreach (com.oddlabs.tt.model.Building t in java.lang.JIter.of(intel.towers))
                    {
                        existing.add(new int[] { t.getGridX(), t.getGridY() });
                    }
                    foreach (com.oddlabs.tt.model.Building t_2 in java.lang.JIter.of(intel.tower_sites))
                    {
                        existing.add(new int[] { t_2.getGridX(), t_2.getGridY() });
                    }
                    int[] center = towerAnchor(tower_count);
                    int[] face = new int[] { ai.planner().getEnemyX(), ai.planner().getEnemyY() };
                    if (fronts && tower_count % 2 == 1)
                    {
                        int[][] front = enemyFront(tower_count / 2);
                        if (front != null)
                        {
                            center = front[0];
                            face = front[1];
                        }
                    }
                    com.oddlabs.tt.player.ai.SitePlanner.Site site_4 = ai.planner().findTowerSite(reservedSites(null), center[0], center[1], 7, 15, existing, face[0], face[1]);
                    if (site_4 != null)
                    {
                        addProject(com.oddlabs.tt.model.Race.BUILDING_TOWER, site_4, 8);
                    }
                }
                planForwardTower();
            }
        }
        // Java: 571
        public void escortForward()
        {
            com.oddlabs.tt.player.ai.Military military = ai.military();
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.forward && military.canEscort())
                {
                    military.escort(p.site.x, p.site.y);
                    return;
                }
            }
        }
        // Java: 582
        public int countForward()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.forward)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 591
        public void planForwardTower()
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.ai.Military military = ai.military();
            for (java.util.Iterator it = projects.iterator(); it.hasNext(); )
            {
                com.oddlabs.tt.player.ai.Economy.Project p = ((com.oddlabs.tt.player.ai.Economy.Project)it.next());
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
            com.oddlabs.tt.player.ai.SitePlanner.Site site = ai.planner().findTowerSite(reservedSites(null), spot[0], spot[1], 3, 6, existing, ai.planner().getStartX(), ai.planner().getStartY());
            if (site == null)
            {
                return;
            }
            com.oddlabs.tt.player.ai.Economy.Project p_2 = addProject(com.oddlabs.tt.model.Race.BUILDING_TOWER, site, 3);
            p_2.forward = true;
        }
        // Java: 620
        public com.oddlabs.tt.player.ai.SitePlanner.Site safeArmorySite()
        {
            com.oddlabs.tt.player.ai.Military military = ai.military();
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
        // Java: 641
        public void checkRush()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            if (rush_alert || !ai.strategy().rush_response || !intel.armories.isEmpty())
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
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY)
                {
                    p.priority = 1;
                }
            }
            projects.sort(java.util.Comparator_.comparingInt(new java.util.function.ToIntFunction_L((object p_2_l) =>
            {
                com.oddlabs.tt.player.ai.Economy.Project p_2 = (com.oddlabs.tt.player.ai.Economy.Project)p_2_l;
                return p_2.priority;
            })));
        }
        // Java: 670
        public bool armsRace()
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ai.intel().warriors))
            {
                ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(w));
            }
            if (!rush_alert || ai.time() > (float)(rush_alert_time + strategy.rush_seconds))
            {
                return false;
            }
            float theirs = 0.0f;
            foreach (com.oddlabs.tt.model.Unit w_2 in java.lang.JIter.of(ai.intel().enemy_warriors))
            {
                theirs = (float)(theirs + com.oddlabs.tt.player.ai.Combat.@value(w_2));
            }
            return ours < (float)((float)(1.2f * theirs) + 4.0f);
        }
        // Java: 684
        public bool underPressure()
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.ai.Military military = ai.military();
            if (!strategy.pressure_response || ai.time() >= strategy.pressure_time || military.baseThreatLevel() == 0)
            {
                return false;
            }
            float ours = 0.0f;
            foreach (com.oddlabs.tt.model.Unit w in java.lang.JIter.of(ai.intel().warriors))
            {
                ours = (float)(ours + com.oddlabs.tt.player.ai.Combat.@value(w));
            }
            return ours < (float)((float)(1.2f * military.threatStrength()) + 4.0f);
        }
        // Java: 696
        public int[] towerAnchor(int tower_count)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            if (tower_count % 3 == 2 && !intel.quarters.isEmpty())
            {
                com.oddlabs.tt.model.Building exposed = null;
                float best = (float)(-1.0f);
                foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
                {
                    float e = ai.planner().exposure(q.getGridX(), q.getGridY());
                    if (e > best && com.oddlabs.tt.player.ai.MapAnalysis.dist2(q.getGridX(), q.getGridY(), armory.getGridX(), armory.getGridY()) > 20 * 20)
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
            return new int[] { armory.getGridX(), armory.getGridY() };
        }
        // Java: 721
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
            int ex = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartX());
            int ey = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(enemy.getStartY());
            java.util.List own = new java.util.ArrayList(ai.intel().quarters);
            own.addAll(ai.intel().armories);
            com.oddlabs.tt.model.Building nearest_v = null;
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(own))
            {
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(ex, ey, b.getGridX(), b.getGridY());
                if (d < best)
                {
                    best = d;
                    nearest_v = b;
                }
            }
            if (nearest_v == null)
            {
                return null;
            }
            return new int[][] { new int[] { nearest_v.getGridX(), nearest_v.getGridY() }, new int[] { ex, ey } };
        }
        // Java: 748
        public int countProjects(int type, bool unplaced_only)
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
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
        // Java: 766
        public int holdFor(com.oddlabs.tt.model.Building quarters)
        {
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            com.oddlabs.tt.player.Player owner = ai.owner();
            int pop = owner.getUnitCountContainer().getNumSupplies();
            int max = owner.getWorld().getMaxUnitCount();
            if (pop >= max - 2)
            {
                return 0;
            }
            if (quarters.getChieftainContainer() != null && quarters.getChieftainContainer().isTraining())
            {
                return strategy.hold_chieftain;
            }
            if (ai.intel().armories.isEmpty() && !ai.intel().quarters.isEmpty() && needsBuilders())
            {
                return java.lang.Math.min(2, strategy.hold_early);
            }
            if (pop > max * 7 / 10)
            {
                return strategy.hold_late;
            }
            int hold = (ai.time() < strategy.hold_mid_time ? strategy.hold_early : strategy.hold_mid);
            return (armsRace() ? java.lang.Math.min(2, hold) : hold);
        }
        // Java: 783
        public bool needsBuilders()
        {
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.isPlaced() && builderCount(p.building) < buildersWanted(p))
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 791
        public bool gatherUnderThreat()
        {
            return ai.strategy().gather_under_threat && (ai.enemiesAlive() > 1 || armsRace() || underPressure() || ai.strategy().gather_threat_1v1);
        }
        // Java: 796
        public void manageQuarters()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            bool threatened = ai.military().baseThreatLevel() > 1;
            foreach (com.oddlabs.tt.model.Building q in java.lang.JIter.of(intel.quarters))
            {
                int inside = q.getUnitContainer().getNumSupplies();
                int hold = holdFor(q);
                if (threatened && ai.military().threatNear(q.getGridX(), q.getGridY(), (gatherUnderThreat() ? 12 : 20)))
                {
                    continue;
                }
                if (inside > hold)
                {
                    ai.owner().deployUnits(q, com.oddlabs.tt.model.DeployType.PEON, inside - hold);
                }
            }
        }
        // Java: 813
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
        // Java: 828
        public void choosePrimaryArmory()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building primary = null;
            if (expansion != null && !expansion.isDead() && expansion.isComplete())
            {
                primary = expansion;
            }
            intel.setPrimaryArmory(primary);
        }
        // Java: 841
        public void considerExpansion()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            if (armory == null || armory_field == null || ai.time() < 300.0f || (float)(ai.time() - last_expansion_check) < 30.0f || !ai.strategy().expansion)
            {
                return;
            }
            if (ai.military().baseThreatLevel() > 0 || countProjects(com.oddlabs.tt.model.Race.BUILDING_ARMORY, false) > 0)
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
            com.oddlabs.tt.player.ai.SitePlanner.Site site = ai.planner().findExpansionSite(reservedSites(null), armory_field);
            float better = java.lang.Float.MAX_VALUE;
            if (site != null)
            {
                better = ai.planner().warriorGatherCost(ai.map().computeField(site.x, site.y, 220));
            }
            if (ai.strategy().far_expansion && better > (float)(0.75f * current))
            {
                com.oddlabs.tt.player.ai.SitePlanner.Site far = ai.planner().findFarExpansionSite(reservedSites(null));
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
            ai.log(java.lang.JString.format("expansion check: current armory %.0f (iron %.0fs), best site %d,%d %.0f", current, iron_cycle, site.x, site.y, better));
            float gain = (iron_cycle >= ai.strategy().desperate_iron_cycle ? ai.strategy().desperate_expansion : 0.75f);
            if (better > (float)(gain * current))
            {
                return;
            }
            com.oddlabs.tt.player.ai.Economy.Project p = addProject(com.oddlabs.tt.model.Race.BUILDING_ARMORY, site, 1);
            expansion_project = p;
        }
        // Java: 878
        public com.oddlabs.tt.player.ai.DistanceField armoryField()
        {
            return (ai.intel().armory() != null ? armory_field : (com.oddlabs.tt.player.ai.DistanceField)(null));
        }
        // Java: 883
        public void manageArmory()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
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
        // Java: 901
        public void drainSecondary(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            if (ai.strategy().recall_old_gatherers && (float)(ai.time() - last_old_recall) >= 10.0f)
            {
                last_old_recall = ai.time();
                com.oddlabs.tt.player.ai.Intel.PeonState[] states = new com.oddlabs.tt.player.ai.Intel.PeonState[] { com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE, com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON, com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK, com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN };
                System.Type[] types = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.RubberSupply) };
                int recalled = 0;
                for (int t = 0; t < states.Length; t++)
                {
                    int n = ai.intel().countLinkedGatherers(states[t], armory);
                    if (n > 0)
                    {
                        owner.recallGatherers(armory, com.oddlabs.tt.player.ai.Economy.supplyClass(types[t]), n);
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
            int c = java.lang.Math.min(chicken, workers);
            if (c > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, c);
            }
            int i = java.lang.Math.min(iron, workers - c);
            if (i > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, i);
            }
            int r = java.lang.Math.min(rock, workers - c - i);
            if (r > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, r);
            }
            int left = workers - c - i - r;
            bool can_make = armory.getSupplyContainer(typeof(com.oddlabs.tt.landscape.TreeSupply)).getNumSupplies() >= 2 && (armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies() >= 1 || ((rock_weapons || rock_filler) && armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies() >= 1));
            int pending = armory.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            if (!can_make && left > 0 && pending == 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON, left);
            }
        }
        // Java: 943
        public void orderWeapons(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            if (armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() == 0)
            {
                owner.buildIronWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            if (owner.canUseRubber() && armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies() == 0)
            {
                owner.buildRubberWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            int rock_orders = armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            bool make_rock = rock_weapons || rock_filler;
            if (make_rock && rock_orders == 0)
            {
                owner.buildRockWeapons(armory, com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
            }
            else if (!make_rock && rock_orders > 0)
            {
                owner.buildRockWeapons(armory, -rock_orders, false);
            }
        }
        // Java: 957
        public void deployFromPrimary(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.Player owner = ai.owner();
            int workers = armory.getUnitContainer().getNumSupplies();
            int iron = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies();
            int chicken = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
            int rock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies();
            int stock = iron + chicken + rock;
            if (stock == 0 || workers == 0)
            {
                return;
            }
            com.oddlabs.tt.player.ai.Military military = ai.military();
            int pop = owner.getUnitCountContainer().getNumSupplies();
            bool capped = pop >= owner.getWorld().getMaxUnitCount() - 3;
            int deploy = default;
            if (military.wantsEverything() || capped)
            {
                deploy = stock;
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
                if (stock > 12)
                {
                    deploy = java.lang.Math.max(deploy, stock - 12);
                }
            }
            int keep = (military.wantsEverything() ? 0 : java.lang.Math.min(2, workers));
            deploy = java.lang.Math.min(deploy, java.lang.Math.min(stock, workers - keep));
            if (deploy <= 0)
            {
                return;
            }
            int c = java.lang.Math.min(chicken, deploy);
            if (c > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, c);
            }
            int i = java.lang.Math.min(iron, deploy - c);
            if (i > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, i);
            }
            int r = java.lang.Math.min(rock, deploy - c - i);
            if (r > 0)
            {
                owner.deployUnits(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, r);
            }
        }
        // Java: 1002
        public void computeGatherTargets()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            if (armory == null || armory_field == null)
            {
                want_tree = want_iron = want_rock = want_chicken = want_workers = 0;
                return;
            }
            com.oddlabs.tt.player.ai.MapAnalysis map = ai.map();
            tree_cycle = com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(armory_field, map.getTrees(), 60, 10, 120);
            iron_cycle = com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(armory_field, map.getIron(), 30, 10, 400);
            int iron_left = countReachable(map.getIron(), 400);
            rock_weapons = (iron_left == 0 || iron_cycle > 200.0f) && armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies() < 2;
            int iron_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.IronSupply)).getNumSupplies();
            int armory_workers = armory.getUnitContainer().getNumSupplies();
            if (!rock_filler && iron_stock <= 1 && armory_workers >= 14 && iron_cycle > 45.0f)
            {
                rock_filler = true;
            }
            else if (rock_filler && (iron_stock >= 5 || armory_workers < 8))
            {
                rock_filler = false;
            }
            float ore_cycle = (rock_weapons ? com.oddlabs.tt.player.ai.SitePlanner.gatherSeconds(armory_field, map.getRocks(), 30, 10, 240) : iron_cycle);
            float work = (rock_weapons ? (float)(com.oddlabs.tt.player.ai.Economy.IRON_WORK / 2) : com.oddlabs.tt.player.ai.Economy.IRON_WORK);
            int workers = armory.getUnitContainer().getNumSupplies();
            int g_tree = intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE, armory);
            int g_iron = intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON, armory);
            int g_rock = intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK, armory);
            int g_chicken = intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN, armory);
            int transit = intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT);
            int pool = workers + g_tree + g_iron + g_rock + g_chicken + transit;
            want_chicken = 0;
            int chickens_left = countChickens();
            com.oddlabs.tt.player.ai.Strategy strategy = ai.strategy();
            if (ai.owner().canUseRubber() && ai.time() > strategy.chicken_time && pool > 10 && chickens_left > 0)
            {
                want_chicken = java.lang.Math.min(java.lang.Math.min(strategy.chicken_hunters, chickens_left), 2 + pool / strategy.chicken_pool_div);
            }
            int rock_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RockSupply)).getNumSupplies();
            int chicken_stock = armory.getSupplyContainer(typeof(com.oddlabs.tt.model.RubberSupply)).getNumSupplies();
            want_rock = ((want_chicken > 0 || chicken_stock > 0) && rock_stock < 6 ? 1 + want_chicken / 4 : 0);
            if (rock_filler && !rock_weapons && rock_stock < 20)
            {
                want_rock += java.lang.Math.max(2, armory_workers / 10);
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
            if (rock_weapons)
            {
                want_rock += want_ore;
                want_iron = 0;
            }
            else
            {
                want_iron = want_ore;
            }
            want_workers = java.lang.Math.max(2, pool - want_tree - want_ore);
        }
        // Java: 1072
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
        // Java: 1088
        public void allocatePeons()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            java.util.List free = new java.util.ArrayList();
            java.util.List transit = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(peon));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.IDLE))
                {
                    free.add(peon);
                }
                else if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT))
                {
                    transit.add(peon);
                }
            }
            if (scout != null && (scout.isDead() || !scoutHasWork()))
            {
                scout = (scout.isDead() ? (com.oddlabs.tt.model.Unit)(null) : scout);
            }
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (!p.isPlaced())
                {
                    continue;
                }
                int need = buildersWanted(p) - builderCount(p.building);
                if (need <= 0)
                {
                    continue;
                }
                java.util.List chosen = new java.util.ArrayList();
                takeNearest(free, chosen, need, p.site.x, p.site.y);
                takeNearest(transit, chosen, need - chosen.size(), p.site.x, p.site.y);
                if (chosen.size() < need && (p.type == com.oddlabs.tt.model.Race.BUILDING_ARMORY || p.first))
                {
                    takeGatherers(chosen, need - chosen.size(), p.site.x, p.site.y);
                }
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(chosen))
                {
                    if (((object)u == (object)scout) && scoutHasWork())
                    {
                        continue;
                    }
                    order(u, p.building, com.oddlabs.tt.model.Action.DEFAULT);
                    intel.peon_states.put(u, com.oddlabs.tt.player.ai.Intel.PeonState.BUILD);
                    intel.builder_sites.put(u, p.building);
                }
            }
            chieftain_topup = false;
            com.oddlabs.tt.model.Building trainer = ai.chieftain().trainingQuarters();
            if (trainer != null && ai.military().baseThreatLevel() == 0)
            {
                int need_2 = ai.strategy().hold_chieftain - trainer.getUnitContainer().getNumSupplies() - countHeadingTo(trainer);
                if (need_2 > 0)
                {
                    java.util.List chosen_2 = new java.util.ArrayList();
                    takeNearest(free, chosen_2, need_2, trainer.getGridX(), trainer.getGridY());
                    takeNearest(transit, chosen_2, need_2 - chosen_2.size(), trainer.getGridX(), trainer.getGridY());
                    order(chosen_2, trainer, com.oddlabs.tt.model.Action.DEFAULT);
                    chieftain_topup = true;
                }
            }
            if (armory == null)
            {
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(free))
                {
                    com.oddlabs.tt.model.Building q = com.oddlabs.tt.player.ai.Economy.nearest(intel.quarters, u_2.getGridX(), u_2.getGridY());
                    if (q != null && q.getUnitContainer().getNumSupplies() + countHeadingTo(q) < holdFor(q))
                    {
                        order(u_2, q, com.oddlabs.tt.model.Action.DEFAULT);
                    }
                }
                return;
            }
            bool danger = ai.military().baseThreatLevel() > 1 && (!gatherUnderThreat() || ai.military().threatNear(armory.getGridX(), armory.getGridY(), 16));
            int[] have = new int[] { intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE, armory), intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON, armory), intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK, armory), intel.countGatherers(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN, armory) };
            int[] want = new int[] { want_tree, want_iron, want_rock, want_chicken };
            System.Type[] types = new System.Type[] { typeof(com.oddlabs.tt.landscape.TreeSupply), typeof(com.oddlabs.tt.model.IronSupply), typeof(com.oddlabs.tt.model.RockSupply), typeof(com.oddlabs.tt.model.RubberSupply) };
            rebuildSupplyLoad();
            int deploy_for_gathering = 0;
            for (int t = 0; t < 4; t++)
            {
                int need_3 = (danger ? 0 : want[t] - have[t]);
                while (need_3 > 0)
                {
                    com.oddlabs.tt.model.Unit u_3 = (free.isEmpty() ? (com.oddlabs.tt.model.Unit)(null) : ((com.oddlabs.tt.model.Unit)free.removeFirst()));
                    if (u_3 == null && !transit.isEmpty())
                    {
                        u_3 = ((com.oddlabs.tt.model.Unit)transit.removeFirst());
                    }
                    if (u_3 == null)
                    {
                        deploy_for_gathering += need_3;
                        break;
                    }
                    if (!sendGatherer(u_3, types[t], armory))
                    {
                        break;
                    }
                    need_3--;
                }
                if (!danger && have[t] > want[t] + 1)
                {
                    ai.owner().recallGatherers(armory, com.oddlabs.tt.player.ai.Economy.supplyClass(types[t]), have[t] - want[t]);
                }
            }
            retargetGatherers(armory);
            unstickGatherers(armory);
            int workers = armory.getUnitContainer().getNumSupplies();
            int pending = armory.getDeployContainer(com.oddlabs.tt.model.DeployType.PEON).getNumSupplies();
            if (deploy_for_gathering > 0 && workers > 3 && pending == 0)
            {
                ai.owner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON, java.lang.Math.min(deploy_for_gathering, workers - 3));
            }
            foreach (com.oddlabs.tt.model.Unit u_4 in java.lang.JIter.of(free))
            {
                order(u_4, armory, com.oddlabs.tt.model.Action.DEFAULT);
            }
        }
        // Java: 1188
        public bool scoutHasWork()
        {
            foreach (com.oddlabs.tt.player.ai.Economy.Project p in java.lang.JIter.of(projects))
            {
                if (p.use_scout && !p.isPlaced())
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1195
        public static System.Type supplyClass(System.Type c)
        {
            return ((System.Type)c);
        }
        // Java: 1200
        public int countHeadingTo(com.oddlabs.tt.model.Building building)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(ai.intel().peons))
            {
                if (((object)((com.oddlabs.tt.player.ai.Intel.PeonState)ai.intel().peon_states.get(peon)) != (object)com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT))
                {
                    continue;
                }
                if (com.oddlabs.tt.player.ai.MapAnalysis.dist2(peon.getGridX(), peon.getGridY(), building.getGridX(), building.getGridY()) < 900)
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 1211
        public void takeNearest(java.util.List from, java.util.List into, int n, int x, int y)
        {
            for (int k = 0; k < n && !from.isEmpty(); k++)
            {
                com.oddlabs.tt.model.Unit best = null;
                int best_d = java.lang.Integer.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(from))
                {
                    int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(u.getGridX(), u.getGridY(), x, y);
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
        // Java: 1230
        public void takeGatherers(java.util.List into, int n, int x, int y)
        {
            java.util.List gatherers = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(ai.intel().peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)ai.intel().peon_states.get(peon));
                if (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK))
                {
                    gatherers.add(peon);
                }
            }
            takeNearest(gatherers, into, n, x, y);
        }
        // Java: 1240
        public static com.oddlabs.tt.model.Building nearest(java.util.List buildings, int x, int y)
        {
            com.oddlabs.tt.model.Building best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building b in java.lang.JIter.of(buildings))
            {
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(b.getGridX(), b.getGridY(), x, y);
                if (d < best_d)
                {
                    best_d = d;
                    best = b;
                }
            }
            return best;
        }
        // Java: 1253
        public void rebuildSupplyLoad()
        {
            supply_load.clear();
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            for (java.util.Iterator it = gather_targets.entrySet().iterator(); it.hasNext(); )
            {
                java.util.Map.Entry e = ((java.util.Map.Entry)it.next());
                com.oddlabs.tt.model.Unit u = ((com.oddlabs.tt.model.Unit)e.getKey());
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(u));
                if (u.isDead() || s == null || !com.oddlabs.tt.player.ai.Economy.isGathering(s))
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
        // Java: 1270
        public static bool isGathering(com.oddlabs.tt.player.ai.Intel.PeonState s)
        {
            return ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN);
        }
        // Java: 1275
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
        // Java: 1289
        public void unstickGatherers(com.oddlabs.tt.model.Building armory)
        {
            if (!ai.strategy().unstick)
            {
                return;
            }
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            float now = ai.time();
            gather_progress.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            bad_supplies.values().removeIf(new java.util.function.Predicate_L((object until_l) =>
            {
                float? until = (float?)until_l;
                return ((float)until) < now;
            }));
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(peon));
                System.Type type = (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE) ? typeof(com.oddlabs.tt.landscape.TreeSupply) : (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON) ? typeof(com.oddlabs.tt.model.IronSupply) : (((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK) ? typeof(com.oddlabs.tt.model.RockSupply) : (System.Type)(null))));
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
        // Java: 1330
        public void retargetGatherers(com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            int moved = 0;
            foreach (com.oddlabs.tt.model.Unit peon in java.lang.JIter.of(intel.peons))
            {
                if (moved >= 4)
                {
                    return;
                }
                com.oddlabs.tt.player.ai.Intel.PeonState s = ((com.oddlabs.tt.player.ai.Intel.PeonState)intel.peon_states.get(peon));
                if (s == null || !com.oddlabs.tt.player.ai.Economy.isGathering(s) || ((object)s == (object)com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN))
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
        // Java: 1360
        public com.oddlabs.tt.model.Supply pickSupply(System.Type type, com.oddlabs.tt.model.Building armory, com.oddlabs.tt.model.Unit peon)
        {
            if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
            {
                return pickChicken(armory);
            }
            com.oddlabs.tt.player.ai.DistanceField field = armory_field;
            java.util.List supplies = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? ai.map().getTrees() : (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)) ? ai.map().getIron() : ai.map().getRocks()));
            int max_load = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? com.oddlabs.tt.player.ai.Economy.TREE_LOAD : com.oddlabs.tt.player.ai.Economy.ORE_LOAD);
            float load_penalty = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? 9.0f : 6.0f);
            com.oddlabs.tt.model.Supply best = null;
            float best_cost = java.lang.Float.MAX_VALUE;
            int ax = armory.getGridX();
            int ay = armory.getGridY();
            int radius = (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)) ? 60 : 200);
            foreach (com.oddlabs.tt.model.Supply s in java.lang.JIter.of(supplies))
            {
                if (s.isEmpty())
                {
                    continue;
                }
                int d2 = com.oddlabs.tt.player.ai.MapAnalysis.dist2(ax, ay, s.getGridX(), s.getGridY());
                if (d2 > radius * radius)
                {
                    continue;
                }
                int d = (field != null ? field.getAround(s.getGridX(), s.getGridY(), 1) : java.lang.JC.d2i((java.lang.Math.sqrt(d2) * 2)));
                if (d == com.oddlabs.tt.player.ai.DistanceField.UNREACHABLE)
                {
                    continue;
                }
                if (ai.military().threatNear(s.getGridX(), s.getGridY(), 14))
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
        // Java: 1396
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
        // Java: 1418
        public com.oddlabs.tt.model.Supply pickChicken(com.oddlabs.tt.model.Building armory)
        {
            countChickens();
            com.oddlabs.tt.model.RubberSupply best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.RubberSupply c in java.lang.JIter.of(chickens))
            {
                if (c.isEmpty() || c.isHit())
                {
                    continue;
                }
                if (((int)((int?)supply_load.getOrDefault(c, 0))) > 0)
                {
                    continue;
                }
                if (ai.military().enemyStrengthNear(c.getGridX(), c.getGridY(), 20) > 0)
                {
                    continue;
                }
                int d = com.oddlabs.tt.player.ai.MapAnalysis.dist2(armory.getGridX(), armory.getGridY(), c.getGridX(), c.getGridY());
                if (d < best_d)
                {
                    best_d = d;
                    best = c;
                }
            }
            if (best != null && best_d > 150 * 150)
            {
                return null;
            }
            return best;
        }
        // Java: 1443
        public void manageRepairs()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
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
                if (ai.military().threatNear(b_4.getGridX(), b_4.getGridY(), 12))
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
        // Java: 1473
        public int wantWorkers()
        {
            return want_workers;
        }
        // Java: 1477
        public string debugStatus()
        {
            com.oddlabs.tt.player.ai.Intel intel = ai.intel();
            com.oddlabs.tt.model.Building armory = intel.armory();
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            sb.append("Q").append(intel.quarters.size()).append('+').append(intel.quarters_sites.size());
            sb.append(" A").append(intel.armories.size()).append('+').append(intel.armory_sites.size());
            sb.append(" T").append(intel.towers.size()).append('+').append(intel.tower_sites.size());
            sb.append(" proj=").append(projects.size());
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
            sb.append(" (idle ").append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.IDLE));
            sb.append(" bld ").append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.BUILD));
            sb.append(" tr ").append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.TRANSIT));
            sb.append(" g ").append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_TREE)).append('/').append(want_tree);
            sb.append(',').append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_IRON)).append('/').append(want_iron);
            sb.append(',').append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_ROCK)).append('/').append(want_rock);
            sb.append(',').append(intel.countPeons(com.oddlabs.tt.player.ai.Intel.PeonState.GATHER_CHICKEN)).append('/').append(want_chicken);
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
            return java.lang.JO.toString(sb);
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
            this.gather_progress = new java.util.LinkedHashMap();
            this.bad_supplies = new java.util.LinkedHashMap();
        }
        public Economy(com.oddlabs.tt.player.ai.ExpertAI ai) : base()
        {
            init_Economy();
            this.ai = ai;
            openingPlan();
        }
    }
}

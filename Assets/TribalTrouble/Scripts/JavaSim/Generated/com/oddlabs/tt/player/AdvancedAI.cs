// Translated from AdvancedAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 52
    public class AdvancedAI : com.oddlabs.tt.player.AI
    {
        public const int DIFFICULTY_EASY = 0;
        public const int DIFFICULTY_NORMAL = 1;
        public const int DIFFICULTY_HARD = 2;
        public const int DIFFICULTY_INSANE = 3;
        public const int SCORE_PEON = 2;
        public const int SCORE_WARRIOR_ROCK = 5;
        public const int SCORE_WARRIOR_IRON = 10;
        public const int SCORE_WARRIOR_RUBBER = 15;
        public const int SCORE_CHIEFTAIN = 25;
        public static float[] DEFENSE_FACTOR;
        public static int[] MIN_UNITS_BUILDING_WEAPONS;
        public static int[] MIN_WEAPONS_IN_STOCK;
        public static int[] MIN_UNITS_REPRODUCING;
        public static int[] MAX_UNITS_GATHERING_TREE;
        public static int[] MAX_UNITS_GATHERING_ROCK;
        public static int[] MAX_UNITS_GATHERING_IRON;
        public static int[] MAX_UNITS_GATHERING_RUBBER;
        public const float WARRIOR_STUCK_THRESHOLD = 4.0f;
        public java.util.Map warrior_positions;
        public const float CHIEFTAIN_IDLE_RETURN_SECONDS = 15.0f;
        public float chieftain_idle_time;
        public const float CHIEF_HITRUN_HEALTH_PCT = 0.3f;
        public const int CHIEF_HITRUN_MAX_ESCORT = 25;
        public const float CHIEF_HITRUN_SEARCH_RADIUS = 450.0f;
        public const int GROUP_HITRUN_THRESHOLD = 2;
        public java.util.Set chief_hitrun_units;
        public java.util.Set patrol_units;
        public java.util.Set escort_units;
        // Java: 124
        public int escortsPerGatherGroup()
        {
            return getOwner().getWorld().rollMatchValue(com.oddlabs.tt.landscape.World.SALT_ESCORTS_PER_GATHER_GROUP, 4, 9);
        }
        // Java: 127
        public int patrolForceSize()
        {
            return getOwner().getWorld().rollMatchValue(com.oddlabs.tt.landscape.World.SALT_PATROL_FORCE_SIZE, 6, 22);
        }
        public const float PATROL_RADIUS = 300.0f;
        public int patrol_waypoint_index;
        public const int OUTPOST_TRIGGER_DIST = 600;
        public bool outpost_built;
        // Java: 137
        public int targetNumQuarters()
        {
            return getOwner().getWorld().getTargetNumQuarters();
        }
        // Java: 143
        public int targetNumArmories()
        {
            return getOwner().getWorld().getTargetNumArmories();
        }
        // Java: 150
        public int maxConcurrentQuarters()
        {
            return getOwner().getWorld().getMaxConcurrentQuarters();
        }
        // Java: 154
        public int maxConcurrentArmories()
        {
            return getOwner().getWorld().getMaxConcurrentArmories();
        }
        public const float DEFENSE_SCAN_RADIUS = 30.0f;
        public java.util.List occupied_zones;
        public float occupied_zones_cleanup_cooldown;
        // Java: 174
        public void nodeCleanupOccupiedZones(float t)
        {
            occupied_zones_cleanup_cooldown -= t;
            if (occupied_zones_cleanup_cooldown > 0.0f)
            {
                return;
            }
            occupied_zones_cleanup_cooldown = 15.0f;
            occupied_zones.removeIf(new java.util.function.Predicate_L((object zone_l) =>
            {
                int[] zone = (int[])zone_l;
                float world_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(zone[0]);
                float world_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(zone[1]);
                com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(world_x, world_y, 15.0f, null, typeof(com.oddlabs.tt.model.Building));
                getUnitGrid().scan(filter, zone[0], zone[1]);
                foreach (com.oddlabs.tt.model.Building building in java.lang.JIter.of(filter.getResult()))
                {
                    if (!building.isDead() && ((object)building.getOwner() == (object)getOwner()))
                    {
                        return false;
                    }
                }
                return true;
            }));
        }
        public java.util.Set attacking_units;
        public const float RETREAT_SCAN_RADIUS = 30.0f;
        // Java: 197
        public int numResourceTowers()
        {
            return getOwner().getWorld().getNumResourceTowers();
        }
        public int resource_towers_built;
        public const int BUILDERS_PER_TOWER_TARGET = 10;
        public const int TOWER_SPACING_SLOTS = 12;
        public const float TOWER_BASE_RADIUS = 16.0f;
        public const float TOWER_RADIUS_STEP = 10.0f;
        public const int TOWER_SCAN_RANGE = 18;
        // Java: 207
        public int maxConcurrentTowers()
        {
            return getOwner().getWorld().getMaxConcurrentTowers();
        }
        public static int[][] UNITS_PER_TOWER;
        public const int SHIP_PEONS = 26;
        public const int SHIP_WARRIORS = 26;
        public const int SHIP_BUILDERS = 20;
        public const float SHIP_MIN_HEALTH = 0.35f;
        public const int SHIP_HOME_RANGE = 50;
        public const int FLEET_SIZE = 5;
        public const int SEA_ENGAGE_RANGE = 45;
        public const float UNLOAD_SECONDS = 30.0f;
        public java.util.Map ship_unloading;
        public const int LAUNCH_WARRIORS = 20;
        public const int LAUNCH_WARRIORS_MIN = 8;
        public const float LAUNCH_WAIT_SECONDS = 150.0f;
        public const float LOAD_PERIOD_SECONDS = 25.0f;
        public java.util.Map ship_load_tick;
        public java.util.Map ship_ready_since;
        public java.util.Map ship_departures_failed;
        public java.util.Map ship_sail_point;
        public java.util.Map ship_last_pos;
        public const float JAM_SECONDS = 18.0f;
        public java.util.Map ship_hop;
        public int jams;
        public bool last_ship_site;
        // Java: 378
        public class ExplorationTarget : java.lang.Object
        {
            public int island_id() { return island_id_f; }
            public bool hostile() { return hostile_f; }
            public ExplorationTarget(int island_id, bool hostile)
            {
                this.island_id_f = island_id;
                this.hostile_f = hostile;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is ExplorationTarget r)) return false; return true && java.lang.JO.equals(island_id_f, r.island_id_f) && java.lang.JO.equals(hostile_f, r.hostile_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(island_id_f); h = 31 * h + java.lang.JO.hashCode(hostile_f); } return h; }
            public override string toString() { return "ExplorationTarget[" + "island_id=" + java.lang.JO.toString(island_id_f) + ", hostile=" + java.lang.JO.toString(hostile_f) + "]"; }
            public int island_id_f;
            public bool hostile_f;
        }
        public java.util.Map ship_exploration;
        public const int ISLAND_ARRIVAL_RANGE = 60;
        public const int MIN_ISLAND_RESOURCES = 20;
        public const int EXPLORE_PEON_BATCH = 15;
        public const int EXPLORE_WARRIOR_BATCH = 15;
        public int difficulty;
        public bool adaptive;
        public com.oddlabs.tt.player.PlayerStyleContext selected_context;
        public int start_difficulty;
        public com.oddlabs.tt.player.AiBehaviorProfile selected_behavior;
        public int[] NUM_WARRIORS;
        public int[] NUM_WARRIORS_INCREASE;
        public int[] NUM_WARRIORS_MAX;
        public int[] NUM_WARRIORS_FOR_CHIEFTAIN;
        // Java: 412
        public class SupplyLocationFilter : java.lang.Object, com.oddlabs.tt.pathfinder.ScanFilter
        {
            public System.Type supply_class;
            public int range;
            public const int MIN_DISTANCE_FROM_EXCLUDED = 20;
            public java.util.List excluded_zones;
            public com.oddlabs.tt.util.Target result;
            // Java: 435
            public int getMinRadius()
            {
                return 0;
            }
            // Java: 440
            public int getMaxRadius()
            {
                return range;
            }
            // Java: 445
            public bool filter(int grid_x, int grid_y, com.oddlabs.tt.pathfinder.Occupant occ)
            {
                if (occ != null && supply_class.isInstance(occ))
                {
                    foreach (int[] zone in java.lang.JIter.of(excluded_zones))
                    {
                        int dx = grid_x - zone[0];
                        int dy = grid_y - zone[1];
                        if (dx * dx + dy * dy < com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter.MIN_DISTANCE_FROM_EXCLUDED * com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter.MIN_DISTANCE_FROM_EXCLUDED)
                        {
                            return false;
                        }
                    }
                    result = ((com.oddlabs.tt.util.Target)occ);
                    return true;
                }
                return false;
            }
            // Java: 460
            public bool found()
            {
                return result != null;
            }
            // Java: 464
            public com.oddlabs.tt.util.Target getTarget()
            {
                return result;
            }
            // Java: 468
            public int getX()
            {
                return result.getGridX();
            }
            // Java: 472
            public int getY()
            {
                return result.getGridY();
            }
            public SupplyLocationFilter(System.Type supply_class, int range) : this(supply_class, range, java.util.List_.of())
            {
            }
            public SupplyLocationFilter(System.Type supply_class, int range, java.util.List excluded_zones) : base()
            {
                this.supply_class = supply_class;
                this.range = range;
                this.excluded_zones = excluded_zones;
            }
        }
        // Java: 477
        public class Threat : java.lang.Object
        {
            public com.oddlabs.tt.landscape.LandscapeTarget target;
            public int score;
            public string kind;
            public Threat(com.oddlabs.tt.landscape.LandscapeTarget target, int score, string kind) : base()
            {
                this.target = target;
                this.score = score;
                this.kind = kind;
            }
        }
        public com.oddlabs.tt.landscape.LandscapeTarget defense_target;
        public float time_since_attack;
        public com.oddlabs.tt.util.Target tracked_main_attack_target;
        public float main_attack_target_check_cooldown;
        public const float ATTACK_STALL_SECONDS = 90.0f;
        public const int MIN_WARRIORS_FOR_STALL_ATTACK = 8;
        // Java: 558
        public int getDifficulty()
        {
            return difficulty;
        }
        // Java: 563
        public void setTrainingContext(com.oddlabs.tt.player.PlayerStyleContext context)
        {
            if (adaptive)
            {
                selected_context = context;
            }
        }
        // Java: 568
        public com.oddlabs.tt.player.AiBehaviorProfile getSelectedBehavior()
        {
            return selected_behavior;
        }
        // Java: 576
        public override bool isHardDifficulty()
        {
            return !adaptive && difficulty == com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_HARD;
        }
        // Java: 585
        public bool isAdaptive()
        {
            return adaptive;
        }
        // Java: 602
        public void safeRun(string node_name, java.lang.Runnable node)
        {
            try
            {
                node.run();
            }
            catch (System.Exception e)
            {
                java.lang.JSystem.err.println("AdvancedAI node '" + java.lang.JStr.valueOf(node_name) + "' threw and was skipped for this tick:");
                e.printStackTrace();
            }
        }
        // Java: 611
        public override void animate(float t)
        {
            committed_this_tick.clear();
            time_since_attack += t;
            match_elapsed_seconds += t;
            time_since_last_recorded_wave += t;
            safeRun("nodeAdaptiveDifficulty", new java.lang.Runnable_L(() =>
            {
                nodeAdaptiveDifficulty(t);
            }));
            safeRun("nodeUpdatePlayerStyle", new java.lang.Runnable_L(() =>
            {
                nodeUpdatePlayerStyle(t);
            }));
            if (relay() != null)
            {
                return;
            }
            safeRun("nodeCheckIdleChieftain", new java.lang.Runnable_L(() =>
            {
                nodeCheckIdleChieftain(t);
            }));
            safeRun("nodeCheckStuckWarriors", new java.lang.Runnable_L(() =>
            {
                nodeCheckStuckWarriors(t);
            }));
            safeRun("nodeCheckAttackingArmy", new java.lang.Runnable_L(() =>
            {
                nodeCheckAttackingArmy(t);
            }));
            if (!shouldDoAction(t))
            {
                return;
            }
            reclassify();
            safeRun("nodeBuildQuarters", new java.lang.Runnable_L(() => { this.nodeBuildQuarters(); }));
            safeRun("nodeBuildArmory", new java.lang.Runnable_L(() => { this.nodeBuildArmory(); }));
            safeRun("nodeBuildWeapons", new java.lang.Runnable_L(() => { this.nodeBuildWeapons(); }));
            safeRun("nodeCleanupOccupiedZones", new java.lang.Runnable_L(() =>
            {
                nodeCleanupOccupiedZones(t);
            }));
            safeRun("nodeCheckMainAttackTargetStillAlive", new java.lang.Runnable_L(() =>
            {
                nodeCheckMainAttackTargetStillAlive(t);
            }));
            safeRun("nodeDeployArmy", new java.lang.Runnable_L(() => { this.nodeDeployArmy(); }));
            safeRun("nodeTrainChieftain", new java.lang.Runnable_L(() => { this.nodeTrainChieftain(); }));
            safeRun("nodeCheckOutpost", new java.lang.Runnable_L(() => { this.nodeCheckOutpost(); }));
            reclassify();
            safeRun("nodeBuildResourceTowers", new java.lang.Runnable_L(() => { this.nodeBuildResourceTowers(); }));
            reclassify();
            safeRun("nodeDefendBase", new java.lang.Runnable_L(() => { this.nodeDefendBase(); }));
            reclassify();
            reclassify();
            safeRun("nodeDefendAllies", new java.lang.Runnable_L(() => { this.nodeDefendAllies(); }));
            safeRun("nodeDonateToAllies", new java.lang.Runnable_L(() => { this.nodeDonateToAllies(); }));
            safeRun("nodeGarrisonAllyTowers", new java.lang.Runnable_L(() => { this.nodeGarrisonAllyTowers(); }));
            reclassify();
            safeRun("nodeUpdateTowerTarget", new java.lang.Runnable_L(() => { this.nodeUpdateTowerTarget(); }));
            reclassify();
            if (isArchipelago())
            {
                reclassify();
                if (baseBuildingsDone())
                {
                    safeRun("nodeBuildShipAndLoad", new java.lang.Runnable_L(() => { this.nodeBuildShipAndLoad(); }));
                }
                safeRun("nodeUseShip", new java.lang.Runnable_L(() => { this.nodeUseShip(); }));
            }
            reclassify();
            bool favorable = default;
            bool stalled = default;
            try
            {
                favorable = computeArmyStrength(getOwner()) >= computeStrongestEnemyStrength();
                stalled = time_since_attack >= getStyleAdjustedStallSeconds() && countWarriors() >= getEffectiveMinWarriorsForStallAttack();
            }
            catch (System.Exception e)
            {
                java.lang.JSystem.@out.println("AdvancedAI offense-gate computation threw and was skipped for this tick: " + java.lang.JStr.valueOf(e));
                favorable = false;
                stalled = false;
            }
            if (favorable || stalled)
            {
                time_since_attack = 0.0f;
                main_attack_wave_count++;
                int attack_force = java.lang.JC.f2i(((float)(NUM_WARRIORS[difficulty] * attackForceMultiplier())));
                safeRun("nodeAttackWithWarriorsAndChieftain", new java.lang.Runnable_L(() =>
                {
                    nodeAttackWithWarriorsAndChieftain(attack_force, attack_force >= NUM_WARRIORS_FOR_CHIEFTAIN[difficulty]);
                }));
            }
            safeRun("nodeDeployUnitsInArmory", new java.lang.Runnable_L(() =>
            {
                int target = NUM_WARRIORS[difficulty];
                com.oddlabs.tt.model.Selectable[] idle = getIdleWarriors();
                nodeDeployUnitsInArmory((idle != null ? target - idle.Length : target));
            }));
            safeRun("nodeChiefHitAndRun", new java.lang.Runnable_L(() => { this.nodeChiefHitAndRun(); }));
            safeRun("nodeRespondToBeacon", new java.lang.Runnable_L(() => { this.nodeRespondToBeacon(); }));
            safeRun("nodeChiefHitAndRunReturn", new java.lang.Runnable_L(() => { this.nodeChiefHitAndRunReturn(); }));
            safeRun("nodePatrol", new java.lang.Runnable_L(() => { this.nodePatrol(); }));
            safeRun("nodeAllInAttack", new java.lang.Runnable_L(() => { this.nodeAllInAttack(); }));
            safeRun("nodeFavorableAttack", new java.lang.Runnable_L(() =>
            {
                nodeFavorableAttack(t);
            }));
            safeRun("nodeClosestPlayerAttack", new java.lang.Runnable_L(() =>
            {
                nodeClosestPlayerAttack(t);
            }));
            safeRun("nodeRecallStrandedWarriors", new java.lang.Runnable_L(() =>
            {
                nodeRecallStrandedWarriors(t);
            }));
            safeRun("nodeBestMoveAttack", new java.lang.Runnable_L(() =>
            {
                nodeBestMoveAttack(t);
            }));
            safeRun("nodeMultiFrontAttack", new java.lang.Runnable_L(() =>
            {
                nodeMultiFrontAttack(t);
            }));
            safeRun("nodeCheckAllyOutpost", new java.lang.Runnable_L(() => { this.nodeCheckAllyOutpost(); }));
            safeRun("nodePlayKingOfTheIslandObjective", new java.lang.Runnable_L(() =>
            {
                nodePlayKingOfTheIslandObjective(t);
            }));
            safeRun("nodeEscortGatherers", new java.lang.Runnable_L(() => { this.nodeEscortGatherers(); }));
            safeRun("nodeAssignIdlePeons", new java.lang.Runnable_L(() => { this.nodeAssignIdlePeons(); }));
            reclassify();
            safeRun("nodeAssignIdleGatherers", new java.lang.Runnable_L(() => { this.nodeAssignIdleGatherers(); }));
            com.oddlabs.tt.model.Unit primary_chieftain = getOwner().getChieftain();
            if (primary_chieftain != null && !primary_chieftain.isDead())
            {
                safeRun("ChieftainAI.decide(primary)", new java.lang.Runnable_L(() =>
                {
                    getOwner().getRace().getChieftainAI().decide(primary_chieftain);
                }));
            }
            foreach (com.oddlabs.tt.model.Unit extra in java.lang.JIter.of(getOwner().getExtraChieftains()))
            {
                if (!extra.isDead())
                {
                    safeRun("ChieftainAI.decide(extra)", new java.lang.Runnable_L(() =>
                    {
                        getOwner().getRace().getChieftainAI().decide(extra);
                    }));
                }
            }
        }
        // Java: 836
        public void nodeUpdateTowerTarget()
        {
            int num_supplies = getOwner().getUnitCountContainer().getNumSupplies();
            for (int tier = com.oddlabs.tt.player.AdvancedAI.UNITS_PER_TOWER.Length; tier >= 1; tier--)
            {
                if (num_supplies > com.oddlabs.tt.player.AdvancedAI.UNITS_PER_TOWER[tier - 1][difficulty])
                {
                    nodeGuardTowers(tier);
                    break;
                }
            }
        }
        // Java: 846
        public void nodePatrol()
        {
            patrol_waypoint_index++;
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return;
            }
            int hx = home.getGridX();
            int hy = home.getGridY();
            java.util.List points = new java.util.ArrayList();
            int num_points = 6;
            for (int i = 0; i < num_points; i++)
            {
                double angle = (patrol_waypoint_index + i) * (2 * java.lang.Math.PI / num_points);
                double radius = com.oddlabs.tt.player.AdvancedAI.PATROL_RADIUS;
                int px = java.lang.JC.d2i((hx + radius * java.lang.StrictMath.cos(angle)));
                int py = java.lang.JC.d2i((hy + radius * java.lang.StrictMath.sin(angle)));
                double min_radius = com.oddlabs.tt.player.AdvancedAI.PATROL_RADIUS * 0.3d;
                while (countNearbyEnemyTowers(px, py) > 0 && radius > min_radius)
                {
                    radius -= com.oddlabs.tt.player.AdvancedAI.PATROL_RADIUS * 0.15d;
                    px = java.lang.JC.d2i((hx + radius * java.lang.StrictMath.cos(angle)));
                    py = java.lang.JC.d2i((hy + radius * java.lang.StrictMath.sin(angle)));
                }
                points.add(new int[] { px, py });
            }
            addGathererLocation(points, getGatherTreePeons());
            addGathererLocation(points, getGatherRockPeons());
            addGathererLocation(points, getGatherIronPeons());
            addGathererLocation(points, getGatherRubberPeons());
            com.oddlabs.tt.model.Selectable[] idle = getIdleWarriors();
            if (idle == null || idle.Length == 0 || points.isEmpty())
            {
                return;
            }
            int patrol_count = java.lang.Math.min(patrolForceSize(), idle.Length);
            for (int i_2 = 0; i_2 < patrol_count; i_2++)
            {
                int[] point = ((int[])points.get(i_2 % points.size()));
                com.oddlabs.tt.model.Unit warrior = ((com.oddlabs.tt.model.Unit)idle[i_2]);
                getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(warrior), point[0], point[1], com.oddlabs.tt.model.Action.DEFEND, true);
                patrol_units.add(warrior);
            }
        }
        // Java: 899
        public void nodeEscortGatherers()
        {
            escortGroup(getGatherTreePeons());
            escortGroup(getGatherRockPeons());
            escortGroup(getGatherIronPeons());
            escortGroup(getGatherRubberPeons());
        }
        // Java: 906
        public void escortGroup(com.oddlabs.tt.model.Selectable[] gatherers)
        {
            if (gatherers == null || gatherers.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            if (idle_warriors == null || idle_warriors.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable rep = gatherers[0];
            int count = java.lang.Math.min(escortsPerGatherGroup(), idle_warriors.Length);
            for (int i = 0; i < count; i++)
            {
                com.oddlabs.tt.model.Unit warrior = ((com.oddlabs.tt.model.Unit)idle_warriors[i]);
                getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(warrior), rep.getGridX(), rep.getGridY(), com.oddlabs.tt.model.Action.DEFEND, true);
                escort_units.add(warrior);
            }
        }
        // Java: 923
        public void addGathererLocation(java.util.List points, com.oddlabs.tt.model.Selectable[] gatherers)
        {
            if (gatherers != null && gatherers.Length > 0)
            {
                com.oddlabs.tt.model.Selectable rep = gatherers[0];
                points.add(new int[] { rep.getGridX(), rep.getGridY() });
            }
        }
        // Java: 933
        public void nodeCheckAttackingArmy(float t)
        {
            attacking_units.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            if (attacking_units.isEmpty())
            {
                return;
            }
            float cx = 0.0f;
            float cy = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(attacking_units))
            {
                cx = (float)(cx + u.getGridX());
                cy = (float)(cy + u.getGridY());
            }
            cx = (float)(cx / attacking_units.size());
            cy = (float)(cy / attacking_units.size());
            float own_score = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(attacking_units))
            {
                own_score = (float)(own_score + getUnitScore(u_2));
            }
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(cx, cy, com.oddlabs.tt.player.AdvancedAI.RETREAT_SCAN_RADIUS, null, typeof(com.oddlabs.tt.model.Unit));
            getUnitGrid().scan(filter, java.lang.JC.f2i(cx), java.lang.JC.f2i(cy));
            float enemy_score = 0.0f;
            foreach (com.oddlabs.tt.model.Unit u_3 in java.lang.JIter.of(filter.getResult()))
            {
                if (!u_3.isDead() && getOwner().isEnemy(u_3.getOwner()))
                {
                    enemy_score = (float)(enemy_score + getUnitScore(u_3));
                }
            }
            if (enemy_score > (float)(own_score * 1.5f))
            {
                com.oddlabs.tt.model.Building home = homeBuilding();
                if (home != null)
                {
                    getOwner().setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])attacking_units.toArray(new com.oddlabs.tt.model.Selectable[0])), home.getGridX(), home.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                }
                attacking_units.clear();
            }
        }
        public float stuck_check_interval;
        public const float STUCK_CHECK_INTERVAL_SECONDS = 0.5f;
        // Java: 975
        public void nodeCheckStuckWarriors(float t)
        {
            stuck_check_interval += t;
            if (stuck_check_interval < com.oddlabs.tt.player.AdvancedAI.STUCK_CHECK_INTERVAL_SECONDS)
            {
                return;
            }
            float elapsed = stuck_check_interval;
            stuck_check_interval = 0.0f;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if (!((s is com.oddlabs.tt.model.Unit unit)) || unit.isDead() || unit.isMounted() || !unit.isWarrior())
                {
                    continue;
                }
                if (!unit.isMoving())
                {
                    continue;
                }
                float[] rec = ((float[])warrior_positions.get(unit));
                if (rec == null)
                {
                    warrior_positions.put(unit, new float[] { unit.getGridX(), unit.getGridY(), 0.0f });
                    continue;
                }
                float dx = (float)(unit.getGridX() - rec[0]);
                float dy = (float)(unit.getGridY() - rec[1]);
                if ((float)((float)(dx * dx) + (float)(dy * dy)) < 4.0f)
                {
                    rec[2] += elapsed;
                    if (rec[2] > com.oddlabs.tt.player.AdvancedAI.WARRIOR_STUCK_THRESHOLD)
                    {
                        com.oddlabs.tt.model.Building home = homeBuilding();
                        if (home != null)
                        {
                            if (unit.getGridX() == home.getGridX() && unit.getGridY() == home.getGridY())
                            {
                                rec[2] = 0.0f;
                                continue;
                            }
                            getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(unit), home.getGridX(), home.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                        }
                        rec[2] = 0.0f;
                    }
                }
                else
                {
                    rec[0] = unit.getGridX();
                    rec[1] = unit.getGridY();
                    rec[2] = 0.0f;
                }
            }
            warrior_positions.keySet().removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
        }
        // Java: 1028
        public void nodeCheckIdleChieftain(float t)
        {
            com.oddlabs.tt.model.Selectable[] idle_chieftains = getIdleChieftains();
            if (idle_chieftains != null && idle_chieftains.Length > 0)
            {
                chieftain_idle_time += t;
                if (chieftain_idle_time > com.oddlabs.tt.player.AdvancedAI.CHIEFTAIN_IDLE_RETURN_SECONDS)
                {
                    com.oddlabs.tt.model.Building home = homeBuilding();
                    if (home != null)
                    {
                        com.oddlabs.tt.model.Unit chief = ((com.oddlabs.tt.model.Unit)idle_chieftains[0]);
                        if (chief.getGridX() != home.getGridX() || chief.getGridY() != home.getGridY())
                        {
                            getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(chief), home.getGridX(), home.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                        }
                    }
                    chieftain_idle_time = 0.0f;
                }
            }
            else
            {
                chieftain_idle_time = 0.0f;
            }
        }
        // Java: 1053
        public void nodeCheckOutpost()
        {
            if (outpost_built)
            {
                return;
            }
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return;
            }
            int hx = home.getGridX();
            int hy = home.getGridY();
            if (isGatheringTooFar(getGatherTreePeons(), hx, hy) || isGatheringTooFar(getGatherRockPeons(), hx, hy) || isGatheringTooFar(getGatherIronPeons(), hx, hy) || isGatheringTooFar(getGatherRubberPeons(), hx, hy))
            {
                buildResourceOutpost(hx, hy);
            }
        }
        // Java: 1070
        public bool isGatheringTooFar(com.oddlabs.tt.model.Selectable[] gatherers, int hx, int hy)
        {
            if (gatherers == null)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Selectable s in gatherers)
            {
                int dx = s.getGridX() - hx;
                int dy = s.getGridY() - hy;
                if (dx * dx + dy * dy > com.oddlabs.tt.player.AdvancedAI.OUTPOST_TRIGGER_DIST * com.oddlabs.tt.player.AdvancedAI.OUTPOST_TRIGGER_DIST)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 1082
        public void buildResourceOutpost(int hx, int hy)
        {
            com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter filter = new com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter(typeof(com.oddlabs.tt.model.IronSupply), 200);
            getUnitGrid().scan(filter, hx, hy);
            int ox = (filter.found() ? filter.getX() : hx);
            int oy = (filter.found() ? filter.getY() : hy);
            com.oddlabs.tt.model.Selectable[] builders = getPeons(20);
            if (builders.Length < 20)
            {
                return;
            }
            bool quarters_ok = buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, firstN(builders, 10), ox, oy, 15);
            bool armory_ok = buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, lastN(builders, 10), ox, oy, 15);
            if (quarters_ok && armory_ok)
            {
                outpost_built = true;
                occupied_zones.add(new int[] { ox, oy });
            }
        }
        // Java: 1103
        public void nodeDefendBase()
        {
            java.util.List threats = new java.util.ArrayList();
            if (getQuarters() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
                {
                    if (q.isDead())
                    {
                        continue;
                    }
                    int score = scanForEnemies(((com.oddlabs.tt.model.Building)q), "quarters");
                    if (score > 0)
                    {
                        threats.add(new com.oddlabs.tt.player.AdvancedAI.Threat(defense_target, score, "quarters"));
                    }
                }
            }
            if (getArmory() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable a in getArmory())
                {
                    com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)a);
                    if (armory.isDead())
                    {
                        continue;
                    }
                    int score_2 = scanForEnemies(armory, "armory");
                    if (score_2 > 0)
                    {
                        threats.add(new com.oddlabs.tt.player.AdvancedAI.Threat(defense_target, score_2, "armory"));
                    }
                }
            }
            foreach (int[] zone in java.lang.JIter.of(occupied_zones))
            {
                int score_3 = scanForEnemiesAt(zone[0], zone[1], "outpost");
                if (score_3 > 0)
                {
                    threats.add(new com.oddlabs.tt.player.AdvancedAI.Threat(defense_target, score_3, "outpost"));
                }
            }
            if (getTowers() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable tower in getTowers())
                {
                    if (tower.isDead())
                    {
                        continue;
                    }
                    int score_4 = scanForEnemies(tower, "tower");
                    if (score_4 > 0)
                    {
                        threats.add(new com.oddlabs.tt.player.AdvancedAI.Threat(defense_target, score_4, "tower"));
                    }
                }
            }
            patrol_units.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            foreach (com.oddlabs.tt.model.Unit patrol_unit in java.lang.JIter.of(patrol_units))
            {
                int score_5 = scanForEnemies(patrol_unit, "occupied_zone");
                if (score_5 > 0)
                {
                    threats.add(new com.oddlabs.tt.player.AdvancedAI.Threat(defense_target, score_5, "occupied_zone"));
                }
            }
            if (threats.isEmpty())
            {
                return;
            }
            int total_defense_score = 0;
            if (getDefendingUnits() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable d in getDefendingUnits())
                {
                    total_defense_score += getUnitScore(((com.oddlabs.tt.model.Unit)d));
                }
            }
            java.util.List scaled = new java.util.ArrayList();
            foreach (com.oddlabs.tt.player.AdvancedAI.Threat t in java.lang.JIter.of(threats))
            {
                scaled.add(new com.oddlabs.tt.player.AdvancedAI.Threat(t.target, java.lang.JC.f2i(((float)((float)((float)(com.oddlabs.tt.player.AdvancedAI.DEFENSE_FACTOR[difficulty] * t.score) * vulnerabilityWeight(t.kind)) * rushDefenseMultiplier()))), t.kind));
            }
            emergency = getOwner().getWorld().isExtended();
            try
            {
                nodeDeployArmy();
            }
            finally
            {
                emergency = false;
            }
            nodeDefendMultiple(scaled, total_defense_score);
        }
        public bool emergency;
        public const float MAX_PEON_DEFENSE_FRACTION = 0.4f;
        // Java: 1191
        public void nodeDefendMultiple(java.util.List threats, int total_defense_score)
        {
            int total_score_needed = 0;
            foreach (com.oddlabs.tt.player.AdvancedAI.Threat t in java.lang.JIter.of(threats))
            {
                total_score_needed += t.score;
            }
            total_score_needed -= total_defense_score;
            if (total_score_needed <= 0)
            {
                return;
            }
            patrol_units.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            escort_units.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            java.util.List pool = new java.util.ArrayList();
            int result = 0;
            foreach (com.oddlabs.tt.model.Unit u in java.lang.JIter.of(new java.util.ArrayList(patrol_units)))
            {
                if (result >= total_score_needed)
                {
                    break;
                }
                if (!com.oddlabs.tt.player.AdvancedAI.canReceiveOrders(u))
                {
                    continue;
                }
                pool.add(u);
                result += getUnitScore(u);
                patrol_units.remove(u);
            }
            if (result < total_score_needed)
            {
                foreach (com.oddlabs.tt.model.Unit u_2 in java.lang.JIter.of(new java.util.ArrayList(escort_units)))
                {
                    if (result >= total_score_needed)
                    {
                        break;
                    }
                    if (!com.oddlabs.tt.player.AdvancedAI.canReceiveOrders(u_2))
                    {
                        continue;
                    }
                    pool.add(u_2);
                    result += getUnitScore(u_2);
                    escort_units.remove(u_2);
                }
            }
            if (result < total_score_needed && getIdleWarriors() != null)
            {
                result = addFromList(getIdleWarriors(), pool, result, total_score_needed);
            }
            int total_peons = countTotalPeons();
            int peon_cap = java.lang.JC.f2i(((float)(total_peons * standingGarrisonFraction())));
            int peons_drafted = countDefendingPeons();
            int remaining_peon_capacity = java.lang.Math.max(0, peon_cap - peons_drafted);
            if (result < total_score_needed && remaining_peon_capacity > 0 && getIdlePeons() != null)
            {
                int before = pool.size();
                result = addFromListCapped(getIdlePeons(), pool, result, total_score_needed, remaining_peon_capacity);
                peons_drafted += pool.size() - before;
                remaining_peon_capacity = java.lang.Math.max(0, peon_cap - peons_drafted);
            }
            if (result < total_score_needed && remaining_peon_capacity > 0 && getGatherTreePeons() != null)
            {
                int before_2 = pool.size();
                result = addFromListCapped(getGatherTreePeons(), pool, result, total_score_needed, remaining_peon_capacity);
                peons_drafted += pool.size() - before_2;
                remaining_peon_capacity = java.lang.Math.max(0, peon_cap - peons_drafted);
            }
            if (result < total_score_needed && remaining_peon_capacity > 0 && getGatherRockPeons() != null)
            {
                int before_3 = pool.size();
                result = addFromListCapped(getGatherRockPeons(), pool, result, total_score_needed, remaining_peon_capacity);
                peons_drafted += pool.size() - before_3;
                remaining_peon_capacity = java.lang.Math.max(0, peon_cap - peons_drafted);
            }
            if (result < total_score_needed && remaining_peon_capacity > 0 && getGatherIronPeons() != null)
            {
                int before_4 = pool.size();
                result = addFromListCapped(getGatherIronPeons(), pool, result, total_score_needed, remaining_peon_capacity);
                peons_drafted += pool.size() - before_4;
                remaining_peon_capacity = java.lang.Math.max(0, peon_cap - peons_drafted);
            }
            if (result < total_score_needed && remaining_peon_capacity > 0 && getGatherRubberPeons() != null)
            {
                addFromListCapped(getGatherRubberPeons(), pool, result, total_score_needed, remaining_peon_capacity);
            }
            if (pool.isEmpty())
            {
                return;
            }
            int pool_index = 0;
            foreach (com.oddlabs.tt.player.AdvancedAI.Threat t_2 in java.lang.JIter.of(threats))
            {
                if (pool_index >= pool.size())
                {
                    break;
                }
                double proportion = ((double)(t_2.score)) / total_score_needed;
                int alloc = java.lang.Math.max(1, unchecked((int)(java.lang.Math.round(proportion * pool.size()))));
                alloc = java.lang.Math.min(alloc, pool.size() - pool_index);
                java.util.List group = pool.subList(pool_index, pool_index + alloc);
                com.oddlabs.tt.model.Unit[] units = ((com.oddlabs.tt.model.Unit[])group.stream().filter(new java.util.function.Predicate_L((object a0) => { return com.oddlabs.tt.player.AdvancedAI.canReceiveOrders(((com.oddlabs.tt.model.Unit)a0)); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Unit[a0]; })));
                if (units.Length > 0)
                {
                    getOwner().setLandscapeTarget(units, t_2.target.getGridX(), t_2.target.getGridY(), com.oddlabs.tt.model.Action.DEFEND, true);
                }
                pool_index += alloc;
            }
        }
        // Java: 1294
        public int countTotalPeons()
        {
            int count = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead() && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                {
                    count++;
                }
            }
            return count;
        }
        // Java: 1310
        public int countDefendingPeons()
        {
            int count = 0;
            if (getDefendingUnits() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable s in getDefendingUnits())
                {
                    if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead() && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        // Java: 1327
        public int currentDefenseScore()
        {
            int score = 0;
            if (getDefendingUnits() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable s in getDefendingUnits())
                {
                    if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead())
                    {
                        score += getUnitScore(unit);
                    }
                }
            }
            return score;
        }
        // Java: 1342
        public int addFromListCapped(com.oddlabs.tt.model.Selectable[] list, java.util.List new_list, int progress, int score, int max_additional)
        {
            int result = progress;
            int added = 0;
            foreach (com.oddlabs.tt.model.Selectable list1 in list)
            {
                if (added >= max_additional)
                {
                    break;
                }
                com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)list1);
                if (!com.oddlabs.tt.player.AdvancedAI.canReceiveOrders(unit))
                {
                    continue;
                }
                new_list.add(unit);
                result += getUnitScore(unit);
                added++;
                if (result > score)
                {
                    break;
                }
            }
            return result;
        }
        // Java: 1361
        public void nodeDefendAllies()
        {
            int my_team = getOwner().getPlayerInfo().getTeam();
            if (my_team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return;
            }
            foreach (com.oddlabs.tt.player.Player ally in getOwner().getWorld().getPlayers())
            {
                if (((object)ally == (object)getOwner()) || ally.getPlayerInfo().getTeam() != my_team)
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ally.getUnits().getSet()))
                {
                    if (!((s is com.oddlabs.tt.model.Building building)) || building.isDead())
                    {
                        continue;
                    }
                    bool is_reproduce_or_armory = building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) || building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES);
                    bool is_tower = !is_reproduce_or_armory && (s.getPrimaryController() is com.oddlabs.tt.model.behaviour.NullController) && building.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK);
                    if (!is_reproduce_or_armory && !is_tower)
                    {
                        continue;
                    }
                    int score = scanForEnemies(building, null);
                    if (score > 0 && defense_target != null)
                    {
                        java.util.List threats = new java.util.ArrayList();
                        threats.add(new com.oddlabs.tt.player.AdvancedAI.Threat(defense_target, java.lang.JC.f2i(((float)(com.oddlabs.tt.player.AdvancedAI.DEFENSE_FACTOR[difficulty] * score))), null));
                        nodeDefendMultiple(threats, currentDefenseScore());
                        return;
                    }
                }
            }
        }
        public const float ALLY_STRUGGLING_RATIO = 0.3f;
        public const int ALLY_DONATION_MAX_UNITS = 30;
        public const int ALLY_DONATION_MIN_OWN_RESERVE = 15;
        // Java: 1419
        public void nodeDonateToAllies()
        {
            int my_team = getOwner().getPlayerInfo().getTeam();
            if (my_team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return;
            }
            float strongest_enemy = computeStrongestEnemyStrength();
            if (strongest_enemy <= 0.0f)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle_peons = getIdlePeons();
            if (idle_peons == null || idle_peons.Length <= com.oddlabs.tt.player.AdvancedAI.ALLY_DONATION_MIN_OWN_RESERVE)
            {
                return;
            }
            foreach (com.oddlabs.tt.player.Player ally in getOwner().getWorld().getPlayers())
            {
                if (((object)ally == (object)getOwner()) || ally.getPlayerInfo().getTeam() != my_team)
                {
                    continue;
                }
                if (computeArmyStrength(ally) >= (float)(strongest_enemy * com.oddlabs.tt.player.AdvancedAI.ALLY_STRUGGLING_RATIO))
                {
                    continue;
                }
                com.oddlabs.tt.model.Building recipient = null;
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ally.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && (b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) || b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES)))
                    {
                        recipient = b;
                        break;
                    }
                }
                if (recipient == null)
                {
                    continue;
                }
                int spare = idle_peons.Length - com.oddlabs.tt.player.AdvancedAI.ALLY_DONATION_MIN_OWN_RESERVE;
                int donate_count = java.lang.Math.min(com.oddlabs.tt.player.AdvancedAI.ALLY_DONATION_MAX_UNITS, spare);
                if (donate_count <= 0)
                {
                    continue;
                }
                getOwner().setTarget(firstN(idle_peons, donate_count), recipient, com.oddlabs.tt.model.Action.DEFAULT, false);
                return;
            }
        }
        public const int ALLY_TOWER_GARRISON_MIN_OWN_RESERVE = 15;
        // Java: 1476
        public void nodeGarrisonAllyTowers()
        {
            int my_team = getOwner().getPlayerInfo().getTeam();
            if (my_team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            if (idle_warriors == null || idle_warriors.Length <= com.oddlabs.tt.player.AdvancedAI.ALLY_TOWER_GARRISON_MIN_OWN_RESERVE)
            {
                return;
            }
            foreach (com.oddlabs.tt.player.Player ally in getOwner().getWorld().getPlayers())
            {
                if (((object)ally == (object)getOwner()) || ally.getPlayerInfo().getTeam() != my_team)
                {
                    continue;
                }
                com.oddlabs.tt.model.Building empty_tower = null;
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(ally.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Building b) && !b.isDead() && (s.getPrimaryController() is com.oddlabs.tt.model.behaviour.NullController) && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK) && !b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) && !b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) && !b.getUnitContainer().isSupplyFull())
                    {
                        empty_tower = b;
                        break;
                    }
                }
                if (empty_tower == null)
                {
                    continue;
                }
                getOwner().setTarget(firstN(idle_warriors, 1), empty_tower, com.oddlabs.tt.model.Action.DEFAULT, false);
                return;
            }
        }
        public const int BEACON_RESPONSE_WARRIORS = 15;
        public float[] pending_beacon;
        // Java: 1527
        public void onAllyBeacon(float x, float y)
        {
            pending_beacon = new float[] { x, y };
        }
        // Java: 1531
        public void nodeRespondToBeacon()
        {
            if (pending_beacon == null)
            {
                return;
            }
            float x = pending_beacon[0];
            float y = pending_beacon[1];
            pending_beacon = null;
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            if (idle_warriors == null || idle_warriors.Length == 0)
            {
                return;
            }
            int send_count = java.lang.Math.min(com.oddlabs.tt.player.AdvancedAI.BEACON_RESPONSE_WARRIORS, idle_warriors.Length);
            getOwner().setLandscapeTarget(firstN(idle_warriors, send_count), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(y), com.oddlabs.tt.model.Action.ATTACK, true);
        }
        public const int STRENGTH_CACHE_REFRESH_TICKS = 30;
        public static java.util.Map strength_cache;
        public static int last_cache_refresh_tick;
        public static com.oddlabs.tt.landscape.World last_cache_world;
        public static java.util.Map status_cache;
        // Java: 1598
        public void ensureCacheFresh()
        {
            com.oddlabs.tt.landscape.World world = getOwner().getWorld();
            int tick = world.getTick();
            if (((object)world != (object)com.oddlabs.tt.player.AdvancedAI.last_cache_world) || tick - com.oddlabs.tt.player.AdvancedAI.last_cache_refresh_tick >= com.oddlabs.tt.player.AdvancedAI.STRENGTH_CACHE_REFRESH_TICKS)
            {
                com.oddlabs.tt.player.AdvancedAI.strength_cache.clear();
                com.oddlabs.tt.player.AdvancedAI.status_cache.clear();
                com.oddlabs.tt.player.AdvancedAI.last_cache_refresh_tick = tick;
                com.oddlabs.tt.player.AdvancedAI.last_cache_world = world;
            }
        }
        // Java: 1609
        public int cachedPlayerStatus(com.oddlabs.tt.player.Player player)
        {
            ensureCacheFresh();
            int? cached = ((int?)com.oddlabs.tt.player.AdvancedAI.status_cache.get(player));
            if (cached != null)
            {
                return ((int)cached);
            }
            int status = player.getStatus();
            com.oddlabs.tt.player.AdvancedAI.status_cache.put(player, status);
            return status;
        }
        // Java: 1619
        public float computeArmyStrength(com.oddlabs.tt.player.Player player)
        {
            ensureCacheFresh();
            float? cached = ((float?)com.oddlabs.tt.player.AdvancedAI.strength_cache.get(player));
            if (cached != null)
            {
                return ((float)cached);
            }
            float total = 0.0f;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead() && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                {
                    total = (float)(total + getUnitScore(unit));
                }
            }
            com.oddlabs.tt.player.AdvancedAI.strength_cache.put(player, total);
            return total;
        }
        // Java: 1640
        public float computeStrongestEnemyStrength()
        {
            float strongest = 0.0f;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (getOwner().isEnemy(p))
                {
                    strongest = java.lang.Math.max(strongest, computeArmyStrength(p));
                }
            }
            return strongest;
        }
        public java.util.Set committed_this_tick;
        // Java: 1656
        public com.oddlabs.tt.model.Selectable[] claimIdleWarriors(int count)
        {
            com.oddlabs.tt.model.Selectable[] idle = getIdleWarriors();
            if (idle == null || count <= 0)
            {
                return new com.oddlabs.tt.model.Selectable[0];
            }
            int tick = getOwner().getWorld().getTick();
            java.util.List available = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in idle)
            {
                if (s.getHumanOverrideUntilTick() > tick)
                {
                    continue;
                }
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !committed_this_tick.contains(u))
                {
                    available.add(s);
                }
                if (available.size() == count)
                {
                    break;
                }
            }
            if (available.size() < count)
            {
                return new com.oddlabs.tt.model.Selectable[0];
            }
            foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(available))
            {
                committed_this_tick.add(((com.oddlabs.tt.model.Unit)s_2));
            }
            return ((com.oddlabs.tt.model.Selectable[])available.toArray(new com.oddlabs.tt.model.Selectable[0]));
        }
        public const int BEST_MOVE_SCAN_RADIUS = 140;
        public float best_move_cooldown;
        // Java: 1685
        public int nearbyDefenderCount(com.oddlabs.tt.model.Selectable candidate)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(candidate.getGridX(), candidate.getGridY(), 10, null, com.oddlabs.tt.model.Selectable.genericClass());
            getUnitGrid().scan(filter, candidate.getGridX(), candidate.getGridY());
            int count = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(filter.getResult()))
            {
                if (!s.isDead() && ((object)s.getOwner() == (object)candidate.getOwner()) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                {
                    count++;
                }
            }
            return count;
        }
        // Java: 1709
        public static int hitPointsOf(com.oddlabs.tt.model.Selectable s)
        {
            if ((s is com.oddlabs.tt.model.Unit u))
            {
                return u.getHitPoints();
            }
            if ((s is com.oddlabs.tt.model.Building b))
            {
                return b.getHitPoints();
            }
            return 0;
        }
        // Java: 1717
        public com.oddlabs.tt.util.Target findWeakestTarget(int start_x, int start_y)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(start_x, start_y, com.oddlabs.tt.player.AdvancedAI.BEST_MOVE_SCAN_RADIUS, null, com.oddlabs.tt.model.Selectable.genericClass());
            getUnitGrid().scan(filter, start_x, start_y);
            com.oddlabs.tt.util.Target weakest = null;
            float best_score = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(filter.getResult()))
            {
                if (s.isDead() || !getOwner().isEnemy(s.getOwner()))
                {
                    continue;
                }
                float score = (float)(com.oddlabs.tt.player.AdvancedAI.hitPointsOf(s) + ((float)(nearbyDefenderCount(s) * 50.0f)));
                if (score < best_score)
                {
                    best_score = score;
                    weakest = s;
                }
            }
            return weakest;
        }
        // Java: 1744
        public com.oddlabs.tt.player.Formation chooseTacticalFormation(int force_size)
        {
            if (force_size < 8)
            {
                return com.oddlabs.tt.player.Formation.TIGHT;
            }
            if (force_size >= NUM_WARRIORS[difficulty])
            {
                return com.oddlabs.tt.player.Formation.DIAMOND;
            }
            return com.oddlabs.tt.player.Formation.SQUARE;
        }
        // Java: 1752
        public void nodeBestMoveAttack(float t)
        {
            best_move_cooldown -= t;
            if (best_move_cooldown > 0.0f)
            {
                return;
            }
            best_move_cooldown = 8.0f;
            com.oddlabs.tt.model.Selectable[] force = claimIdleWarriors(java.lang.Math.max(3, NUM_WARRIORS[difficulty] / 3));
            if (force.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.util.Target weak = findWeakestTarget(force[0].getGridX(), force[0].getGridY());
            if (weak != null)
            {
                getOwner().setFormation(force, chooseTacticalFormation(force.Length));
                flankingAttack(force, weak.getGridX(), weak.getGridY());
            }
        }
        public float multi_front_cooldown;
        // Java: 1778
        public void flankingAttack(com.oddlabs.tt.model.Selectable[] force, int gx, int gy)
        {
            if (!getOwner().getWorld().isExtended() || force.Length < com.oddlabs.tt.player.AdvancedAI.FLANK_MIN_FORCE)
            {
                getOwner().setLandscapeTarget(force, gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
                return;
            }
            float cx = 0.0f;
            float cy = 0.0f;
            foreach (com.oddlabs.tt.model.Selectable s in force)
            {
                cx = (float)(cx + s.getGridX());
                cy = (float)(cy + s.getGridY());
            }
            cx = (float)(cx / force.Length);
            cy = (float)(cy / force.Length);
            float dx = (float)(gx - cx);
            float dy = (float)(gy - cy);
            float len = ((float)(java.lang.StrictMath.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < com.oddlabs.tt.player.AdvancedAI.FLANK_MIN_DISTANCE)
            {
                getOwner().setLandscapeTarget(force, gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
                return;
            }
            java.util.List flank = new java.util.ArrayList();
            java.util.List main = new java.util.ArrayList();
            int want = java.lang.Math.max(2, force.Length / 3);
            foreach (com.oddlabs.tt.model.Selectable s_2 in force)
            {
                if (flank.size() < want && (s_2 is com.oddlabs.tt.model.Unit u) && com.oddlabs.tt.model.Counters.role(u) == com.oddlabs.tt.model.Counters.FAST)
                {
                    flank.add(s_2);
                }
            }
            foreach (com.oddlabs.tt.model.Selectable s_3 in force)
            {
                if (flank.size() < want && !flank.contains(s_3) && (s_3 is com.oddlabs.tt.model.Unit u_2) && com.oddlabs.tt.model.Counters.role(u_2) == com.oddlabs.tt.model.Counters.LIGHT)
                {
                    flank.add(s_3);
                }
            }
            foreach (com.oddlabs.tt.model.Selectable s_4 in force)
            {
                if (!flank.contains(s_4))
                {
                    main.add(s_4);
                }
            }
            if (flank.size() < 2 || main.isEmpty())
            {
                getOwner().setLandscapeTarget(force, gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
                return;
            }
            float px = (float)((float)(-dy) / len);
            float py = (float)(dx / len);
            float @out = java.lang.Math.min(12.0f, (float)(len / 3.0f));
            com.oddlabs.tt.pathfinder.UnitGrid grid = getOwner().getWorld().getUnitGrid();
            int wx = (-1);
            int wy = (-1);
            foreach (int side in new int[] { ((gx + gy) % 2 == 0 ? 1 : (-1)), ((gx + gy) % 2 == 0 ? (-1) : 1) })
            {
                int x = java.lang.Math.round((float)((float)(gx - (float)((float)(dx / len) * 3.0f)) + (float)((float)(px * @out) * side)));
                int y = java.lang.Math.round((float)((float)(gy - (float)((float)(dy / len) * 3.0f)) + (float)((float)(py * @out) * side)));
                if (x > 2 && y > 2 && x < grid.getGridSize() - 3 && y < grid.getGridSize() - 3 && grid.getRegion(x, y) != null)
                {
                    wx = x;
                    wy = y;
                    break;
                }
            }
            if (wx < 0)
            {
                getOwner().setLandscapeTarget(force, gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
                return;
            }
            com.oddlabs.tt.model.Selectable[] flankers = ((com.oddlabs.tt.model.Selectable[])flank.toArray(new com.oddlabs.tt.model.Selectable[0]));
            com.oddlabs.tt.player.AdvancedAI.flank_orders++;
            getOwner().setLandscapeTarget(flankers, wx, wy, com.oddlabs.tt.model.Action.ATTACK, true);
            java.util.List fighters = new java.util.ArrayList();
            java.util.List throwers = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s_5 in java.lang.JIter.of(main))
            {
                if ((s_5 is com.oddlabs.tt.model.Unit u_3) && u_3.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
                {
                    throwers.add(s_5);
                }
                else
                {
                    fighters.add(s_5);
                }
            }
            if (fighters.isEmpty() || throwers.isEmpty())
            {
                getOwner().setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])main.toArray(new com.oddlabs.tt.model.Selectable[0])), gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
                return;
            }
            getOwner().setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])fighters.toArray(new com.oddlabs.tt.model.Selectable[0])), gx, gy, com.oddlabs.tt.model.Action.ATTACK, true);
            int bx = java.lang.Math.round((float)(gx - (float)((float)(dx / len) * 4.0f)));
            int by = java.lang.Math.round((float)(gy - (float)((float)(dy / len) * 4.0f)));
            com.oddlabs.tt.model.Selectable[] t = ((com.oddlabs.tt.model.Selectable[])throwers.toArray(new com.oddlabs.tt.model.Selectable[0]));
            getOwner().setLandscapeTarget(t, java.lang.Math.clamp(bx, 1, grid.getGridSize() - 2), java.lang.Math.clamp(by, 1, grid.getGridSize() - 2), com.oddlabs.tt.model.Action.ATTACK, true);
        }
        public const int FLANK_MIN_FORCE = 8;
        public static int flank_orders;
        public const float FLANK_MIN_DISTANCE = 24.0f;
        // Java: 1857
        public void nodeMultiFrontAttack(float t)
        {
            multi_front_cooldown -= t;
            if (multi_front_cooldown > 0.0f)
            {
                return;
            }
            multi_front_cooldown = 15.0f;
            int per_front = java.lang.Math.max(3, NUM_WARRIORS[difficulty] / 4);
            com.oddlabs.tt.model.Selectable[] front_a = claimIdleWarriors(per_front);
            if (front_a.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] front_b = claimIdleWarriors(per_front);
            if (front_b.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.player.Player focus = chooseFocusEnemy();
            if (focus == null)
            {
                return;
            }
            com.oddlabs.tt.util.Target target_a = findTarget(front_a[0].getGridX(), front_a[0].getGridY());
            com.oddlabs.tt.util.Target target_b = findWeakestTarget(front_b[0].getGridX(), front_b[0].getGridY());
            com.oddlabs.tt.player.Formation front_formation = chooseTacticalFormation(per_front);
            getOwner().setFormation(front_a, front_formation);
            getOwner().setFormation(front_b, front_formation);
            if (target_a != null)
            {
                flankingAttack(front_a, target_a.getGridX(), target_a.getGridY());
            }
            if (target_b != null)
            {
                flankingAttack(front_b, target_b.getGridX(), target_b.getGridY());
            }
        }
        public const float FAVORABLE_ATTACK_MARGIN = 1.3f;
        public float favorable_attack_cooldown;
        // Java: 1896
        public com.oddlabs.tt.player.Player findWeakestEnemyPlayer()
        {
            com.oddlabs.tt.player.Player weakest = null;
            float weakest_strength = java.lang.Float.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(p) || p.getUnits().getSet().isEmpty())
                {
                    continue;
                }
                float strength = computeArmyStrength(p);
                if (strength < weakest_strength)
                {
                    weakest_strength = strength;
                    weakest = p;
                }
            }
            return weakest;
        }
        // Java: 1911
        public void nodeFavorableAttack(float t)
        {
            favorable_attack_cooldown -= t;
            if (favorable_attack_cooldown > 0.0f)
            {
                return;
            }
            favorable_attack_cooldown = 20.0f;
            com.oddlabs.tt.player.Player weakest = findWeakestEnemyPlayer();
            if (weakest == null)
            {
                return;
            }
            if (computeArmyStrength(getOwner()) < (float)(computeArmyStrength(weakest) * com.oddlabs.tt.player.AdvancedAI.FAVORABLE_ATTACK_MARGIN))
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] force = claimIdleWarriors(java.lang.Math.max(3, NUM_WARRIORS[difficulty] / 3));
            if (force.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.util.Target target = findNearestOfPlayer(weakest, force[0].getGridX(), force[0].getGridY(), false);
            if (target == null)
            {
                target = findNearestOfPlayer(weakest, force[0].getGridX(), force[0].getGridY(), true);
            }
            if (target != null)
            {
                getOwner().setFormation(force, chooseTacticalFormation(force.Length));
                flankingAttack(force, target.getGridX(), target.getGridY());
            }
        }
        public float closest_player_attack_cooldown;
        public const float CLOSEST_PLAYER_ATTACK_COOLDOWN_SECONDS = 10.0f;
        // Java: 1941
        public com.oddlabs.tt.player.Player findClosestEnemyPlayer()
        {
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return null;
            }
            int hx = home.getGridX();
            int hy = home.getGridY();
            com.oddlabs.tt.player.Player closest = null;
            double closest_dist_sqr = java.lang.Double.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(p) || p.getUnits().getSet().isEmpty())
                {
                    continue;
                }
                int ex = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                int ey = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                double dx = ex - hx;
                double dy = ey - hy;
                double dist_sqr = dx * dx + dy * dy;
                if (dist_sqr < closest_dist_sqr)
                {
                    closest_dist_sqr = dist_sqr;
                    closest = p;
                }
            }
            return closest;
        }
        // Java: 1965
        public void nodeClosestPlayerAttack(float t)
        {
            closest_player_attack_cooldown -= t;
            if (closest_player_attack_cooldown > 0.0f)
            {
                return;
            }
            closest_player_attack_cooldown = com.oddlabs.tt.player.AdvancedAI.CLOSEST_PLAYER_ATTACK_COOLDOWN_SECONDS;
            com.oddlabs.tt.player.Player closest = findClosestEnemyPlayer();
            if (closest == null)
            {
                return;
            }
            int target_force = NUM_WARRIORS[difficulty];
            int min_force = java.lang.Math.max(3, target_force / 2);
            com.oddlabs.tt.model.Selectable[] warriors = claimIdleWarriors(min_force);
            if (warriors.Length == 0)
            {
                return;
            }
            bool use_chieftain = warriors.Length >= NUM_WARRIORS_FOR_CHIEFTAIN[difficulty] && getOwner().hasActiveChieftain();
            bool idle_chieftain = getIdleChieftains() != null && getIdleChieftains().Length >= 1;
            if (idle_chieftain && use_chieftain)
            {
                com.oddlabs.tt.model.Selectable[] with_chieftain = com.oddlabs.tt.model.Selectable.newArray(warriors.Length + 1);
                java.lang.JSystem.arraycopy(warriors, 0, with_chieftain, 0, warriors.Length);
                with_chieftain[warriors.Length] = getIdleChieftains()[0];
                warriors = with_chieftain;
            }
            com.oddlabs.tt.util.Target target = findNearestOfPlayer(closest, warriors[0].getGridX(), warriors[0].getGridY(), false);
            if (target == null)
            {
                target = findNearestOfPlayer(closest, warriors[0].getGridX(), warriors[0].getGridY(), true);
            }
            if (target != null)
            {
                getOwner().setFormation(warriors, com.oddlabs.tt.player.Formation.SQUARE);
                flankingAttack(warriors, target.getGridX(), target.getGridY());
            }
        }
        public const int STRANDED_SAFE_RADIUS_SQ = 1600;
        public float recall_stranded_cooldown;
        // Java: 2017
        public bool isNearSafeZone(int gx, int gy)
        {
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home != null)
            {
                int dx = home.getGridX() - gx;
                int dy = home.getGridY() - gy;
                if (dx * dx + dy * dy <= com.oddlabs.tt.player.AdvancedAI.STRANDED_SAFE_RADIUS_SQ)
                {
                    return true;
                }
            }
            foreach (int[] zone in java.lang.JIter.of(occupied_zones))
            {
                int dx_2 = zone[0] - gx;
                int dy_2 = zone[1] - gy;
                if (dx_2 * dx_2 + dy_2 * dy_2 <= com.oddlabs.tt.player.AdvancedAI.STRANDED_SAFE_RADIUS_SQ)
                {
                    return true;
                }
            }
            if (getTowers() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable tower in getTowers())
                {
                    if (tower.isDead())
                    {
                        continue;
                    }
                    int dx_3 = tower.getGridX() - gx;
                    int dy_3 = tower.getGridY() - gy;
                    if (dx_3 * dx_3 + dy_3 * dy_3 <= com.oddlabs.tt.player.AdvancedAI.STRANDED_SAFE_RADIUS_SQ)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 2043
        public void nodeRecallStrandedWarriors(float t)
        {
            recall_stranded_cooldown -= t;
            if (recall_stranded_cooldown > 0.0f)
            {
                return;
            }
            recall_stranded_cooldown = 2.0f;
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            if (idle_warriors == null || idle_warriors.Length == 0)
            {
                return;
            }
            java.util.List stranded = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable w in idle_warriors)
            {
                if (!isNearSafeZone(w.getGridX(), w.getGridY()))
                {
                    stranded.add(w);
                }
            }
            if (stranded.isEmpty())
            {
                return;
            }
            getOwner().setLandscapeTarget(((com.oddlabs.tt.model.Selectable[])stranded.toArray(new com.oddlabs.tt.model.Selectable[0])), home.getGridX(), home.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
        }
        public const int KOTH_GARRISON_SIZE = 15;
        public const int KOTI_GARRISON_PER_STATUE = 8;
        public float koth_check_cooldown;
        // Java: 2089
        public void nodePlayKingOfTheIslandObjective(float t)
        {
            koth_check_cooldown -= t;
            if (koth_check_cooldown > 0.0f)
            {
                return;
            }
            koth_check_cooldown = 8.0f;
            com.oddlabs.matchmaking.GameMode mode = getOwner().getWorld().getGameMode();
            if (((object)mode != (object)com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND) && ((object)mode != (object)com.oddlabs.matchmaking.GameMode.KING_OF_THE_HILL))
            {
                return;
            }
            int my_team = getOwner().getPlayerInfo().getTeam();
            if (my_team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return;
            }
            if (((object)mode == (object)com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND))
            {
                com.oddlabs.tt.gamemode.GameModeRules rules = com.oddlabs.tt.gamemode.GameModeRegistry.get(com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND);
                if (!((rules is com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules koti)))
                {
                    return;
                }
                if (koti.getWinningTeam() != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    return;
                }
                for (int i = 0; i < koti.getStatueCount(); i++)
                {
                    if (koti.getStatueControllingTeam(i) == my_team)
                    {
                        continue;
                    }
                    com.oddlabs.tt.model.Selectable[] force = claimIdleWarriors(com.oddlabs.tt.player.AdvancedAI.KOTI_GARRISON_PER_STATUE);
                    if (force.Length == 0)
                    {
                        break;
                    }
                    int grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(koti.getStatueX(i));
                    int grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(koti.getStatueY(i));
                    getOwner().setLandscapeTarget(force, grid_x, grid_y, com.oddlabs.tt.model.Action.ATTACK, true);
                }
            }
            else
            {
                com.oddlabs.tt.gamemode.GameModeRules rules_2 = com.oddlabs.tt.gamemode.GameModeRegistry.get(com.oddlabs.matchmaking.GameMode.KING_OF_THE_HILL);
                if (!((rules_2 is com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules koth)))
                {
                    return;
                }
                if (koth.getWinningTeam() != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
                {
                    return;
                }
                com.oddlabs.tt.model.Selectable[] force_2 = claimIdleWarriors(com.oddlabs.tt.player.AdvancedAI.KOTH_GARRISON_SIZE);
                if (force_2.Length == 0)
                {
                    return;
                }
                int grid_x_2 = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(koth.getCenterX());
                int grid_y_2 = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(koth.getCenterY());
                getOwner().setLandscapeTarget(force_2, grid_x_2, grid_y_2, com.oddlabs.tt.model.Action.ATTACK, true);
            }
        }
        public const int WAVES_PER_ALL_IN = 2;
        public int main_attack_wave_count;
        // Java: 2147
        public void nodeAllInAttack()
        {
            if (main_attack_wave_count < com.oddlabs.tt.player.AdvancedAI.WAVES_PER_ALL_IN)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle = getIdleWarriors();
            if (idle == null || idle.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] everyone = claimIdleWarriors(idle.Length);
            if (everyone.Length == 0)
            {
                return;
            }
            main_attack_wave_count = 0;
            com.oddlabs.tt.util.Target target = findTarget(everyone[0].getGridX(), everyone[0].getGridY());
            if (target != null)
            {
                getOwner().setFormation(everyone, chooseTacticalFormation(everyone.Length));
                flankingAttack(everyone, target.getGridX(), target.getGridY());
            }
        }
        public bool ally_outpost_built;
        // Java: 2167
        public com.oddlabs.tt.player.Player findAlly()
        {
            com.oddlabs.tt.player.Player owner = getOwner();
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (((object)p != (object)owner) && !owner.isEnemy(p) && p.getPlayerInfo().getTeam() != com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL && p.getPlayerInfo().getTeam() == owner.getPlayerInfo().getTeam())
                {
                    return p;
                }
            }
            return null;
        }
        // Java: 2178
        public void nodeCheckAllyOutpost()
        {
            if (ally_outpost_built)
            {
                return;
            }
            com.oddlabs.tt.player.Player ally = findAlly();
            if (ally == null)
            {
                return;
            }
            if (getArmory() == null || getQuarters() == null)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] builders = getPeons(com.oddlabs.tt.player.AdvancedAI.BUILDERS_PER_TOWER_TARGET);
            if (builders.Length < com.oddlabs.tt.player.AdvancedAI.BUILDERS_PER_TOWER_TARGET)
            {
                return;
            }
            int ax = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(ally.getStartX());
            int ay = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(ally.getStartY());
            if (buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, builders, ax, ay, 30))
            {
                ally_outpost_built = true;
            }
        }
        public const float ADAPTIVE_CHECK_INTERVAL_SECONDS = 10.0f;
        public const float ADAPTIVE_REBALANCE_RATIO = 1.6f;
        public float time_since_adaptive_check;
        public int armory_deploy_rotation;
        public float match_elapsed_seconds;
        public const int TIER_EXPERT = 4;
        public const int TIER_GAUNTLET = 5;
        public static float[] ADAPT_INTERVAL;
        public static float[] ADAPT_UP;
        public const float ADAPT_DOWN = 1.6f;
        public const float ADAPT_DOWN_BRAIN = 2.2f;
        public static float[] ADAPT_DWELL;
        public static int[] ADAPT_CONFIRM;
        public int behind_count;
        public int tier;
        public int top_tier;
        public float tier_time;
        public com.oddlabs.tt.player.AI expert_brain;
        public com.oddlabs.tt.player.AI gauntlet_brain;
        // Java: 2243
        public int getTier()
        {
            return tier;
        }
        // Java: 2248
        public int getTopTier()
        {
            return top_tier;
        }
        // Java: 2253
        public int getStartDifficulty()
        {
            return start_difficulty;
        }
        // Java: 2258
        public void startAtTier(int n)
        {
            setTier(java.lang.Math.clamp(n, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_EASY, com.oddlabs.tt.player.AdvancedAI.TIER_GAUNTLET));
        }
        public java.lang.StringBuilder tier_trail;
        // Java: 2265
        public string getTierTrail()
        {
            return java.lang.JO.toString(tier_trail).trim();
        }
        // Java: 2269
        public void setTier(int n)
        {
            tier_trail.append(' ').append(n).append('@').append(java.lang.JString.format("%.1f", (float)(match_elapsed_seconds / 60.0f)));
            tier = n;
            tier_time = 0.0f;
            top_tier = java.lang.Math.max(top_tier, n);
            if (n <= com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE)
            {
                difficulty = n;
                relayTo(null, match_elapsed_seconds);
            }
            else if (n == com.oddlabs.tt.player.AdvancedAI.TIER_EXPERT)
            {
                if (expert_brain == null)
                {
                    expert_brain = ((com.oddlabs.tt.player.ai.ExpertAI)com.oddlabs.tt.player.AI.makeBrain(new java.util.function.Supplier_L(() =>
                    {
                        return new com.oddlabs.tt.player.ai.ExpertAI(getOwner(), null);
                    })));
                }
                relayTo(expert_brain, match_elapsed_seconds);
            }
            else
            {
                if (gauntlet_brain == null)
                {
                    gauntlet_brain = ((com.oddlabs.tt.player.gauntlet.GauntletAI)com.oddlabs.tt.player.AI.makeBrain(new java.util.function.Supplier_L(() =>
                    {
                        return new com.oddlabs.tt.player.gauntlet.GauntletAI(getOwner(), null, "");
                    })));
                }
                relayTo(gauntlet_brain, match_elapsed_seconds);
            }
            com.oddlabs.tt.animation.AnimationManager manager = getOwner().getWorld().getAnimationManagerGameTime();
            if (n > com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE && getOwner().getWorld().isExtended())
            {
                bool g = n == com.oddlabs.tt.player.AdvancedAI.TIER_GAUNTLET;
                if (tactics == null || tactics_gauntlet != g)
                {
                    if (tactics != null)
                    {
                        manager.removeAnimation(tactics);
                    }
                    tactics = new com.oddlabs.tt.player.ExtendedTactics(getOwner(), relay(), g);
                    tactics.lateStart(match_elapsed_seconds);
                    tactics_gauntlet = g;
                }
                else
                {
                    manager.registerAnimation(tactics);
                }
            }
            else if (tactics != null)
            {
                manager.removeAnimation(tactics);
            }
        }
        public com.oddlabs.tt.player.ExtendedTactics tactics;
        public bool tactics_gauntlet;
        // Java: 2307
        public void nodeAdaptiveDifficulty(float t)
        {
            if (!adaptive)
            {
                return;
            }
            time_since_adaptive_check += t;
            tier_time += t;
            int sd = java.lang.Math.clamp(start_difficulty, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_EASY, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE);
            if (time_since_adaptive_check < com.oddlabs.tt.player.AdvancedAI.ADAPT_INTERVAL[sd])
            {
                return;
            }
            time_since_adaptive_check = 0.0f;
            float own = com.oddlabs.tt.player.AI.tribePower(getOwner());
            float enemy = com.oddlabs.tt.player.AI.strongestEnemyPower(getOwner());
            if (own <= 0.0f && enemy <= 0.0f)
            {
                return;
            }
            float up = com.oddlabs.tt.player.AdvancedAI.ADAPT_UP[sd];
            behind_count = (enemy > (float)(own * up) ? behind_count + 1 : 0);
            if (behind_count >= com.oddlabs.tt.player.AdvancedAI.ADAPT_CONFIRM[sd] && tier < com.oddlabs.tt.player.AdvancedAI.TIER_GAUNTLET)
            {
                behind_count = 0;
                int next = java.lang.Math.min(com.oddlabs.tt.player.AdvancedAI.TIER_GAUNTLET, tier + ((enemy > (float)((float)(own * up) * up) ? 2 : 1)));
                if (next == com.oddlabs.tt.player.AdvancedAI.TIER_EXPERT && getOwner().getWorld().isExtended())
                {
                    next = com.oddlabs.tt.player.AdvancedAI.TIER_GAUNTLET;
                }
                setTier(next);
            }
            else if (own > (float)((float)(enemy * up) * ((tier > com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE ? com.oddlabs.tt.player.AdvancedAI.ADAPT_DOWN_BRAIN : com.oddlabs.tt.player.AdvancedAI.ADAPT_DOWN))) && tier_time >= com.oddlabs.tt.player.AdvancedAI.ADAPT_DWELL[sd])
            {
                int next_2 = tier - 1;
                if (next_2 == com.oddlabs.tt.player.AdvancedAI.TIER_EXPERT && getOwner().getWorld().isExtended())
                {
                    next_2 = com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE;
                }
                if (next_2 >= java.lang.Math.max(com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_EASY, top_tier - 1))
                {
                    setTier(next_2);
                }
            }
        }
        // Java: 2349
        public void nodeUpdatePlayerStyle(float t)
        {
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (getOwner().isEnemy(p) && p.getAI() == null)
                {
                    com.oddlabs.tt.player.PlayerStyleProfile.get().update(p, t);
                    return;
                }
            }
        }
        // Java: 2365
        public float getStyleAdjustedStallSeconds()
        {
            float aggression = com.oddlabs.tt.player.PlayerStyleProfile.get().getAggression();
            float multiplier = (float)(1.5f - aggression);
            if (selected_behavior != null)
            {
                multiplier = (float)(multiplier * selected_behavior.getStallMultiplier());
            }
            return (float)(com.oddlabs.tt.player.AdvancedAI.ATTACK_STALL_SECONDS * multiplier);
        }
        // Java: 2385
        public int getEffectiveMinWarriorsForStallAttack()
        {
            if (selected_behavior == null)
            {
                return com.oddlabs.tt.player.AdvancedAI.MIN_WARRIORS_FOR_STALL_ATTACK;
            }
            return java.lang.Math.max(1, java.lang.Math.round((float)(com.oddlabs.tt.player.AdvancedAI.MIN_WARRIORS_FOR_STALL_ATTACK * selected_behavior.getMinWarriorsMultiplier())));
        }
        // Java: 2396
        public void recordBanditOutcome(bool ai_won)
        {
            recordBanditOutcome((ai_won ? 1.0f : 0.0f));
        }
        // Java: 2401
        public void recordBanditOutcome(float reward)
        {
            if (selected_context != null && selected_behavior != null)
            {
                com.oddlabs.tt.player.AiBehaviorBandit.get().recordOutcome(selected_context, start_difficulty, selected_behavior, reward);
            }
        }
        // Java: 2407
        public int countWarriors()
        {
            int count = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit unit) && !unit.isDead() && unit.isWarrior())
                {
                    count++;
                }
            }
            return count;
        }
        // Java: 2416
        public void nodeDeployArmy()
        {
            if (getArmory() == null)
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Selectable a in getArmory())
            {
                com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)a);
                if (armory.isDead())
                {
                    continue;
                }
                int num_units = armory.getUnitContainer().getNumSupplies();
                int num_weapons = numWeapons(armory) - ((emergency ? 0 : com.oddlabs.tt.player.AdvancedAI.MIN_WEAPONS_IN_STOCK[difficulty]));
                if (num_units <= 0)
                {
                    continue;
                }
                int num_warriors = 0;
                if (num_weapons > 0)
                {
                    num_warriors = java.lang.Math.min(num_units, num_weapons);
                }
                int num_rubber_units = java.lang.Math.min(num_warriors, armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies());
                int num_iron_units = java.lang.Math.min(num_warriors - num_rubber_units, armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies());
                int num_rock_units = java.lang.Math.min(num_warriors - num_rubber_units - num_iron_units, armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies());
                if (num_rubber_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, num_rubber_units);
                }
                if (num_iron_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, num_iron_units);
                }
                if (num_rock_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, num_rock_units);
                }
                int remaining_units = num_units - num_warriors;
                if (remaining_units > 0)
                {
                    nodeGather(armory, remaining_units);
                }
            }
        }
        // Java: 2462
        public class ClusterResult : java.lang.Object
        {
            public com.oddlabs.tt.model.Unit unit;
            public float score;
            public ClusterResult(com.oddlabs.tt.model.Unit unit, float score) : base()
            {
                this.unit = unit;
                this.score = score;
            }
        }
        // Java: 2468
        public static long cellKey(int cx, int cy)
        {
            return (((long)(cx)) << 32) ^ (cy & 4294967295L);
        }
        // Java: 2480
        public com.oddlabs.tt.player.AdvancedAI.ClusterResult findBestClusterTarget(com.oddlabs.tt.player.Player p)
        {
            int CELL_SIZE = 20;
            java.util.Map buckets = new java.util.LinkedHashMap();
            java.util.List alive_units = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead())
                {
                    alive_units.add(u);
                    long key = com.oddlabs.tt.player.AdvancedAI.cellKey(u.getGridX() / CELL_SIZE, u.getGridY() / CELL_SIZE);
                    ((java.util.List)buckets.computeIfAbsent(key, new java.util.function.Function_L((object k_l) =>
                    {
                        long? k = (long?)k_l;
                        return new java.util.ArrayList();
                    }))).add(u);
                }
            }
            com.oddlabs.tt.model.Unit best = null;
            float best_score = 0.0f;
            foreach (com.oddlabs.tt.model.Unit candidate in java.lang.JIter.of(alive_units))
            {
                int cx = candidate.getGridX() / CELL_SIZE;
                int cy = candidate.getGridY() / CELL_SIZE;
                float score = 0.0f;
                for (int dx = (-1); dx <= 1; dx++)
                {
                    for (int dy = (-1); dy <= 1; dy++)
                    {
                        java.util.List bucket = ((java.util.List)buckets.get(com.oddlabs.tt.player.AdvancedAI.cellKey(cx + dx, cy + dy)));
                        if (bucket == null)
                        {
                            continue;
                        }
                        foreach (com.oddlabs.tt.model.Unit other in java.lang.JIter.of(bucket))
                        {
                            int ddx = other.getGridX() - candidate.getGridX();
                            int ddy = other.getGridY() - candidate.getGridY();
                            if (ddx * ddx + ddy * ddy < 400)
                            {
                                score = (float)(score + getUnitScore(other));
                            }
                        }
                    }
                }
                if (score > best_score)
                {
                    best_score = score;
                    best = candidate;
                }
            }
            return (best != null ? new com.oddlabs.tt.player.AdvancedAI.ClusterResult(best, best_score) : (com.oddlabs.tt.player.AdvancedAI.ClusterResult)(null));
        }
        // Java: 2524
        public com.oddlabs.tt.model.Unit getUsableChieftain()
        {
            com.oddlabs.tt.model.Unit primary = getOwner().getChieftain();
            if (primary != null && !primary.isDead())
            {
                return primary;
            }
            foreach (com.oddlabs.tt.model.Unit extra in java.lang.JIter.of(getOwner().getExtraChieftains()))
            {
                if (!extra.isDead())
                {
                    return extra;
                }
            }
            return null;
        }
        // Java: 2535
        public void nodeChiefHitAndRun()
        {
            com.oddlabs.tt.model.Selectable[] idle_chiefs = getIdleChieftains();
            if (idle_chiefs == null)
            {
                return;
            }
            java.util.List ready = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in idle_chiefs)
            {
                if (!((s is com.oddlabs.tt.model.Unit chief)) || chief.isMounted() || chief_hitrun_units.contains(chief))
                {
                    continue;
                }
                bool fully_healed = chief.getHitPoints() >= ((com.oddlabs.tt.model.UnitTemplate)chief.getTemplate()).getMaxHitPoints();
                bool opportunistic = chief.getHitPoints() > (float)(com.oddlabs.tt.player.AdvancedAI.CHIEF_HITRUN_HEALTH_PCT * ((com.oddlabs.tt.model.UnitTemplate)chief.getTemplate()).getMaxHitPoints());
                if (fully_healed || opportunistic)
                {
                    ready.add(chief);
                }
            }
            if (ready.isEmpty())
            {
                return;
            }
            if (ready.size() > com.oddlabs.tt.player.AdvancedAI.GROUP_HITRUN_THRESHOLD)
            {
                launchHitAndRun(ready);
            }
            else
            {
                foreach (com.oddlabs.tt.model.Unit chief_2 in java.lang.JIter.of(ready))
                {
                    launchHitAndRun(java.util.List_.of(chief_2));
                }
            }
        }
        // Java: 2584
        public void launchHitAndRun(java.util.List raiders)
        {
            com.oddlabs.tt.model.Unit anchor = ((com.oddlabs.tt.model.Unit)raiders.get(0));
            int nearby_own = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !raiders.contains(u))
                {
                    int dx = u.getGridX() - anchor.getGridX();
                    int dy = u.getGridY() - anchor.getGridY();
                    if (dx * dx + dy * dy < (float)(com.oddlabs.tt.player.AdvancedAI.CHIEF_HITRUN_SEARCH_RADIUS * com.oddlabs.tt.player.AdvancedAI.CHIEF_HITRUN_SEARCH_RADIUS))
                    {
                        nearby_own++;
                    }
                }
            }
            if (nearby_own >= com.oddlabs.tt.player.AdvancedAI.CHIEF_HITRUN_MAX_ESCORT)
            {
                return;
            }
            com.oddlabs.tt.model.Unit best_target = null;
            float best_score = 0.0f;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(p))
                {
                    continue;
                }
                com.oddlabs.tt.player.AdvancedAI.ClusterResult result = findBestClusterTarget(p);
                if (result != null && result.score > best_score)
                {
                    best_score = result.score;
                    best_target = result.unit;
                }
            }
            if (best_target == null)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            java.util.List raid_party = new java.util.ArrayList();
            raid_party.addAll(raiders);
            if (idle_warriors != null && idle_warriors.Length > 0)
            {
                int escort_count = getOwner().getWorld().getRandom().nextInt(idle_warriors.Length + 1);
                for (int i = 0; i < escort_count; i++)
                {
                    raid_party.add(idle_warriors[i]);
                }
            }
            com.oddlabs.tt.model.Selectable[] raid_array = ((com.oddlabs.tt.model.Selectable[])raid_party.toArray(com.oddlabs.tt.model.Selectable.newArray(raid_party.size())));
            chief_hitrun_units.addAll(raiders);
            getOwner().setLandscapeTarget(raid_array, best_target.getGridX(), best_target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
            foreach (com.oddlabs.tt.model.Unit chief in java.lang.JIter.of(raiders))
            {
                for (int i_2 = 0; i_2 < com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT; i_2++)
                {
                    if (chief.canDoMagic(i_2))
                    {
                        chief.doMagic(i_2, false);
                        break;
                    }
                }
            }
        }
        // Java: 2649
        public com.oddlabs.tt.model.Building findNearestEnemyTower(int grid_x, int grid_y)
        {
            float world_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float world_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(world_x, world_y, com.oddlabs.tt.player.AdvancedAI.ENEMY_TOWER_DEFENSE_RADIUS, null, typeof(com.oddlabs.tt.model.Building));
            getUnitGrid().scan(filter, grid_x, grid_y);
            com.oddlabs.tt.model.Building nearest = null;
            int nearest_dist_sqr = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Building building in java.lang.JIter.of(filter.getResult()))
            {
                if (building.isDead() || !isTowerTemplate(building) || !getOwner().isEnemy(building.getOwner()))
                {
                    continue;
                }
                int dx = building.getGridX() - grid_x;
                int dy = building.getGridY() - grid_y;
                int dist_sqr = dx * dx + dy * dy;
                if (dist_sqr < nearest_dist_sqr)
                {
                    nearest_dist_sqr = dist_sqr;
                    nearest = building;
                }
            }
            return nearest;
        }
        // Java: 2671
        public void nodeChiefHitAndRunReturn()
        {
            if (chief_hitrun_units.isEmpty())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Unit chief in java.lang.JIter.of(new java.util.ArrayList(chief_hitrun_units)))
            {
                if (chief.isDead())
                {
                    chief_hitrun_units.remove(chief);
                    continue;
                }
                if (chief.getHitPoints() <= (float)(0.4f * ((com.oddlabs.tt.model.UnitTemplate)chief.getTemplate()).getMaxHitPoints()) || (getIdleChieftains() != null && java.util.Arrays.asList(getIdleChieftains()).contains(chief)))
                {
                    com.oddlabs.tt.model.Building home = homeBuilding();
                    if (home != null)
                    {
                        if (chief.getGridX() != home.getGridX() || chief.getGridY() != home.getGridY())
                        {
                            com.oddlabs.tt.model.Building nearby_tower = findNearestEnemyTower(chief.getGridX(), chief.getGridY());
                            if (nearby_tower != null)
                            {
                                int away_dx = chief.getGridX() - nearby_tower.getGridX();
                                int away_dy = chief.getGridY() - nearby_tower.getGridY();
                                double len = java.lang.Math.sqrt(away_dx * away_dx + away_dy * away_dy);
                                if (len < 0.001d)
                                {
                                    away_dx = 1;
                                    len = 1;
                                }
                                int step_x = chief.getGridX() + java.lang.JC.d2i((away_dx / len * 20));
                                int step_y = chief.getGridY() + java.lang.JC.d2i((away_dy / len * 20));
                                getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(chief), step_x, step_y, com.oddlabs.tt.model.Action.MOVE, false);
                                getOwner().queueLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(chief), home.getGridX(), home.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                            }
                            else
                            {
                                getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(chief), home.getGridX(), home.getGridY(), com.oddlabs.tt.model.Action.MOVE, false);
                            }
                        }
                    }
                    chief_hitrun_units.remove(chief);
                }
            }
        }
        // Java: 2728
        public static bool canReceiveOrders(com.oddlabs.tt.model.Unit unit)
        {
            return !unit.isDead() && !unit.isMounted();
        }
        // Java: 2732
        public int addFromList(com.oddlabs.tt.model.Selectable[] list, java.util.List new_list, int progress, int score)
        {
            int result = progress;
            foreach (com.oddlabs.tt.model.Selectable list1 in list)
            {
                com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)list1);
                if (!com.oddlabs.tt.player.AdvancedAI.canReceiveOrders(unit))
                {
                    continue;
                }
                new_list.add(unit);
                result += getUnitScore(unit);
                if (result > score)
                {
                    break;
                }
            }
            return result;
        }
        // Java: 2746
        public int scanForEnemies(com.oddlabs.tt.model.Selectable src, string kind)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(src.getPositionX(), src.getPositionY(), com.oddlabs.tt.player.AdvancedAI.DEFENSE_SCAN_RADIUS, src, typeof(com.oddlabs.tt.model.Unit));
            getUnitGrid().scan(filter, src.getGridX(), src.getGridY());
            int score = 0;
            int human_unit_count = 0;
            int human_score = 0;
            com.oddlabs.tt.player.Player human_attacker = null;
            defense_target = null;
            foreach (com.oddlabs.tt.model.Unit unit in java.lang.JIter.of(filter.getResult()))
            {
                if (!unit.isDead() && getOwner().isEnemy(unit.getOwner()))
                {
                    int unit_score = getUnitScore(unit);
                    score += unit_score;
                    if (unit.getOwner().getAI() == null)
                    {
                        human_attacker = unit.getOwner();
                        human_unit_count++;
                        human_score += unit_score;
                    }
                    if (defense_target == null)
                    {
                        defense_target = new com.oddlabs.tt.landscape.LandscapeTarget(unit.getGridX(), unit.getGridY());
                    }
                }
            }
            recordPlayerAttackObservation(kind, human_attacker, human_unit_count, human_score);
            return score;
        }
        // Java: 2772
        public int scanForEnemiesAt(int grid_x, int grid_y, string kind)
        {
            float px = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float py = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(px, py, com.oddlabs.tt.player.AdvancedAI.DEFENSE_SCAN_RADIUS, null, typeof(com.oddlabs.tt.model.Unit));
            getUnitGrid().scan(filter, grid_x, grid_y);
            int score = 0;
            int human_unit_count = 0;
            int human_score = 0;
            com.oddlabs.tt.player.Player human_attacker = null;
            defense_target = null;
            foreach (com.oddlabs.tt.model.Unit unit in java.lang.JIter.of(filter.getResult()))
            {
                if (!unit.isDead() && getOwner().isEnemy(unit.getOwner()))
                {
                    int unit_score = getUnitScore(unit);
                    score += unit_score;
                    if (unit.getOwner().getAI() == null)
                    {
                        human_attacker = unit.getOwner();
                        human_unit_count++;
                        human_score += unit_score;
                    }
                    if (defense_target == null)
                    {
                        defense_target = new com.oddlabs.tt.landscape.LandscapeTarget(unit.getGridX(), unit.getGridY());
                    }
                }
            }
            recordPlayerAttackObservation(kind, human_attacker, human_unit_count, human_score);
            return score;
        }
        public const float ATTACK_WAVE_COOLDOWN_SECONDS = 15.0f;
        public bool first_attack_recorded;
        public float time_since_last_recorded_wave;
        public com.oddlabs.tt.player.Player observed_human_opponent;
        public string cached_map_size_label;
        // Java: 2831
        public void recordPlayerAttackObservation(string kind, com.oddlabs.tt.player.Player human_attacker, int force_size, int force_score)
        {
            if (!adaptive || kind == null || human_attacker == null)
            {
                return;
            }
            if (observed_human_opponent == null)
            {
                observed_human_opponent = human_attacker;
            }
            string key = currentSegmentKey();
            if (!first_attack_recorded)
            {
                first_attack_recorded = true;
                com.oddlabs.tt.player.PlayerBehaviorProfile.get().recordFirstAttackTime(key, match_elapsed_seconds);
            }
            if (time_since_last_recorded_wave < com.oddlabs.tt.player.AdvancedAI.ATTACK_WAVE_COOLDOWN_SECONDS)
            {
                return;
            }
            time_since_last_recorded_wave = 0.0f;
            com.oddlabs.tt.player.PlayerBehaviorProfile.get().recordAttackWave(key, kind, force_size, force_score);
        }
        // Java: 2853
        public string opponentRaceLabel()
        {
            if (observed_human_opponent == null)
            {
                return "unknown";
            }
            return ((observed_human_opponent.getPlayerInfo().getRace()) switch { 0 => (string)("natives"), 1 => (string)("vikings"), _ => (string)("unknown") });
        }
        // Java: 2867
        public string mapSizeLabel()
        {
            if (cached_map_size_label == null)
            {
                int size = getUnitGrid().getGridSize();
                cached_map_size_label = (size < 192 ? "small" : (size < 384 ? "medium" : "large"));
            }
            return cached_map_size_label;
        }
        // Java: 2881
        public string difficultyLabel()
        {
            return ((difficulty) switch { 0 => (string)("easy"), 2 => (string)("hard"), 3 => (string)("insane"), _ => (string)("normal") });
        }
        // Java: 2890
        public string currentSegmentKey()
        {
            return com.oddlabs.tt.player.PlayerBehaviorProfile.segmentKey(opponentRaceLabel(), mapSizeLabel(), difficultyLabel());
        }
        // Java: 2898
        public void finalizeAdaptiveLearning()
        {
            if (!adaptive || observed_human_opponent == null)
            {
                return;
            }
            com.oddlabs.tt.player.PlayerBehaviorProfile.get().recordMatchExposure(currentSegmentKey(), (float)(match_elapsed_seconds / 60.0f));
        }
        // Java: 2908
        public float vulnerabilityWeight(string kind)
        {
            if (!adaptive || kind == null)
            {
                return 1.0f;
            }
            string key = currentSegmentKey();
            return (float)(1.0f + ((kind) switch { "quarters" => (float)(com.oddlabs.tt.player.PlayerBehaviorProfile.get().getQuartersHitShare(key)), "armory" => (float)(com.oddlabs.tt.player.PlayerBehaviorProfile.get().getArmoryHitShare(key)), "outpost" => (float)(com.oddlabs.tt.player.PlayerBehaviorProfile.get().getOutpostHitShare(key)), _ => (float)(0.0f) }));
        }
        public const float EARLY_GAME_WINDOW_SECONDS = 240.0f;
        public const float EARLY_RUSH_DEFENSE_BOOST = 1.5f;
        // Java: 2927
        public float rushDefenseMultiplier()
        {
            if (!adaptive || match_elapsed_seconds >= com.oddlabs.tt.player.AdvancedAI.EARLY_GAME_WINDOW_SECONDS)
            {
                return 1.0f;
            }
            return (com.oddlabs.tt.player.PlayerBehaviorProfile.get().isEarlyRusher(currentSegmentKey()) ? com.oddlabs.tt.player.AdvancedAI.EARLY_RUSH_DEFENSE_BOOST : 1.0f);
        }
        // Java: 2938
        public float attackForceMultiplier()
        {
            if (!adaptive)
            {
                return 1.0f;
            }
            return com.oddlabs.tt.player.PlayerBehaviorProfile.get().getForceSizeMultiplier(currentSegmentKey(), NUM_WARRIORS[difficulty]);
        }
        public const float MAX_STANDING_GARRISON_FRACTION = 0.6f;
        public const float HIGH_FREQUENCY_ATTACKS_PER_MINUTE = 1.0f;
        // Java: 2956
        public float standingGarrisonFraction()
        {
            if (!adaptive)
            {
                return com.oddlabs.tt.player.AdvancedAI.MAX_PEON_DEFENSE_FRACTION;
            }
            float attacks_per_minute = com.oddlabs.tt.player.PlayerBehaviorProfile.get().getAttacksPerMinute(currentSegmentKey());
            float t = java.lang.Math.clamp((float)(attacks_per_minute / com.oddlabs.tt.player.AdvancedAI.HIGH_FREQUENCY_ATTACKS_PER_MINUTE), 0.0f, 1.0f);
            return (float)(com.oddlabs.tt.player.AdvancedAI.MAX_PEON_DEFENSE_FRACTION + (float)(t * ((float)(com.oddlabs.tt.player.AdvancedAI.MAX_STANDING_GARRISON_FRACTION - com.oddlabs.tt.player.AdvancedAI.MAX_PEON_DEFENSE_FRACTION))));
        }
        // Java: 2964
        public com.oddlabs.tt.model.Unit findBestIdleWarrior(com.oddlabs.tt.model.Selectable[] idle_warriors, bool[] used)
        {
            int tick = getOwner().getWorld().getTick();
            int best_index = (-1);
            int best_score = (-1);
            for (int i = 0; i < idle_warriors.Length; i++)
            {
                if (used[i])
                {
                    continue;
                }
                if (idle_warriors[i].getHumanOverrideUntilTick() > tick)
                {
                    continue;
                }
                com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)idle_warriors[i]);
                int score = getUnitScore(unit);
                if (score > best_score)
                {
                    best_score = score;
                    best_index = i;
                }
            }
            if (best_index == (-1))
            {
                return null;
            }
            used[best_index] = true;
            return ((com.oddlabs.tt.model.Unit)idle_warriors[best_index]);
        }
        // Java: 2989
        public int getUnitScore(com.oddlabs.tt.model.Unit unit)
        {
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
            {
                return com.oddlabs.tt.player.AdvancedAI.SCORE_PEON;
            }
            else if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
            {
                return com.oddlabs.tt.player.AdvancedAI.SCORE_CHIEFTAIN;
            }
            else if (((object)unit.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)unit.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
            {
                return com.oddlabs.tt.player.AdvancedAI.SCORE_WARRIOR_ROCK;
            }
            else if (((object)unit.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)) || ((object)unit.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.IronSpearWeapon)))
            {
                return com.oddlabs.tt.player.AdvancedAI.SCORE_WARRIOR_IRON;
            }
            else if (((object)unit.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)unit.getWeaponFactory().getType() == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return com.oddlabs.tt.player.AdvancedAI.SCORE_WARRIOR_RUBBER;
            }
            return com.oddlabs.tt.player.AdvancedAI.SCORE_PEON;
        }
        // Java: 3018
        public void nodeGuardTowers(int num_towers)
        {
            if ((getTowers() == null && num_towers > 0) || (getTowers() != null && num_towers > getTowers().Length))
            {
                nodeBuildTower(num_towers);
            }
            if (num_towers > 0 && getTowers() != null)
            {
                com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
                bool[] used = (idle_warriors != null ? new bool[idle_warriors.Length] : (bool[])(null));
                for (int i = 0; i < getTowers().Length; i++)
                {
                    com.oddlabs.tt.model.Building tower = ((com.oddlabs.tt.model.Building)getTowers()[i]);
                    if (!tower.getUnitContainer().isSupplyFull() && idle_warriors != null)
                    {
                        com.oddlabs.tt.model.Unit best = findBestIdleWarrior(idle_warriors, used);
                        if (best != null)
                        {
                            getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(best), tower, com.oddlabs.tt.model.Action.DEFAULT, false);
                            nodeDeployUnitsInArmory(1);
                        }
                    }
                    else if ((tower.getUnitContainer() is com.oddlabs.tt.model.MountUnitContainer mount_container) && idle_warriors != null)
                    {
                        com.oddlabs.tt.model.Unit mounted = mount_container.getUnit();
                        if (mounted != null)
                        {
                            int mounted_score = getUnitScore(mounted);
                            com.oddlabs.tt.model.Unit best_2 = findBestIdleWarrior(idle_warriors, used);
                            if (best_2 != null && getUnitScore(best_2) > mounted_score)
                            {
                                getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(best_2), tower, com.oddlabs.tt.model.Action.DEFAULT, false);
                            }
                            else if (best_2 != null)
                            {
                                for (int j = 0; j < idle_warriors.Length; j++)
                                {
                                    if (((object)idle_warriors[j] == (object)best_2))
                                    {
                                        used[j] = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        // Java: 3072
        public void nodeBuildTower(int number)
        {
            int in_progress = countTowersUnderConstruction();
            int existing = (getTowers() != null ? getTowers().Length : 0);
            if (in_progress >= maxConcurrentTowers() || existing + in_progress >= number || getQuarters() == null || getArmory() == null)
            {
                return;
            }
            if (countWarriors() < NUM_WARRIORS[difficulty] / 2)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] builders = getPeons(10);
            if (builders.Length < 10)
            {
                return;
            }
            int ox = default;
            int oy = default;
            if (!occupied_zones.isEmpty() && (existing + in_progress) % 3 == 2)
            {
                int[] zone = ((int[])occupied_zones.get((existing + in_progress) % occupied_zones.size()));
                ox = zone[0];
                oy = zone[1];
            }
            else
            {
                com.oddlabs.tt.model.Building origin = homeBuilding();
                if (origin == null)
                {
                    return;
                }
                ox = origin.getGridX();
                oy = origin.getGridY();
            }
            double base_angle = default;
            com.oddlabs.tt.player.Player focus = chooseFocusEnemy();
            if (focus != null)
            {
                int ex = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(focus.getStartX());
                int ey = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(focus.getStartY());
                base_angle = java.lang.StrictMath.atan2(ey - oy, ex - ox);
            }
            else
            {
                int center = getOwner().getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
                base_angle = java.lang.StrictMath.atan2(center - oy, center - ox);
            }
            double spread = java.lang.Math.PI / 2;
            double random_offset = ((float)(getOwner().getWorld().getRandom().nextFloat() - 0.5f)) * spread;
            double angle = base_angle + random_offset;
            int slot = existing + in_progress;
            float radius = (float)((float)(com.oddlabs.tt.player.AdvancedAI.TOWER_BASE_RADIUS + (float)(((float)(((float)(slot)) / com.oddlabs.tt.player.AdvancedAI.TOWER_SPACING_SLOTS)) * com.oddlabs.tt.player.AdvancedAI.TOWER_RADIUS_STEP)) + (float)(getOwner().getWorld().getRandom().nextFloat() * 6.0f));
            int tx = java.lang.JC.d2i((ox + radius * java.lang.StrictMath.cos(angle)));
            int ty = java.lang.JC.d2i((oy + radius * java.lang.StrictMath.sin(angle)));
            buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, builders, tx, ty, com.oddlabs.tt.player.AdvancedAI.TOWER_SCAN_RANGE);
        }
        // Java: 3135
        public void nodeBuildResourceTowers()
        {
            if (resource_towers_built >= numResourceTowers())
            {
                return;
            }
            if (countTowersUnderConstruction() >= maxConcurrentTowers())
            {
                return;
            }
            if (getQuarters() == null || getArmory() == null)
            {
                return;
            }
            if (countWarriors() < NUM_WARRIORS[difficulty] / 2)
            {
                return;
            }
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return;
            }
            com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter filter = new com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter(typeof(com.oddlabs.tt.model.IronSupply), 250, occupied_zones);
            getUnitGrid().scan(filter, home.getGridX(), home.getGridY());
            if (!filter.found())
            {
                filter = new com.oddlabs.tt.player.AdvancedAI.SupplyLocationFilter(typeof(com.oddlabs.tt.model.RockSupply), 250, occupied_zones);
                getUnitGrid().scan(filter, home.getGridX(), home.getGridY());
            }
            if (!filter.found())
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] builders = getPeons(10);
            if (builders.Length < 10)
            {
                return;
            }
            if (buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, firstN(builders, 10), filter.getTarget().getGridX(), filter.getTarget().getGridY(), 20))
            {
                resource_towers_built++;
                occupied_zones.add(new int[] { filter.getTarget().getGridX(), filter.getTarget().getGridY() });
            }
        }
        public int under_construction_cache_tick;
        public java.util.Set towers_under_construction_cache;
        public java.util.Set quarters_under_construction_cache;
        public java.util.Set armories_under_construction_cache;
        // Java: 3202
        public void ensureUnderConstructionCacheFresh()
        {
            int tick = getOwner().getWorld().getTick();
            if (tick == under_construction_cache_tick)
            {
                return;
            }
            under_construction_cache_tick = tick;
            java.util.Set towers = new java.util.LinkedHashSet();
            java.util.Set quarters = new java.util.LinkedHashSet();
            java.util.Set armories = new java.util.LinkedHashSet();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if (s.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.behaviour.Controller c = s.getPrimaryController();
                com.oddlabs.tt.model.Building building = null;
                if ((c is com.oddlabs.tt.model.behaviour.PlaceBuildingController pbc))
                {
                    building = pbc.getBuilding();
                }
                else if ((c is com.oddlabs.tt.model.behaviour.RepairController rc))
                {
                    building = rc.getBuilding();
                }
                if (building == null || building.isDead() || building.isComplete())
                {
                    continue;
                }
                if (isTowerTemplate(building))
                {
                    towers.add(building);
                }
                else if (isQuartersTemplate(building))
                {
                    quarters.add(building);
                }
                else if (isArmoryTemplate(building))
                {
                    armories.add(building);
                }
            }
            towers_under_construction_cache = towers;
            quarters_under_construction_cache = quarters;
            armories_under_construction_cache = armories;
        }
        // Java: 3231
        public bool isTowerTemplate(com.oddlabs.tt.model.Building building)
        {
            com.oddlabs.tt.model.BuildingTemplate tower_template = getOwner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_TOWER);
            return ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == tower_template.getTemplateID();
        }
        // Java: 3236
        public java.util.Set getTowersUnderConstruction()
        {
            ensureUnderConstructionCacheFresh();
            return towers_under_construction_cache;
        }
        // Java: 3241
        public int countTowersUnderConstruction()
        {
            return getTowersUnderConstruction().size();
        }
        // Java: 3245
        public bool isQuartersTemplate(com.oddlabs.tt.model.Building building)
        {
            com.oddlabs.tt.model.BuildingTemplate quarters_template = getOwner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            return ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == quarters_template.getTemplateID();
        }
        // Java: 3250
        public java.util.Set getQuartersUnderConstruction()
        {
            ensureUnderConstructionCacheFresh();
            return quarters_under_construction_cache;
        }
        // Java: 3255
        public int countQuartersUnderConstruction()
        {
            return getQuartersUnderConstruction().size();
        }
        // Java: 3261
        public bool isArmoryTemplate(com.oddlabs.tt.model.Building building)
        {
            com.oddlabs.tt.model.BuildingTemplate armory_template = getOwner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_ARMORY);
            return ((com.oddlabs.tt.model.BuildingTemplate)building.getTemplate()).getTemplateID() == armory_template.getTemplateID();
        }
        // Java: 3266
        public java.util.Set getArmoriesUnderConstruction()
        {
            ensureUnderConstructionCacheFresh();
            return armories_under_construction_cache;
        }
        // Java: 3271
        public int countArmoriesUnderConstruction()
        {
            return getArmoriesUnderConstruction().size();
        }
        // Java: 3275
        public void nodeAssignIdlePeons()
        {
            if (getIdlePeons() == null)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable site = firstAliveConstructionSite();
            if (quartersUnderConstruction() && site != null)
            {
                getOwner().setTarget(getIdlePeons(), site, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
            else if (armoryUnderConstruction() && site != null)
            {
                getOwner().setTarget(getIdlePeons(), site, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
            else if (assignIdlePeonsToTowers())
            {
            }
            else if (shipUnderConstruction() && site != null)
            {
                getOwner().setTarget(getIdlePeons(), site, com.oddlabs.tt.model.Action.DEFAULT, false);
            }
        }
        // Java: 3303
        public void nodeAssignIdleGatherers()
        {
            com.oddlabs.tt.model.Selectable[] idle = getIdlePeons();
            if (idle == null || idle.Length == 0)
            {
                return;
            }
            com.oddlabs.tt.model.Building drop_off = gatherDropOffBuilding();
            if (drop_off == null)
            {
                return;
            }
            int tree = com.oddlabs.tt.player.AdvancedAI.countOrZero(getGatherTreePeons());
            int rock = com.oddlabs.tt.player.AdvancedAI.countOrZero(getGatherRockPeons());
            int iron = com.oddlabs.tt.player.AdvancedAI.countOrZero(getGatherIronPeons());
            int rubber = com.oddlabs.tt.player.AdvancedAI.countOrZero(getGatherRubberPeons());
            foreach (com.oddlabs.tt.model.Selectable s in idle)
            {
                if (!((s is com.oddlabs.tt.model.Unit unit)) || unit.isDead() || unit.isMounted())
                {
                    continue;
                }
                if (!unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                {
                    continue;
                }
                int tree_room = com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_TREE[difficulty] - tree;
                int rock_room = com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_ROCK[difficulty] - rock;
                int iron_room = com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_IRON[difficulty] - iron;
                int rubber_room = com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_RUBBER[difficulty] - rubber;
                int best_room = java.lang.Math.max(java.lang.Math.max(tree_room, rock_room), java.lang.Math.max(iron_room, rubber_room));
                System.Type supply_type = default;
                if (best_room > 0)
                {
                    if (tree_room == best_room)
                    {
                        supply_type = typeof(com.oddlabs.tt.landscape.TreeSupply);
                        tree++;
                    }
                    else if (rock_room == best_room)
                    {
                        supply_type = typeof(com.oddlabs.tt.model.RockSupply);
                        rock++;
                    }
                    else if (iron_room == best_room)
                    {
                        supply_type = typeof(com.oddlabs.tt.model.IronSupply);
                        iron++;
                    }
                    else
                    {
                        supply_type = typeof(com.oddlabs.tt.model.RubberSupply);
                        rubber++;
                    }
                }
                else
                {
                    int min = java.lang.Math.min(java.lang.Math.min(tree, rock), java.lang.Math.min(iron, rubber));
                    if (tree == min)
                    {
                        supply_type = typeof(com.oddlabs.tt.landscape.TreeSupply);
                        tree++;
                    }
                    else if (rock == min)
                    {
                        supply_type = typeof(com.oddlabs.tt.model.RockSupply);
                        rock++;
                    }
                    else if (iron == min)
                    {
                        supply_type = typeof(com.oddlabs.tt.model.IronSupply);
                        iron++;
                    }
                    else
                    {
                        supply_type = typeof(com.oddlabs.tt.model.RubberSupply);
                        rubber++;
                    }
                }
                unit.initGather(supply_type, drop_off);
            }
        }
        // Java: 3350
        public com.oddlabs.tt.model.Selectable firstAliveConstructionSite()
        {
            com.oddlabs.tt.model.Selectable[] sites = getConstructionSites();
            if (sites == null)
            {
                return null;
            }
            foreach (com.oddlabs.tt.model.Selectable site in sites)
            {
                if (!site.isDead())
                {
                    return site;
                }
            }
            return null;
        }
        // Java: 3361
        public static int countOrZero(com.oddlabs.tt.model.Selectable[] list)
        {
            return (list == null ? 0 : list.Length);
        }
        // Java: 3367
        public com.oddlabs.tt.model.Building gatherDropOffBuilding()
        {
            com.oddlabs.tt.model.Selectable[] armory = getArmory();
            if (armory != null)
            {
                foreach (com.oddlabs.tt.model.Selectable a in armory)
                {
                    if ((a is com.oddlabs.tt.model.Building b) && !b.isDead() && b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SUPPLY_CONTAINER))
                    {
                        return b;
                    }
                }
            }
            return null;
        }
        // Java: 3378
        public bool assignIdlePeonsToTowers()
        {
            java.util.Set towers = getTowersUnderConstruction();
            if (towers.isEmpty())
            {
                return false;
            }
            com.oddlabs.tt.model.Selectable[] idle = getIdlePeons();
            if (idle == null || idle.Length == 0)
            {
                return false;
            }
            int next = 0;
            foreach (com.oddlabs.tt.model.Building tower in java.lang.JIter.of(towers))
            {
                if (next >= idle.Length)
                {
                    break;
                }
                if (tower.isDead())
                {
                    continue;
                }
                int current_builders = countBuilders(tower);
                int needed = com.oddlabs.tt.player.AdvancedAI.BUILDERS_PER_TOWER_TARGET - current_builders;
                if (needed <= 0)
                {
                    continue;
                }
                int take = java.lang.Math.min(needed, idle.Length - next);
                com.oddlabs.tt.model.Selectable[] batch = new com.oddlabs.tt.model.Selectable[take];
                java.lang.JSystem.arraycopy(idle, next, batch, 0, take);
                getOwner().setTarget(batch, tower, com.oddlabs.tt.model.Action.DEFAULT, false);
                next += take;
            }
            return true;
        }
        // Java: 3406
        public void nodeCheckMainAttackTargetStillAlive(float t)
        {
            if (tracked_main_attack_target == null)
            {
                return;
            }
            main_attack_target_check_cooldown -= t;
            if (main_attack_target_check_cooldown > 0.0f)
            {
                return;
            }
            main_attack_target_check_cooldown = 5.0f;
            if (tracked_main_attack_target.isDead())
            {
                time_since_attack = getStyleAdjustedStallSeconds();
                tracked_main_attack_target = null;
            }
        }
        // Java: 3428
        public void nodeAttackWithWarriorsAndChieftain(int num_warriors, bool use_chieftain)
        {
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            int min_force = java.lang.Math.max(3, num_warriors / 2);
            if (idle_warriors != null && idle_warriors.Length >= min_force && (!use_chieftain || getOwner().hasActiveChieftain()))
            {
                int actual_num_warriors = java.lang.Math.min(num_warriors, idle_warriors.Length);
                bool idle_chieftain = getIdleChieftains() != null && getIdleChieftains().Length >= 1;
                com.oddlabs.tt.model.Selectable[] warriors = default;
                if (idle_chieftain && use_chieftain)
                {
                    warriors = com.oddlabs.tt.model.Selectable.newArray(actual_num_warriors + 1);
                    warriors[actual_num_warriors] = getIdleChieftains()[0];
                }
                else
                {
                    warriors = com.oddlabs.tt.model.Selectable.newArray(actual_num_warriors);
                }
                java.lang.JSystem.arraycopy(idle_warriors, 0, warriors, 0, actual_num_warriors);
                com.oddlabs.tt.util.Target target = findTarget(warriors[0].getGridX(), warriors[0].getGridY(), warriors.Length);
                if (target != null)
                {
                    tracked_main_attack_target = target;
                    getOwner().setFormation(warriors, com.oddlabs.tt.player.Formation.SQUARE);
                    if (warriors.Length >= com.oddlabs.tt.player.AdvancedAI.FLANK_MIN_FORCE_SIZE)
                    {
                        launchFlankingAttack(warriors, target);
                    }
                    else
                    {
                        getOwner().setLandscapeTarget(warriors, target.getGridX(), target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
                    }
                    if (NUM_WARRIORS[difficulty] < NUM_WARRIORS_MAX[difficulty])
                    {
                        NUM_WARRIORS[difficulty] += NUM_WARRIORS_INCREASE[difficulty];
                    }
                    foreach (com.oddlabs.tt.model.Selectable w in warriors)
                    {
                        if ((w is com.oddlabs.tt.model.Unit u))
                        {
                            attacking_units.add(u);
                        }
                    }
                }
            }
            else
            {
                if (idle_warriors != null)
                {
                    nodeDeployUnitsInArmory(num_warriors - idle_warriors.Length);
                }
                else
                {
                    nodeDeployUnitsInArmory(num_warriors);
                }
            }
        }
        public const int FLANK_MIN_FORCE_SIZE = 15;
        public const float FLANK_GROUP_FRACTION = 0.4f;
        public const float FLANK_OFFSET_DISTANCE = 40.0f;
        // Java: 3525
        public void launchFlankingAttack(com.oddlabs.tt.model.Selectable[] warriors, com.oddlabs.tt.util.Target target)
        {
            int flank_count = java.lang.Math.round((float)(warriors.Length * com.oddlabs.tt.player.AdvancedAI.FLANK_GROUP_FRACTION));
            flank_count = java.lang.Math.max(1, java.lang.Math.min(flank_count, warriors.Length - 1));
            float tx = target.getPositionX();
            float ty = target.getPositionY();
            float dx = (float)(tx - warriors[0].getPositionX());
            float dy = (float)(ty - warriors[0].getPositionY());
            float len = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (len < 1.0f)
            {
                getOwner().setLandscapeTarget(warriors, target.getGridX(), target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
                return;
            }
            dx = (float)(dx / len);
            dy = (float)(dy / len);
            float side = (getOwner().getWorld().getRandom().nextBoolean() ? 1.0f : (float)(-1.0f));
            float perp_x = (float)((float)(-dy) * side);
            float perp_y = (float)(dx * side);
            float waypoint_x = (float)(tx + (float)(perp_x * com.oddlabs.tt.player.AdvancedAI.FLANK_OFFSET_DISTANCE));
            float waypoint_y = (float)(ty + (float)(perp_y * com.oddlabs.tt.player.AdvancedAI.FLANK_OFFSET_DISTANCE));
            com.oddlabs.tt.util.Target flank_waypoint = new com.oddlabs.tt.landscape.LandscapeTarget(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(waypoint_x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(waypoint_y));
            com.oddlabs.tt.model.Selectable[] main_group = new com.oddlabs.tt.model.Selectable[warriors.Length - flank_count];
            com.oddlabs.tt.model.Selectable[] flank_group = new com.oddlabs.tt.model.Selectable[flank_count];
            java.lang.JSystem.arraycopy(warriors, 0, main_group, 0, main_group.Length);
            java.lang.JSystem.arraycopy(warriors, main_group.Length, flank_group, 0, flank_count);
            getOwner().setLandscapeTarget(main_group, target.getGridX(), target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
            foreach (com.oddlabs.tt.model.Selectable unit in flank_group)
            {
                if (unit.getGridX() == flank_waypoint.getGridX() && unit.getGridY() == flank_waypoint.getGridY())
                {
                    unit.initTarget(target, com.oddlabs.tt.model.Action.ATTACK, true);
                }
                else
                {
                    unit.initTarget(flank_waypoint, com.oddlabs.tt.model.Action.MOVE, false);
                    unit.enqueueTarget(target, com.oddlabs.tt.model.Action.ATTACK, true);
                }
            }
        }
        public static int[] CHIEFTAIN_TRAINING_RESERVE;
        // Java: 3593
        public void nodeTrainChieftain()
        {
            if (getQuarters() == null)
            {
                return;
            }
            int reserve = com.oddlabs.tt.player.AdvancedAI.CHIEFTAIN_TRAINING_RESERVE[difficulty];
            foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
            {
                if (q.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.Building quarters = ((com.oddlabs.tt.model.Building)q);
                if (quarters.getUnitContainer().getNumSupplies() < reserve)
                {
                    continue;
                }
                if (quarters.canBuildChieftain())
                {
                    getOwner().trainChieftain(quarters, true);
                }
            }
        }
        // Java: 3635
        public void nodeDeployUnitsInArmory(int num_warriors)
        {
            if (getArmory() == null || getArmory().Length == 0)
            {
                nodeBuildArmory();
                return;
            }
            int alive_armory_count = 0;
            foreach (com.oddlabs.tt.model.Selectable a in getArmory())
            {
                if (!a.isDead())
                {
                    alive_armory_count++;
                }
            }
            int per_armory_cap = (alive_armory_count > 0 ? java.lang.Math.max(1, (num_warriors + alive_armory_count - 1) / alive_armory_count) : num_warriors);
            int remaining = num_warriors;
            java.util.List understaffed_armories = new java.util.ArrayList();
            java.util.List under_weaponed_armories = new java.util.ArrayList();
            java.util.List ordered_armory = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable a_2 in getArmory())
            {
                ordered_armory.add(a_2);
            }
            int rotation = (ordered_armory.isEmpty() ? 0 : armory_deploy_rotation % ordered_armory.size());
            armory_deploy_rotation++;
            for (int offset = 0; offset < ordered_armory.size(); offset++)
            {
                com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)((com.oddlabs.tt.model.Selectable)ordered_armory.get((offset + rotation) % ordered_armory.size())));
                if (armory.isDead())
                {
                    continue;
                }
                int num_units = armory.getUnitContainer().getNumSupplies() - com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_BUILDING_WEAPONS[difficulty];
                int num_weapons = numWeapons(armory) - com.oddlabs.tt.player.AdvancedAI.MIN_WEAPONS_IN_STOCK[difficulty];
                if (num_units < per_armory_cap)
                {
                    understaffed_armories.add(armory);
                }
                if (num_weapons < per_armory_cap)
                {
                    under_weaponed_armories.add(armory);
                }
                if (remaining <= 0 || num_units <= 0 || num_weapons <= 0)
                {
                    continue;
                }
                int deployable = java.lang.Math.min(remaining, java.lang.Math.min(num_units, num_weapons));
                deployable = java.lang.Math.min(deployable, per_armory_cap);
                if (deployable <= 0)
                {
                    continue;
                }
                int num_rubber_units = java.lang.Math.min(deployable, armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies());
                int num_iron_units = java.lang.Math.min(deployable - num_rubber_units, armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies());
                int num_rock_units = java.lang.Math.min(deployable - num_rubber_units - num_iron_units, armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies());
                if (num_rubber_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, num_rubber_units);
                }
                if (num_iron_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, num_iron_units);
                }
                if (num_rock_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, num_rock_units);
                }
                remaining -= deployable;
            }
            int transfer_budget = java.lang.Math.max(remaining, per_armory_cap * understaffed_armories.size());
            int gather_budget = java.lang.Math.max(remaining, per_armory_cap);
            if (!understaffed_armories.isEmpty())
            {
                nodeTransferUnits(transfer_budget, understaffed_armories);
            }
            foreach (com.oddlabs.tt.model.Building armory_2 in java.lang.JIter.of(under_weaponed_armories))
            {
                nodeGather(armory_2, gather_budget);
            }
            if (alive_armory_count == 0)
            {
                nodeBuildArmory();
            }
        }
        // Java: 3758
        public void nodeGather(com.oddlabs.tt.model.Building armory, int num_units)
        {
            int[] gatherer_counts = getOwner().getGathererCounts(armory);
            int tree = gatherer_counts[0];
            int rock = gatherer_counts[1];
            int iron = gatherer_counts[2];
            int rubber = gatherer_counts[3];
            if (tree >= com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_TREE[difficulty])
            {
                tree = java.lang.Integer.MAX_VALUE;
            }
            if (rock >= com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_ROCK[difficulty])
            {
                rock = java.lang.Integer.MAX_VALUE;
            }
            if (iron >= com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_IRON[difficulty])
            {
                iron = java.lang.Integer.MAX_VALUE;
            }
            if (rubber >= com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_RUBBER[difficulty])
            {
                rubber = java.lang.Integer.MAX_VALUE;
            }
            bool deployed = default;
            do
            {
                deployed = false;
                if (num_units > 0 && tree < com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_TREE[difficulty] && tree <= rock && tree <= iron && tree <= rubber)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_TREE, 1);
                    deployed = true;
                    tree++;
                }
                else if (num_units > 0 && rock < com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_ROCK[difficulty] && rock <= tree && rock <= iron && rock <= rubber)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_ROCK, 1);
                    deployed = true;
                    rock++;
                }
                else if (num_units > 0 && iron < com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_IRON[difficulty] && iron <= tree && iron <= rock && iron <= rubber)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_IRON, 1);
                    deployed = true;
                    iron++;
                }
                else if (num_units > 0 && rubber < com.oddlabs.tt.player.AdvancedAI.MAX_UNITS_GATHERING_RUBBER[difficulty] && rubber <= tree && rubber <= rock && rubber <= iron)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON_HARVEST_RUBBER, 1);
                    deployed = true;
                    rubber++;
                }
                num_units--;
            }
            while (deployed);
        }
        public int transfer_armory_index;
        // Java: 3841
        public void nodeTransferUnits(int num_units, java.util.List understaffed_armories)
        {
            if (getQuarters() == null || getQuarters().Length == 0)
            {
                nodeBuildQuarters();
                return;
            }
            if (understaffed_armories.isEmpty())
            {
                return;
            }
            int remaining = num_units;
            foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
            {
                if (remaining <= 0)
                {
                    break;
                }
                com.oddlabs.tt.model.Building quarters = ((com.oddlabs.tt.model.Building)q);
                if (quarters.isDead())
                {
                    continue;
                }
                int tick = getOwner().getWorld().getTick();
                bool quarters_human_overridden = quarters.getHumanOverrideUntilTick() > tick;
                com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)understaffed_armories.get(transfer_armory_index % understaffed_armories.size()));
                transfer_armory_index++;
                int surplus = quarters.getUnitContainer().getNumSupplies() - com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty];
                if (surplus > 0)
                {
                    if (!quarters_human_overridden)
                    {
                        quarters.setRallyPoint(armory);
                    }
                    int units = java.lang.Math.min(remaining, surplus);
                    getOwner().deployUnits(quarters, com.oddlabs.tt.model.DeployType.PEON, units);
                    remaining -= units;
                }
            }
        }
        // Java: 3886
        public void nodeBuildArmory()
        {
            if (!quartersUnderConstruction() && getQuarters() == null)
            {
                nodeBuildQuarters();
            }
            com.oddlabs.tt.model.Building quarters = null;
            bool any_reproduce = false;
            if (getQuarters() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
                {
                    if (!q.isDead())
                    {
                        if (quarters == null)
                        {
                            quarters = ((com.oddlabs.tt.model.Building)q);
                        }
                        if ((((com.oddlabs.tt.model.Building)q)).getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
                        {
                            any_reproduce = true;
                        }
                    }
                }
            }
            int existing_armories = (getArmory() != null ? getArmory().Length : 0);
            int armories_in_progress = countArmoriesUnderConstruction();
            bool should_build_more = armories_in_progress < maxConcurrentArmories() && existing_armories + armories_in_progress < targetNumArmories();
            if (should_build_more && any_reproduce)
            {
                com.oddlabs.tt.model.Selectable[] builders = getPeons(20);
                if (builders.Length < 20 && getQuarters() != null)
                {
                    foreach (com.oddlabs.tt.model.Selectable q_2 in getQuarters())
                    {
                        com.oddlabs.tt.model.Building b = ((com.oddlabs.tt.model.Building)q_2);
                        if (!b.isDead() && b.getUnitContainer().getNumSupplies() >= 20)
                        {
                            getOwner().deployUnits(b, com.oddlabs.tt.model.DeployType.PEON, 20);
                            break;
                        }
                    }
                }
                if (builders.Length == 0)
                {
                    return;
                }
                com.oddlabs.tt.model.Building origin = homeBuilding();
                int ox = (origin != null ? origin.getGridX() : builders[0].getGridX());
                int oy = (origin != null ? origin.getGridY() : builders[0].getGridY());
                int center = getOwner().getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
                int dx = center - ox;
                int dy = center - oy;
                double angle_offset = existing_armories * (java.lang.Math.PI / 4);
                double angle = java.lang.StrictMath.atan2(dy, dx) + angle_offset;
                int tx = java.lang.JC.d2i((ox + 10.0f * java.lang.StrictMath.cos(angle)));
                int ty = java.lang.JC.d2i((oy + 10.0f * java.lang.StrictMath.sin(angle)));
                setArmoryUnderConstruction(buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, builders, tx, ty));
                reclassify();
            }
        }
        // Java: 3944
        public void nodeBuildQuarters()
        {
            int existing = (getQuarters() != null ? getQuarters().Length : 0);
            int in_progress = countQuartersUnderConstruction();
            if (existing >= 1 && getArmory() == null)
            {
                return;
            }
            if (in_progress < maxConcurrentQuarters() && existing + in_progress < targetNumQuarters())
            {
                if (existing >= 2 && !anyQuartersMetReserve())
                {
                    return;
                }
                com.oddlabs.tt.model.Selectable[] builders = getPeons(com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty]);
                if (builders.Length == 0)
                {
                    return;
                }
                com.oddlabs.tt.model.Building origin = homeBuilding();
                int ox = (origin != null ? origin.getGridX() : builders[0].getGridX());
                int oy = (origin != null ? origin.getGridY() : builders[0].getGridY());
                int center = getOwner().getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
                int dx = center - ox;
                int dy = center - oy;
                double angle_offset = (existing + in_progress) * (java.lang.Math.PI / 4);
                double angle = java.lang.StrictMath.atan2(dy, dx) + angle_offset;
                int tx = java.lang.JC.d2i((ox + 10.0f * java.lang.StrictMath.cos(angle)));
                int ty = java.lang.JC.d2i((oy + 10.0f * java.lang.StrictMath.sin(angle)));
                buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, builders, tx, ty);
                reclassify();
            }
        }
        // Java: 3997
        public bool anyQuartersMetReserve()
        {
            if (getQuarters() == null)
            {
                return false;
            }
            int reserve = com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty];
            foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
            {
                if (q.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.Building quarters = ((com.oddlabs.tt.model.Building)q);
                if (quarters.getUnitContainer().getNumSupplies() >= reserve)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4011
        public bool isArchipelago()
        {
            return getOwner().getWorld().isArchipelago();
        }
        // Java: 4015
        public bool baseBuildingsDone()
        {
            return getQuarters() != null && getArmory() != null && !quartersUnderConstruction() && !armoryUnderConstruction() && !towerUnderConstruction();
        }
        // Java: 4020
        public void nodeBuildShipAndLoad()
        {
            java.util.List ships = getOwnShips();
            int seaworthy = 0;
            foreach (com.oddlabs.tt.model.Ship s in java.lang.JIter.of(ships))
            {
                if (!s.isComplete() || !shipTooDamaged(s))
                {
                    seaworthy++;
                }
            }
            nodeBuildShip(seaworthy);
            int now = getOwner().getWorld().getTick();
            foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(ships))
            {
                if (!ship.isComplete() || ship.isMoving() || !shipAtHome(ship) || ship_unloading.containsKey(ship))
                {
                    continue;
                }
                int peons = ship.getShipHR().countPeons();
                com.oddlabs.tt.model.Building home = homeBuilding();
                if (shipTooDamaged(ship) && ship.getShipHR().countUnits() > 0)
                {
                    if (home != null)
                    {
                        ship.setRallyPoint(home);
                    }
                    com.oddlabs.tt.model.ShipHR hr = ship.getShipHR();
                    int rock = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                    int iron = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                    int rubber = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
                    if (rock > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, rock);
                    }
                    if (iron > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, iron);
                    }
                    if (rubber > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, rubber);
                    }
                    if (peons > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, peons);
                    }
                    ship_unloading.put(ship, now);
                }
                else if (peons > com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS + 6)
                {
                    if (home != null)
                    {
                        ship.setRallyPoint(home);
                    }
                    getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, peons - com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS);
                    ship_unloading.put(ship, now);
                }
            }
            foreach (com.oddlabs.tt.model.Ship ship_2 in java.lang.JIter.of(ships))
            {
                if (ship_2.isComplete() && !ship_2.isMoving() && shipAtHome(ship_2) && !shipFullyCrewed(ship_2) && !shipTooDamaged(ship_2))
                {
                    int? last = ((int?)ship_load_tick.get(ship_2));
                    if (last != null && (float)((now - ((int)last)) * com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK) < com.oddlabs.tt.player.AdvancedAI.LOAD_PERIOD_SECONDS)
                    {
                        continue;
                    }
                    ship_load_tick.put(ship_2, now);
                    nodeLoadShip(ship_2);
                    return;
                }
            }
        }
        // Java: 4064
        public java.util.List getOwnShips()
        {
            java.util.List ships = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead())
                {
                    ships.add(ship);
                }
            }
            return ships;
        }
        // Java: 4073
        public void nodeUseShip()
        {
            java.util.List ships = getOwnShips();
            foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(ships))
            {
                useShip(ship);
            }
        }
        // Java: 4080
        public void useShip(com.oddlabs.tt.model.Ship ship)
        {
            if (ship.isDead() || !ship.isComplete())
            {
                return;
            }
            if (unjam(ship))
            {
                return;
            }
            com.oddlabs.tt.player.AdvancedAI.ExplorationTarget hop_target = ((com.oddlabs.tt.player.AdvancedAI.ExplorationTarget)ship_hop.get(ship));
            if (hop_target != null)
            {
                if (ship.isMoving())
                {
                    return;
                }
                ship_hop.remove(ship);
                sailToIsland(ship, hop_target);
                ship_exploration.put(ship, hop_target);
                return;
            }
            com.oddlabs.tt.player.AdvancedAI.ExplorationTarget exploring = ((com.oddlabs.tt.player.AdvancedAI.ExplorationTarget)ship_exploration.get(ship));
            if (exploring != null)
            {
                if (ship.isMoving())
                {
                    ship_departures_failed.remove(ship);
                    return;
                }
                int[] aim0 = ((int[])ship_sail_point.get(ship));
                bool near_aim = aim0 != null && (ship.getGridX() - aim0[0]) * (ship.getGridX() - aim0[0]) + (ship.getGridY() - aim0[1]) * (ship.getGridY() - aim0[1]) <= 40 * 40;
                if (!near_aim && !closeToIsland(ship, exploring.island_id()))
                {
                    int fails = ((int)((int?)ship_departures_failed.merge(ship, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }))));
                    ship_exploration.remove(ship);
                    if (fails % 3 == 2 || fails % 3 == 0)
                    {
                        com.oddlabs.tt.landscape.IslandInfo info0 = getUnitGrid().getIslandInfo(exploring.island_id());
                        int tx = (info0 != null ? info0.startX() : getUnitGrid().getGridSize() / 2);
                        int ty = (info0 != null ? info0.startY() : getUnitGrid().getGridSize() / 2);
                        float part = (fails % 3 == 2 ? 0.35f : 0.15f);
                        int[] hop = openWater(ship.getGridX() + java.lang.JC.f2i(((float)((tx - ship.getGridX()) * part))), ship.getGridY() + java.lang.JC.f2i(((float)((ty - ship.getGridY()) * part))));
                        if (hop != null)
                        {
                            getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), hop[0], hop[1], com.oddlabs.tt.model.Action.MOVE, false);
                            if (fails < 9)
                            {
                                ship_hop.put(ship, exploring);
                            }
                            return;
                        }
                    }
                    com.oddlabs.tt.landscape.IslandInfo info = getUnitGrid().getIslandInfo(exploring.island_id());
                    if (info != null && fails % 3 == 1)
                    {
                        getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), info.startX(), info.startY(), com.oddlabs.tt.model.Action.MOVE, false);
                        ship_exploration.put(ship, exploring);
                        return;
                    }
                }
                ship_exploration.remove(ship);
                int[] aim = ((int[])ship_sail_point.remove(ship));
                bool arrived = closeToIsland(ship, exploring.island_id()) || aim != null && (ship.getGridX() - aim[0]) * (ship.getGridX() - aim[0]) + (ship.getGridY() - aim[1]) * (ship.getGridY() - aim[1]) <= 40 * 40;
                if (arrived)
                {
                    deployCrewAtIsland(ship, exploring);
                    ship_unloading.put(ship, getOwner().getWorld().getTick());
                    return;
                }
            }
            int? unloading_since = ((int?)ship_unloading.get(ship));
            if (unloading_since != null)
            {
                int warriors_aboard = ship.getShipHR().countUnits() - ship.getShipHR().countPeons();
                float waited = (float)((getOwner().getWorld().getTick() - ((int)unloading_since)) * com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK);
                if (warriors_aboard > 0 && waited < com.oddlabs.tt.player.AdvancedAI.UNLOAD_SECONDS)
                {
                    return;
                }
                ship_unloading.remove(ship);
            }
            if (ship.isMoving())
            {
                return;
            }
            bool at_home = shipAtHome(ship);
            bool should_escape = shipShouldEscape(ship);
            bool battle_ready = !should_escape && shipLaunchReady(ship, at_home);
            if (battle_ready)
            {
                com.oddlabs.tt.model.Selectable sea_enemy = getOwner().findNearestEnemyShip(ship.getGridX(), ship.getGridY());
                if (sea_enemy != null && (sea_enemy.isDead() || !com.oddlabs.tt.player.AdvancedAI.withinCells(ship, sea_enemy, com.oddlabs.tt.player.AdvancedAI.SEA_ENGAGE_RANGE)))
                {
                    sea_enemy = null;
                }
                com.oddlabs.tt.model.Selectable beach_enemy = getOwner().findNearestEnemyOnBeach(ship.getGridX(), ship.getGridY());
                if (beach_enemy != null && (beach_enemy.isDead() || !com.oddlabs.tt.player.AdvancedAI.withinCells(ship, beach_enemy, com.oddlabs.tt.player.AdvancedAI.SEA_ENGAGE_RANGE)))
                {
                    beach_enemy = null;
                }
                com.oddlabs.tt.model.Selectable enemy = null;
                if (beach_enemy != null && sea_enemy != null)
                {
                    int sea_dx = sea_enemy.getGridX() - ship.getGridX();
                    int sea_dy = sea_enemy.getGridY() - ship.getGridY();
                    int sea_d2 = sea_dx * sea_dx + sea_dy * sea_dy;
                    int beach_dx = beach_enemy.getGridX() - ship.getGridX();
                    int beach_dy = beach_enemy.getGridY() - ship.getGridY();
                    int beach_d2 = beach_dx * beach_dx + beach_dy * beach_dy;
                    if (sea_d2 > beach_d2)
                    {
                        enemy = beach_enemy;
                    }
                    else
                    {
                        enemy = sea_enemy;
                    }
                }
                else if (beach_enemy != null)
                {
                    enemy = beach_enemy;
                }
                else
                {
                    enemy = sea_enemy;
                }
                if (enemy != null)
                {
                    getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), enemy, com.oddlabs.tt.model.Action.MOVE, true);
                    return;
                }
                if (at_home)
                {
                    com.oddlabs.tt.player.AdvancedAI.ExplorationTarget target = chooseExplorationIsland();
                    if (target != null)
                    {
                        sailToIsland(ship, target);
                        ship_exploration.put(ship, target);
                        return;
                    }
                }
            }
            if (!at_home)
            {
                com.oddlabs.tt.model.Building origin = homeBuilding();
                if (origin != null)
                {
                    getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), origin, com.oddlabs.tt.model.Action.MOVE, false);
                }
            }
        }
        // Java: 4218
        public string debugShips()
        {
            java.lang.StringBuilder sb = new java.lang.StringBuilder();
            foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(getOwnShips()))
            {
                sb.append(" [ship ").append(ship.getGridX()).append(',').append(ship.getGridY());
                if (!ship.isComplete())
                {
                    sb.append(" building]");
                    continue;
                }
                int peons = ship.getShipHR().countPeons();
                sb.append(" p").append(peons).append(" w").append(ship.getShipHR().countUnits() - peons);
                sb.append((ship.isMoving() ? " moving" : " still")).append((shipAtHome(ship) ? " home" : " away"));
                sb.append((shipBattleReady(ship) ? " ready" : "")).append((shipShouldEscape(ship) ? " escape" : ""));
                sb.append((ship_exploration.containsKey(ship) ? " voyage" + java.lang.JStr.valueOf(((com.oddlabs.tt.player.AdvancedAI.ExplorationTarget)ship_exploration.get(ship))) : ""));
                sb.append((ship_unloading.containsKey(ship) ? " unloading" : ""));
                sb.append(" hp ").append(ship.getHitPoints());
                sb.append(" ctl ").append((ship.getCurrentController() == null ? "-" : ship.getCurrentController().GetType().getSimpleName()));
                sb.append(" beh ").append((ship.getCurrentBehaviour() == null ? "-" : ship.getCurrentBehaviour().GetType().getSimpleName())).append(']');
            }
            sb.append(" base ").append(baseBuildingsDone()).append(" q/a/t ").append(quartersUnderConstruction()).append('/').append(armoryUnderConstruction()).append('/').append(towerUnderConstruction());
            sb.append(" idleP ").append((getIdlePeons() == null ? 0 : getIdlePeons().Length)).append(" woodP ").append((getGatherTreePeons() == null ? 0 : getGatherTreePeons().Length));
            sb.append(" fleet ").append(getOwnShips().size()).append('/').append(wantedFleet()).append(" incomplete ").append(getIncompleteShip() != null);
            sb.append(" plans simple/region/noSrc/noDst/noPath/noTarget ").append(com.oddlabs.tt.pathfinder.ShipTrajectory.dbg_simple).append('/').append(com.oddlabs.tt.pathfinder.ShipTrajectory.dbg_region_ok).append('/').append(com.oddlabs.tt.pathfinder.ShipTrajectory.dbg_no_src).append('/').append(com.oddlabs.tt.pathfinder.ShipTrajectory.dbg_no_dst).append('/').append(com.oddlabs.tt.pathfinder.ShipTrajectory.dbg_no_path).append('/').append(com.oddlabs.tt.pathfinder.ShipTrajectory.dbg_no_target);
            sb.append(" lastShipSite ").append(last_ship_site);
            sb.append(" jams ").append(jams).append(" home island ").append(homeIsland()).append(" target ").append(chooseExplorationIsland());
            return java.lang.JO.toString(sb);
        }
        // Java: 4243
        public int[] openWater(int gx, int gy)
        {
            com.oddlabs.tt.landscape.HeightMap hm = getOwner().getWorld().getHeightMap();
            int size = getUnitGrid().getGridSize();
            float deep = (float)(hm.getSeaLevelMeters() - 2.0f);
            for (int r = 0; r <= 40; r += 2)
            {
                for (int i = -r; i <= r; i += 2)
                {
                    foreach (int[] c in new int[][] { new int[] { gx + i, gy - r }, new int[] { gx + i, gy + r }, new int[] { gx - r, gy + i }, new int[] { gx + r, gy + i } })
                    {
                        if (c[0] < 6 || c[1] < 6 || c[0] >= size - 6 || c[1] >= size - 6)
                        {
                            continue;
                        }
                        if (hm.getNearestHeight(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(c[0]), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(c[1])) < deep)
                        {
                            return c;
                        }
                    }
                }
            }
            return null;
        }
        // Java: 4260
        public bool unjam(com.oddlabs.tt.model.Ship ship)
        {
            int now = getOwner().getWorld().getTick();
            int x = ship.getGridX();
            int y = ship.getGridY();
            int[] last = ((int[])ship_last_pos.get(ship));
            if (last == null || java.lang.Math.abs(last[0] - x) > 4 || java.lang.Math.abs(last[1] - y) > 4 || !ship.isMoving())
            {
                ship_last_pos.put(ship, new int[] { x, y, now });
                return false;
            }
            if ((float)((now - last[2]) * com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK) < com.oddlabs.tt.player.AdvancedAI.JAM_SECONDS)
            {
                return false;
            }
            ship_last_pos.put(ship, new int[] { x, y, now });
            jams++;
            com.oddlabs.tt.model.Selectable other = null;
            int best = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                foreach (com.oddlabs.tt.model.Selectable o in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if ((o is com.oddlabs.tt.model.Ship os) && ((object)os != (object)ship) && !os.isDead())
                    {
                        int d = (os.getGridX() - x) * (os.getGridX() - x) + (os.getGridY() - y) * (os.getGridY() - y);
                        if (d < best)
                        {
                            best = d;
                            other = os;
                        }
                    }
                }
            }
            int dx = (other != null ? java.lang.Integer.signum(x - other.getGridX()) : 1);
            int dy = (other != null ? java.lang.Integer.signum(y - other.getGridY()) : 0);
            if (dx == 0 && dy == 0)
            {
                dx = 1;
            }
            int size = getUnitGrid().getGridSize();
            getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), java.lang.Math.clamp(x + dx * 20 - dy * 8, 4, size - 4), java.lang.Math.clamp(y + dy * 20 + dx * 8, 4, size - 4), com.oddlabs.tt.model.Action.MOVE, false);
            ship_exploration.remove(ship);
            ship_sail_point.remove(ship);
            return true;
        }
        // Java: 4291
        public static bool withinCells(com.oddlabs.tt.model.Selectable a, com.oddlabs.tt.model.Selectable b, int cells)
        {
            int dx = a.getGridX() - b.getGridX();
            int dy = a.getGridY() - b.getGridY();
            return dx * dx + dy * dy <= cells * cells;
        }
        // Java: 4297
        public bool shipBattleReady(com.oddlabs.tt.model.Ship ship)
        {
            return shipFullyCrewed(ship) && !shipNeedsRepair(ship);
        }
        // Java: 4301
        public bool shipShouldEscape(com.oddlabs.tt.model.Ship ship)
        {
            return shipTooDamaged(ship);
        }
        // Java: 4307
        public bool shipLaunchReady(com.oddlabs.tt.model.Ship ship, bool at_home)
        {
            int warriors = ship.getShipHR().countUnits() - ship.getShipHR().countPeons();
            if (!at_home)
            {
                ship_ready_since.remove(ship);
                return warriors > 0;
            }
            if (warriors >= com.oddlabs.tt.player.AdvancedAI.LAUNCH_WARRIORS)
            {
                return true;
            }
            if (warriors < com.oddlabs.tt.player.AdvancedAI.LAUNCH_WARRIORS_MIN)
            {
                ship_ready_since.remove(ship);
                return false;
            }
            int now = getOwner().getWorld().getTick();
            int? since = ((int?)ship_ready_since.putIfAbsent(ship, now));
            float waited = (since == null ? 0.0f : (float)((now - ((int)since)) * com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK));
            return getOwner().getUnitCountContainer().isSupplyFull() || waited >= com.oddlabs.tt.player.AdvancedAI.LAUNCH_WAIT_SECONDS;
        }
        // Java: 4325
        public bool shipTooDamaged(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getHitPoints() < (float)(com.oddlabs.tt.player.AdvancedAI.SHIP_MIN_HEALTH * ship.getBuildingTemplate().getMaxHitPoints());
        }
        // Java: 4329
        public bool shipNeedsRepair(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getHitPoints() < ship.getBuildingTemplate().getMaxHitPoints();
        }
        // Java: 4333
        public bool shipFullyCrewed(com.oddlabs.tt.model.Ship ship)
        {
            int peons = ship.getShipHR().countPeons();
            int warriors = ship.getShipHR().countUnits() - peons;
            return peons >= com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS && warriors >= com.oddlabs.tt.player.AdvancedAI.SHIP_WARRIORS;
        }
        // Java: 4339
        public bool shipCrewedEnough(com.oddlabs.tt.model.Ship ship)
        {
            int peons = ship.getShipHR().countPeons();
            int warriors = ship.getShipHR().countUnits() - peons;
            return peons >= com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS / 2 && warriors >= com.oddlabs.tt.player.AdvancedAI.SHIP_WARRIORS / 3;
        }
        // Java: 4345
        public com.oddlabs.tt.model.Building homeBuilding()
        {
            if (getQuarters() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
                {
                    if (!q.isDead())
                    {
                        return ((com.oddlabs.tt.model.Building)q);
                    }
                }
            }
            if (getArmory() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable a in getArmory())
                {
                    if (!a.isDead())
                    {
                        return ((com.oddlabs.tt.model.Building)a);
                    }
                }
            }
            return null;
        }
        // Java: 4361
        public int homeIsland()
        {
            com.oddlabs.tt.model.Building home = homeBuilding();
            return (home != null ? home.getIslandId() : (-1));
        }
        // Java: 4366
        public bool shipAtHome(com.oddlabs.tt.model.Ship ship)
        {
            com.oddlabs.tt.model.Building entrance = ship.getEntrance();
            if (entrance == null)
            {
                return false;
            }
            return closeToAny(entrance, getQuarters()) || closeToAny(entrance, getArmory());
        }
        // Java: 4374
        public bool closeToAny(com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Selectable[] buildings)
        {
            if (buildings == null)
            {
                return false;
            }
            foreach (com.oddlabs.tt.model.Selectable building in buildings)
            {
                if (building.isDead())
                {
                    continue;
                }
                int dx = building.getGridX() - target.getGridX();
                int dy = building.getGridY() - target.getGridY();
                if (dx * dx + dy * dy <= com.oddlabs.tt.player.AdvancedAI.SHIP_HOME_RANGE * com.oddlabs.tt.player.AdvancedAI.SHIP_HOME_RANGE)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4389
        public bool shipDockedAt(com.oddlabs.tt.model.Ship ship, int island)
        {
            com.oddlabs.tt.model.Building entrance = ship.getEntrance();
            return ((object)entrance != (object)ship) && !entrance.isDead() && entrance.getIslandId() == island;
        }
        // Java: 4401
        public com.oddlabs.tt.player.AdvancedAI.ExplorationTarget chooseExplorationIsland()
        {
            int home_island = homeIsland();
            int best_resource_island = (-1);
            int best_resource_amount = com.oddlabs.tt.player.AdvancedAI.MIN_ISLAND_RESOURCES;
            int hostile_fallback = (-1);
            foreach (int island_id in java.lang.JIter.of(getUnitGrid().getIslandIds()))
            {
                if (island_id == home_island)
                {
                    continue;
                }
                if (islandHasOwnBuilding(island_id))
                {
                    continue;
                }
                if (islandHasEnemyBuilding(island_id))
                {
                    return new com.oddlabs.tt.player.AdvancedAI.ExplorationTarget(island_id, true);
                }
                if (hostile_fallback < 0 && islandHasEnemy(island_id))
                {
                    hostile_fallback = island_id;
                }
                com.oddlabs.tt.landscape.IslandInfo info = getUnitGrid().getIslandInfo(island_id);
                if (info == null)
                {
                    continue;
                }
                int resources = info.trees() + info.rocks() + info.iron();
                if (resources > best_resource_amount)
                {
                    best_resource_amount = resources;
                    best_resource_island = island_id;
                }
            }
            if (hostile_fallback != (-1))
            {
                return new com.oddlabs.tt.player.AdvancedAI.ExplorationTarget(hostile_fallback, true);
            }
            if (best_resource_island != (-1))
            {
                return new com.oddlabs.tt.player.AdvancedAI.ExplorationTarget(best_resource_island, false);
            }
            return null;
        }
        // Java: 4434
        public bool islandHasOwnBuilding(int island_id)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building building) && !building.isDead() && building.getIslandId() == island_id)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 4446
        public bool islandHasEnemyBuilding(int island_id)
        {
            foreach (com.oddlabs.tt.player.Player player in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(player))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.LandBuilding) && !s.isDead() && s.getIslandId() == island_id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 4457
        public bool islandHasEnemy(int island_id)
        {
            foreach (com.oddlabs.tt.player.Player player in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(player))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if (!s.isDead() && s.getIslandId() == island_id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 4473
        public com.oddlabs.tt.model.Selectable findEnemyOnIsland(int island_id)
        {
            com.oddlabs.tt.model.Building home = homeBuilding();
            int home_x = (home != null ? home.getGridX() : 0);
            int home_y = (home != null ? home.getGridY() : 0);
            com.oddlabs.tt.model.Selectable best = null;
            int best_dist_squared = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player player in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(player))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                {
                    if (s.isDead() || s.getIslandId() != island_id || (s is com.oddlabs.tt.model.Ship))
                    {
                        continue;
                    }
                    int dx = s.getGridX() - home_x;
                    int dy = s.getGridY() - home_y;
                    int dist_squared = dx * dx + dy * dy + (((s is com.oddlabs.tt.model.LandBuilding) ? 0 : 200 * 200));
                    if (dist_squared < best_dist_squared)
                    {
                        best_dist_squared = dist_squared;
                        best = s;
                    }
                }
            }
            return best;
        }
        // Java: 4503
        public void sailToIsland(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.player.AdvancedAI.ExplorationTarget target)
        {
            if (target.hostile())
            {
                com.oddlabs.tt.model.Selectable enemy = findEnemyOnIsland(target.island_id());
                if (enemy != null)
                {
                    getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), enemy, com.oddlabs.tt.model.Action.MOVE, true);
                    ship_sail_point.put(ship, new int[] { enemy.getGridX(), enemy.getGridY() });
                    return;
                }
            }
            com.oddlabs.tt.landscape.IslandInfo info = getUnitGrid().getIslandInfo(target.island_id());
            if (info == null)
            {
                return;
            }
            getOwner().setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), info.startX(), info.startY(), com.oddlabs.tt.model.Action.MOVE, false);
            ship_sail_point.put(ship, new int[] { info.startX(), info.startY() });
        }
        // Java: 4523
        public bool closeToIsland(com.oddlabs.tt.model.Ship ship, int island_id)
        {
            com.oddlabs.tt.landscape.IslandInfo info = getUnitGrid().getIslandInfo(island_id);
            if (info == null)
            {
                return false;
            }
            int dx = ship.getGridX() - info.startX();
            int dy = ship.getGridY() - info.startY();
            return dx * dx + dy * dy <= com.oddlabs.tt.player.AdvancedAI.ISLAND_ARRIVAL_RANGE * com.oddlabs.tt.player.AdvancedAI.ISLAND_ARRIVAL_RANGE;
        }
        // Java: 4544
        public void deployCrewAtIsland(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.player.AdvancedAI.ExplorationTarget target)
        {
            com.oddlabs.tt.landscape.IslandInfo info = getUnitGrid().getIslandInfo(target.island_id());
            if (info == null)
            {
                return;
            }
            ship.setRallyPoint(new com.oddlabs.tt.landscape.LandscapeTarget(info.startX(), info.startY()));
            if (target.hostile())
            {
                int warriors_aboard = ship.getShipHR().countUnits() - ship.getShipHR().countPeons();
                if (warriors_aboard > 0)
                {
                    com.oddlabs.tt.model.ShipHR hr = ship.getShipHR();
                    int rock = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
                    int iron = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
                    int rubber = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
                    if (rock > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, rock);
                    }
                    if (iron > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, iron);
                    }
                    if (rubber > 0)
                    {
                        getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, rubber);
                    }
                    com.oddlabs.tt.model.Selectable foe = findEnemyOnIsland(target.island_id());
                    if (foe != null)
                    {
                        ship.setRallyPoint(new com.oddlabs.tt.landscape.LandscapeTarget(foe.getGridX(), foe.getGridY()));
                    }
                }
            }
            else
            {
                int peons_aboard = ship.getShipHR().countPeons();
                if (peons_aboard > 0)
                {
                    getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, java.lang.Math.min(com.oddlabs.tt.player.AdvancedAI.EXPLORE_PEON_BATCH, peons_aboard));
                }
            }
        }
        // Java: 4582
        public int wantedFleet()
        {
            int enemies = 0;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (((object)p != (object)getOwner()) && getOwner().isEnemy(p) && p.isAlive())
                {
                    enemies++;
                }
            }
            return java.lang.Math.max(1, java.lang.Math.min(com.oddlabs.tt.player.AdvancedAI.FLEET_SIZE, enemies + 1));
        }
        // Java: 4590
        public void nodeBuildShip(int fleet_size)
        {
            com.oddlabs.tt.model.Ship ship = getIncompleteShip();
            if (ship == null && fleet_size >= wantedFleet())
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle = getIdlePeons();
            int idle_count = (idle != null ? idle.Length : 0);
            int missing = com.oddlabs.tt.player.AdvancedAI.SHIP_BUILDERS - ((ship != null ? countBuilders(ship.getEntrance()) : 0));
            if (missing > idle_count)
            {
                int remaining = missing - idle_count;
                if (getQuarters() != null)
                {
                    foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
                    {
                        if (remaining <= 0)
                        {
                            break;
                        }
                        com.oddlabs.tt.model.Building quarters = ((com.oddlabs.tt.model.Building)q);
                        remaining -= deployPeonsFromQuarters(quarters, remaining, quarters);
                    }
                }
                if (getArmory() != null)
                {
                    foreach (com.oddlabs.tt.model.Selectable a in getArmory())
                    {
                        if (remaining <= 0)
                        {
                            break;
                        }
                        com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)a);
                        remaining -= deployPeonsFromQuarters(armory, remaining, armory);
                    }
                }
            }
            if (idle_count == 0 && missing > 0 && getGatherTreePeons() != null && getGatherTreePeons().Length > 8)
            {
                idle = getGatherTreePeons();
                idle_count = java.lang.Math.min(missing, idle.Length - 8);
                missing = idle_count;
            }
            if (idle_count == 0 || missing <= 0)
            {
                return;
            }
            if (ship != null)
            {
                if (ship.isComplete())
                {
                    return;
                }
                com.oddlabs.tt.model.Action action = (ship.isComplete() ? com.oddlabs.tt.model.Action.GATHER_REPAIR : com.oddlabs.tt.model.Action.DEFAULT);
                getOwner().setTarget(lastN(idle, missing), ship, action, false);
            }
            else
            {
                com.oddlabs.tt.model.Building origin = homeBuilding();
                int[] site = com.oddlabs.tt.player.NavalCommander.seaShipSite(getOwner(), origin.getGridX(), origin.getGridY());
                last_ship_site = site != null;
                if (site != null)
                {
                    getOwner().placeBuilding(firstN(idle, com.oddlabs.tt.player.AdvancedAI.SHIP_BUILDERS), com.oddlabs.tt.model.Race.BUILDING_SHIP, site[0], site[1], false);
                }
            }
        }
        // Java: 4647
        public bool shipIncomplete(com.oddlabs.tt.model.Ship ship)
        {
            return !ship.isComplete() || ship.isDamaged();
        }
        // Java: 4651
        public com.oddlabs.tt.model.Ship getIncompleteShip()
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead() && !ship.isComplete())
                {
                    return ship;
                }
            }
            com.oddlabs.tt.model.Selectable[] placing = getPlaceBuildingPeons();
            if (placing != null)
            {
                foreach (com.oddlabs.tt.model.Selectable s_2 in placing)
                {
                    if (!s_2.isDead() && (s_2.getPrimaryController() is com.oddlabs.tt.model.behaviour.PlaceBuildingController controller) && (controller.getBuilding() is com.oddlabs.tt.model.Ship ship_2) && !ship_2.isDead())
                    {
                        return ship_2;
                    }
                }
            }
            return null;
        }
        // Java: 4667
        public int countBuilders(com.oddlabs.tt.model.Building b)
        {
            int builders = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if (s.isDead())
                {
                    continue;
                }
                com.oddlabs.tt.model.behaviour.Controller controller = s.getPrimaryController();
                if ((controller is com.oddlabs.tt.model.behaviour.RepairController repair) && ((object)repair.getBuilding() == (object)b))
                {
                    builders++;
                }
                else if ((controller is com.oddlabs.tt.model.behaviour.PlaceBuildingController placing) && ((object)placing.getBuilding() == (object)b))
                {
                    builders++;
                }
            }
            return builders;
        }
        // Java: 4683
        public void nodeLoadShip(com.oddlabs.tt.model.Ship ship)
        {
            int peons_aboard = ship.getShipHR().countPeons();
            int warriors_aboard = ship.getShipHR().countUnits() - peons_aboard;
            if (peons_aboard > com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS + 6)
            {
                com.oddlabs.tt.model.Building home = homeBuilding();
                if (home != null)
                {
                    ship.setRallyPoint(home);
                }
                getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, peons_aboard - com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS);
                return;
            }
            int peons_needed = com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS - peons_aboard;
            if (peons_needed > 0)
            {
                if (getIdlePeons() != null && getIdlePeons().Length > 0)
                {
                    getOwner().setTarget(firstN(getIdlePeons(), peons_needed), ship, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else
                {
                    nodeDeployPeonsFromQuarters(ship, peons_needed);
                    if (last_quarters_deploy == 0 && peons_aboard < com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS / 2 && getGatherTreePeons() != null && getGatherTreePeons().Length > 8)
                    {
                        getOwner().setTarget(firstN(getGatherTreePeons(), java.lang.Math.min(peons_needed, getGatherTreePeons().Length - 8)), ship, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                }
            }
            int warriors_needed = com.oddlabs.tt.player.AdvancedAI.SHIP_WARRIORS - warriors_aboard;
            if (warriors_needed > 0)
            {
                if (getIdleWarriors() != null && getIdleWarriors().Length > 0)
                {
                    getOwner().setTarget(firstN(getIdleWarriors(), warriors_needed), ship, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else
                {
                    nodeDeployUnitsInArmory(warriors_needed);
                }
            }
        }
        // Java: 4716
        public void nodeDeployPeonsFromQuarters(com.oddlabs.tt.model.Ship ship, int num_peons)
        {
            if (getQuarters() == null)
            {
                return;
            }
            int remaining = num_peons;
            foreach (com.oddlabs.tt.model.Selectable q in getQuarters())
            {
                if (remaining <= 0)
                {
                    break;
                }
                remaining -= deployPeonsFromQuarters(((com.oddlabs.tt.model.Building)q), remaining, ((com.oddlabs.tt.model.Building)q));
            }
            last_quarters_deploy = num_peons - remaining;
        }
        public int last_quarters_deploy;
        // Java: 4736
        public int deployPeonsFromQuarters(com.oddlabs.tt.model.Building quarters, int num_peons, com.oddlabs.tt.util.Target rally)
        {
            if (quarters.isDead())
            {
                return 0;
            }
            int available = quarters.getUnitContainer().getNumSupplies() - com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty];
            if (available <= 0)
            {
                return 0;
            }
            int ordered = java.lang.Math.min(num_peons, available);
            quarters.setRallyPoint(rally);
            getOwner().deployUnits(quarters, com.oddlabs.tt.model.DeployType.PEON, ordered);
            return ordered;
        }
        // Java: 4750
        public com.oddlabs.tt.model.Selectable[] firstN(com.oddlabs.tt.model.Selectable[] list, int n)
        {
            n = java.lang.Math.min(n, list.Length);
            com.oddlabs.tt.model.Selectable[] result = com.oddlabs.tt.model.Selectable.newArray(n);
            java.lang.JSystem.arraycopy(list, 0, result, 0, n);
            return result;
        }
        // Java: 4757
        public com.oddlabs.tt.model.Selectable[] lastN(com.oddlabs.tt.model.Selectable[] list, int n)
        {
            n = java.lang.Math.min(n, list.Length);
            com.oddlabs.tt.model.Selectable[] result = com.oddlabs.tt.model.Selectable.newArray(n);
            java.lang.JSystem.arraycopy(list, list.Length - n, result, 0, n);
            return result;
        }
        // Java: 4764
        public com.oddlabs.tt.model.Selectable[] getPeons(int min_num_peons)
        {
            com.oddlabs.tt.model.Selectable[] idle = getIdlePeons();
            int idleCount = (idle != null ? idle.Length : 0);
            java.util.List peons = new java.util.ArrayList();
            if (idle != null)
            {
                peons.addAll(java.util.Arrays.asList(idle));
            }
            int needed = java.lang.Math.max(min_num_peons - idleCount, 0);
            if (needed > 0)
            {
                foreach (com.oddlabs.tt.model.Selectable[] group in new com.oddlabs.tt.model.Selectable[][] { getGatherIronPeons(), getGatherRockPeons(), getGatherTreePeons(), getGatherRubberPeons() })
                {
                    if (group == null)
                    {
                        continue;
                    }
                    foreach (com.oddlabs.tt.model.Selectable peon in group)
                    {
                        if (peons.size() >= min_num_peons)
                        {
                            break;
                        }
                        peons.add(peon);
                    }
                    if (peons.size() >= min_num_peons)
                    {
                        break;
                    }
                }
            }
            return ((com.oddlabs.tt.model.Selectable[])peons.toArray(com.oddlabs.tt.model.Selectable.newArray(peons.size())));
        }
        // Java: 4808
        public int numWeapons(com.oddlabs.tt.model.Building armory)
        {
            return armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies() + armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() + armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
        }
        public const int WEAPON_STOCK_TARGET_MULTIPLIER = 3;
        public const int WEAPON_BUILD_BATCH = 10;
        // Java: 4832
        public void nodeBuildWeapons()
        {
            if (getArmory() == null)
            {
                return;
            }
            int target = com.oddlabs.tt.player.AdvancedAI.MIN_WEAPONS_IN_STOCK[difficulty] * com.oddlabs.tt.player.AdvancedAI.WEAPON_STOCK_TARGET_MULTIPLIER;
            foreach (com.oddlabs.tt.model.Selectable a in getArmory())
            {
                if (!((a is com.oddlabs.tt.model.Building armory)) || armory.isDead())
                {
                    continue;
                }
                if (numWeapons(armory) >= target)
                {
                    continue;
                }
                bool already_building = armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumOrders() > 0 || armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumOrders() > 0 || armory.getBuildSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumOrders() > 0;
                if (already_building)
                {
                    continue;
                }
                armory.buildWeapons(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), com.oddlabs.tt.player.AdvancedAI.WEAPON_BUILD_BATCH, false);
                armory.buildWeapons(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), com.oddlabs.tt.player.AdvancedAI.WEAPON_BUILD_BATCH, false);
                armory.buildWeapons(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), com.oddlabs.tt.player.AdvancedAI.WEAPON_BUILD_BATCH, false);
            }
        }
        // Java: 4860
        public com.oddlabs.tt.player.Player chooseFocusEnemy()
        {
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return null;
            }
            int hx = home.getGridX();
            int hy = home.getGridY();
            com.oddlabs.tt.player.Player best = null;
            double best_score = java.lang.Double.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in getOwner().getWorld().getPlayers())
            {
                if (!getOwner().isEnemy(p) || p.getUnits().getSet().isEmpty())
                {
                    continue;
                }
                int ex = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartX());
                int ey = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(p.getStartY());
                int dx = ex - hx;
                int dy = ey - hy;
                double dist = java.lang.Math.sqrt(dx * dx + dy * dy);
                int strength = cachedPlayerStatus(p);
                double score = dist * (1.0d + strength / 1000.0d);
                if (score < best_score)
                {
                    best_score = score;
                    best = p;
                }
            }
            return best;
        }
        // Java: 4899
        public com.oddlabs.tt.util.Target findNearestOfPlayer(com.oddlabs.tt.player.Player target_player, int start_x, int start_y, bool buildings_only)
        {
            com.oddlabs.tt.model.Selectable best = null;
            int best_dist_sqr = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(target_player.getUnits().getSet()))
            {
                if (s.isDead() || (buildings_only && !((s is com.oddlabs.tt.model.LandBuilding))))
                {
                    continue;
                }
                int dx = s.getGridX() - start_x;
                int dy = s.getGridY() - start_y;
                int dist_sqr = dx * dx + dy * dy;
                if (dist_sqr < best_dist_sqr)
                {
                    best_dist_sqr = dist_sqr;
                    best = s;
                }
            }
            return best;
        }
        public const float ENEMY_TOWER_DEFENSE_RADIUS = 40.0f;
        public const int WARRIORS_PER_TOWER_TO_ENGAGE = 7;
        // Java: 4933
        public int countNearbyEnemyTowers(int grid_x, int grid_y)
        {
            float world_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float world_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(world_x, world_y, com.oddlabs.tt.player.AdvancedAI.ENEMY_TOWER_DEFENSE_RADIUS, null, typeof(com.oddlabs.tt.model.Building));
            getUnitGrid().scan(filter, grid_x, grid_y);
            int count = 0;
            foreach (com.oddlabs.tt.model.Building building in java.lang.JIter.of(filter.getResult()))
            {
                if (!building.isDead() && isTowerTemplate(building) && getOwner().isEnemy(building.getOwner()))
                {
                    count++;
                }
            }
            return count;
        }
        // Java: 4949
        public com.oddlabs.tt.util.Target findTarget(int start_x, int start_y)
        {
            return findTarget(start_x, start_y, java.lang.Integer.MAX_VALUE);
        }
        // Java: 4956
        public com.oddlabs.tt.util.Target findTarget(int start_x, int start_y, int num_warriors)
        {
            com.oddlabs.tt.player.Player focus = chooseFocusEnemy();
            com.oddlabs.tt.util.Target best_building = default;
            com.oddlabs.tt.util.Target best_target = default;
            if (focus != null)
            {
                best_building = findNearestOfPlayer(focus, start_x, start_y, true);
                best_target = findNearestOfPlayer(focus, start_x, start_y, false);
            }
            else
            {
                best_building = getOwner().findNearestEnemyBuilding(start_x, start_y);
                best_target = getOwner().findNearestEnemy(start_x, start_y);
            }
            if (best_building != null && countNearbyEnemyTowers(best_building.getGridX(), best_building.getGridY()) * com.oddlabs.tt.player.AdvancedAI.WARRIORS_PER_TOWER_TO_ENGAGE > num_warriors)
            {
                if (best_target != null)
                {
                    return best_target;
                }
            }
            if (best_building == null)
            {
                return best_target;
            }
            if (best_target == null)
            {
                return null;
            }
            int squared_dist_building = (best_building.getGridX() - start_x) * (best_building.getGridX() - start_x) + (best_building.getGridY() - start_y) * (best_building.getGridY() - start_y);
            int squared_dist_target = (best_target.getGridX() - start_x) * (best_target.getGridX() - start_x) + (best_target.getGridY() - start_y) * (best_target.getGridY() - start_y);
            return (squared_dist_target < squared_dist_building / 2 ? best_target : best_building);
        }
        // Java: 4992
        public bool buildBuilding(int building_type, com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
            return buildBuilding(building_type, selection, grid_x, grid_y, 40);
        }
        // Java: 4996
        public bool buildBuilding(int building_type, com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, int range)
        {
            com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(getUnitGrid(), getOwner().getRace().getBuildingTemplate(building_type), range, true);
            getUnitGrid().scan(filter, grid_x, grid_y);
            java.util.List target_list = filter.getResult();
            if (!target_list.isEmpty())
            {
                com.oddlabs.tt.util.Target target = ((com.oddlabs.tt.util.Target)target_list.getFirst());
                getOwner().placeBuilding(selection, building_type, target.getGridX(), target.getGridY(), false);
                return true;
            }
            else
            {
                return false;
            }
        }
        static AdvancedAI()
        {
            AdvancedAI.DEFENSE_FACTOR = new float[] { 0.5f, 1.0f, 1.5f, 2.0f };
            AdvancedAI.MIN_UNITS_BUILDING_WEAPONS = new int[] { 5, 10, 15, 20 };
            AdvancedAI.MIN_WEAPONS_IN_STOCK = new int[] { 25, 15, 10, 5 };
            AdvancedAI.MIN_UNITS_REPRODUCING = new int[] { 5, 8, 14, 18 };
            AdvancedAI.MAX_UNITS_GATHERING_TREE = new int[] { 2, 5, 30, 50 };
            AdvancedAI.MAX_UNITS_GATHERING_ROCK = new int[] { 1, 3, 8, 12 };
            AdvancedAI.MAX_UNITS_GATHERING_IRON = new int[] { 1, 3, 25, 50 };
            AdvancedAI.MAX_UNITS_GATHERING_RUBBER = new int[] { 1, 3, 10, 15 };
            AdvancedAI.UNITS_PER_TOWER = new int[][] { new int[] { 80, 60, 40, 20 }, new int[] { 200, 120, 80, 40 }, new int[] { 320, 180, 120, 60 }, new int[] { 440, 240, 160, 80 }, new int[] { 560, 300, 200, 100 }, new int[] { 680, 360, 240, 120 }, new int[] { 800, 420, 280, 140 }, new int[] { 920, 480, 320, 160 }, new int[] { 1040, 540, 360, 180 }, new int[] { 1160, 600, 400, 200 }, new int[] { 1280, 660, 440, 220 }, new int[] { 1400, 720, 480, 240 }, new int[] { 1520, 780, 520, 260 }, new int[] { 1640, 840, 560, 280 }, new int[] { 1760, 900, 600, 300 }, new int[] { 1880, 960, 640, 320 }, new int[] { 2000, 1020, 680, 340 }, new int[] { 2120, 1080, 720, 360 }, new int[] { 2240, 1140, 760, 380 }, new int[] { 2360, 1200, 800, 400 }, new int[] { 2480, 1260, 840, 420 }, new int[] { 2600, 1320, 880, 440 }, new int[] { 2720, 1380, 920, 460 }, new int[] { 2840, 1440, 960, 480 }, new int[] { 2960, 1500, 1000, 500 }, new int[] { 3080, 1560, 1040, 520 }, new int[] { 3200, 1620, 1080, 540 }, new int[] { 3320, 1680, 1120, 560 }, new int[] { 3440, 1740, 1160, 580 }, new int[] { 3560, 1800, 1200, 600 }, new int[] { 3680, 1860, 1240, 620 }, new int[] { 3800, 1920, 1280, 640 }, new int[] { 3920, 1980, 1320, 660 }, new int[] { 4040, 2040, 1360, 680 }, new int[] { 4160, 2100, 1400, 700 }, new int[] { 4280, 2160, 1440, 720 }, new int[] { 4400, 2220, 1480, 740 }, new int[] { 4520, 2280, 1520, 760 }, new int[] { 4640, 2340, 1560, 780 }, new int[] { 4760, 2400, 1600, 800 }, new int[] { 4880, 2460, 1640, 820 }, new int[] { 5000, 2520, 1680, 840 }, new int[] { 5120, 2580, 1720, 860 }, new int[] { 5240, 2640, 1760, 880 }, new int[] { 5360, 2700, 1800, 900 }, new int[] { 5480, 2760, 1840, 920 }, new int[] { 5600, 2820, 1880, 940 }, new int[] { 5720, 2880, 1920, 960 }, new int[] { 5840, 2940, 1960, 980 }, new int[] { 5960, 3000, 2000, 1000 }, new int[] { 6080, 3060, 2040, 1020 }, new int[] { 6200, 3120, 2080, 1040 }, new int[] { 6320, 3180, 2120, 1060 }, new int[] { 6440, 3240, 2160, 1080 }, new int[] { 6560, 3300, 2200, 1100 }, new int[] { 6680, 3360, 2240, 1120 }, new int[] { 6800, 3420, 2280, 1140 }, new int[] { 6920, 3480, 2320, 1160 }, new int[] { 7040, 3540, 2360, 1180 }, new int[] { 7160, 3600, 2400, 1200 }, new int[] { 7280, 3660, 2440, 1220 }, new int[] { 7400, 3720, 2480, 1240 }, new int[] { 7520, 3780, 2520, 1260 }, new int[] { 7640, 3840, 2560, 1280 }, new int[] { 7760, 3900, 2600, 1300 }, new int[] { 7880, 3960, 2640, 1320 }, new int[] { 8000, 4020, 2680, 1340 }, new int[] { 8120, 4080, 2720, 1360 }, new int[] { 8240, 4140, 2760, 1380 }, new int[] { 8360, 4200, 2800, 1400 }, new int[] { 8480, 4260, 2840, 1420 }, new int[] { 8600, 4320, 2880, 1440 }, new int[] { 8720, 4380, 2920, 1460 }, new int[] { 8840, 4440, 2960, 1480 }, new int[] { 8960, 4500, 3000, 1500 }, new int[] { 9080, 4560, 3040, 1520 }, new int[] { 9200, 4620, 3080, 1540 }, new int[] { 9320, 4680, 3120, 1560 }, new int[] { 9440, 4740, 3160, 1580 }, new int[] { 9560, 4800, 3200, 1600 }, new int[] { 9680, 4860, 3240, 1620 }, new int[] { 9800, 4920, 3280, 1640 }, new int[] { 9920, 4980, 3320, 1660 }, new int[] { 10040, 5040, 3360, 1680 }, new int[] { 10160, 5100, 3400, 1700 }, new int[] { 10280, 5160, 3440, 1720 }, new int[] { 10400, 5220, 3480, 1740 }, new int[] { 10520, 5280, 3520, 1760 }, new int[] { 10640, 5340, 3560, 1780 }, new int[] { 10760, 5400, 3600, 1800 }, new int[] { 10880, 5460, 3640, 1820 }, new int[] { 11000, 5520, 3680, 1840 }, new int[] { 11120, 5580, 3720, 1860 }, new int[] { 11240, 5640, 3760, 1880 }, new int[] { 11360, 5700, 3800, 1900 }, new int[] { 11480, 5760, 3840, 1920 }, new int[] { 11600, 5820, 3880, 1940 }, new int[] { 11720, 5880, 3920, 1960 }, new int[] { 11840, 5940, 3960, 1980 }, new int[] { 11960, 6000, 4000, 2000 }, new int[] { 12080, 6060, 4040, 2020 }, new int[] { 12200, 6120, 4080, 2040 }, new int[] { 12320, 6180, 4120, 2060 }, new int[] { 12440, 6240, 4160, 2080 }, new int[] { 12560, 6300, 4200, 2100 }, new int[] { 12680, 6360, 4240, 2120 }, new int[] { 12800, 6420, 4280, 2140 }, new int[] { 12920, 6480, 4320, 2160 }, new int[] { 13040, 6540, 4360, 2180 }, new int[] { 13160, 6600, 4400, 2200 }, new int[] { 13280, 6660, 4440, 2220 }, new int[] { 13400, 6720, 4480, 2240 }, new int[] { 13520, 6780, 4520, 2260 }, new int[] { 13640, 6840, 4560, 2280 }, new int[] { 13760, 6900, 4600, 2300 }, new int[] { 13880, 6960, 4640, 2320 }, new int[] { 14000, 7020, 4680, 2340 }, new int[] { 14120, 7080, 4720, 2360 }, new int[] { 14240, 7140, 4760, 2380 }, new int[] { 14360, 7200, 4800, 2400 }, new int[] { 14480, 7260, 4840, 2420 } };
            AdvancedAI.strength_cache = new java.util.LinkedHashMap();
            AdvancedAI.last_cache_refresh_tick = java.lang.Integer.MIN_VALUE;
            AdvancedAI.last_cache_world = null;
            AdvancedAI.status_cache = new java.util.LinkedHashMap();
            AdvancedAI.ADAPT_INTERVAL = new float[] { 24.0f, 18.0f, 12.0f, 8.0f };
            AdvancedAI.ADAPT_UP = new float[] { 1.45f, 1.35f, 1.25f, 1.15f };
            AdvancedAI.ADAPT_DWELL = new float[] { 150.0f, 120.0f, 90.0f, 60.0f };
            AdvancedAI.ADAPT_CONFIRM = new int[] { 2, 2, 1, 1 };
            AdvancedAI.CHIEFTAIN_TRAINING_RESERVE = new int[] { 5, 15, 25, 30 };
        }
        private void init_AdvancedAI()
        {
            this.warrior_positions = new java.util.LinkedHashMap();
            this.chieftain_idle_time = 0.0f;
            this.chief_hitrun_units = new java.util.LinkedHashSet();
            this.patrol_units = new java.util.LinkedHashSet();
            this.escort_units = new java.util.LinkedHashSet();
            this.patrol_waypoint_index = 0;
            this.outpost_built = false;
            this.occupied_zones = new java.util.ArrayList();
            this.occupied_zones_cleanup_cooldown = 0.0f;
            this.attacking_units = new java.util.LinkedHashSet();
            this.resource_towers_built = 0;
            this.ship_unloading = new java.util.LinkedHashMap();
            this.ship_load_tick = new java.util.LinkedHashMap();
            this.ship_ready_since = new java.util.LinkedHashMap();
            this.ship_departures_failed = new java.util.LinkedHashMap();
            this.ship_sail_point = new java.util.LinkedHashMap();
            this.ship_last_pos = new java.util.LinkedHashMap();
            this.ship_hop = new java.util.LinkedHashMap();
            this.ship_exploration = new java.util.LinkedHashMap();
            this.NUM_WARRIORS = new int[] { 3, 7, 14, 30 };
            this.NUM_WARRIORS_INCREASE = new int[] { 2, 5, 13, 21 };
            this.NUM_WARRIORS_MAX = new int[] { 50, 100, 250, 500 };
            this.NUM_WARRIORS_FOR_CHIEFTAIN = new int[] { 80, 40, 25, 15 };
            this.defense_target = null;
            this.time_since_attack = 0.0f;
            this.tracked_main_attack_target = null;
            this.main_attack_target_check_cooldown = 0.0f;
            this.stuck_check_interval = 0.0f;
            this.pending_beacon = null;
            this.committed_this_tick = new java.util.LinkedHashSet();
            this.best_move_cooldown = 0.0f;
            this.multi_front_cooldown = 0.0f;
            this.favorable_attack_cooldown = 0.0f;
            this.closest_player_attack_cooldown = 0.0f;
            this.recall_stranded_cooldown = 0.0f;
            this.koth_check_cooldown = 0.0f;
            this.main_attack_wave_count = 0;
            this.ally_outpost_built = false;
            this.time_since_adaptive_check = 0.0f;
            this.armory_deploy_rotation = 0;
            this.match_elapsed_seconds = 0.0f;
            this.tier_trail = new java.lang.StringBuilder();
            this.first_attack_recorded = false;
            this.time_since_last_recorded_wave = com.oddlabs.tt.player.AdvancedAI.ATTACK_WAVE_COOLDOWN_SECONDS;
            this.observed_human_opponent = null;
            this.cached_map_size_label = null;
            this.under_construction_cache_tick = java.lang.Integer.MIN_VALUE;
            this.towers_under_construction_cache = java.util.Set_.of();
            this.quarters_under_construction_cache = java.util.Set_.of();
            this.armories_under_construction_cache = java.util.Set_.of();
            this.transfer_armory_index = 0;
        }
        public AdvancedAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, int difficulty) : this(owner, unit_info, difficulty, false)
        {
        }
        public AdvancedAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, int difficulty, bool adaptive) : this(owner, unit_info, difficulty, adaptive, (-1))
        {
        }
        public AdvancedAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, int difficulty, bool adaptive, int shared_seed_difficulty) : base(owner, unit_info)
        {
            init_AdvancedAI();
            this.adaptive = adaptive;
            this.difficulty = (!adaptive ? difficulty : java.lang.Math.clamp(difficulty, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_EASY, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_INSANE));
            this.start_difficulty = this.difficulty;
            this.tier = this.difficulty;
            this.top_tier = this.tier;
            if (adaptive)
            {
                int open = com.oddlabs.tt.player.AdaptiveAIProfile.unpackOpeningTier(shared_seed_difficulty, this.difficulty);
                if (open < 0)
                {
                    open = com.oddlabs.tt.player.AdaptiveAIProfile.get().openingTier(this.difficulty);
                }
                if (open == com.oddlabs.tt.player.AdvancedAI.TIER_EXPERT && owner.getWorld().isExtended())
                {
                    open = com.oddlabs.tt.player.AdvancedAI.TIER_GAUNTLET;
                }
                if (open != tier)
                {
                    setTier(open);
                }
            }
            if (adaptive)
            {
                this.selected_context = com.oddlabs.tt.player.PlayerStyleContext.fromAggression(com.oddlabs.tt.player.PlayerStyleProfile.get().getAggression());
                this.selected_behavior = com.oddlabs.tt.player.AiBehaviorBandit.get().selectArm(selected_context, this.difficulty);
            }
            else
            {
                this.selected_context = null;
                this.selected_behavior = null;
            }
        }
    }
}

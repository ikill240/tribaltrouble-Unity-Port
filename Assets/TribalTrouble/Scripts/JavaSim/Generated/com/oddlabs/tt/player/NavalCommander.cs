// Translated from NavalCommander.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 39
    public class NavalCommander : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float PERIOD = 2.0f;
        public const float START_TIME = 180.0f;
        public const int ROWERS = 16;
        public const int LAUNCH_WARRIORS = 20;
        public const int LAUNCH_WARRIORS_MIN = 8;
        public const float LAUNCH_WAIT = 120.0f;
        public const float UNLOAD_SECONDS = 30.0f;
        public const float LOAD_PERIOD = 25.0f;
        public const float JAM_SECONDS = 18.0f;
        public const int SHIP_BUILDERS = 8;
        public const int HOME_RANGE = 50;
        public const int ENGAGE_RANGE = 45;
        public const int ARRIVAL_RANGE = 60;
        public com.oddlabs.tt.player.Player owner;
        public float time;
        public float next_tick;
        public java.util.Map voyage;
        public java.util.Map aim;
        public java.util.Map unloading;
        public java.util.Map loaded_at;
        public java.util.Map ready_since;
        public java.util.Map last_pos;
        public java.util.Map fails;
        public java.util.Map moved;
        public java.util.Map nudges;
        public java.util.Map landings;
        public java.util.Map landing_island;
        // Java: 70
        public static void attach(com.oddlabs.tt.player.Player owner)
        {
            if (owner.getWorld().isArchipelago() && !owner.hasNavalCommander())
            {
                owner.setNavalCommander();
                new com.oddlabs.tt.player.NavalCommander(owner);
            }
        }
        // Java: 82
        public void animate(float t)
        {
            time += t;
            if (time < next_tick)
            {
                return;
            }
            next_tick = (float)(time + com.oddlabs.tt.player.NavalCommander.PERIOD);
            if (!owner.isAlive())
            {
                owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
                return;
            }
            if (time < com.oddlabs.tt.player.NavalCommander.START_TIME)
            {
                return;
            }
            try
            {
                tick();
            }
            catch (System.Exception e)
            {
            }
        }
        // Java: 103
        public java.util.List ships()
        {
            java.util.List r = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead())
                {
                    r.add(ship);
                }
            }
            return r;
        }
        // Java: 111
        public com.oddlabs.tt.model.LandBuilding building(int template)
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead() && b.isComplete() && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() == template)
                {
                    return b;
                }
            }
            return null;
        }
        // Java: 118
        public com.oddlabs.tt.model.LandBuilding home()
        {
            com.oddlabs.tt.model.LandBuilding q = building(com.oddlabs.tt.model.Race.BUILDING_QUARTERS);
            return (q != null ? q : building(com.oddlabs.tt.model.Race.BUILDING_ARMORY));
        }
        // Java: 123
        public int enemies()
        {
            int n = 0;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (((object)p != (object)owner) && owner.isEnemy(p) && p.isAlive())
                {
                    n++;
                }
            }
            return n;
        }
        // Java: 131
        public java.util.List woodcutters()
        {
            java.util.List r = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && (u.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController g) && ((object)g.getSupplyType() == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                {
                    r.add(u);
                }
            }
            return r;
        }
        // Java: 140
        public java.util.List idle(bool warriors_v, com.oddlabs.tt.model.Building near, int range)
        {
            bool free = warriors_v && !enemyOnIsland(near.getIslandId());
            java.util.List r = new java.util.ArrayList();
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(owner.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && ((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController) || free) && ((warriors_v ? u.isWarrior() && !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) && (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW) || u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MELEE)) : u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))) && (com.oddlabs.tt.player.NavalCommander.within(u, near, range) || owner.getWorld().getUnitGrid().getIslandId(u.getGridX(), u.getGridY()) == near.getIslandId()))
                {
                    r.add(u);
                }
            }
            return r;
        }
        // Java: 156
        public bool enemyOnIsland(int island)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (!owner.isEnemy(p) || !p.isAlive())
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if (!s.isDead() && !((s is com.oddlabs.tt.model.Unit u) && u.isMounted()) && grid.getIslandId(s.getGridX(), s.getGridY()) == island)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 168
        public static bool within(com.oddlabs.tt.model.Selectable a, com.oddlabs.tt.model.Selectable b, int cells)
        {
            int dx = a.getGridX() - b.getGridX();
            int dy = a.getGridY() - b.getGridY();
            return dx * dx + dy * dy <= cells * cells;
        }
        // Java: 173
        public bool atHome(com.oddlabs.tt.model.Ship ship)
        {
            com.oddlabs.tt.model.LandBuilding h = home();
            return h != null && com.oddlabs.tt.player.NavalCommander.within(ship, h, com.oddlabs.tt.player.NavalCommander.HOME_RANGE);
        }
        // Java: 178
        public int warriors(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getShipHR().countUnits() - ship.getShipHR().countPeons();
        }
        // Java: 182
        public bool tooDamaged(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getHitPoints() < (float)(0.35f * ship.getBuildingTemplate().getMaxHitPoints());
        }
        // Java: 188
        public void tick()
        {
            com.oddlabs.tt.model.LandBuilding home_v = home();
            if (home_v == null || enemies() == 0)
            {
                return;
            }
            java.util.List fleet = ships();
            buildShip(fleet, home_v);
            foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(fleet))
            {
                if (ship.isComplete())
                {
                    sail(ship, home_v);
                }
            }
        }
        // Java: 199
        public void buildShip(java.util.List fleet, com.oddlabs.tt.model.LandBuilding home_v)
        {
            com.oddlabs.tt.model.Ship building_v = null;
            foreach (com.oddlabs.tt.model.Ship s in java.lang.JIter.of(fleet))
            {
                if (!s.isComplete())
                {
                    building_v = s;
                }
            }
            int complete = 0;
            foreach (com.oddlabs.tt.model.Ship s_2 in java.lang.JIter.of(fleet))
            {
                if (s_2.isComplete() && !tooDamaged(s_2))
                {
                    complete++;
                }
            }
            if (building_v == null && complete >= java.lang.Math.min(4, enemies() + 2))
            {
                return;
            }
            java.util.List wood = woodcutters();
            if (wood.size() <= 2)
            {
                wood = new java.util.ArrayList(wood);
                foreach (com.oddlabs.tt.model.Selectable s_3 in java.lang.JIter.of(owner.getUnits().getSet()))
                {
                    if ((s_3 is com.oddlabs.tt.model.Unit u) && !u.isDead() && !u.isMounted() && u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD) && !u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC) && !wood.contains(u) && ((u.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController) || (u.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)))
                    {
                        wood.add(u);
                    }
                }
                if (wood.size() <= 4)
                {
                    return;
                }
            }
            int take = java.lang.Math.min(com.oddlabs.tt.player.NavalCommander.SHIP_BUILDERS, wood.size() - 2);
            com.oddlabs.tt.model.Selectable[] builders = ((com.oddlabs.tt.model.Selectable[])wood.subList(0, take).toArray(new com.oddlabs.tt.model.Selectable[0]));
            if (building_v != null)
            {
                int on = 0;
                foreach (com.oddlabs.tt.model.Selectable s_4 in java.lang.JIter.of(owner.getUnits().getSet()))
                {
                    if ((s_4 is com.oddlabs.tt.model.Unit u_2) && !u_2.isDead() && u_2.getPrimaryController() != null && !((u_2.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController)) && !((u_2.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController)) && com.oddlabs.tt.player.NavalCommander.within(u_2, building_v, 12))
                    {
                        on++;
                    }
                }
                if (on < 4)
                {
                    owner.setTarget(builders, building_v, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                return;
            }
            int[] site = com.oddlabs.tt.player.NavalCommander.seaShipSite(owner, home_v.getGridX(), home_v.getGridY());
            if (site != null)
            {
                owner.placeBuilding(builders, com.oddlabs.tt.model.Race.BUILDING_SHIP, site[0], site[1], false);
            }
        }
        // Java: 245
        public static int[] seaShipSite(com.oddlabs.tt.player.Player owner, int ox, int oy)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            com.oddlabs.tt.pathfinder.Region enemy_sea = null;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (!owner.isEnemy(p))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if ((s is com.oddlabs.tt.model.LandBuilding b) && !b.isDead())
                    {
                        enemy_sea = com.oddlabs.tt.player.NavalCommander.seaRegionNear(owner, b.getGridX(), b.getGridY(), 80);
                        if (enemy_sea != null)
                        {
                            break;
                        }
                    }
                }
                if (enemy_sea != null)
                {
                    break;
                }
            }
            int[] fallback = null;
            int[] fallback_wet = null;
            foreach (int range in new int[] { 40, 90, 160 })
            {
                com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(grid, owner.getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP), range, false);
                grid.scan(filter, ox, oy);
                int @checked = 0;
                foreach (com.oddlabs.tt.landscape.LandscapeTarget site in java.lang.JIter.of(filter.getResult()))
                {
                    if (fallback == null)
                    {
                        fallback = new int[] { site.getGridX(), site.getGridY() };
                    }
                    if (@checked++ > 40)
                    {
                        break;
                    }
                    com.oddlabs.tt.pathfinder.Region here = com.oddlabs.tt.player.NavalCommander.seaRegionNear(owner, site.getGridX(), site.getGridY(), 10);
                    if (here == null)
                    {
                        continue;
                    }
                    if (fallback_wet == null)
                    {
                        fallback_wet = new int[] { site.getGridX(), site.getGridY() };
                    }
                    if ((((object)here == (object)enemy_sea) || com.oddlabs.tt.pathfinder.PathFinder.findPathRegion(grid, here, enemy_sea) != null))
                    {
                        return new int[] { site.getGridX(), site.getGridY() };
                    }
                    if (enemy_sea == null)
                    {
                        return fallback_wet;
                    }
                }
            }
            return fallback_wet;
        }
        // Java: 288
        public static com.oddlabs.tt.pathfinder.Region seaRegionNear(com.oddlabs.tt.player.Player owner, int gx, int gy, int max)
        {
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            com.oddlabs.tt.landscape.HeightMap hm = owner.getWorld().getHeightMap();
            float deep = (float)(hm.getSeaLevelMeters() - 1.5f);
            int size = grid.getGridSize();
            for (int r = 0; r <= max; r += 2)
            {
                for (int i = -r; i <= r; i += 2)
                {
                    foreach (int[] c in new int[][] { new int[] { gx + i, gy - r }, new int[] { gx + i, gy + r }, new int[] { gx - r, gy + i }, new int[] { gx + r, gy + i } })
                    {
                        if (c[0] < 0 || c[1] < 0 || c[0] >= size || c[1] >= size)
                        {
                            continue;
                        }
                        if (hm.getNearestHeight(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(c[0]), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(c[1])) >= deep)
                        {
                            continue;
                        }
                        com.oddlabs.tt.pathfinder.Region reg = grid.getRegion(c[0], c[1], com.oddlabs.tt.pathfinder.UnitGrid.SEA);
                        if (reg != null)
                        {
                            return reg;
                        }
                    }
                }
            }
            return null;
        }
        // Java: 307
        public void load(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.LandBuilding home_v)
        {
            float? last = ((float?)loaded_at.get(ship));
            if (last != null && (float)(time - ((float)last)) < com.oddlabs.tt.player.NavalCommander.LOAD_PERIOD)
            {
                return;
            }
            loaded_at.put(ship, time);
            int peons = ship.getShipHR().countPeons();
            if (peons > com.oddlabs.tt.player.NavalCommander.ROWERS + 6)
            {
                ship.setRallyPoint(home_v);
                owner.deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, peons - com.oddlabs.tt.player.NavalCommander.ROWERS);
                return;
            }
            if (peons < com.oddlabs.tt.player.NavalCommander.ROWERS)
            {
                java.util.List idle_v = idle(false, home_v, 70);
                java.util.List wood = woodcutters();
                java.util.List send = new java.util.ArrayList(idle_v.subList(0, java.lang.Math.min(idle_v.size(), com.oddlabs.tt.player.NavalCommander.ROWERS - peons)));
                for (int i = 0; send.size() < com.oddlabs.tt.player.NavalCommander.ROWERS - peons && i < wood.size() - 4; i++)
                {
                    send.add(((com.oddlabs.tt.model.Unit)wood.get(i)));
                }
                if (!send.isEmpty())
                {
                    owner.setTarget(((com.oddlabs.tt.model.Selectable[])send.toArray(new com.oddlabs.tt.model.Selectable[0])), ship, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
            int w = warriors(ship);
            if (w < com.oddlabs.tt.player.NavalCommander.LAUNCH_WARRIORS + 6)
            {
                java.util.List idle_2 = idle(true, home_v, 90);
                if (!idle_2.isEmpty())
                {
                    owner.setTarget(((com.oddlabs.tt.model.Selectable[])idle_2.subList(0, java.lang.Math.min(idle_2.size(), com.oddlabs.tt.player.NavalCommander.LAUNCH_WARRIORS + 6 - w)).toArray(new com.oddlabs.tt.model.Selectable[0])), ship, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
        }
        // Java: 336
        public bool launchReady(com.oddlabs.tt.model.Ship ship)
        {
            int w = warriors(ship);
            if (w >= com.oddlabs.tt.player.NavalCommander.LAUNCH_WARRIORS)
            {
                return true;
            }
            if (w < com.oddlabs.tt.player.NavalCommander.LAUNCH_WARRIORS_MIN)
            {
                ready_since.remove(ship);
                return false;
            }
            float? since = ((float?)ready_since.putIfAbsent(ship, time));
            return owner.getUnitCountContainer().isSupplyFull() || since != null && (float)(time - ((float)since)) >= com.oddlabs.tt.player.NavalCommander.LAUNCH_WAIT;
        }
        // Java: 349
        public bool unjam(com.oddlabs.tt.model.Ship ship)
        {
            float[] last = ((float[])last_pos.get(ship));
            int x = ship.getGridX();
            int y = ship.getGridY();
            if (last == null || java.lang.Math.abs((float)(last[0] - x)) > 4 || java.lang.Math.abs((float)(last[1] - y)) > 4 || !ship.isMoving())
            {
                last_pos.put(ship, new float[] { x, y, time });
                return false;
            }
            if ((float)(time - last[2]) < com.oddlabs.tt.player.NavalCommander.JAM_SECONDS)
            {
                return false;
            }
            last_pos.put(ship, new float[] { x, y, time });
            int[] w = openWater(x + 20, y + 12);
            if (w != null)
            {
                owner.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), w[0], w[1], com.oddlabs.tt.model.Action.MOVE, false);
            }
            voyage.remove(ship);
            aim.remove(ship);
            return true;
        }
        // Java: 369
        public bool unstick(com.oddlabs.tt.model.Ship ship)
        {
            float[] m = ((float[])moved.get(ship));
            int x = ship.getGridX();
            int y = ship.getGridY();
            if (m == null || java.lang.Math.abs((float)(m[0] - x)) > 4 || java.lang.Math.abs((float)(m[1] - y)) > 4)
            {
                moved.put(ship, new float[] { x, y, time });
                return false;
            }
            bool wants_to_move = voyage.containsKey(ship) || !atHome(ship);
            if (!wants_to_move || unloading.containsKey(ship) || (float)(time - m[2]) < 40.0f)
            {
                return false;
            }
            moved.put(ship, new float[] { x, y, time });
            int n = ((int)((int?)nudges.merge(ship, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }))));
            int[][] dirs = new int[][] { new int[] { 1, 0 }, new int[] { 0, 1 }, new int[] { (-1), 0 }, new int[] { 0, (-1) }, new int[] { 1, 1 }, new int[] { (-1), 1 }, new int[] { (-1), (-1) }, new int[] { 1, (-1) } };
            int[] d = dirs[n % dirs.Length];
            int[] w = openWater(x + d[0] * 30, y + d[1] * 30);
            if (w != null)
            {
                owner.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), w[0], w[1], com.oddlabs.tt.model.Action.MOVE, false);
            }
            voyage.remove(ship);
            aim.remove(ship);
            return true;
        }
        // Java: 391
        public void sail(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.LandBuilding home_v)
        {
            if (unjam(ship) || unstick(ship))
            {
                return;
            }
            float? unload = ((float?)unloading.get(ship));
            if (unload != null)
            {
                if (warriors(ship) > 0 && (float)(time - ((float)unload)) < com.oddlabs.tt.player.NavalCommander.UNLOAD_SECONDS)
                {
                    return;
                }
                unloading.remove(ship);
                int? isl = ((int?)landing_island.get(ship));
                int tries = ((int)((int?)landings.getOrDefault(ship, 0)));
                if (warriors(ship) > 0 && isl != null && tries < 3)
                {
                    landings.put(ship, tries + 1);
                    com.oddlabs.tt.model.Selectable foe = enemyOn(((int)isl), ship);
                    if (foe != null)
                    {
                        int[] beach = beachNear(((int)isl), foe.getGridX(), foe.getGridY(), tries);
                        if (beach != null)
                        {
                            owner.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), beach[0], beach[1], com.oddlabs.tt.model.Action.MOVE, false);
                        }
                        else
                        {
                            owner.setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), foe, com.oddlabs.tt.model.Action.MOVE, true);
                        }
                        voyage.put(ship, isl);
                        aim.put(ship, (beach != null ? beach : new int[] { foe.getGridX(), foe.getGridY() }));
                        return;
                    }
                }
                landings.remove(ship);
                landing_island.remove(ship);
            }
            int? island = ((int?)voyage.get(ship));
            if (island != null)
            {
                if (ship.isMoving())
                {
                    return;
                }
                voyage.remove(ship);
                int[] a = ((int[])aim.remove(ship));
                bool arrived = a != null && (ship.getGridX() - a[0]) * (ship.getGridX() - a[0]) + (ship.getGridY() - a[1]) * (ship.getGridY() - a[1]) <= 40 * 40 || closeToIsland(ship, ((int)island));
                if (arrived)
                {
                    land(ship, ((int)island));
                    return;
                }
                int n = ((int)((int?)fails.merge(ship, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }))));
                if (n < 8 && a != null)
                {
                    int[] hop = openWater(ship.getGridX() + (a[0] - ship.getGridX()) / 3, ship.getGridY() + (a[1] - ship.getGridY()) / 3);
                    if (hop != null)
                    {
                        owner.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), hop[0], hop[1], com.oddlabs.tt.model.Action.MOVE, false);
                        voyage.put(ship, island);
                        aim.put(ship, a);
                        return;
                    }
                }
            }
            if (ship.isMoving())
            {
                return;
            }
            bool at_home = atHome(ship);
            if (tooDamaged(ship))
            {
                if (!at_home)
                {
                    owner.setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), home_v, com.oddlabs.tt.model.Action.MOVE, false);
                }
                else if (ship.getShipHR().countUnits() > 0)
                {
                    ship.setRallyPoint(home_v);
                    unloadWarriors(ship);
                    owner.deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, ship.getShipHR().countPeons());
                    unloading.put(ship, time);
                }
                return;
            }
            if (!at_home)
            {
                com.oddlabs.tt.model.Selectable foe_2 = owner.findNearestEnemyShip(ship.getGridX(), ship.getGridY());
                if (foe_2 == null || foe_2.isDead() || !com.oddlabs.tt.player.NavalCommander.within(ship, foe_2, com.oddlabs.tt.player.NavalCommander.ENGAGE_RANGE))
                {
                    foe_2 = owner.findNearestEnemyOnBeach(ship.getGridX(), ship.getGridY());
                }
                if (warriors(ship) > 0 && foe_2 != null && !foe_2.isDead() && com.oddlabs.tt.player.NavalCommander.within(ship, foe_2, com.oddlabs.tt.player.NavalCommander.ENGAGE_RANGE))
                {
                    owner.setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), foe_2, com.oddlabs.tt.model.Action.MOVE, true);
                    return;
                }
                owner.setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), home_v, com.oddlabs.tt.model.Action.MOVE, false);
                return;
            }
            load(ship, home_v);
            if (!launchReady(ship))
            {
                return;
            }
            int target = hostileIsland(home_v);
            if (target < 0)
            {
                return;
            }
            com.oddlabs.tt.model.Selectable foe_3 = enemyOn(target, home_v);
            int tx = default;
            int ty = default;
            if (foe_3 != null)
            {
                owner.setTarget(com.oddlabs.tt.model.Selectable.newArray(ship), foe_3, com.oddlabs.tt.model.Action.MOVE, true);
                tx = foe_3.getGridX();
                ty = foe_3.getGridY();
            }
            else
            {
                com.oddlabs.tt.landscape.IslandInfo info = owner.getWorld().getUnitGrid().getIslandInfo(target);
                if (info == null)
                {
                    return;
                }
                owner.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(ship), info.startX(), info.startY(), com.oddlabs.tt.model.Action.MOVE, false);
                tx = info.startX();
                ty = info.startY();
            }
            voyage.put(ship, target);
            aim.put(ship, new int[] { tx, ty });
            fails.remove(ship);
            ready_since.remove(ship);
        }
        // Java: 496
        public void land(com.oddlabs.tt.model.Ship ship, int island)
        {
            com.oddlabs.tt.model.LandBuilding home_v = home();
            com.oddlabs.tt.model.Selectable foe = enemyOn(island, (home_v != null ? (com.oddlabs.tt.model.Selectable)(home_v) : (com.oddlabs.tt.model.Selectable)(ship)));
            if (foe != null)
            {
                ship.setRallyPoint(new com.oddlabs.tt.landscape.LandscapeTarget(foe.getGridX(), foe.getGridY()));
            }
            unloadWarriors(ship);
            unloading.put(ship, time);
            landing_island.put(ship, island);
        }
        // Java: 506
        public void unloadWarriors(com.oddlabs.tt.model.Ship ship)
        {
            com.oddlabs.tt.model.ShipHR hr = ship.getShipHR();
            int rock = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon));
            int iron = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon));
            int rubber = hr.countUnitsOfType(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon));
            if (rock > 0)
            {
                owner.deployUnits(ship, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, rock);
            }
            if (iron > 0)
            {
                owner.deployUnits(ship, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, iron);
            }
            if (rubber > 0)
            {
                owner.deployUnits(ship, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, rubber);
            }
        }
        // Java: 518
        public int hostileIsland(com.oddlabs.tt.model.Building home_v)
        {
            int home_island = home_v.getIslandId();
            int best = (-1);
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (!owner.isEnemy(p))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if (!((s is com.oddlabs.tt.model.LandBuilding)) || s.isDead() || s.getIslandId() == home_island)
                    {
                        continue;
                    }
                    int dx = s.getGridX() - home_v.getGridX();
                    int dy = s.getGridY() - home_v.getGridY();
                    int d = dx * dx + dy * dy;
                    if (d < best_d)
                    {
                        best_d = d;
                        best = s.getIslandId();
                    }
                }
            }
            return best;
        }
        // Java: 539
        public com.oddlabs.tt.model.Selectable enemyOn(int island, com.oddlabs.tt.model.Selectable from)
        {
            com.oddlabs.tt.model.Selectable best = null;
            int best_d = java.lang.Integer.MAX_VALUE;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (!owner.isEnemy(p))
                {
                    continue;
                }
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
                {
                    if (s.isDead() || (s is com.oddlabs.tt.model.Ship) || s.getIslandId() != island)
                    {
                        continue;
                    }
                    int dx = s.getGridX() - from.getGridX();
                    int dy = s.getGridY() - from.getGridY();
                    int d = dx * dx + dy * dy + (((s is com.oddlabs.tt.model.LandBuilding) ? 0 : 200 * 200));
                    if (d < best_d)
                    {
                        best_d = d;
                        best = s;
                    }
                }
            }
            return best;
        }
        // Java: 559
        public bool closeToIsland(com.oddlabs.tt.model.Ship ship, int island)
        {
            com.oddlabs.tt.landscape.IslandInfo info = owner.getWorld().getUnitGrid().getIslandInfo(island);
            if (info == null)
            {
                return false;
            }
            int dx = ship.getGridX() - info.startX();
            int dy = ship.getGridY() - info.startY();
            return dx * dx + dy * dy <= com.oddlabs.tt.player.NavalCommander.ARRIVAL_RANGE * com.oddlabs.tt.player.NavalCommander.ARRIVAL_RANGE;
        }
        // Java: 569
        public int[] beachNear(int island, int gx, int gy, int attempt)
        {
            com.oddlabs.tt.landscape.HeightMap hm = owner.getWorld().getHeightMap();
            com.oddlabs.tt.pathfinder.UnitGrid grid = owner.getWorld().getUnitGrid();
            int size = grid.getGridSize();
            float sea = hm.getSeaLevelMeters();
            int found = 0;
            for (int r = 6; r <= 120; r += 3)
            {
                int steps = java.lang.Math.max(8, r);
                for (int k = 0; k < steps; k++)
                {
                    double a = 2 * java.lang.Math.PI * k / steps;
                    int x = gx + unchecked((int)(java.lang.Math.round(java.lang.StrictMath.cos(a) * r)));
                    int y = gy + unchecked((int)(java.lang.Math.round(java.lang.StrictMath.sin(a) * r)));
                    if (x < 8 || y < 8 || x >= size - 8 || y >= size - 8 || grid.getIslandId(x, y) != island)
                    {
                        continue;
                    }
                    float h = hm.getNearestHeight(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(x), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(y));
                    if (h < (float)(sea + 0.3f) || h > (float)(sea + 3.0f))
                    {
                        continue;
                    }
                    int wx = x + unchecked((int)(java.lang.Math.round(java.lang.StrictMath.cos(a) * 6)));
                    int wy = y + unchecked((int)(java.lang.Math.round(java.lang.StrictMath.sin(a) * 6)));
                    if (wx < 6 || wy < 6 || wx >= size - 6 || wy >= size - 6)
                    {
                        continue;
                    }
                    if (hm.getNearestHeight(com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(wx), com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(wy)) < (float)(sea - 1.5f) && found++ >= attempt * 3)
                    {
                        return new int[] { wx, wy };
                    }
                }
            }
            return null;
        }
        // Java: 595
        public int[] openWater(int gx, int gy)
        {
            com.oddlabs.tt.landscape.HeightMap hm = owner.getWorld().getHeightMap();
            int size = owner.getWorld().getUnitGrid().getGridSize();
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
        private void init_NavalCommander()
        {
            this.next_tick = com.oddlabs.tt.player.NavalCommander.PERIOD;
            this.voyage = new java.util.HashMap();
            this.aim = new java.util.HashMap();
            this.unloading = new java.util.HashMap();
            this.loaded_at = new java.util.HashMap();
            this.ready_since = new java.util.HashMap();
            this.last_pos = new java.util.HashMap();
            this.fails = new java.util.HashMap();
            this.moved = new java.util.HashMap();
            this.nudges = new java.util.HashMap();
            this.landings = new java.util.HashMap();
            this.landing_island = new java.util.HashMap();
        }
        public NavalCommander(com.oddlabs.tt.player.Player owner) : base()
        {
            init_NavalCommander();
            this.owner = owner;
            owner.getWorld().getAnimationManagerGameTime().registerAnimation(this);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}

// Translated from AI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 32
    public abstract class AI : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float SLEEP_SECONDS = 2.0f;
        public const float MIN_SLEEP_SECONDS = 5.0f;
        public com.oddlabs.tt.player.Player owner;
        public int INDEX_IDLE_PEONS;
        public int INDEX_IDLE_CHIEFTAINS;
        public int INDEX_IDLE_WARRIORS;
        public int INDEX_GATHER_TREE_PEONS;
        public int INDEX_GATHER_ROCK_PEONS;
        public int INDEX_GATHER_IRON_PEONS;
        public int INDEX_GATHER_RUBBER_PEONS;
        public int INDEX_ARMORY;
        public int INDEX_QUARTERS;
        public int INDEX_TOWERS;
        public int INDEX_SHIPS;
        public int INDEX_CONSTRUCTION_SITES;
        public int INDEX_PLACE_BUILDING_PEONS;
        public int INDEX_DEFENDING_UNITS;
        public com.oddlabs.tt.model.Selectable[][] lists;
        public bool armory_under_construction;
        public bool quarters_under_construction;
        public bool tower_under_construction;
        public bool ship_under_construction;
        public bool found_island;
        public float sleep_time;
        // Java: 146
        public virtual bool isArchipelago()
        {
            return getOwner().getWorld().getMapSize() == com.oddlabs.matchmaking.Game.SIZE_ARCHIPELAGO;
        }
        // Java: 150
        public virtual bool hasFoundIsland()
        {
            return found_island;
        }
        // Java: 154
        public virtual void setFoundIsland()
        {
            found_island = true;
        }
        // Java: 158
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return owner.getWorld().getUnitGrid();
        }
        // Java: 162
        public com.oddlabs.tt.player.Player getOwner()
        {
            return owner;
        }
        // Java: 166
        public void reclassify()
        {
            lists = getOwner().classifyUnits();
            classifyIndex(lists);
        }
        // Java: 171
        public com.oddlabs.tt.model.Selectable[] getIdlePeons()
        {
            return (INDEX_IDLE_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_IDLE_PEONS]);
        }
        // Java: 175
        public com.oddlabs.tt.model.Selectable[] getIdleChieftains()
        {
            return (INDEX_IDLE_CHIEFTAINS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_IDLE_CHIEFTAINS]);
        }
        // Java: 179
        public com.oddlabs.tt.model.Selectable[] getIdleWarriors()
        {
            return (INDEX_IDLE_WARRIORS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_IDLE_WARRIORS]);
        }
        // Java: 183
        public com.oddlabs.tt.model.Selectable[] getGatherTreePeons()
        {
            return (INDEX_GATHER_TREE_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_TREE_PEONS]);
        }
        // Java: 187
        public com.oddlabs.tt.model.Selectable[] getGatherRockPeons()
        {
            return (INDEX_GATHER_ROCK_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_ROCK_PEONS]);
        }
        // Java: 191
        public com.oddlabs.tt.model.Selectable[] getGatherIronPeons()
        {
            return (INDEX_GATHER_IRON_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_IRON_PEONS]);
        }
        // Java: 195
        public com.oddlabs.tt.model.Selectable[] getGatherRubberPeons()
        {
            return (INDEX_GATHER_RUBBER_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_RUBBER_PEONS]);
        }
        // Java: 199
        public com.oddlabs.tt.model.Selectable[] getArmory()
        {
            return (INDEX_ARMORY == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_ARMORY]);
        }
        // Java: 203
        public com.oddlabs.tt.model.Selectable[] getQuarters()
        {
            return (INDEX_QUARTERS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_QUARTERS]);
        }
        // Java: 207
        public com.oddlabs.tt.model.Selectable[] getTowers()
        {
            return (INDEX_TOWERS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_TOWERS]);
        }
        // Java: 211
        public com.oddlabs.tt.model.Selectable[] getShips()
        {
            return (INDEX_SHIPS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_SHIPS]);
        }
        // Java: 215
        public com.oddlabs.tt.model.Selectable[] getConstructionSites()
        {
            return (INDEX_CONSTRUCTION_SITES == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_CONSTRUCTION_SITES]);
        }
        // Java: 219
        public com.oddlabs.tt.model.Selectable[] getPlaceBuildingPeons()
        {
            return (INDEX_PLACE_BUILDING_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_PLACE_BUILDING_PEONS]);
        }
        // Java: 223
        public com.oddlabs.tt.model.Selectable[] getDefendingUnits()
        {
            return (INDEX_DEFENDING_UNITS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_DEFENDING_UNITS]);
        }
        // Java: 227
        public void classifyIndex(com.oddlabs.tt.model.Selectable[][] lists)
        {
            INDEX_IDLE_PEONS = (-1);
            INDEX_IDLE_CHIEFTAINS = (-1);
            INDEX_IDLE_WARRIORS = (-1);
            INDEX_GATHER_TREE_PEONS = (-1);
            INDEX_GATHER_ROCK_PEONS = (-1);
            INDEX_GATHER_IRON_PEONS = (-1);
            INDEX_GATHER_RUBBER_PEONS = (-1);
            INDEX_ARMORY = (-1);
            INDEX_QUARTERS = (-1);
            INDEX_TOWERS = (-1);
            INDEX_SHIPS = (-1);
            INDEX_CONSTRUCTION_SITES = (-1);
            INDEX_PLACE_BUILDING_PEONS = (-1);
            INDEX_DEFENDING_UNITS = (-1);
            for (int i = 0; i < lists.Length; i++)
            {
                com.oddlabs.tt.model.Selectable s = lists[i][0];
                if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.IdleController))
                {
                    if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD))
                    {
                        INDEX_IDLE_PEONS = i;
                    }
                    else if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        INDEX_IDLE_CHIEFTAINS = i;
                    }
                    else if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                    {
                        INDEX_IDLE_WARRIORS = i;
                    }
                }
                else if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController gc))
                {
                    System.Type supply_type = gc.getSupplyType();
                    if (((object)supply_type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                    {
                        INDEX_GATHER_TREE_PEONS = i;
                    }
                    else if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
                    {
                        INDEX_GATHER_ROCK_PEONS = i;
                    }
                    else if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
                    {
                        INDEX_GATHER_IRON_PEONS = i;
                    }
                    else if (((object)supply_type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
                    {
                        INDEX_GATHER_RUBBER_PEONS = i;
                    }
                }
                else if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.NullController))
                {
                    if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                    {
                        INDEX_ARMORY = i;
                        armory_under_construction = false;
                        getOwner().buildRockWeapons(((com.oddlabs.tt.model.Building)s), com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                        getOwner().buildIronWeapons(((com.oddlabs.tt.model.Building)s), com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                        getOwner().buildRubberWeapons(((com.oddlabs.tt.model.Building)s), com.oddlabs.tt.gui.BuildSpinner.INFINITE_LIMIT, true);
                    }
                    else if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
                    {
                        INDEX_QUARTERS = i;
                        quarters_under_construction = false;
                    }
                    else if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
                    {
                        INDEX_TOWERS = i;
                    }
                    else if (s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.SAIL))
                    {
                        INDEX_SHIPS = i;
                        ship_under_construction = false;
                    }
                    else
                    {
                        INDEX_CONSTRUCTION_SITES = i;
                    }
                }
                else if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.PlaceBuildingController))
                {
                    INDEX_PLACE_BUILDING_PEONS = i;
                }
                else if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.DefendController))
                {
                    INDEX_DEFENDING_UNITS = i;
                }
            }
            if (INDEX_CONSTRUCTION_SITES == (-1) && INDEX_PLACE_BUILDING_PEONS == (-1))
            {
                armory_under_construction = false;
                quarters_under_construction = false;
                tower_under_construction = false;
                ship_under_construction = false;
            }
        }
        // Java: 296
        public bool armoryUnderConstruction()
        {
            return armory_under_construction;
        }
        // Java: 300
        public void setArmoryUnderConstruction(bool armory_under_construction)
        {
            this.armory_under_construction = armory_under_construction;
        }
        // Java: 304
        public bool quartersUnderConstruction()
        {
            return quarters_under_construction;
        }
        // Java: 308
        public void setQuartersUnderConstruction(bool quarters_under_construction)
        {
            this.quarters_under_construction = quarters_under_construction;
        }
        // Java: 312
        public bool towerUnderConstruction()
        {
            return tower_under_construction;
        }
        // Java: 316
        public void setTowerUnderConstruction(bool tower_under_construction)
        {
            this.tower_under_construction = tower_under_construction;
        }
        // Java: 320
        public bool shipUnderConstruction()
        {
            return ship_under_construction;
        }
        // Java: 324
        public void setShipUnderConstruction(bool ship_under_construction)
        {
            this.ship_under_construction = ship_under_construction;
        }
        // Java: 328
        public void reset()
        {
            sleep_time = (float)((float)(owner.getWorld().getRandom().nextFloat() * com.oddlabs.tt.player.AI.SLEEP_SECONDS) + com.oddlabs.tt.player.AI.MIN_SLEEP_SECONDS);
        }
        // Java: 332
        public bool shouldDoAction(float time)
        {
            sleep_time -= time;
            if (!com.oddlabs.tt.global.Globals.run_ai || sleep_time >= 0)
            {
                return false;
            }
            reset();
            return true;
        }
        // Java: 340
        public void manTowers(int num_towers)
        {
            reclassify();
            com.oddlabs.tt.model.Selectable[] towers = getTowers();
            com.oddlabs.tt.model.Selectable[] idle_warriors = getIdleWarriors();
            if (towers == null || idle_warriors == null)
            {
                return;
            }
            int length = java.lang.Math.min(idle_warriors.Length, towers.Length);
            for (int i = 0; i < length; i++)
            {
                owner.setTarget(com.oddlabs.tt.model.Selectable.newArray(idle_warriors[i]), towers[i], com.oddlabs.tt.model.Action.DEFAULT, false);
            }
        }
        // Java: 353
        public static int attackLandscape(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.util.Target target, int num_warriors)
        {
            int ordered = 0;
            com.oddlabs.tt.model.Selectable[][] lists = owner.classifyUnits();
            foreach (com.oddlabs.tt.model.Selectable[] list in lists)
            {
                com.oddlabs.tt.model.Selectable s = list[0];
                if ((s is com.oddlabs.tt.model.Unit unit) && !((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.WalkController) && (((com.oddlabs.tt.model.behaviour.WalkController)s.getPrimaryController())).isAgressive()))
                {
                    foreach (com.oddlabs.tt.model.Selectable thrower in list)
                    {
                        if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
                        {
                            owner.setLandscapeTarget(com.oddlabs.tt.model.Selectable.newArray(thrower), target.getGridX(), target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
                            ordered++;
                            if (ordered == num_warriors)
                            {
                                return ordered;
                            }
                        }
                    }
                }
            }
            return ordered;
        }
        // Java: 375
        public static com.oddlabs.tt.model.Unit getWarrior(com.oddlabs.tt.player.Player owner)
        {
            com.oddlabs.tt.model.Selectable[][] lists = owner.classifyUnits();
            foreach (com.oddlabs.tt.model.Selectable[] list in lists)
            {
                com.oddlabs.tt.model.Selectable s = list[0];
                if ((s is com.oddlabs.tt.model.Unit unit) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.THROW))
                {
                    return unit;
                }
            }
            return null;
        }
        // Java: 386
        public com.oddlabs.tt.util.Target getTarget(java.util.Random random)
        {
            float RADIUS = 30;
            float target_x = (float)(owner.getStartX() + (float)(((float)((float)(random.nextFloat() * 2) - 1)) * RADIUS));
            float target_y = (float)(owner.getStartY() + (float)(((float)((float)(random.nextFloat() * 2) - 1)) * RADIUS));
            return getUnitGrid().findGridTargets(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(target_x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(target_y), 1, false)[0];
        }
        private void init_AI()
        {
            this.armory_under_construction = false;
            this.quarters_under_construction = false;
            this.tower_under_construction = false;
            this.ship_under_construction = false;
            this.found_island = false;
        }
        public AI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info) : base()
        {
            init_AI();
            this.owner = owner;
            owner.getWorld().getAnimationManagerRealTime().registerAnimation(this);
            reset();
            if (unit_info != null)
            {
                int grid_start_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartX());
                int grid_start_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartY());
                if (isArchipelago())
                {
                    java.util.List ships = com.oddlabs.tt.model.Ship.newStartingShips(owner, grid_start_x, grid_start_y, unit_info.numPeonsAndWarriors());
                    com.oddlabs.tt.model.Ship ship = ((com.oddlabs.tt.model.Ship)ships.get(0));
                    for (int i = 0; i < unit_info.numPeons(); i++)
                    {
                        com.oddlabs.tt.model.Unit unit = new com.oddlabs.tt.model.Unit(owner, ship.getPositionX(), ship.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                        com.oddlabs.tt.model.Ship.board(ships, unit);
                    }
                    for (int i_2 = 0; i_2 < unit_info.numRockWarriors(); i_2++)
                    {
                        com.oddlabs.tt.model.Unit unit_2 = new com.oddlabs.tt.model.Unit(owner, ship.getPositionX(), ship.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                        com.oddlabs.tt.model.Ship.board(ships, unit_2);
                    }
                    for (int i_3 = 0; i_3 < unit_info.numIronWarriors(); i_3++)
                    {
                        com.oddlabs.tt.model.Unit unit_3 = new com.oddlabs.tt.model.Unit(owner, ship.getPositionX(), ship.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                        com.oddlabs.tt.model.Ship.board(ships, unit_3);
                    }
                    for (int i_4 = 0; i_4 < unit_info.numRubberWarriors(); i_4++)
                    {
                        com.oddlabs.tt.model.Unit unit_4 = new com.oddlabs.tt.model.Unit(owner, ship.getPositionX(), ship.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                        com.oddlabs.tt.model.Ship.board(ships, unit_4);
                    }
                    found_island = false;
                }
                else
                {
                    if (unit_info.hasQuarters())
                    {
                        owner.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, grid_start_x, grid_start_y);
                    }
                    if (unit_info.hasArmory())
                    {
                        owner.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, grid_start_x, grid_start_y);
                    }
                    for (int i_5 = 0; i_5 < unit_info.numTowers(); i_5++)
                    {
                        int center = owner.getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
                        int dx = center - grid_start_x;
                        int dy = center - grid_start_y;
                        float inv_dist = (float)(1.0f / ((float)(java.lang.Math.sqrt(dx * dx + dy * dy))));
                        int tx = java.lang.JC.f2i(((float)(grid_start_x + (float)((float)(10.0f * dx) * inv_dist))));
                        int ty = java.lang.JC.f2i(((float)(grid_start_y + (float)((float)(10.0f * dy) * inv_dist))));
                        owner.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, tx, ty);
                    }
                    java.util.Random random = new java.util.Random(42);
                    if (unit_info.hasChieftain())
                    {
                        com.oddlabs.tt.util.Target t = getTarget(random);
                        com.oddlabs.tt.model.Unit chieftain = new com.oddlabs.tt.model.Unit(owner, t.getPositionX(), t.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN));
                        owner.setActiveChieftain(chieftain);
                    }
                    for (int i_6 = 0; i_6 < unit_info.numPeons(); i_6++)
                    {
                        com.oddlabs.tt.util.Target t_2 = getTarget(random);
                        new com.oddlabs.tt.model.Unit(owner, t_2.getPositionX(), t_2.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                    }
                    for (int i_7 = 0; i_7 < unit_info.numRockWarriors(); i_7++)
                    {
                        com.oddlabs.tt.util.Target t_3 = getTarget(random);
                        new com.oddlabs.tt.model.Unit(owner, t_3.getPositionX(), t_3.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                    }
                    for (int i_8 = 0; i_8 < unit_info.numIronWarriors(); i_8++)
                    {
                        com.oddlabs.tt.util.Target t_4 = getTarget(random);
                        new com.oddlabs.tt.model.Unit(owner, t_4.getPositionX(), t_4.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                    }
                    for (int i_9 = 0; i_9 < unit_info.numRubberWarriors(); i_9++)
                    {
                        com.oddlabs.tt.util.Target t_5 = getTarget(random);
                        new com.oddlabs.tt.model.Unit(owner, t_5.getPositionX(), t_5.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                    }
                    found_island = true;
                }
            }
        }
        public abstract void animate(float p0);
        // mixin of default method Animated.updateChecksum
        public virtual void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}

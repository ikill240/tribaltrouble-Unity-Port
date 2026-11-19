// Translated from AdvancedAI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 35
    public class AdvancedAI : com.oddlabs.tt.player.AI
    {
        public const int DIFFICULTY_EASY = 0;
        public const int DIFFICULTY_NORMAL = 1;
        public const int DIFFICULTY_HARD = 2;
        public const int SCORE_PEON = 1;
        public const int SCORE_WARRIOR_ROCK = 4;
        public const int SCORE_WARRIOR_IRON = 5;
        public const int SCORE_WARRIOR_RUBBER = 10;
        public const int SCORE_CHIEFTAIN = 25;
        public static float[] DEFENSE_FACTOR;
        public static int[] MIN_UNITS_BUILDING_WEAPONS;
        public static int[] MIN_WEAPONS_IN_STOCK;
        public static int[] MIN_UNITS_REPRODUCING;
        public static int[] MAX_UNITS_GATHERING_TREE;
        public static int[] MAX_UNITS_GATHERING_ROCK;
        public static int[] MAX_UNITS_GATHERING_IRON;
        public static int[] MAX_UNITS_GATHERING_RUBBER;
        public static int[] UNITS_PER_TOWER1;
        public static int[] UNITS_PER_TOWER2;
        public const int SHIP_PEONS = 26;
        public const int SHIP_WARRIORS = 50;
        public const int SHIP_BUILDERS = 20;
        public const float SHIP_MIN_HEALTH = 0.5f;
        public const int SHIP_HOME_RANGE = 50;
        public const int FLEET_SIZE = 3;
        public int difficulty;
        public int[] NUM_WARRIORS;
        public int[] NUM_WARRIORS_INCREASE;
        public int[] NUM_WARRIORS_MAX;
        public int[] NUM_WARRIORS_FOR_CHIEFTAIN;
        public com.oddlabs.tt.landscape.LandscapeTarget defense_target;
        public com.oddlabs.tt.landscape.IslandInfo init_island;
        public int init_island_contact_x;
        public int init_island_contact_y;
        public bool home_is_safe;
        public com.oddlabs.tt.landscape.DensityMap density_map;
        // Java: 88
        public int getDifficulty()
        {
            return difficulty;
        }
        // Java: 92
        public override void animate(float t)
        {
            if (!shouldDoAction(t))
            {
                return;
            }
            reclassify();
            nodeDefendBase();
            reclassify();
            if (getOwner().getUnitCountContainer().getNumSupplies() > com.oddlabs.tt.player.AdvancedAI.UNITS_PER_TOWER2[difficulty])
            {
                nodeGuardTowers(2);
            }
            else if (getOwner().getUnitCountContainer().getNumSupplies() > com.oddlabs.tt.player.AdvancedAI.UNITS_PER_TOWER1[difficulty])
            {
                nodeGuardTowers(1);
            }
            if (isArchipelago())
            {
                density_map.update();
                if (hasFoundIsland())
                {
                    home_is_safe = !islandHasEnemies(init_island);
                    reclassify();
                    if (baseBuildingsDone())
                    {
                        nodeBuildShipAndLoad();
                    }
                    nodeUseShip();
                }
                else
                {
                    nodePickInitIsland();
                }
            }
            reclassify();
            nodeAttackWithWarriorsAndChieftain(NUM_WARRIORS[difficulty], NUM_WARRIORS[difficulty] >= NUM_WARRIORS_FOR_CHIEFTAIN[difficulty]);
            nodeAssignIdlePeons();
            if (getOwner().hasActiveChieftain())
            {
                getOwner().getRace().getChieftainAI().decide(getOwner().getChieftain());
            }
        }
        // Java: 126
        public void nodeDefendBase()
        {
            int enemy_score = 0;
            if (getQuarters() != null)
            {
                enemy_score = scanForEnemies(getQuarters()[0]);
            }
            if (getArmory() != null && enemy_score == 0)
            {
                enemy_score = scanForEnemies(getArmory()[0]);
            }
            enemy_score = java.lang.JC.f2i(((float)(com.oddlabs.tt.player.AdvancedAI.DEFENSE_FACTOR[difficulty] * enemy_score)));
            if (getDefendingUnits() != null)
            {
                foreach (com.oddlabs.tt.model.Selectable defendingUnit in getDefendingUnits())
                {
                    enemy_score -= getUnitScore(((com.oddlabs.tt.model.Unit)defendingUnit));
                }
            }
            if (enemy_score > 0)
            {
                nodeDeployArmy();
                nodeDefend(enemy_score);
            }
        }
        // Java: 146
        public void nodeDeployArmy()
        {
            if (getArmory() != null)
            {
                com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)getArmory()[0]);
                int num_units = armory.getUnitContainer().getNumSupplies() - com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_BUILDING_WEAPONS[difficulty];
                int num_weapons = numWeapons(armory) - com.oddlabs.tt.player.AdvancedAI.MIN_WEAPONS_IN_STOCK[difficulty];
                if (num_units <= 0 || num_weapons <= 0)
                {
                    return;
                }
                int num_warriors = java.lang.Math.min(num_units, num_weapons);
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
                num_units = armory.getUnitContainer().getNumSupplies();
                if (num_units > 0)
                {
                    getOwner().deployUnits(armory, com.oddlabs.tt.model.DeployType.PEON, num_units);
                }
            }
        }
        // Java: 182
        public void nodeDefend(int score)
        {
            java.util.List unit_list = new java.util.ArrayList();
            int result = 0;
            if (getIdleWarriors() != null && result < score)
            {
                result = addFromList(getIdleWarriors(), unit_list, result, score);
            }
            if (getIdlePeons() != null && result < score)
            {
                result = addFromList(getIdlePeons(), unit_list, result, score);
            }
            if (getGatherTreePeons() != null && result < score)
            {
                result = addFromList(getGatherTreePeons(), unit_list, result, score);
            }
            if (getGatherRockPeons() != null && result < score)
            {
                result = addFromList(getGatherRockPeons(), unit_list, result, score);
            }
            if (getGatherIronPeons() != null && result < score)
            {
                result = addFromList(getGatherIronPeons(), unit_list, result, score);
            }
            if (getGatherRubberPeons() != null && result < score)
            {
                result = addFromList(getGatherRubberPeons(), unit_list, result, score);
            }
            if (result > 0)
            {
                com.oddlabs.tt.model.Unit[] units = new com.oddlabs.tt.model.Unit[unit_list.size()];
                unit_list.toArray(units);
                getOwner().setLandscapeTarget(units, defense_target.getGridX(), defense_target.getGridY(), com.oddlabs.tt.model.Action.DEFEND, true);
            }
        }
        // Java: 213
        public int addFromList(com.oddlabs.tt.model.Selectable[] list, java.util.List new_list, int progress, int score)
        {
            int result = progress;
            foreach (com.oddlabs.tt.model.Selectable list1 in list)
            {
                com.oddlabs.tt.model.Unit unit = ((com.oddlabs.tt.model.Unit)list1);
                new_list.add(unit);
                result += getUnitScore(unit);
                if (result > score)
                {
                    break;
                }
            }
            return result;
        }
        // Java: 225
        public int scanForEnemies(com.oddlabs.tt.model.Selectable src)
        {
            com.oddlabs.tt.pathfinder.FindOccupantFilter filter = new com.oddlabs.tt.pathfinder.FindOccupantFilter(src.getPositionX(), src.getPositionY(), 30.0f, src, typeof(com.oddlabs.tt.model.Unit));
            getUnitGrid().scan(filter, src.getGridX(), src.getGridY());
            int score = 0;
            defense_target = null;
            foreach (com.oddlabs.tt.model.Unit unit in java.lang.JIter.of(filter.getResult()))
            {
                if (!unit.isDead() && getOwner().isEnemy(unit.getOwner()))
                {
                    score += getUnitScore(unit);
                    if (defense_target == null)
                    {
                        defense_target = new com.oddlabs.tt.landscape.LandscapeTarget(unit.getGridX(), unit.getGridY());
                    }
                }
            }
            return score;
        }
        // Java: 241
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
            throw new System.Exception();
        }
        // Java: 259
        public void nodeGuardTowers(int num_towers)
        {
            if ((getTowers() == null && num_towers > 0) || (getTowers() != null && num_towers > getTowers().Length))
            {
                nodeBuildTower(num_towers);
            }
            else if (num_towers > 0)
            {
                for (int i = 0; i < getTowers().Length; i++)
                {
                    if (!(((com.oddlabs.tt.model.Building)getTowers()[i])).getUnitContainer().isSupplyFull() && getIdleWarriors() != null && getIdleWarriors().Length > i)
                    {
                        getOwner().setTarget(com.oddlabs.tt.model.Selectable.newArray(getIdleWarriors()[i]), getTowers()[i], com.oddlabs.tt.model.Action.DEFAULT, false);
                        nodeDeployUnitsInArmory(1);
                    }
                }
            }
        }
        // Java: 274
        public void nodeBuildTower(int number)
        {
            if (!towerUnderConstruction() && ((getTowers() == null && number == 1) || (getTowers() != null && getTowers().Length < number)) && getQuarters() != null && getArmory() != null)
            {
                com.oddlabs.tt.model.Selectable[] builders = getPeons(10);
                if (builders.Length == 0)
                {
                    return;
                }
                com.oddlabs.tt.model.Building origin = (number % 2 == 1 ? ((com.oddlabs.tt.model.Building)getQuarters()[0]) : ((com.oddlabs.tt.model.Building)getArmory()[0]));
                int ox = origin.getGridX();
                int oy = origin.getGridY();
                int center = getOwner().getWorld().getHeightMap().getGridUnitsPerWorld() / 2;
                int dx = center - ox;
                int dy = center - oy;
                float inv_dist = (float)(1.0f / ((float)(java.lang.Math.sqrt(dx * dx + dy * dy))));
                int tx = java.lang.JC.f2i(((float)(ox + (float)((float)(10.0f * dx) * inv_dist))));
                int ty = java.lang.JC.f2i(((float)(oy + (float)((float)(10.0f * dy) * inv_dist))));
                setTowerUnderConstruction(buildBuilding(com.oddlabs.tt.model.Race.BUILDING_TOWER, builders, tx, ty));
            }
        }
        // Java: 294
        public void nodeAssignIdlePeons()
        {
            if (getIdlePeons() != null)
            {
                if (quartersUnderConstruction() && getConstructionSites() != null)
                {
                    getOwner().setTarget(getIdlePeons(), getConstructionSites()[0], com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else if (armoryUnderConstruction() && getConstructionSites() != null)
                {
                    getOwner().setTarget(getIdlePeons(), getConstructionSites()[0], com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else if (towerUnderConstruction() && getConstructionSites() != null)
                {
                    getOwner().setTarget(getIdlePeons(), getConstructionSites()[0], com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else if (shipUnderConstruction() && getConstructionSites() != null)
                {
                    getOwner().setTarget(getIdlePeons(), getConstructionSites()[0], com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else if (getQuarters() != null && !getQuarters()[0].isDead())
                {
                    getOwner().setTarget(getIdlePeons(), getQuarters()[0], com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
        }
        // Java: 310
        public void nodeAttackWithWarriorsAndChieftain(int num_warriors, bool use_chieftain)
        {
            if (getIdleWarriors() != null && getIdleWarriors().Length >= num_warriors && (!use_chieftain || getOwner().hasActiveChieftain()))
            {
                bool idle_chieftain = getIdleChieftains() != null && getIdleChieftains().Length >= 1;
                com.oddlabs.tt.model.Selectable[] warriors = default;
                if (idle_chieftain && use_chieftain)
                {
                    warriors = com.oddlabs.tt.model.Selectable.newArray(num_warriors + 1);
                    warriors[num_warriors] = getIdleChieftains()[0];
                }
                else
                {
                    warriors = com.oddlabs.tt.model.Selectable.newArray(num_warriors);
                }
                java.lang.JSystem.arraycopy(getIdleWarriors(), 0, warriors, 0, num_warriors);
                com.oddlabs.tt.util.Target target = findTarget(warriors[0].getGridX(), warriors[0].getGridY());
                if (target != null)
                {
                    getOwner().setLandscapeTarget(warriors, target.getGridX(), target.getGridY(), com.oddlabs.tt.model.Action.ATTACK, true);
                    if (NUM_WARRIORS[difficulty] < NUM_WARRIORS_MAX[difficulty])
                    {
                        NUM_WARRIORS[difficulty] += NUM_WARRIORS_INCREASE[difficulty];
                    }
                }
            }
            else
            {
                if (getIdleWarriors() != null)
                {
                    nodeDeployUnitsInArmory(num_warriors - getIdleWarriors().Length);
                }
                else
                {
                    nodeDeployUnitsInArmory(num_warriors);
                }
                if (use_chieftain)
                {
                    nodeTrainChieftain();
                }
            }
        }
        // Java: 347
        public void nodeTrainChieftain()
        {
            if (!getOwner().hasActiveChieftain() && !getOwner().isTrainingChieftain())
            {
                if (getQuarters() != null)
                {
                    getOwner().trainChieftain(((com.oddlabs.tt.model.Building)getQuarters()[0]), true);
                }
            }
        }
        // Java: 355
        public void nodeDeployUnitsInArmory(int num_warriors)
        {
            com.oddlabs.tt.model.Building armory = null;
            if (getArmory() != null && getArmory().Length > 0)
            {
                armory = ((com.oddlabs.tt.model.Building)getArmory()[0]);
            }
            if (armory != null)
            {
                if (!armory.isDead())
                {
                    int num_units = armory.getUnitContainer().getNumSupplies() - com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_BUILDING_WEAPONS[difficulty];
                    int num_weapons = numWeapons(armory) - com.oddlabs.tt.player.AdvancedAI.MIN_WEAPONS_IN_STOCK[difficulty];
                    if (num_units >= num_warriors && num_weapons >= num_warriors)
                    {
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
                    }
                    else
                    {
                        if (num_units < num_warriors)
                        {
                            nodeTransferUnits(num_warriors - num_units, armory);
                        }
                        if (num_weapons < num_warriors)
                        {
                            nodeGather(armory, num_units);
                        }
                    }
                }
            }
            else
            {
                nodeBuildArmory();
            }
        }
        // Java: 392
        public void nodeGather(com.oddlabs.tt.model.Building armory, int num_units)
        {
            int tree = 0;
            int rock = 0;
            int iron = 0;
            int rubber = 0;
            if (getGatherTreePeons() != null)
            {
                tree = getGatherTreePeons().Length;
            }
            if (getGatherRockPeons() != null)
            {
                rock = getGatherRockPeons().Length;
            }
            if (getGatherIronPeons() != null)
            {
                iron = getGatherIronPeons().Length;
            }
            if (getGatherRubberPeons() != null)
            {
                rubber = getGatherRubberPeons().Length;
            }
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
        // Java: 444
        public void nodeTransferUnits(int num_units, com.oddlabs.tt.model.Building armory)
        {
            com.oddlabs.tt.model.Building quarters = null;
            if (getQuarters() != null && getQuarters().Length > 0)
            {
                quarters = ((com.oddlabs.tt.model.Building)getQuarters()[0]);
            }
            if (quarters != null)
            {
                if (!quarters.isDead())
                {
                    quarters.setRallyPoint(armory);
                    if (quarters.getUnitContainer().getNumSupplies() > com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty])
                    {
                        int units = java.lang.Math.min(num_units, quarters.getUnitContainer().getNumSupplies() - com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty]);
                        getOwner().deployUnits(quarters, com.oddlabs.tt.model.DeployType.PEON, units);
                    }
                }
            }
            else
            {
                nodeBuildQuarters();
            }
        }
        // Java: 463
        public void nodeBuildArmory()
        {
            if (!quartersUnderConstruction() && getQuarters() == null)
            {
                nodeBuildQuarters();
            }
            com.oddlabs.tt.model.Building quarters = null;
            if (getQuarters() != null && getQuarters().Length > 0)
            {
                quarters = ((com.oddlabs.tt.model.Building)getQuarters()[0]);
            }
            if (!armoryUnderConstruction() && getArmory() == null && getQuarters() != null && getQuarters()[0].getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
            {
                com.oddlabs.tt.model.Selectable[] builders = getPeons(20);
                if (builders.Length < 20)
                {
                    if (quarters != null && !quarters.isDead() && quarters.getUnitContainer().getNumSupplies() >= 20)
                    {
                        getOwner().deployUnits(quarters, com.oddlabs.tt.model.DeployType.PEON, 20);
                    }
                }
                if (builders.Length == 0)
                {
                    return;
                }
                setArmoryUnderConstruction(buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, builders, builders[0].getGridX(), builders[0].getGridY()));
                reclassify();
            }
        }
        // Java: 488
        public void nodeBuildQuarters()
        {
            if (!quartersUnderConstruction() && getQuarters() == null)
            {
                com.oddlabs.tt.model.Selectable[] builders = getPeons(com.oddlabs.tt.player.AdvancedAI.MIN_UNITS_REPRODUCING[difficulty]);
                if (builders.Length == 0)
                {
                    return;
                }
                setQuartersUnderConstruction(buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, builders, builders[0].getGridX(), builders[0].getGridY()));
                reclassify();
            }
        }
        // Java: 501
        public bool baseBuildingsDone()
        {
            return getQuarters() != null && getArmory() != null && !quartersUnderConstruction() && !armoryUnderConstruction();
        }
        // Java: 506
        public void nodeBuildShipAndLoad()
        {
            java.util.List ships = getOwnShips();
            nodeBuildShip(ships.size());
            foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(ships.subList(0, java.lang.Math.min(ships.size(), getMaxFleetSize()))))
            {
                if (ship.isComplete() && !ship.isMoving() && shipAtHome(ship) && !shipFullyCrewed(ship))
                {
                    nodeLoadShip(ship);
                    return;
                }
            }
        }
        // Java: 519
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
        // Java: 528
        public void nodePickInitIsland()
        {
            com.oddlabs.tt.model.Ship ship = getInitShip();
            if (ship == null)
            {
                return;
            }
            if (init_island == null)
            {
                java.util.List islands = getOwner().getWorld().getHeightMap().getIslandInfos();
                com.oddlabs.tt.landscape.IslandInfo best = null;
                int best_d2 = 1000;
                int contact_x = 0;
                int contact_y = 0;
                for (int i = 0; i < islands.size(); i++)
                {
                    com.oddlabs.tt.landscape.IslandInfo island = ((com.oddlabs.tt.landscape.IslandInfo)islands.get(i));
                    if (island.trees() > 25 && island.rocks() > 10 && island.iron() > 10)
                    {
                        foreach (int[] pt in java.lang.JIter.of(island.contourPoints()))
                        {
                            int dx = pt[0] - ship.getGridX();
                            int dy = pt[1] - ship.getGridY();
                            int d2 = dx * dx + dy * dy;
                            if (best == null || d2 < best_d2)
                            {
                                best = island;
                                best_d2 = d2;
                                contact_x = pt[0];
                                contact_y = pt[1];
                            }
                        }
                    }
                }
                if (best != null)
                {
                    init_island_contact_x = contact_x;
                    init_island_contact_y = contact_y;
                    init_island = best;
                }
            }
            bool landed = true;
            foreach (com.oddlabs.tt.model.Ship start_ship in java.lang.JIter.of(getOwnShips()))
            {
                if (shipIncomplete(start_ship))
                {
                    continue;
                }
                if (((object)start_ship.getEntrance() == (object)start_ship) || start_ship.getEntrance().getIslandId() != init_island.id())
                {
                    if (!start_ship.isMoving())
                    {
                        getOwner().setSailingTarget(com.oddlabs.tt.model.Selectable.newArray(start_ship), init_island_contact_x, init_island_contact_y);
                    }
                    deployWholeShip(start_ship);
                    landed = false;
                }
                else if (start_ship.getShipHR().countUnits() > 0)
                {
                    deployWholeShip(start_ship);
                    landed = false;
                }
            }
            if (landed)
            {
                setFoundIsland();
            }
        }
        // Java: 586
        public void nodeUseShip()
        {
            java.util.List ships = getOwnShips();
            foreach (com.oddlabs.tt.model.Ship ship in java.lang.JIter.of(ships))
            {
                useShip(ship);
            }
        }
        // Java: 593
        public bool islandHasEnemies(com.oddlabs.tt.landscape.IslandInfo island)
        {
            if (density_map.sum(island.minX(), island.minY(), island.maxX(), island.maxY()) == 0)
            {
                return false;
            }
            com.oddlabs.tt.model.Selectable enemy = getOwner().findNearestEnemy(island.centerX(), island.centerY(), new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return s.getIslandId() == island.id();
            }));
            return enemy != null;
        }
        // Java: 603
        public void pickSafeLandingPoint(com.oddlabs.tt.model.Ship ship, int islandId)
        {
            com.oddlabs.tt.landscape.IslandInfo info = getOwner().getWorld().getHeightMap().getIslandInfo(islandId);
            java.util.List pts = info.contourPoints();
            int best_weight = 10000;
            int[] best_pt = ((int[])pts.get(0));
            for (int i = 0; i < pts.size(); i++)
            {
                int[] pt = ((int[])pts.get(i));
                int weight = density_map.getWeight(pt[0], pt[1], 1);
                if (i == 0 || weight < best_weight)
                {
                    best_weight = weight;
                    best_pt = pt;
                }
            }
            getOwner().setSailingTarget(com.oddlabs.tt.model.Selectable.newArray(ship), best_pt[0], best_pt[1]);
        }
        // Java: 619
        public void useShip(com.oddlabs.tt.model.Ship ship)
        {
            if (ship.isDead() || !ship.isComplete() || ship.isMoving())
            {
                return;
            }
            bool at_home = shipAtHome(ship);
            bool should_escape = shipShouldEscape(ship);
            bool battle_ready = shipBattleReady(ship);
            if (should_escape)
            {
                com.oddlabs.tt.model.Building home = homeBuilding();
                if (home != null && ship.getEntrance().getIslandId() != home.getIslandId())
                {
                    getOwner().setSailingTarget(com.oddlabs.tt.model.Selectable.newArray(ship), home);
                }
                return;
            }
            if (!home_is_safe)
            {
                com.oddlabs.tt.model.Selectable enemy = getOwner().findNearestEnemy(ship.getGridX(), ship.getGridY(), new java.util.function.Predicate_L((object s_l) =>
                {
                    com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                    return (s is com.oddlabs.tt.model.Ship) || objectOnBeach(s);
                }));
                if (battle_ready)
                {
                    if (enemy != null)
                    {
                        getOwner().setSailingTarget(com.oddlabs.tt.model.Selectable.newArray(ship), enemy);
                        return;
                    }
                }
                if (!at_home && enemy == null)
                {
                    com.oddlabs.tt.model.Building home_2 = homeBuilding();
                    if (home_2 != null)
                    {
                        getOwner().setSailingTarget(com.oddlabs.tt.model.Selectable.newArray(ship), home_2);
                    }
                }
            }
            else
            {
                if (battle_ready)
                {
                    com.oddlabs.tt.model.Selectable enemy_2 = getOwner().findNearestEnemy(ship.getGridX(), ship.getGridY(), new java.util.function.Predicate_L((object s_2_l) =>
                    {
                        com.oddlabs.tt.model.Selectable s_2 = (com.oddlabs.tt.model.Selectable)s_2_l;
                        return !((s_2 is com.oddlabs.tt.model.Ship)) && (s_2.getIslandId() != 0);
                    }));
                    if (enemy_2 != null)
                    {
                        if (ship.getEntrance().getIslandId() != enemy_2.getIslandId())
                        {
                            pickSafeLandingPoint(ship, enemy_2.getIslandId());
                            return;
                        }
                        if (((object)ship.getEntrance() == (object)ship))
                        {
                            deployShipWarriors(ship);
                            return;
                        }
                        if (ship.getEntrance().getIslandId() == enemy_2.getIslandId())
                        {
                            if (shipHasWarriors(ship))
                            {
                                deployShipWarriors(ship);
                                return;
                            }
                        }
                    }
                }
                if (!shipHasWarriors(ship) && !at_home)
                {
                    com.oddlabs.tt.model.Building home_3 = homeBuilding();
                    if (home_3 != null)
                    {
                        getOwner().setSailingTarget(com.oddlabs.tt.model.Selectable.newArray(ship), home_3);
                    }
                }
            }
        }
        // Java: 686
        public bool objectOnBeach(com.oddlabs.tt.model.Selectable s)
        {
            sbyte[][] dock = getOwner().getWorld().getHeightMap().getDockGrid();
            int map_size = getOwner().getWorld().getHeightMap().getGridUnitsPerWorld();
            int x = s.getGridX();
            int y = s.getGridY();
            int size = java.lang.StrictMath.round(s.getSize());
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    int cx = x + i - size / 2;
                    int cy = y + i - size / 2;
                    if (cx < 0 || cx >= map_size || cy < 0 || cy >= map_size)
                    {
                        continue;
                    }
                    if (dock[cy][cx] != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // Java: 707
        public bool shipBattleReady(com.oddlabs.tt.model.Ship ship)
        {
            return shipFullyCrewed(ship) && !shipNeedsRepair(ship);
        }
        // Java: 711
        public bool shipShouldEscape(com.oddlabs.tt.model.Ship ship)
        {
            return shipTooDamaged(ship);
        }
        // Java: 715
        public bool shipTooDamaged(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getHitPoints() < (float)(com.oddlabs.tt.player.AdvancedAI.SHIP_MIN_HEALTH * ship.getBuildingTemplate().getMaxHitPoints());
        }
        // Java: 719
        public bool shipNeedsRepair(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getHitPoints() < ship.getBuildingTemplate().getMaxHitPoints();
        }
        // Java: 723
        public bool shipFullyCrewed(com.oddlabs.tt.model.Ship ship)
        {
            int peons = ship.getShipHR().countPeons();
            int warriors = ship.getShipHR().countUnits() - peons;
            return peons >= com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS && warriors >= getMinWarriorsOnShip();
        }
        // Java: 729
        public bool shipCrewedEnough(com.oddlabs.tt.model.Ship ship)
        {
            int peons = ship.getShipHR().countPeons();
            int warriors = ship.getShipHR().countUnits() - peons;
            return peons >= com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS / 2 && warriors >= getMinWarriorsOnShip() / 3;
        }
        // Java: 735
        public com.oddlabs.tt.model.Building homeBuilding()
        {
            if (getQuarters() != null)
            {
                return ((com.oddlabs.tt.model.Building)getQuarters()[0]);
            }
            if (getArmory() != null)
            {
                return ((com.oddlabs.tt.model.Building)getArmory()[0]);
            }
            return null;
        }
        // Java: 743
        public int homeIsland()
        {
            com.oddlabs.tt.model.Building home = homeBuilding();
            return (home != null ? home.getIslandId() : (-1));
        }
        // Java: 748
        public bool isShipNearIsland(com.oddlabs.tt.model.Selectable s, com.oddlabs.tt.landscape.IslandInfo info)
        {
            if (!((s is com.oddlabs.tt.model.Ship)))
            {
                return false;
            }
            int dx = info.centerX() - s.getGridX();
            int dy = info.centerY() - s.getGridY();
            int d2 = dx * dx + dy * dy;
            int r = java.lang.Math.max(info.maxX() - info.minX(), info.maxY() - info.minY());
            if (d2 < r * r)
            {
                return true;
            }
            return false;
        }
        // Java: 764
        public bool shipsAroundIsland(com.oddlabs.tt.landscape.IslandInfo info)
        {
            com.oddlabs.tt.model.Selectable enemy = getOwner().findNearestEnemy(info.centerX(), info.centerY(), new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return isShipNearIsland(s, info);
            }));
            return enemy != null;
        }
        // Java: 770
        public bool islandHasEnemies(int island)
        {
            com.oddlabs.tt.landscape.IslandInfo info = getOwner().getWorld().getHeightMap().getIslandInfo(island);
            if (info == null)
            {
                return false;
            }
            com.oddlabs.tt.model.Selectable enemy = getOwner().findNearestEnemy(info.centerX(), info.centerY(), new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return s.getIslandId() == island;
            }));
            return enemy != null;
        }
        // Java: 780
        public bool homeHasEnemies()
        {
            int id = homeIsland();
            com.oddlabs.tt.landscape.IslandInfo info = getOwner().getWorld().getHeightMap().getIslandInfo(id);
            bool on_land = islandHasEnemies(id);
            bool around = shipsAroundIsland(info);
            return on_land || around;
        }
        // Java: 788
        public void deployWholeShip(com.oddlabs.tt.model.Ship ship)
        {
            int peons = ship.getShipHR().countPeons();
            deployShipWarriors(ship);
            if (peons > 0)
            {
                getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.PEON, peons);
            }
        }
        // Java: 796
        public int getMaxFleetSize()
        {
            return (home_is_safe ? com.oddlabs.tt.player.AdvancedAI.FLEET_SIZE : 1);
        }
        // Java: 800
        public int getMinWarriorsOnShip()
        {
            return (home_is_safe ? com.oddlabs.tt.player.AdvancedAI.SHIP_WARRIORS : 15);
        }
        // Java: 804
        public bool shipHasWarriors(com.oddlabs.tt.model.Ship ship)
        {
            return ship.getShipHR().countPeons() < ship.getShipHR().countUnits();
        }
        // Java: 808
        public void deployShipWarriors(com.oddlabs.tt.model.Ship ship)
        {
            int peons = ship.getShipHR().countPeons();
            int warriors = ship.getShipHR().countUnits() - peons;
            if (warriors > 0)
            {
                getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.RUBBER_WARRIOR, warriors);
                getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.IRON_WARRIOR, warriors);
                getOwner().deployUnits(ship, com.oddlabs.tt.model.DeployType.ROCK_WARRIOR, warriors);
            }
        }
        // Java: 818
        public bool shipAtHome(com.oddlabs.tt.model.Ship ship)
        {
            com.oddlabs.tt.model.Building entrance = ship.getEntrance();
            com.oddlabs.tt.model.Building home = homeBuilding();
            if (home == null)
            {
                return false;
            }
            return entrance.getIslandId() == home.getIslandId();
        }
        // Java: 827
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
        // Java: 842
        public bool shipDockedAt(com.oddlabs.tt.model.Ship ship, int island)
        {
            com.oddlabs.tt.model.Building entrance = ship.getEntrance();
            return ((object)entrance != (object)ship) && !entrance.isDead() && entrance.getIslandId() == island;
        }
        // Java: 847
        public void nodeBuildShip(int fleet_size)
        {
            com.oddlabs.tt.model.Ship ship = getIncompleteShip();
            if (ship == null && fleet_size >= getMaxFleetSize())
            {
                return;
            }
            com.oddlabs.tt.model.Selectable[] idle = getIdlePeons();
            int idle_count = (idle != null ? idle.Length : 0);
            int missing = com.oddlabs.tt.player.AdvancedAI.SHIP_BUILDERS - ((ship != null ? countBuilders(ship.getEntrance()) : 0));
            if (missing > idle_count && getQuarters() != null)
            {
                com.oddlabs.tt.model.Building quarters = ((com.oddlabs.tt.model.Building)getQuarters()[0]);
                deployPeonsFromQuarters(quarters, missing - idle_count, quarters);
                com.oddlabs.tt.model.Building armory = ((com.oddlabs.tt.model.Building)getArmory()[0]);
                deployPeonsFromQuarters(armory, missing - idle_count, armory);
            }
            if (idle_count == 0 || missing <= 0)
            {
                return;
            }
            if (ship != null)
            {
                com.oddlabs.tt.model.Action action = (ship.isComplete() ? com.oddlabs.tt.model.Action.GATHER_REPAIR : com.oddlabs.tt.model.Action.DEFAULT);
                getOwner().setTarget(lastN(idle, missing), ship, action, false);
            }
            else
            {
                com.oddlabs.tt.model.Building origin = homeBuilding();
                buildShip(firstN(idle, com.oddlabs.tt.player.AdvancedAI.SHIP_BUILDERS), origin.getGridX(), origin.getGridY());
            }
        }
        // Java: 876
        public bool shipIncomplete(com.oddlabs.tt.model.Ship ship)
        {
            return !ship.isComplete() || ship.isDamaged();
        }
        // Java: 880
        public com.oddlabs.tt.model.Ship getIncompleteShip()
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead() && shipIncomplete(ship))
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
        // Java: 896
        public com.oddlabs.tt.model.Ship getInitShip()
        {
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(getOwner().getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Ship ship) && !ship.isDead() && !shipIncomplete(ship))
                {
                    return ship;
                }
            }
            return null;
        }
        // Java: 904
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
        // Java: 920
        public void nodeLoadShip(com.oddlabs.tt.model.Ship ship)
        {
            int peons_aboard = ship.getShipHR().countPeons();
            int warriors_aboard = ship.getShipHR().countUnits() - peons_aboard;
            int peons_needed = com.oddlabs.tt.player.AdvancedAI.SHIP_PEONS - peons_aboard;
            if (peons_needed > 0)
            {
                if (getIdlePeons() != null && getIdlePeons().Length > 0)
                {
                    getOwner().setTarget(firstN(getIdlePeons(), peons_needed), ship, com.oddlabs.tt.model.Action.MOVE, false);
                }
                else
                {
                    nodeDeployPeonsFromQuarters(ship, peons_needed);
                }
            }
            int warriors_needed = getMinWarriorsOnShip() - warriors_aboard;
            if (warriors_needed > 0)
            {
                if (getIdleWarriors() != null && getIdleWarriors().Length > 0)
                {
                    getOwner().setTarget(firstN(getIdleWarriors(), warriors_needed), ship, com.oddlabs.tt.model.Action.MOVE, false);
                }
                else
                {
                    nodeDeployUnitsInArmory(warriors_needed);
                }
            }
        }
        // Java: 943
        public void nodeDeployPeonsFromQuarters(com.oddlabs.tt.model.Ship ship, int num_peons)
        {
            if (getQuarters() == null)
            {
                return;
            }
            deployPeonsFromQuarters(((com.oddlabs.tt.model.Building)getQuarters()[0]), num_peons, ship);
        }
        // Java: 949
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
        // Java: 963
        public com.oddlabs.tt.model.Selectable[] firstN(com.oddlabs.tt.model.Selectable[] list, int n)
        {
            n = java.lang.Math.min(n, list.Length);
            com.oddlabs.tt.model.Selectable[] result = com.oddlabs.tt.model.Selectable.newArray(n);
            java.lang.JSystem.arraycopy(list, 0, result, 0, n);
            return result;
        }
        // Java: 970
        public com.oddlabs.tt.model.Selectable[] lastN(com.oddlabs.tt.model.Selectable[] list, int n)
        {
            n = java.lang.Math.min(n, list.Length);
            com.oddlabs.tt.model.Selectable[] result = com.oddlabs.tt.model.Selectable.newArray(n);
            java.lang.JSystem.arraycopy(list, list.Length - n, result, 0, n);
            return result;
        }
        // Java: 977
        public com.oddlabs.tt.model.Selectable[] getPeons(int min_num_peons)
        {
            com.oddlabs.tt.model.Selectable[] idle = getIdlePeons();
            int idleCount = (null != idle ? idle.Length : 0);
            java.util.stream.Stream active = java.util.stream.Stream_.of(((java.util.function.Supplier)new java.util.function.Supplier_L(() => { return this.getGatherIronPeons(); })), new java.util.function.Supplier_L(() => { return this.getGatherRockPeons(); }), new java.util.function.Supplier_L(() => { return this.getGatherTreePeons(); }), new java.util.function.Supplier_L(() => { return this.getGatherRubberPeons(); })).map(new java.util.function.Function_L((object a0) => { return ((java.util.function.Supplier)a0).get(); })).filter(new java.util.function.Predicate_L((object a0) => { return java.util.Objects.nonNull(a0); })).flatMap(new java.util.function.Function_L((object a0) => { return java.util.Arrays.stream(((object[])a0)); })).limit(java.lang.Math.max(min_num_peons - idleCount, 0));
            return ((com.oddlabs.tt.model.Selectable[])((null != idle ? java.util.stream.Stream_.concat(java.util.Arrays.stream(idle), active) : active)).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Selectable[a0]; })));
        }
        // Java: 998
        public int numWeapons(com.oddlabs.tt.model.Building armory)
        {
            return armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)).getNumSupplies() + armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon)).getNumSupplies() + armory.getSupplyContainer(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)).getNumSupplies();
        }
        // Java: 1004
        public com.oddlabs.tt.util.Target findTarget(int start_x, int start_y)
        {
            com.oddlabs.tt.util.Target best_building = getOwner().findNearestEnemyBuilding(start_x, start_y);
            com.oddlabs.tt.util.Target best_target = getOwner().findNearestEnemy(start_x, start_y);
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
        // Java: 1020
        public bool buildShip(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
            if (selection.Length == 0)
            {
                return false;
            }
            com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(getUnitGrid(), getOwner().getRace().getBuildingTemplate(com.oddlabs.tt.model.Race.BUILDING_SHIP), 100, true, selection[0].getIslandId());
            getUnitGrid().scan(filter, grid_x, grid_y);
            java.util.List target_list = filter.getResult();
            if (!target_list.isEmpty())
            {
                com.oddlabs.tt.util.Target target = ((com.oddlabs.tt.util.Target)target_list.getFirst());
                getOwner().placeBuilding(selection, com.oddlabs.tt.model.Race.BUILDING_SHIP, target.getGridX(), target.getGridY());
                return true;
            }
            else
            {
                return false;
            }
        }
        // Java: 1037
        public bool buildBuilding(int building_type, com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
            if (selection.Length == 0)
            {
                return false;
            }
            com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(getUnitGrid(), getOwner().getRace().getBuildingTemplate(building_type), 40, true, selection[0].getIslandId());
            getUnitGrid().scan(filter, grid_x, grid_y);
            java.util.List target_list = filter.getResult();
            if (!target_list.isEmpty())
            {
                com.oddlabs.tt.util.Target target = ((com.oddlabs.tt.util.Target)target_list.getFirst());
                getOwner().placeBuilding(selection, building_type, target.getGridX(), target.getGridY());
                return true;
            }
            else
            {
                return false;
            }
        }
        static AdvancedAI()
        {
            AdvancedAI.DEFENSE_FACTOR = new float[] { 1.0f, 1.5f, 2.0f };
            AdvancedAI.MIN_UNITS_BUILDING_WEAPONS = new int[] { 0, 3, 8 };
            AdvancedAI.MIN_WEAPONS_IN_STOCK = new int[] { 10, 5, 0 };
            AdvancedAI.MIN_UNITS_REPRODUCING = new int[] { 0, 5, 20 };
            AdvancedAI.MAX_UNITS_GATHERING_TREE = new int[] { 2, 5, 15 };
            AdvancedAI.MAX_UNITS_GATHERING_ROCK = new int[] { 1, 3, 9 };
            AdvancedAI.MAX_UNITS_GATHERING_IRON = new int[] { 1, 3, 10 };
            AdvancedAI.MAX_UNITS_GATHERING_RUBBER = new int[] { 0, 0, 3 };
            AdvancedAI.UNITS_PER_TOWER1 = new int[] { 1000, 1000, 90 };
            AdvancedAI.UNITS_PER_TOWER2 = new int[] { 1000, 1000, 120 };
        }
        private void init_AdvancedAI()
        {
            this.NUM_WARRIORS = new int[] { 3, 7, 10 };
            this.NUM_WARRIORS_INCREASE = new int[] { 1, 3, 5 };
            this.NUM_WARRIORS_MAX = new int[] { 10, 19, 40 };
            this.NUM_WARRIORS_FOR_CHIEFTAIN = new int[] { 1000, 1000, 20 };
            this.defense_target = null;
            this.init_island = null;
            this.init_island_contact_x = (-1);
            this.init_island_contact_y = (-1);
            this.home_is_safe = false;
        }
        public AdvancedAI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info, int difficulty) : base(owner, unit_info)
        {
            init_AdvancedAI();
            this.difficulty = difficulty;
            this.density_map = new com.oddlabs.tt.landscape.DensityMap(owner.getWorld(), owner.getPlayerInfo().getTeam());
        }
    }
}

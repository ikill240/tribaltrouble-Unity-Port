// Translated from AI.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 29
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
        public float sleep_time;
        public java.util.List helpers;
        // Java: 62
        public void sleep()
        {
            thinks_in_game_time = owner.getWorld().getAnimationManagerGameTime().isRegistered(this);
            owner.getWorld().getAnimationManagerRealTime().removeAnimation(this);
            owner.getWorld().getAnimationManagerGameTime().removeAnimation(this);
            if (relay_f != null)
            {
                relay_f.sleep();
            }
            sleepHelpers();
        }
        public bool thinks_in_game_time;
        // Java: 74
        public void wake()
        {
            ((thinks_in_game_time ? owner.getWorld().getAnimationManagerGameTime() : owner.getWorld().getAnimationManagerRealTime())).registerAnimation(this);
            if (relay_f != null)
            {
                relay_f.wake();
            }
            wakeHelpers();
        }
        // Java: 83
        public void addHelper(com.oddlabs.tt.animation.Animated h)
        {
            helpers.add(h);
            owner.getWorld().getAnimationManagerGameTime().registerAnimation(h);
        }
        // Java: 88
        public void sleepHelpers()
        {
            foreach (com.oddlabs.tt.animation.Animated h in java.lang.JIter.of(helpers))
            {
                owner.getWorld().getAnimationManagerGameTime().removeAnimation(h);
            }
        }
        // Java: 93
        public void wakeHelpers()
        {
            foreach (com.oddlabs.tt.animation.Animated h in java.lang.JIter.of(helpers))
            {
                owner.getWorld().getAnimationManagerGameTime().registerAnimation(h);
            }
        }
        public com.oddlabs.tt.player.AI relay_f;
        public java.util.List relays;
        // Java: 105
        public com.oddlabs.tt.player.AI brain()
        {
            return (relay_f != null ? relay_f.brain() : this);
        }
        // Java: 109
        public com.oddlabs.tt.player.AI relay()
        {
            return relay_f;
        }
        // Java: 117
        public static float tribePower(com.oddlabs.tt.player.Player p)
        {
            float total = 0.0f;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(p.getUnits().getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit u))
                {
                    if (u.isDead())
                    {
                        continue;
                    }
                    if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                    {
                        total = (float)(total + 2.0f);
                    }
                    else if (u.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        total = (float)(total + 25.0f);
                    }
                    else if (u.isWarrior())
                    {
                        total = (float)(total + com.oddlabs.tt.player.AI.warriorPower(u));
                    }
                }
                else if ((s is com.oddlabs.tt.model.Building b) && !b.isDead())
                {
                    total = (float)(total + (b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE) || b.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES) ? 20.0f : 8.0f));
                }
            }
            return total;
        }
        // Java: 136
        public static float warriorPower(com.oddlabs.tt.model.Unit u)
        {
            System.Type w = u.getWeaponFactory().getType();
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon)) || ((object)w == (object)typeof(com.oddlabs.tt.model.weapon.RockSpearWeapon)))
            {
                return 5.0f;
            }
            if (((object)w == (object)typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon)) || ((object)w == (object)typeof(com.oddlabs.tt.model.weapon.RubberSpearWeapon)))
            {
                return 15.0f;
            }
            return 10.0f;
        }
        // Java: 146
        public static float strongestEnemyPower(com.oddlabs.tt.player.Player owner)
        {
            float enemy = 0.0f;
            foreach (com.oddlabs.tt.player.Player p in owner.getWorld().getPlayers())
            {
                if (owner.isEnemy(p) && p.isAlive())
                {
                    enemy = java.lang.Math.max(enemy, com.oddlabs.tt.player.AI.tribePower(p));
                }
            }
            return enemy;
        }
        public static bool making_brain;
        // Java: 158
        public static com.oddlabs.tt.player.AI makeBrain(java.util.function.Supplier make)
        {
            com.oddlabs.tt.player.AI.making_brain = true;
            try
            {
                return ((com.oddlabs.tt.player.AI)make.get());
            }
            finally
            {
                com.oddlabs.tt.player.AI.making_brain = false;
            }
        }
        // Java: 176
        public virtual void lateStart(float match_seconds)
        {
        }
        // Java: 179
        public void relayTo(com.oddlabs.tt.player.AI brain_v, float match_seconds)
        {
            if (((object)brain_v == (object)relay_f))
            {
                return;
            }
            if (relay_f != null)
            {
                relay_f.sleep();
            }
            relay_f = brain_v;
            if (brain_v == null)
            {
            }
            else if (relays.contains(brain_v))
            {
                brain_v.wake();
            }
            else
            {
                relays.add(brain_v);
                brain_v.lateStart(match_seconds);
            }
        }
        // Java: 259
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return owner.getWorld().getUnitGrid();
        }
        // Java: 263
        public com.oddlabs.tt.player.Player getOwner()
        {
            return owner;
        }
        // Java: 268
        public virtual bool isHardDifficulty()
        {
            return false;
        }
        // Java: 272
        public void reclassify()
        {
            lists = getOwner().classifyUnits();
            classifyIndex(lists);
        }
        // Java: 277
        public com.oddlabs.tt.model.Selectable[] getIdlePeons()
        {
            return (lists == null || INDEX_IDLE_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_IDLE_PEONS]);
        }
        // Java: 281
        public com.oddlabs.tt.model.Selectable[] getIdleChieftains()
        {
            return (lists == null || INDEX_IDLE_CHIEFTAINS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_IDLE_CHIEFTAINS]);
        }
        // Java: 285
        public com.oddlabs.tt.model.Selectable[] getIdleWarriors()
        {
            return (lists == null || INDEX_IDLE_WARRIORS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_IDLE_WARRIORS]);
        }
        // Java: 289
        public com.oddlabs.tt.model.Selectable[] getGatherTreePeons()
        {
            return (lists == null || INDEX_GATHER_TREE_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_TREE_PEONS]);
        }
        // Java: 293
        public com.oddlabs.tt.model.Selectable[] getGatherRockPeons()
        {
            return (lists == null || INDEX_GATHER_ROCK_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_ROCK_PEONS]);
        }
        // Java: 297
        public com.oddlabs.tt.model.Selectable[] getGatherIronPeons()
        {
            return (lists == null || INDEX_GATHER_IRON_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_IRON_PEONS]);
        }
        // Java: 301
        public com.oddlabs.tt.model.Selectable[] getGatherRubberPeons()
        {
            return (lists == null || INDEX_GATHER_RUBBER_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_GATHER_RUBBER_PEONS]);
        }
        // Java: 305
        public com.oddlabs.tt.model.Selectable[] getArmory()
        {
            return (lists == null || INDEX_ARMORY == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_ARMORY]);
        }
        // Java: 309
        public com.oddlabs.tt.model.Selectable[] getQuarters()
        {
            return (lists == null || INDEX_QUARTERS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_QUARTERS]);
        }
        // Java: 316
        public com.oddlabs.tt.model.Selectable[] getTowers()
        {
            return (lists == null || INDEX_TOWERS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_TOWERS]);
        }
        // Java: 320
        public com.oddlabs.tt.model.Selectable[] getShips()
        {
            return (lists == null || INDEX_SHIPS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_SHIPS]);
        }
        // Java: 324
        public com.oddlabs.tt.model.Selectable[] getConstructionSites()
        {
            return (lists == null || INDEX_CONSTRUCTION_SITES == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_CONSTRUCTION_SITES]);
        }
        // Java: 328
        public com.oddlabs.tt.model.Selectable[] getPlaceBuildingPeons()
        {
            return (lists == null || INDEX_PLACE_BUILDING_PEONS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_PLACE_BUILDING_PEONS]);
        }
        // Java: 332
        public com.oddlabs.tt.model.Selectable[] getDefendingUnits()
        {
            return (lists == null || INDEX_DEFENDING_UNITS == (-1) ? (com.oddlabs.tt.model.Selectable[])(null) : lists[INDEX_DEFENDING_UNITS]);
        }
        // Java: 336
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
                        tower_under_construction = false;
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
        // Java: 408
        public bool armoryUnderConstruction()
        {
            return armory_under_construction;
        }
        // Java: 412
        public void setArmoryUnderConstruction(bool armory_under_construction)
        {
            this.armory_under_construction = armory_under_construction;
        }
        // Java: 416
        public bool quartersUnderConstruction()
        {
            return quarters_under_construction;
        }
        // Java: 420
        public void setQuartersUnderConstruction(bool quarters_under_construction)
        {
            this.quarters_under_construction = quarters_under_construction;
        }
        // Java: 432
        public bool towerUnderConstruction()
        {
            return tower_under_construction;
        }
        // Java: 436
        public void setTowerUnderConstruction(bool tower_under_construction)
        {
            this.tower_under_construction = tower_under_construction;
        }
        // Java: 440
        public bool shipUnderConstruction()
        {
            return ship_under_construction;
        }
        // Java: 444
        public void setShipUnderConstruction(bool ship_under_construction)
        {
            this.ship_under_construction = ship_under_construction;
        }
        // Java: 448
        public void reset()
        {
            sleep_time = (float)((float)(owner.getWorld().getRandom().nextFloat() * com.oddlabs.tt.player.AI.SLEEP_SECONDS) + com.oddlabs.tt.player.AI.MIN_SLEEP_SECONDS);
        }
        // Java: 452
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
        // Java: 460
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
        // Java: 473
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
        // Java: 495
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
        // Java: 506
        public com.oddlabs.tt.util.Target getTarget(java.util.Random random)
        {
            float RADIUS = 30;
            float target_x = (float)(owner.getStartX() + (float)(((float)((float)(random.nextFloat() * 2) - 1)) * RADIUS));
            float target_y = (float)(owner.getStartY() + (float)(((float)((float)(random.nextFloat() * 2) - 1)) * RADIUS));
            com.oddlabs.tt.util.Target target = getUnitGrid().findGridTargets(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(target_x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(target_y), 1, false)[0];
            if (target != null)
            {
                return target;
            }
            return getUnitGrid().findGridTargets(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartX()), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartY()), 1, false)[0];
        }
        private void init_AI()
        {
            this.INDEX_IDLE_PEONS = (-1);
            this.INDEX_IDLE_CHIEFTAINS = (-1);
            this.INDEX_IDLE_WARRIORS = (-1);
            this.INDEX_GATHER_TREE_PEONS = (-1);
            this.INDEX_GATHER_ROCK_PEONS = (-1);
            this.INDEX_GATHER_IRON_PEONS = (-1);
            this.INDEX_GATHER_RUBBER_PEONS = (-1);
            this.INDEX_ARMORY = (-1);
            this.INDEX_QUARTERS = (-1);
            this.INDEX_TOWERS = (-1);
            this.INDEX_SHIPS = (-1);
            this.INDEX_CONSTRUCTION_SITES = (-1);
            this.INDEX_PLACE_BUILDING_PEONS = (-1);
            this.INDEX_DEFENDING_UNITS = (-1);
            this.armory_under_construction = false;
            this.quarters_under_construction = false;
            this.tower_under_construction = false;
            this.ship_under_construction = false;
            this.helpers = new java.util.ArrayList();
            this.relays = new java.util.ArrayList();
        }
        public AI(com.oddlabs.tt.player.Player owner, com.oddlabs.tt.player.UnitInfo unit_info) : base()
        {
            init_AI();
            this.owner = owner;
            owner.getWorld().getAnimationManagerRealTime().registerAnimation(this);
            reset();
            if (owner.getWorld().isExtended() && !com.oddlabs.tt.player.AI.making_brain)
            {
                helpers.add(new com.oddlabs.tt.player.ExtendedAIPlan(owner));
            }
            if (owner.getWorld().isExtended() && !com.oddlabs.tt.player.AI.making_brain && ((this is com.oddlabs.tt.player.ai.ExpertAI) || (this is com.oddlabs.tt.player.gauntlet.GauntletAI)))
            {
                helpers.add(new com.oddlabs.tt.player.ExtendedTactics(owner, this, (this is com.oddlabs.tt.player.gauntlet.GauntletAI)));
            }
            if (unit_info != null)
            {
                int grid_start_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartX());
                int grid_start_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(owner.getStartY());
                if (unit_info.hasQuarters())
                {
                    owner.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_QUARTERS, grid_start_x, grid_start_y);
                }
                if (unit_info.hasArmory())
                {
                    owner.buildBuilding(com.oddlabs.tt.model.Race.BUILDING_ARMORY, grid_start_x, grid_start_y);
                }
                for (int i = 0; i < unit_info.numTowers(); i++)
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
                for (int i_2 = 0; i_2 < unit_info.numPeons(); i_2++)
                {
                    com.oddlabs.tt.util.Target t_2 = getTarget(random);
                    new com.oddlabs.tt.model.Unit(owner, t_2.getPositionX(), t_2.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                for (int i_3 = 0; i_3 < unit_info.numRockWarriors(); i_3++)
                {
                    com.oddlabs.tt.util.Target t_3 = getTarget(random);
                    new com.oddlabs.tt.model.Unit(owner, t_3.getPositionX(), t_3.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                }
                for (int i_4 = 0; i_4 < unit_info.numIronWarriors(); i_4++)
                {
                    com.oddlabs.tt.util.Target t_4 = getTarget(random);
                    new com.oddlabs.tt.model.Unit(owner, t_4.getPositionX(), t_4.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                }
                for (int i_5 = 0; i_5 < unit_info.numRubberWarriors(); i_5++)
                {
                    com.oddlabs.tt.util.Target t_5 = getTarget(random);
                    new com.oddlabs.tt.model.Unit(owner, t_5.getPositionX(), t_5.getPositionY(), null, owner.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
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

// Translated from Player.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 41
    public class Player : java.lang.Object, com.oddlabs.tt.player.PlayerInterface
    {
        public const int INITIAL_UNIT_COUNT = 20;
        public const int DEFAULT_MAX_UNIT_COUNT = 250;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.player.PlayerInfo player_info;
        public com.oddlabs.tt.model.Army units;
        public com.oddlabs.tt.model.SupplyContainer unit_count;
        public com.oddlabs.tt.model.SupplyContainer building_count;
        public org.joml.Vector4fc color;
        public com.oddlabs.tt.player.PlayerSkins skins;
        public com.oddlabs.tt.player.AI ai;
        public com.oddlabs.tt.model.Unit chieftain;
        public bool training_chieftain;
        public float start_x;
        public float start_y;
        public int units_lost;
        public int buildings_lost;
        public int units_killed;
        public int buildings_destroyed;
        public int units_moved;
        public int weapons_thrown;
        public int magics;
        public int tree_harvested;
        public int rock_harvested;
        public int iron_harvested;
        public int rubber_harvested;
        public bool can_build_chieftains;
        public bool can_repair;
        public bool can_attack;
        public bool[] can_build;
        public bool can_move;
        public bool can_exit_towers;
        public bool can_use_rubber;
        public bool can_set_rally;
        public bool can_harvest;
        public bool can_build_armies;
        public bool can_build_weapons;
        public bool can_transport;
        public bool[] can_do_magic;
        public float hit_bonus;
        public int preferred_speed;
        // Java: 107
        public void changePreferredGamespeed(int delta)
        {
            int old_speed = getGamespeed();
            int new_speed = java.lang.Math.clamp(old_speed + delta, com.oddlabs.matchmaking.Game.GAMESPEED_PAUSE, com.oddlabs.matchmaking.Game.GAMESPEED_LUDICROUS);
            setPreferredGamespeed(new_speed);
        }
        // Java: 114
        public void setPreferredGamespeed(int speed)
        {
            if (!com.oddlabs.tt.landscape.World.isValidPreferredGamespeed(speed))
            {
                return;
            }
            if (preferred_speed != speed)
            {
                int old_speed = preferred_speed;
                this.preferred_speed = speed;
                if (com.oddlabs.tt.landscape.World.isValidGamespeed(preferred_speed) && com.oddlabs.tt.landscape.World.isValidGamespeed(old_speed))
                {
                    world.getNotificationListener().playerGamespeedChanged();
                }
                world.gamespeedChanged();
            }
        }
        // Java: 127
        public int getGamespeed()
        {
            return (com.oddlabs.tt.landscape.World.isValidGamespeed(preferred_speed) ? preferred_speed : world.getGamespeed());
        }
        // Java: 131
        public void notifyOrder(com.oddlabs.tt.util.Target target)
        {
            if (target == null)
            {
                return;
            }
            world.getNotificationListener().playerOrder(this, target.getPositionX(), target.getPositionY());
        }
        // Java: 137
        public void viewCamera(float target_x, float target_y, float target_z, float horiz_angle, float vert_angle)
        {
            world.getNotificationListener().playerCamera(this, target_x, target_y, target_z, horiz_angle, vert_angle);
        }
        // Java: 142
        public void viewCursor(float x, float y, bool on_map)
        {
            world.getNotificationListener().playerCursor(this, x, y, on_map);
        }
        // Java: 147
        public void viewSelection(com.oddlabs.tt.model.Selectable[] selection)
        {
            world.getNotificationListener().playerSelection(this, selection);
        }
        // Java: 152
        public void viewMapMode(bool on)
        {
            world.getNotificationListener().playerMapMode(this, on);
        }
        // Java: 157
        public void viewTargeting(bool on)
        {
            world.getNotificationListener().playerTargeting(this, on);
        }
        // Java: 162
        public void viewPanelMenu(int submenu)
        {
            world.getNotificationListener().playerPanelMenu(this, submenu);
        }
        // Java: 167
        public void viewPlacing(int building_index, int grid_x, int grid_y, bool placing)
        {
            world.getNotificationListener().playerPlacing(this, building_index, grid_x, grid_y, placing);
        }
        // Java: 172
        public void viewSelectionBox(float x1, float y1, float x2, float y2, bool active)
        {
            world.getNotificationListener().playerSelectionBox(this, x1, y1, x2, y2, active);
        }
        // Java: 177
        public void viewBeacon(float x, float y)
        {
            world.getNotificationListener().playerBeacon(this, x, y);
        }
        // Java: 182
        public int getPreferredGamespeed()
        {
            return preferred_speed;
        }
        // Java: 186
        public float getHitBonus()
        {
            return hit_bonus;
        }
        // Java: 190
        public void setHitBonus(float bonus)
        {
            this.hit_bonus = bonus;
        }
        // Java: 194
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 198
        public void enableArmies(bool enabled)
        {
            can_build_armies = enabled;
        }
        // Java: 202
        public void enableWeapons(bool enabled)
        {
            can_build_weapons = enabled;
        }
        // Java: 206
        public void enableTransporting(bool enabled)
        {
            can_transport = enabled;
        }
        // Java: 210
        public void enableHarvesting(bool enabled)
        {
            can_harvest = enabled;
        }
        // Java: 214
        public void enableRubber(bool enabled)
        {
            can_use_rubber = enabled;
        }
        // Java: 218
        public void enableChieftains(bool enabled)
        {
            can_build_chieftains = enabled;
        }
        // Java: 222
        public void enableTowerExits(bool enabled)
        {
            can_exit_towers = enabled;
        }
        // Java: 226
        public void enableRepairing(bool enabled)
        {
            can_repair = enabled;
        }
        // Java: 230
        public void enableBuilding(int building, bool enabled)
        {
            can_build[building] = enabled;
        }
        // Java: 234
        public void enableAttacking(bool enabled)
        {
            can_attack = enabled;
        }
        // Java: 238
        public void enableRallyPoints(bool enabled)
        {
            can_set_rally = enabled;
        }
        // Java: 242
        public void enableMoving(bool enabled)
        {
            can_move = enabled;
        }
        // Java: 246
        public bool canTransport()
        {
            return can_transport;
        }
        // Java: 250
        public bool canBuildWeapons()
        {
            return can_build_weapons;
        }
        // Java: 254
        public bool canHarvest()
        {
            return can_harvest;
        }
        // Java: 258
        public bool canBuildArmies()
        {
            return can_build_armies;
        }
        // Java: 262
        public bool canSetRallyPoints()
        {
            return can_set_rally;
        }
        // Java: 266
        public bool canUseRubber()
        {
            return can_use_rubber;
        }
        // Java: 270
        public void enableMagic(int magic_index, bool enabled)
        {
            can_do_magic[magic_index] = enabled;
        }
        // Java: 274
        public bool canDoMagic(int magic_index)
        {
            return can_do_magic[magic_index];
        }
        // Java: 278
        public bool canExitTowers()
        {
            return can_exit_towers;
        }
        // Java: 282
        public bool canAttack()
        {
            return can_attack;
        }
        // Java: 286
        public bool canMove()
        {
            return can_move;
        }
        // Java: 290
        public bool canBuild(int building)
        {
            return can_build[building] && getBuildingCountContainer().getNumSupplies() < world.getMaxBuildingCount();
        }
        // Java: 294
        public bool canRepair()
        {
            return can_repair;
        }
        // Java: 298
        public bool canBuildChieftains()
        {
            return can_build_chieftains;
        }
        // Java: 302
        public override string toString()
        {
            return java.lang.JO.toString(player_info);
        }
        // Java: 307
        public com.oddlabs.tt.player.PlayerInfo getPlayerInfo()
        {
            return player_info;
        }
        // Java: 311
        public void setAI(com.oddlabs.tt.player.AI ai)
        {
            this.ai = ai;
        }
        // Java: 315
        public com.oddlabs.tt.player.AI getAI()
        {
            return ai;
        }
        // Java: 319
        public com.oddlabs.tt.model.Building buildBuilding(int building_type, int grid_x, int grid_y)
        {
            com.oddlabs.tt.player.BuildingSiteScanFilter filter = new com.oddlabs.tt.player.BuildingSiteScanFilter(world.getUnitGrid(), getRace().getBuildingTemplate(building_type), 40, true);
            world.getUnitGrid().scan(filter, grid_x, grid_y);
            java.util.List target_list = filter.getResult();
            com.oddlabs.tt.model.Building b = null;
            if (!target_list.isEmpty())
            {
                com.oddlabs.tt.util.Target t = ((com.oddlabs.tt.landscape.LandscapeTarget)target_list.getFirst());
                b = getRace().getBuildingTemplate(building_type).create(this, t.getGridX(), t.getGridY());
                b.place();
                b.repair(1000);
            }
            return b;
        }
        // Java: 334
        public void init(float[] starting_location)
        {
            this.start_x = starting_location[0];
            this.start_y = starting_location[1];
        }
        // Java: 339
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y)
        {
            return findNearestEnemy(start_x, start_y, new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return true;
            }));
        }
        // Java: 343
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y, com.oddlabs.tt.model.Selectable target)
        {
            return findNearestEnemy(start_x, start_y, target, com.oddlabs.tt.model.Selectable.genericClass());
        }
        // Java: 347
        public int getStatus()
        {
            return getUnits().getSet().stream().mapToInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Selectable)a0).getStatusValue(); })).sum();
        }
        // Java: 351
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y, com.oddlabs.tt.model.Selectable target, System.Type type)
        {
            return findNearestEnemy(start_x, start_y, new java.util.function.Predicate_L((object s_l) =>
            {
                com.oddlabs.tt.model.Selectable s = (com.oddlabs.tt.model.Selectable)s_l;
                return type.isInstance(s) && ((object)s != (object)target);
            }));
        }
        // Java: 356
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y, java.util.function.Predicate filter)
        {
            int best_dist_squared = java.lang.Integer.MAX_VALUE;
            com.oddlabs.tt.model.Selectable best_target = null;
            foreach (com.oddlabs.tt.player.Player player in world.getPlayers())
            {
                if (isEnemy(player))
                {
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                    {
                        if (!filter.test(s))
                        {
                            continue;
                        }
                        int dx = s.getGridX() - start_x;
                        int dy = s.getGridY() - start_y;
                        int dist_squared = dx * dx + dy * dy;
                        if (best_dist_squared > dist_squared)
                        {
                            best_dist_squared = dist_squared;
                            best_target = s;
                        }
                    }
                }
            }
            return best_target;
        }
        // Java: 379
        public com.oddlabs.tt.model.Selectable findNearestEnemyBuilding(int start_x, int start_y)
        {
            return findNearestEnemy(start_x, start_y, null, typeof(com.oddlabs.tt.model.LandBuilding));
        }
        // Java: 383
        public com.oddlabs.tt.model.Race getRace()
        {
            return getWorld().getRacesResources().getRace(player_info.getRace());
        }
        // Java: 387
        public com.oddlabs.tt.player.PlayerSkins getSkins()
        {
            return skins;
        }
        // Java: 395
        public void setSkins(com.oddlabs.tt.player.PlayerSkins skins)
        {
            this.skins = skins;
        }
        // Java: 399
        public com.oddlabs.tt.model.SupplyContainer getUnitCountContainer()
        {
            return unit_count;
        }
        // Java: 403
        public com.oddlabs.tt.model.SupplyContainer getBuildingCountContainer()
        {
            return building_count;
        }
        // Java: 407
        public void setActiveChieftain(com.oddlabs.tt.model.Unit chieftain)
        {
            this.chieftain = chieftain;
        }
        // Java: 411
        public com.oddlabs.tt.model.Building getArmory()
        {
            com.oddlabs.tt.model.Selectable[][] lists = classifyUnits();
            foreach (com.oddlabs.tt.model.Selectable[] list in lists)
            {
                com.oddlabs.tt.model.Selectable s = list[0];
                if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.NullController) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD_ARMIES))
                {
                    return ((com.oddlabs.tt.model.Building)s);
                }
            }
            return null;
        }
        // Java: 423
        public com.oddlabs.tt.model.Building getQuarters()
        {
            com.oddlabs.tt.model.Selectable[][] lists = classifyUnits();
            foreach (com.oddlabs.tt.model.Selectable[] list in lists)
            {
                com.oddlabs.tt.model.Selectable s = list[0];
                if ((s.getPrimaryController() is com.oddlabs.tt.model.behaviour.NullController) && s.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.REPRODUCE))
                {
                    return ((com.oddlabs.tt.model.Building)s);
                }
            }
            return null;
        }
        // Java: 435
        public bool isAlive()
        {
            return com.oddlabs.tt.gamemode.GameModeRegistry.get(world.getGameMode()).isPlayerAlive(this);
        }
        // Java: 440
        public bool hasActiveChieftain()
        {
            return chieftain != null;
        }
        // Java: 444
        public com.oddlabs.tt.model.Unit getChieftain()
        {
            return chieftain;
        }
        // Java: 448
        public void setTrainingChieftain(bool training_chieftain)
        {
            this.training_chieftain = training_chieftain;
        }
        // Java: 453
        public bool isTrainingChieftain()
        {
            return training_chieftain;
        }
        // Java: 457
        public org.joml.Vector4fc getColor()
        {
            return color;
        }
        // Java: 461
        public void deployUnits(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int num_units)
        {
            if (isValid(building))
            {
                building.deployUnits(type, num_units);
            }
        }
        // Java: 467
        public int getGathererCount(System.Type supply_type, com.oddlabs.tt.model.Building building)
        {
            int count = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units.getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit) && (s.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController gather))
                {
                    if (((object)gather.getSupplyType() == (object)supply_type) && ((object)gather.getAssignedBuilding() == (object)building))
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        // Java: 479
        public void recallGatherers(com.oddlabs.tt.model.Building building, System.Type supply_type, int amount)
        {
            if (!isValid(building))
            {
                return;
            }
            float bx = building.getPositionX();
            float by = building.getPositionY();
            for (int i = 0; i < amount; i++)
            {
                com.oddlabs.tt.model.Unit nearest = null;
                float nearest_dist_sq = java.lang.Float.MAX_VALUE;
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units.getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit unit) && (s.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController gather))
                    {
                        if (((object)gather.getSupplyType() == (object)supply_type) && ((object)gather.getAssignedBuilding() == (object)building))
                        {
                            float dx = (float)(s.getPositionX() - bx);
                            float dy = (float)(s.getPositionY() - by);
                            float dist_sq = (float)((float)(dx * dx) + (float)(dy * dy));
                            if (dist_sq < nearest_dist_sq)
                            {
                                nearest_dist_sq = dist_sq;
                                nearest = unit;
                            }
                        }
                    }
                }
                if (nearest != null)
                {
                    nearest.initTarget(building, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
                else
                {
                    break;
                }
            }
        }
        // Java: 512
        public void createHarvesters(com.oddlabs.tt.model.Building building, int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            if (isValid(building))
            {
                building.createHarvesters(num_tree, num_rock, num_iron, num_rubber);
            }
        }
        // Java: 518
        public void buildRockWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
            if (isValid(building))
            {
                building.buildWeapons(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), num_weapons, infinite);
            }
        }
        // Java: 524
        public void buildIronWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
            if (isValid(building))
            {
                building.buildWeapons(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), num_weapons, infinite);
            }
        }
        // Java: 530
        public void buildRubberWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
            if (isValid(building))
            {
                building.buildWeapons(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), num_weapons, infinite);
            }
        }
        // Java: 536
        public void doMagic(com.oddlabs.tt.model.Unit chieftain, int magic)
        {
            if (isValid(chieftain))
            {
                chieftain.doMagic(magic, true);
            }
        }
        // Java: 542
        public void exitTower(com.oddlabs.tt.model.Building building)
        {
            if (isValid(building))
            {
                building.exitTower();
            }
        }
        // Java: 548
        public void trainChieftain(com.oddlabs.tt.model.Building building, bool start)
        {
            if (isValid(building))
            {
                building.trainChieftain(start);
            }
        }
        // Java: 554
        public void deployChieftainFromShip(com.oddlabs.tt.model.Building building)
        {
            if (isValid(building) && (building is com.oddlabs.tt.model.Ship ship))
            {
                ship.deployChieftain();
            }
        }
        // Java: 560
        public void placeBuilding(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y)
        {
            com.oddlabs.tt.model.Building building = getRace().getBuildingTemplate(template_id).create(this, placing_grid_x, placing_grid_y);
            notifyOrder(building);
            foreach (com.oddlabs.tt.model.Selectable selection1 in selection)
            {
                if (isValid(selection1))
                {
                    selection1.initTarget(building, com.oddlabs.tt.model.Action.DEFAULT, false);
                }
            }
        }
        // Java: 573
        public void setRallyPoint(com.oddlabs.tt.model.Building building, com.oddlabs.tt.util.Target target)
        {
            if (isValid(building) && target != null)
            {
                building.setRallyPoint(target);
                notifyOrder(target);
            }
        }
        // Java: 581
        public void setRallyPoint(com.oddlabs.tt.model.Building building, int grid_x, int grid_y)
        {
            setRallyPoint(building, new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y));
        }
        // Java: 586
        public void setTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (selection.Length == 0)
            {
                return;
            }
            notifyOrder(target);
            foreach (com.oddlabs.tt.model.Selectable selection1 in selection)
            {
                if (isValid(selection1))
                {
                    selection1.initTarget(target, action, aggressive);
                }
            }
        }
        // Java: 598
        public void killSelection(com.oddlabs.tt.model.Selectable[] selection)
        {
            foreach (com.oddlabs.tt.model.Selectable selection1 in selection)
            {
                if (selection1 != null)
                {
                    selection1.hit(10000, 0.0f, 1.0f, this);
                }
            }
        }
        // Java: 606
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target)
        {
            if (selection.Length == 0)
            {
                return;
            }
            notifyOrder(target);
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]))
                {
                    selection[i].initTarget(target, com.oddlabs.tt.model.Action.MOVE, false);
                }
            }
        }
        // Java: 616
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
            if (selection.Length == 0)
            {
                return;
            }
            int grid_size = world.getUnitGrid().getGridSize();
            if (grid_x < 0 || grid_x >= grid_size || grid_y < 0 || grid_y >= grid_size)
            {
                return;
            }
            com.oddlabs.tt.util.Target target = new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y);
            notifyOrder(target);
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]))
                {
                    selection[i].initTarget(target, com.oddlabs.tt.model.Action.MOVE, false);
                }
            }
        }
        // Java: 629
        public void setLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (selection.Length == 0)
            {
                return;
            }
            int grid_size = world.getUnitGrid().getGridSize();
            if (grid_x < 0 || grid_x >= grid_size || grid_y < 0 || grid_y >= grid_size)
            {
                return;
            }
            notifyOrder(new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y));
            com.oddlabs.tt.util.Target[] targets = world.getUnitGrid().findGridTargets(grid_x, grid_y, selection.Length, selection.Length != 1);
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]))
                {
                    selection[i].initTarget(targets[i], action, aggressive);
                }
            }
        }
        // Java: 645
        public bool isValid(com.oddlabs.tt.model.Selectable s)
        {
            return s != null && !s.isDead() && ((object)s.getOwner() == (object)this);
        }
        // Java: 649
        public float getStartX()
        {
            return start_x;
        }
        // Java: 653
        public void setStartX(float x)
        {
            start_x = x;
        }
        // Java: 657
        public float getStartY()
        {
            return start_y;
        }
        // Java: 661
        public void setStartY(float y)
        {
            start_y = y;
        }
        // Java: 665
        public bool isEnemy(com.oddlabs.tt.player.Player other_player)
        {
            if (other_player.player_info.getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || this.player_info.getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return false;
            }
            return other_player.player_info.getTeam() != this.player_info.getTeam();
        }
        // Java: 673
        public bool teamHasBuilding()
        {
            foreach (com.oddlabs.tt.player.Player player in world.getPlayers())
            {
                if (player.getPlayerInfo().getTeam() == player_info.getTeam() && player.getBuildingCountContainer().getNumSupplies() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        // Java: 683
        public com.oddlabs.tt.model.Army getUnits()
        {
            return units;
        }
        // Java: 687
        public com.oddlabs.tt.model.Selectable[][] classifyUnits()
        {
            java.util.Map map = ((java.util.LinkedHashMap)units.getSet().stream().collect(java.util.stream.Collectors.groupingBy(new java.util.function.Function_L((object u_l) =>
            {
                com.oddlabs.tt.model.Selectable u = (com.oddlabs.tt.model.Selectable)u_l;
                return u.getPrimaryController().getKey();
            }), new java.util.function.Supplier_L(() => { return new java.util.LinkedHashMap(); }), java.util.stream.Collectors.toList())));
            return ((com.oddlabs.tt.model.Selectable[][])map.values().stream().map(new java.util.function.Function_L((object list_l) =>
            {
                java.util.List list = (java.util.List)list_l;
                return ((com.oddlabs.tt.model.Selectable[])list.toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Selectable[a0]; })));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Selectable[a0][]; })));
        }
        // Java: 693
        public void magicCast()
        {
            magics++;
        }
        // Java: 697
        public int getMagics()
        {
            return magics;
        }
        // Java: 701
        public void weaponThrown()
        {
            weapons_thrown++;
        }
        // Java: 705
        public int getWeaponsThrown()
        {
            return weapons_thrown;
        }
        // Java: 709
        public void unitMoved()
        {
            units_moved++;
        }
        // Java: 713
        public int getUnitsMoved()
        {
            return units_moved;
        }
        // Java: 717
        public void unitLost()
        {
            units_lost++;
        }
        // Java: 722
        public int getUnitsLost()
        {
            return units_lost;
        }
        // Java: 726
        public void buildingLost()
        {
            buildings_lost++;
        }
        // Java: 730
        public int getBuildingsLost()
        {
            return buildings_lost;
        }
        // Java: 734
        public void unitKilled()
        {
            units_killed++;
        }
        // Java: 738
        public int getUnitsKilled()
        {
            return units_killed;
        }
        // Java: 742
        public void buildingDestroyed()
        {
            buildings_destroyed++;
        }
        // Java: 746
        public int getBuildingsDestroyed()
        {
            return buildings_destroyed;
        }
        // Java: 750
        public void harvested(System.Type type)
        {
            if (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
            {
                tree_harvested++;
            }
            else if (((object)type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
            {
                rock_harvested++;
            }
            else if (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
            {
                iron_harvested++;
            }
            else if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
            {
                rubber_harvested++;
            }
            else
            {
                throw new System.Exception();
            }
        }
        // Java: 763
        public int getTreeHarvested()
        {
            return tree_harvested;
        }
        // Java: 767
        public int getRockHarvested()
        {
            return rock_harvested;
        }
        // Java: 771
        public int getIronHarvested()
        {
            return iron_harvested;
        }
        // Java: 775
        public int getRubberHarvested()
        {
            return rubber_harvested;
        }
        private void init_Player()
        {
            this.units = new com.oddlabs.tt.model.Army();
            this.skins = com.oddlabs.tt.player.PlayerSkins.NONE;
            this.ai = null;
            this.chieftain = null;
            this.training_chieftain = false;
            this.can_build_chieftains = true;
            this.can_repair = true;
            this.can_attack = true;
            this.can_build = new bool[com.oddlabs.tt.model.Race.NUM_BUILDINGS];
            this.can_move = true;
            this.can_exit_towers = true;
            this.can_use_rubber = true;
            this.can_set_rally = true;
            this.can_harvest = true;
            this.can_build_armies = true;
            this.can_build_weapons = true;
            this.can_transport = true;
            this.can_do_magic = new bool[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
            this.preferred_speed = com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE;
        }
        public Player(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.player.PlayerInfo player_info, org.joml.Vector4fc color) : base()
        {
            init_Player();
            this.world = world;
            this.color = color;
            java.util.Arrays.fill(can_do_magic, true);
            java.util.Arrays.fill(can_build, true);
            this.player_info = player_info;
            this.unit_count = new com.oddlabs.tt.model.SupplyContainer(world.getMaxUnitCount());
            this.building_count = new com.oddlabs.tt.model.SupplyContainer(world.getMaxBuildingCount());
        }
    }
}

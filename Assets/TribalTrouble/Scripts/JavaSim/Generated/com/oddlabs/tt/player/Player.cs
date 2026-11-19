// Translated from Player.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 44
    public class Player : java.lang.Object, com.oddlabs.tt.player.PlayerInterface
    {
        public const int INITIAL_UNIT_COUNT = 20;
        public const int MAX_BUILDING_COUNT = 20;
        public const int DEFAULT_MAX_UNIT_COUNT = 250;
        public const int NEW_TIDES_MAX_UNIT_COUNT = 500;
        public const int DEFAULT_MAX_CHIEFTAINS = 1;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.player.PlayerInfo player_info;
        public com.oddlabs.tt.model.Army units;
        public com.oddlabs.tt.model.SupplyContainer unit_count;
        public com.oddlabs.tt.model.SupplyContainer building_count;
        public org.joml.Vector4fc color;
        public com.oddlabs.tt.player.AI ai;
        public com.oddlabs.tt.model.Unit chieftain;
        public java.util.List extra_chieftains;
        public int training_chieftain_count;
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
        public int[] rare_harvested;
        public bool can_build_chieftains;
        public bool can_use_chiefs_courage;
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
        public int[] upgrade_levels;
        public com.oddlabs.tt.model.ResearchContainer[] researching;
        public float hit_bonus;
        public int preferred_speed;
        // Java: 135
        public void changePreferredGamespeed(int delta)
        {
            int old_speed = getGamespeed();
            int new_speed = java.lang.Math.clamp(old_speed + delta, com.oddlabs.matchmaking.Game.GAMESPEED_PAUSE, com.oddlabs.matchmaking.Game.GAMESPEED_LUDICROUS);
            setPreferredGamespeed(new_speed);
        }
        // Java: 142
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
        // Java: 155
        public int getGamespeed()
        {
            return (com.oddlabs.tt.landscape.World.isValidGamespeed(preferred_speed) ? preferred_speed : world.getGamespeed());
        }
        // Java: 159
        public int getPreferredGamespeed()
        {
            return preferred_speed;
        }
        // Java: 163
        public float getHitBonus()
        {
            return hit_bonus;
        }
        // Java: 167
        public void setHitBonus(float bonus)
        {
            this.hit_bonus = bonus;
        }
        // Java: 171
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 175
        public void enableArmies(bool enabled)
        {
            can_build_armies = enabled;
        }
        // Java: 179
        public void enableWeapons(bool enabled)
        {
            can_build_weapons = enabled;
        }
        // Java: 183
        public void enableTransporting(bool enabled)
        {
            can_transport = enabled;
        }
        // Java: 187
        public void enableHarvesting(bool enabled)
        {
            can_harvest = enabled;
        }
        // Java: 191
        public void enableRubber(bool enabled)
        {
            can_use_rubber = enabled;
        }
        // Java: 195
        public void enableChieftains(bool enabled)
        {
            can_build_chieftains = enabled;
        }
        // Java: 206
        public com.oddlabs.tt.player.Formation resolveFormation(com.oddlabs.tt.model.Selectable[] selection)
        {
            java.util.Map counts = new java.util.HashMap();
            foreach (com.oddlabs.tt.model.Selectable s in selection)
            {
                if (!isValid(s))
                {
                    continue;
                }
                com.oddlabs.tt.player.Formation f = s.getFormation();
                if (f != null)
                {
                    counts.merge(f, 1, new java.util.function.BiFunction_L((object a0, object a1) => { return java.lang.Integer.sum(((int)a0), ((int)a1)); }));
                }
            }
            com.oddlabs.tt.player.Formation best = null;
            int best_count = 0;
            foreach (java.util.Map.Entry entry in java.lang.JIter.of(counts.entrySet()))
            {
                if (((int)((int?)entry.getValue())) > best_count)
                {
                    best_count = ((int)((int?)entry.getValue()));
                    best = ((com.oddlabs.tt.player.Formation)entry.getKey());
                }
            }
            return (best != null ? best : com.oddlabs.tt.player.Formation.LOOSE);
        }
        // Java: 230
        public void setFormation(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.player.Formation formation)
        {
            foreach (com.oddlabs.tt.model.Selectable s in selection)
            {
                if (isValid(s))
                {
                    s.setFormation(formation);
                }
            }
        }
        // Java: 243
        public void enableChiefsCourage(bool enabled)
        {
            can_use_chiefs_courage = enabled;
        }
        // Java: 247
        public bool canUseChiefsCourage()
        {
            return can_use_chiefs_courage;
        }
        public int chiefs_courage_uses;
        // Java: 258
        public int incrementAndGetChiefsCourageUses()
        {
            return ++chiefs_courage_uses;
        }
        public const float CHIEFS_COURAGE_COOLDOWN_SECONDS = 40.0f;
        public float chiefs_courage_cooldown_remaining;
        // Java: 276
        public bool isChiefsCourageReady()
        {
            return chiefs_courage_cooldown_remaining <= 0.0f;
        }
        // Java: 284
        public void chiefsCourageTriggered()
        {
            chiefs_courage_cooldown_remaining = com.oddlabs.tt.player.Player.CHIEFS_COURAGE_COOLDOWN_SECONDS;
        }
        // Java: 293
        public void tickChiefsCourageCooldown(float t)
        {
            if (chiefs_courage_cooldown_remaining > 0.0f)
            {
                chiefs_courage_cooldown_remaining = java.lang.Math.max(0.0f, (float)(chiefs_courage_cooldown_remaining - t));
            }
        }
        // Java: 298
        public void enableTowerExits(bool enabled)
        {
            can_exit_towers = enabled;
        }
        // Java: 302
        public void enableRepairing(bool enabled)
        {
            can_repair = enabled;
        }
        public java.util.BitSet locked;
        // Java: 309
        public void @lock(int key)
        {
            locked.set(key);
        }
        // Java: 313
        public void unlock(int key)
        {
            locked.clear(key);
        }
        // Java: 317
        public bool isLocked(int key)
        {
            return key >= 0 && locked.get(key);
        }
        // Java: 321
        public void enableBuilding(int building, bool enabled)
        {
            can_build[building] = enabled;
        }
        // Java: 325
        public void enableAttacking(bool enabled)
        {
            can_attack = enabled;
        }
        // Java: 329
        public void enableRallyPoints(bool enabled)
        {
            can_set_rally = enabled;
        }
        // Java: 333
        public void enableMoving(bool enabled)
        {
            can_move = enabled;
        }
        // Java: 337
        public bool canTransport()
        {
            return can_transport;
        }
        // Java: 341
        public bool canBuildWeapons()
        {
            return can_build_weapons;
        }
        // Java: 345
        public bool canHarvest()
        {
            return can_harvest;
        }
        // Java: 349
        public bool canBuildArmies()
        {
            return can_build_armies;
        }
        // Java: 353
        public bool canSetRallyPoints()
        {
            return can_set_rally;
        }
        // Java: 357
        public bool canUseRubber()
        {
            return can_use_rubber;
        }
        // Java: 361
        public void enableMagic(int magic_index, bool enabled)
        {
            can_do_magic[magic_index] = enabled;
        }
        public bool hold_weapons;
        // Java: 368
        public bool holdsWeapons()
        {
            return hold_weapons;
        }
        // Java: 372
        public void setHoldWeapons(bool hold)
        {
            hold_weapons = hold;
        }
        // Java: 376
        public int getUpgradeLevel(int upgrade_v)
        {
            return upgrade_levels[upgrade_v];
        }
        // Java: 381
        public bool isResearching(int upgrade_v)
        {
            com.oddlabs.tt.model.ResearchContainer r = researching[upgrade_v];
            return r != null && r.isResearching() && !r.getBuilding().isDead();
        }
        // Java: 386
        public void setResearching(int upgrade_v, com.oddlabs.tt.model.ResearchContainer container)
        {
            researching[upgrade_v] = container;
        }
        // Java: 390
        public float getUpgradeSpeedMultiplier()
        {
            int l = upgrade_levels[com.oddlabs.tt.model.ResearchContainer.SPEED];
            return (float)((float)(1.0f + (float)(0.1f * java.lang.Math.min(l, 3))) + (float)(0.05f * java.lang.Math.max(0, l - 3)));
        }
        // Java: 395
        public float getUpgradeHarvestMultiplier()
        {
            return (float)(1.0f + (float)(0.25f * upgrade_levels[com.oddlabs.tt.model.ResearchContainer.TOOLS]));
        }
        // Java: 399
        public float getUpgradeRangeBonus()
        {
            int l = upgrade_levels[com.oddlabs.tt.model.ResearchContainer.RANGE];
            return (float)(java.lang.Math.min(l, 3) + (float)(0.5f * java.lang.Math.max(0, l - 3)));
        }
        // Java: 404
        public float getUpgradeGrowthMultiplier()
        {
            return (float)(1.0f + (float)(0.3f * upgrade_levels[com.oddlabs.tt.model.ResearchContainer.GROWTH]));
        }
        // Java: 408
        public float getUpgradeTowerRangeBonus()
        {
            return (float)(2.0f * upgrade_levels[com.oddlabs.tt.model.ResearchContainer.TOWER_RANGE]);
        }
        // Java: 413
        public float getUpgradeAttackSpeedMultiplier()
        {
            int level = upgrade_levels[com.oddlabs.tt.model.ResearchContainer.RACE_2];
            if (level == 0 || !com.oddlabs.tt.model.ExtendedCombat.isVikings(this))
            {
                return 1.0f;
            }
            return (float)(1.0f + (float)(0.15f * level));
        }
        // Java: 420
        public float getUpgradeLoreMultiplier()
        {
            return (float)(1.0f + (float)(0.25f * upgrade_levels[com.oddlabs.tt.model.ResearchContainer.LORE]));
        }
        public int[] research_cap;
        public int research_cost_percent;
        public bool campaign_tribe;
        // Java: 432
        public void setCampaignTribe(bool b)
        {
            campaign_tribe = b;
        }
        // Java: 436
        public bool isCampaignTribe()
        {
            return campaign_tribe;
        }
        // Java: 440
        public void setResearchCap(int[] caps)
        {
            this.research_cap = caps;
        }
        // Java: 445
        public int getResearchCap(int upgrade_v)
        {
            int max = com.oddlabs.tt.model.ResearchContainer.getMaxLevel(upgrade_v);
            return (research_cap == null || upgrade_v >= research_cap.Length ? max : java.lang.Math.min(max, research_cap[upgrade_v]));
        }
        // Java: 450
        public void setResearchCostPercent(int percent)
        {
            this.research_cost_percent = percent;
        }
        // Java: 454
        public int getResearchCostPercent()
        {
            return research_cost_percent;
        }
        // Java: 459
        public void upgrade(int upgrade_v)
        {
            if (upgrade_levels[upgrade_v] >= com.oddlabs.tt.model.ResearchContainer.getMaxLevel(upgrade_v))
            {
                return;
            }
            upgrade_levels[upgrade_v]++;
            if (upgrade_v == com.oddlabs.tt.model.ResearchContainer.ARMOR)
            {
                foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units.getSet()))
                {
                    if ((s is com.oddlabs.tt.model.Unit u) && !u.isDead() && u.getAbilities().isWarrior())
                    {
                        u.heal(1);
                    }
                }
            }
            else if (upgrade_v == com.oddlabs.tt.model.ResearchContainer.STURDY)
            {
                foreach (com.oddlabs.tt.model.Selectable s_2 in java.lang.JIter.of(units.getSet()))
                {
                    if ((s_2 is com.oddlabs.tt.model.Unit u_2) && !u_2.isDead() && u_2.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.HARVEST))
                    {
                        u_2.heal(1);
                    }
                }
            }
            else if (upgrade_v == com.oddlabs.tt.model.ResearchContainer.VIGOR)
            {
                foreach (com.oddlabs.tt.model.Selectable s_3 in java.lang.JIter.of(units.getSet()))
                {
                    if ((s_3 is com.oddlabs.tt.model.Unit u_3) && !u_3.isDead() && u_3.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC))
                    {
                        u_3.heal(java.lang.Math.max(1, java.lang.Math.round((float)(u_3.getEffectiveMaxHitPoints() * 0.2f))));
                    }
                }
            }
        }
        // Java: 481
        public bool canDoMagic(int magic_index)
        {
            if (magic_index >= com.oddlabs.tt.model.RacesResources.NUM_CLASSIC_MAGIC && !world.isExtended())
            {
                return false;
            }
            return can_do_magic[magic_index];
        }
        // Java: 487
        public bool canExitTowers()
        {
            return can_exit_towers;
        }
        // Java: 491
        public bool canAttack()
        {
            return can_attack;
        }
        // Java: 495
        public bool canMove()
        {
            return can_move;
        }
        // Java: 499
        public bool canBuild(int building)
        {
            if (building >= com.oddlabs.tt.model.Race.NUM_CLASSIC_BUILDINGS && !world.isExtended())
            {
                return false;
            }
            if (world.isExtended() && !withinLimits(building))
            {
                return false;
            }
            if (building == com.oddlabs.tt.model.Race.BUILDING_BRIDGE && getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.BRIDGES) == 0)
            {
                return false;
            }
            return can_build[building] && getBuildingCountContainer().getNumSupplies() < world.getMaxBuildingCount();
        }
        public static int[] FLEET_CAP;
        public static int[] BUILDING_CAP;
        public int chief_serial;
        // Java: 519
        public int nextChiefSerial()
        {
            return chief_serial++;
        }
        // Java: 523
        public int getFleetCap()
        {
            return com.oddlabs.tt.player.Player.FLEET_CAP[java.lang.Math.min(3, getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.FLEET))];
        }
        // Java: 527
        public int getBuildingCap()
        {
            return com.oddlabs.tt.player.Player.BUILDING_CAP[java.lang.Math.min(3, getUpgradeLevel(com.oddlabs.tt.model.ResearchContainer.BUILDINGS))];
        }
        // Java: 532
        public int countOwn(bool ships)
        {
            int n = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units.getSet()))
            {
                if ((s is com.oddlabs.tt.model.Building b) && !b.isDead())
                {
                    bool ship = (b is com.oddlabs.tt.model.Ship);
                    if ((ships ? ship : !ship && ((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getTemplateID() != com.oddlabs.tt.model.Race.BUILDING_PALISADE))
                    {
                        n++;
                    }
                }
            }
            return n;
        }
        // Java: 543
        public bool withinLimits(int building)
        {
            if (building == com.oddlabs.tt.model.Race.BUILDING_PALISADE)
            {
                return true;
            }
            if (building == com.oddlabs.tt.model.Race.BUILDING_SHIP)
            {
                return countOwn(true) < getFleetCap();
            }
            return countOwn(false) < getBuildingCap();
        }
        // Java: 551
        public bool canRepair()
        {
            return can_repair;
        }
        // Java: 555
        public bool canBuildChieftains()
        {
            return can_build_chieftains;
        }
        // Java: 559
        public override string toString()
        {
            return java.lang.JO.toString(player_info);
        }
        // Java: 564
        public com.oddlabs.tt.player.PlayerInfo getPlayerInfo()
        {
            return player_info;
        }
        // Java: 568
        public void setAI(com.oddlabs.tt.player.AI ai)
        {
            this.ai = ai;
        }
        // Java: 572
        public com.oddlabs.tt.player.AI getAI()
        {
            return (ai_asleep ? (com.oddlabs.tt.player.AI)(null) : ai);
        }
        public bool ai_asleep;
        public int last_order_tick;
        // Java: 584
        public com.oddlabs.tt.player.AI getStandbyAI()
        {
            return ai;
        }
        // Java: 588
        public bool isAIAsleep()
        {
            return ai_asleep;
        }
        // Java: 592
        public void setAIAsleep(bool asleep)
        {
            if (ai == null || asleep == ai_asleep)
            {
                return;
            }
            ai_asleep = asleep;
            if (asleep)
            {
                ai.sleep();
            }
            else
            {
                ai.wake();
            }
            last_order_tick = world.getTick();
        }
        // Java: 603
        public void ordered()
        {
            if (ai_asleep)
            {
                last_order_tick = world.getTick();
            }
        }
        // Java: 609
        public float secondsSinceOrder()
        {
            return (last_order_tick < 0 ? java.lang.Float.MAX_VALUE : (float)((world.getTick() - last_order_tick) * com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK));
        }
        // Java: 615
        public void islandWarsControl(int what)
        {
            if (((object)world.getGameMode() != (object)com.oddlabs.matchmaking.GameMode.ISLAND_WARS))
            {
                return;
            }
            if (what == 0 || what == 1)
            {
                setAIAsleep(what == 0);
                return;
            }
            int slot = what - 100;
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            if (slot < 0 || slot >= players.Length || ((object)players[slot] == (object)this))
            {
                return;
            }
            com.oddlabs.tt.player.Player other = players[slot];
            if (other.isAIAsleep() && other.secondsSinceOrder() >= com.oddlabs.tt.player.Player.TAKE_OVER_SILENCE)
            {
                other.setAIAsleep(false);
            }
        }
        public const float TAKE_OVER_SILENCE = 20.0f;
        public bool naval_commander;
        // Java: 646
        public bool hasNavalCommander()
        {
            return naval_commander;
        }
        // Java: 650
        public void setNavalCommander()
        {
            naval_commander = true;
        }
        // Java: 654
        public bool usesReferenceCommandRules()
        {
            com.oddlabs.tt.player.AI b = (ai != null ? ai.brain() : (com.oddlabs.tt.player.AI)(null));
            return (b is com.oddlabs.tt.player.ai.ExpertAI) || (b is com.oddlabs.tt.player.ultra.UltraAI) || (b is com.oddlabs.tt.player.fable.FableAI) || (b is com.oddlabs.tt.player.gauntlet.GauntletAI);
        }
        // Java: 659
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
                b.repair(((com.oddlabs.tt.model.BuildingTemplate)b.getTemplate()).getMaxHitPoints());
            }
            return b;
        }
        // Java: 682
        public void init(float[] starting_location)
        {
            this.start_x = starting_location[0];
            this.start_y = starting_location[1];
        }
        // Java: 687
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y)
        {
            return findNearestEnemy(start_x, start_y, null);
        }
        // Java: 691
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y, com.oddlabs.tt.model.Selectable target)
        {
            return findNearestEnemy(start_x, start_y, target, com.oddlabs.tt.model.Selectable.genericClass());
        }
        // Java: 695
        public int getStatus()
        {
            return getUnits().getSet().stream().mapToInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.model.Selectable)a0).getStatusValue(); })).sum();
        }
        // Java: 699
        public com.oddlabs.tt.model.Selectable findNearestEnemy(int start_x, int start_y, com.oddlabs.tt.model.Selectable target, System.Type type)
        {
            int best_dist_squared = java.lang.Integer.MAX_VALUE;
            com.oddlabs.tt.model.Selectable best_target = null;
            foreach (com.oddlabs.tt.player.Player player in world.getPlayers())
            {
                if (isEnemy(player))
                {
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                    {
                        if (!(type.isInstance(s)) || ((object)s == (object)target))
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
        // Java: 722
        public com.oddlabs.tt.model.Selectable findNearestEnemyBuilding(int start_x, int start_y)
        {
            return findNearestEnemy(start_x, start_y, null, typeof(com.oddlabs.tt.model.LandBuilding));
        }
        // Java: 726
        public com.oddlabs.tt.model.Selectable findNearestEnemyShip(int start_x, int start_y)
        {
            return findNearestEnemy(start_x, start_y, null, typeof(com.oddlabs.tt.model.Ship));
        }
        // Java: 730
        public com.oddlabs.tt.model.Selectable findNearestEnemyOnBeach(int start_x, int start_y)
        {
            int best_dist_squared = java.lang.Integer.MAX_VALUE;
            sbyte[][] dock = getWorld().getHeightMap().getDockGrid();
            int map_size = getWorld().getHeightMap().getGridUnitsPerWorld();
            com.oddlabs.tt.model.Selectable best_target = null;
            foreach (com.oddlabs.tt.player.Player player in world.getPlayers())
            {
                if (isEnemy(player))
                {
                    foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(player.getUnits().getSet()))
                    {
                        int x = s.getGridX();
                        int y = s.getGridY();
                        int size = java.lang.StrictMath.round(s.getSize());
                        bool on_beach = false;
                        for (int i = 0; i < size && !on_beach; i++)
                        {
                            for (int j = 0; j < size && !on_beach; j++)
                            {
                                int cx = x + i - size / 2;
                                int cy = y + i - size / 2;
                                if (cx < 0 || cx >= map_size || cy < 0 || cy >= map_size)
                                {
                                    continue;
                                }
                                if (dock[cy][cx] != 0)
                                {
                                    on_beach = true;
                                }
                            }
                        }
                        if (!on_beach)
                        {
                            continue;
                        }
                        int dx = x - start_x;
                        int dy = y - start_y;
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
        // Java: 770
        public com.oddlabs.tt.model.Race getRace()
        {
            return getWorld().getRacesResources().getRace(player_info.getRace());
        }
        // Java: 774
        public com.oddlabs.tt.model.SupplyContainer getUnitCountContainer()
        {
            return unit_count;
        }
        // Java: 778
        public com.oddlabs.tt.model.SupplyContainer getBuildingCountContainer()
        {
            return building_count;
        }
        // Java: 782
        public void setActiveChieftain(com.oddlabs.tt.model.Unit chieftain)
        {
            this.chieftain = chieftain;
        }
        // Java: 790
        public void addExtraChieftain(com.oddlabs.tt.model.Unit extra)
        {
            extra_chieftains.add(extra);
        }
        // Java: 796
        public java.util.List getExtraChieftains()
        {
            extra_chieftains.removeIf(new java.util.function.Predicate_L((object a0) => { return ((com.oddlabs.tt.model.Unit)a0).isDead(); }));
            return extra_chieftains;
        }
        // Java: 801
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
        // Java: 813
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
        // Java: 825
        public bool isAlive()
        {
            return com.oddlabs.tt.gamemode.GameModeRegistry.get(world.getGameMode()).isPlayerAlive(this);
        }
        // Java: 830
        public bool hasActiveChieftain()
        {
            return (chieftain != null && !chieftain.isDead()) || !getExtraChieftains().isEmpty();
        }
        // Java: 834
        public com.oddlabs.tt.model.Unit getChieftain()
        {
            return chieftain;
        }
        // Java: 838
        public void setTrainingChieftain(bool training_chieftain)
        {
            if (training_chieftain)
            {
                training_chieftain_count++;
            }
            else if (training_chieftain_count > 0)
            {
                training_chieftain_count--;
            }
        }
        // Java: 846
        public bool isTrainingChieftain()
        {
            return training_chieftain_count > 0;
        }
        // Java: 853
        public int getTotalChieftainCount()
        {
            int count = (chieftain != null && !chieftain.isDead() ? 1 : 0);
            count += getExtraChieftains().size();
            count += training_chieftain_count;
            return count;
        }
        // Java: 864
        public int getMaxChieftains()
        {
            return getWorld().getMaxChieftains();
        }
        // Java: 869
        public bool canTrainMoreChieftains()
        {
            return getTotalChieftainCount() < getMaxChieftains();
        }
        // Java: 873
        public org.joml.Vector4fc getColor()
        {
            return color;
        }
        // Java: 883
        public org.joml.Vector4fc getTeamBuildingColor()
        {
            int team = player_info.getTeam();
            org.joml.Vector4fc[] team_colours = com.oddlabs.tt.global.Settings.getSettings().team_colours;
            if (team == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || team < 0 || team >= team_colours.Length)
            {
                return color;
            }
            return team_colours[team];
        }
        // Java: 891
        public void deployUnits(com.oddlabs.tt.model.Building building, com.oddlabs.tt.model.DeployType type, int num_units)
        {
            ordered();
            if (num_units > 0 && !locked.isEmpty() && isLocked(com.oddlabs.tt.model.Unlocks.keyFor(type)))
            {
                return;
            }
            if (isValid(building))
            {
                building.deployUnits(type, num_units);
            }
        }
        // Java: 900
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
        // Java: 921
        public int[] getGathererCounts(com.oddlabs.tt.model.Building building)
        {
            int tree = 0;
            int rock = 0;
            int iron = 0;
            int rubber = 0;
            foreach (com.oddlabs.tt.model.Selectable s in java.lang.JIter.of(units.getSet()))
            {
                if ((s is com.oddlabs.tt.model.Unit) && (s.getPrimaryController() is com.oddlabs.tt.model.behaviour.GatherController gather) && ((object)gather.getAssignedBuilding() == (object)building))
                {
                    System.Type type = gather.getSupplyType();
                    if (((object)type == (object)typeof(com.oddlabs.tt.landscape.TreeSupply)))
                    {
                        tree++;
                    }
                    else if (((object)type == (object)typeof(com.oddlabs.tt.model.RockSupply)))
                    {
                        rock++;
                    }
                    else if (((object)type == (object)typeof(com.oddlabs.tt.model.IronSupply)))
                    {
                        iron++;
                    }
                    else if (((object)type == (object)typeof(com.oddlabs.tt.model.RubberSupply)))
                    {
                        rubber++;
                    }
                }
            }
            return new int[] { tree, rock, iron, rubber };
        }
        // Java: 936
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
        // Java: 969
        public void createHarvesters(com.oddlabs.tt.model.Building building, int num_tree, int num_rock, int num_iron, int num_rubber)
        {
            if (isValid(building))
            {
                building.createHarvesters(num_tree, num_rock, num_iron, num_rubber);
            }
        }
        // Java: 975
        public void buildRockWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
            if (isValid(building) && !(hold_weapons && num_weapons > 0))
            {
                building.buildWeapons(typeof(com.oddlabs.tt.model.weapon.RockAxeWeapon), num_weapons, infinite);
            }
        }
        // Java: 981
        public void buildIronWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
            if (isValid(building) && !(hold_weapons && num_weapons > 0))
            {
                building.buildWeapons(typeof(com.oddlabs.tt.model.weapon.IronAxeWeapon), num_weapons, infinite);
            }
        }
        // Java: 987
        public void buildRubberWeapons(com.oddlabs.tt.model.Building building, int num_weapons, bool infinite)
        {
            if (isValid(building) && !(hold_weapons && num_weapons > 0))
            {
                building.buildWeapons(typeof(com.oddlabs.tt.model.weapon.RubberAxeWeapon), num_weapons, infinite);
            }
        }
        // Java: 993
        public void doMagic(com.oddlabs.tt.model.Unit chieftain, int magic)
        {
            if (isValid(chieftain))
            {
                chieftain.doMagic(magic, usesReferenceCommandRules());
            }
        }
        // Java: 1007
        public void exitTower(com.oddlabs.tt.model.Building building)
        {
            if (isValid(building))
            {
                building.exitTower();
            }
        }
        // Java: 1013
        public void standGround(com.oddlabs.tt.model.Selectable[] units)
        {
            foreach (com.oddlabs.tt.model.Selectable s in units)
            {
                if (isValid(s) && (s is com.oddlabs.tt.model.Unit unit))
                {
                    unit.standGround();
                }
            }
        }
        // Java: 1022
        public void trainChieftain(com.oddlabs.tt.model.Building building, bool start)
        {
            if (isValid(building))
            {
                building.trainChieftain(start);
            }
        }
        // Java: 1028
        public void placeBuilding(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y, bool queue)
        {
            ordered();
            if (isLocked(com.oddlabs.tt.model.Unlocks.building(template_id)))
            {
                return;
            }
            if (world.isExtended() && !withinLimits(template_id))
            {
                return;
            }
            com.oddlabs.tt.model.Building building = getRace().getBuildingTemplate(template_id).create(this, placing_grid_x, placing_grid_y);
            foreach (com.oddlabs.tt.model.Selectable selection1 in selection)
            {
                if (isValid(selection1))
                {
                    if (queue)
                    {
                        selection1.enqueueTarget(building, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                    else
                    {
                        selection1.initTarget(building, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                }
            }
        }
        // Java: 1056
        public void placeBuildingFree(com.oddlabs.tt.model.Selectable[] selection, int template_id, int placing_grid_x, int placing_grid_y, int offset_x, int offset_y, bool queue)
        {
            if (isLocked(com.oddlabs.tt.model.Unlocks.building(template_id)))
            {
                return;
            }
            if (world.isExtended() && !withinLimits(template_id))
            {
                return;
            }
            com.oddlabs.tt.model.Building building = getRace().getBuildingTemplate(template_id).create(this, placing_grid_x, placing_grid_y);
            if ((building is com.oddlabs.tt.model.LandBuilding land))
            {
                land.freeFormOffset((float)(offset_x * 0.01f), (float)(offset_y * 0.01f));
            }
            foreach (com.oddlabs.tt.model.Selectable unit in selection)
            {
                if (isValid(unit))
                {
                    if (queue)
                    {
                        unit.enqueueTarget(building, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                    else
                    {
                        unit.initTarget(building, com.oddlabs.tt.model.Action.DEFAULT, false);
                    }
                }
            }
        }
        // Java: 1076
        public void setRallyPoint(com.oddlabs.tt.model.Building building, com.oddlabs.tt.util.Target target)
        {
            if (isValid(building) && target != null)
            {
                building.setRallyPoint(target);
            }
        }
        // Java: 1082
        public void setRallyPoint(com.oddlabs.tt.model.Building building, int grid_x, int grid_y)
        {
            setRallyPoint(building, new com.oddlabs.tt.landscape.LandscapeTarget(grid_x, grid_y));
        }
        // Java: 1087
        public void setTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            ordered();
            if (target == null || target.isDead())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Selectable selection1 in selection)
            {
                if (isValid(selection1))
                {
                    selection1.initTarget(target, action, aggressive);
                }
            }
        }
        // Java: 1105
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
        // Java: 1113
        public void setSailingTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target)
        {
            if (selection.Length == 0)
            {
                return;
            }
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]))
                {
                    selection[i].initTarget(target, com.oddlabs.tt.model.Action.MOVE, false);
                }
            }
        }
        // Java: 1122
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
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]))
                {
                    selection[i].initTarget(target, com.oddlabs.tt.model.Action.MOVE, false);
                }
            }
        }
        // Java: 1134
        public void setLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            ordered();
            if (selection.Length == 0)
            {
                return;
            }
            int grid_size = world.getUnitGrid().getGridSize();
            if (grid_x < 0 || grid_x >= grid_size || grid_y < 0 || grid_y >= grid_size)
            {
                return;
            }
            com.oddlabs.tt.util.Target[] targets = (usesReferenceCommandRules() ? world.getUnitGrid().findGridTargets(grid_x, grid_y, selection.Length, selection.Length != 1) : computeFormationTargets(selection, grid_x, grid_y));
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]) && targets[i] != null)
                {
                    selection[i].initTarget(targets[i], action, aggressive);
                }
            }
        }
        // Java: 1159
        public com.oddlabs.tt.util.Target[] computeFormationTargets(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y)
        {
            com.oddlabs.tt.util.Target[] targets = new com.oddlabs.tt.util.Target[selection.Length];
            if (selection.Length == 1)
            {
                targets[0] = world.getUnitGrid().findGridTargets(grid_x, grid_y, 1, false)[0];
                return targets;
            }
            com.oddlabs.tt.player.Formation formation = resolveFormation(selection);
            if (((object)formation == (object)com.oddlabs.tt.player.Formation.DEFAULT))
            {
                return world.getUnitGrid().findGridTargets(grid_x, grid_y, selection.Length, false);
            }
            float avg_x = 0.0f;
            float avg_y = 0.0f;
            int valid_count = 0;
            foreach (com.oddlabs.tt.model.Selectable s in selection)
            {
                if (isValid(s))
                {
                    avg_x = (float)(avg_x + s.getPositionX());
                    avg_y = (float)(avg_y + s.getPositionY());
                    valid_count++;
                }
            }
            float dest_x = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_x);
            float dest_y = com.oddlabs.tt.pathfinder.UnitGrid.coordinateFromGrid(grid_y);
            float facing_dx = 0.0f;
            float facing_dy = 0.0f;
            if (valid_count > 0)
            {
                avg_x = (float)(avg_x / valid_count);
                avg_y = (float)(avg_y / valid_count);
                facing_dx = (float)(dest_x - avg_x);
                facing_dy = (float)(dest_y - avg_y);
            }
            java.util.List offsets = com.oddlabs.tt.player.FormationLayout.computeOffsets((((object)formation == (object)com.oddlabs.tt.player.Formation.BY_TYPE) ? com.oddlabs.tt.player.Formation.SQUARE : formation), selection.Length, facing_dx, facing_dy);
            int?[] selection_order = new int?[selection.Length];
            for (int i = 0; i < selection.Length; i++)
            {
                selection_order[i] = i;
            }
            if (((object)formation == (object)com.oddlabs.tt.player.Formation.BY_TYPE))
            {
                float len = ((float)(java.lang.Math.sqrt((float)((float)(facing_dx * facing_dx) + (float)(facing_dy * facing_dy)))));
                float dir_x = (len > 1.0E-4f ? (float)(facing_dx / len) : 0.0f);
                float dir_y = (len > 1.0E-4f ? (float)(facing_dy / len) : 0.0f);
                offsets.sort(new java.util.Comparator_L((object a_l, object b_l) =>
                {
                    float[] a = (float[])a_l;
                    float[] b = (float[])b_l;
                    return java.lang.Float.compare((float)((float)(b[0] * dir_x) + (float)(b[1] * dir_y)), (float)((float)(a[0] * dir_x) + (float)(a[1] * dir_y)));
                }));
                java.util.Arrays.sort(selection_order, new java.util.Comparator_L((object a_2_l, object b_2_l) =>
                {
                    int? a_2 = (int?)a_2_l;
                    int? b_2 = (int?)b_2_l;
                    int a_toughness = (isValid(selection[((int)a_2)]) ? com.oddlabs.tt.player.Player.unitToughness(selection[((int)a_2)]) : 0);
                    int b_toughness = (isValid(selection[((int)b_2)]) ? com.oddlabs.tt.player.Player.unitToughness(selection[((int)b_2)]) : 0);
                    return java.lang.Integer.compare(a_toughness, b_toughness);
                }));
            }
            for (int i_2 = 0; i_2 < selection.Length && i_2 < offsets.size(); i_2++)
            {
                float[] offset = ((float[])offsets.get(i_2));
                int unit_index = ((int)selection_order[i_2]);
                int offset_grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(dest_x + offset[0]));
                int offset_grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate((float)(dest_y + offset[1]));
                if (offset_grid_x < 0 || offset_grid_x >= world.getUnitGrid().getGridSize() || offset_grid_y < 0 || offset_grid_y >= world.getUnitGrid().getGridSize())
                {
                    targets[unit_index] = world.getUnitGrid().findGridTargets(grid_x, grid_y, 1, false)[0];
                }
                else
                {
                    targets[unit_index] = world.getUnitGrid().findGridTargets(offset_grid_x, offset_grid_y, 1, false)[0];
                }
            }
            if ((world.isExtended() || world.isFreeForm()) && ((object)formation != (object)com.oddlabs.tt.player.Formation.BY_TYPE))
            {
                return matchTargets(selection, targets);
            }
            return targets;
        }
        // Java: 1245
        public com.oddlabs.tt.util.Target[] matchTargets(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target[] targets)
        {
            int n = selection.Length;
            if (n < 2 || n > 2000)
            {
                return targets;
            }
            long[] keys = new long[n * n];
            int k = 0;
            for (int i = 0; i < n; i++)
            {
                if (!isValid(selection[i]))
                {
                    continue;
                }
                int ux = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(selection[i].getPositionX());
                int uy = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(selection[i].getPositionY());
                for (int j = 0; j < n; j++)
                {
                    if (targets[j] == null)
                    {
                        continue;
                    }
                    long dx = targets[j].getGridX() - ux;
                    long dy = targets[j].getGridY() - uy;
                    keys[k++] = ((dx * dx + dy * dy) * n + i) * n + j;
                }
            }
            long[] sorted = java.util.Arrays.copyOf(keys, k);
            java.util.Arrays.sort(sorted);
            com.oddlabs.tt.util.Target[] result = new com.oddlabs.tt.util.Target[n];
            bool[] unit_done = new bool[n];
            bool[] target_done = new bool[n];
            foreach (long key in sorted)
            {
                int j_2 = unchecked((int)((key % n)));
                int i_2 = unchecked((int)(((key / n) % n)));
                if (unit_done[i_2] || target_done[j_2])
                {
                    continue;
                }
                unit_done[i_2] = true;
                target_done[j_2] = true;
                result[i_2] = targets[j_2];
            }
            return result;
        }
        // Java: 1282
        public static int unitToughness(com.oddlabs.tt.model.Selectable s)
        {
            if ((s is com.oddlabs.tt.model.Unit u))
            {
                return ((com.oddlabs.tt.model.UnitTemplate)u.getTemplate()).getMaxHitPoints();
            }
            return 0;
        }
        // Java: 1288
        public void queueTarget(com.oddlabs.tt.model.Selectable[] selection, com.oddlabs.tt.util.Target target, com.oddlabs.tt.model.Action action, bool aggressive)
        {
            if (target == null || target.isDead())
            {
                return;
            }
            foreach (com.oddlabs.tt.model.Selectable selectable in selection)
            {
                if (isValid(selectable))
                {
                    selectable.enqueueTarget(target, action, aggressive);
                }
            }
        }
        // Java: 1300
        public void queueLandscapeTarget(com.oddlabs.tt.model.Selectable[] selection, int grid_x, int grid_y, com.oddlabs.tt.model.Action action, bool aggressive)
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
            com.oddlabs.tt.util.Target[] targets = computeFormationTargets(selection, grid_x, grid_y);
            for (int i = 0; i < selection.Length; i++)
            {
                if (isValid(selection[i]) && targets[i] != null)
                {
                    selection[i].enqueueTarget(targets[i], action, aggressive);
                }
            }
        }
        public bool directing;
        // Java: 1320
        public void setDirecting(bool directing)
        {
            this.directing = directing;
        }
        // Java: 1324
        public bool isValid(com.oddlabs.tt.model.Selectable s)
        {
            if ((s is com.oddlabs.tt.model.Unit held) && held.isObjectiveHold() && !directing && getAI() != null)
            {
                return false;
            }
            if ((s is com.oddlabs.tt.model.Unit unit) && unit.isMounted())
            {
                return false;
            }
            if (s == null || s.isDead())
            {
                return false;
            }
            if (((object)s.getOwner() == (object)this))
            {
                return true;
            }
            if (isEnemy(s.getOwner()))
            {
                return false;
            }
            s.setHumanOverrideUntilTick(world.getTick() + com.oddlabs.tt.player.Player.HUMAN_OVERRIDE_TICKS);
            return true;
        }
        public const int HUMAN_OVERRIDE_TICKS = 300;
        // Java: 1366
        public float getStartX()
        {
            return start_x;
        }
        // Java: 1370
        public void setStartX(float x)
        {
            start_x = x;
        }
        // Java: 1374
        public float getStartY()
        {
            return start_y;
        }
        // Java: 1378
        public void setStartY(float y)
        {
            start_y = y;
        }
        // Java: 1382
        public bool isEnemy(com.oddlabs.tt.player.Player other_player)
        {
            if (other_player.player_info.getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || this.player_info.getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return false;
            }
            return other_player.player_info.getTeam() != this.player_info.getTeam();
        }
        // Java: 1395
        public bool isAlly(com.oddlabs.tt.player.Player other_player)
        {
            if (((object)other_player == (object)this))
            {
                return true;
            }
            if (this.player_info.getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL || other_player.player_info.getTeam() == com.oddlabs.tt.player.PlayerInfo.TEAM_NEUTRAL)
            {
                return false;
            }
            return other_player.player_info.getTeam() == this.player_info.getTeam();
        }
        // Java: 1405
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
        // Java: 1415
        public com.oddlabs.tt.model.Army getUnits()
        {
            return units;
        }
        // Java: 1419
        public com.oddlabs.tt.model.Selectable[][] classifyUnits()
        {
            java.util.Map map = ((java.util.Map)units.getSet().stream().collect(java.util.stream.Collectors.groupingBy(new java.util.function.Function_L((object u_l) =>
            {
                com.oddlabs.tt.model.Selectable u = (com.oddlabs.tt.model.Selectable)u_l;
                return u.getPrimaryController().getKey();
            }))));
            return ((com.oddlabs.tt.model.Selectable[][])map.values().stream().map(new java.util.function.Function_L((object list_l) =>
            {
                java.util.List list = (java.util.List)list_l;
                return ((com.oddlabs.tt.model.Selectable[])list.toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Selectable[a0]; })));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.model.Selectable[a0][]; })));
        }
        // Java: 1425
        public void magicCast()
        {
            magics++;
        }
        // Java: 1429
        public int getMagics()
        {
            return magics;
        }
        // Java: 1433
        public void weaponThrown()
        {
            weapons_thrown++;
        }
        // Java: 1437
        public int getWeaponsThrown()
        {
            return weapons_thrown;
        }
        // Java: 1441
        public void unitMoved()
        {
            units_moved++;
        }
        // Java: 1445
        public int getUnitsMoved()
        {
            return units_moved;
        }
        // Java: 1449
        public void unitLost()
        {
            units_lost++;
        }
        // Java: 1454
        public int getUnitsLost()
        {
            return units_lost;
        }
        // Java: 1458
        public void buildingLost()
        {
            buildings_lost++;
        }
        // Java: 1462
        public int getBuildingsLost()
        {
            return buildings_lost;
        }
        // Java: 1466
        public void unitKilled()
        {
            units_killed++;
        }
        // Java: 1470
        public int getUnitsKilled()
        {
            return units_killed;
        }
        // Java: 1474
        public void buildingDestroyed()
        {
            buildings_destroyed++;
        }
        // Java: 1478
        public int getBuildingsDestroyed()
        {
            return buildings_destroyed;
        }
        // Java: 1482
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
            else if (com.oddlabs.tt.model.ExtendedResources.isRare(type))
            {
                rare_harvested[com.oddlabs.tt.model.ExtendedResources.rareIndex(type)]++;
            }
            else
            {
                throw new System.Exception();
            }
        }
        public bool elemental;
        // Java: 1504
        public bool isElemental()
        {
            return elemental;
        }
        // Java: 1508
        public void setElemental(bool elemental)
        {
            this.elemental = elemental;
        }
        // Java: 1512
        public int getRareHarvested(int rare_index)
        {
            return rare_harvested[rare_index];
        }
        // Java: 1516
        public int getTreeHarvested()
        {
            return tree_harvested;
        }
        // Java: 1520
        public int getRockHarvested()
        {
            return rock_harvested;
        }
        // Java: 1524
        public int getIronHarvested()
        {
            return iron_harvested;
        }
        // Java: 1528
        public int getRubberHarvested()
        {
            return rubber_harvested;
        }
        static Player()
        {
            Player.FLEET_CAP = new int[] { 1, 5, 10, 20 };
            Player.BUILDING_CAP = new int[] { 40, 60, 90, 120 };
        }
        private void init_Player()
        {
            this.units = new com.oddlabs.tt.model.Army();
            this.ai = null;
            this.chieftain = null;
            this.extra_chieftains = new java.util.ArrayList();
            this.training_chieftain_count = 0;
            this.rare_harvested = new int[5];
            this.can_build_chieftains = true;
            this.can_use_chiefs_courage = true;
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
            this.upgrade_levels = new int[com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES];
            this.researching = new com.oddlabs.tt.model.ResearchContainer[com.oddlabs.tt.model.ResearchContainer.NUM_UPGRADES];
            this.preferred_speed = com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE;
            this.chiefs_courage_uses = 0;
            this.chiefs_courage_cooldown_remaining = 0.0f;
            this.locked = new java.util.BitSet();
            this.research_cost_percent = 100;
            this.last_order_tick = (-1);
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

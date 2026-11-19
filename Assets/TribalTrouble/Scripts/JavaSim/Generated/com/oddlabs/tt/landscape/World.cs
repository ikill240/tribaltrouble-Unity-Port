// Translated from World.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 29
    public class World : java.lang.Object
    {
        public const int GAMESPEED_DONTCARE = (-2);
        public static float[] GAMESPEEDS;
        public com.oddlabs.tt.landscape.HeightMap world;
        public java.util.Random random;
        public com.oddlabs.tt.animation.AnimationManager animation_manager_game_time;
        public com.oddlabs.tt.animation.AnimationManager animation_manager_real_time;
        public com.oddlabs.tt.landscape.AudioImplementation audio_impl;
        public int max_unit_count;
        public string map_code;
        // Java: 45
        public string getMapCode()
        {
            return map_code;
        }
        public int max_building_count;
        public int koth_statue_count;
        public int initial_unit_count;
        public int max_chieftains;
        public int max_chieftains_per_quarters;
        public float chieftain_heal_idle_seconds;
        public int chieftain_heal_amount;
        public int target_num_quarters;
        public int target_num_armories;
        public int max_concurrent_quarters;
        public int max_concurrent_armories;
        public int num_resource_towers;
        public int max_concurrent_towers;
        public float magic1_cost;
        public float magic2_cost;
        public float magic3_cost;
        public float[] extra_magic_cost;
        public float building_health_multiplier;
        public float viking_chief_health_multiplier;
        public float native_chief_health_multiplier;
        public float unit_range_multiplier;
        public bool free_form;
        public bool weather_enabled;
        public com.oddlabs.tt.procedural.Landscape.TerrainType terrain_type;
        // Java: 77
        public com.oddlabs.tt.procedural.Landscape.TerrainType getTerrainType()
        {
            return terrain_type;
        }
        public com.oddlabs.tt.landscape.Weather weather;
        public bool extended;
        public string island_wars;
        public com.oddlabs.tt.landscape.NotificationListener notification_listener;
        public com.oddlabs.tt.player.Player[] players;
        public com.oddlabs.tt.model.SupplyManagers supply_managers;
        public com.oddlabs.tt.pathfinder.UnitGrid unit_grid;
        public com.oddlabs.tt.landscape.PatchGroup patch_root;
        public com.oddlabs.tt.landscape.AbstractTreeGroup tree_root;
        public java.util.List treePositions;
        public com.oddlabs.tt.model.AbstractElementNode element_root;
        public com.oddlabs.tt.model.RacesResources races_resources;
        public com.oddlabs.tt.landscape.LandscapeResources landscape_resources;
        public com.oddlabs.tt.resource.FogInfo fog;
        public int global_checksum;
        public int gamespeed;
        public int map_size;
        public com.oddlabs.matchmaking.GameMode mode;
        // Java: 101
        public static com.oddlabs.tt.landscape.LandscapeResources loadCommon(com.oddlabs.tt.render.RenderQueues queues)
        {
            com.oddlabs.tt.landscape.LandscapeResources landscape_resources = new com.oddlabs.tt.landscape.LandscapeResources(queues);
            com.oddlabs.tt.form.ProgressForm.progress();
            return landscape_resources;
        }
        // Java: 107
        public static com.oddlabs.tt.model.RacesResources loadInGame(com.oddlabs.tt.render.RenderQueues queues)
        {
            return new com.oddlabs.tt.model.RacesResources(queues);
        }
        // Java: 111
        public static com.oddlabs.tt.landscape.World newWorld(com.oddlabs.tt.landscape.AudioImplementation audio_implementation, com.oddlabs.tt.landscape.LandscapeResources landscape_resources, com.oddlabs.tt.model.RacesResources races_resources, com.oddlabs.tt.landscape.NotificationListener notification_listener, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, com.oddlabs.tt.player.PlayerInfo[] player_infos, org.joml.Vector4fc[] player_colors, com.oddlabs.tt.resource.FogInfo fog)
        {
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.tt.landscape.World world = new com.oddlabs.tt.landscape.World(audio_implementation, landscape_resources, races_resources, notification_listener, world_params, world_info, terrain, player_infos, player_colors, fog);
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 5.0f));
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player player = players[i];
                if (i < world_info.starting_locations().Length)
                {
                    player.init(world_info.starting_locations()[i]);
                }
                else
                {
                    float c = (float)(world.getHeightMap().getMetersPerWorld() * 0.5f);
                    player.init(new float[] { c, c });
                }
            }
            return world;
        }
        // Java: 137
        public com.oddlabs.tt.resource.FogInfo getFog()
        {
            return fog;
        }
        // Java: 141
        public com.oddlabs.tt.landscape.LandscapeResources getLandscapeResources()
        {
            return landscape_resources;
        }
        // Java: 145
        public com.oddlabs.tt.model.RacesResources getRacesResources()
        {
            return races_resources;
        }
        // Java: 149
        public com.oddlabs.tt.landscape.AudioImplementation getAudio()
        {
            return audio_impl;
        }
        // Java: 153
        public int getChecksum()
        {
            return global_checksum;
        }
        // Java: 157
        public void updateGlobalChecksum(int @value)
        {
            global_checksum += @value;
        }
        // Java: 161
        public int getGamespeed()
        {
            return gamespeed;
        }
        // Java: 165
        public int getMapSize()
        {
            return map_size;
        }
        public bool many_islands;
        // Java: 176
        public bool isArchipelago()
        {
            return map_size == com.oddlabs.matchmaking.Game.SIZE_ARCHIPELAGO || extended && many_islands;
        }
        // Java: 180
        public com.oddlabs.matchmaking.GameMode getGameMode()
        {
            return mode;
        }
        // Java: 184
        public float getSecondsPerTick()
        {
            return com.oddlabs.tt.landscape.World.GAMESPEEDS[gamespeed];
        }
        // Java: 188
        public static bool isValidPreferredGamespeed(int speed)
        {
            return speed == com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE || com.oddlabs.tt.landscape.World.isValidGamespeed(speed);
        }
        // Java: 192
        public static bool isValidGamespeed(int speed)
        {
            return speed >= 0 && speed < com.oddlabs.tt.landscape.World.GAMESPEEDS.Length;
        }
        // Java: 196
        public void gamespeedChanged()
        {
            int new_gamespeed = com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE;
            foreach (com.oddlabs.tt.player.Player player in players)
            {
                int gamespeed = player.getPreferredGamespeed();
                if (gamespeed != com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE)
                {
                    if (new_gamespeed != com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE && gamespeed != new_gamespeed)
                    {
                        return;
                    }
                    new_gamespeed = gamespeed;
                }
            }
            if (new_gamespeed != com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE && new_gamespeed != this.gamespeed)
            {
                this.gamespeed = new_gamespeed;
                getNotificationListener().gamespeedChanged(this.gamespeed);
            }
        }
        // Java: 212
        public void tick(float t)
        {
            float game_time_t = (float)((float)(getSecondsPerTick() * t) / com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK);
            foreach (com.oddlabs.tt.player.Player player in getPlayers())
            {
                player.tickChiefsCourageCooldown(game_time_t);
            }
            getAnimationManagerGameTime().runAnimations(game_time_t);
            if (weather_enabled && players != null && players.Length > 0)
            {
                if (weather == null)
                {
                    weather = new com.oddlabs.tt.landscape.Weather(this);
                }
                weather.animate(game_time_t);
            }
            else if (weather != null)
            {
                weather.animate(game_time_t);
            }
            if (market_prices != null)
            {
                market_prices.animate(game_time_t);
            }
            getAnimationManagerRealTime().runAnimations(t);
        }
        // Java: 234
        public int getTick()
        {
            return getAnimationManagerRealTime().getTick();
        }
        // Java: 356
        public com.oddlabs.tt.model.AbstractElementNode getElementRoot()
        {
            return element_root;
        }
        // Java: 360
        public com.oddlabs.tt.landscape.AbstractTreeGroup getTreeRoot()
        {
            return tree_root;
        }
        // Java: 364
        public java.util.List getTreePositions()
        {
            return treePositions;
        }
        // Java: 368
        public com.oddlabs.tt.landscape.AbstractPatchGroup getPatchRoot()
        {
            return patch_root;
        }
        // Java: 372
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return unit_grid;
        }
        // Java: 376
        public com.oddlabs.tt.model.SupplyManager getSupplyManager(System.Type cl)
        {
            return supply_managers.getSupplyManager(cl);
        }
        // Java: 380
        public com.oddlabs.tt.player.Player[] getPlayers()
        {
            return players;
        }
        // Java: 384
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 389
        public int getMaxBuildingCount()
        {
            return max_building_count;
        }
        // Java: 395
        public int getKothStatueCount()
        {
            return koth_statue_count;
        }
        // Java: 400
        public int getInitialUnitCount()
        {
            return initial_unit_count;
        }
        // Java: 405
        public int getMaxChieftains()
        {
            return max_chieftains;
        }
        // Java: 410
        public int getMaxChieftainsPerQuarters()
        {
            return max_chieftains_per_quarters;
        }
        public const int SALT_NUM_RESOURCE_TOWERS = 1381259057;
        public const int SALT_ESCORTS_PER_GATHER_GROUP = 1163084594;
        public const int SALT_PATROL_FORCE_SIZE = 1346458675;
        // Java: 423
        public int rollMatchValue(int salt, int min, int max_inclusive)
        {
            if (max_inclusive <= min)
            {
                return min;
            }
            long seed = (java.lang.JO.hashCode(map_code) * (-7046029254386353131L)) ^ salt;
            return min + new java.util.Random(seed).nextInt(max_inclusive - min + 1);
        }
        // Java: 431
        public float getChieftainHealIdleSeconds()
        {
            return chieftain_heal_idle_seconds;
        }
        // Java: 436
        public int getChieftainHealAmount()
        {
            return chieftain_heal_amount;
        }
        // Java: 441
        public int getTargetNumQuarters()
        {
            return target_num_quarters;
        }
        // Java: 446
        public int getTargetNumArmories()
        {
            return target_num_armories;
        }
        // Java: 451
        public int getMaxConcurrentQuarters()
        {
            return max_concurrent_quarters;
        }
        // Java: 456
        public int getMaxConcurrentArmories()
        {
            return max_concurrent_armories;
        }
        // Java: 461
        public int getNumResourceTowers()
        {
            return num_resource_towers;
        }
        // Java: 466
        public int getMaxConcurrentTowers()
        {
            return max_concurrent_towers;
        }
        // Java: 470
        public float getMagic1Cost()
        {
            return magic1_cost;
        }
        // Java: 474
        public float getMagic2Cost()
        {
            return magic2_cost;
        }
        // Java: 478
        public float getMagic3Cost()
        {
            return magic3_cost;
        }
        // the extended magics' cost: the game's own unless the setup changed it
        public float getExtraMagicCost(int index)
        {
            float c = (extra_magic_cost != null && index >= 0 && index < extra_magic_cost.Length ? extra_magic_cost[index] : 0.0f);
            return (c > 0.0f ? c : com.oddlabs.tt.model.Unit.MAX_MAGIC_ENERGY[index]);
        }
        // Java: 482
        public float getBuildingHealthMultiplier()
        {
            return building_health_multiplier;
        }
        // Java: 489
        public float getChiefHealthMultiplier(int race_index)
        {
            if (race_index == com.oddlabs.tt.model.RacesResources.RACE_VIKINGS)
            {
                return viking_chief_health_multiplier;
            }
            if (race_index == com.oddlabs.tt.model.RacesResources.RACE_NATIVES)
            {
                return native_chief_health_multiplier;
            }
            return 1.0f;
        }
        // Java: 498
        public bool isWeather()
        {
            return weather_enabled;
        }
        public com.oddlabs.tt.model.MarketContainer.Prices market_prices;
        // Java: 506
        public com.oddlabs.tt.model.MarketContainer.Prices getMarketPrices()
        {
            if (market_prices == null)
            {
                market_prices = new com.oddlabs.tt.model.MarketContainer.Prices();
            }
            return market_prices;
        }
        public const int ZOMBIE_TEAM = 13;
        // Java: 516
        public interface FallListener
        {
            // Java: 517
            void fell(com.oddlabs.tt.model.Unit unit);
        }
        public sealed class FallListener_L : java.lang.Object, FallListener
        {
            readonly System.Action<com.oddlabs.tt.model.Unit> f;
            public FallListener_L(System.Action<com.oddlabs.tt.model.Unit> f) { this.f = f; }
            public void fell(com.oddlabs.tt.model.Unit p0) { f(p0); }
        }
        public com.oddlabs.tt.landscape.World.FallListener fall_listener;
        // Java: 522
        public void setFallListener(com.oddlabs.tt.landscape.World.FallListener listener)
        {
            fall_listener = listener;
        }
        // Java: 526
        public com.oddlabs.tt.landscape.World.FallListener getFallListener()
        {
            return fall_listener;
        }
        // Java: 530
        public void unitFell(com.oddlabs.tt.model.Unit unit)
        {
            if (fall_listener != null)
            {
                fall_listener.fell(unit);
            }
        }
        // Java: 536
        public com.oddlabs.tt.player.Player getZombiePlayer()
        {
            foreach (com.oddlabs.tt.player.Player p in players)
            {
                if (p.getPlayerInfo().getTeam() == com.oddlabs.tt.landscape.World.ZOMBIE_TEAM)
                {
                    return p;
                }
            }
            return null;
        }
        // Java: 544
        public bool summonWeather(int kind, float x, float y)
        {
            if (!extended || players == null || players.Length == 0)
            {
                return false;
            }
            if (weather == null)
            {
                weather = (weather_enabled ? new com.oddlabs.tt.landscape.Weather(this) : com.oddlabs.tt.landscape.Weather.summonedOnly(this));
            }
            weather.summon(kind, x, y);
            return true;
        }
        // Java: 553
        public com.oddlabs.tt.landscape.Weather getWeather()
        {
            return weather;
        }
        // Java: 557
        public bool isFreeForm()
        {
            return free_form || isExtended();
        }
        // Java: 562
        public string getIslandWars()
        {
            return island_wars;
        }
        // Java: 567
        public bool isExtended()
        {
            return extended;
        }
        // Java: 571
        public float getUnitRangeMultiplier()
        {
            return unit_range_multiplier;
        }
        // Java: 575
        public com.oddlabs.tt.landscape.NotificationListener getNotificationListener()
        {
            return notification_listener;
        }
        // Java: 579
        public com.oddlabs.tt.landscape.HeightMap getHeightMap()
        {
            return world;
        }
        // Java: 583
        public com.oddlabs.tt.animation.AnimationManager getAnimationManagerGameTime()
        {
            return animation_manager_game_time;
        }
        // Java: 587
        public com.oddlabs.tt.animation.AnimationManager getAnimationManagerRealTime()
        {
            return animation_manager_real_time;
        }
        // Java: 591
        public java.util.Random getRandom()
        {
            return random;
        }
        static World()
        {
            World.GAMESPEEDS = new float[] { 0.0f, (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK / 2), (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK * 2.0f), (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK * 3.0f), (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK * 6.0f) };
        }
        public World(com.oddlabs.tt.landscape.AudioImplementation audio_implementation, com.oddlabs.tt.landscape.LandscapeResources landscape_resources, com.oddlabs.tt.model.RacesResources races_resources, com.oddlabs.tt.landscape.NotificationListener notification_listener, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, com.oddlabs.tt.player.PlayerInfo[] player_infos, org.joml.Vector4fc[] player_colors, com.oddlabs.tt.resource.FogInfo fog) : base()
        {
            java.lang.IO.println("****************** Generating landscape at tick " + (com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick()) + " ********************");
            this.fog = fog;
            this.landscape_resources = landscape_resources;
            this.races_resources = races_resources;
            this.audio_impl = audio_implementation;
            this.max_unit_count = java.lang.Math.max(world_params.getMaxUnitCount(), world_params.getInitialUnitCount());
            this.map_code = world_params.getMapcode();
            this.max_building_count = world_params.getMaxBuildingCount();
            this.koth_statue_count = world_params.getKothStatueCount();
            this.initial_unit_count = world_params.getInitialUnitCount();
            this.max_chieftains = (!world_params.isExtended() ? world_params.getMaxChieftains() : (((object)world_params.getGameMode() == (object)com.oddlabs.matchmaking.GameMode.MAGIC_RAMPAGE) ? java.lang.Math.max(world_params.getMaxChieftains(), com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules.CHIEFTAINS) : 1));
            this.max_chieftains_per_quarters = world_params.getMaxChieftainsPerQuarters();
            this.chieftain_heal_idle_seconds = world_params.getChieftainHealIdleSeconds();
            this.chieftain_heal_amount = world_params.getChieftainHealAmount();
            this.target_num_quarters = world_params.getTargetNumQuarters();
            this.target_num_armories = world_params.getTargetNumArmories();
            this.max_concurrent_quarters = world_params.getMaxConcurrentQuarters();
            this.max_concurrent_armories = world_params.getMaxConcurrentArmories();
            int requested_towers = world_params.getNumResourceTowers();
            this.num_resource_towers = (requested_towers >= 0 ? requested_towers : rollMatchValue(com.oddlabs.tt.landscape.World.SALT_NUM_RESOURCE_TOWERS, com.oddlabs.tt.landscape.WorldParameters.MIN_RANDOM_RESOURCE_TOWERS, com.oddlabs.tt.landscape.WorldParameters.MAX_RANDOM_RESOURCE_TOWERS));
            this.max_concurrent_towers = world_params.getMaxConcurrentTowers();
            this.magic1_cost = world_params.getMagic1Cost();
            this.magic2_cost = world_params.getMagic2Cost();
            this.magic3_cost = world_params.getMagic3Cost();
            this.extra_magic_cost = new float[com.oddlabs.tt.model.RacesResources.NUM_MAGIC];
            for (int m_2 = com.oddlabs.tt.model.RacesResources.NUM_CLASSIC_MAGIC; m_2 < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m_2++)
            {
                this.extra_magic_cost[m_2] = world_params.getExtraMagicCost(m_2);
            }
            this.building_health_multiplier = world_params.getBuildingHealthMultiplier();
            this.viking_chief_health_multiplier = world_params.getChiefHealthMultiplier(com.oddlabs.tt.model.RacesResources.RACE_VIKINGS);
            this.native_chief_health_multiplier = world_params.getChiefHealthMultiplier(com.oddlabs.tt.model.RacesResources.RACE_NATIVES);
            this.unit_range_multiplier = world_params.getUnitRangeMultiplier();
            this.free_form = world_params.isFreeForm();
            this.weather_enabled = world_params.isWeather();
            this.terrain_type = terrain;
            this.extended = world_params.isExtended();
            this.island_wars = world_params.getIslandWars();
            this.notification_listener = notification_listener;
            this.gamespeed = world_params.getInitialGameSpeed();
            this.map_size = world_params.getMapSize();
            this.mode = world_params.getGameMode();
            long time_start = java.lang.JSystem.currentTimeMillis();
            world = new com.oddlabs.tt.landscape.HeightMap(this, world_info.meters_per_world(), world_info.sea_level_meters(), world_info.texels_per_colormap(), world_info.chunks_per_colormap(), world_info.heightmap(), world_info.trees(), world_info.access_grid(), world_info.dock_grid(), world_info.water_grid(), world_info.build_grid(), world_info.island_ids(), world_info.island_infos());
            animation_manager_game_time = new com.oddlabs.tt.animation.AnimationManager();
            animation_manager_real_time = new com.oddlabs.tt.animation.AnimationManager();
            if (world_params.isExtended())
            {
                animation_manager_game_time.containErrors();
            }
            random = new java.util.Random(42);
            if (((object)world_params.getGameMode() == (object)com.oddlabs.matchmaking.GameMode.ZOMBIES) && world_params.isExtended())
            {
                player_infos = ((com.oddlabs.tt.player.PlayerInfo[])java.util.Arrays.copyOf(player_infos, player_infos.Length + 1));
                player_infos[player_infos.Length - 1] = new com.oddlabs.tt.player.PlayerInfo(com.oddlabs.tt.landscape.World.ZOMBIE_TEAM, com.oddlabs.tt.model.RacesResources.RACE_NATIVES, "The Dead");
                player_colors = ((org.joml.Vector4fc[])java.util.Arrays.copyOf(player_colors, player_colors.Length + 1));
                player_colors[player_colors.Length - 1] = new org.joml.Vector4f(0.42f, 0.55f, 0.36f, 1.0f);
            }
            players = new com.oddlabs.tt.player.Player[player_infos.Length];
            for (int i = 0; i < player_infos.Length; i++)
            {
                players[i] = new com.oddlabs.tt.player.Player(this, player_infos[i], player_colors[i]);
                players[i].enableMagic(0, world_params.isMagic1Enabled());
                players[i].enableMagic(1, world_params.isMagic2Enabled());
                players[i].enableMagic(com.oddlabs.tt.model.RacesResources.INDEX_MAGIC_CONVERT, world_params.isMagic3Enabled());
                for (int m = com.oddlabs.tt.model.RacesResources.NUM_CLASSIC_MAGIC; m < com.oddlabs.tt.model.RacesResources.NUM_MAGIC; m++)
                {
                    players[i].enableMagic(m, world_params.isExtended() && world_params.isExtraMagicEnabled(m));
                }
                players[i].enableChiefsCourage(world_params.isChiefsCourageEnabled());
                if (player_infos[i].isElementalRace())
                {
                    com.oddlabs.tt.player.campaign.SagaIsland.makeElemental(players[i]);
                }
            }
            long time_stop = java.lang.JSystem.currentTimeMillis();
            java.lang.IO.println("****************** Finished landscape in " + java.lang.JStr.valueOf(((float)((time_stop - time_start) / 1000.0f))) + " sec ********************");
            this.supply_managers = new com.oddlabs.tt.model.SupplyManagers(this);
            this.unit_grid = new com.oddlabs.tt.pathfinder.UnitGrid(world);
            bool archipelago = world_info.island_infos().size() > 1;
            this.many_islands = archipelago;
            if (archipelago)
            {
                com.oddlabs.tt.pathfinder.RegionBuilder.buildRegions(unit_grid);
            }
            else
            {
                com.oddlabs.tt.pathfinder.RegionBuilder.buildRegions(unit_grid, world_info.starting_locations()[0][0], world_info.starting_locations()[0][1]);
            }
            this.patch_root = new com.oddlabs.tt.landscape.PatchGroup(this);
            this.treePositions = world_info.trees();
            this.tree_root = com.oddlabs.tt.landscape.AbstractTreeGroup.newRoot(this, world_info.trees(), world_info.palm_trees(), terrain);
            this.element_root = com.oddlabs.tt.model.AbstractElementNode.newRoot(world);
            com.oddlabs.tt.model.AbstractElementNode.buildSupplies(this, world_info.iron(), world_info.rocks(), world_info.plants(), terrain);
            if (extended)
            {
                com.oddlabs.tt.model.ExtendedResources.placeDeposits(this, world_info.rocks(), world_info.iron(), world_info.starting_locations());
            }
        }
    }
}

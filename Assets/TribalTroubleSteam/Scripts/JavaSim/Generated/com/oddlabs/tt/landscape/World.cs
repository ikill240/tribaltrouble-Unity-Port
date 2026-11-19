// Translated from World.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 32
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
        public int max_building_count;
        public bool ships;
        public bool night;
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
        // Java: 66
        public static com.oddlabs.tt.landscape.LandscapeResources loadCommon(com.oddlabs.tt.render.RenderQueues queues)
        {
            com.oddlabs.tt.landscape.LandscapeResources landscape_resources = new com.oddlabs.tt.landscape.LandscapeResources(queues);
            com.oddlabs.tt.form.ProgressForm.progress();
            return landscape_resources;
        }
        // Java: 72
        public static com.oddlabs.tt.model.RacesResources loadInGame(com.oddlabs.tt.render.RenderQueues queues)
        {
            return new com.oddlabs.tt.model.RacesResources(queues);
        }
        // Java: 76
        public static com.oddlabs.tt.landscape.World newWorld(com.oddlabs.tt.landscape.AudioImplementation audio_implementation, com.oddlabs.tt.landscape.LandscapeResources landscape_resources, com.oddlabs.tt.model.RacesResources races_resources, com.oddlabs.tt.landscape.NotificationListener notification_listener, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, com.oddlabs.tt.player.PlayerInfo[] player_infos, com.oddlabs.tt.resource.FogInfo fog)
        {
            org.joml.Vector4fc[] colors = ((org.joml.Vector4f[])java.util.Arrays.copyOf(com.oddlabs.tt.global.Settings.getSettings().team_colours, player_infos.Length));
            return com.oddlabs.tt.landscape.World.newWorld(audio_implementation, landscape_resources, races_resources, notification_listener, world_params, world_info, terrain, player_infos, fog, colors);
        }
        // Java: 86
        public static com.oddlabs.tt.landscape.World newWorld(com.oddlabs.tt.landscape.AudioImplementation audio_implementation, com.oddlabs.tt.landscape.LandscapeResources landscape_resources, com.oddlabs.tt.model.RacesResources races_resources, com.oddlabs.tt.landscape.NotificationListener notification_listener, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, com.oddlabs.tt.player.PlayerInfo[] player_infos, com.oddlabs.tt.resource.FogInfo fog, org.joml.Vector4fc[] colors)
        {
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.tt.landscape.World world = new com.oddlabs.tt.landscape.World(audio_implementation, landscape_resources, races_resources, notification_listener, world_params, world_info, terrain, player_infos, fog, colors);
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.tt.form.ProgressForm.progress((float)(1 / 5.0f));
            com.oddlabs.tt.form.ProgressForm.progress();
            com.oddlabs.tt.player.Player[] players = world.getPlayers();
            for (short i = unchecked((short)(0)); i < players.Length; i++)
            {
                com.oddlabs.tt.player.Player player = players[i];
                player.init(world_info.starting_locations()[i]);
            }
            return world;
        }
        // Java: 107
        public com.oddlabs.tt.resource.FogInfo getFog()
        {
            return fog;
        }
        // Java: 111
        public com.oddlabs.tt.landscape.LandscapeResources getLandscapeResources()
        {
            return landscape_resources;
        }
        // Java: 115
        public com.oddlabs.tt.model.RacesResources getRacesResources()
        {
            return races_resources;
        }
        // Java: 119
        public com.oddlabs.tt.landscape.AudioImplementation getAudio()
        {
            return audio_impl;
        }
        // Java: 123
        public int getChecksum()
        {
            return global_checksum;
        }
        // Java: 127
        public void updateGlobalChecksum(int @value)
        {
            global_checksum += @value;
        }
        // Java: 131
        public int getGamespeed()
        {
            return gamespeed;
        }
        // Java: 135
        public int getMapSize()
        {
            return map_size;
        }
        // Java: 139
        public com.oddlabs.matchmaking.GameMode getGameMode()
        {
            return mode;
        }
        // Java: 143
        public float getSecondsPerTick()
        {
            return com.oddlabs.tt.landscape.World.GAMESPEEDS[gamespeed];
        }
        // Java: 147
        public static bool isValidPreferredGamespeed(int speed)
        {
            return speed == com.oddlabs.tt.landscape.World.GAMESPEED_DONTCARE || com.oddlabs.tt.landscape.World.isValidGamespeed(speed);
        }
        // Java: 151
        public static bool isValidGamespeed(int speed)
        {
            return speed >= 0 && speed < com.oddlabs.tt.landscape.World.GAMESPEEDS.Length;
        }
        // Java: 155
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
        // Java: 171
        public void tick(float t)
        {
            getAnimationManagerGameTime().runAnimations((float)((float)(getSecondsPerTick() * t) / com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK));
            getAnimationManagerRealTime().runAnimations(t);
        }
        // Java: 177
        public int getTick()
        {
            return getAnimationManagerRealTime().getTick();
        }
        // Java: 238
        public com.oddlabs.tt.model.AbstractElementNode getElementRoot()
        {
            return element_root;
        }
        // Java: 242
        public com.oddlabs.tt.landscape.AbstractTreeGroup getTreeRoot()
        {
            return tree_root;
        }
        // Java: 246
        public java.util.List getTreePositions()
        {
            return treePositions;
        }
        // Java: 250
        public com.oddlabs.tt.landscape.AbstractPatchGroup getPatchRoot()
        {
            return patch_root;
        }
        // Java: 254
        public com.oddlabs.tt.pathfinder.UnitGrid getUnitGrid()
        {
            return unit_grid;
        }
        // Java: 258
        public com.oddlabs.tt.model.SupplyManager getSupplyManager(System.Type cl)
        {
            return supply_managers.getSupplyManager(cl);
        }
        // Java: 262
        public com.oddlabs.tt.player.Player[] getPlayers()
        {
            return players;
        }
        // Java: 266
        public int getMaxUnitCount()
        {
            return max_unit_count;
        }
        // Java: 270
        public int getMaxBuildingCount()
        {
            return max_building_count;
        }
        // Java: 274
        public bool isShipsEnabled()
        {
            return ships;
        }
        // Java: 278
        public bool isNight()
        {
            return night;
        }
        // Java: 282
        public void toggleNight()
        {
            night = !night;
        }
        // Java: 286
        public com.oddlabs.tt.landscape.NotificationListener getNotificationListener()
        {
            return notification_listener;
        }
        // Java: 290
        public com.oddlabs.tt.landscape.HeightMap getHeightMap()
        {
            return world;
        }
        // Java: 294
        public com.oddlabs.tt.animation.AnimationManager getAnimationManagerGameTime()
        {
            return animation_manager_game_time;
        }
        // Java: 298
        public com.oddlabs.tt.animation.AnimationManager getAnimationManagerRealTime()
        {
            return animation_manager_real_time;
        }
        // Java: 302
        public java.util.Random getRandom()
        {
            return random;
        }
        static World()
        {
            World.GAMESPEEDS = new float[] { 0.0f, (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK / 2), com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK, (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK * 1.75f), (float)(com.oddlabs.tt.animation.AnimationManager.ANIMATION_SECONDS_PER_TICK * 4) };
        }
        public World(com.oddlabs.tt.landscape.AudioImplementation audio_implementation, com.oddlabs.tt.landscape.LandscapeResources landscape_resources, com.oddlabs.tt.model.RacesResources races_resources, com.oddlabs.tt.landscape.NotificationListener notification_listener, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.procedural.Landscape.TerrainType terrain, com.oddlabs.tt.player.PlayerInfo[] player_infos, com.oddlabs.tt.resource.FogInfo fog, org.joml.Vector4fc[] colors) : base()
        {
            java.lang.IO.println("****************** Generating landscape at tick " + (com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().getTick()) + " ********************");
            this.fog = fog;
            this.landscape_resources = landscape_resources;
            this.races_resources = races_resources;
            this.audio_impl = audio_implementation;
            this.max_unit_count = world_params.getMaxUnitCount();
            this.max_building_count = world_params.getMaxBuildingCount();
            this.ships = world_params.isShipsEnabled();
            this.night = world_params.isNight();
            this.notification_listener = notification_listener;
            this.gamespeed = world_params.getInitialGameSpeed();
            this.map_size = world_params.getMapSize();
            this.mode = com.oddlabs.matchmaking.GameMode.STANDARD;
            long time_start = java.lang.JSystem.currentTimeMillis();
            world = new com.oddlabs.tt.landscape.HeightMap(this, world_info.meters_per_world(), world_info.sea_level_meters(), world_info.texels_per_colormap(), world_info.chunks_per_colormap(), world_info.heightmap(), world_info.trees(), world_info.access_grid(), world_info.dock_grid(), world_info.water_grid(), world_info.build_grid(), world_info.island_ids(), world_info.island_infos());
            animation_manager_game_time = new com.oddlabs.tt.animation.AnimationManager();
            animation_manager_real_time = new com.oddlabs.tt.animation.AnimationManager();
            random = new java.util.Random(42);
            java.util.Iterator eachColor = java.util.Arrays.asList(colors).iterator();
            players = ((com.oddlabs.tt.player.Player[])java.util.Arrays.stream(player_infos).map(new java.util.function.Function_L((object info_l) =>
            {
                com.oddlabs.tt.player.PlayerInfo info = (com.oddlabs.tt.player.PlayerInfo)info_l;
                return new com.oddlabs.tt.player.Player(this, info, ((org.joml.Vector4fc)eachColor.next()));
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.player.Player[a0]; })));
            long time_stop = java.lang.JSystem.currentTimeMillis();
            java.lang.IO.println("****************** Finished landscape in " + java.lang.JStr.valueOf(((float)((time_stop - time_start) / 1000.0f))) + " sec ********************");
            this.supply_managers = new com.oddlabs.tt.model.SupplyManagers(this);
            this.unit_grid = new com.oddlabs.tt.pathfinder.UnitGrid(world);
            bool archipelago = world_info.island_infos().size() > 1;
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
            landscape_resources.getDecorations().place(this, world_info.ground(), world_info.seed());
        }
    }
}

// Translated from WorldViewer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 61
    public class WorldViewer : java.lang.Object, com.oddlabs.tt.animation.Animated, java.lang.AutoCloseable, com.oddlabs.tt.camera.CameraHost
    {
        public static string[] GAMESPEED_STRINGS;
        public com.oddlabs.tt.camera.GameCamera camera;
        public com.oddlabs.tt.gui.ActionButtonPanel panel;
        public com.oddlabs.tt.@delegate.SelectionDelegate @delegate;
        public com.oddlabs.tt.net.DistributableTable distributable_table;
        public com.oddlabs.tt.net.PeerHub peerhub;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public com.oddlabs.tt.viewer.NotificationManager notification_manager;
        public com.oddlabs.tt.viewer.InGameInfo ingame_info;
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.viewer.Selection selection;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.render.Picker picker;
        public com.oddlabs.tt.render.DefaultRenderer renderer;
        public com.oddlabs.tt.render.LandscapeRenderer landscape_renderer;
        public com.oddlabs.tt.player.Player local_player;
        public com.oddlabs.tt.landscape.WorldParameters world_params;
        public com.oddlabs.tt.animation.AnimationManager animation_manager_local;
        public com.oddlabs.tt.viewer.Cheat cheat;
        // Java: 200
        public com.oddlabs.tt.animation.AnimationManager getAnimationManagerLocal()
        {
            return animation_manager_local;
        }
        // Java: 204
        public void animate(float t)
        {
            animation_manager_local.runAnimations(t);
        }
        // Java: 209
        public void close()
        {
            com.oddlabs.tt.viewer.WorldViewer.open_viewers = java.lang.Math.max(0, com.oddlabs.tt.viewer.WorldViewer.open_viewers - 1);
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().removeAnimation(this);
            peerhub.close();
            ingame_info.close(this);
            com.oddlabs.tt.render.Renderer.getRenderer().setCheat(null);
        }
        public static int open_viewers;
        // Java: 221
        public com.oddlabs.tt.landscape.WorldParameters getParameters()
        {
            return world_params;
        }
        // Java: 225
        public com.oddlabs.tt.viewer.Cheat getCheat()
        {
            return cheat;
        }
        public bool paused;
        // Java: 231
        public void setPaused(bool paused)
        {
            if (this.paused != paused)
            {
                this.paused = paused;
                peerhub.setPaused(paused);
            }
        }
        // Java: 238
        public bool isPaused()
        {
            return peerhub.isPaused();
        }
        // Java: 242
        public com.oddlabs.tt.player.Player getLocalPlayer()
        {
            return local_player;
        }
        // Java: 246
        public void initPlayer(java.util.ResourceBundle bundle, float[] starting_location, com.oddlabs.tt.net.PlayerSlot slot, com.oddlabs.tt.player.Player player, com.oddlabs.tt.player.UnitInfo unit_info, int initial_gamespeed)
        {
            if (slot.getType() == com.oddlabs.tt.net.PlayerSlot.AI)
            {
                com.oddlabs.tt.player.AI ai = null;
                switch ((ingame_info.resolveAIDifficulty(slot.getAIDifficulty())))
                {
                    case 2:
                    {
                        ai = new com.oddlabs.tt.player.AdvancedAI(player, unit_info, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_NORMAL, world_params.isAdaptiveAiEnabled() || ingame_info.adaptiveAI(), world_params.getAdaptiveAiSeedDifficulty());
                        break;
                    }
                    case 3:
                    {
                        ai = new com.oddlabs.tt.player.AdvancedAI(player, unit_info, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_HARD, world_params.isAdaptiveAiEnabled() || ingame_info.adaptiveAI(), world_params.getAdaptiveAiSeedDifficulty());
                        break;
                    }
                    case 1:
                    {
                        ai = new com.oddlabs.tt.player.AdvancedAI(player, unit_info, com.oddlabs.tt.player.AdvancedAI.DIFFICULTY_EASY, world_params.isAdaptiveAiEnabled() || ingame_info.adaptiveAI(), world_params.getAdaptiveAiSeedDifficulty());
                        break;
                    }
                    case 9:
                    {
                        com.oddlabs.tt.player.ai.ExpertAI expert = new com.oddlabs.tt.player.ai.ExpertAI(player, unit_info);
                        expert.logTo(com.oddlabs.tt.global.Settings.getSettings().last_event_log_dir);
                        ai = expert;
                        break;
                    }
                    case 10:
                    case 11:
                    case 12:
                    {
                        ai = new com.oddlabs.tt.player.gauntlet.GauntletAI(player, unit_info, "");
                        break;
                    }
                    case 6:
                    {
                        ai = new com.oddlabs.tt.player.PassiveAI(player, unit_info, true);
                        break;
                    }
                    case 4:
                    {
                        break;
                    }
                    case 5:
                    {
                        new com.oddlabs.tt.model.Unit(player, 100, 100, null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                        new com.oddlabs.tt.model.Unit(player, 200, 100, null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                        new com.oddlabs.tt.model.Unit(player, 40, 200, null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                        break;
                    }
                    case 7:
                    {
                        ai = new com.oddlabs.tt.player.PassiveAI(player, unit_info, true);
                        break;
                    }
                    case 8:
                    {
                        ai = new com.oddlabs.tt.player.PassiveAI(player, unit_info, false);
                        break;
                    }
                    default:
                    {
                        throw new System.ArgumentException("unexpected difficulty: " + (slot.getAIDifficulty()));
                    }
                }
                player.setAI(ai);
            }
            else
            {
                player.setPreferredGamespeed(initial_gamespeed);
                int slots = java.lang.Math.max(1, starting_location.Length / 2);
                int i = 0;
                for (int j = 0; j < unit_info.numPeons(); j++, i++)
                {
                    new com.oddlabs.tt.model.Unit(player, starting_location[2 * (i % slots)], starting_location[2 * (i % slots) + 1], null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_PEON));
                }
                for (int j_2 = 0; j_2 < unit_info.numRockWarriors(); j_2++, i++)
                {
                    new com.oddlabs.tt.model.Unit(player, starting_location[2 * (i % slots)], starting_location[2 * (i % slots) + 1], null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_ROCK));
                }
                for (int j_3 = 0; j_3 < unit_info.numIronWarriors(); j_3++, i++)
                {
                    new com.oddlabs.tt.model.Unit(player, starting_location[2 * (i % slots)], starting_location[2 * (i % slots) + 1], null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_IRON));
                }
                for (int j_4 = 0; j_4 < unit_info.numRubberWarriors(); j_4++, i++)
                {
                    new com.oddlabs.tt.model.Unit(player, starting_location[2 * (i % slots)], starting_location[2 * (i % slots) + 1], null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_WARRIOR_RUBBER));
                }
                if (unit_info.hasChieftain())
                {
                    com.oddlabs.tt.model.Unit chieftain = default;
                    string chosen = player.getPlayerInfo().getChiefName();
                    if (chosen.isEmpty() && !ingame_info.isMultiplayer() && ((object)player == (object)local_player))
                    {
                        chosen = com.oddlabs.tt.player.PlayerInfo.clean(com.oddlabs.tt.global.Settings.getSettings().chief_name);
                    }
                    if ((player.getRace().getChieftainAI() is com.oddlabs.tt.player.VikingChieftainAI))
                    {
                        chieftain = new com.oddlabs.tt.model.Unit(player, starting_location[2 * (i % slots)], starting_location[2 * (i % slots) + 1], null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), (!chosen.isEmpty() ? chosen : com.oddlabs.tt.util.Utils.getBundleString(bundle, "chieftain_name")), false);
                    }
                    else if ((player.getRace().getChieftainAI() is com.oddlabs.tt.player.NativeChieftainAI))
                    {
                        chieftain = new com.oddlabs.tt.model.Unit(player, starting_location[2 * (i % slots)], starting_location[2 * (i % slots) + 1], null, player.getRace().getUnitTemplate(com.oddlabs.tt.model.Race.UNIT_CHIEFTAIN), (!chosen.isEmpty() ? chosen : com.oddlabs.tt.util.Utils.getBundleString(bundle, "native_chieftain_name")), false);
                    }
                    else
                    {
                        throw new System.Exception("Unknown chieftain AI");
                    }
                    chieftain.increaseMagicEnergy(0, 1000);
                    chieftain.increaseMagicEnergy(1, 1000);
                    player.setActiveChieftain(chieftain);
                    i++;
                }
            }
        }
        // Java: 327
        public void initPlayers(float[][] starting_locations, com.oddlabs.tt.net.PlayerSlot[] slots, com.oddlabs.tt.player.Player[] players, com.oddlabs.tt.player.UnitInfo[] unit_infos, int initial_gamespeed)
        {
            java.util.ResourceBundle bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.player.Player).getName());
            for (int i = 0; i < slots.Length; i++)
            {
                initPlayer(bundle, starting_locations[i], slots[i], players[i], unit_infos[i], initial_gamespeed);
            }
        }
        // Java: 335
        public com.oddlabs.tt.render.LandscapeRenderer getLandscapeRenderer()
        {
            return landscape_renderer;
        }
        // Java: 339
        public com.oddlabs.tt.render.Picker getPicker()
        {
            return picker;
        }
        // Java: 343
        public com.oddlabs.tt.render.DefaultRenderer getRenderer()
        {
            return renderer;
        }
        // Java: 347
        public com.oddlabs.tt.landscape.World getWorld()
        {
            return world;
        }
        // Java: 351
        public com.oddlabs.net.NetworkSelector getNetwork()
        {
            return network;
        }
        // Java: 355
        public com.oddlabs.tt.viewer.Selection getSelection()
        {
            return selection;
        }
        // Java: 359
        public com.oddlabs.tt.viewer.NotificationManager getNotificationManager()
        {
            return notification_manager;
        }
        // Java: 363
        public com.oddlabs.tt.net.DistributableTable getDistributableTable()
        {
            return distributable_table;
        }
        // Java: 367
        public com.oddlabs.tt.gui.GUIRoot getGUIRoot()
        {
            return gui_root;
        }
        // Java: 371
        public bool isMultiplayer()
        {
            return ingame_info.isMultiplayer();
        }
        // Java: 375
        public void abort()
        {
            ingame_info.abort(this);
        }
        // Java: 379
        public void addGameOverGUI(com.oddlabs.tt.@delegate.GameStatsDelegate @delegate, int header_y, com.oddlabs.tt.gui.Group buttons)
        {
            ingame_info.addGameOverGUI(this, @delegate, header_y, buttons);
        }
        // Java: 383
        public void addGUI(com.oddlabs.tt.@delegate.InGameMainMenu menu, com.oddlabs.tt.gui.Group game_infos)
        {
            ingame_info.addGUI(this, menu, game_infos);
        }
        // Java: 387
        public com.oddlabs.tt.camera.GameCamera getCamera()
        {
            return camera;
        }
        // Java: 391
        public com.oddlabs.tt.net.PeerHub getPeerHub()
        {
            return peerhub;
        }
        // Java: 395
        public com.oddlabs.tt.gui.ActionButtonPanel getPanel()
        {
            return panel;
        }
        // Java: 399
        public com.oddlabs.tt.@delegate.SelectionDelegate getDelegate()
        {
            return @delegate;
        }
        static WorldViewer()
        {
            WorldViewer.GAMESPEED_STRINGS = new string[] { "paused", "slow", "normal", "fast", "ludicrous" };
        }
        public WorldViewer(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.landscape.WorldParameters world_params, com.oddlabs.tt.viewer.InGameInfo ingame_info, com.oddlabs.tt.resource.WorldGenerator generator, com.oddlabs.tt.net.PlayerSlot[] player_slots, com.oddlabs.tt.player.UnitInfo[] unit_infos, short player_slot, com.oddlabs.router.SessionID session_id) : base()
        {
            com.oddlabs.tt.viewer.WorldViewer.open_viewers++;
            this.world_params = world_params;
            this.ingame_info = ingame_info;
            this.network = network;
            this.notification_manager = new com.oddlabs.tt.viewer.NotificationManager(gui_root);
            this.cheat = new com.oddlabs.tt.viewer.Cheat(!ingame_info.isMultiplayer());
            com.oddlabs.tt.render.Renderer.getRenderer().setCheat(cheat);
            this.animation_manager_local = new com.oddlabs.tt.animation.AnimationManager();
            com.oddlabs.tt.resource.FogInfo worldFog = generator.getFogInfo();
            com.oddlabs.tt.camera.CameraState camera_state = new com.oddlabs.tt.camera.CameraState(worldFog);
            com.oddlabs.tt.render.MatrixStack modelViewStack = new com.oddlabs.tt.render.MatrixStack();
            com.oddlabs.tt.render.MatrixStack projectionStack = new com.oddlabs.tt.render.MatrixStack();
            com.oddlabs.tt.render.RenderQueues render_queues = new com.oddlabs.tt.render.RenderQueues();
            com.oddlabs.tt.landscape.LandscapeResources landscape_resources = com.oddlabs.tt.landscape.World.loadCommon(render_queues);
            com.oddlabs.tt.model.RacesResources races_resources = com.oddlabs.tt.landscape.World.loadInGame(render_queues);
            com.oddlabs.tt.landscape.AudioImplementation audio_impl = new com.oddlabs.tt.landscape.AudioImplementation_L((com.oddlabs.tt.audio.AudioParameters @params) =>
            {
                return com.oddlabs.tt.audio.AudioManager.getManager().newAudio(camera_state, @params);
            });
            this.distributable_table = new com.oddlabs.tt.net.DistributableTable();
            com.oddlabs.tt.landscape.NotificationListener listener = new WorldViewer.Anon1_NotificationListener(this, gui_root);
            com.oddlabs.tt.player.PlayerInfo[] player_infos = ((com.oddlabs.tt.player.PlayerInfo[])java.util.Arrays.stream(player_slots).map(new java.util.function.Function_L((object a0) => { return ((com.oddlabs.tt.net.PlayerSlot)a0).getInfo(); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.player.PlayerInfo[a0]; })));
            org.joml.Vector4fc[] team_colours = com.oddlabs.tt.global.Settings.getSettings().team_colours;
            org.joml.Vector4fc[] player_colors = new org.joml.Vector4fc[player_slots.Length];
            for (int i = 0; i < player_slots.Length; i++)
            {
                player_colors[i] = team_colours[player_slots[i].getSlot()];
            }
            int[] player_teams = new int[player_infos.Length];
            for (int i_2 = 0; i_2 < player_infos.Length; i_2++)
            {
                player_teams[i_2] = player_infos[i_2].getTeam();
            }
            com.oddlabs.tt.resource.WorldInfo world_info = generator.generate(player_infos.Length, world_params.getInitialUnitCount(), ingame_info.getRandomStartPosition(), world_params.isTeamTogether(), player_teams);
            this.world = com.oddlabs.tt.landscape.World.newWorld(audio_impl, landscape_resources, races_resources, listener, world_params, world_info, generator.getTerrainType(), player_infos, player_colors, worldFog);
            this.local_player = world.getPlayers()[player_slot];
            this.selection = new com.oddlabs.tt.viewer.Selection(local_player);
            landscape_renderer = new com.oddlabs.tt.render.LandscapeRenderer(world, world_info, animation_manager_local);
            this.picker = new com.oddlabs.tt.render.Picker(animation_manager_local, local_player, gui_root, render_queues, landscape_renderer, selection);
            this.renderer = new com.oddlabs.tt.render.DefaultRenderer(cheat, local_player, render_queues, world_info, landscape_renderer, picker, selection, generator, modelViewStack, projectionStack);
            this.gui_root = gui_root;
            bool spectator = (ingame_info is com.oddlabs.tt.viewer.SpectatorInGameInfo);
            this.peerhub = new com.oddlabs.tt.net.PeerHub(animation_manager_local, ingame_info.isMultiplayer(), ingame_info.isRated(), spectator, local_player, player_slots, network, gui_root, notification_manager, distributable_table, session_id, new com.oddlabs.tt.viewer.ViewerStallHandler(this));
            this.camera = new com.oddlabs.tt.camera.GameCamera(this, camera_state);
            this.panel = new com.oddlabs.tt.gui.ActionButtonPanel(this, camera);
            this.@delegate = new com.oddlabs.tt.@delegate.SelectionDelegate(this, camera);
            camera.reset(getLocalPlayer().getStartX(), getLocalPlayer().getStartY());
            initPlayers(world_info.starting_locations(), player_slots, world.getPlayers(), unit_infos, world_params.getInitialGameSpeed());
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager().registerAnimation(this);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
        // mixin of default method CameraHost.getMaxCameraZ
        public float getMaxCameraZ()
        {
            return com.oddlabs.tt.camera.GameCamera.MAX_Z;
        }
        // Java: anonymous class at line 105
        public sealed class Anon1_NotificationListener : java.lang.Object, com.oddlabs.tt.landscape.NotificationListener
        {
            public com.oddlabs.tt.viewer.WorldViewer this_0;
            public com.oddlabs.tt.gui.GUIRoot gui_root;
            public Anon1_NotificationListener(com.oddlabs.tt.viewer.WorldViewer this_0_, com.oddlabs.tt.gui.GUIRoot gui_root_c)
            {
                this.this_0 = this_0_;
                this.gui_root = gui_root_c;
            }
            // Java: 106
            public void gamespeedChanged(int speed)
            {
                gui_root.getInfoPrinter().print(com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.net.PeerHub.bundle, "changed_to_" + java.lang.JStr.valueOf(com.oddlabs.tt.viewer.WorldViewer.GAMESPEED_STRINGS[speed])));
                com.oddlabs.tt.global.Globals.gamespeed = speed;
            }
            // Java: 113
            public void playerGamespeedChanged()
            {
                string result = ((string)java.util.Arrays.stream(this_0.world.getPlayers()).filter(new java.util.function.Predicate_L((object p_l) =>
                {
                    com.oddlabs.tt.player.Player p = (com.oddlabs.tt.player.Player)p_l;
                    return com.oddlabs.tt.landscape.World.isValidGamespeed(p.getPreferredGamespeed());
                })).map(new java.util.function.Function_L((object p_2_l) =>
                {
                    com.oddlabs.tt.player.Player p_2 = (com.oddlabs.tt.player.Player)p_2_l;
                    return java.lang.JStr.valueOf(p_2.getPlayerInfo().getName()) + ": " + java.lang.JStr.valueOf(com.oddlabs.tt.util.ServerMessageBundler.getGamespeedString(p_2.getPreferredGamespeed()));
                })).collect(java.util.stream.Collectors.joining(", ")));
                if (!result.isEmpty() && this_0.isMultiplayer())
                {
                    gui_root.getInfoPrinter().print(result);
                }
            }
            // Java: 123
            public void newAttackNotification(com.oddlabs.tt.model.Selectable target)
            {
                com.oddlabs.tt.player.Player owner = target.getOwner();
                if (((object)owner == (object)this_0.getLocalPlayer()))
                {
                    this_0.notification_manager.newAttackNotification(this_0.animation_manager_local, target, this_0.getLocalPlayer());
                }
            }
            // Java: 130
            public void newSelectableNotification(com.oddlabs.tt.model.Selectable target)
            {
                com.oddlabs.tt.player.Player owner = target.getOwner();
                if (((object)owner == (object)this_0.getLocalPlayer()))
                {
                    this_0.notification_manager.newSelectableNotification(target, this_0.animation_manager_local, this_0.getLocalPlayer());
                }
            }
            // Java: 137
            public void registerTarget(com.oddlabs.tt.util.Target target)
            {
                this_0.distributable_table.register(target);
            }
            // Java: 142
            public void patchesEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1)
            {
                com.oddlabs.tt.util.PortHost.heightsEdited(patch_x0, patch_y0, patch_x1, patch_y1);
            }
            // Java: 147
            public void unregisterTarget(com.oddlabs.tt.util.Target target)
            {
                this_0.distributable_table.unregister(target);
                if ((target is com.oddlabs.tt.model.Selectable selectable))
                {
                    this_0.getSelection().removeFromArmies(selectable);
                }
            }
        }
            public com.oddlabs.tt.viewer.InGameInfo getInGameInfo()
        {
            return ingame_info;
        }
    }
}

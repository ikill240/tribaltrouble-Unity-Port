// Translated from MapEditorLoader.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.mapeditor
{
    // Java: 36
    public class MapEditorLoader : java.lang.Object, com.oddlabs.tt.form.LoadCallback
    {
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.tt.mapeditor.MapSettings settings;
        public string map_name;
        public float[][] heights;
        public com.oddlabs.tt.mapeditor.MapFile.Resources resources;
        public java.util.List starts;
        // Java: 55
        public com.oddlabs.tt.render.UIRenderer load(com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.tt.animation.AnimationManager.freezeTime();
            com.oddlabs.tt.resource.IslandGenerator generator = settings.createGenerator();
            com.oddlabs.tt.player.PlayerInfo[] players = new com.oddlabs.tt.player.PlayerInfo[] { new com.oddlabs.tt.player.PlayerInfo(0, 0, "") };
            com.oddlabs.tt.landscape.WorldParameters world_params = new com.oddlabs.tt.landscape.WorldParameters(com.oddlabs.matchmaking.Game.GAMESPEED_NORMAL, "", 2, com.oddlabs.tt.player.Player.DEFAULT_MAX_UNIT_COUNT);
            com.oddlabs.tt.resource.WorldInfo world_info = generator.generate(players.Length, world_params.getInitialUnitCount(), 0.0f, false, null);
            float[][] terrain = world_info.heightmap();
            bool edited = heights != null && heights.Length == terrain.Length;
            if (edited)
            {
                for (int y = 0; y < terrain.Length; y++)
                {
                    java.lang.JSystem.arraycopy(heights[y], 0, terrain[y], 0, terrain[y].Length);
                }
                com.oddlabs.tt.mapeditor.TerrainEditor.pinEdges(terrain);
            }
            if (resources != null)
            {
                java.util.List lists = java.util.List_.of(world_info.trees(), world_info.palm_trees(), world_info.rocks(), world_info.iron());
                foreach (com.oddlabs.tt.mapeditor.Resource kind in com.oddlabs.tt.mapeditor.Resource.values())
                {
                    ((java.util.List)lists.get(kind.ordinal())).clear();
                    ((java.util.List)lists.get(kind.ordinal())).addAll(resources.of(kind));
                }
            }
            com.oddlabs.tt.render.RenderQueues render_queues = new com.oddlabs.tt.render.RenderQueues();
            com.oddlabs.tt.landscape.LandscapeResources landscape_resources = com.oddlabs.tt.landscape.World.loadCommon(render_queues);
            com.oddlabs.tt.landscape.World world = com.oddlabs.tt.landscape.World.newWorld(com.oddlabs.tt.audio.AudioManager.getManager(), landscape_resources, null, new MapEditorLoader.Anon1_NotificationListener(this), world_params, world_info, generator.getTerrainType(), players, new org.joml.Vector4fc[] { com.oddlabs.tt.global.Settings.getSettings().team_colours[0] }, generator.getFogInfo());
            com.oddlabs.tt.animation.AnimationManager manager = new com.oddlabs.tt.animation.AnimationManager();
            com.oddlabs.tt.render.LandscapeRenderer landscape_renderer = new com.oddlabs.tt.render.LandscapeRenderer(world, world_info, manager);
            com.oddlabs.tt.player.Player local_player = world.getPlayers()[0];
            com.oddlabs.tt.viewer.Selection selection = new com.oddlabs.tt.viewer.Selection(local_player);
            com.oddlabs.tt.render.Picker picker = new com.oddlabs.tt.render.Picker(manager, local_player, gui_root, render_queues, landscape_renderer, selection);
            com.oddlabs.tt.viewer.Cheat view = new com.oddlabs.tt.viewer.Cheat();
            com.oddlabs.tt.render.UIRenderer renderer = new com.oddlabs.tt.render.DefaultRenderer(view, local_player, render_queues, world_info, landscape_renderer, picker, selection, generator, new com.oddlabs.tt.render.MatrixStack(), new com.oddlabs.tt.render.MatrixStack());
            com.oddlabs.tt.mapeditor.GroundTextures ground = com.oddlabs.tt.mapeditor.GroundTextures.create(world_info, settings, com.oddlabs.tt.render.Renderer.getRenderer().getRenderContext(), resources == null);
            if ((edited || resources != null) && ground != null)
            {
                ground.rebuildAll();
            }
            com.oddlabs.tt.mapeditor.ResourceSnapper snapper = new com.oddlabs.tt.mapeditor.ResourceSnapper(world);
            com.oddlabs.tt.mapeditor.AccessMap access_map = new com.oddlabs.tt.mapeditor.AccessMap(terrain, settings);
            com.oddlabs.tt.mapeditor.AccessOverlay access = new com.oddlabs.tt.mapeditor.AccessOverlay(access_map);
            com.oddlabs.tt.mapeditor.TerrainEditor editor = new com.oddlabs.tt.mapeditor.TerrainEditor(world.getHeightMap(), terrain, new com.oddlabs.tt.mapeditor.TerrainEditor.ChangeListener_L((int x0, int y0, int x1, int y1) =>
            {
                snapper.snap(x0, y0, x1, y1);
                if (ground != null)
                {
                    ground.heightsChanged(x0, y0, x1, y1);
                }
                access_map.heightsChanged();
            }));
            com.oddlabs.tt.mapeditor.ResourceLayer layer = new com.oddlabs.tt.mapeditor.ResourceLayer(world, access_map, settings, new com.oddlabs.tt.mapeditor.ResourceLayer.ChangeListener_L((com.oddlabs.tt.mapeditor.ResourceLayer changed, int x0_2, int y0_2, int x1_2, int y1_2) =>
            {
                if (ground != null)
                {
                    ground.resourcesChanged(changed, x0_2, y0_2, x1_2, y1_2);
                }
            }));
            com.oddlabs.tt.mapeditor.MapEditorDelegate @delegate = new com.oddlabs.tt.mapeditor.MapEditorDelegate(network, gui_root, world, manager, picker, view, new com.oddlabs.tt.camera.CameraState(generator.getFogInfo()), editor, ground, access_map, access, layer, settings, map_name, edited, resources != null, starts);
            com.oddlabs.tt.render.Renderer.getRenderer().setMusicPath("/music/menu.ogg", 0.0f);
            gui_root.pushDelegate(@delegate);
            return renderer;
        }
        public MapEditorLoader(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.mapeditor.MapSettings settings, string map_name, float[][] heights, com.oddlabs.tt.mapeditor.MapFile.Resources resources, java.util.List starts) : base()
        {
            this.network = network;
            this.settings = settings;
            this.map_name = map_name;
            this.heights = heights;
            this.resources = resources;
            this.starts = starts;
        }
        // Java: anonymous class at line 85
        public sealed class Anon1_NotificationListener : java.lang.Object, com.oddlabs.tt.landscape.NotificationListener
        {
            public com.oddlabs.tt.mapeditor.MapEditorLoader this_0;
            public Anon1_NotificationListener(com.oddlabs.tt.mapeditor.MapEditorLoader this_0_)
            {
                this.this_0 = this_0_;
            }
            // mixin of default method NotificationListener.newAttackNotification
            public void newAttackNotification(com.oddlabs.tt.model.Selectable target)
            {
            }
            // mixin of default method NotificationListener.newSelectableNotification
            public void newSelectableNotification(com.oddlabs.tt.model.Selectable target)
            {
            }
            // mixin of default method NotificationListener.registerTarget
            public void registerTarget(com.oddlabs.tt.util.Target target)
            {
            }
            // mixin of default method NotificationListener.unregisterTarget
            public void unregisterTarget(com.oddlabs.tt.util.Target target)
            {
            }
            // mixin of default method NotificationListener.patchesEdited
            public void patchesEdited(int patch_x0, int patch_y0, int patch_x1, int patch_y1)
            {
            }
            // mixin of default method NotificationListener.gamespeedChanged
            public void gamespeedChanged(int speed)
            {
            }
            // mixin of default method NotificationListener.playerGamespeedChanged
            public void playerGamespeedChanged()
            {
            }
        }
    }
}

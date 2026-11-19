// GUI port: the world renderer of a WorldViewer / the menu. The engine draws the world itself (FaithfulView); this
// keeps what the Java renderer owned besides drawing: the picker's hover and tool tips, the selected building's
// rally point, and it tells the engine which world and camera to show (TribalTroubleSteam.GuiPort.Host).
#pragma warning disable
using J2CS.Ext;
using TribalTroubleSteam.GuiPort;
namespace com.oddlabs.tt.render
{
    public class DefaultRenderer : java.lang.Object, com.oddlabs.tt.render.UIRenderer
    {
        public readonly com.oddlabs.tt.viewer.Cheat cheat;
        public readonly com.oddlabs.tt.player.Player localPlayer;
        public readonly com.oddlabs.tt.resource.WorldInfo worldInfo;
        public readonly com.oddlabs.tt.render.Picker picker;
        public readonly com.oddlabs.tt.viewer.Selection selection;
        public readonly com.oddlabs.tt.resource.WorldGenerator generator;
        public com.oddlabs.tt.model.Building selectedBuilding { get; private set; }
        public com.oddlabs.tt.camera.CameraState camera { get; private set; }
        public com.oddlabs.tt.gui.GUIRoot guiRoot { get; private set; }
        public bool closed { get; private set; }
        /// <summary>Steam: the spectator's view (who to see the world as), null when playing.</summary>
        public readonly com.oddlabs.tt.viewer.SpectatorView spectatorView;

        public DefaultRenderer(com.oddlabs.tt.viewer.Cheat cheat, com.oddlabs.tt.player.Player local_player, com.oddlabs.tt.render.RenderQueues render_queues, com.oddlabs.tt.resource.WorldInfo world_info, com.oddlabs.tt.render.LandscapeRenderer landscape_renderer, com.oddlabs.tt.render.Picker picker, com.oddlabs.tt.viewer.Selection selection, com.oddlabs.tt.resource.WorldGenerator generator, com.oddlabs.tt.render.MatrixStack model_view, com.oddlabs.tt.render.MatrixStack projection, com.oddlabs.tt.viewer.SpectatorView spectator_view)
        {
            spectatorView = spectator_view;
            this.cheat = cheat; localPlayer = local_player; worldInfo = world_info; this.picker = picker;
            this.selection = selection; this.generator = generator;
        }

        public DefaultRenderer() { }

        public com.oddlabs.tt.landscape.World world => localPlayer?.getWorld();
        public com.oddlabs.tt.resource.WorldInfo WorldInfo => worldInfo;
        public com.oddlabs.tt.resource.WorldGenerator Generator => generator;

        public virtual void setSelectedBuilding(com.oddlabs.tt.model.Building building) => selectedBuilding = building;

        public virtual bool isCheater() => cheat != null && cheat.isEnabled();

        public virtual void startFrame(com.oddlabs.tt.render.state.RenderContext context) { }

        public virtual void render(com.oddlabs.tt.render.state.RenderContext context, com.oddlabs.tt.viewer.AmbientAudio ambient, com.oddlabs.tt.camera.CameraState frustum_state, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            camera = frustum_state; guiRoot = gui_root;
            Host.CurrentWorld = this;
            Host.CurrentCamera = frustum_state;
        }

        public virtual void endFrame(com.oddlabs.tt.render.state.RenderContext context, java.util.function.Consumer guiRenderCallback)
            => guiRenderCallback.accept(context);

        public virtual void pickHover(bool can_hover_behind, com.oddlabs.tt.camera.CameraState camera, int x, int y)
        {
            if (picker == null) return;
            if (can_hover_behind)
            {
                var input = Renderer.getLocalInput();
                picker.pickHoverPhysical(camera, input.getMouseX(), input.getMouseY());
            }
            else picker.resetCurrentHovered();
        }

        public virtual com.oddlabs.tt.gui.ToolTip getToolTip() => picker?.getCurrentToolTip();

        public virtual void close()
        {
            closed = true;
            if (Host.CurrentWorld == this) Host.CurrentWorld = null;
        }
    }
}

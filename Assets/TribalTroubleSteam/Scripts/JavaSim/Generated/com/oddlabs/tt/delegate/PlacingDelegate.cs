// Translated from PlacingDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 22
    public class PlacingDelegate : com.oddlabs.tt.@delegate.ControllableCameraDelegate
    {
        public static java.util.logging.Logger logger;
        public static com.oddlabs.tt.render.LandscapeLocation landscape_hit;
        public com.oddlabs.tt.render.BuildingGhostRenderer ghost;
        public int building_index;
        // Java: 34
        public override int getPlacingBuildingIndex()
        {
            return building_index;
        }
        // Java: 39
        public com.oddlabs.tt.model.BuildingTemplate getTemplate()
        {
            return getViewer().getLocalPlayer().getRace().getBuildingTemplate(building_index);
        }
        // Java: 43
        public void placeObject()
        {
            if (!getViewer().getPicker().pickLocation(getCamera().getState(), com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit))
            {
                com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Pick failed (off map?)");
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid unit_grid = getViewer().getWorld().getUnitGrid();
            int placing_grid_x = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x);
            int placing_grid_y = com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y);
            if (getTemplate().isPlacingLegal(getViewer().getWorld().getUnitGrid(), placing_grid_x, placing_grid_y))
            {
                com.oddlabs.tt.model.Selectable[] peons = getViewer().getSelection().getCurrentSelection().filter(com.oddlabs.tt.model.Abilities.BUILD);
                if (peons.Length > 0)
                {
                    com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Placing building at " + (placing_grid_x) + "," + (placing_grid_y));
                    getViewer().getPeerHub().getPlayerInterface().placeBuilding(peons, building_index, placing_grid_x, placing_grid_y);
                }
                else
                {
                    com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: No peons selected");
                }
                com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Popping delegate");
                pop();
            }
            else
            {
                com.oddlabs.tt.@delegate.PlacingDelegate.logger.info("placeObject: Placement illegal");
            }
        }
        // Java: 67
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
                {
                    placeObject();
                }
                @event.consume();
                return;
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
                {
                    pop();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 88
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            switch ((button).ordinal())
            {
                case 0 /*LEFT*/:
                {
                    placeObject();
                    break;
                }
                case 1 /*RIGHT*/:
                {
                    pop();
                    break;
                }
                default:
                {
                    base.mousePressed(button, x, y);
                    break;
                }
            }
        }
        // Java: 97
        public override void render3D(com.oddlabs.tt.render.LandscapeRenderer renderer, com.oddlabs.tt.render.RenderQueues queues, com.oddlabs.tt.camera.CameraState state, com.oddlabs.tt.render.MatrixStack modelViewStack, com.oddlabs.tt.render.MatrixStack projectionStack)
        {
            if (!getViewer().getPicker().pickLocation(getCamera().getState(), com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit))
            {
                return;
            }
            ghost.render(getViewer().getWorld(), getTemplate(), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.x), com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(com.oddlabs.tt.@delegate.PlacingDelegate.landscape_hit.y), renderer, queues, modelViewStack, projectionStack);
        }
        static PlacingDelegate()
        {
            PlacingDelegate.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.@delegate.PlacingDelegate).getName());
            PlacingDelegate.landscape_hit = new com.oddlabs.tt.render.LandscapeLocation();
        }
        private void init_PlacingDelegate()
        {
            this.ghost = new com.oddlabs.tt.render.BuildingGhostRenderer();
        }
        public PlacingDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.CameraState old_camera, int building_index) : base(viewer, new com.oddlabs.tt.camera.GameCamera(viewer, old_camera))
        {
            init_PlacingDelegate();
            this.building_index = building_index;
        }
    }
}

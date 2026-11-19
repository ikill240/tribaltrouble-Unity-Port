// Translated from RallyPointDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 11
    public class RallyPointDelegate : com.oddlabs.tt.@delegate.TargetDelegate
    {
        public com.oddlabs.tt.model.Building building;
        // Java: 19
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (building.isDead())
            {
                pop();
                return;
            }
            com.oddlabs.tt.util.Target target = getViewer().getPicker().pickRallyPoint(getViewer().getGUIRoot().getDelegate().getCamera().getState(), x, y, building);
            if (target == null)
            {
                return;
            }
            if (building.isValidRallyPoint(target))
            {
                getViewer().getPeerHub().getPlayerInterface().setRallyPoint(building, target);
            }
            else
            {
                getViewer().getPeerHub().getPlayerInterface().setRallyPoint(building, target.getGridX(), target.getGridY());
            }
            pop();
        }
        public RallyPointDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera, com.oddlabs.tt.model.Building building) : base(viewer, camera, com.oddlabs.tt.model.Action.DEFAULT)
        {
            this.building = building;
        }
    }
}

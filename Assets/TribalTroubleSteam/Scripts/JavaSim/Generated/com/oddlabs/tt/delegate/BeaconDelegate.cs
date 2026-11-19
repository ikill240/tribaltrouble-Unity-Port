// Translated from BeaconDelegate.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@delegate
{
    // Java: 10
    public class BeaconDelegate : com.oddlabs.tt.@delegate.TargetDelegate
    {
        // Java: 15
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            com.oddlabs.tt.render.LandscapeLocation landscape_hit = new com.oddlabs.tt.render.LandscapeLocation();
            getViewer().getPicker().pickLocation(getCamera().getState(), landscape_hit);
            getViewer().getPeerHub().sendBeacon(landscape_hit.x, landscape_hit.y);
            pop();
        }
        public BeaconDelegate(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.GameCamera camera) : base(viewer, camera, com.oddlabs.tt.model.Action.DEFAULT)
        {
        }
    }
}

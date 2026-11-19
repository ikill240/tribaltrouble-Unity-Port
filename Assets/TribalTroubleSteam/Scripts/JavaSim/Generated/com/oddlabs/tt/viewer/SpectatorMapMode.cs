// Translated from SpectatorMapMode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 10
    public class SpectatorMapMode : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.viewer.SpectatorView view;
        public com.oddlabs.tt.viewer.PlayerView last_followed;
        public bool last_followed_map_mode;
        public bool last_own_map_mode;
        public bool driving;
        public bool drove_in;
        // Java: 24
        public void animate(float t)
        {
            com.oddlabs.tt.@delegate.SelectionDelegate @delegate = viewer.getDelegate();
            bool own_map_mode = @delegate.isOnMap();
            com.oddlabs.tt.viewer.PlayerView followed = view.getFollowedView();
            if (followed == null && own_map_mode && drove_in && (@delegate.getCamera() is com.oddlabs.tt.camera.MapCamera map_camera))
            {
                map_camera.leaveMap();
                drove_in = false;
            }
            if (followed == null || ((object)viewer.getGUIRoot().getDelegate() != (object)@delegate))
            {
                last_followed = followed;
                last_followed_map_mode = followed != null && followed.isMapMode();
                last_own_map_mode = own_map_mode;
                driving = false;
                return;
            }
            bool followed_map_mode = followed.isMapMode();
            bool followed_changed = ((object)followed != (object)last_followed) || followed_map_mode != last_followed_map_mode;
            last_followed = followed;
            if (own_map_mode == followed_map_mode)
            {
                driving = false;
            }
            else if (followed_changed || driving)
            {
                driving = true;
                if (own_map_mode)
                {
                    (((com.oddlabs.tt.camera.MapCamera)@delegate.getCamera())).leaveMap();
                    drove_in = false;
                }
                else
                {
                    @delegate.enterMapMode();
                    drove_in = true;
                }
            }
            else if (own_map_mode != last_own_map_mode)
            {
                view.freeCamera();
            }
            last_followed_map_mode = followed_map_mode;
            last_own_map_mode = own_map_mode;
        }
        public SpectatorMapMode(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.viewer.SpectatorView view) : base()
        {
            this.viewer = viewer;
            this.view = view;
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}

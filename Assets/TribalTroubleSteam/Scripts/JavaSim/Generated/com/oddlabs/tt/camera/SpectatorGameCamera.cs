// Translated from SpectatorGameCamera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 9
    public class SpectatorGameCamera : com.oddlabs.tt.camera.GameCamera
    {
        public com.oddlabs.tt.viewer.SpectatorView view;
        public bool following;
        public float followed_x;
        public float followed_y;
        public float followed_z;
        public float followed_horiz_angle;
        public float followed_vert_angle;
        // Java: 24
        public override bool limitsUnlocked()
        {
            return following || base.limitsUnlocked();
        }
        // Java: 29
        public override bool edgeScrollEnabled()
        {
            return !following;
        }
        // Java: 34
        public override void doControl(float t)
        {
            com.oddlabs.tt.viewer.PlayerView followed = view.getFollowedView();
            bool snap = followed != null && view.consumeSnap();
            if (followed != null && following && !snap && !atFollowedTarget())
            {
                view.freeCamera();
                followed = null;
            }
            following = followed != null;
            if (following)
            {
                follow(followed);
            }
            base.doControl(t);
            if (!following)
            {
                return;
            }
            if (!atFollowedTarget())
            {
                view.freeCamera();
                following = false;
            }
            else if (snap)
            {
                getState().snapToTarget();
            }
        }
        // Java: 56
        public void follow(com.oddlabs.tt.viewer.PlayerView followed)
        {
            com.oddlabs.tt.camera.CameraState state = getState();
            state.setMaxVertAngle(com.oddlabs.tt.camera.CameraState.MAX_ANGLE_UNLOCKED);
            state.setTargetX(followed.getCameraX());
            state.setTargetY(followed.getCameraY());
            state.setTargetZ(followed.getCameraZ());
            state.setTargetHorizAngle(com.oddlabs.tt.camera.SpectatorGameCamera.nearestAngle(followed.getCameraHorizAngle(), state.getHorizAngle()));
            state.setTargetVertAngle(followed.getCameraVertAngle());
            followed_x = state.getTargetX();
            followed_y = state.getTargetY();
            followed_z = state.getTargetZ();
            followed_horiz_angle = state.getTargetHorizAngle();
            followed_vert_angle = state.getTargetVertAngle();
        }
        // Java: 71
        public bool atFollowedTarget()
        {
            com.oddlabs.tt.camera.CameraState state = getState();
            return state.getTargetX() == followed_x && state.getTargetY() == followed_y && state.getTargetZ() == followed_z && state.getTargetHorizAngle() == followed_horiz_angle && state.getTargetVertAngle() == followed_vert_angle;
        }
        // Java: 78
        public static float nearestAngle(float angle, float reference)
        {
            double turns = java.lang.Math.rint(((float)(angle - reference)) / (2 * java.lang.Math.PI));
            return ((float)((angle - turns * 2 * java.lang.Math.PI)));
        }
        public SpectatorGameCamera(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.camera.CameraState camera, com.oddlabs.tt.viewer.SpectatorView view) : base(viewer, camera)
        {
            this.view = view;
        }
    }
}

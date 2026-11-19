// Translated from MenuCamera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 7
    public class MenuCamera : com.oddlabs.tt.camera.Camera
    {
        public const float ANGLE_DELTA = 0.02f;
        public const float RADIUS = 176.0f;
        public const float HEIGHT = 0.0f;
        public const float LANDSCAPE_OFFSET = 5.0f;
        public const float CENTER_Z = 128.0f;
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.animation.AnimationManager manager;
        public float centerX;
        public float centerY;
        public float center_angle;
        // Java: 29
        public void reset()
        {
            center_angle = 1;
            getState().setCurrentVertAngle((float)(-((float)(java.lang.Math.atan((float)(((float)(com.oddlabs.tt.camera.MenuCamera.HEIGHT - com.oddlabs.tt.camera.MenuCamera.CENTER_Z)) / com.oddlabs.tt.camera.MenuCamera.RADIUS))))));
            updatePos(0.0f);
        }
        // Java: 35
        public void updatePos(float t)
        {
            center_angle = (float)(((float)(center_angle + (float)(com.oddlabs.tt.camera.MenuCamera.ANGLE_DELTA * t))) % ((float)(2 * ((float)(java.lang.Math.PI)))));
            getState().setCurrentX((float)(centerX + (float)(com.oddlabs.tt.camera.MenuCamera.RADIUS * ((float)(java.lang.Math.cos(center_angle))))));
            getState().setCurrentY((float)(centerY + (float)(com.oddlabs.tt.camera.MenuCamera.RADIUS * ((float)(java.lang.Math.sin(center_angle))))));
            getState().setCurrentHorizAngle((float)((float)(((float)(java.lang.Math.PI)) * 0.925f) + center_angle));
            getState().setCurrentZ(com.oddlabs.tt.camera.MenuCamera.LANDSCAPE_OFFSET);
        }
        // Java: 43
        public override void doAnimate(float t)
        {
            updatePos(t);
            world.tick(t);
            manager.runAnimations(t);
        }
        public MenuCamera(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.animation.AnimationManager manager) : base(world.getHeightMap(), new com.oddlabs.tt.camera.CameraState())
        {
            this.world = world;
            this.manager = manager;
            this.centerX = (float)(world.getHeightMap().getMetersPerWorld() / 2.0f);
            this.centerY = (float)(world.getHeightMap().getMetersPerWorld() / 2.0f);
            reset();
        }
    }
}

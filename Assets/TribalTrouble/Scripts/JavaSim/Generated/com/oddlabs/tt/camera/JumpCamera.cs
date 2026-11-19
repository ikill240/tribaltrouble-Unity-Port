// Translated from JumpCamera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 7
    public class JumpCamera : com.oddlabs.tt.camera.Camera
    {
        public const float DEFAULT_METERS_PER_SECOND = 300.0f;
        public const float DEFAULT_MAX_SECONDS = 0.5f;
        public com.oddlabs.tt.@delegate.CameraDelegate @delegate;
        public float dst_x;
        public float dst_y;
        public float dst_z;
        public float dx;
        public float dy;
        public float dz;
        public float factor;
        public float z_accel;
        public float seconds;
        public float z_speed;
        public float temp_z;
        // Java: 57
        public override void doAnimate(float t)
        {
            if (seconds <= 0.0f)
            {
                getState().setTargetX(dst_x);
                getState().setTargetY(dst_y);
                getState().setTargetZ(dst_z);
                @delegate.pop();
                return;
            }
            seconds -= t;
            getState().setTargetX((float)(getState().getTargetX() + (float)((float)(dx * factor) * t)));
            getState().setTargetY((float)(getState().getTargetY() + (float)((float)(dy * factor) * t)));
            temp_z += (float)(z_speed * t);
            getState().setTargetZ(temp_z);
            z_speed += (float)(z_accel * t);
            bounce(getState().getTargetX(), getState().getTargetY(), getState().getTargetZ(), @delegate.getGUIRoot().getWidth(), @delegate.getGUIRoot().getHeight());
        }
        public JumpCamera(com.oddlabs.tt.@delegate.JumpDelegate @delegate, com.oddlabs.tt.camera.GameCamera old_camera, float x, float y) : this(@delegate, old_camera, x, y, com.oddlabs.tt.camera.JumpCamera.DEFAULT_METERS_PER_SECOND, com.oddlabs.tt.camera.JumpCamera.DEFAULT_MAX_SECONDS)
        {
        }
        public JumpCamera(com.oddlabs.tt.@delegate.JumpDelegate @delegate, com.oddlabs.tt.camera.GameCamera old_camera, float x, float y, float meters_per_second, float max_seconds) : base(old_camera.getHeightMap(), old_camera.getState())
        {
            this.@delegate = @delegate;
            @delegate.getViewer().getPicker().pickRotate(old_camera);
            float[] target = old_camera.getRotationPoint();
            float dx = (float)(target[0] - getState().getTargetX());
            float dy = (float)(target[1] - getState().getTargetY());
            float dz = (float)(getHeightMap().getNearestHeight(target[0], target[1]) - getState().getTargetZ());
            float distance_to_landscape = ((float)(java.lang.Math.sqrt((float)((float)((float)(dx * dx) + (float)(dy * dy)) + (float)(dz * dz)))));
            float dir_x = (float)(dx / distance_to_landscape);
            float dir_y = (float)(dy / distance_to_landscape);
            float dir_z = (float)(dz / distance_to_landscape);
            dst_x = (float)(java.lang.JC.f2i(x) - (float)(dir_x * distance_to_landscape));
            dst_y = (float)(java.lang.JC.f2i(y) - (float)(dir_y * distance_to_landscape));
            dst_z = (float)(getHeightMap().getNearestHeight(java.lang.JC.f2i(x), java.lang.JC.f2i(y)) - (float)(dir_z * distance_to_landscape));
            this.dx = (float)(dst_x - getState().getTargetX());
            this.dy = (float)(dst_y - getState().getTargetY());
            this.dz = (float)(dst_z - getState().getTargetZ());
            float distance_to_dst = ((float)(java.lang.Math.sqrt((float)((float)((float)(this.dx * this.dx) + (float)(this.dy * this.dy)) + (float)(this.dz * this.dz)))));
            seconds = java.lang.Math.min((float)(distance_to_dst / meters_per_second), max_seconds);
            factor = (float)(1.0f / seconds);
            z_accel = (float)((float)(-((float)(distance_to_dst / 32))) / ((float)(seconds * 0.5f)));
            z_speed = (float)((float)(this.dz * factor) - (float)(z_accel * ((float)(seconds / 2.0f))));
            temp_z = getState().getTargetZ();
        }
    }
}

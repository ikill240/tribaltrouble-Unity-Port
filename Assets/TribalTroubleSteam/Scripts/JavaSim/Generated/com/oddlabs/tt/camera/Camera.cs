// Translated from Camera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 22
    public abstract class Camera : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public const float LANDSCAPE_OFFSET = 6.0f;
        public const float SMOOTHNESS_FACTOR = 15.0f;
        public const float GROUND_CLEARANCE = 2.2f;
        public java.nio.IntBuffer viewport;
        public org.joml.Matrix4f proj;
        public com.oddlabs.tt.camera.CameraState tmp_camera;
        public int[] viewportArray;
        public float[] hit_result_array;
        public com.oddlabs.tt.landscape.HeightMap heightmap;
        public com.oddlabs.tt.camera.CameraState state;
        public float smoothness_factor;
        // Java: 57
        public com.oddlabs.tt.landscape.HeightMap getHeightMap()
        {
            return heightmap;
        }
        // Java: 61
        public void setSmoothnessFactor(float f)
        {
            smoothness_factor = f;
        }
        // Java: 65
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            state.updateChecksum(checksum);
        }
        // Java: 71
        public void animate(float delta_t)
        {
            doAnimate(delta_t);
            state.animate(delta_t, smoothness_factor);
        }
        // Java: 77
        public abstract void doAnimate(float delta_t);
        // Java: 79
        public void checkPosition()
        {
            int mid = heightmap.getMetersPerWorld() / 2;
            float dx = ((float)(state.getTargetX() - mid));
            float dy = ((float)(state.getTargetY() - mid));
            float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
            if (squared_dist > heightmap.getMetersPerWorld() * heightmap.getMetersPerWorld())
            {
                float scale = (float)(heightmap.getMetersPerWorld() / ((float)(java.lang.Math.sqrt(squared_dist))));
                state.setTargetX((float)((float)(dx * scale) + mid));
                state.setTargetY((float)((float)(dy * scale) + mid));
            }
            if (!bounce(state.getTargetX(), state.getTargetY(), state.getTargetZ(), state.getWidth(), state.getHeight()))
            {
                if (state.getTargetZ() > getMaxZ())
                {
                    state.setTargetZ(getMaxZ());
                }
            }
        }
        // Java: 95
        public virtual float getMaxZ()
        {
            return com.oddlabs.tt.camera.GameCamera.MAX_Z;
        }
        // Java: 99
        public bool bounce(float x, float y, float z, int width, int height)
        {
            bool bounced = false;
            viewport.clear();
            viewport.put(0).put(0).put(width).put(height);
            viewport.flip();
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    float fovy = com.oddlabs.tt.global.Globals.FOV;
                    float aspect = (float)(((float)(width)) / height);
                    float zNear = com.oddlabs.tt.global.Globals.VIEW_MIN;
                    float zFar = com.oddlabs.tt.global.Globals.VIEW_MAX;
                    proj.setPerspective(((float)(java.lang.Math.toRadians(fovy))), aspect, zNear, zFar);
                    tmp_camera.set(state);
                    tmp_camera.setTargetView(proj);
                    org.joml.Matrix4f combinedMatrix = new org.joml.Matrix4f(proj).mul(tmp_camera.getModelView());
                    unproject(i * width, j * height, 0.0f, tmp_camera.getModelView(), combinedMatrix);
                    float hit_x = hit_result_array[0];
                    float hit_y = hit_result_array[1];
                    float hit_z = hit_result_array[2];
                    float dx1 = (float)(hit_x - x);
                    float dy1 = (float)(hit_y - y);
                    float dz1 = (float)(hit_z - z);
                    float inv_length = (float)(com.oddlabs.tt.camera.Camera.LANDSCAPE_OFFSET / ((float)(java.lang.Math.sqrt((float)((float)((float)(dx1 * dx1) + (float)(dy1 * dy1)) + (float)(dz1 * dz1))))));
                    dx1 = (float)(dx1 * inv_length);
                    dy1 = (float)(dy1 * inv_length);
                    dz1 = (float)(dz1 * inv_length);
                    float min_height = java.lang.Math.max(heightmap.getNearestHeight((float)(x + dx1), (float)(y + dy1)), heightmap.getSeaLevelMeters());
                    hit_z = (float)(z + dz1);
                    if (hit_z < min_height)
                    {
                        bounced = true;
                        z = (float)((float)(z + min_height) - hit_z);
                    }
                }
            }
            float min_height_2 = (float)(heightmap.getNearestHeight(x, y) + com.oddlabs.tt.camera.Camera.GROUND_CLEARANCE);
            if (z < min_height_2)
            {
                bounced = true;
                z = min_height_2;
            }
            if (bounced)
            {
                state.setTargetZ(z);
            }
            return bounced;
        }
        // Java: 148
        public void unproject(float winx, float winy, float winz, org.joml.Matrix4f model, org.joml.Matrix4f proj)
        {
            viewport.get(0, viewportArray, 0, 4);
            org.joml.Vector3f tempVector = new org.joml.Vector3f();
            proj.mul(model);
            proj.unproject(winx, winy, winz, viewportArray, tempVector);
            hit_result_array[0] = tempVector.x_f;
            hit_result_array[1] = tempVector.y_f;
            hit_result_array[2] = tempVector.z_f;
        }
        // Java: 161
        public com.oddlabs.tt.camera.CameraState getState()
        {
            return state;
        }
        // Java: 165
        public void disable()
        {
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().removeAnimation(this);
        }
        // Java: 169
        public virtual void enable()
        {
            com.oddlabs.tt.@event.LocalEventQueue.getQueue().getHighPrecisionManager().registerAnimation(this);
        }
        // Java: 173
        public virtual void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
        }
        // Java: 176
        public virtual void mouseScrolled(int amount)
        {
        }
        // Java: 179
        public virtual void mouseMoved(int x, int y)
        {
        }
        private void init_Camera()
        {
            this.viewport = ((java.nio.IntBuffer)java.util.Objects.requireNonNull(org.lwjgl.BufferUtils.createIntBuffer(16)));
            this.proj = new org.joml.Matrix4f();
            this.tmp_camera = new com.oddlabs.tt.camera.CameraState();
            this.viewportArray = new int[4];
            this.hit_result_array = new float[3];
            this.smoothness_factor = com.oddlabs.tt.camera.Camera.SMOOTHNESS_FACTOR;
        }
        public Camera(com.oddlabs.tt.landscape.HeightMap heightmap, com.oddlabs.tt.camera.CameraState state) : base()
        {
            init_Camera();
            this.heightmap = heightmap;
            this.state = state;
        }
    }
}

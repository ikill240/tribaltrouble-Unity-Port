// Translated from CameraState.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 10
    public class CameraState : java.lang.Object
    {
        public const float MIN_ANGLE = (-1.5707964f);
        public const float MAX_ANGLE = (-1.0E-4f);
        public org.joml.Matrix4f modl;
        public org.joml.Matrix4f proj;
        public org.joml.Matrix4f proj_modl;
        public float[][] frustum;
        public org.joml.Vector3f f;
        public org.joml.Vector3f s;
        public org.joml.Vector3f u;
        public float target_camera_x;
        public float target_camera_y;
        public float target_camera_z;
        public float target_vert_angle;
        public float target_horiz_angle;
        public float camera_x;
        public float camera_y;
        public float camera_z;
        public float vert_angle;
        public float horiz_angle;
        public com.oddlabs.tt.resource.FogInfo fog;
        public bool no_detail_mode;
        public int width;
        public int height;
        // Java: 48
        public void setFog(com.oddlabs.tt.resource.FogInfo fog)
        {
            this.fog = fog;
        }
        // Java: 52
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            checksum.update(camera_x);
            checksum.update(camera_y);
            checksum.update(camera_z);
            checksum.update(vert_angle);
            checksum.update(horiz_angle);
        }
        // Java: 61
        public float getTargetX()
        {
            return target_camera_x;
        }
        // Java: 65
        public float getCurrentX()
        {
            return camera_x;
        }
        // Java: 69
        public void setTargetX(float x)
        {
            target_camera_x = x;
        }
        // Java: 73
        public void setCurrentX(float x)
        {
            camera_x = target_camera_x = x;
        }
        // Java: 77
        public float getTargetY()
        {
            return target_camera_y;
        }
        // Java: 81
        public float getCurrentY()
        {
            return camera_y;
        }
        // Java: 85
        public void setTargetY(float y)
        {
            target_camera_y = y;
        }
        // Java: 89
        public void setCurrentY(float y)
        {
            camera_y = target_camera_y = y;
        }
        // Java: 93
        public float getTargetZ()
        {
            return target_camera_z;
        }
        // Java: 97
        public float getCurrentZ()
        {
            return camera_z;
        }
        // Java: 101
        public com.oddlabs.tt.resource.FogInfo getFog()
        {
            return fog;
        }
        // Java: 105
        public void snapToTarget()
        {
            camera_x = target_camera_x;
            camera_y = target_camera_y;
            camera_z = target_camera_z;
            vert_angle = target_vert_angle;
            horiz_angle = target_horiz_angle;
        }
        // Java: 113
        public void animate(float delta_t, float smoothness_factor)
        {
            camera_x = animateValue(delta_t, camera_x, target_camera_x, smoothness_factor);
            camera_y = animateValue(delta_t, camera_y, target_camera_y, smoothness_factor);
            camera_z = animateValue(delta_t, camera_z, target_camera_z, smoothness_factor);
            vert_angle = animateValue(delta_t, vert_angle, target_vert_angle, smoothness_factor);
            horiz_angle = animateValue(delta_t, horiz_angle, target_horiz_angle, smoothness_factor);
        }
        // Java: 121
        public float animateValue(float delta_t, float current, float target, float smoothness_factor)
        {
            return (float)(current + (float)(((float)(target - current)) * java.lang.Math.min((float)(delta_t * smoothness_factor), 1.0f)));
        }
        // Java: 125
        public void setTargetZ(float z)
        {
            target_camera_z = z;
        }
        // Java: 129
        public void setCurrentZ(float z)
        {
            camera_z = target_camera_z = z;
        }
        // Java: 133
        public void setTargetVertAngle(float angle)
        {
            target_vert_angle = capVertAngle(angle);
        }
        // Java: 137
        public bool inNoDetailMode()
        {
            return no_detail_mode;
        }
        // Java: 141
        public void setNoDetailMode(bool s)
        {
            no_detail_mode = s;
        }
        // Java: 145
        public void set(com.oddlabs.tt.camera.CameraState camera)
        {
            setTargetX(camera.getTargetX());
            setTargetY(camera.getTargetY());
            setTargetZ(camera.getTargetZ());
            setTargetVertAngle(camera.getTargetVertAngle());
            setTargetHorizAngle(camera.getTargetHorizAngle());
            setNoDetailMode(camera.inNoDetailMode());
            setFog(camera.getFog());
            camera_x = camera.camera_x;
            camera_y = camera.camera_y;
            camera_z = camera.camera_z;
            vert_angle = camera.vert_angle;
            horiz_angle = camera.horiz_angle;
            modl.set(camera.modl);
            proj.set(camera.proj);
            proj_modl.set(camera.proj_modl);
            for (int i = 0; i < frustum.Length; i++)
            {
                java.lang.JSystem.arraycopy(camera.frustum[i], 0, frustum[i], 0, frustum[i].Length);
            }
            f.set(camera.f);
            s.set(camera.s);
            u.set(camera.u);
            width = camera.width;
            height = camera.height;
        }
        // Java: 171
        public void setCurrentVertAngle(float angle)
        {
            vert_angle = target_vert_angle = capVertAngle(angle);
        }
        // Java: 175
        public float capVertAngle(float angle)
        {
            if (angle < com.oddlabs.tt.camera.CameraState.MIN_ANGLE)
            {
                return com.oddlabs.tt.camera.CameraState.MIN_ANGLE;
            }
            else
            {
                return java.lang.Math.min(angle, com.oddlabs.tt.camera.CameraState.MAX_ANGLE);
            }
        }
        // Java: 181
        public float getTargetVertAngle()
        {
            return target_vert_angle;
        }
        // Java: 185
        public float getCurrentVertAngle()
        {
            return vert_angle;
        }
        // Java: 189
        public void setTargetHorizAngle(float angle)
        {
            target_horiz_angle = angle;
        }
        // Java: 193
        public void setCurrentHorizAngle(float angle)
        {
            horiz_angle = target_horiz_angle = angle;
        }
        // Java: 197
        public float getTargetHorizAngle()
        {
            return target_horiz_angle;
        }
        // Java: 201
        public float getHorizAngle()
        {
            return horiz_angle;
        }
        // Java: 205
        public com.oddlabs.tt.camera.CameraState reflectCamera(float seaLevel)
        {
            com.oddlabs.tt.camera.CameraState m = new com.oddlabs.tt.camera.CameraState(fog);
            m.width = width;
            m.height = height;
            m.no_detail_mode = no_detail_mode;
            m.camera_x = camera_x;
            m.target_camera_x = target_camera_x;
            m.camera_y = camera_y;
            m.target_camera_y = target_camera_y;
            m.camera_z = (float)(camera_z - (float)(2 * ((float)(camera_z - seaLevel))));
            m.target_camera_z = (float)(target_camera_z - (float)(2 * ((float)(target_camera_z - seaLevel))));
            m.vert_angle = (float)(-vert_angle);
            m.target_vert_angle = (float)(-target_vert_angle);
            m.horiz_angle = horiz_angle;
            m.target_horiz_angle = target_horiz_angle;
            m.doSetView(m.camera_x, m.camera_y, m.camera_z, m.horiz_angle, m.vert_angle, new org.joml.Matrix4f(proj));
            return m;
        }
        // Java: 224
        public void setCamera(float x, float y, float z, float va, float ha)
        {
            setCurrentX(x);
            setCurrentY(y);
            setCurrentZ(z);
            setCurrentVertAngle(va);
            setCurrentHorizAngle(ha);
        }
        // Java: 232
        public void updateDirectionAndNormal(org.joml.Vector3f f, org.joml.Vector3f u, org.joml.Vector3f s)
        {
            com.oddlabs.tt.camera.CameraState.updateDirectionAndNormal(horiz_angle, vert_angle, f, u, s);
        }
        public static org.joml.Vector3f tmp;
        public static org.joml.Vector3f tmp2;
        // Java: 239
        public static void updateDirectionAndNormal(float hangle, float vangle, org.joml.Vector3f f, org.joml.Vector3f u, org.joml.Vector3f s)
        {
            float radius = ((float)(java.lang.Math.cos(vangle)));
            float dir_x = ((float)(java.lang.Math.cos(hangle)));
            float dir_y = ((float)(java.lang.Math.sin(hangle)));
            float dir_z = ((float)(java.lang.Math.sin(vangle)));
            f.set((float)(dir_x * radius), (float)(dir_y * radius), dir_z);
            com.oddlabs.tt.camera.CameraState.tmp.set((float)(-dir_y), dir_x, 0);
            f.cross(com.oddlabs.tt.camera.CameraState.tmp, com.oddlabs.tt.camera.CameraState.tmp2);
            f.cross(com.oddlabs.tt.camera.CameraState.tmp2, s);
            s.normalize();
            s.cross(f, u);
            u.normalize();
        }
        // Java: 254
        public void setTargetView(org.joml.Matrix4f proj)
        {
            doSetView(target_camera_x, target_camera_y, target_camera_z, target_horiz_angle, target_vert_angle, proj);
        }
        // Java: 258
        public void setView(org.joml.Matrix4f proj, int width, int height)
        {
            this.width = width;
            this.height = height;
            doSetView(camera_x, camera_y, camera_z, horiz_angle, vert_angle, proj);
        }
        // Java: 264
        public org.joml.Matrix4f getModelView()
        {
            return modl;
        }
        // Java: 268
        public org.joml.Matrix4f getProjectionMatrix()
        {
            return proj;
        }
        // Java: 272
        public org.joml.Matrix4f getProjectionModelView()
        {
            return proj_modl;
        }
        // Java: 276
        public float[][] getFrustum()
        {
            return frustum;
        }
        // Java: 280
        public void doSetView(float cx, float cy, float cz, float hangle, float vangle, org.joml.Matrix4f proj)
        {
            com.oddlabs.tt.camera.CameraState.updateDirectionAndNormal(hangle, vangle, f, u, s);
            modl.identity().lookAt(cx, cy, cz, (float)(cx + f.x_f), (float)(cy + f.y_f), (float)(cz + f.z_f), u.x_f, u.y_f, u.z_f);
            this.proj.set(proj);
            proj.mul(modl, proj_modl);
            findFrustumPlanes(proj_modl);
        }
        // Java: 294
        public void findFrustumPlanes(org.joml.Matrix4f matrix)
        {
            frustum[0][0] = (float)(matrix.m03() - matrix.m00());
            frustum[0][1] = (float)(matrix.m13() - matrix.m10());
            frustum[0][2] = (float)(matrix.m23() - matrix.m20());
            frustum[0][3] = (float)(matrix.m33() - matrix.m30());
            frustum[1][0] = (float)(matrix.m03() + matrix.m00());
            frustum[1][1] = (float)(matrix.m13() + matrix.m10());
            frustum[1][2] = (float)(matrix.m23() + matrix.m20());
            frustum[1][3] = (float)(matrix.m33() + matrix.m30());
            frustum[2][0] = (float)(matrix.m03() + matrix.m01());
            frustum[2][1] = (float)(matrix.m13() + matrix.m11());
            frustum[2][2] = (float)(matrix.m23() + matrix.m21());
            frustum[2][3] = (float)(matrix.m33() + matrix.m31());
            frustum[3][0] = (float)(matrix.m03() - matrix.m01());
            frustum[3][1] = (float)(matrix.m13() - matrix.m11());
            frustum[3][2] = (float)(matrix.m23() - matrix.m21());
            frustum[3][3] = (float)(matrix.m33() - matrix.m31());
            frustum[4][0] = (float)(matrix.m03() - matrix.m02());
            frustum[4][1] = (float)(matrix.m13() - matrix.m12());
            frustum[4][2] = (float)(matrix.m23() - matrix.m22());
            frustum[4][3] = (float)(matrix.m33() - matrix.m32());
            frustum[5][0] = (float)(matrix.m03() + matrix.m02());
            frustum[5][1] = (float)(matrix.m13() + matrix.m12());
            frustum[5][2] = (float)(matrix.m23() + matrix.m22());
            frustum[5][3] = (float)(matrix.m33() + matrix.m32());
            foreach (float[] frustum1 in frustum)
            {
                float length_inv = (float)(1.0f / (((float)(java.lang.Math.sqrt((float)((float)((float)(frustum1[0] * frustum1[0]) + (float)(frustum1[1] * frustum1[1])) + (float)(frustum1[2] * frustum1[2])))))));
                frustum1[0] *= length_inv;
                frustum1[1] *= length_inv;
                frustum1[2] *= length_inv;
                frustum1[3] *= length_inv;
            }
        }
        // Java: 342
        public int getWidth()
        {
            return width;
        }
        // Java: 346
        public int getHeight()
        {
            return height;
        }
        static CameraState()
        {
            CameraState.tmp = new org.joml.Vector3f();
            CameraState.tmp2 = new org.joml.Vector3f();
        }
        private void init_CameraState()
        {
            this.modl = new org.joml.Matrix4f();
            this.proj = new org.joml.Matrix4f();
            this.proj_modl = new org.joml.Matrix4f();
            this.frustum = java.lang.JA.newArray2<float>(6, 4);
            this.f = new org.joml.Vector3f();
            this.s = new org.joml.Vector3f();
            this.u = new org.joml.Vector3f();
        }
        public CameraState() : this(new com.oddlabs.tt.resource.FogInfo(com.oddlabs.tt.resource.FogInfo.Mode.NONE, com.oddlabs.util.Color.BLACK, 0.0f))
        {
        }
        public CameraState(com.oddlabs.tt.resource.FogInfo fog) : base()
        {
            init_CameraState();
            this.fog = fog;
        }
    }
}

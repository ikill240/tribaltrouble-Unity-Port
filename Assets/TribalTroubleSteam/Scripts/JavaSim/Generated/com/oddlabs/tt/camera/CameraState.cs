// Translated from CameraState.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 11
    public class CameraState : java.lang.Object
    {
        public const float MIN_ANGLE = (-1.5707964f);
        public const float MAX_ANGLE = (-1.0E-4f);
        public const float MAX_ANGLE_UNLOCKED = 0.5235988f;
        public const float HORIZON_EPSILON = 0.01f;
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
        public float max_vert_angle;
        public com.oddlabs.tt.resource.FogInfo fog;
        public bool no_detail_mode;
        public int width;
        public int height;
        // Java: 52
        public void setFog(com.oddlabs.tt.resource.FogInfo fog)
        {
            this.fog = fog;
        }
        // Java: 56
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            checksum.update(camera_x);
            checksum.update(camera_y);
            checksum.update(camera_z);
            checksum.update(vert_angle);
            checksum.update(horiz_angle);
        }
        // Java: 65
        public float getTargetX()
        {
            return target_camera_x;
        }
        // Java: 69
        public float getCurrentX()
        {
            return camera_x;
        }
        // Java: 73
        public void setTargetX(float x)
        {
            target_camera_x = x;
        }
        // Java: 77
        public void setCurrentX(float x)
        {
            camera_x = target_camera_x = x;
        }
        // Java: 81
        public float getTargetY()
        {
            return target_camera_y;
        }
        // Java: 85
        public float getCurrentY()
        {
            return camera_y;
        }
        // Java: 89
        public void setTargetY(float y)
        {
            target_camera_y = y;
        }
        // Java: 93
        public void setCurrentY(float y)
        {
            camera_y = target_camera_y = y;
        }
        // Java: 97
        public float getTargetZ()
        {
            return target_camera_z;
        }
        // Java: 101
        public float getCurrentZ()
        {
            return camera_z;
        }
        // Java: 105
        public com.oddlabs.tt.resource.FogInfo getFog()
        {
            return fog;
        }
        // Java: 109
        public void snapToTarget()
        {
            camera_x = target_camera_x;
            camera_y = target_camera_y;
            camera_z = target_camera_z;
            vert_angle = target_vert_angle;
            horiz_angle = target_horiz_angle;
        }
        // Java: 117
        public void animate(float delta_t, float smoothness_factor)
        {
            camera_x = animateValue(delta_t, camera_x, target_camera_x, smoothness_factor);
            camera_y = animateValue(delta_t, camera_y, target_camera_y, smoothness_factor);
            camera_z = animateValue(delta_t, camera_z, target_camera_z, smoothness_factor);
            vert_angle = animateValue(delta_t, vert_angle, target_vert_angle, smoothness_factor);
            horiz_angle = animateValue(delta_t, horiz_angle, target_horiz_angle, smoothness_factor);
        }
        // Java: 125
        public float animateValue(float delta_t, float current, float target, float smoothness_factor)
        {
            return (float)(current + (float)(((float)(target - current)) * java.lang.Math.min((float)(delta_t * smoothness_factor), 1.0f)));
        }
        // Java: 129
        public void setTargetZ(float z)
        {
            target_camera_z = z;
        }
        // Java: 133
        public void setCurrentZ(float z)
        {
            camera_z = target_camera_z = z;
        }
        // Java: 137
        public void setTargetVertAngle(float angle)
        {
            target_vert_angle = capVertAngle(angle);
        }
        // Java: 141
        public bool inNoDetailMode()
        {
            return no_detail_mode;
        }
        // Java: 145
        public void setNoDetailMode(bool s)
        {
            no_detail_mode = s;
        }
        // Java: 149
        public void set(com.oddlabs.tt.camera.CameraState camera)
        {
            max_vert_angle = camera.max_vert_angle;
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
        // Java: 176
        public void setCurrentVertAngle(float angle)
        {
            vert_angle = target_vert_angle = capVertAngle(angle);
        }
        // Java: 180
        public float capVertAngle(float angle)
        {
            if (angle < com.oddlabs.tt.camera.CameraState.MIN_ANGLE)
            {
                return com.oddlabs.tt.camera.CameraState.MIN_ANGLE;
            }
            else
            {
                return java.lang.Math.min(angle, max_vert_angle);
            }
        }
        // Java: 186
        public void setMaxVertAngle(float angle)
        {
            max_vert_angle = java.lang.Math.max(com.oddlabs.tt.camera.CameraState.MIN_ANGLE, java.lang.Math.min(angle, com.oddlabs.tt.camera.CameraState.MAX_ANGLE_UNLOCKED));
            target_vert_angle = capVertAngle(target_vert_angle);
        }
        // Java: 191
        public float getTargetVertAngle()
        {
            return target_vert_angle;
        }
        // Java: 195
        public float getCurrentVertAngle()
        {
            return vert_angle;
        }
        // Java: 199
        public void setTargetHorizAngle(float angle)
        {
            target_horiz_angle = angle;
        }
        // Java: 203
        public void setCurrentHorizAngle(float angle)
        {
            horiz_angle = target_horiz_angle = angle;
        }
        // Java: 207
        public float getTargetHorizAngle()
        {
            return target_horiz_angle;
        }
        // Java: 211
        public float getHorizAngle()
        {
            return horiz_angle;
        }
        // Java: 215
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
        // Java: 234
        public org.joml.Vector3f solveForZ(org.joml.Vector3f p0, org.joml.Vector3f p1, float z)
        {
            float dz = (float)(p1.z() - p0.z());
            float t = (float)(((float)(z - p0.z())) / dz);
            return new org.joml.Vector3f((float)(p0.x() + (float)(t * ((float)(p1.x() - p0.x())))), (float)(p0.y() + (float)(t * ((float)(p1.y() - p0.y())))), z);
        }
        // Java: 243
        public com.oddlabs.tt.camera.CameraState sunCamera(org.joml.Vector3f lightDirection, float minZ, float maxZ, float size, float seaLevel, int width, int height)
        {
            com.oddlabs.tt.camera.CameraState m = new com.oddlabs.tt.camera.CameraState();
            m.width = width;
            m.height = height;
            float halfFov = (float)(((float)(java.lang.Math.toRadians(com.oddlabs.tt.global.Globals.FOV))) * 0.5f);
            float maxVertAngle = (float)(vert_angle + halfFov);
            bool belowHorizon = java.lang.Math.sin(maxVertAngle) < 0.0f;
            float upperPtRatio = (belowHorizon ? 1.0f : (float)(((float)((java.lang.Math.tan((float)(-vert_angle)) / java.lang.Math.tan(halfFov)))) - com.oddlabs.tt.camera.CameraState.HORIZON_EPSILON));
            float maxX = size;
            float minX = 0.0f;
            float maxY = size;
            if (upperPtRatio > (float)(-1.0f))
            {
                org.joml.Matrix4f invMat = new org.joml.Matrix4f();
                getProjectionModelView().invert(invMat);
                org.joml.Vector3f lowerLeft0 = new org.joml.Vector3f();
                org.joml.Vector3f lowerLeft1 = new org.joml.Vector3f();
                org.joml.Vector3f lowerRight0 = new org.joml.Vector3f();
                org.joml.Vector3f lowerRight1 = new org.joml.Vector3f();
                org.joml.Vector3f upperLeft0 = new org.joml.Vector3f();
                org.joml.Vector3f upperLeft1 = new org.joml.Vector3f();
                org.joml.Vector3f upperRight0 = new org.joml.Vector3f();
                org.joml.Vector3f upperRight1 = new org.joml.Vector3f();
                invMat.transformProject((float)(-1.0f), (float)(-1.0f), (float)(-1.0f), lowerLeft0);
                invMat.transformProject((float)(-1.0f), (float)(-1.0f), +1.0f, lowerLeft1);
                invMat.transformProject(+1.0f, (float)(-1.0f), (float)(-1.0f), lowerRight0);
                invMat.transformProject(+1.0f, (float)(-1.0f), +1.0f, lowerRight1);
                invMat.transformProject((float)(-1.0f), upperPtRatio, (float)(-1.0f), upperLeft0);
                invMat.transformProject((float)(-1.0f), upperPtRatio, +1.0f, upperLeft1);
                invMat.transformProject(+1.0f, upperPtRatio, (float)(-1.0f), upperRight0);
                invMat.transformProject(+1.0f, upperPtRatio, +1.0f, upperRight1);
                org.joml.Vector3f[] pts = new org.joml.Vector3f[8];
                pts[0] = solveForZ(lowerLeft0, lowerLeft1, seaLevel);
                pts[1] = solveForZ(lowerRight0, lowerRight1, seaLevel);
                pts[2] = solveForZ(upperLeft0, upperLeft1, seaLevel);
                pts[3] = solveForZ(upperRight0, upperRight1, seaLevel);
                pts[4] = solveForZ(lowerLeft0, lowerLeft1, maxZ);
                pts[5] = solveForZ(lowerRight0, lowerRight1, maxZ);
                pts[6] = solveForZ(upperLeft0, upperLeft1, maxZ);
                pts[7] = solveForZ(upperRight0, upperRight1, maxZ);
                maxX = pts[0].x();
                minX = pts[0].x();
                maxY = pts[0].y();
                for (int i = 1; i < pts.Length; i++)
                {
                    maxX = java.lang.Math.max(maxX, pts[i].x());
                    minX = java.lang.Math.min(minX, pts[i].x());
                    maxY = java.lang.Math.max(maxY, pts[i].y());
                }
                minX = (float)(((float)(java.lang.Math.floor((float)(minX / 32.0f)))) * 32.0f);
                maxX = (float)(((float)(java.lang.Math.ceil((float)(maxX / 32.0f)))) * 32.0f);
                maxY = (float)(((float)(java.lang.Math.ceil((float)(maxY / 32.0f)))) * 32.0f);
                maxX = java.lang.Math.clamp(maxX, 0.0f, size);
                minX = java.lang.Math.clamp(minX, 0.0f, size);
                maxY = java.lang.Math.clamp(maxY, 0.0f, size);
            }
            m.camera_x = m.target_camera_x = (float)(((float)(maxX + minX)) * 0.5f);
            m.camera_y = m.target_camera_y = 0.0f;
            m.camera_z = m.target_camera_z = maxZ;
            float sin = lightDirection.z();
            float lightAngle = ((float)(java.lang.Math.asin(sin)));
            float cos = ((float)(java.lang.Math.cos(lightAngle)));
            m.horiz_angle = m.target_horiz_angle = (float)(((float)(java.lang.Math.PI)) * 0.5f);
            m.vert_angle = m.target_vert_angle = (float)(-lightAngle);
            float rangeZ = (float)(maxZ - minZ);
            float opp = (float)(sin * rangeZ);
            float lowerY = ((float)(java.lang.Math.sqrt((float)((float)(rangeZ * rangeZ) - (float)(opp * opp)))));
            float upperY = (float)(sin * maxY);
            float halfRangeX = (float)(((float)(maxX - minX)) * 0.5f);
            float far = (float)(maxY / cos);
            org.joml.Matrix4f ortho = new org.joml.Matrix4f().ortho((float)(-halfRangeX), halfRangeX, (float)(-lowerY), upperY, +0.0f, far);
            m.doSetView(m.camera_x, m.camera_y, m.camera_z, m.horiz_angle, m.vert_angle, ortho);
            return m;
        }
        // Java: 342
        public void setCamera(float x, float y, float z, float va, float ha)
        {
            setCurrentX(x);
            setCurrentY(y);
            setCurrentZ(z);
            setCurrentVertAngle(va);
            setCurrentHorizAngle(ha);
        }
        // Java: 350
        public void updateDirectionAndNormal(org.joml.Vector3f f, org.joml.Vector3f u, org.joml.Vector3f s)
        {
            com.oddlabs.tt.camera.CameraState.updateDirectionAndNormal(horiz_angle, vert_angle, f, u, s);
        }
        public static org.joml.Vector3f tmp;
        public static org.joml.Vector3f tmp2;
        // Java: 357
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
        // Java: 372
        public void setTargetView(org.joml.Matrix4f proj)
        {
            doSetView(target_camera_x, target_camera_y, target_camera_z, target_horiz_angle, target_vert_angle, proj);
        }
        // Java: 376
        public void setView(org.joml.Matrix4f proj, int width, int height)
        {
            this.width = width;
            this.height = height;
            doSetView(camera_x, camera_y, camera_z, horiz_angle, vert_angle, proj);
        }
        // Java: 382
        public org.joml.Matrix4f getModelView()
        {
            return modl;
        }
        // Java: 386
        public org.joml.Matrix4f getProjectionMatrix()
        {
            return proj;
        }
        // Java: 390
        public org.joml.Matrix4f getProjectionModelView()
        {
            return proj_modl;
        }
        // Java: 394
        public float[][] getFrustum()
        {
            return frustum;
        }
        // Java: 398
        public void doSetView(float cx, float cy, float cz, float hangle, float vangle, org.joml.Matrix4f proj)
        {
            com.oddlabs.tt.camera.CameraState.updateDirectionAndNormal(hangle, vangle, f, u, s);
            modl.identity().lookAt(cx, cy, cz, (float)(cx + f.x_f), (float)(cy + f.y_f), (float)(cz + f.z_f), u.x_f, u.y_f, u.z_f);
            this.proj.set(proj);
            proj.mul(modl, proj_modl);
            findFrustumPlanes(proj_modl);
        }
        // Java: 412
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
        // Java: 460
        public int getWidth()
        {
            return width;
        }
        // Java: 464
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
            this.max_vert_angle = com.oddlabs.tt.camera.CameraState.MAX_ANGLE;
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

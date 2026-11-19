// Translated from GameCamera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 16
    public class GameCamera : com.oddlabs.tt.camera.Camera
    {
        public const int SCROLL_BUFFER = 5;
        public const float INIT_DISTANCE = 50.0f;
        public const float ANGLE_DELTA = 1.5707964f;
        public const float MAX_Z = 1500.0f;
        public const float GROUND_CLEARANCE = 1.0f;
        public static float ZOOM_Z_DIR_MIN;
        public const float SCROLL_ACCELERATION_SECONDS_MAX = 1.0f;
        public const float SCROLL_ACCELERATION_FACTOR = 2.5f;
        public const float SCROLL_START_MAX_SPEED = 80.0f;
        public const float ROTATE_PICKING_ANGLE_MAX = (-0.6981317f);
        public const float ZOOM_SPEED = 40.0f;
        public com.oddlabs.tt.camera.CameraHost viewer;
        public float left_dir_x;
        public float left_dir_y;
        public float scroll_x;
        public float scroll_y;
        public float scrolling_x;
        public float scrolling_y;
        public float scroll_acceleration_seconds;
        public float scroll_start_speed;
        public bool scroll_start;
        public float zoom_time;
        public float default_rotate_radius;
        public float last_zoom_factor;
        public com.oddlabs.tt.util.Target rotation_point;
        public com.oddlabs.tt.@delegate.SelectionDelegate owner;
        public bool pitch_up;
        public bool pitch_down;
        public bool rotate_left;
        public bool rotate_right;
        // Java: 60
        public virtual void setOwner(com.oddlabs.tt.@delegate.SelectionDelegate owner)
        {
            this.owner = owner;
        }
        // Java: 64
        public virtual float getScrollX()
        {
            return scrolling_x;
        }
        // Java: 68
        public virtual float getScrollY()
        {
            return scrolling_y;
        }
        // Java: 72
        public virtual void resetLastZoomFactor()
        {
            last_zoom_factor = 0.0f;
        }
        // Java: 76
        public virtual float getLastZoomFactor()
        {
            return last_zoom_factor;
        }
        // Java: 80
        public virtual bool pitchUp()
        {
            return pitch_up;
        }
        // Java: 84
        public virtual bool pitchDown()
        {
            return pitch_down;
        }
        // Java: 88
        public virtual bool rotateRight()
        {
            return rotate_right;
        }
        // Java: 92
        public virtual bool rotateLeft()
        {
            return rotate_left;
        }
        // Java: 106
        public virtual void reset()
        {
        }
        // Java: 109
        public virtual void reset(float x, float y)
        {
            float dx = (float)(x - (float)(0.5f * getHeightMap().getMetersPerWorld()));
            float dy = (float)(y - (float)(0.5f * getHeightMap().getMetersPerWorld()));
            float r = ((float)(java.lang.Math.sqrt((float)((float)(dx * dx) + (float)(dy * dy)))));
            if (dy > 0)
            {
                getState().setCurrentHorizAngle(((float)((java.lang.Math.PI + java.lang.Math.acos((float)(dx / r))))));
            }
            else
            {
                getState().setCurrentHorizAngle((float)(-((float)((java.lang.Math.PI + java.lang.Math.acos((float)(dx / r)))))));
            }
            getState().setCurrentVertAngle((float)((float)((float)(-45.0f) * ((float)(java.lang.Math.PI))) / 180.0f));
            setPos(x, y);
            zoom_time = 0.0f;
            updateDirection();
        }
        // Java: 127
        public virtual void setPos(float x, float y)
        {
            float radius = ((float)(java.lang.Math.cos(getState().getTargetVertAngle())));
            float dir_x = (float)(((float)(java.lang.Math.cos(getState().getTargetHorizAngle()))) * radius);
            float dir_y = (float)(((float)(java.lang.Math.sin(getState().getTargetHorizAngle()))) * radius);
            float dir_z = ((float)(java.lang.Math.sin(getState().getTargetVertAngle())));
            getState().setCurrentX((float)(x - (float)(dir_x * com.oddlabs.tt.camera.GameCamera.INIT_DISTANCE)));
            getState().setCurrentY((float)(y - (float)(dir_y * com.oddlabs.tt.camera.GameCamera.INIT_DISTANCE)));
            getState().setCurrentZ((float)(getHeightMap().getNearestHeight(x, y) - (float)(dir_z * com.oddlabs.tt.camera.GameCamera.INIT_DISTANCE)));
            checkPosition();
        }
        // Java: 138
        public void updateDirection()
        {
            left_dir_x = (float)(-((float)(java.lang.Math.sin(getState().getTargetHorizAngle()))));
            left_dir_y = ((float)(java.lang.Math.cos(getState().getTargetHorizAngle())));
        }
        // Java: 143
        public void doZoom(float time_delta)
        {
            zoom((float)((float)((float)(zoom_time * time_delta) * com.oddlabs.tt.camera.GameCamera.ZOOM_SPEED) * getState().getTargetZ()));
            if (zoom_time < 0.0f)
            {
                zoom_time = java.lang.Math.min(0.0f, (float)(zoom_time + time_delta));
            }
            else if (zoom_time > 0.0f)
            {
                zoom_time = java.lang.Math.max(0.0f, (float)(zoom_time - time_delta));
            }
        }
        // Java: 151
        public virtual void zoom(float zoom_factor)
        {
            if (zoom_factor != 0.0f)
            {
                last_zoom_factor = zoom_factor;
                float radius = ((float)(java.lang.Math.cos(getState().getTargetVertAngle())));
                float dir_x = (float)(((float)(java.lang.Math.cos(getState().getTargetHorizAngle()))) * radius);
                float dir_y = (float)(((float)(java.lang.Math.sin(getState().getTargetHorizAngle()))) * radius);
                float dir_z = ((float)(java.lang.Math.sin(getState().getTargetVertAngle())));
                if (dir_z > com.oddlabs.tt.camera.GameCamera.ZOOM_Z_DIR_MIN)
                {
                    dir_z = com.oddlabs.tt.camera.GameCamera.ZOOM_Z_DIR_MIN;
                    float inv_length = (float)(1 / ((float)(java.lang.Math.sqrt((float)((float)((float)(dir_x * dir_x) + (float)(dir_y * dir_y)) + (float)(dir_z * dir_z))))));
                    dir_x = (float)(dir_x * inv_length);
                    dir_y = (float)(dir_y * inv_length);
                    dir_z = (float)(dir_z * inv_length);
                }
                float temp_x = (float)(getState().getTargetX() + (float)(dir_x * zoom_factor));
                float temp_y = (float)(getState().getTargetY() + (float)(dir_y * zoom_factor));
                float temp_z = (float)(getState().getTargetZ() + (float)(dir_z * zoom_factor));
                float min_z_level = (float)(getHeightMap().getSeaLevelMeters() + com.oddlabs.tt.camera.GameCamera.GROUND_CLEARANCE);
                temp_z = java.lang.Math.max(temp_z, min_z_level);
                float backup_x = getState().getTargetX();
                float backup_y = getState().getTargetY();
                float backup_z = getState().getTargetZ();
                int mid = getHeightMap().getMetersPerWorld() / 2;
                float dx = ((float)(temp_x - mid));
                float dy = ((float)(temp_y - mid));
                float squared_dist = (float)((float)(dx * dx) + (float)(dy * dy));
                if (squared_dist < getHeightMap().getMetersPerWorld() * getHeightMap().getMetersPerWorld() && temp_z < getMaxZ())
                {
                    getState().setTargetX(temp_x);
                    getState().setTargetY(temp_y);
                    getState().setTargetZ(temp_z);
                    if (bounce(getState().getTargetX(), getState().getTargetY(), getState().getTargetZ(), viewer.getGUIRoot().getWidth(), viewer.getGUIRoot().getHeight()))
                    {
                        getState().setTargetX(backup_x);
                        getState().setTargetY(backup_y);
                        getState().setTargetZ(backup_z);
                    }
                    checkPosition();
                }
            }
        }
        // Java: 195
        public void doScroll(float time_delta)
        {
            if (!viewer.getGUIRoot().getDelegate().canScroll())
            {
                return;
            }
            com.oddlabs.tt.input.InputManager inputManager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
            float scroll_speed = (float)(scroll_start_speed * ((float)(0.4f + (float)(((float)(scroll_acceleration_seconds / com.oddlabs.tt.camera.GameCamera.SCROLL_ACCELERATION_SECONDS_MAX)) * com.oddlabs.tt.camera.GameCamera.SCROLL_ACCELERATION_FACTOR))));
            float scroll_factor = (float)(time_delta * scroll_speed);
            bool blocked = viewer.getGUIRoot().getDelegate().keyboardBlocked();
            scrolling_x = (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT) && !inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT) && !blocked ? (float)(-1.0f) : (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT) && !inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT) && !blocked ? 1.0f : scroll_x));
            scrolling_y = (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN) && !inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP) && !blocked ? (float)(-1.0f) : (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP) && !inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN) && !blocked ? 1.0f : scroll_y));
            float new_x = (float)(getState().getTargetX() - (float)(((float)((float)(scrolling_x * left_dir_x) + (float)(scrolling_y * (float)(-left_dir_y)))) * scroll_factor));
            float new_y = (float)(getState().getTargetY() - (float)(((float)((float)(scrolling_x * left_dir_y) + (float)(scrolling_y * left_dir_x))) * scroll_factor));
            if (new_x != getState().getTargetX() || new_y != getState().getTargetY())
            {
                getState().setTargetX(new_x);
                getState().setTargetY(new_y);
                checkPosition();
            }
            scroll_acceleration_seconds = java.lang.Math.min((float)(scroll_acceleration_seconds + time_delta), com.oddlabs.tt.camera.GameCamera.SCROLL_ACCELERATION_SECONDS_MAX);
        }
        // Java: 223
        public void doPitch(float time_delta)
        {
            checkKeys();
            if (pitch_up != pitch_down)
            {
                float da = (pitch_up ? (float)(time_delta * com.oddlabs.tt.camera.GameCamera.ANGLE_DELTA) : (float)((float)(-time_delta) * com.oddlabs.tt.camera.GameCamera.ANGLE_DELTA));
                if (com.oddlabs.tt.global.Settings.getSettings().invert_camera_pitch)
                {
                    da = (float)(da * (-1));
                }
                getState().setTargetVertAngle((float)(getState().getTargetVertAngle() + da));
                checkPosition();
            }
        }
        // Java: 235
        public void doRotate(float time_delta)
        {
            checkKeys();
            if (rotate_left != rotate_right)
            {
                float da = (rotate_left ? (float)(time_delta * com.oddlabs.tt.camera.GameCamera.ANGLE_DELTA) : (float)((float)(-time_delta) * com.oddlabs.tt.camera.GameCamera.ANGLE_DELTA));
                if (com.oddlabs.tt.global.Settings.getSettings().invert_camera_yaw)
                {
                    da = (float)(da * (-1));
                }
                getState().setTargetHorizAngle((float)(getState().getTargetHorizAngle() + da));
            }
        }
        // Java: 246
        public virtual int getRotateY()
        {
            int center_y = viewer.getGUIRoot().getHeight() / 2;
            if (getState().getTargetVertAngle() < com.oddlabs.tt.camera.GameCamera.ROTATE_PICKING_ANGLE_MAX)
            {
                return center_y;
            }
            else
            {
                float da = (float)(getState().getTargetVertAngle() - com.oddlabs.tt.camera.GameCamera.ROTATE_PICKING_ANGLE_MAX);
                int dy = java.lang.JC.d2i((java.lang.Math.tan(da) * com.oddlabs.tt.global.Globals.VIEW_MIN));
                int y = center_y - dy;
                return y;
            }
        }
        // Java: 261
        public bool insideWorld(float x, float y)
        {
            return x > 0 && x < getHeightMap().getMetersPerWorld() && y > 0 && y < getHeightMap().getMetersPerWorld();
        }
        // Java: 265
        public override void doAnimate(float t)
        {
            doZoom(t);
            doScroll(t);
            doPitch(t);
            doRotate(t);
            updateDirection();
            getState().setFog(viewer.getWorld().getFog());
            getState().getFog().setEnabled(true);
        }
        // Java: 277
        public override void mouseScrolled(int amount)
        {
            zoom_time = java.lang.Math.clamp((float)(zoom_time + (float)(amount * 0.05f)), (float)(-0.15f), 0.15f);
        }
        // Java: 282
        public virtual void setRotationPoint(com.oddlabs.tt.util.Target target)
        {
            rotation_point = target;
        }
        // Java: 286
        public virtual float[] getRotationPoint()
        {
            float[] point = new float[2];
            if (rotation_point != null)
            {
                point[0] = rotation_point.getPositionX();
                point[1] = rotation_point.getPositionY();
            }
            else
            {
                point[0] = getState().getTargetX();
                point[1] = getState().getTargetY();
            }
            return point;
        }
        // Java: 298
        public override void mouseMoved(int x, int y)
        {
            int view_width = viewer.getGUIRoot().getWidth();
            int view_height = viewer.getGUIRoot().getHeight();
            if ((owner == null || !owner.isSelecting()) && (x < com.oddlabs.tt.camera.GameCamera.SCROLL_BUFFER || y < com.oddlabs.tt.camera.GameCamera.SCROLL_BUFFER || x > view_width - 1 - com.oddlabs.tt.camera.GameCamera.SCROLL_BUFFER || y > view_height - 1 - com.oddlabs.tt.camera.GameCamera.SCROLL_BUFFER))
            {
                if (scroll_start)
                {
                    scroll_start = false;
                    if (!scrollSpeedLocked(null))
                    {
                        scroll_acceleration_seconds = 0;
                        setScrollSpeed();
                    }
                }
                scroll_x = ((float)(x - (float)(view_width / 2.0f)));
                scroll_y = ((float)(y - (float)(view_height / 2.0f)));
                float inv_length = (float)(1.0f / ((float)(java.lang.Math.sqrt((float)((float)(scroll_x * scroll_x) + (float)(scroll_y * scroll_y))))));
                scroll_x *= inv_length;
                scroll_y *= inv_length;
            }
            else
            {
                scroll_start = true;
                scroll_x = 0;
                scroll_y = 0;
            }
        }
        // Java: 323
        public bool scrollSpeedLocked(com.oddlabs.tt.input.GameAction action)
        {
            com.oddlabs.tt.input.InputManager inputManager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
            return scroll_x != 0 || scroll_y != 0 || (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP) && ((object)action != (object)com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP)) || (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN) && ((object)action != (object)com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN)) || (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT) && ((object)action != (object)com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT)) || (inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT) && ((object)action != (object)com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT));
        }
        // Java: 333
        public void setScrollSpeed()
        {
            viewer.getPicker().pickRotate(this);
            float[] landscape_point = getRotationPoint();
            float landscape_z = getHeightMap().getNearestHeight(landscape_point[0], landscape_point[1]);
            float dx = (float)(landscape_point[0] - getState().getTargetX());
            float dy = (float)(landscape_point[1] - getState().getTargetY());
            float dz = (float)(landscape_z - getState().getTargetZ());
            scroll_start_speed = java.lang.Math.min(((float)(java.lang.Math.sqrt((float)((float)((float)(dx * dx) + (float)(dy * dy)) + (float)(dz * dz))))), com.oddlabs.tt.camera.GameCamera.SCROLL_START_MAX_SPEED);
        }
        // Java: 343
        public virtual com.oddlabs.tt.landscape.World getWorld()
        {
            return viewer.getWorld();
        }
        // Java: 347
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
            {
                bool handled = false;
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP))
                {
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN))
                {
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT))
                {
                    viewer.getPicker().pickRotate(this);
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT))
                {
                    viewer.getPicker().pickRotate(this);
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_IN))
                {
                    mouseScrolled((-2));
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_OUT))
                {
                    mouseScrolled(2);
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP))
                {
                    if (!scrollSpeedLocked(com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP))
                    {
                        scroll_acceleration_seconds = 0;
                        setScrollSpeed();
                    }
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN))
                {
                    if (!scrollSpeedLocked(com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN))
                    {
                        scroll_acceleration_seconds = 0;
                        setScrollSpeed();
                    }
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT))
                {
                    if (!scrollSpeedLocked(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT))
                    {
                        scroll_acceleration_seconds = 0;
                        setScrollSpeed();
                    }
                    handled = true;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT))
                {
                    if (!scrollSpeedLocked(com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT))
                    {
                        scroll_acceleration_seconds = 0;
                        setScrollSpeed();
                    }
                    handled = true;
                }
                if (handled)
                {
                    @event.consume();
                }
            }
        }
        // Java: 407
        public void checkKeys()
        {
            if (viewer.getGUIRoot().getDelegate().keyboardBlocked() || viewer.getGUIRoot().getModalDelegate() != null)
            {
                pitch_up = false;
                pitch_down = false;
                rotate_right = false;
                rotate_left = false;
                return;
            }
            com.oddlabs.tt.input.InputManager inputManager = com.oddlabs.tt.render.Renderer.getLocalInput().getInputManager();
            pitch_up = inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP);
            pitch_down = inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN);
            rotate_right = inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT);
            rotate_left = inputManager.isActive(com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT);
        }
        // Java: 423
        public override void enable()
        {
            base.enable();
            com.oddlabs.tt.gui.LocalInput localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
            float scale = viewer.getGUIRoot().getGlobalScale();
            mouseMoved(java.lang.Math.round((float)(localInput.getMouseX() / scale)), java.lang.Math.round((float)(localInput.getMouseY() / scale)));
        }
        static GameCamera()
        {
            GameCamera.ZOOM_Z_DIR_MIN = (float)(-((float)(java.lang.Math.tan(java.lang.Math.PI / 6))));
        }
        private void init_GameCamera()
        {
            this.rotation_point = null;
        }
        public GameCamera(com.oddlabs.tt.camera.CameraHost viewer, com.oddlabs.tt.camera.CameraState camera) : base(viewer.getWorld().getHeightMap(), camera)
        {
            init_GameCamera();
            this.default_rotate_radius = (float)(viewer.getWorld().getHeightMap().getMetersPerWorld() / 4.0f);
            this.viewer = viewer;
            checkPosition();
            updateDirection();
        }
    }
}

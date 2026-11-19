// Translated from MapCamera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 23
    public class MapCamera : com.oddlabs.tt.camera.Camera
    {
        public const float MAP_THRESHOLD = 0.1f;
        public const float MAP_TIME_FACTOR = 1000.0f;
        public const float SMOOTHNESS_FACTOR = 200.0f;
        public const float MAP_Z_FACTOR = 1.3f;
        public const float LANDING_ANGLE = (-0.7853982f);
        public const float MIN_LANDING_DISTANCE = 20.0f;
        public static java.util.Set BLOCKED_ACTIONS;
        // Java: 46
        public sealed class MapMode : java.lang.Enum
        {
            public static MapMode[] values() { return (MapMode[])VALUES_.Clone(); }
            public static MapMode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly MapMode TO_MAP = new MapMode("TO_MAP", 0);
            public static readonly MapMode IN_MAP = new MapMode("IN_MAP", 1);
            public static readonly MapMode FROM_MAP = new MapMode("FROM_MAP", 2);
            public static readonly MapMode[] VALUES_ = new MapMode[] { TO_MAP, IN_MAP, FROM_MAP };
            public MapMode(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.camera.MapCameraOwner @delegate;
        public com.oddlabs.tt.camera.CameraState original_camera_state;
        public com.oddlabs.tt.gui.Label label;
        public com.oddlabs.tt.camera.MapCamera.MapMode map_mode;
        public float fogTime;
        // Java: 69
        public static com.oddlabs.tt.camera.CameraState mapState(com.oddlabs.tt.camera.GameCamera old_camera)
        {
            com.oddlabs.tt.resource.FogInfo radialFog = new com.oddlabs.tt.resource.RadialFogInfo(com.oddlabs.util.Color.WHITE, 0.25f);
            com.oddlabs.tt.camera.CameraState mapCameraState = new com.oddlabs.tt.camera.CameraState(radialFog);
            mapCameraState.set(old_camera.getState());
            mapCameraState.setFog(radialFog);
            return mapCameraState;
        }
        // Java: 77
        public override void doAnimate(float t)
        {
            float factor = (float)((float)(t * 1000.0f) / java.lang.Math.max((float)(t * 1000.0f), (float)(com.oddlabs.tt.global.Settings.getSettings().mapmode_delay * com.oddlabs.tt.camera.MapCamera.MAP_TIME_FACTOR)));
            float dx = default;
            float dy = default;
            float dz = default;
            float da = default;
            float map_x = (float)(getHeightMap().getMetersPerWorld() / 2.0f);
            float map_y = (float)(getHeightMap().getMetersPerWorld() / 2.0f);
            float map_z = (float)(getHeightMap().getMetersPerWorld() * com.oddlabs.tt.camera.MapCamera.MAP_Z_FACTOR);
            float start_z = original_camera_state.getTargetZ();
            float current_z = getState().getTargetZ();
            float total_dist = (float)(map_z - start_z);
            float progress = (java.lang.Math.abs(total_dist) > 0.001f ? java.lang.Math.clamp((float)(((float)(current_z - start_z)) / total_dist), 0.0f, 1.0f) : 1.0f);
            fogTime += t;
            float pulse = ((float)((java.lang.Math.sin((float)(fogTime * 0.4125f)) * 0.5d + java.lang.Math.sin((float)(fogTime * 0.8625f)) * 0.3d + java.lang.Math.sin((float)(fogTime * 1.7625f)) * 0.2d)));
            float baseDensity = (float)(0.3f + (float)(pulse * 0.12f));
            if (progress < 0.25f)
            {
                getState().setFog(original_camera_state.getFog());
            }
            else
            {
                float fade = (float)(((float)(progress - 0.25f)) / 0.75f);
                float radiusScale = (float)(1.5f - ((float)(0.5f * fade)));
                getState().setFog(new com.oddlabs.tt.resource.RadialFogInfo(com.oddlabs.util.Color.WHITE, (float)(baseDensity * fade), radiusScale));
            }
            getState().getFog().setEnabled(false);
            switch ((map_mode).ordinal())
            {
                case 0 /*TO_MAP*/:
                {
                    dx = (float)(map_x - original_camera_state.getTargetX());
                    dy = (float)(map_y - original_camera_state.getTargetY());
                    dz = (float)(map_z - original_camera_state.getTargetZ());
                    com.oddlabs.tt.camera.CameraState state = getState();
                    state.setTargetX((float)(getState().getTargetX() + (float)(dx * factor)));
                    state.setTargetY((float)(getState().getTargetY() + (float)(dy * factor)));
                    state.setTargetZ((float)(getState().getTargetZ() + (float)(dz * factor)));
                    if (state.getTargetZ() > (float)(map_z - com.oddlabs.tt.camera.MapCamera.MAP_THRESHOLD))
                    {
                        state.setTargetX(map_x);
                        state.setTargetY(map_y);
                        state.setTargetZ(map_z);
                        changeMode(com.oddlabs.tt.camera.MapCamera.MapMode.IN_MAP);
                    }
                    da = (float)(com.oddlabs.tt.camera.CameraState.MIN_ANGLE - original_camera_state.getTargetVertAngle());
                    state.setTargetVertAngle((float)(state.getTargetVertAngle() + (float)(da * factor)));
                    break;
                }
                case 1 /*IN_MAP*/:
                {
                    break;
                }
                case 2 /*FROM_MAP*/:
                {
                    dx = (float)(original_camera_state.getTargetX() - map_x);
                    dy = (float)(original_camera_state.getTargetY() - map_y);
                    dz = (float)(original_camera_state.getTargetZ() - map_z);
                    com.oddlabs.tt.camera.CameraState camera_state = getState();
                    camera_state.setTargetX((float)(getState().getTargetX() + (float)(dx * factor)));
                    camera_state.setTargetY((float)(getState().getTargetY() + (float)(dy * factor)));
                    camera_state.setTargetZ((float)(getState().getTargetZ() + (float)(dz * factor)));
                    if (camera_state.getTargetZ() <= original_camera_state.getTargetZ())
                    {
                        camera_state.setTargetX(original_camera_state.getTargetX());
                        camera_state.setTargetY(original_camera_state.getTargetY());
                        camera_state.setTargetZ(original_camera_state.getTargetZ());
                        camera_state.setTargetVertAngle(original_camera_state.getTargetVertAngle());
                        checkPosition();
                        getState().setNoDetailMode(false);
                        @delegate.exitMapMode();
                        break;
                    }
                    da = (float)(original_camera_state.getTargetVertAngle() - com.oddlabs.tt.camera.CameraState.MIN_ANGLE);
                    camera_state.setTargetVertAngle((float)(getState().getTargetVertAngle() + (float)(da * factor)));
                    break;
                }
            }
        }
        // Java: 160
        public void changeMode(com.oddlabs.tt.camera.MapCamera.MapMode mode)
        {
            map_mode = mode;
            switch ((mode).ordinal())
            {
                case 0 /*TO_MAP*/:
                {
                    break;
                }
                case 1 /*IN_MAP*/:
                {
                    label.setPos((@delegate.getGUIRoot().getWidth() - label.getWidth()) / 2, @delegate.getGUIRoot().getHeight() - label.getHeight());
                    @delegate.addChild(label);
                    break;
                }
                case 2 /*FROM_MAP*/:
                {
                    label.remove();
                    break;
                }
            }
        }
        // Java: 176
        public void mapGoto(float x, float y)
        {
            this.mapGoto(x, y, false);
        }
        // Java: 180
        public void mapGoto(float x, float y, bool @override)
        {
            if (((object)map_mode == (object)com.oddlabs.tt.camera.MapCamera.MapMode.IN_MAP) || @override)
            {
                float clickGroundZ = getHeightMap().getNearestHeight(x, y);
                float sinDown = (float)(-((float)(java.lang.Math.sin(com.oddlabs.tt.camera.MapCamera.LANDING_ANGLE))));
                float minEyeZ = (float)(clickGroundZ + (float)(com.oddlabs.tt.camera.MapCamera.MIN_LANDING_DISTANCE * sinDown));
                float landingEyeZ = java.lang.Math.min(java.lang.Math.max(minEyeZ, original_camera_state.getTargetZ()), @delegate.getMaxCameraZ());
                float landing_distance = java.lang.Math.max(com.oddlabs.tt.camera.MapCamera.MIN_LANDING_DISTANCE, (float)(((float)(landingEyeZ - clickGroundZ)) / sinDown));
                float radius = ((float)(java.lang.Math.cos(com.oddlabs.tt.camera.MapCamera.LANDING_ANGLE)));
                float old_dir_x = (float)(((float)(java.lang.Math.cos(getState().getHorizAngle()))) * radius);
                float old_dir_y = (float)(((float)(java.lang.Math.sin(getState().getHorizAngle()))) * radius);
                original_camera_state.setTargetX((float)(x - (float)(old_dir_x * landing_distance)));
                original_camera_state.setTargetY((float)(y - (float)(old_dir_y * landing_distance)));
                original_camera_state.setTargetZ(landingEyeZ);
                changeMode(com.oddlabs.tt.camera.MapCamera.MapMode.FROM_MAP);
            }
        }
        // Java: 206
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            foreach (com.oddlabs.tt.input.GameAction action in java.lang.JIter.of(com.oddlabs.tt.camera.MapCamera.BLOCKED_ACTIONS))
            {
                if (@event.consumeAction(action))
                {
                    @event.consume();
                    return;
                }
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.CAMERA_MAP_MODE))
                {
                    changeMode((((object)map_mode == (object)com.oddlabs.tt.camera.MapCamera.MapMode.TO_MAP) || ((object)map_mode == (object)com.oddlabs.tt.camera.MapCamera.MapMode.IN_MAP) ? com.oddlabs.tt.camera.MapCamera.MapMode.FROM_MAP : com.oddlabs.tt.camera.MapCamera.MapMode.TO_MAP));
                    @event.consume();
                }
            }
        }
        static MapCamera()
        {
            MapCamera.BLOCKED_ACTIONS = java.util.EnumSet.of(com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT, com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT, com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP, com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN, com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP, com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN, com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT, com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT, com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_IN, com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_OUT, com.oddlabs.tt.input.GameAction.CAMERA_FIRST_PERSON, com.oddlabs.tt.input.GameAction.CAMERA_ZOOM_MODE);
        }
        private void init_MapCamera()
        {
            this.label = new com.oddlabs.tt.gui.Label(com.oddlabs.tt.util.Utils.getBundleString(java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.camera.MapCamera).getName()), "map_mode"), com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont());
            this.map_mode = com.oddlabs.tt.camera.MapCamera.MapMode.TO_MAP;
            this.fogTime = 0.0f;
        }
        public MapCamera(com.oddlabs.tt.camera.MapCameraOwner @delegate, com.oddlabs.tt.camera.GameCamera old_camera) : base(old_camera.getHeightMap(), com.oddlabs.tt.camera.MapCamera.mapState(old_camera))
        {
            init_MapCamera();
            original_camera_state = old_camera.getState();
            this.@delegate = @delegate;
            setSmoothnessFactor(com.oddlabs.tt.camera.MapCamera.SMOOTHNESS_FACTOR);
            getState().setNoDetailMode(true);
        }
    }
}

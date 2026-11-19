// Translated from FirstPersonCamera.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 13
    public class FirstPersonCamera : com.oddlabs.tt.camera.Camera
    {
        public const float MOUSE_LOOK_SPEED = 0.002f;
        public const float KEY_LOOK_SPEED = 1.5707964f;
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public com.oddlabs.tt.gui.LocalInput localInput;
        public int physicalStartX;
        public int physicalStartY;
        // Java: 34
        public override void doAnimate(float dt)
        {
            updateKeyboardLook(dt);
            updateKeyboardPan(dt);
        }
        // Java: 40
        public override void mouseMoved(int x, int y)
        {
            int dx = localInput.getMouseX() - physicalStartX;
            int dy = localInput.getMouseY() - physicalStartY;
            if (dx == 0 && dy == 0)
            {
                return;
            }
            yaw((float)(-dx * com.oddlabs.tt.camera.FirstPersonCamera.MOUSE_LOOK_SPEED));
            pitch((float)(dy * com.oddlabs.tt.camera.FirstPersonCamera.MOUSE_LOOK_SPEED));
            localInput.getPointerInput().setCursorPosition(physicalStartX, physicalStartY);
        }
        // Java: 60
        public void yaw(float radians)
        {
            if (com.oddlabs.tt.global.Settings.getSettings().invert_camera_yaw)
            {
                radians = (float)(radians * (-1));
            }
            getState().setTargetHorizAngle((float)(getState().getTargetHorizAngle() + radians));
        }
        // Java: 68
        public void pitch(float radians)
        {
            if (com.oddlabs.tt.global.Settings.getSettings().invert_camera_pitch)
            {
                radians = (float)(radians * (-1));
            }
            getState().setTargetVertAngle((float)(getState().getTargetVertAngle() + radians));
        }
        // Java: 75
        public void updateKeyboardLook(float dt)
        {
            if (isKeyboardBlocked())
            {
                return;
            }
            com.oddlabs.tt.input.InputManager inputManager = localInput.getInputManager();
            float amount = (float)((float)(dt * com.oddlabs.tt.camera.FirstPersonCamera.KEY_LOOK_SPEED) * com.oddlabs.tt.camera.GameCamera.rotateSpeedFactor());
            yaw((float)(amount * com.oddlabs.tt.camera.FirstPersonCamera.axis(inputManager, com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_LEFT, com.oddlabs.tt.input.GameAction.CAMERA_ROTATE_RIGHT)));
            pitch((float)(amount * com.oddlabs.tt.camera.FirstPersonCamera.axis(inputManager, com.oddlabs.tt.input.GameAction.CAMERA_PITCH_UP, com.oddlabs.tt.input.GameAction.CAMERA_PITCH_DOWN)));
        }
        // Java: 87
        public void updateKeyboardPan(float dt)
        {
            if (isKeyboardBlocked())
            {
                return;
            }
            com.oddlabs.tt.input.InputManager inputManager = localInput.getInputManager();
            int moveRight = com.oddlabs.tt.camera.FirstPersonCamera.axis(inputManager, com.oddlabs.tt.input.GameAction.CAMERA_PAN_RIGHT, com.oddlabs.tt.input.GameAction.CAMERA_PAN_LEFT);
            int moveForward = com.oddlabs.tt.camera.FirstPersonCamera.axis(inputManager, com.oddlabs.tt.input.GameAction.CAMERA_PAN_UP, com.oddlabs.tt.input.GameAction.CAMERA_PAN_DOWN);
            if (moveRight == 0 && moveForward == 0)
            {
                return;
            }
            float heading = getState().getTargetHorizAngle();
            float forwardX = ((float)(java.lang.Math.cos(heading)));
            float forwardY = ((float)(java.lang.Math.sin(heading)));
            float rightX = forwardY;
            float rightY = (float)(-forwardX);
            float distance = (float)((float)(getState().getTargetZ() * dt) * com.oddlabs.tt.camera.GameCamera.panSpeedFactor());
            getState().setTargetX((float)(getState().getTargetX() + (float)(((float)((float)(moveForward * forwardX) + (float)(moveRight * rightX))) * distance)));
            getState().setTargetY((float)(getState().getTargetY() + (float)(((float)((float)(moveForward * forwardY) + (float)(moveRight * rightY))) * distance)));
            checkPosition();
        }
        // Java: 112
        public bool isKeyboardBlocked()
        {
            return viewer.getGUIRoot().getDelegate().keyboardBlocked() || viewer.getGUIRoot().getModalDelegate() != null;
        }
        // Java: 117
        public static int axis(com.oddlabs.tt.input.InputManager inputManager, com.oddlabs.tt.input.GameAction positive, com.oddlabs.tt.input.GameAction negative)
        {
            int pos = (inputManager.isActive(positive) ? 1 : 0);
            int neg = (inputManager.isActive(negative) ? 1 : 0);
            return pos - neg;
        }
        private void init_FirstPersonCamera()
        {
            this.localInput = com.oddlabs.tt.render.Renderer.getLocalInput();
        }
        public FirstPersonCamera(com.oddlabs.tt.viewer.WorldViewer viewer, com.oddlabs.tt.landscape.HeightMap heightmap, com.oddlabs.tt.camera.CameraState camera) : base(heightmap, camera)
        {
            init_FirstPersonCamera();
            this.viewer = viewer;
            physicalStartX = localInput.getMouseX();
            physicalStartY = localInput.getMouseY();
        }
    }
}

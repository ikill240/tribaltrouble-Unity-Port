using UnityEngine;

namespace TribalTrouble.Game
{
    /// <summary>RTS camera: WASD / arrows / screen-edge pan, wheel zoom, Q/E or middle-drag rotate.</summary>
    public sealed class RtsCamera : MonoBehaviour
    {
        public GameWorld world;
        public Vector3 focus;
        public float distance = 60f;
        public float yaw = 0f;
        public float minDistance = 12f, maxDistance = 220f;
        public bool edgePan = true;
        [Tooltip("WASD pan and Q/E rotate. Off in the faithful game, where those letters are the original unit hotkeys.")]
        public bool letterKeys = true;
        /// <summary>Used instead of <see cref="world"/> when set (faithful game): height at (x, z) metres.</summary>
        public System.Func<float, float, float> heightAt;
        public float mapMeters, seaLevel;
        /// <summary>Map mode (Java MapCamera, Space): look straight down on the whole island.</summary>
        public bool mapMode;
        /// <summary>Extra pitch (degrees) from Home/End or numpad 8/2 and from mouse look (F or middle mouse).</summary>
        public float pitchOffset;
        /// <summary>Ctrl / Shift + wheel belong to someone else (the map editor's radius and intensity).</summary>
        public bool reserveModifierWheel;
        /// <summary>Keyboard and wheel input go to a text field / dialog this frame.</summary>
        public bool inputBlocked;

        // What the player did with the camera this frame (the Java GameCamera's state; the tutorial watches it).
        public float scrollX, scrollY, zoomDelta;
        public bool rotatingLeft, rotatingRight, pitchingUp, pitchingDown, mouseLook;

        float Meters => world != null ? world.meters : mapMeters;
        float Sea => world != null ? world.seaLevel : seaLevel;
        float H(float x, float z) => world != null ? world.HeightAt(x, z) : heightAt != null ? heightAt(x, z) : 0f;

        public void LookAt(Vector2 meters)
        {
            focus = new Vector3(meters.x, 0, meters.y);
        }

        void LateUpdate()
        {
            if (world == null && heightAt == null) return;
            float dt = Time.unscaledDeltaTime;
            if (mapMode)
            {
                float m = Meters;
                var cam = GetComponent<Camera>();
                float fov = cam != null ? cam.fieldOfView : 60f;
                float h = m * 0.5f / Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad) * 1.08f;
                transform.SetPositionAndRotation(new Vector3(m * 0.5f, Sea + h, m * 0.5f), Quaternion.Euler(90f, yaw, 0f));
                return;
            }
            scrollX = scrollY = zoomDelta = 0f;
            rotatingLeft = rotatingRight = pitchingUp = pitchingDown = mouseLook = false;
            float speed = distance * 1.2f * GameSettings.scrollSpeed;
            Vector3 fwd = Quaternion.Euler(0, yaw, 0) * Vector3.forward;
            Vector3 right = Quaternion.Euler(0, yaw, 0) * Vector3.right;
            Vector3 move = Vector3.zero;
            bool alt = Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
            if (!alt && !inputBlocked)
            {
                if ((letterKeys && Input.GetKey(KeyCode.W)) || Input.GetKey(KeyCode.UpArrow)) move += fwd;
                if ((letterKeys && Input.GetKey(KeyCode.S)) || Input.GetKey(KeyCode.DownArrow)) move -= fwd;
                if ((letterKeys && Input.GetKey(KeyCode.D)) || Input.GetKey(KeyCode.RightArrow)) move += right;
                if ((letterKeys && Input.GetKey(KeyCode.A)) || Input.GetKey(KeyCode.LeftArrow)) move -= right;
            }
            if (edgePan && GameSettings.edgePan && Application.isFocused)
            {
                var mp = Input.mousePosition;
                const float edge = 6f;
                if (mp.x <= edge) move -= right;
                if (mp.x >= Screen.width - edge) move += right;
                if (mp.y <= edge) move -= fwd;
                if (mp.y >= Screen.height - edge) move += fwd;
            }
            focus += move * speed * dt;
            scrollX = Vector3.Dot(move, right);
            scrollY = Vector3.Dot(move, fwd);
            if ((letterKeys && Input.GetKey(KeyCode.Q)) || Input.GetKey(KeyCode.Delete) || Input.GetKey(KeyCode.Keypad4) || (alt && Input.GetKey(KeyCode.LeftArrow))) { yaw -= (GameSettings.invertYaw ? -90f : 90f) * dt; rotatingLeft = true; }
            if ((letterKeys && Input.GetKey(KeyCode.E)) || Input.GetKey(KeyCode.Insert) || Input.GetKey(KeyCode.Keypad6) || (alt && Input.GetKey(KeyCode.RightArrow))) { yaw += (GameSettings.invertYaw ? -90f : 90f) * dt; rotatingRight = true; }
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Keypad8)) { pitchOffset = Mathf.Max(-30f, pitchOffset - 40f * dt); pitchingUp = true; }
            if (Input.GetKey(KeyCode.End) || Input.GetKey(KeyCode.Keypad2)) { pitchOffset = Mathf.Min(25f, pitchOffset + 40f * dt); pitchingDown = true; }
            if (Input.GetKey(KeyCode.PageUp) || Input.GetKey(KeyCode.Keypad9)) { distance = Mathf.Max(minDistance, distance * (1f - dt)); zoomDelta = 1f; }
            if (Input.GetKey(KeyCode.PageDown) || Input.GetKey(KeyCode.Keypad3)) { distance = Mathf.Min(maxDistance, distance * (1f + dt)); zoomDelta = -1f; }
            // mouse look (Java FirstPersonDelegate): hold F or the middle mouse button and move the mouse
            if (Input.GetMouseButton(2) || (!letterKeys && Input.GetKey(KeyCode.F) && !Input.GetKey(KeyCode.LeftControl)))
            {
                mouseLook = true;
                float mx = Input.GetAxis("Mouse X"), my = Input.GetAxis("Mouse Y") * (GameSettings.invertMouseLook ? -1f : 1f);
                yaw += mx * 3f * (GameSettings.invertYaw ? -1f : 1f);
                pitchOffset = Mathf.Clamp(pitchOffset - my * 2f, -30f, 25f);
                if (mx < -0.01f) rotatingLeft = true; else if (mx > 0.01f) rotatingRight = true;
            }
            float wheel = Input.mouseScrollDelta.y;
            if (reserveModifierWheel && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) wheel = 0f;
            if (inputBlocked) wheel = 0f;
            if (Mathf.Abs(wheel) > 0.01f) { distance = Mathf.Clamp(distance * (1f - wheel * 0.12f), minDistance, maxDistance); zoomDelta = wheel; }

            focus.x = Mathf.Clamp(focus.x, 0, Meters);
            focus.z = Mathf.Clamp(focus.z, 0, Meters);
            focus.y = Mathf.Max(H(focus.x, focus.z), Sea);
            float pitch = Mathf.Clamp(Mathf.Lerp(40f, 62f, Mathf.InverseLerp(minDistance, maxDistance, distance)) + pitchOffset, 8f, 89f);
            var rot = Quaternion.Euler(pitch, yaw, 0);
            var pos = focus - rot * Vector3.forward * distance;
            pos.y = Mathf.Max(pos.y, H(pos.x, pos.z) + 3f);
            transform.SetPositionAndRotation(pos, rot);
        }
    }
}

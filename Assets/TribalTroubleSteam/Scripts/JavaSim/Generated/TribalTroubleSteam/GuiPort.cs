// The Java GUI running on an engine that is not LWJGL: the engine (Unity) implements IGuiBackend; the translated
// GUI talks to it through the boundary classes (Renderer, GUIRenderer, Texture, FontFile, the window and the
// input provider below). Engine-free, so the headless tests run the GUI too (with the null backend).
#pragma warning disable
using System;
using System.Collections.Generic;

namespace TribalTroubleSteam.GuiPort
{
    /// <summary>One GUI quad in GUI pixels (y up, origin bottom left), corners counter-clockwise from bottom left.</summary>
    public struct GuiQuad
    {
        public int tex;                 // texture handle, or -1 for a plain coloured quad
        public float x0, y0, x1, y1, x2, y2, x3, y3;
        public float u1, v1, u2, v2;    // OpenGL texture coordinates (v up)
        public int abgr;                // colour, 0xAABBGGRR
    }

    public interface IGuiBackend
    {
        /// <summary>Window size in pixels.</summary>
        int Width { get; }
        int Height { get; }
        bool Active { get; }
        bool Fullscreen { get; }
        /// <summary>Loads "/textures/gui/mainmenu" style resources. Returns a handle (>= 0) or -1.</summary>
        int LoadTexture(string path, out int width, out int height);
        /// <summary>Creates a texture from 0xAABBGGRR pixels, row 0 at the bottom.</summary>
        int CreateTexture(int width, int height, int[] abgr);
        void ReleaseTexture(int handle);
        /// <summary>The GUI of one frame, drawn over the world.</summary>
        void DrawGui(float guiWidth, float guiHeight, List<GuiQuad> quads);
        void SetMusic(string path, float delay);
        void ToggleFullscreen();
        /// <summary>Cursor image from /textures/gui (null = system arrow), visibility, grab.</summary>
        void SetCursor(string image, int hotX, int hotY);
        void SetGrabbed(bool grabbed);
        void SetCursorPosition(int x, int y);
        void Quit();
    }

    /// <summary>No engine (headless tests): an 800x600 window that draws nothing.</summary>
    public sealed class NullBackend : IGuiBackend
    {
        int _next;
        public int W = 1920, H = 1080;
        public int Width => W;
        public int Height => H;
        public bool Active => true;
        public bool Fullscreen => false;
        public int LoadTexture(string path, out int width, out int height) { width = 256; height = 256; return _next++; }
        public int CreateTexture(int width, int height, int[] abgr) => _next++;
        public void ReleaseTexture(int handle) { }
        public void DrawGui(float w, float h, List<GuiQuad> quads) { LastQuadCount = quads.Count; }
        public int LastQuadCount;
        public void SetMusic(string path, float delay) { }
        public void ToggleFullscreen() { }
        public void SetCursor(string image, int hotX, int hotY) { }
        public void SetGrabbed(bool grabbed) { }
        public void SetCursorPosition(int x, int y) { }
        public void Quit() { }
    }

    public static class Host
    {
        public static IGuiBackend Backend = new NullBackend();
        public static readonly HostInput Input = new HostInput();
        public static readonly HostWindow Window = new HostWindow();
        /// <summary>The world the GUI currently shows (set by DefaultRenderer.render), for the engine to draw.</summary>
        public static com.oddlabs.tt.render.DefaultRenderer CurrentWorld;
        public static com.oddlabs.tt.camera.CameraState CurrentCamera;
        public static bool Finished;
        public static float Fps = 60f;
        /// <summary>Frames the engine has drawn (Renderer.Frame counts them).</summary>
        public static int FrameCount;
    }

    /// <summary>GLFW-style input queues the engine fills (codes and modifier bits as GLFW; y measured from the
    /// bottom of the window), read by the Java KeyboardInput / PointerInput exactly like LWJGL3InputProvider.</summary>
    public sealed class HostInput : java.lang.Object, com.oddlabs.tt.input.InputProvider
    {
        const int PRESS = 1, RELEASE = 0, REPEAT = 2;
        sealed class KeyEvent { public int key, action, mods; public char character; }
        struct MouseEvent { public int button; public bool state; public int x, y, dWheel; }

        readonly Queue<KeyEvent> _keys = new Queue<KeyEvent>();
        readonly List<KeyEvent> _keyList = new List<KeyEvent>();
        KeyEvent _curKey;
        readonly Queue<MouseEvent> _mouse = new Queue<MouseEvent>();
        MouseEvent? _curMouse;
        int _mouseX, _mouseY;
        readonly HashSet<int> _down = new HashSet<int>();
        readonly bool[] _buttons = new bool[8];
        bool _grabbed;
        readonly object _lock = new object();

        // ---- engine side
        public void KeyEventIn(int glfwKey, bool down, bool repeat, int mods)
        {
            lock (_lock)
            {
                if (down) _down.Add(glfwKey); else _down.Remove(glfwKey);
                var e = new KeyEvent { key = glfwKey, action = !down ? RELEASE : repeat ? REPEAT : PRESS, mods = mods };
                _keys.Enqueue(e); _keyList.Add(e); if (_keyList.Count > 64) _keyList.RemoveAt(0);
            }
        }

        /// <summary>A typed character: attached to the last press like GLFW's char callback.</summary>
        public void CharIn(char c)
        {
            lock (_lock)
            {
                if (_keyList.Count > 0)
                {
                    var last = _keyList[_keyList.Count - 1];
                    if ((last.action == PRESS || last.action == REPEAT) && last.character == 0 && _keys.Contains(last)) { last.character = c; return; }
                }
                var e = new KeyEvent { key = 0, action = PRESS, mods = 0, character = c };
                _keys.Enqueue(e); _keyList.Add(e);
            }
        }

        public void MouseMoveIn(int x, int y)
        {
            lock (_lock)
            {
                if (x == _mouseX && y == _mouseY) return;
                _mouseX = x; _mouseY = y;
                _mouse.Enqueue(new MouseEvent { button = -1, x = x, y = y });
            }
        }

        public void MouseButtonIn(int button, bool down)
        {
            lock (_lock)
            {
                if (button >= 0 && button < _buttons.Length) _buttons[button] = down;
                _mouse.Enqueue(new MouseEvent { button = button, state = down, x = _mouseX, y = _mouseY });
            }
        }

        public void WheelIn(float yoffset)
        {
            lock (_lock) _mouse.Enqueue(new MouseEvent { button = -1, x = _mouseX, y = _mouseY, dWheel = (int)(yoffset * 120) });
        }

        public void ReleaseAll()
        {
            lock (_lock)
            {
                foreach (var k in new List<int>(_down)) KeyEventIn(k, false, false, 0);
                for (int b = 0; b < _buttons.Length; b++) if (_buttons[b]) MouseButtonIn(b, false);
            }
        }

        public bool Grabbed => _grabbed;

        // ---- InputProvider (Java side)
        public void pollKeyboard() { }
        public bool nextKeyboardEvent() { lock (_lock) { if (_keys.Count == 0) return false; _curKey = _keys.Dequeue(); return true; } }
        public int getEventKey() => _curKey != null ? _curKey.key : 0;
        public bool getEventKeyState() => _curKey != null && _curKey.action != RELEASE;
        public int getEventKeyMods() => _curKey != null ? _curKey.mods : 0;
        public char getEventCharacter() => _curKey != null ? _curKey.character : (char)0;
        public bool isRepeatEvent() => _curKey != null && _curKey.action == REPEAT;
        public bool isKeyDown(int keyCode) { lock (_lock) return _down.Contains(keyCode); }
        public void pollMouse() { }
        public bool nextMouseEvent() { lock (_lock) { if (_mouse.Count == 0) return false; _curMouse = _mouse.Dequeue(); return true; } }
        public int getEventButton() => _curMouse.HasValue ? _curMouse.Value.button : -1;
        public bool getEventButtonState() => _curMouse.HasValue && _curMouse.Value.state;
        public int getEventDWheel() => _curMouse.HasValue ? _curMouse.Value.dWheel : 0;
        public int getEventX() => _curMouse.HasValue ? _curMouse.Value.x : _mouseX;
        public int getEventY() => _curMouse.HasValue ? _curMouse.Value.y : _mouseY;
        public int getMouseX() => _mouseX;
        public int getMouseY() => _mouseY;
        public bool isButtonDown(int button) => button >= 0 && button < _buttons.Length && _buttons[button];
        public void setCursorPosition(int x, int y) { _mouseX = x; _mouseY = y; Host.Backend.SetCursorPosition(x, y); }
        public void setGrabbed(bool grabbed) { _grabbed = grabbed; Host.Backend.SetGrabbed(grabbed); }
        public bool isGrabbed() => _grabbed;
        public bool isCursorInWindow() => true;
        public void setNativeCursor(object cursor)
        {
            var c = cursor is long id ? com.oddlabs.tt.gui.Cursor.ById(id) : cursor as com.oddlabs.tt.gui.Cursor;
            Host.Backend.SetCursor(c?.image, c?.hotX ?? 0, c?.hotY ?? 0);
        }
        public void close() { }
    }

    /// <summary>Steam port: the Steam game has no map editor, but the game's own height edits come here too (GroundEdited).
    /// What the Java map editor hands the engine instead of drawing it with OpenGL: changed heights and
    /// ground alphas (GroundTextures), the brush's dots (BrushRenderer) and the access tint (AccessOverlay). Written
    /// by the editor (the world load runs on a worker), read by the engine on its main thread.</summary>
    public static class EditorView
    {
        public struct Dot { public float x, y, r, g, b, a; }
        public struct Alphas { public int x0, y0, x1, y1; public byte[] low, high; }

        static readonly object s_lock = new object();
        static List<Dot> s_building = new List<Dot>(), s_dots = new List<Dot>();
        static readonly Queue<Alphas> s_alphas = new Queue<Alphas>();
        static int s_hx0 = int.MaxValue, s_hy0 = int.MaxValue, s_hx1 = int.MinValue, s_hy1 = int.MinValue;
        static int s_dotsFrame;

        /// <summary>The access tint: shown or not, its colours (ABGR, row 0 at y 0) and a version that changes with them.</summary>
        public static volatile bool AccessVisible;
        public static int[] AccessColors;
        public static int AccessSize, AccessVersion;

        public static void BeginDots() { lock (s_lock) s_building.Clear(); }
        public static void AddDot(float x, float y, float r, float g, float b, float a) { lock (s_lock) s_building.Add(new Dot { x = x, y = y, r = r, g = g, b = b, a = a }); }
        public static void EndDots() { lock (s_lock) { var t = s_dots; s_dots = s_building; s_building = t; s_dotsFrame = Host.FrameCount; } }
        /// <summary>The dots of the last finished batch, if it was drawn in the last couple of frames.</summary>
        public static List<Dot> Dots { get { lock (s_lock) return Host.FrameCount - s_dotsFrame <= 2 ? s_dots : null; } }

        public static void HeightsChanged(int x0, int y0, int x1, int y1)
        {
            lock (s_lock) { s_hx0 = Math.Min(s_hx0, x0); s_hy0 = Math.Min(s_hy0, y0); s_hx1 = Math.Max(s_hx1, x1); s_hy1 = Math.Max(s_hy1, y1); }
        }
        public static bool TakeHeights(out int x0, out int y0, out int x1, out int y1)
        {
            lock (s_lock)
            {
                x0 = s_hx0; y0 = s_hy0; x1 = s_hx1; y1 = s_hy1;
                s_hx0 = s_hy0 = int.MaxValue; s_hx1 = s_hy1 = int.MinValue;
                return x0 <= x1;
            }
        }
        public static void AlphasChanged(int x0, int y0, int x1, int y1, byte[] low, byte[] high) { lock (s_lock) s_alphas.Enqueue(new Alphas { x0 = x0, y0 = y0, x1 = x1, y1 = y1, low = low, high = high }); }
        public static bool TakeAlphas(out Alphas a) { lock (s_lock) { if (s_alphas.Count == 0) { a = default; return false; } a = s_alphas.Dequeue(); return true; } }

        /// <summary>Forgets everything (a new world).</summary>
        /// <summary>In-game height edits (a building flattening its pad): the engine re-meshes these tiles of
        /// GroundTile x GroundTile cells. Tiles, not one rectangle, since buildings go up all over the map.</summary>
        public const int GroundTile = 32;
        static readonly HashSet<long> s_ground = new HashSet<long>();
        public static void GroundEdited(int x0, int y0, int x1, int y1)
        {
            lock (s_lock)
                for (int ty = Math.Max(0, y0) / GroundTile; ty <= Math.Max(0, y1) / GroundTile; ty++)
                    for (int tx = Math.Max(0, x0) / GroundTile; tx <= Math.Max(0, x1) / GroundTile; tx++)
                        s_ground.Add(((long)tx << 32) | (uint)ty);
        }
        public static bool TakeGroundTile(out int tx, out int ty)
        {
            lock (s_lock)
            {
                foreach (var k in s_ground) { s_ground.Remove(k); tx = (int)(k >> 32); ty = (int)(uint)k; return true; }
                tx = ty = 0; return false;
            }
        }

        public static void Reset()
        {
            lock (s_lock)
            {
                s_alphas.Clear(); s_building.Clear(); s_dots.Clear(); s_ground.Clear();
                s_hx0 = s_hy0 = int.MaxValue; s_hx1 = s_hy1 = int.MinValue;
                AccessVisible = false; AccessColors = null; AccessVersion++;
            }
        }
    }

    /// <summary>GL state scopes do nothing here: the engine renders.</summary>
    public sealed class NullRenderContext : java.lang.Object, com.oddlabs.tt.render.state.RenderContext
    {
        public void clear(bool color, bool depth) { }
    }

    public sealed class HostWindow : java.lang.Object, com.oddlabs.tt.window.Window, com.oddlabs.tt.input.InputProviderSource
    {
        bool _closeRequested;
        int _lastW, _lastH;
        public com.oddlabs.tt.input.InputProvider createInputProvider() => Host.Input;
        public void create(com.oddlabs.tt.render.SerializableDisplayMode mode, bool fullscreen) { }
        public void close() { }
        public void update() { }
        public void pollEvents() { }
        public bool isOpen() => !Host.Finished;
        public bool isCloseRequested() => _closeRequested;
        public void setCloseRequested(bool value) => _closeRequested = value;
        public void RequestClose() => _closeRequested = true;
        public bool isActive() => Host.Backend.Active;
        public bool isVisible() => true;
        public bool isIconified() => false;
        public bool wasResized()
        {
            int w = getWidth(), h = getHeight();
            bool r = w != _lastW || h != _lastH;
            _lastW = w; _lastH = h;
            return r;
        }
        public int getWidth() => Host.Backend.Width;
        public int getHeight() => Host.Backend.Height;
        public int getLogicalWidth() => Host.Backend.Width;
        public int getLogicalHeight() => Host.Backend.Height;
        public void setTitle(string title) { }
        public void setVSyncEnabled(bool enabled) { }
        public com.oddlabs.tt.render.SerializableDisplayMode[] getAvailableDisplayModes() => new com.oddlabs.tt.render.SerializableDisplayMode[0];
        public com.oddlabs.tt.render.SerializableDisplayMode getDisplayMode() => null;
        public void setIcon(java.nio.file.Path imagePath) { }
        public void restore() { }
        public void minimize() { }
        public void show() { }
        public void focus() { }
        public bool isFullscreen() => Host.Backend.Fullscreen;
        public void setFullscreen(bool fullscreen) { if (fullscreen != Host.Backend.Fullscreen) Host.Backend.ToggleFullscreen(); }
        public void setDisplayMode(com.oddlabs.tt.render.SerializableDisplayMode mode) { }
        public void makeCurrent() { }
        public int[] getMonitorPhysicalSize() => new[] { 520, 290 };
        public float[] getMonitorContentScale() => new[] { 1f, 1f };
        public float[] getWindowContentScale() => new[] { 1f, 1f };
    }
}

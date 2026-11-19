// Translated from LocalInput.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 24
    public class LocalInput : java.lang.Object, java.lang.AutoCloseable
    {
        public static java.util.logging.Logger logger;
        public const int CURSOR_ONE_BIT_TRANSPARENCY = 1;
        public const int CURSOR_8_BIT_ALPHA = 2;
        public int mouse_x;
        public int mouse_y;
        public com.oddlabs.tt.input.InputProvider inputProvider;
        public com.oddlabs.tt.input.InputManager inputManager;
        public com.oddlabs.tt.input.KeyboardInput keyboardInput;
        public com.oddlabs.tt.input.PointerInput pointerInput;
        public java.util.Set keys;
        public java.util.Set global_modifiers;
        public java.nio.file.Path game_dir;
        public int revision;
        // Java: 55
        public void poll(com.oddlabs.tt.gui.GUIRoot root)
        {
            pointerInput.poll(root);
            keyboardInput.poll(inputProvider, this, root);
        }
        // Java: 60
        public void checkMagicKeys()
        {
            keyboardInput.checkMagicKeys(inputProvider);
        }
        // Java: 64
        public void setKeys(com.oddlabs.tt.input.Key key, bool state, java.util.Set modifiers)
        {
            if (state)
            {
                keys.add(key);
            }
            else
            {
                keys.remove(key);
            }
            global_modifiers.clear();
            global_modifiers.addAll(modifiers);
        }
        // Java: 73
        public void mouseDragged(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.MouseButton button, short x, short y)
        {
            mouse_x = x;
            mouse_y = y;
            gui_root.getInputState().mouseDragged(button, x, y);
        }
        // Java: 79
        public void mouseReleased(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.MouseButton button)
        {
            gui_root.getInputState().mouseReleased(button);
        }
        // Java: 83
        public void mousePressed(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.MouseButton button)
        {
            gui_root.getInputState().mousePressed(button);
        }
        // Java: 87
        public void mouseScrolled(com.oddlabs.tt.gui.GUIRoot gui_root, int dz)
        {
            gui_root.getInputState().mouseScrolled(dz);
        }
        // Java: 91
        public void mouseMoved(com.oddlabs.tt.gui.GUIRoot gui_root, short x, short y)
        {
            mouse_x = x;
            mouse_y = y;
            gui_root.getInputState().mouseMoved(x, y);
        }
        // Java: 97
        public bool isShiftDownCurrently()
        {
            return global_modifiers.contains(com.oddlabs.tt.input.Modifier.SHIFT);
        }
        // Java: 101
        public bool isControlDownCurrently()
        {
            return global_modifiers.contains(com.oddlabs.tt.input.Modifier.CONTROL);
        }
        // Java: 105
        public bool isAltDownCurrently()
        {
            return global_modifiers.contains(com.oddlabs.tt.input.Modifier.ALT);
        }
        // Java: 109
        public bool isSuperDownCurrently()
        {
            return global_modifiers.contains(com.oddlabs.tt.input.Modifier.META);
        }
        // Java: 113
        public void resetKeys()
        {
            keyboardInput.reset(inputProvider);
            inputManager.reset();
            keys.clear();
        }
        // Java: 119
        public bool isKeyDown(com.oddlabs.tt.input.Key key)
        {
            return keys.contains(key);
        }
        // Java: 123
        public void resetKeyboard()
        {
            resetKeys();
            global_modifiers.clear();
        }
        // Java: 128
        public int getMouseY()
        {
            return mouse_y;
        }
        // Java: 132
        public int getMouseX()
        {
            return mouse_x;
        }
        // Java: 136
        public bool audioIsCreated()
        {
            return com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic().log(com.oddlabs.tt.audio.AudioManager.getManager() != null);
        }
        // Java: 140
        public java.nio.file.Path getGameDir()
        {
            return game_dir;
        }
        // Java: 144
        public int getRevision()
        {
            return revision;
        }
        // Java: 148
        public void settings(java.nio.file.Path game_dir, java.nio.file.Path event_log_dir, com.oddlabs.tt.global.Settings settings_v)
        {
            setSettings(game_dir, event_log_dir, revision, settings_v);
        }
        // Java: 152
        public void setSettings(java.nio.file.Path game_dir, java.nio.file.Path event_log_dir, int revision, com.oddlabs.tt.global.Settings settings_v)
        {
            com.oddlabs.tt.gui.LocalInput.logger.config("revision = " + (revision));
            this.game_dir = game_dir;
            this.revision = revision;
            settings_v.last_event_log_dir = event_log_dir.toAbsolutePath();
            settings_v.last_revision = revision;
            settings_v.crashed = true;
            settings_v.save();
            settings_v.crashed = false;
        }
        // Java: 164
        public void init()
        {
            if ((inputProvider is com.oddlabs.tt.input.LWJGL3InputProvider lwjgl3InputProvider))
            {
                lwjgl3InputProvider.initCallbacks();
            }
            pointerInput.loadCursors();
            com.oddlabs.@event.Deterministic deterministic = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
            mouse_x = deterministic.log(inputProvider.getMouseX());
            mouse_y = deterministic.log(inputProvider.getMouseY());
        }
        // Java: 174
        public void close()
        {
            inputProvider.close();
        }
        // Java: 178
        public com.oddlabs.tt.input.InputProvider getInputProvider()
        {
            return ((com.oddlabs.tt.input.InputProvider)inputProvider);
        }
        // Java: 183
        public com.oddlabs.tt.input.InputManager getInputManager()
        {
            return inputManager;
        }
        // Java: 187
        public com.oddlabs.tt.input.KeyboardInput getKeyboardInput()
        {
            return keyboardInput;
        }
        // Java: 191
        public com.oddlabs.tt.input.PointerInput getPointerInput()
        {
            return pointerInput;
        }
        static LocalInput()
        {
            LocalInput.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.gui.LocalInput).getName());
        }
        private void init_LocalInput()
        {
            this.inputManager = new com.oddlabs.tt.input.InputManager();
            this.keyboardInput = new com.oddlabs.tt.input.KeyboardInput();
            this.keys = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Key));
            this.global_modifiers = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier));
        }
        public LocalInput(com.oddlabs.tt.window.Window lwjglWindow) : base()
        {
            init_LocalInput();
            if ((lwjglWindow is com.oddlabs.tt.window.LWJGL3Window win))
            {
                inputProvider = new com.oddlabs.tt.input.LWJGL3InputProvider(win);
            }
            else if ((lwjglWindow is com.oddlabs.tt.input.InputProviderSource source))
            {
                inputProvider = source.createInputProvider();
            }
            else
            {
                throw new System.InvalidOperationException("Window is not LWJGL3Window");
            }
            pointerInput = new com.oddlabs.tt.input.PointerInput(inputProvider, this);
        }
    }
}

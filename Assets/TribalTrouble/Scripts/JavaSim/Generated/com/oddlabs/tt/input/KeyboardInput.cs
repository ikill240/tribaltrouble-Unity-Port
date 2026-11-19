// Translated from KeyboardInput.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 21
    public class KeyboardInput : java.lang.Object
    {
        public static java.util.logging.Logger logger;
        public const int LITTLE_WARP = 1000;
        public const int MEDIUM_WARP = 10000;
        public const int LARGE_WARP = 100000;
        public const int GOTO_END_OF_LOG_WARP = 1073741823;
        public bool left_shift_down;
        public bool right_shift_down;
        public bool left_control_down;
        public bool right_control_down;
        public bool left_alt_down;
        public bool right_alt_down;
        public bool left_meta_down;
        public bool right_meta_down;
        // Java: 37
        public void reset(com.oddlabs.tt.input.InputProvider input)
        {
            while (input != null && input.nextKeyboardEvent())
            {
                ;
            }
            left_shift_down = false;
            right_shift_down = false;
            left_control_down = false;
            right_control_down = false;
            left_alt_down = false;
            right_alt_down = false;
            left_meta_down = false;
            right_meta_down = false;
        }
        // Java: 50
        public bool isAltDown()
        {
            return left_alt_down || right_alt_down;
        }
        // Java: 54
        public bool isMetaDown()
        {
            return left_meta_down || right_meta_down;
        }
        // Java: 67
        public bool checkMagicKey(bool event_key_down, com.oddlabs.tt.input.Key event_key, bool playback, bool repeat)
        {
            bool control_down = left_control_down || right_control_down;
            bool shift_down = left_shift_down || right_shift_down;
            bool keys_enabled = com.oddlabs.tt.global.Settings.getSettings().inDeveloperMode() && control_down && shift_down && !repeat;
            if (event_key_down && (keys_enabled || playback))
            {
                switch ((event_key).ordinal())
                {
                    case 3 /*RIGHT*/:
                    {
                        com.oddlabs.tt.animation.AnimationManager.warpTime(com.oddlabs.tt.input.KeyboardInput.LITTLE_WARP);
                        return true;
                    }
                    case 0 /*UP*/:
                    {
                        com.oddlabs.tt.animation.AnimationManager.warpTime(com.oddlabs.tt.input.KeyboardInput.MEDIUM_WARP);
                        return true;
                    }
                    case 64 /*PAGE_UP*/:
                    {
                        com.oddlabs.tt.animation.AnimationManager.warpTime(com.oddlabs.tt.input.KeyboardInput.LARGE_WARP);
                        return true;
                    }
                    case 36 /*Q*/:
                    {
                        com.oddlabs.tt.render.Renderer.shutdown();
                        return true;
                    }
                    case 5 /*SPACE*/:
                    {
                        com.oddlabs.tt.animation.AnimationManager.toggleTimeStop();
                        return true;
                    }
                    case 62 /*END*/:
                    {
                        com.oddlabs.tt.input.KeyboardInput.logger.info("WARP UNTIL END OF EVENT LOG");
                        com.oddlabs.tt.animation.AnimationManager.warpTime(com.oddlabs.tt.input.KeyboardInput.GOTO_END_OF_LOG_WARP);
                        return true;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            return false;
        }
        // Java: 106
        public void checkMagicKeys(com.oddlabs.tt.input.InputProvider input)
        {
            com.oddlabs.@event.Deterministic deterministic = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
            if (deterministic.isPlayback())
            {
                input.pollKeyboard();
                while (input.nextKeyboardEvent())
                {
                    int event_key_code = input.getEventKey();
                    com.oddlabs.tt.input.Key event_key = com.oddlabs.tt.input.Key.fromGlfwCode(event_key_code);
                    if (((object)com.oddlabs.tt.input.Key.KEY_UNKNOWN != (object)event_key))
                    {
                        bool event_key_state = input.getEventKeyState();
                        checkMagicKey(event_key_state, event_key, true, input.isRepeatEvent());
                    }
                }
            }
        }
        // Java: 122
        public bool poll(com.oddlabs.tt.input.InputProvider input, com.oddlabs.tt.gui.LocalInput localInput, com.oddlabs.tt.gui.GUIRoot gui_root)
        {
            com.oddlabs.@event.Deterministic deterministic = com.oddlabs.tt.@event.LocalEventQueue.getQueue().getDeterministic();
            bool result = false;
            input.pollKeyboard();
            left_shift_down = input.isKeyDown(com.oddlabs.tt.input.Key.LSHIFT.getGlfwCode());
            right_shift_down = input.isKeyDown(com.oddlabs.tt.input.Key.RSHIFT.getGlfwCode());
            left_control_down = input.isKeyDown(com.oddlabs.tt.input.Key.LCONTROL.getGlfwCode());
            right_control_down = input.isKeyDown(com.oddlabs.tt.input.Key.RCONTROL.getGlfwCode());
            left_alt_down = input.isKeyDown(com.oddlabs.tt.input.Key.LALT.getGlfwCode());
            right_alt_down = input.isKeyDown(com.oddlabs.tt.input.Key.RALT.getGlfwCode());
            left_meta_down = input.isKeyDown(com.oddlabs.tt.input.Key.LSUPER.getGlfwCode());
            right_meta_down = input.isKeyDown(com.oddlabs.tt.input.Key.RSUPER.getGlfwCode());
            while (deterministic.log(input.nextKeyboardEvent()))
            {
                result = true;
                int event_key_code = deterministic.log(input.getEventKey());
                int event_key_mods = deterministic.log(input.getEventKeyMods());
                com.oddlabs.tt.input.Key event_key = com.oddlabs.tt.input.Key.fromGlfwCode(event_key_code);
                bool event_key_down = deterministic.log(input.getEventKeyState());
                char event_character = deterministic.log(input.getEventCharacter());
                bool repeat_event = deterministic.log(input.isRepeatEvent());
                switch ((event_key).ordinal())
                {
                    case 66 /*LSHIFT*/:
                    {
                        left_shift_down = event_key_down;
                        break;
                    }
                    case 67 /*RSHIFT*/:
                    {
                        right_shift_down = event_key_down;
                        break;
                    }
                    case 68 /*LCONTROL*/:
                    {
                        left_control_down = event_key_down;
                        break;
                    }
                    case 69 /*RCONTROL*/:
                    {
                        right_control_down = event_key_down;
                        break;
                    }
                    case 70 /*LALT*/:
                    {
                        left_alt_down = event_key_down;
                        break;
                    }
                    case 71 /*RALT*/:
                    {
                        right_alt_down = event_key_down;
                        break;
                    }
                    case 72 /*LSUPER*/:
                    {
                        left_meta_down = event_key_down;
                        break;
                    }
                    case 73 /*RSUPER*/:
                    {
                        right_meta_down = event_key_down;
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
                if (!gui_root.isCapturingKeyBinding() && checkMagicKey(event_key_down, event_key, false, repeat_event))
                {
                    continue;
                }
                java.util.Set modifiers = java.util.EnumSet.noneOf(typeof(com.oddlabs.tt.input.Modifier));
                if ((event_key_mods & org.lwjgl.glfw.GLFW.GLFW_MOD_CONTROL) != 0)
                {
                    modifiers.add(com.oddlabs.tt.input.Modifier.CONTROL);
                }
                if ((event_key_mods & org.lwjgl.glfw.GLFW.GLFW_MOD_SHIFT) != 0)
                {
                    modifiers.add(com.oddlabs.tt.input.Modifier.SHIFT);
                }
                if ((event_key_mods & org.lwjgl.glfw.GLFW.GLFW_MOD_ALT) != 0)
                {
                    modifiers.add(com.oddlabs.tt.input.Modifier.ALT);
                }
                if ((event_key_mods & org.lwjgl.glfw.GLFW.GLFW_MOD_SUPER) != 0)
                {
                    modifiers.add(com.oddlabs.tt.input.Modifier.META);
                }
                if (event_key_code == 0 && modifiers.isEmpty())
                {
                    gui_root.getInputState().keyTyped(event_key_code, event_character);
                }
                else if (event_key_down)
                {
                    if (((object)com.oddlabs.tt.input.Key.KEY_UNKNOWN != (object)event_key) || event_character != 0)
                    {
                        localInput.setKeys(event_key, true, modifiers);
                        gui_root.getInputState().keyPressed(event_key, event_character, modifiers, repeat_event);
                    }
                }
                else
                {
                    if (((object)com.oddlabs.tt.input.Key.KEY_UNKNOWN != (object)event_key) || event_character != 0)
                    {
                        localInput.setKeys(event_key, false, modifiers);
                        gui_root.getInputState().keyReleased(event_key, event_character, modifiers);
                    }
                }
            }
            return result;
        }
        static KeyboardInput()
        {
            KeyboardInput.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.input.KeyboardInput).getName());
        }
        public KeyboardInput() : base()
        {
        }
    }
}

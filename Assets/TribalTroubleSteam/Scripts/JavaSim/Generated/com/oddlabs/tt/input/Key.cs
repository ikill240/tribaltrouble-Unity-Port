// Translated from Key.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 11
    public sealed class Key : java.lang.Enum
    {
        public static Key[] values() { return (Key[])VALUES_.Clone(); }
        public static Key valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Key UP = new Key("UP", 0, org.lwjgl.glfw.GLFW.GLFW_KEY_UP);
        public static readonly Key DOWN = new Key("DOWN", 1, org.lwjgl.glfw.GLFW.GLFW_KEY_DOWN);
        public static readonly Key LEFT = new Key("LEFT", 2, org.lwjgl.glfw.GLFW.GLFW_KEY_LEFT);
        public static readonly Key RIGHT = new Key("RIGHT", 3, org.lwjgl.glfw.GLFW.GLFW_KEY_RIGHT);
        public static readonly Key ESCAPE = new Key("ESCAPE", 4, org.lwjgl.glfw.GLFW.GLFW_KEY_ESCAPE);
        public static readonly Key SPACE = new Key("SPACE", 5, org.lwjgl.glfw.GLFW.GLFW_KEY_SPACE);
        public static readonly Key RETURN = new Key("RETURN", 6, org.lwjgl.glfw.GLFW.GLFW_KEY_ENTER);
        public static readonly Key TAB = new Key("TAB", 7, org.lwjgl.glfw.GLFW.GLFW_KEY_TAB);
        public static readonly Key F1 = new Key("F1", 8, org.lwjgl.glfw.GLFW.GLFW_KEY_F1);
        public static readonly Key F2 = new Key("F2", 9, org.lwjgl.glfw.GLFW.GLFW_KEY_F2);
        public static readonly Key F3 = new Key("F3", 10, org.lwjgl.glfw.GLFW.GLFW_KEY_F3);
        public static readonly Key F4 = new Key("F4", 11, org.lwjgl.glfw.GLFW.GLFW_KEY_F4);
        public static readonly Key F5 = new Key("F5", 12, org.lwjgl.glfw.GLFW.GLFW_KEY_F5);
        public static readonly Key F6 = new Key("F6", 13, org.lwjgl.glfw.GLFW.GLFW_KEY_F6);
        public static readonly Key F7 = new Key("F7", 14, org.lwjgl.glfw.GLFW.GLFW_KEY_F7);
        public static readonly Key F8 = new Key("F8", 15, org.lwjgl.glfw.GLFW.GLFW_KEY_F8);
        public static readonly Key F9 = new Key("F9", 16, org.lwjgl.glfw.GLFW.GLFW_KEY_F9);
        public static readonly Key F10 = new Key("F10", 17, org.lwjgl.glfw.GLFW.GLFW_KEY_F10);
        public static readonly Key F11 = new Key("F11", 18, org.lwjgl.glfw.GLFW.GLFW_KEY_F11);
        public static readonly Key F12 = new Key("F12", 19, org.lwjgl.glfw.GLFW.GLFW_KEY_F12);
        public static readonly Key A = new Key("A", 20, org.lwjgl.glfw.GLFW.GLFW_KEY_A);
        public static readonly Key B = new Key("B", 21, org.lwjgl.glfw.GLFW.GLFW_KEY_B);
        public static readonly Key C = new Key("C", 22, org.lwjgl.glfw.GLFW.GLFW_KEY_C);
        public static readonly Key D = new Key("D", 23, org.lwjgl.glfw.GLFW.GLFW_KEY_D);
        public static readonly Key E = new Key("E", 24, org.lwjgl.glfw.GLFW.GLFW_KEY_E);
        public static readonly Key F = new Key("F", 25, org.lwjgl.glfw.GLFW.GLFW_KEY_F);
        public static readonly Key G = new Key("G", 26, org.lwjgl.glfw.GLFW.GLFW_KEY_G);
        public static readonly Key H = new Key("H", 27, org.lwjgl.glfw.GLFW.GLFW_KEY_H);
        public static readonly Key I = new Key("I", 28, org.lwjgl.glfw.GLFW.GLFW_KEY_I);
        public static readonly Key J = new Key("J", 29, org.lwjgl.glfw.GLFW.GLFW_KEY_J);
        public static readonly Key K = new Key("K", 30, org.lwjgl.glfw.GLFW.GLFW_KEY_K);
        public static readonly Key L = new Key("L", 31, org.lwjgl.glfw.GLFW.GLFW_KEY_L);
        public static readonly Key M = new Key("M", 32, org.lwjgl.glfw.GLFW.GLFW_KEY_M);
        public static readonly Key N = new Key("N", 33, org.lwjgl.glfw.GLFW.GLFW_KEY_N);
        public static readonly Key O = new Key("O", 34, org.lwjgl.glfw.GLFW.GLFW_KEY_O);
        public static readonly Key P = new Key("P", 35, org.lwjgl.glfw.GLFW.GLFW_KEY_P);
        public static readonly Key Q = new Key("Q", 36, org.lwjgl.glfw.GLFW.GLFW_KEY_Q);
        public static readonly Key R = new Key("R", 37, org.lwjgl.glfw.GLFW.GLFW_KEY_R);
        public static readonly Key S = new Key("S", 38, org.lwjgl.glfw.GLFW.GLFW_KEY_S);
        public static readonly Key T = new Key("T", 39, org.lwjgl.glfw.GLFW.GLFW_KEY_T);
        public static readonly Key U = new Key("U", 40, org.lwjgl.glfw.GLFW.GLFW_KEY_U);
        public static readonly Key V = new Key("V", 41, org.lwjgl.glfw.GLFW.GLFW_KEY_V);
        public static readonly Key W = new Key("W", 42, org.lwjgl.glfw.GLFW.GLFW_KEY_W);
        public static readonly Key X = new Key("X", 43, org.lwjgl.glfw.GLFW.GLFW_KEY_X);
        public static readonly Key Y = new Key("Y", 44, org.lwjgl.glfw.GLFW.GLFW_KEY_Y);
        public static readonly Key Z = new Key("Z", 45, org.lwjgl.glfw.GLFW.GLFW_KEY_Z);
        public static readonly Key NUMPAD0 = new Key("NUMPAD0", 46, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_0);
        public static readonly Key NUMPAD1 = new Key("NUMPAD1", 47, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_1);
        public static readonly Key NUMPAD2 = new Key("NUMPAD2", 48, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_2);
        public static readonly Key NUMPAD3 = new Key("NUMPAD3", 49, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_3);
        public static readonly Key NUMPAD4 = new Key("NUMPAD4", 50, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_4);
        public static readonly Key NUMPAD5 = new Key("NUMPAD5", 51, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_5);
        public static readonly Key NUMPAD6 = new Key("NUMPAD6", 52, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_6);
        public static readonly Key NUMPAD7 = new Key("NUMPAD7", 53, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_7);
        public static readonly Key NUMPAD8 = new Key("NUMPAD8", 54, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_8);
        public static readonly Key NUMPAD9 = new Key("NUMPAD9", 55, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_9);
        public static readonly Key MULTIPLY = new Key("MULTIPLY", 56, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_MULTIPLY);
        public static readonly Key DIVIDE = new Key("DIVIDE", 57, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_DIVIDE);
        public static readonly Key DECIMAL = new Key("DECIMAL", 58, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_DECIMAL);
        public static readonly Key DELETE = new Key("DELETE", 59, org.lwjgl.glfw.GLFW.GLFW_KEY_DELETE);
        public static readonly Key BACK = new Key("BACK", 60, org.lwjgl.glfw.GLFW.GLFW_KEY_BACKSPACE);
        public static readonly Key HOME = new Key("HOME", 61, org.lwjgl.glfw.GLFW.GLFW_KEY_HOME);
        public static readonly Key END = new Key("END", 62, org.lwjgl.glfw.GLFW.GLFW_KEY_END);
        public static readonly Key INSERT = new Key("INSERT", 63, org.lwjgl.glfw.GLFW.GLFW_KEY_INSERT);
        public static readonly Key PAGE_UP = new Key("PAGE_UP", 64, org.lwjgl.glfw.GLFW.GLFW_KEY_PAGE_UP);
        public static readonly Key PAGE_DOWN = new Key("PAGE_DOWN", 65, org.lwjgl.glfw.GLFW.GLFW_KEY_PAGE_DOWN);
        public static readonly Key LSHIFT = new Key("LSHIFT", 66, org.lwjgl.glfw.GLFW.GLFW_KEY_LEFT_SHIFT);
        public static readonly Key RSHIFT = new Key("RSHIFT", 67, org.lwjgl.glfw.GLFW.GLFW_KEY_RIGHT_SHIFT);
        public static readonly Key LCONTROL = new Key("LCONTROL", 68, org.lwjgl.glfw.GLFW.GLFW_KEY_LEFT_CONTROL);
        public static readonly Key RCONTROL = new Key("RCONTROL", 69, org.lwjgl.glfw.GLFW.GLFW_KEY_RIGHT_CONTROL);
        public static readonly Key LALT = new Key("LALT", 70, org.lwjgl.glfw.GLFW.GLFW_KEY_LEFT_ALT);
        public static readonly Key RALT = new Key("RALT", 71, org.lwjgl.glfw.GLFW.GLFW_KEY_RIGHT_ALT);
        public static readonly Key LSUPER = new Key("LSUPER", 72, org.lwjgl.glfw.GLFW.GLFW_KEY_LEFT_SUPER);
        public static readonly Key RSUPER = new Key("RSUPER", 73, org.lwjgl.glfw.GLFW.GLFW_KEY_RIGHT_SUPER);
        public static readonly Key COMMA = new Key("COMMA", 74, org.lwjgl.glfw.GLFW.GLFW_KEY_COMMA);
        public static readonly Key PERIOD = new Key("PERIOD", 75, org.lwjgl.glfw.GLFW.GLFW_KEY_PERIOD);
        public static readonly Key SLASH = new Key("SLASH", 76, org.lwjgl.glfw.GLFW.GLFW_KEY_SLASH);
        public static readonly Key BACKSLASH = new Key("BACKSLASH", 77, org.lwjgl.glfw.GLFW.GLFW_KEY_BACKSLASH);
        public static readonly Key SEMICOLON = new Key("SEMICOLON", 78, org.lwjgl.glfw.GLFW.GLFW_KEY_SEMICOLON);
        public static readonly Key APOSTROPHE = new Key("APOSTROPHE", 79, org.lwjgl.glfw.GLFW.GLFW_KEY_APOSTROPHE);
        public static readonly Key LBRACKET = new Key("LBRACKET", 80, org.lwjgl.glfw.GLFW.GLFW_KEY_LEFT_BRACKET);
        public static readonly Key RBRACKET = new Key("RBRACKET", 81, org.lwjgl.glfw.GLFW.GLFW_KEY_RIGHT_BRACKET);
        public static readonly Key GRAVE = new Key("GRAVE", 82, org.lwjgl.glfw.GLFW.GLFW_KEY_GRAVE_ACCENT);
        public static readonly Key KEY_1 = new Key("KEY_1", 83, org.lwjgl.glfw.GLFW.GLFW_KEY_1);
        public static readonly Key KEY_2 = new Key("KEY_2", 84, org.lwjgl.glfw.GLFW.GLFW_KEY_2);
        public static readonly Key KEY_3 = new Key("KEY_3", 85, org.lwjgl.glfw.GLFW.GLFW_KEY_3);
        public static readonly Key KEY_4 = new Key("KEY_4", 86, org.lwjgl.glfw.GLFW.GLFW_KEY_4);
        public static readonly Key KEY_5 = new Key("KEY_5", 87, org.lwjgl.glfw.GLFW.GLFW_KEY_5);
        public static readonly Key KEY_6 = new Key("KEY_6", 88, org.lwjgl.glfw.GLFW.GLFW_KEY_6);
        public static readonly Key KEY_7 = new Key("KEY_7", 89, org.lwjgl.glfw.GLFW.GLFW_KEY_7);
        public static readonly Key KEY_8 = new Key("KEY_8", 90, org.lwjgl.glfw.GLFW.GLFW_KEY_8);
        public static readonly Key KEY_9 = new Key("KEY_9", 91, org.lwjgl.glfw.GLFW.GLFW_KEY_9);
        public static readonly Key KEY_0 = new Key("KEY_0", 92, org.lwjgl.glfw.GLFW.GLFW_KEY_0);
        public static readonly Key EQUALS = new Key("EQUALS", 93, org.lwjgl.glfw.GLFW.GLFW_KEY_EQUAL);
        public static readonly Key MINUS = new Key("MINUS", 94, org.lwjgl.glfw.GLFW.GLFW_KEY_MINUS);
        public static readonly Key ADD = new Key("ADD", 95, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_ADD);
        public static readonly Key SUBTRACT = new Key("SUBTRACT", 96, org.lwjgl.glfw.GLFW.GLFW_KEY_KP_SUBTRACT);
        public static readonly Key KEY_UNKNOWN = new Key("KEY_UNKNOWN", 97, org.lwjgl.glfw.GLFW.GLFW_KEY_UNKNOWN);
        public int glfwCode;
        public static java.util.Map from_glfw_map;
        // Java: 120
        public int getGlfwCode()
        {
            return glfwCode;
        }
        // Java: 124
        public static com.oddlabs.tt.input.Key fromGlfwCode(int code)
        {
            return ((com.oddlabs.tt.input.Key)com.oddlabs.tt.input.Key.from_glfw_map.getOrDefault(code, com.oddlabs.tt.input.Key.KEY_UNKNOWN));
        }
        // Java: 128
        public string getDisplayName()
        {
            string name_v = name();
            if (name_v.startsWith("KEY_"))
            {
                return name_v.substring(4);
            }
            if (name_v.startsWith("NUMPAD"))
            {
                return "Num " + java.lang.JStr.valueOf(name_v.substring(6));
            }
            return ((this).ordinal() switch { 66 /*LSHIFT*/ => (string)("LShift"), 67 /*RSHIFT*/ => (string)("RShift"), 68 /*LCONTROL*/ => (string)("LCtrl"), 69 /*RCONTROL*/ => (string)("RCtrl"), 70 /*LALT*/ => (string)("LAlt"), 71 /*RALT*/ => (string)("RAlt"), 72 /*LSUPER*/ => (string)("LMeta"), 73 /*RSUPER*/ => (string)("RMeta"), 6 /*RETURN*/ => (string)("Enter"), 60 /*BACK*/ => (string)("Backspace"), 93 /*EQUALS*/ => (string)("="), 94 /*MINUS*/ => (string)("-"), 74 /*COMMA*/ => (string)(","), 75 /*PERIOD*/ => (string)("."), 76 /*SLASH*/ => (string)("/"), 77 /*BACKSLASH*/ => (string)("\\"), 78 /*SEMICOLON*/ => (string)(";"), 79 /*APOSTROPHE*/ => (string)("'"), 80 /*LBRACKET*/ => (string)("["), 81 /*RBRACKET*/ => (string)("]"), 82 /*GRAVE*/ => (string)("`"), 95 /*ADD*/ => (string)("Num +"), 96 /*SUBTRACT*/ => (string)("Num -"), 56 /*MULTIPLY*/ => (string)("Num *"), 57 /*DIVIDE*/ => (string)("Num /"), 58 /*DECIMAL*/ => (string)("Num ."), _ => (string)(name_v) });
        }
        public static readonly Key[] VALUES_ = new Key[] { UP, DOWN, LEFT, RIGHT, ESCAPE, SPACE, RETURN, TAB, F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, NUMPAD0, NUMPAD1, NUMPAD2, NUMPAD3, NUMPAD4, NUMPAD5, NUMPAD6, NUMPAD7, NUMPAD8, NUMPAD9, MULTIPLY, DIVIDE, DECIMAL, DELETE, BACK, HOME, END, INSERT, PAGE_UP, PAGE_DOWN, LSHIFT, RSHIFT, LCONTROL, RCONTROL, LALT, RALT, LSUPER, RSUPER, COMMA, PERIOD, SLASH, BACKSLASH, SEMICOLON, APOSTROPHE, LBRACKET, RBRACKET, GRAVE, KEY_1, KEY_2, KEY_3, KEY_4, KEY_5, KEY_6, KEY_7, KEY_8, KEY_9, KEY_0, EQUALS, MINUS, ADD, SUBTRACT, KEY_UNKNOWN };
        static Key()
        {
            Key.from_glfw_map = ((java.util.Map)java.util.Arrays.stream(com.oddlabs.tt.input.Key.values()).collect(java.util.stream.Collectors.toMap(new java.util.function.Function_L((object a0) => { return ((com.oddlabs.tt.input.Key)a0).getGlfwCode(); }), java.util.function.Function_.identity())));
        }
        public Key(string name_, int ordinal_, int glfwCode) : base(name_, ordinal_)
        {
            this.glfwCode = glfwCode;
        }
    }
}

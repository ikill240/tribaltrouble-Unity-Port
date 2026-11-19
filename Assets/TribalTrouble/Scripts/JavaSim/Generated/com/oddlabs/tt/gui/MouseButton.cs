// Translated from MouseButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public sealed class MouseButton : java.lang.Enum
    {
        public static MouseButton[] values() { return (MouseButton[])VALUES_.Clone(); }
        public static MouseButton valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly MouseButton LEFT = new MouseButton("LEFT", 0);
        public static readonly MouseButton RIGHT = new MouseButton("RIGHT", 1);
        public static readonly MouseButton MIDDLE = new MouseButton("MIDDLE", 2);
        // Java: 10
        public static com.oddlabs.tt.gui.MouseButton fromInt(int button)
        {
            return ((button) switch { 0 => (com.oddlabs.tt.gui.MouseButton)(com.oddlabs.tt.gui.MouseButton.LEFT), 1 => (com.oddlabs.tt.gui.MouseButton)(com.oddlabs.tt.gui.MouseButton.RIGHT), 2 => (com.oddlabs.tt.gui.MouseButton)(com.oddlabs.tt.gui.MouseButton.MIDDLE), _ => (com.oddlabs.tt.gui.MouseButton)(null) });
        }
        public static readonly MouseButton[] VALUES_ = new MouseButton[] { LEFT, RIGHT, MIDDLE };
        public MouseButton(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

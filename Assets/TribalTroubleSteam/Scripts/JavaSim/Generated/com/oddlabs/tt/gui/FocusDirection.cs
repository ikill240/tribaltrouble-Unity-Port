// Translated from FocusDirection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public sealed class FocusDirection : java.lang.Enum
    {
        public static FocusDirection[] values() { return (FocusDirection[])VALUES_.Clone(); }
        public static FocusDirection valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly FocusDirection FORWARD = new FocusDirection("FORWARD", 0);
        public static readonly FocusDirection BACKWARD = new FocusDirection("BACKWARD", 1);
        public static readonly FocusDirection UP = new FocusDirection("UP", 2);
        public static readonly FocusDirection DOWN = new FocusDirection("DOWN", 3);
        public static readonly FocusDirection LEFT = new FocusDirection("LEFT", 4);
        public static readonly FocusDirection RIGHT = new FocusDirection("RIGHT", 5);
        public static readonly FocusDirection[] VALUES_ = new FocusDirection[] { FORWARD, BACKWARD, UP, DOWN, LEFT, RIGHT };
        public FocusDirection(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

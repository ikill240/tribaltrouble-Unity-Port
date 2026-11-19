// Translated from Placement.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public sealed class Placement : java.lang.Enum
    {
        public static Placement[] values() { return (Placement[])VALUES_.Clone(); }
        public static Placement valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Placement TOP_LEFT = new Placement("TOP_LEFT", 0);
        public static readonly Placement TOP_MID = new Placement("TOP_MID", 1);
        public static readonly Placement TOP_RIGHT = new Placement("TOP_RIGHT", 2);
        public static readonly Placement BOTTOM_LEFT = new Placement("BOTTOM_LEFT", 3);
        public static readonly Placement BOTTOM_MID = new Placement("BOTTOM_MID", 4);
        public static readonly Placement BOTTOM_RIGHT = new Placement("BOTTOM_RIGHT", 5);
        public static readonly Placement LEFT_TOP = new Placement("LEFT_TOP", 6);
        public static readonly Placement LEFT_MID = new Placement("LEFT_MID", 7);
        public static readonly Placement LEFT_BOTTOM = new Placement("LEFT_BOTTOM", 8);
        public static readonly Placement RIGHT_TOP = new Placement("RIGHT_TOP", 9);
        public static readonly Placement RIGHT_MID = new Placement("RIGHT_MID", 10);
        public static readonly Placement RIGHT_BOTTOM = new Placement("RIGHT_BOTTOM", 11);
        public static readonly Placement[] VALUES_ = new Placement[] { TOP_LEFT, TOP_MID, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_MID, BOTTOM_RIGHT, LEFT_TOP, LEFT_MID, LEFT_BOTTOM, RIGHT_TOP, RIGHT_MID, RIGHT_BOTTOM };
        public Placement(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

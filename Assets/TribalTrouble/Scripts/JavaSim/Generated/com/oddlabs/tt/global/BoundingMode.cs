// Translated from BoundingMode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.global
{
    // Java: 7
    public sealed class BoundingMode : java.lang.Enum
    {
        public static BoundingMode[] values() { return (BoundingMode[])VALUES_.Clone(); }
        public static BoundingMode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly BoundingMode NONE = new BoundingMode("NONE", 0);
        public static readonly BoundingMode UNIT_GRID = new BoundingMode("UNIT_GRID", 1);
        public static readonly BoundingMode LANDSCAPE = new BoundingMode("LANDSCAPE", 2);
        public static readonly BoundingMode PLAYERS = new BoundingMode("PLAYERS", 3);
        public static readonly BoundingMode OCCUPATION = new BoundingMode("OCCUPATION", 4);
        public static readonly BoundingMode REGIONS = new BoundingMode("REGIONS", 5);
        public static readonly BoundingMode SHIPS = new BoundingMode("SHIPS", 6);
        public static readonly BoundingMode ALL = new BoundingMode("ALL", 7);
        // Java: 46
        public com.oddlabs.tt.global.BoundingMode next()
        {
            int nextOrdinal = (ordinal() + 1) % com.oddlabs.tt.global.BoundingMode.values().Length;
            return com.oddlabs.tt.global.BoundingMode.values()[nextOrdinal];
        }
        public static readonly BoundingMode[] VALUES_ = new BoundingMode[] { NONE, UNIT_GRID, LANDSCAPE, PLAYERS, OCCUPATION, REGIONS, SHIPS, ALL };
        public BoundingMode(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

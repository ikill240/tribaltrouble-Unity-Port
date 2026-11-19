// Translated from Origin.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public sealed class Origin : java.lang.Enum
    {
        public static Origin[] values() { return (Origin[])VALUES_.Clone(); }
        public static Origin valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Origin AT_START = new Origin("AT_START", 0);
        public static readonly Origin AT_MIDDLE = new Origin("AT_MIDDLE", 1);
        public static readonly Origin AT_END = new Origin("AT_END", 2);
        public static readonly Origin[] VALUES_ = new Origin[] { AT_START, AT_MIDDLE, AT_END };
        public Origin(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

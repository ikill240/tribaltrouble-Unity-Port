// Translated from Modifier.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 3
    public sealed class Modifier : java.lang.Enum
    {
        public static Modifier[] values() { return (Modifier[])VALUES_.Clone(); }
        public static Modifier valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Modifier SHIFT = new Modifier("SHIFT", 0);
        public static readonly Modifier CONTROL = new Modifier("CONTROL", 1);
        public static readonly Modifier ALT = new Modifier("ALT", 2);
        public static readonly Modifier META = new Modifier("META", 3);
        public static readonly Modifier[] VALUES_ = new Modifier[] { SHIFT, CONTROL, ALT, META };
        public Modifier(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

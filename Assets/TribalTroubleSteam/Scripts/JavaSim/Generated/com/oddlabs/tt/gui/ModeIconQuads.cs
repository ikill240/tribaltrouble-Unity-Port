// Translated from ModeIconQuads.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 10
    public class ModeIconQuads : java.util.EnumMap
    {
        // Java: 12
        public sealed class Mode : java.lang.Enum
        {
            public static Mode[] values() { return (Mode[])VALUES_.Clone(); }
            public static Mode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly Mode NORMAL = new Mode("NORMAL", 0);
            public static readonly Mode ACTIVE = new Mode("ACTIVE", 1);
            public static readonly Mode DISABLED = new Mode("DISABLED", 2);
            public static readonly Mode[] VALUES_ = new Mode[] { NORMAL, ACTIVE, DISABLED };
            public Mode(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 25
        public virtual com.oddlabs.tt.gui.IconQuad quad(com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode)
        {
            return ((com.oddlabs.tt.gui.IconQuad)get(skinMode));
        }
        public ModeIconQuads(com.oddlabs.tt.gui.IconQuad normal, com.oddlabs.tt.gui.IconQuad active, com.oddlabs.tt.gui.IconQuad disabled) : base(typeof(com.oddlabs.tt.gui.ModeIconQuads.Mode))
        {
            put(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL, normal);
            put(com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE, active);
            put(com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED, disabled);
        }
    }
}

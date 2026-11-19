// Translated from InputPhase.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 3
    public sealed class InputPhase : java.lang.Enum
    {
        public static InputPhase[] values() { return (InputPhase[])VALUES_.Clone(); }
        public static InputPhase valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly InputPhase PRESSED = new InputPhase("PRESSED", 0);
        public static readonly InputPhase RELEASED = new InputPhase("RELEASED", 1);
        public static readonly InputPhase REPEAT = new InputPhase("REPEAT", 2);
        public static readonly InputPhase[] VALUES_ = new InputPhase[] { PRESSED, RELEASED, REPEAT };
        public InputPhase(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

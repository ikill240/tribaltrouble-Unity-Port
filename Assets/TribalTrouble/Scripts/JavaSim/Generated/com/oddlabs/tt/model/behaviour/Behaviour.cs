// Translated from Behaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 5
    public interface Behaviour
    {
        // Java: 6
        public sealed class State : java.lang.Enum
        {
            public static State[] values() { return (State[])VALUES_.Clone(); }
            public static State valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly State UNINTERRUPTIBLE = new State("UNINTERRUPTIBLE", 0);
            public static readonly State INTERRUPTIBLE = new State("INTERRUPTIBLE", 1);
            public static readonly State DONE = new State("DONE", 2);
            public static readonly State[] VALUES_ = new State[] { UNINTERRUPTIBLE, INTERRUPTIBLE, DONE };
            public State(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        // Java: 12
        com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t);
        // Java: 15
        bool isBlocking();
        // Java: 17
        void forceInterrupted();
    }
}

// Translated from Action.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public sealed class Action : java.lang.Enum
    {
        public static Action[] values() { return (Action[])VALUES_.Clone(); }
        public static Action valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly Action DEFAULT = new Action("DEFAULT", 0);
        public static readonly Action MOVE = new Action("MOVE", 1);
        public static readonly Action ATTACK = new Action("ATTACK", 2);
        public static readonly Action GATHER_REPAIR = new Action("GATHER_REPAIR", 3);
        public static readonly Action DEFEND = new Action("DEFEND", 4);
        public static readonly Action[] VALUES_ = new Action[] { DEFAULT, MOVE, ATTACK, GATHER_REPAIR, DEFEND };
        public Action(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

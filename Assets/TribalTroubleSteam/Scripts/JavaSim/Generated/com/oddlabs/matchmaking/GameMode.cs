// Translated from GameMode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 3
    public sealed class GameMode : java.lang.Enum
    {
        public static GameMode[] values() { return (GameMode[])VALUES_.Clone(); }
        public static GameMode valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly GameMode STANDARD = new GameMode("STANDARD", 0);
        public static readonly GameMode[] VALUES_ = new GameMode[] { STANDARD };
        public GameMode(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

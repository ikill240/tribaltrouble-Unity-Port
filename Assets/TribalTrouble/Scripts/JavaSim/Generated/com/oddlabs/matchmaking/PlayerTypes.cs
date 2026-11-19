// Translated from PlayerTypes.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 3
    public sealed class PlayerTypes : java.lang.Enum
    {
        public static PlayerTypes[] values() { return (PlayerTypes[])VALUES_.Clone(); }
        public static PlayerTypes valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly PlayerTypes None = new PlayerTypes("None", 0);
        public static readonly PlayerTypes Human = new PlayerTypes("Human", 1);
        public static readonly PlayerTypes AIEasy = new PlayerTypes("AIEasy", 2);
        public static readonly PlayerTypes AINormal = new PlayerTypes("AINormal", 3);
        public static readonly PlayerTypes AIHard = new PlayerTypes("AIHard", 4);
        public static readonly PlayerTypes[] VALUES_ = new PlayerTypes[] { None, Human, AIEasy, AINormal, AIHard };
        public PlayerTypes(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

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
        public static readonly GameMode KING_OF_THE_ISLAND = new GameMode("KING_OF_THE_ISLAND", 1);
        public static readonly GameMode KING_OF_THE_HILL = new GameMode("KING_OF_THE_HILL", 2);
        public static readonly GameMode SURVIVAL = new GameMode("SURVIVAL", 3);
        public static readonly GameMode ZOMBIES = new GameMode("ZOMBIES", 4);
        public static readonly GameMode REGICIDE = new GameMode("REGICIDE", 5);
        public static readonly GameMode CAPTURE_THE_FLAG = new GameMode("CAPTURE_THE_FLAG", 6);
        public static readonly GameMode MAGIC_RAMPAGE = new GameMode("MAGIC_RAMPAGE", 7);
        public static readonly GameMode ISLAND_WARS = new GameMode("ISLAND_WARS", 8);
        public static readonly GameMode[] VALUES_ = new GameMode[] { STANDARD, KING_OF_THE_ISLAND, KING_OF_THE_HILL, SURVIVAL, ZOMBIES, REGICIDE, CAPTURE_THE_FLAG, MAGIC_RAMPAGE, ISLAND_WARS };
        public GameMode(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

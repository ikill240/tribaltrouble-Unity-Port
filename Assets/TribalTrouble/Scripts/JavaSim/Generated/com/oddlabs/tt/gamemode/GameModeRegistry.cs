// Translated from GameModeRegistry.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode
{
    // Java: 17
    public class GameModeRegistry : java.lang.Object
    {
        public static java.util.Map REGISTRY;
        // Java: 36
        public static com.oddlabs.tt.gamemode.GameModeRules get(com.oddlabs.matchmaking.GameMode mode)
        {
            com.oddlabs.tt.gamemode.GameModeRules rules = ((com.oddlabs.tt.gamemode.GameModeRules)com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.get(mode));
            if (rules == null)
            {
                throw new System.InvalidOperationException("No rules registered for mode: " + java.lang.JStr.valueOf(mode));
            }
            return rules;
        }
        // Java: 44
        public static bool isRegistered(com.oddlabs.matchmaking.GameMode mode)
        {
            return com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.containsKey(mode);
        }
        static GameModeRegistry()
        {
            GameModeRegistry.REGISTRY = new java.util.EnumMap(typeof(com.oddlabs.matchmaking.GameMode));
            {
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.STANDARD, new com.oddlabs.tt.gamemode.standard.StandardModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND, new com.oddlabs.tt.gamemode.koth.KingOfTheIslandModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.KING_OF_THE_HILL, new com.oddlabs.tt.gamemode.koth.KingOfTheHillModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.SURVIVAL, new com.oddlabs.tt.gamemode.survival.SurvivalModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.ZOMBIES, new com.oddlabs.tt.gamemode.zombies.ZombieModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.REGICIDE, new com.oddlabs.tt.gamemode.regicide.RegicideModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.CAPTURE_THE_FLAG, new com.oddlabs.tt.gamemode.ctf.CaptureTheFlagModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.MAGIC_RAMPAGE, new com.oddlabs.tt.gamemode.rampage.MagicRampageModeRules());
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.ISLAND_WARS, new com.oddlabs.tt.islandwars.IslandWarsRules());
            }
        }
        public GameModeRegistry() : base()
        {
        }
    }
}

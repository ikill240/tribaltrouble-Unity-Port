// Translated from GameModeRegistry.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode
{
    // Java: 14
    public class GameModeRegistry : java.lang.Object
    {
        public static java.util.Map REGISTRY;
        // Java: 25
        public static com.oddlabs.tt.gamemode.GameModeRules get(com.oddlabs.matchmaking.GameMode mode)
        {
            com.oddlabs.tt.gamemode.GameModeRules rules = ((com.oddlabs.tt.gamemode.GameModeRules)com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.get(mode));
            if (rules == null)
            {
                throw new System.InvalidOperationException("No rules registered for mode: " + java.lang.JStr.valueOf(mode));
            }
            return rules;
        }
        // Java: 33
        public static bool isRegistered(com.oddlabs.matchmaking.GameMode mode)
        {
            return com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.containsKey(mode);
        }
        static GameModeRegistry()
        {
            GameModeRegistry.REGISTRY = new java.util.EnumMap(typeof(com.oddlabs.matchmaking.GameMode));
            {
                com.oddlabs.tt.gamemode.GameModeRegistry.REGISTRY.put(com.oddlabs.matchmaking.GameMode.STANDARD, new com.oddlabs.tt.gamemode.standard.StandardModeRules());
            }
        }
        public GameModeRegistry() : base()
        {
        }
    }
}

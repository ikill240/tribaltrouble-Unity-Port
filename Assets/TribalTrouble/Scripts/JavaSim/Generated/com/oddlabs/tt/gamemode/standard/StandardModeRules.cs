// Translated from StandardModeRules.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gamemode.standard
{
    // Java: 11
    public class StandardModeRules : java.lang.Object, com.oddlabs.tt.gamemode.GameModeRules
    {
        public const string OPTION_RATED = "rated";
        public static java.util.List OPTIONS;
        // Java: 18
        public java.util.List getOptions()
        {
            return com.oddlabs.tt.gamemode.standard.StandardModeRules.OPTIONS;
        }
        // Java: 23
        public bool isPlayerAlive(com.oddlabs.tt.player.Player player)
        {
            int units = player.getUnitCountContainer().getNumSupplies();
            return units > 0 || player.hasActiveChieftain() || player.getQuarters() != null;
        }
        // Java: 29
        public void onGameStart(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
        }
        static StandardModeRules()
        {
            StandardModeRules.OPTIONS = java.util.List_.of(new com.oddlabs.matchmaking.GameModeOption(com.oddlabs.tt.gamemode.standard.StandardModeRules.OPTION_RATED, com.oddlabs.matchmaking.GameModeOption.Type.BOOL, false, "rated_game"));
        }
        public StandardModeRules() : base()
        {
        }
        // mixin of default method GameModeRules.onWorldStart
        public void onWorldStart(com.oddlabs.tt.landscape.World world, java.util.function.Consumer status)
        {
        }
        // mixin of default method GameModeRules.describe
        public string describe()
        {
            return "";
        }
    }
}

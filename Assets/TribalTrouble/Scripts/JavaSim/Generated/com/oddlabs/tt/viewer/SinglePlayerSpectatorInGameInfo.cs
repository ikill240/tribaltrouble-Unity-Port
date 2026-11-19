// Translated from SinglePlayerSpectatorInGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 13
    public class SinglePlayerSpectatorInGameInfo : com.oddlabs.tt.viewer.SpectatorInGameInfo
    {
        // Java: 19
        public override bool isMultiplayer()
        {
            return false;
        }
        public SinglePlayerSpectatorInGameInfo(float random_start_position) : base(random_start_position)
        {
        }
    }
}

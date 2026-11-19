// Translated from SpectatorInGameInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 7
    public class SpectatorInGameInfo : com.oddlabs.tt.viewer.DefaultInGameInfo
    {
        public float random_start_position;
        // Java: 14
        public override bool isMultiplayer()
        {
            return true;
        }
        // Java: 19
        public override float getRandomStartPosition()
        {
            return random_start_position;
        }
        public SpectatorInGameInfo(float random_start_position) : base()
        {
            this.random_start_position = random_start_position;
        }
    }
}

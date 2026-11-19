// Translated from GameTime.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.aikit
{
    // Java: 11
    public class GameTime : java.lang.Object
    {
        public const int TICKS_PER_SECOND = 50;
        // Java: 18
        public static double seconds(com.oddlabs.tt.landscape.World world)
        {
            return world.getTick() / ((double)(com.oddlabs.tt.aikit.GameTime.TICKS_PER_SECOND));
        }
        public GameTime() : base()
        {
        }
    }
}

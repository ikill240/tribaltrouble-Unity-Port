// Translated from GameStartedTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 7
    public class GameStartedTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public java.lang.Runnable runnable;
        // Java: 15
        public override void check()
        {
            triggered();
        }
        // Java: 20
        public override void done()
        {
            runnable.run();
        }
        public GameStartedTrigger(com.oddlabs.tt.landscape.World world, java.lang.Runnable runnable) : base(world, 0.25f, 0.0f)
        {
            this.runnable = runnable;
        }
    }
}

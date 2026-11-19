// Translated from TimeTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 7
    public class TimeTrigger : com.oddlabs.tt.trigger.IntervalTrigger
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
        public TimeTrigger(com.oddlabs.tt.landscape.World world, float time, java.lang.Runnable runnable) : base(time, 0.0f, world.getAnimationManagerGameTime())
        {
            this.runnable = runnable;
        }
    }
}

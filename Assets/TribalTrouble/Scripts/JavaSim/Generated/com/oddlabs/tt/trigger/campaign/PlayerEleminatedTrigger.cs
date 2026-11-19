// Translated from PlayerEleminatedTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 7
    public class PlayerEleminatedTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public java.lang.Runnable runnable;
        public com.oddlabs.tt.player.Player player;
        // Java: 17
        public override void check()
        {
            int units = player.getUnitCountContainer().getNumSupplies();
            if (units == 0 && !player.hasActiveChieftain())
            {
                triggered();
            }
        }
        // Java: 25
        public override void done()
        {
            runnable.run();
        }
        public PlayerEleminatedTrigger(java.lang.Runnable runnable, com.oddlabs.tt.player.Player player) : base(player.getWorld(), 0.5f, 0.0f)
        {
            this.runnable = runnable;
            this.player = player;
        }
    }
}

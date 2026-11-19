// Translated from VictoryTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 8
    public class VictoryTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public java.lang.Runnable runnable;
        // Java: 18
        public override void check()
        {
            com.oddlabs.tt.player.Player[] players = viewer.getWorld().getPlayers();
            com.oddlabs.tt.player.Player local = viewer.getLocalPlayer();
            foreach (com.oddlabs.tt.player.Player current in players)
            {
                if (local.isEnemy(current))
                {
                    int units = current.getUnitCountContainer().getNumSupplies();
                    if (units > 0 || current.hasActiveChieftain())
                    {
                        return;
                    }
                }
            }
            triggered();
        }
        // Java: 34
        public override void done()
        {
            runnable.run();
        }
        public VictoryTrigger(com.oddlabs.tt.viewer.WorldViewer viewer, java.lang.Runnable runnable) : base(viewer.getWorld(), 0.5f, 0.0f)
        {
            this.viewer = viewer;
            this.runnable = runnable;
        }
    }
}

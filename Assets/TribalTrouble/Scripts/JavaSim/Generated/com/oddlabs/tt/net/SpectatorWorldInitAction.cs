// Translated from SpectatorWorldInitAction.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public class SpectatorWorldInitAction : java.lang.Object, com.oddlabs.tt.net.WorldInitAction
    {
        // Java: 7
        public void run(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.@delegate.Menu.completeGameSetupHack(viewer);
            viewer.getDelegate().setObserverMode();
        }
        public SpectatorWorldInitAction() : base()
        {
        }
    }
}

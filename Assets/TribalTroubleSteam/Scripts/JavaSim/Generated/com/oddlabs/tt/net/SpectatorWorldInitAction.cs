// Translated from SpectatorWorldInitAction.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 6
    public class SpectatorWorldInitAction : java.lang.Object, com.oddlabs.tt.net.WorldInitAction
    {
        public int followed_slot;
        // Java: 13
        public void run(com.oddlabs.tt.viewer.WorldViewer viewer)
        {
            com.oddlabs.tt.@delegate.Menu.completeGameSetupHack(viewer);
            viewer.getDelegate().setObserverMode();
            if (viewer.getSpectatorView() != null)
            {
                viewer.getSpectatorView().follow(followed_slot);
            }
        }
        public SpectatorWorldInitAction(int followed_slot) : base()
        {
            this.followed_slot = followed_slot;
        }
    }
}

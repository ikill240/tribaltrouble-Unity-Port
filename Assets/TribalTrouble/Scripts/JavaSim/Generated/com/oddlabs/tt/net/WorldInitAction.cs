// Translated from WorldInitAction.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 5
    public interface WorldInitAction
    {
        // Java: 6
        void run(com.oddlabs.tt.viewer.WorldViewer viewer);
    }
    public sealed class WorldInitAction_L : java.lang.Object, WorldInitAction
    {
        readonly System.Action<com.oddlabs.tt.viewer.WorldViewer> f;
        public WorldInitAction_L(System.Action<com.oddlabs.tt.viewer.WorldViewer> f) { this.f = f; }
        public void run(com.oddlabs.tt.viewer.WorldViewer p0) { f(p0); }
    }
}

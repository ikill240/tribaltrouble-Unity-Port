// Translated from LoadCallback.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 7
    public interface LoadCallback
    {
        // Java: 9
        com.oddlabs.tt.render.UIRenderer load(com.oddlabs.tt.gui.GUIRoot gui_root);
    }
    public sealed class LoadCallback_L : java.lang.Object, LoadCallback
    {
        readonly System.Func<com.oddlabs.tt.gui.GUIRoot, com.oddlabs.tt.render.UIRenderer> f;
        public LoadCallback_L(System.Func<com.oddlabs.tt.gui.GUIRoot, com.oddlabs.tt.render.UIRenderer> f) { this.f = f; }
        public com.oddlabs.tt.render.UIRenderer load(com.oddlabs.tt.gui.GUIRoot p0) { return f(p0); }
    }
}

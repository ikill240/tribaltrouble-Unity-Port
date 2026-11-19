// Translated from Fadable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public interface Fadable
    {
        // Java: 4
        void fadingDone();
    }
    public sealed class Fadable_L : java.lang.Object, Fadable
    {
        readonly System.Action f;
        public Fadable_L(System.Action f) { this.f = f; }
        public void fadingDone() { f(); }
    }
}

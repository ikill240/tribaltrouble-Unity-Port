// Translated from Updatable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.animation
{
    // Java: 5
    public interface Updatable
    {
        // Java: 7
        void update(object anim);
    }
    public sealed class Updatable_L : java.lang.Object, Updatable
    {
        readonly System.Action<object> f;
        public Updatable_L(System.Action<object> f) { this.f = f; }
        public void update(object p0) { f(p0); }
    }
}

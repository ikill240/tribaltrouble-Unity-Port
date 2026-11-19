// Translated from RouterListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 5
    public interface RouterListener
    {
        // Java: 6
        void routerFailed(System.IO.IOException e);
    }
    public sealed class RouterListener_L : java.lang.Object, RouterListener
    {
        readonly System.Action<System.IO.IOException> f;
        public RouterListener_L(System.Action<System.IO.IOException> f) { this.f = f; }
        public void routerFailed(System.IO.IOException p0) { f(p0); }
    }
}

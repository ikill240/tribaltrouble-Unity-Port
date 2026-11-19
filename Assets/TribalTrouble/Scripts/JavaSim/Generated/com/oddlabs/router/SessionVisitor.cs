// Translated from SessionVisitor.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 3
    public interface SessionVisitor
    {
        // Java: 4
        void visit(com.oddlabs.router.RouterClient client);
    }
    public sealed class SessionVisitor_L : java.lang.Object, SessionVisitor
    {
        readonly System.Action<com.oddlabs.router.RouterClient> f;
        public SessionVisitor_L(System.Action<com.oddlabs.router.RouterClient> f) { this.f = f; }
        public void visit(com.oddlabs.router.RouterClient p0) { f(p0); }
    }
}

// Translated from ARMIEventWriter.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 3
    public interface ARMIEventWriter
    {
        // Java: 4
        void handle(com.oddlabs.net.ARMIEvent @event);
    }
    public sealed class ARMIEventWriter_L : java.lang.Object, ARMIEventWriter
    {
        readonly System.Action<com.oddlabs.net.ARMIEvent> f;
        public ARMIEventWriter_L(System.Action<com.oddlabs.net.ARMIEvent> f) { this.f = f; }
        public void handle(com.oddlabs.net.ARMIEvent p0) { f(p0); }
    }
}

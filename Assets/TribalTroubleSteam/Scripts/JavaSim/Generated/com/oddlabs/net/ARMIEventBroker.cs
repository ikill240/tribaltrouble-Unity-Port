// Translated from ARMIEventBroker.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 3
    public interface ARMIEventBroker
    {
        // Java: 4
        void handle(object sender, com.oddlabs.net.ARMIEvent @event);
    }
    public sealed class ARMIEventBroker_L : java.lang.Object, ARMIEventBroker
    {
        readonly System.Action<object, com.oddlabs.net.ARMIEvent> f;
        public ARMIEventBroker_L(System.Action<object, com.oddlabs.net.ARMIEvent> f) { this.f = f; }
        public void handle(object p0, com.oddlabs.net.ARMIEvent p1) { f(p0, p1); }
    }
}

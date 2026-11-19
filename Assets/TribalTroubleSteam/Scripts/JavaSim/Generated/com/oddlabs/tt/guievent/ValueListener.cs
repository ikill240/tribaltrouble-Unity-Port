// Translated from ValueListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 3
    public interface ValueListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 5
        void valueSet(long @value);
    }
    public sealed class ValueListener_L : java.lang.Object, ValueListener
    {
        readonly System.Action<long> f;
        public ValueListener_L(System.Action<long> f) { this.f = f; }
        public void valueSet(long p0) { f(p0); }
    }
}

// Translated from ErrorListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 3
    public interface ErrorListener
    {
        // Java: 4
        void connectionLost();
    }
    public sealed class ErrorListener_L : java.lang.Object, ErrorListener
    {
        readonly System.Action f;
        public ErrorListener_L(System.Action f) { this.f = f; }
        public void connectionLost() { f(); }
    }
}

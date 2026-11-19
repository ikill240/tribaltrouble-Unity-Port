// Translated from ChatMethod.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 5
    public interface ChatMethod
    {
        // Java: 7
        void execute(com.oddlabs.tt.gui.InfoPrinter info_printer, string text);
    }
    public sealed class ChatMethod_L : java.lang.Object, ChatMethod
    {
        readonly System.Action<com.oddlabs.tt.gui.InfoPrinter, string> f;
        public ChatMethod_L(System.Action<com.oddlabs.tt.gui.InfoPrinter, string> f) { this.f = f; }
        public void execute(com.oddlabs.tt.gui.InfoPrinter p0, string p1) { f(p0, p1); }
    }
}

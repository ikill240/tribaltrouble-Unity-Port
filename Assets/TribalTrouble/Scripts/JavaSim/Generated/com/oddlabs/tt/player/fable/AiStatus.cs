// Translated from AiStatus.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player.fable
{
    // Java: 6
    public interface AiStatus
    {
        // Java: 7
        string debugStatus();
    }
    public sealed class AiStatus_L : java.lang.Object, AiStatus
    {
        readonly System.Func<string> f;
        public AiStatus_L(System.Func<string> f) { this.f = f; }
        public string debugStatus() { return f(); }
    }
}

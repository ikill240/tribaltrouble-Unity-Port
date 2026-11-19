// Translated from PresetDeleteListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface PresetDeleteListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 8
        void presetDeleted(com.oddlabs.matchmaking.Preset preset);
    }
    public sealed class PresetDeleteListener_L : java.lang.Object, PresetDeleteListener
    {
        readonly System.Action<com.oddlabs.matchmaking.Preset> f;
        public PresetDeleteListener_L(System.Action<com.oddlabs.matchmaking.Preset> f) { this.f = f; }
        public void presetDeleted(com.oddlabs.matchmaking.Preset p0) { f(p0); }
    }
}

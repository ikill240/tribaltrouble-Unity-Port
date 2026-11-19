// Translated from PresetChosenListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.guievent
{
    // Java: 6
    public interface PresetChosenListener : com.oddlabs.tt.guievent.EventListener
    {
        // Java: 8
        void presetChosen(com.oddlabs.matchmaking.Preset preset);
    }
    public sealed class PresetChosenListener_L : java.lang.Object, PresetChosenListener
    {
        readonly System.Action<com.oddlabs.matchmaking.Preset> f;
        public PresetChosenListener_L(System.Action<com.oddlabs.matchmaking.Preset> f) { this.f = f; }
        public void presetChosen(com.oddlabs.matchmaking.Preset p0) { f(p0); }
    }
}

// Translated from ModeAndPresetsHandler.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 12
    public interface ModeAndPresetsHandler
    {
        // Java: 13
        void modeChosen(com.oddlabs.matchmaking.GameMode mode);
        // Java: 15
        void presetChosen(com.oddlabs.matchmaking.Preset preset);
        // Java: 17
        void presetDeleted(com.oddlabs.matchmaking.Preset preset);
        // Java: 19
        void saveClicked();
        // Java: 21
        void resetClicked();
        // Java: 23
        void updateClicked();
    }
}

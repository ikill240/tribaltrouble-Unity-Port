// Translated from MapCameraOwner.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.camera
{
    // Java: 8
    public interface MapCameraOwner
    {
        // Java: 9
        com.oddlabs.tt.gui.GUIRoot getGUIRoot();
        // Java: 12
        void addChild(com.oddlabs.tt.gui.GUIObject child);
        // Java: 15
        void exitMapMode();
        // Java: 18
        float getMaxCameraZ();
    }
}

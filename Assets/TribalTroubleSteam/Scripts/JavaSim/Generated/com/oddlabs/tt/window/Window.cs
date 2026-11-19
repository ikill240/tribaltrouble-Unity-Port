// Translated from Window.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.window
{
    // Java: 6
    public interface Window : java.lang.AutoCloseable
    {
        // Java: 7
        void create(com.oddlabs.tt.render.SerializableDisplayMode mode, bool fullscreen);
        // Java: 9
        void close();
        // Java: 11
        void update();
        // Java: 13
        void pollEvents();
        // Java: 15
        bool isOpen();
        // Java: 17
        bool isCloseRequested();
        // Java: 19
        void setCloseRequested(bool @value);
        // Java: 21
        bool isActive();
        // Java: 23
        bool isVisible();
        // Java: 25
        bool isIconified();
        // Java: 27
        bool wasResized();
        // Java: 35
        int getWidth();
        // Java: 42
        int getHeight();
        // Java: 51
        int getLogicalWidth();
        // Java: 58
        int getLogicalHeight();
        // Java: 60
        void setTitle(string title);
        // Java: 62
        void setVSyncEnabled(bool enabled);
        // Java: 64
        void setFullscreen(bool fullscreen);
        // Java: 66
        com.oddlabs.tt.render.SerializableDisplayMode[] getAvailableDisplayModes();
        // Java: 69
        com.oddlabs.tt.render.SerializableDisplayMode getDisplayMode();
        // Java: 72
        void setDisplayMode(com.oddlabs.tt.render.SerializableDisplayMode mode);
        // Java: 74
        void setIcon(java.nio.file.Path imagePath);
        // Java: 76
        void restore();
        // Java: 78
        void minimize();
        // Java: 80
        void show();
        // Java: 82
        void focus();
        // Java: 84
        void makeCurrent();
        // Java: 86
        bool isFullscreen();
        // Java: 93
        int[] getMonitorPhysicalSize();
        // Java: 100
        float[] getMonitorContentScale();
        // Java: 107
        float[] getWindowContentScale();
    }
}

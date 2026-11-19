// Translated from InputProvider.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.input
{
    // Java: 5
    public interface InputProvider : java.lang.AutoCloseable
    {
        // Java: 7
        void pollKeyboard();
        // Java: 9
        bool nextKeyboardEvent();
        // Java: 11
        int getEventKey();
        // Java: 16
        bool getEventKeyState();
        // Java: 18
        int getEventKeyMods();
        // Java: 20
        char getEventCharacter();
        // Java: 22
        bool isRepeatEvent();
        // Java: 24
        bool isKeyDown(int keyCode);
        // Java: 27
        void pollMouse();
        // Java: 29
        bool nextMouseEvent();
        // Java: 31
        int getEventButton();
        // Java: 33
        bool getEventButtonState();
        // Java: 35
        int getEventDWheel();
        // Java: 37
        int getEventX();
        // Java: 39
        int getEventY();
        // Java: 43
        int getMouseX();
        // Java: 45
        int getMouseY();
        // Java: 47
        bool isButtonDown(int button);
        // Java: 49
        void setCursorPosition(int x, int y);
        // Java: 51
        void setGrabbed(bool grabbed);
        // Java: 53
        bool isGrabbed();
        // Java: 56
        bool isCursorInWindow();
        // Java: 59
        void setNativeCursor(object cursor);
        // Java: 61
        void close();
    }
}

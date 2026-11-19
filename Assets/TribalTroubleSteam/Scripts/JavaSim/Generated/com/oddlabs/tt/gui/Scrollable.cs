// Translated from Scrollable.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public interface Scrollable
    {
        // Java: 4
        void setOffsetY(int new_offset);
        // Java: 6
        int getOffsetY();
        // Java: 8
        int getStepHeight();
        // Java: 10
        void jumpPage(bool up);
        // Java: 12
        float getScrollBarRatio();
        // Java: 14
        float getScrollBarOffset();
        // Java: 16
        void setScrollBarOffset(float offset);
    }
}

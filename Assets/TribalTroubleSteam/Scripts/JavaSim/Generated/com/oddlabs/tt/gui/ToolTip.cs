// Translated from ToolTip.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 3
    public interface ToolTip
    {
        // Java: 4
        void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip);
        // Java: 6
        bool hasToolTip();
    }
    public sealed class ToolTip_L : java.lang.Object, ToolTip
    {
        readonly System.Action<com.oddlabs.tt.gui.ToolTipBox> f;
        public ToolTip_L(System.Action<com.oddlabs.tt.gui.ToolTipBox> f) { this.f = f; }
        public void appendToolTip(com.oddlabs.tt.gui.ToolTipBox p0) { f(p0); }
        public bool hasToolTip()
        {
            return true;
        }
    }
}

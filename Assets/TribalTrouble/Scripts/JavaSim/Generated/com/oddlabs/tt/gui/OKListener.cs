// Translated from OKListener.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class OKListener : java.lang.Object, com.oddlabs.tt.guievent.MouseClickListener
    {
        public com.oddlabs.tt.gui.Form form;
        // Java: 13
        public void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            form.remove();
        }
        public OKListener(com.oddlabs.tt.gui.Form form) : base()
        {
            this.form = form;
        }
    }
}

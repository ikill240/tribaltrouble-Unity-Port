// Translated from InGameCampaignDialogForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 9
    public class InGameCampaignDialogForm : com.oddlabs.tt.form.CampaignDialogForm
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        public InGameCampaignDialogForm(com.oddlabs.tt.viewer.WorldViewer viewer, string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align) : this(viewer, header, text, image, align, null)
        {
        }
        public InGameCampaignDialogForm(com.oddlabs.tt.viewer.WorldViewer viewer, string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align, java.lang.Runnable runnable) : this(viewer, header, text, image, align, runnable, false)
        {
        }
        public InGameCampaignDialogForm(com.oddlabs.tt.viewer.WorldViewer viewer, string header, string text, com.oddlabs.tt.gui.IconQuad image, com.oddlabs.tt.gui.Origin align, java.lang.Runnable runnable, bool cancel_v) : base(header, text, image, align, runnable, cancel_v)
        {
            this.viewer = viewer;
            viewer.setPaused(true);
            addCloseListener(new com.oddlabs.tt.guievent.CloseListener_L(() =>
            {
                viewer.setPaused(false);
            }));
        }
    }
}

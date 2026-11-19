// Translated from MessageForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 16
    public class MessageForm : com.oddlabs.tt.gui.Form
    {
        public const int MAX_WIDTH = 500;
        public MessageForm(string head, string message) : this(head, message, null, null)
        {
        }
        public MessageForm(string head, string message, string button, com.oddlabs.tt.guievent.MouseClickListener listener) : base()
        {
            int head_width = java.lang.Math.min(com.oddlabs.tt.form.MessageForm.MAX_WIDTH, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont().getWidth(head));
            int message_width = java.lang.Math.min(com.oddlabs.tt.form.MessageForm.MAX_WIDTH, com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getWidth(message));
            int width = java.lang.Math.max(head_width, message_width);
            com.oddlabs.tt.gui.LabelBox head_label = new com.oddlabs.tt.gui.LabelBox(head, com.oddlabs.tt.gui.Skin.getSkin().getHeadlineFont(), width);
            addChild(head_label);
            com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox(message, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), width);
            addChild(info_label);
            com.oddlabs.tt.gui.HorizButton ok_button = default;
            if (button == null)
            {
                ok_button = new com.oddlabs.tt.gui.OKButton(70);
                ok_button.addMouseClickListener(new com.oddlabs.tt.gui.OKListener(this));
            }
            else
            {
                ok_button = new com.oddlabs.tt.gui.HorizButton(button, 70);
                ok_button.addMouseClickListener(listener);
            }
            addChild(ok_button);
            head_label.place();
            info_label.place(head_label, com.oddlabs.tt.gui.Placement.BOTTOM_LEFT);
            ok_button.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
        }
        public MessageForm(string message) : base()
        {
            int width = java.lang.Math.min(500, com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getWidth(message));
            com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox(message, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), width);
            addChild(info_label);
            com.oddlabs.tt.gui.HorizButton ok_button = new com.oddlabs.tt.gui.OKButton(70);
            addChild(ok_button);
            ok_button.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            ok_button.addMouseClickListener(new com.oddlabs.tt.gui.OKListener(this));
            info_label.place();
            compileCanvas();
            centerPos();
        }
    }
}

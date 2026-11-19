// Translated from QuestionForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 18
    public class QuestionForm : com.oddlabs.tt.gui.Form
    {
        public com.oddlabs.tt.gui.HorizButton yes_button;
        public java.lang.Runnable no_action;
        // Java: 53
        public override void setFocus()
        {
            yes_button.setFocus();
        }
        // Java: 58
        public override void doCancel()
        {
            if (no_action != null)
            {
                no_action.run();
            }
        }
        // Java: 65
        public void connectionLost()
        {
            remove();
        }
        public QuestionForm(string message, com.oddlabs.tt.guievent.MouseClickListener yes_action) : this(message, yes_action, null)
        {
        }
        public QuestionForm(string message, com.oddlabs.tt.guievent.MouseClickListener yes_action, java.lang.Runnable no_action) : base()
        {
            this.no_action = no_action;
            int message_width = com.oddlabs.tt.gui.Skin.getSkin().getEditFont().getWidth(message);
            com.oddlabs.tt.gui.LabelBox info_label = new com.oddlabs.tt.gui.LabelBox(message, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), java.lang.Math.min(400, message_width));
            addChild(info_label);
            com.oddlabs.tt.gui.Group button_group = new com.oddlabs.tt.gui.Group();
            yes_button = new com.oddlabs.tt.gui.OKButton(80);
            yes_button.addMouseClickListener(new com.oddlabs.tt.gui.OKListener(this));
            yes_button.addMouseClickListener(yes_action);
            button_group.addChild(yes_button);
            com.oddlabs.tt.gui.HorizButton no_button = new com.oddlabs.tt.gui.CancelButton(80);
            no_button.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton unused, int unused_2, int unused_3, int unused_4) =>
            {
                this.cancel();
            }));
            button_group.addChild(no_button);
            yes_button.place();
            no_button.place(yes_button, com.oddlabs.tt.gui.Placement.RIGHT_MID);
            button_group.compileCanvas();
            addChild(button_group);
            info_label.place();
            button_group.place(info_label, com.oddlabs.tt.gui.Placement.BOTTOM_MID);
            compileCanvas();
            centerPos();
        }
    }
}

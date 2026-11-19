// Translated from IconSpinnerButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class IconSpinnerButton : com.oddlabs.tt.gui.NonFocusIconButton
    {
        public com.oddlabs.tt.gui.IconSpinner owner;
        // Java: 19
        public override bool hasToolTip()
        {
            return (isDisabled() ? owner.hasToolTip() : base.hasToolTip());
        }
        // Java: 24
        public override void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            if (isDisabled())
            {
                owner.appendToolTip(tool_tip_box);
            }
            else
            {
                base.appendToolTip(tool_tip_box);
            }
        }
        public IconSpinnerButton(com.oddlabs.tt.gui.ModeIconQuads icon_quad, com.oddlabs.tt.input.GameAction action, java.util.function.Supplier tool_tip, com.oddlabs.tt.gui.IconSpinner owner) : base(icon_quad, action, tool_tip)
        {
            this.owner = owner;
        }
    }
}

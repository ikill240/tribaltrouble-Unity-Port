// Translated from RadioButtonGroup.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class RadioButtonGroup : java.lang.Object
    {
        public java.util.List buttons;
        // Java: 15
        public void mark(com.oddlabs.tt.gui.RadioButtonGroupElement button)
        {
            com.oddlabs.tt.gui.RadioButtonGroupElement marked = getMarked();
            if (marked != null)
            {
                marked.setMarked(false);
            }
            button.setMarked(true);
        }
        // Java: 22
        public void add(com.oddlabs.tt.gui.RadioButtonGroupElement button)
        {
            buttons.add(button);
        }
        // Java: 27
        public com.oddlabs.tt.gui.RadioButtonGroupElement getMarked()
        {
            foreach (com.oddlabs.tt.gui.RadioButtonGroupElement button in java.lang.JIter.of(buttons))
            {
                if (button.isMarked())
                {
                    return button;
                }
            }
            return null;
        }
        private void init_RadioButtonGroup()
        {
            this.buttons = new java.util.ArrayList();
        }
        public RadioButtonGroup() : base()
        {
            init_RadioButtonGroup();
        }
    }
}

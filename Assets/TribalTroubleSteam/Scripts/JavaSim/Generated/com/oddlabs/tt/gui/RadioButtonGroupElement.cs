// Translated from RadioButtonGroupElement.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 5
    public abstract class RadioButtonGroupElement : com.oddlabs.tt.gui.GUIObject
    {
        public bool marked;
        public com.oddlabs.tt.gui.RadioButtonGroup group;
        // Java: 17
        public bool isMarked()
        {
            return marked;
        }
        // Java: 21
        public void setMarked(bool marked)
        {
            this.marked = marked;
        }
        // Java: 25
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            group.mark(this);
        }
        private void init_RadioButtonGroupElement()
        {
            this.marked = false;
        }
        public RadioButtonGroupElement(bool marked, com.oddlabs.tt.gui.RadioButtonGroup group) : base()
        {
            init_RadioButtonGroupElement();
            this.group = group;
            group.add(this);
            if (marked)
            {
                group.mark(this);
            }
        }
    }
}

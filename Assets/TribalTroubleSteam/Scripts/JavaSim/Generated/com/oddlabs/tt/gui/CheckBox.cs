// Translated from CheckBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class CheckBox : com.oddlabs.tt.gui.GUIObject
    {
        public java.util.List event_listeners;
        public bool marked;
        public bool pressed;
        // Java: 32
        public bool isMarked()
        {
            return marked;
        }
        // Java: 36
        public void setMarked(bool marked)
        {
            if (marked != this.marked)
            {
                this.marked = marked;
                checkedAll(marked);
            }
        }
        // Java: 43
        public void toggleMarked()
        {
            marked = !marked;
            checkedAll(marked);
        }
        // Java: 48
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            toggleMarked();
        }
        // Java: 53
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = false;
        }
        // Java: 58
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = true;
        }
        // Java: 63
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            com.oddlabs.tt.gui.ModeIconQuads quad_to_render = (isMarked() ? ((((object)skinMode == (object)com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE) && pressed && isHovered() ? com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxUnmarked() : com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxMarked())) : ((((object)skinMode == (object)com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE) && pressed && isHovered() ? com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxMarked() : com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxUnmarked())));
            renderer.drawModeIcon(quad_to_render, skinMode, 0, 0);
        }
        // Java: 77
        public void checkedAll(bool marked)
        {
            @checked(marked);
            foreach (com.oddlabs.tt.guievent.CheckBoxListener listener in java.lang.JIter.of(event_listeners))
            {
                listener.@checked(marked);
            }
        }
        // Java: 84
        public void @checked(bool marked)
        {
        }
        // Java: 92
        public void addCheckBoxListener(com.oddlabs.tt.guievent.CheckBoxListener listener)
        {
            event_listeners.add(listener);
        }
        // Java: 96
        public void removeCheckBoxListener(com.oddlabs.tt.guievent.CheckBoxListener listener)
        {
            event_listeners.remove(listener);
        }
        private void init_CheckBox()
        {
            this.event_listeners = new java.util.ArrayList();
            this.pressed = false;
        }
        public CheckBox(bool marked, string text) : this(marked, text, "")
        {
        }
        public CheckBox(bool marked, string text, string tool_tip) : base((tool_tip != null && !tool_tip.isEmpty() ? new java.util.function.Supplier_L(() =>
        {
            return tool_tip;
        }) : (java.util.function.Supplier)(null)))
        {
            init_CheckBox();
            this.marked = marked;
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(text, com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            addChild(label);
            label.setPos(com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxMarked().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth(), (com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxMarked().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight() - label.getHeight()) / 2);
            setDim(com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxMarked().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth() + label.getWidth(), com.oddlabs.tt.gui.Skin.getSkin().getCheckBoxMarked().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight());
            setCanFocus(true);
        }
    }
}

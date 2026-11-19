// Translated from RadioButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class RadioButton : com.oddlabs.tt.gui.RadioButtonGroupElement
    {
        public bool pressed;
        // Java: 21
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = false;
        }
        // Java: 26
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = true;
        }
        // Java: 31
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            com.oddlabs.tt.gui.IconQuad quad_to_render = (isMarked() ? com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonMarked().quad(skinMode) : (((object)skinMode == (object)com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE) && pressed && isHovered() ? com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonMarked().quad(skinMode) : com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonUnmarked().quad(skinMode)));
            renderer.drawIcon(quad_to_render, 0, 0);
        }
        private void init_RadioButton()
        {
            this.pressed = false;
        }
        public RadioButton(bool marked, com.oddlabs.tt.gui.RadioButtonGroup group, string text) : base(marked, group)
        {
            init_RadioButton();
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(text, com.oddlabs.tt.gui.Skin.getSkin().getEditFont());
            label.setPos(((com.oddlabs.tt.gui.IconQuad)com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonMarked().get(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)).getWidth(), (((com.oddlabs.tt.gui.IconQuad)com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonMarked().get(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)).getHeight() - label.getHeight()) / 2);
            addChild(label);
            setDim(((com.oddlabs.tt.gui.IconQuad)com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonMarked().get(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)).getWidth() + label.getWidth(), ((com.oddlabs.tt.gui.IconQuad)com.oddlabs.tt.gui.Skin.getSkin().getRadioButtonMarked().get(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)).getHeight());
            setCanFocus(true);
        }
    }
}

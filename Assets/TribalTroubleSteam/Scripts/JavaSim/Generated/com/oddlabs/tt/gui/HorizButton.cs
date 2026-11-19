// Translated from HorizButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class HorizButton : com.oddlabs.tt.gui.ButtonObject
    {
        // Java: 16
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isPressed() && isHovered() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)));
            com.oddlabs.tt.gui.Horizontal horizButton = (((object)skinMode == (object)com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE) && isPressed() && isHovered() ? com.oddlabs.tt.gui.Skin.getSkin().getHorizButtonPressed() : com.oddlabs.tt.gui.Skin.getSkin().getHorizButtonUnpressed());
            horizButton.render(renderer, 0, 0, getWidth(), skinMode);
        }
        public HorizButton(string caption, int width) : base(com.oddlabs.tt.gui.Skin.getSkin().getButtonFont())
        {
            setDim(width, com.oddlabs.tt.gui.Skin.getSkin().getHorizButtonPressed().getHeight());
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(caption, getFont());
            label.setPos((width - label.getWidth()) / 2, (com.oddlabs.tt.gui.Skin.getSkin().getHorizButtonPressed().getHeight() - getFont().getHeight()) / 2);
            addChild(label);
        }
    }
}

// Translated from SliderButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class SliderButton : com.oddlabs.tt.gui.ButtonObject
    {
        public com.oddlabs.tt.gui.Slider slider;
        public com.oddlabs.tt.gui.ModeIconQuads button;
        // Java: 20
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.GUIObject parent = ((com.oddlabs.tt.gui.GUIObject)getParent());
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (parent.isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isHovered() || parent.isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            renderer.drawModeIcon(button, skinMode, 0, 0);
        }
        // Java: 29
        public override void mouseHeld(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
        }
        // Java: 33
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT))
                {
                    slider.setValue(slider.getValue() + 1);
                    return;
                }
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT))
                {
                    slider.setValue(slider.getValue() - 1);
                    return;
                }
            }
            @event.consume();
        }
        public SliderButton(com.oddlabs.tt.gui.Slider slider, com.oddlabs.tt.gui.ModeIconQuads button) : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont())
        {
            setDim(button.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth(), button.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight());
            this.slider = slider;
            this.button = button;
        }
    }
}

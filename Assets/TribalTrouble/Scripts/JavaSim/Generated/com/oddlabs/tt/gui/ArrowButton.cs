// Translated from ArrowButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 9
    public class ArrowButton : com.oddlabs.tt.gui.ButtonObject
    {
        public com.oddlabs.tt.gui.ModeIconQuads pressed;
        public com.oddlabs.tt.gui.ModeIconQuads unpressed;
        public com.oddlabs.tt.gui.ModeIconQuads arrow;
        // Java: 22
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
            {
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED))
                {
                    mousePressedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0);
                }
                else if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
                {
                    mouseReleasedAll(com.oddlabs.tt.gui.MouseButton.LEFT, 0, 0);
                }
                return;
            }
            if (@event.hasAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT) || @event.hasAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
            {
                return;
            }
            @event.consume();
        }
        // Java: 42
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isPressed() && isHovered() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)));
            com.oddlabs.tt.gui.ModeIconQuads quad_to_render_button = ((!isDisabled() && isPressed() && isHovered() ? pressed : unpressed));
            renderer.drawModeIcon(quad_to_render_button, skinMode, 0, 0);
            renderer.drawModeIcon(arrow, skinMode, 0, 0);
        }
        // Java: 54
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
        }
        public ArrowButton(com.oddlabs.tt.gui.ModeIconQuads pressed, com.oddlabs.tt.gui.ModeIconQuads unpressed, com.oddlabs.tt.gui.ModeIconQuads arrow) : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont())
        {
            setDim(pressed.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getWidth(), pressed.quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL).getHeight());
            this.pressed = pressed;
            this.unpressed = unpressed;
            this.arrow = arrow;
        }
    }
}

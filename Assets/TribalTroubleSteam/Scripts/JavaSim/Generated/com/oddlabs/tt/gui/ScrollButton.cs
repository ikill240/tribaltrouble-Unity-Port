// Translated from ScrollButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 8
    public class ScrollButton : com.oddlabs.tt.gui.GUIObject
    {
        // Java: 14
        public void setupPos(com.oddlabs.tt.gui.ScrollBar owner)
        {
            setPos(owner.getButtonX(), owner.getButtonY());
            setDim(com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollButton().getWidth(), owner.getButtonHeight());
        }
        // Java: 19
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (@event.hasAction(com.oddlabs.tt.input.GameAction.UI_FOCUS_NEXT) || @event.hasAction(com.oddlabs.tt.input.GameAction.UI_CANCEL))
            {
                return;
            }
            @event.consume();
        }
        // Java: 29
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            com.oddlabs.tt.gui.Skin.getSkin().getScrollBarData().scrollButton().render(renderer, 0, 0, getHeight(), skinMode);
        }
        // Java: 36
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
        }
        public ScrollButton() : base()
        {
            setCanFocus(true);
        }
    }
}

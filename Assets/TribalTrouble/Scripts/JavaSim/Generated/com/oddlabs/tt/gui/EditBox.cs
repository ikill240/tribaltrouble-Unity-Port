// Translated from EditBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 13
    public class EditBox : com.oddlabs.tt.gui.TextBox
    {
        public int index;
        // Java: 21
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            base.renderBox(renderer, (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            org.joml.Vector4fc c = (isDisabled() ? com.oddlabs.tt.gui.Label.DISABLED_COLOR : com.oddlabs.util.Color.WHITE);
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, getTextLayout(), edit_box.getLeftOffset(), getHeight() - edit_box.getBottomOffset() - getFont().getHeight() + getOffsetY(), edit_box.getLeftOffset(), getWidth() - edit_box.getRightOffset(), c);
            if (isActive())
            {
                com.oddlabs.tt.font.TextLayout layout = getTextLayout();
                int cursorLine = layout.getCursorLine(index);
                int cursorX = layout.getCursorX(index);
                int cursorY = getHeight() - edit_box.getBottomOffset() - getFont().getHeight() - (cursorLine * getFont().getHeight()) + getOffsetY();
                com.oddlabs.tt.font.Index.renderIndex(renderer, edit_box.getLeftOffset() + cursorX, cursorY, getFont(), c);
            }
        }
        // Java: 40
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                bool consumed = true;
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
                {
                    if (insert(index, '\n'))
                    {
                        index++;
                    }
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT))
                {
                    if (index > 0)
                    {
                        index--;
                    }
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT))
                {
                    if (index < getText().length())
                    {
                        index++;
                    }
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_UP))
                {
                    int currentLine = getTextLayout().getCursorLine(index);
                    if (currentLine > 0)
                    {
                        int xPos = getTextLayout().getCursorX(index);
                        index = getTextLayout().getCharacterIndexAt(xPos, (float)(((float)(currentLine - 0.5f)) * getFont().getHeight()), getTextLayout().getTextHeight());
                    }
                    else
                    {
                        index = 0;
                    }
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN))
                {
                    int currentLine_2 = getTextLayout().getCursorLine(index);
                    if (currentLine_2 < getTextLayout().getLines().size() - 1)
                    {
                        int xPos_2 = getTextLayout().getCursorX(index);
                        index = getTextLayout().getCharacterIndexAt(xPos_2, (float)(((float)(currentLine_2 + 1.5f)) * getFont().getHeight()), getTextLayout().getTextHeight());
                    }
                    else
                    {
                        index = getText().length();
                    }
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_HOME))
                {
                    index = getTextLayout().getLineStartCharIndex(getTextLayout().getCursorLine(index));
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_END))
                {
                    index = getTextLayout().getLineEndCharIndex(getTextLayout().getCursorLine(index));
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_BACKSPACE))
                {
                    if (index > 0)
                    {
                        delete(--index);
                    }
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_DELETE))
                {
                    if (index < getText().length())
                    {
                        delete(index);
                    }
                }
                else if (!@event.isControlDown() && !@event.isMetaDown() && !@event.isAltDown())
                {
                    char key = @event.getCharacter();
                    if (key != 0 && !java.lang.Character.isISOControl(key) && getFont().getQuad(key) != null)
                    {
                        if (insert(index, key))
                        {
                            index++;
                        }
                    }
                    else
                    {
                        consumed = false;
                    }
                }
                else
                {
                    consumed = false;
                }
                if (consumed)
                {
                    correctOffsetY();
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 98
        public void correctOffsetY()
        {
            com.oddlabs.tt.font.TextLayout layout = getTextLayout();
            int cursorLine = layout.getCursorLine(index);
            int lineHeight = getFont().getHeight();
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            int visibleHeight = getHeight() - edit_box.getTopOffset() - edit_box.getBottomOffset();
            int visibleLines = visibleHeight / lineHeight;
            int currentTopLine = -getOffsetY() / lineHeight;
            if (cursorLine < currentTopLine)
            {
                setOffsetY(-cursorLine * lineHeight);
            }
            else if (cursorLine >= currentTopLine + visibleLines)
            {
                setOffsetY(-(cursorLine - visibleLines + 1) * lineHeight);
            }
        }
        // Java: 116
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return (isDisabled() ? com.oddlabs.tt.gui.CursorType.NORMAL : com.oddlabs.tt.gui.CursorType.TEXT);
        }
        // Java: 121
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
            {
                com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
                float relativeX = (float)(x - ((float)(getRootX() + edit_box.getLeftOffset())));
                float relativeY = (float)(y - ((float)((float)(getRootY() + edit_box.getBottomOffset()) + getOffsetY())));
                index = getTextLayout().getCharacterIndexAt(relativeX, relativeY, getTextLayout().getTextHeight());
                correctOffsetY();
            }
        }
        public EditBox(int width, int height, int max_chars) : base(width, height, com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), max_chars)
        {
            index = 0;
        }
    }
}

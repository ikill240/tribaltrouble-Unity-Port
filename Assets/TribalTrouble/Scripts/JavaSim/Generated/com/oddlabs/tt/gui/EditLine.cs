// Translated from EditLine.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 25
    public class EditLine : com.oddlabs.tt.gui.TextField, com.oddlabs.tt.gui.Clipped
    {
        public java.util.Set enter_listeners;
        public com.oddlabs.tt.gui.Origin alignment;
        public string allowed_chars;
        public int max_text_width;
        public int offset_x;
        public int index;
        public int selectionStart;
        public int selectionEnd;
        public long errorFlashStart;
        public static com.oddlabs.tt.audio.Audio ERROR_SOUND;
        // Java: 58
        public override com.oddlabs.tt.gui.CursorType getCursorType()
        {
            return (isDisabled() ? com.oddlabs.tt.gui.CursorType.NORMAL : com.oddlabs.tt.gui.CursorType.TEXT);
        }
        // Java: 63
        public virtual string getDisplayText()
        {
            return getText();
        }
        // Java: 67
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            com.oddlabs.tt.gui.ModeIconQuads.Mode mode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : ((isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)));
            edit_box.render(renderer, 0.0f, 0.0f, getWidth(), getHeight(), mode);
            long elapsed = java.lang.JSystem.currentTimeMillis() - errorFlashStart;
            if (elapsed < 200)
            {
                float alpha = (float)(0.5f * ((float)(1.0f - ((float)(elapsed / 200.0f)))));
                renderer.drawColoredQuad(2, 2, getWidth() - 4, getHeight() - 4, new org.joml.Vector4f(1.0f, 1.0f, 1.0f, alpha));
            }
            int render_index = (isActive() ? index : (-1));
            renderText(renderer, edit_box, offset_x, render_index);
        }
        // Java: 83
        public virtual int getRenderedWidth(string text)
        {
            int x_border = getFont().getXBorder();
            int half_border = x_border / 2;
            return (text.isEmpty() ? half_border : getFont().getWidth(text) - (x_border - half_border));
        }
        // Java: 89
        public virtual void renderText(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.gui.Box box, int offset_x, int render_index)
        {
            string displayText = getDisplayText();
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, getFont(), displayText, box.getLeftOffset() + offset_x, box.getBottomOffset(), box.getLeftOffset() + 1, getWidth() - box.getRightOffset() - 1, com.oddlabs.util.Color.WHITE);
            if (render_index != (-1))
            {
                int cursorX = getRenderedWidth(displayText.subSequence(0, render_index));
                com.oddlabs.tt.font.Index.renderIndex(renderer, box.getLeftOffset() + offset_x + cursorX, box.getBottomOffset(), getFont(), com.oddlabs.util.Color.WHITE);
            }
            renderSelectionHighlight(renderer, displayText, box, offset_x);
        }
        // Java: 101
        public override bool insert(int index, char key)
        {
            bool result = base.insert(index, key);
            if (result)
            {
                this.index++;
            }
            return result;
        }
        public const bool ERROR_SOUND_ENABLED = false;
        // Java: 112
        public virtual void triggerError()
        {
            errorFlashStart = java.lang.JSystem.currentTimeMillis();
            if (com.oddlabs.tt.gui.EditLine.ERROR_SOUND_ENABLED)
            {
                try
                {
                    com.oddlabs.tt.audio.AudioManager.getManager().newAudio(new com.oddlabs.tt.audio.AudioParameters(com.oddlabs.tt.gui.EditLine.ERROR_SOUND, 0.0f, 0.0f, 0.0f, com.oddlabs.tt.audio.AudioPlayer.AUDIO_RANK_NOTIFICATION, com.oddlabs.tt.audio.AudioPlayer.AUDIO_DISTANCE_NOTIFICATION, 0.5f, 1.0f, 0.5f, false, true));
                }
                catch (System.Exception unused)
                {
                }
            }
        }
        // Java: 125
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
            {
                if (((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.RETURN) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
                {
                    enterPressedAll();
                    return;
                }
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                if (handleClipboardInput(@event) || handleSelectionNavigation(@event))
                {
                    correctOffsetX();
                    @event.consume();
                    return;
                }
                if (handleSelectionReplacement(@event))
                {
                    correctOffsetX();
                    @event.consume();
                    return;
                }
                bool consumed = true;
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT))
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
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_HOME))
                {
                    index = 0;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_END))
                {
                    index = getText().length();
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_BACKSPACE))
                {
                    if (index > 0)
                    {
                        delete(--index);
                    }
                    else
                    {
                        triggerError();
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
                    char c = @event.getCharacter();
                    if (c != 0 && !java.lang.Character.isISOControl(c))
                    {
                        consumed = insert(index, c);
                        if (!consumed)
                        {
                            triggerError();
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
                    correctOffsetX();
                    @event.consume();
                    return;
                }
                if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) && !@event.isControlDown() && !@event.isAltDown() && !@event.isMetaDown())
                {
                    char c_2 = @event.getCharacter();
                    if (c_2 != 0 && !java.lang.Character.isISOControl(c_2))
                    {
                        @event.consume();
                        return;
                    }
                }
            }
            base.handleInput(@event);
        }
        // Java: 199
        public override bool isAllowed(char ch)
        {
            return base.isAllowed(ch) && getFont().getQuad(ch) != null && (allowed_chars == null || allowed_chars.indexOf(ch) != (-1));
        }
        // Java: 205
        public void correctOffsetX()
        {
            string displayText = getDisplayText();
            int cursorX = getRenderedWidth(displayText.subSequence(0, index));
            int textWidth = getRenderedWidth(displayText);
            if (((object)alignment == (object)com.oddlabs.tt.gui.Origin.AT_END))
            {
                offset_x = max_text_width - textWidth;
            }
            else
            {
                if (cursorX + offset_x >= max_text_width)
                {
                    offset_x = max_text_width - cursorX - com.oddlabs.tt.font.Index.INDEX_WIDTH;
                }
                if (cursorX + offset_x < 0)
                {
                    offset_x = -cursorX;
                }
                if (textWidth + offset_x < max_text_width)
                {
                    offset_x = java.lang.Math.min(0, max_text_width - textWidth);
                }
                if (offset_x > 0)
                {
                    offset_x = 0;
                }
            }
        }
        // Java: 233
        public int getIndex()
        {
            return index;
        }
        // Java: 237
        public void setIndex(int index)
        {
            this.index = index;
        }
        // Java: 241
        public override void clear()
        {
            base.clear();
            index = 0;
            clearSelection();
            correctOffsetX();
        }
        // Java: 249
        public override void appendNotify(string str)
        {
            correctOffsetX();
        }
        // Java: 254
        public override void focusNotify(bool focus)
        {
            if (focus)
            {
                index = getText().length();
            }
            clearSelection();
            correctOffsetX();
        }
        // Java: 262
        public override void mouseEntered()
        {
        }
        // Java: 266
        public override void mouseExited()
        {
        }
        // Java: 270
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.LEFT))
            {
                com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
                float relativeX = x - (edit_box.getLeftOffset() + offset_x);
                int bestIndex = 0;
                float bestDx = java.lang.Float.MAX_VALUE;
                string displayText = getDisplayText();
                for (int i = 0; i <= displayText.length(); i++)
                {
                    int charX = getRenderedWidth(displayText.subSequence(0, i));
                    float dx = java.lang.Math.abs((float)(relativeX - charX));
                    if (dx < bestDx)
                    {
                        bestDx = dx;
                        bestIndex = i;
                    }
                }
                index = bestIndex;
                clearSelection();
                correctOffsetX();
            }
        }
        // Java: 294
        public void enterPressedAll()
        {
            string text = getText();
            enterPressed(text);
            foreach (com.oddlabs.tt.guievent.EnterListener listener in java.lang.JIter.of(enter_listeners))
            {
                listener.enterPressed(text);
            }
        }
        // Java: 302
        public virtual void enterPressed(string text)
        {
        }
        // Java: 305
        public void addEnterListener(com.oddlabs.tt.guievent.EnterListener listener)
        {
            enter_listeners.add(listener);
        }
        // Java: 314
        public bool handleClipboardInput(com.oddlabs.tt.input.InputEvent @event)
        {
            bool ctrl = @event.isControlDown() || @event.isMetaDown();
            if (!ctrl)
            {
                return false;
            }
            if (((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.A))
            {
                selectionStart = 0;
                selectionEnd = getText().length();
                index = selectionEnd;
                return true;
            }
            else if (((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.C))
            {
                copyToClipboard();
                return true;
            }
            else if (((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.V))
            {
                pasteFromClipboard();
                return true;
            }
            else if (((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.X))
            {
                cutToClipboard();
                return true;
            }
            return false;
        }
        // Java: 339
        public bool handleSelectionNavigation(com.oddlabs.tt.input.InputEvent @event)
        {
            bool shift = @event.isShiftDown();
            bool ctrl = @event.isControlDown() || @event.isMetaDown();
            if (!shift && !ctrl)
            {
                return false;
            }
            int oldIndex = index;
            if (ctrl && ((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.LEFT))
            {
                index = wordBoundaryLeft(index);
                updateSelection(shift, oldIndex);
                return true;
            }
            else if (ctrl && ((object)@event.getKeyCode() == (object)com.oddlabs.tt.input.Key.RIGHT))
            {
                index = wordBoundaryRight(index);
                updateSelection(shift, oldIndex);
                return true;
            }
            else if (shift && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT))
            {
                if (index > 0)
                {
                    index--;
                }
                updateSelection(true, oldIndex);
                return true;
            }
            else if (shift && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT))
            {
                if (index < getText().length())
                {
                    index++;
                }
                updateSelection(true, oldIndex);
                return true;
            }
            else if (shift && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_HOME))
            {
                index = 0;
                updateSelection(true, oldIndex);
                return true;
            }
            else if (shift && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_END))
            {
                index = getText().length();
                updateSelection(true, oldIndex);
                return true;
            }
            return false;
        }
        // Java: 383
        public bool handleSelectionReplacement(com.oddlabs.tt.input.InputEvent @event)
        {
            if (!hasSelection())
            {
                return false;
            }
            if (@event.hasAction(com.oddlabs.tt.input.GameAction.UI_BACKSPACE) || @event.hasAction(com.oddlabs.tt.input.GameAction.UI_DELETE))
            {
                @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_BACKSPACE);
                @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_DELETE);
                deleteSelection();
                return true;
            }
            if (!@event.isControlDown() && !@event.isMetaDown() && !@event.isAltDown())
            {
                char c = @event.getCharacter();
                if (c != 0 && !java.lang.Character.isISOControl(c))
                {
                    deleteSelection();
                    bool result = insert(index, c);
                    if (!result)
                    {
                        triggerError();
                    }
                    return true;
                }
            }
            if (!@event.isShiftDown())
            {
                if (@event.hasAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT) || @event.hasAction(com.oddlabs.tt.input.GameAction.UI_NAV_HOME))
                {
                    @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_LEFT);
                    @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_HOME);
                    index = java.lang.Math.min(selectionStart, selectionEnd);
                    clearSelection();
                    return true;
                }
                if (@event.hasAction(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT) || @event.hasAction(com.oddlabs.tt.input.GameAction.UI_NAV_END))
                {
                    @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_RIGHT);
                    @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_END);
                    index = java.lang.Math.max(selectionStart, selectionEnd);
                    clearSelection();
                    return true;
                }
                clearSelection();
            }
            return false;
        }
        // Java: 428
        public void renderSelectionHighlight(com.oddlabs.tt.render.GUIRenderer renderer, string displayText, com.oddlabs.tt.gui.Box box, int offset_x)
        {
            if (!hasSelection())
            {
                return;
            }
            int selStartX = getRenderedWidth(displayText.subSequence(0, selectionStart));
            int selEndX = getRenderedWidth(displayText.subSequence(0, selectionEnd));
            int highlightLeft = java.lang.Math.max(box.getLeftOffset() + offset_x + java.lang.Math.min(selStartX, selEndX), box.getLeftOffset() + 1);
            int highlightRight = java.lang.Math.min(box.getLeftOffset() + offset_x + java.lang.Math.max(selStartX, selEndX), getWidth() - box.getRightOffset() - 1);
            if (highlightRight > highlightLeft)
            {
                renderer.drawColoredQuad(highlightLeft, box.getBottomOffset(), highlightRight - highlightLeft, getFont().getHeight(), new org.joml.Vector4f(0.3f, 0.5f, 1.0f, 0.4f));
            }
        }
        // Java: 447
        public bool hasSelection()
        {
            return selectionStart != (-1) && selectionEnd != (-1) && selectionStart != selectionEnd;
        }
        // Java: 451
        public void clearSelection()
        {
            selectionStart = (-1);
            selectionEnd = (-1);
        }
        // Java: 456
        public bool deleteSelection()
        {
            if (!hasSelection())
            {
                return false;
            }
            int start = java.lang.Math.min(selectionStart, selectionEnd);
            int end = java.lang.Math.max(selectionStart, selectionEnd);
            string contents = getContents();
            string newContent = java.lang.JStr.valueOf(contents.substring(0, start)) + java.lang.JStr.valueOf(contents.substring(end));
            set(newContent);
            index = start;
            clearSelection();
            return true;
        }
        // Java: 468
        public void updateSelection(bool shiftDown, int oldIndex)
        {
            if (shiftDown)
            {
                if (selectionStart == (-1))
                {
                    selectionStart = oldIndex;
                    selectionEnd = index;
                }
                else
                {
                    if (oldIndex == selectionStart)
                    {
                        selectionStart = index;
                    }
                    else
                    {
                        selectionEnd = index;
                    }
                }
                if (selectionStart > selectionEnd)
                {
                    int tmp = selectionStart;
                    selectionStart = selectionEnd;
                    selectionEnd = tmp;
                }
            }
            else
            {
                clearSelection();
            }
        }
        // Java: 490
        public void copyToClipboard()
        {
            if (!hasSelection())
            {
                return;
            }
            try
            {
                string selectedText = getContents().substring(java.lang.Math.min(selectionStart, selectionEnd), java.lang.Math.max(selectionStart, selectionEnd));
                com.oddlabs.tt.util.PortHost.setClipboard(selectedText);
            }
            catch (System.Exception e)
            {
                java.lang.JSystem.err.println("Error accessing clipboard: " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
        // Java: 502
        public void pasteFromClipboard()
        {
            try
            {
                string text = com.oddlabs.tt.util.PortHost.getClipboard();
                if (text == null || text.isEmpty())
                {
                    return;
                }
                for (int i = 0; i < text.length(); i++)
                {
                    char c = text.charAt(i);
                    if (!isAllowed(c))
                    {
                        return;
                    }
                }
                deleteSelection();
                string contents = getContents();
                int insertPos = index;
                string newContent = java.lang.JStr.valueOf(contents.substring(0, insertPos)) + java.lang.JStr.valueOf(text) + java.lang.JStr.valueOf(contents.substring(insertPos));
                if (max_chars != (-1) && newContent.length() > max_chars)
                {
                    return;
                }
                set(newContent);
                index = java.lang.Math.min(insertPos + text.length(), newContent.length());
                correctOffsetX();
            }
            catch (System.Exception e)
            {
                java.lang.JSystem.err.println("Error accessing clipboard: " + java.lang.JStr.valueOf(e.getMessage()));
            }
        }
        // Java: 528
        public void cutToClipboard()
        {
            if (!hasSelection())
            {
                return;
            }
            copyToClipboard();
            deleteSelection();
        }
        // Java: 534
        public int wordBoundaryLeft(int pos)
        {
            string contents = getContents();
            int i = pos - 1;
            while (i > 0 && contents.charAt(i) == ' ')
            {
                i--;
            }
            while (i > 0 && contents.charAt(i - 1) != ' ')
            {
                i--;
            }
            return java.lang.Math.max(0, i);
        }
        // Java: 542
        public int wordBoundaryRight(int pos)
        {
            string contents = getContents();
            int i = pos;
            while (i < contents.length() && contents.charAt(i) == ' ')
            {
                i++;
            }
            while (i < contents.length() && contents.charAt(i) != ' ')
            {
                i++;
            }
            return i;
        }
        static EditLine()
        {
            EditLine.ERROR_SOUND = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/chicken_peck.ogg")));
        }
        private void init_EditLine()
        {
            this.enter_listeners = new java.util.concurrent.CopyOnWriteArraySet();
            this.selectionStart = (-1);
            this.selectionEnd = (-1);
            this.errorFlashStart = 0;
        }
        public EditLine(int width, int max_chars) : this(width, max_chars, com.oddlabs.tt.gui.Origin.AT_START)
        {
        }
        public EditLine(int width, int max_chars, com.oddlabs.tt.gui.Origin alignment) : this(width, max_chars, null, alignment)
        {
        }
        public EditLine(int width, int max_chars, string allowed_chars, com.oddlabs.tt.gui.Origin alignment) : base(com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), max_chars)
        {
            init_EditLine();
            this.allowed_chars = allowed_chars;
            this.alignment = alignment;
            com.oddlabs.tt.gui.Box edit_box = com.oddlabs.tt.gui.Skin.getSkin().getEditBox();
            setDim(width, getFont().getHeight() + edit_box.getBottomOffset() + edit_box.getTopOffset());
            setCanFocus(true);
            this.max_text_width = width - edit_box.getLeftOffset() - edit_box.getRightOffset();
            clear();
        }
    }
}

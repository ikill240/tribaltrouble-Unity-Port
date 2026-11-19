// Translated from MultiColumnComboBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class MultiColumnComboBox : com.oddlabs.tt.gui.GUIObject, com.oddlabs.tt.gui.Scrollable
    {
        public com.oddlabs.tt.gui.ColumnInfo[] column_infos;
        public com.oddlabs.tt.gui.RadioButtonGroup group;
        public com.oddlabs.tt.gui.Group focus_group;
        public com.oddlabs.tt.gui.RowCollection rows;
        public com.oddlabs.tt.gui.ScrollBar scroll_bar;
        public bool use_buttons;
        public com.oddlabs.tt.gui.GUIRoot gui_root;
        public int offset_y;
        public com.oddlabs.tt.gui.PulldownMenu pulldown_menu;
        public object right_clicked_row_data;
        // Java: 62
        public int getSize()
        {
            return rows.getSize();
        }
        // Java: 66
        public void selectFirst()
        {
            rows.selectFirst();
            clickedRow();
        }
        // Java: 71
        public void clickedRow()
        {
            object selected = rows.getSelected();
            if (null != selected)
            {
                listeners.forEach(new java.util.function.Consumer_L((object listener_l) =>
                {
                    com.oddlabs.tt.guievent.EventListener listener = (com.oddlabs.tt.guievent.EventListener)listener_l;
                    if ((listener is com.oddlabs.tt.guievent.RowListener rowListener))
                    {
                        rowListener.rowChosen(selected);
                    }
                }));
            }
        }
        // Java: 82
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED))
            {
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
                {
                    doubleClickedRow();
                    @event.consume();
                    return;
                }
            }
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.PRESSED) || ((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.REPEAT))
            {
                bool consumed = true;
                bool navigated = false;
                if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_UP))
                {
                    rows.selectPrior();
                    clickedRow();
                    navigated = true;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_DOWN))
                {
                    rows.selectNext();
                    clickedRow();
                    navigated = true;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_HOME))
                {
                    rows.selectFirst();
                    clickedRow();
                    navigated = true;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_END))
                {
                    rows.selectLast();
                    clickedRow();
                    navigated = true;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_PAGE_UP))
                {
                    jumpPage(true);
                    navigated = true;
                }
                else if (@event.consumeAction(com.oddlabs.tt.input.GameAction.UI_NAV_PAGE_DOWN))
                {
                    jumpPage(false);
                    navigated = true;
                }
                else
                {
                    consumed = false;
                }
                if (navigated)
                {
                    rows.setFocus();
                }
                if (consumed)
                {
                    @event.consume();
                    return;
                }
            }
            base.handleInput(@event);
        }
        // Java: 130
        public void addRowListener(com.oddlabs.tt.guievent.RowListener listener)
        {
            listeners.add(listener);
        }
        // Java: 134
        public void doubleClickedRow()
        {
            object selected = rows.getSelected();
            if (null != selected)
            {
                listeners.forEach(new java.util.function.Consumer_L((object listener_l) =>
                {
                    com.oddlabs.tt.guievent.EventListener listener = (com.oddlabs.tt.guievent.EventListener)listener_l;
                    if ((listener is com.oddlabs.tt.guievent.RowListener rowListener))
                    {
                        rowListener.rowDoubleClicked(selected);
                    }
                }));
            }
        }
        // Java: 146
        public void setPulldownMenu(com.oddlabs.tt.gui.PulldownMenu pulldown_menu)
        {
            this.pulldown_menu = pulldown_menu;
        }
        // Java: 150
        public void rightClickedRow(int x, int y)
        {
            if (pulldown_menu != null)
            {
                int pulldown_x = java.lang.Math.clamp(x, 0, gui_root.getWidth() - pulldown_menu.getWidth());
                int pulldown_y = java.lang.Math.clamp(y - pulldown_menu.getHeight(), 0, gui_root.getHeight() - pulldown_menu.getHeight());
                pulldown_menu.setPos(pulldown_x, pulldown_y);
                gui_root.getDelegate().addChild(pulldown_menu);
                pulldown_menu.setFocus();
                right_clicked_row_data = getSelected();
            }
        }
        // Java: 162
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.Box box = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box();
            com.oddlabs.tt.gui.ModeIconQuads.Mode mode = (isActive() || getFocusedChild() != null ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            box.render(renderer, 0.0f, 0.0f, getWidth() - scroll_bar.getWidth(), getHeight() - ((use_buttons ? group.getMarked().getHeight() : 0)), mode);
        }
        // Java: 170
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            focus_group.setGroupFocus(direction);
        }
        // Java: 175
        public void clear()
        {
            rows.clear();
        }
        // Java: 179
        public void addRow(com.oddlabs.tt.gui.Row row)
        {
            row.setColumnInfos(column_infos);
            rows.addRow(row);
            scroll_bar.update();
        }
        // Java: 185
        public object getSelected()
        {
            return rows.getSelected();
        }
        // Java: 189
        public object getRightClickedRowData()
        {
            return right_clicked_row_data;
        }
        // Java: 193
        public void selectRow(com.oddlabs.tt.gui.Row row)
        {
            rows.selectRow(row);
        }
        // Java: 197
        public override void mouseScrolled(int amount)
        {
            setOffsetY(offset_y + ((amount > 0 ? (-3) : 3)) * getStepHeight());
        }
        // Java: 202
        public void setOffsetY(int new_offset)
        {
            offset_y = java.lang.Math.clamp(new_offset, 0, java.lang.Math.max(rows.getContentHeight() - rows.getHeight(), 0));
            rows.replaceRows();
            scroll_bar.update();
        }
        // Java: 209
        public int getOffsetY()
        {
            return offset_y;
        }
        // Java: 214
        public int getStepHeight()
        {
            return com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().font().getHeight();
        }
        // Java: 219
        public void jumpPage(bool up)
        {
            if (up)
            {
                setOffsetY(offset_y - rows.getHeight());
            }
            else
            {
                setOffsetY(offset_y + rows.getHeight());
            }
        }
        // Java: 227
        public float getScrollBarRatio()
        {
            return (float)(rows.getHeight() / ((float)(java.lang.Math.max(rows.getContentHeight(), offset_y + rows.getHeight()))));
        }
        // Java: 232
        public float getScrollBarOffset()
        {
            int length = java.lang.Math.max(rows.getContentHeight(), offset_y + rows.getHeight());
            return (float)(offset_y / ((float)((length - rows.getHeight()))));
        }
        // Java: 238
        public void setScrollBarOffset(float offset)
        {
            int length = java.lang.Math.max(rows.getContentHeight(), offset_y + rows.getHeight());
            setOffsetY(java.lang.JC.f2i(((float)(offset * (length - rows.getHeight())))));
        }
        private void init_MultiColumnComboBox()
        {
            this.group = new com.oddlabs.tt.gui.RadioButtonGroup();
            this.focus_group = new com.oddlabs.tt.gui.Group();
            this.rows = new com.oddlabs.tt.gui.RowCollection(this, 0, true);
            this.offset_y = 0;
            this.pulldown_menu = null;
        }
        public MultiColumnComboBox(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.ColumnInfo[] column_infos, int height) : this(gui_root, column_infos, height, true)
        {
        }
        public MultiColumnComboBox(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.gui.ColumnInfo[] column_infos, int height, bool use_buttons) : base()
        {
            init_MultiColumnComboBox();
            this.gui_root = gui_root;
            this.column_infos = column_infos;
            this.use_buttons = use_buttons;
            com.oddlabs.tt.gui.Box box = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box();
            int width = 0;
            for (int i = 0; i < column_infos.Length; i++)
            {
                com.oddlabs.tt.gui.ColumnButton column_button = new com.oddlabs.tt.gui.ColumnButton(group, rows, column_infos[i], i, true);
                if (use_buttons)
                {
                    column_button.setPos(width, height - column_button.getHeight());
                    focus_group.addChild(column_button);
                }
                width += column_button.getWidth();
            }
            scroll_bar = new com.oddlabs.tt.gui.ScrollBar(height, this);
            scroll_bar.setPos(width, 0);
            scroll_bar.setTabStop(false);
            focus_group.addChild(scroll_bar);
            setDim(width + scroll_bar.getWidth(), height);
            focus_group.setDim(getWidth(), getHeight());
            focus_group.setPos(0, 0);
            addChild(focus_group);
            if (use_buttons)
            {
                rows.setDim(width - box.getLeftOffset() - box.getRightOffset(), height - box.getBottomOffset() - box.getTopOffset() - group.getMarked().getHeight());
            }
            else
            {
                rows.setDim(width - box.getLeftOffset() - box.getRightOffset(), height - box.getBottomOffset() - box.getTopOffset());
            }
            rows.setPos(box.getLeftOffset(), box.getBottomOffset());
            focus_group.addChild(rows);
            setCanFocus(true);
            scroll_bar.update();
        }
    }
}

// Translated from RowCollection.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 12
    public class RowCollection : com.oddlabs.tt.gui.GUIObject, com.oddlabs.tt.gui.Clipped
    {
        public java.util.List rows;
        public com.oddlabs.tt.gui.MultiColumnComboBox multi_box;
        public com.oddlabs.tt.gui.Row selected_row;
        public int sort_index;
        public bool sorted_descending;
        // Java: 27
        public override void handleInput(com.oddlabs.tt.input.InputEvent @event)
        {
            if (((object)@event.getPhase() == (object)com.oddlabs.tt.input.InputPhase.RELEASED) && @event.consumeAction(com.oddlabs.tt.input.GameAction.UI_ACTIVATE))
            {
                multi_box.doubleClickedRow();
                @event.consume();
                return;
            }
            base.handleInput(@event);
        }
        // Java: 37
        public override void focusNotify(bool focus)
        {
            if (focus && selected_row == null)
            {
                selectFirst();
            }
        }
        // Java: 44
        public void selectNext()
        {
            if (rows.isEmpty())
            {
                return;
            }
            int index = (selected_row == null ? (-1) : rows.indexOf(selected_row));
            if (sorted_descending)
            {
                index++;
            }
            else
            {
                index--;
            }
            if (index >= 0 && index < rows.size())
            {
                selectRow(((com.oddlabs.tt.gui.Row)rows.get(index)));
                ensureVisible(selected_row);
            }
        }
        // Java: 58
        public void selectPrior()
        {
            if (rows.isEmpty())
            {
                return;
            }
            int index = (selected_row == null ? (-1) : rows.indexOf(selected_row));
            if (sorted_descending)
            {
                index--;
            }
            else
            {
                index++;
            }
            if (index >= 0 && index < rows.size())
            {
                selectRow(((com.oddlabs.tt.gui.Row)rows.get(index)));
                ensureVisible(selected_row);
            }
        }
        // Java: 72
        public void selectFirst()
        {
            if (rows.isEmpty())
            {
                return;
            }
            ensureVisible(selectRow((sorted_descending ? ((com.oddlabs.tt.gui.Row)rows.getFirst()) : ((com.oddlabs.tt.gui.Row)rows.getLast()))));
        }
        // Java: 77
        public void selectLast()
        {
            if (rows.isEmpty())
            {
                return;
            }
            ensureVisible(selectRow((sorted_descending ? ((com.oddlabs.tt.gui.Row)rows.getLast()) : ((com.oddlabs.tt.gui.Row)rows.getFirst()))));
        }
        // Java: 82
        public void ensureVisible(com.oddlabs.tt.gui.Row row)
        {
            com.oddlabs.tt.gui.Scrollable scrollable = multi_box;
            int row_top = row.getY() + row.getHeight();
            int row_bottom = row.getY();
            int current_offset = scrollable.getOffsetY();
            if (row_top > getHeight())
            {
                scrollable.setOffsetY(current_offset - (row_top - getHeight()));
            }
            else if (row_bottom < 0)
            {
                scrollable.setOffsetY(current_offset - row_bottom);
            }
        }
        // Java: 95
        public void clear()
        {
            foreach (com.oddlabs.tt.gui.Row row in java.lang.JIter.of(rows))
            {
                row.remove();
            }
            rows.clear();
            selected_row = null;
            replaceRows();
        }
        // Java: 104
        public void addRow(com.oddlabs.tt.gui.Row row)
        {
            rows.add(row);
            row.addMouseClickListener(new com.oddlabs.tt.guievent.MouseClickListener_L((com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks) =>
            {
                setFocus();
                selectRow(row);
                if (((object)button == (object)com.oddlabs.tt.gui.MouseButton.RIGHT))
                {
                    multi_box.clickedRow();
                    multi_box.rightClickedRow(java.lang.JC.f2i(((float)(row.getRootX() + x))), java.lang.JC.f2i(((float)(row.getRootY() + y))));
                }
                else if (clicks == 1)
                {
                    multi_box.clickedRow();
                }
                else if (clicks == 2)
                {
                    multi_box.doubleClickedRow();
                }
            }));
            row.setSortIndex(sort_index);
            addChild(row);
            rows.sort(null);
            replaceRows();
        }
        // Java: 124
        public int getSize()
        {
            return rows.size();
        }
        // Java: 128
        public void markChanged(int index, bool sorted_descending)
        {
            sort_index = index;
            this.sorted_descending = sorted_descending;
            foreach (com.oddlabs.tt.gui.Row row in java.lang.JIter.of(rows))
            {
                row.setSortIndex(sort_index);
            }
            rows.sort(null);
            replaceRows();
        }
        // Java: 138
        public void replaceRows()
        {
            int y = getHeight() + multi_box.getOffsetY();
            for (int i = 0; i < rows.size(); i++)
            {
                com.oddlabs.tt.gui.Row row = default;
                if (sorted_descending)
                {
                    row = ((com.oddlabs.tt.gui.Row)rows.get(i));
                }
                else
                {
                    row = ((com.oddlabs.tt.gui.Row)rows.get(rows.size() - i - 1));
                }
                y -= row.getHeight();
                row.setPos(0, y);
                com.oddlabs.tt.gui.MultiColumnComboBoxData data = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData();
                row.setColor((i % 2 == 0 ? data.color1() : data.color2()));
            }
        }
        // Java: 153
        public int getContentHeight()
        {
            return rows.stream().mapToInt(new java.util.function.ToIntFunction_L((object a0) => { return ((com.oddlabs.tt.gui.Row)a0).getHeight(); })).sum();
        }
        // Java: 157
        public object getSelected()
        {
            return (selected_row != null ? selected_row.getContentObject() : (object)(null));
        }
        // Java: 161
        public com.oddlabs.tt.gui.Row selectRow(com.oddlabs.tt.gui.Row row)
        {
            if (selected_row != null)
            {
                selected_row.mark(false);
            }
            selected_row = row;
            selected_row.mark(true);
            return selected_row;
        }
        private void init_RowCollection()
        {
            this.rows = new java.util.ArrayList();
        }
        public RowCollection(com.oddlabs.tt.gui.MultiColumnComboBox multi_box, int sort_index, bool sorted_descending) : base()
        {
            init_RowCollection();
            this.multi_box = multi_box;
            this.sort_index = sort_index;
            this.sorted_descending = sorted_descending;
            setCanFocus(true);
            setTabStop(true);
        }
    }
}

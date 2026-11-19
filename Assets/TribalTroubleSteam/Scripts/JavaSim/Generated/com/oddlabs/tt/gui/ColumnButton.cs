// Translated from ColumnButton.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 7
    public class ColumnButton : com.oddlabs.tt.gui.RadioButtonGroupElement
    {
        public com.oddlabs.tt.gui.RowCollection rows;
        public int arrow_offset;
        public int column_index;
        public bool sorted_descending;
        public bool pressed;
        // Java: 34
        public override void mouseReleased(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = false;
        }
        // Java: 39
        public override void mousePressed(com.oddlabs.tt.gui.MouseButton button, int x, int y)
        {
            pressed = true;
        }
        // Java: 44
        public override void mouseClicked(com.oddlabs.tt.gui.MouseButton button, int x, int y, int clicks)
        {
            sorted_descending = !isMarked() || !sorted_descending;
            base.mouseClicked(button, x, y, clicks);
            rows.markChanged(column_index, sorted_descending);
        }
        // Java: 51
        public int getColumnIndex()
        {
            return column_index;
        }
        // Java: 55
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (isHovered() && pressed ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : (isActive() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL)));
            com.oddlabs.tt.gui.MultiColumnComboBoxData data = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData();
            com.oddlabs.tt.gui.Horizontal buttonHorizontal = (((object)skinMode == (object)com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE) && isHovered() && pressed ? data.buttonPressed() : data.buttonUnpressed());
            buttonHorizontal.render(renderer, 0, 0, getWidth(), skinMode);
            if (isMarked())
            {
                renderMark(renderer, skinMode);
            }
        }
        // Java: 69
        public void renderMark(com.oddlabs.tt.render.GUIRenderer renderer, com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode)
        {
            com.oddlabs.tt.gui.MultiColumnComboBoxData data = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData();
            com.oddlabs.tt.gui.ModeIconQuads arrow = (sorted_descending ? data.descending() : data.ascending());
            com.oddlabs.tt.gui.IconQuad arrowQuad = arrow.quad(skinMode);
            renderer.drawIcon(arrowQuad, arrow_offset, (float)((getHeight() - arrowQuad.getHeight()) / 2.0f));
        }
        private void init_ColumnButton()
        {
            this.pressed = false;
        }
        public ColumnButton(com.oddlabs.tt.gui.RadioButtonGroup group, com.oddlabs.tt.gui.RowCollection rows, com.oddlabs.tt.gui.ColumnInfo info, int column_index, bool sorted_descending) : base(column_index == 0, group)
        {
            init_ColumnButton();
            this.rows = rows;
            this.column_index = column_index;
            this.sorted_descending = sorted_descending;
            com.oddlabs.tt.gui.MultiColumnComboBoxData data = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData();
            setDim(info.width(), data.buttonUnpressed().getHeight());
            com.oddlabs.tt.font.Font font = data.font();
            com.oddlabs.tt.gui.Label label = new com.oddlabs.tt.gui.Label(info.caption(), font);
            label.setPos(data.captionOffset(), (getHeight() - font.getHeight()) / 2 + 1);
            addChild(label);
            com.oddlabs.tt.gui.IconQuad arrow = com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().descending().quad(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL);
            arrow_offset = info.width() - arrow.getWidth();
            setCanFocus(true);
        }
    }
}

// Translated from Row.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class Row : com.oddlabs.tt.gui.GUIObject, java.lang.Comparable
    {
        public java.util.List columns;
        public object content_object;
        public int sort_index;
        public org.joml.Vector4f color;
        public bool marked;
        // Java: 29
        public com.oddlabs.tt.gui.GUIObject getColumn(int index)
        {
            return ((com.oddlabs.tt.gui.GUIObject)columns.get(index));
        }
        // Java: 33
        public void setColumnInfos(com.oddlabs.tt.gui.ColumnInfo[] column_infos)
        {
            int x = 0;
            for (int i = 0; i < column_infos.Length; i++)
            {
                com.oddlabs.tt.gui.GUIObject gui_object = getColumn(i);
                gui_object.setPos(x, 0);
                addChild(gui_object);
                x += column_infos[i].width();
                if (i == 0)
                {
                    x -= com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box().getLeftOffset();
                }
                if (i == column_infos.Length - 1)
                {
                    x -= com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().box().getRightOffset();
                }
            }
            setDim(x, getHeight());
        }
        // Java: 51
        public void setSortIndex(int sort_index)
        {
            this.sort_index = sort_index;
        }
        // Java: 55
        public int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.Row o = (com.oddlabs.tt.gui.Row)o_p;
            return (((java.lang.Comparable)getColumn(sort_index))).compareTo(o.getColumn(sort_index));
        }
        // Java: 60
        public void setColor(org.joml.Vector4fc color)
        {
            this.color.set(color);
        }
        // Java: 64
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            org.joml.Vector4fc c = (marked ? com.oddlabs.tt.gui.Skin.getSkin().getMultiColumnComboBoxData().colorMarked() : (org.joml.Vector4fc)(color));
            if (c.w() >= 0.2f)
            {
                renderer.drawColoredQuad(0, 0, getWidth(), getHeight(), c);
            }
        }
        // Java: 72
        public object getContentObject()
        {
            return content_object;
        }
        // Java: 76
        public void mark(bool marked)
        {
            this.marked = marked;
        }
        private void init_Row()
        {
            this.color = new org.joml.Vector4f(com.oddlabs.util.Color.TRANSPARENT);
            this.marked = false;
        }
        public Row(com.oddlabs.tt.gui.GUIObject[] columns, object content_object) : this(java.util.List_.of(columns), content_object)
        {
        }
        public Row(java.util.List columns, object content_object) : base()
        {
            init_Row();
            this.columns = columns;
            this.content_object = content_object;
            setDim(0, columns.stream().mapToInt(new java.util.function.ToIntFunction_L((object c_l) =>
            {
                com.oddlabs.tt.gui.GUIObject c = (com.oddlabs.tt.gui.GUIObject)c_l;
                return c.getHeight();
            })).max().orElse(0));
            setCanFocus(true);
        }
    }
}

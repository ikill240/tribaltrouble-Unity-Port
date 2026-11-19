// Translated from LabelBox.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class LabelBox : com.oddlabs.tt.gui.TextField, java.lang.Comparable, com.oddlabs.tt.gui.Clipped
    {
        public com.oddlabs.tt.font.TextLayout textLayout;
        public org.joml.Vector4fc color;
        // Java: 22
        public void updateLayout()
        {
            textLayout = new com.oddlabs.tt.font.TextLayout(getFont(), getText(), getWidth());
            setDim(getWidth(), textLayout.getTextHeight());
        }
        // Java: 27
        public override com.oddlabs.tt.gui.TextField setText(string text)
        {
            base.setText(text);
            updateLayout();
            return this;
        }
        // Java: 34
        public override com.oddlabs.tt.gui.Renderable setDim(int width, int height)
        {
            base.setDim(width, height);
            return this;
        }
        // Java: 40
        public com.oddlabs.tt.gui.LabelBox setColor(org.joml.Vector4fc color)
        {
            this.color = color;
            return this;
        }
        // Java: 45
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            org.joml.Vector4fc c = (isDisabled() ? com.oddlabs.tt.gui.Label.DISABLED_COLOR : color);
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, textLayout, 0, getHeight() - getFont().getHeight(), c);
        }
        // Java: 51
        public virtual int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.LabelBox o = (com.oddlabs.tt.gui.LabelBox)o_p;
            return java.lang.JO.toString(getText()).compareToIgnoreCase(java.lang.JO.toString(o.getText()));
        }
        private void init_LabelBox()
        {
            this.color = com.oddlabs.util.Color.WHITE;
        }
        public LabelBox(string text, com.oddlabs.tt.font.Font font, int width) : base(text, font, java.lang.Integer.MAX_VALUE)
        {
            init_LabelBox();
            textLayout = new com.oddlabs.tt.font.TextLayout(font, text, width);
            setDim(width, textLayout.getTextHeight());
        }
    }
}

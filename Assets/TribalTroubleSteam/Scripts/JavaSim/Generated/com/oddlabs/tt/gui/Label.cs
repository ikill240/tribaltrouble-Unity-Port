// Translated from Label.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 11
    public class Label : com.oddlabs.tt.gui.TextField, java.lang.Comparable
    {
        public static org.joml.Vector4fc DEFAULT_COLOR;
        public static org.joml.Vector4fc DISABLED_COLOR;
        public const int INSET = 2;
        public com.oddlabs.tt.gui.Origin align;
        public org.joml.Vector4f color;
        // Java: 34
        public com.oddlabs.tt.gui.Label setColor(org.joml.Vector4fc color)
        {
            this.color.set(color);
            return this;
        }
        // Java: 39
        public virtual org.joml.Vector4fc getColor()
        {
            return color;
        }
        // Java: 43
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            org.joml.Vector4fc c = (isDisabled() ? com.oddlabs.tt.gui.Label.DISABLED_COLOR : (org.joml.Vector4fc)(color));
            int textWidth = getFont().getWidth(getText());
            int x = ((align).ordinal() switch { 0 /*AT_START*/ => (int)(0), 1 /*AT_MIDDLE*/ => (int)((getWidth() - java.lang.Math.min(getWidth(), textWidth)) / 2), 2 /*AT_END*/ => (int)(getWidth() - textWidth - com.oddlabs.tt.gui.Label.INSET), _ => throw new System.InvalidOperationException("MatchException") });
            com.oddlabs.tt.font.TextLineRenderer.render(renderer, getFont(), getText(), x, 0, 0, getWidth() - com.oddlabs.tt.gui.Label.INSET, c);
        }
        // Java: 55
        public virtual int compareTo(object o_p)
        {
            com.oddlabs.tt.gui.Label o = (com.oddlabs.tt.gui.Label)o_p;
            return java.lang.JO.toString(getText()).compareToIgnoreCase(java.lang.JO.toString(o.getText()));
        }
        static Label()
        {
            Label.DEFAULT_COLOR = com.oddlabs.util.Color.WHITE;
            Label.DISABLED_COLOR = com.oddlabs.util.Color.argb4v((-1296911694));
        }
        private void init_Label()
        {
            this.color = new org.joml.Vector4f(com.oddlabs.tt.gui.Label.DEFAULT_COLOR);
        }
        public Label(string text, com.oddlabs.tt.font.Font font) : this(text, font, font.getWidth(text), com.oddlabs.tt.gui.Origin.AT_START)
        {
        }
        public Label(string text, com.oddlabs.tt.font.Font font, int width) : this(text, font, width, com.oddlabs.tt.gui.Origin.AT_START)
        {
        }
        public Label(string text, com.oddlabs.tt.font.Font font, int width, com.oddlabs.tt.gui.Origin align) : base(text, font, java.lang.Integer.MAX_VALUE)
        {
            init_Label();
            this.align = align;
            setDim(width, font.getHeight());
        }
    }
}

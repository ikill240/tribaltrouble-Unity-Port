// Translated from Diode.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 6
    public class Diode : com.oddlabs.tt.gui.GUIObject
    {
        public bool lit;
        // Java: 15
        public void setLit(bool lit)
        {
            this.lit = lit;
        }
        // Java: 19
        public bool isLit()
        {
            return lit;
        }
        // Java: 23
        public override void renderGeometry(com.oddlabs.tt.render.GUIRenderer renderer)
        {
            com.oddlabs.tt.gui.ModeIconQuads.Mode skinMode = (isDisabled() ? com.oddlabs.tt.gui.ModeIconQuads.Mode.DISABLED : (lit ? com.oddlabs.tt.gui.ModeIconQuads.Mode.ACTIVE : com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            renderer.drawModeIcon(com.oddlabs.tt.gui.Skin.getSkin().getDiode(), skinMode, 0, 0);
        }
        public Diode() : base()
        {
            com.oddlabs.tt.gui.IconQuad normal = ((com.oddlabs.tt.gui.IconQuad)com.oddlabs.tt.gui.Skin.getSkin().getDiode().get(com.oddlabs.tt.gui.ModeIconQuads.Mode.NORMAL));
            setDim(normal.getWidth(), normal.getHeight());
            lit = false;
        }
    }
}

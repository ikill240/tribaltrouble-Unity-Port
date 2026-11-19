// Translated from TerrainMenuForm.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 10
    public class TerrainMenuForm : com.oddlabs.tt.gui.Form, com.oddlabs.tt.form.TerrainMenuListener
    {
        public com.oddlabs.tt.form.TerrainMenu terrain;
        // Java: 20
        public override void setFocus(com.oddlabs.tt.gui.FocusDirection direction)
        {
            if (((object)direction == (object)com.oddlabs.tt.gui.FocusDirection.BACKWARD))
            {
                base.setFocus(direction);
            }
            else
            {
                terrain.getButtonOK().setFocus(direction);
            }
        }
        // Java: 29
        public void terrainMenuCancel()
        {
            cancel();
        }
        // Java: 34
        public void terrainMenuOK()
        {
        }
        public TerrainMenuForm(com.oddlabs.net.NetworkSelector network, com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.@delegate.Menu main_menu) : base()
        {
            terrain = new com.oddlabs.tt.form.TerrainMenu(network, gui_root, main_menu, false, this);
            addChild(terrain);
            terrain.place();
            compileCanvas();
        }
    }
}

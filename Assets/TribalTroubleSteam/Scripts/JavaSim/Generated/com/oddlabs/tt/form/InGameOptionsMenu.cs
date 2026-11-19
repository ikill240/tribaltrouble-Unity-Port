// Translated from InGameOptionsMenu.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.form
{
    // Java: 8
    public class InGameOptionsMenu : com.oddlabs.tt.form.AbstractOptionsMenu
    {
        public com.oddlabs.tt.viewer.WorldViewer viewer;
        // Java: 17
        public int getGamespeed()
        {
            int gamespeed = viewer.getLocalPlayer().getGamespeed();
            if (!com.oddlabs.tt.landscape.World.isValidGamespeed(gamespeed))
            {
                gamespeed = viewer.getWorld().getGamespeed();
            }
            return gamespeed;
        }
        // Java: 24
        public override void changeGamespeed(int index)
        {
            base.changeGamespeed(index);
            viewer.getPeerHub().getPlayerInterface().setPreferredGamespeed(index);
        }
        public InGameOptionsMenu(com.oddlabs.tt.gui.GUIRoot gui_root, com.oddlabs.tt.viewer.WorldViewer viewer) : base(gui_root)
        {
            this.viewer = viewer;
            chooseGamespeed(getGamespeed());
        }
    }
}

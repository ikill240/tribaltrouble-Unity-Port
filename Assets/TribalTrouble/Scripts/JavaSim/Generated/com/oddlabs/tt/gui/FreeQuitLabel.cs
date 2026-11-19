// Translated from FreeQuitLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 12
    public class FreeQuitLabel : com.oddlabs.tt.gui.Label, com.oddlabs.tt.animation.Animated
    {
        public static java.util.ResourceBundle bundle;
        // Java: 15
        public string i18n(string key, params object[] args)
        {
            return com.oddlabs.tt.util.Utils.getBundleString(com.oddlabs.tt.gui.FreeQuitLabel.bundle, key, args);
        }
        public com.oddlabs.tt.landscape.World world;
        public com.oddlabs.tt.animation.AnimationManager manager;
        // Java: 28
        public override void doAdd()
        {
            base.doAdd();
            manager.registerAnimation(this);
        }
        // Java: 34
        public override void doRemove()
        {
            base.doRemove();
            manager.removeAnimation(this);
        }
        // Java: 40
        public void animate(float t)
        {
            int time_left = java.lang.JC.f2i(com.oddlabs.tt.net.PeerHub.getFreeQuitTimeLeft(world));
            if (time_left > 0)
            {
                clear();
                append(i18n("quit_time_left", time_left));
            }
        }
        static FreeQuitLabel()
        {
            FreeQuitLabel.bundle = java.util.ResourceBundle.getBundle(typeof(com.oddlabs.tt.gui.FreeQuitLabel).getName());
        }
        public FreeQuitLabel(com.oddlabs.tt.landscape.World world, com.oddlabs.tt.animation.AnimationManager manager) : base("", com.oddlabs.tt.gui.Skin.getSkin().getEditFont(), 300)
        {
            this.world = world;
            this.manager = manager;
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}

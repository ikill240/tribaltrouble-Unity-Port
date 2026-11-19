// Translated from Cheat.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.viewer
{
    // Java: 3
    public class Cheat : java.lang.Object
    {
        public bool can_enable;
        public bool enabled;
        public bool draw_trees;
        public bool line_mode;
        // Java: 17
        public bool isEnabled()
        {
            return enabled;
        }
        // Java: 21
        public void enable()
        {
            if (can_enable)
            {
                enabled = true;
            }
        }
        private void init_Cheat()
        {
            this.enabled = false;
            this.draw_trees = true;
            this.line_mode = false;
        }
        public Cheat() : this(java.lang.Boolean.getBoolean("com.oddlabs.tt.developer"))
        {
        }
        public Cheat(bool can_enable) : base()
        {
            init_Cheat();
            this.can_enable = can_enable;
        }
    }
}

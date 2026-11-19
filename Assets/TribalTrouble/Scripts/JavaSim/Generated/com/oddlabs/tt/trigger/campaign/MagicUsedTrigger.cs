// Translated from MagicUsedTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 8
    public class MagicUsedTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public com.oddlabs.tt.model.Unit chieftain;
        public float x;
        public float y;
        public float r;
        public int index;
        public java.lang.Runnable runnable;
        public bool blowing;
        // Java: 28
        public override void check()
        {
            float dx = (float)(chieftain.getPositionX() - x);
            float dy = (float)(chieftain.getPositionY() - y);
            if (!chieftain.isDead())
            {
                if ((float)(r * r) > (float)((float)(dx * dx) + (float)(dy * dy)))
                {
                    if (!blowing && (chieftain.getPrimaryController() is com.oddlabs.tt.model.behaviour.MagicController) && chieftain.getLastMagicIndex() == index)
                    {
                        blowing = true;
                    }
                    if (blowing && !((chieftain.getPrimaryController() is com.oddlabs.tt.model.behaviour.MagicController)))
                    {
                        triggered();
                    }
                }
            }
        }
        // Java: 43
        public override void done()
        {
            runnable.run();
        }
        private void init_MagicUsedTrigger()
        {
            this.blowing = false;
        }
        public MagicUsedTrigger(com.oddlabs.tt.model.Unit chieftain, float x, float y, float r, int index, java.lang.Runnable runnable) : base(chieftain.getOwner().getWorld(), 0.0f, 0.0f)
        {
            init_MagicUsedTrigger();
            this.chieftain = chieftain;
            this.x = x;
            this.y = y;
            this.r = r;
            this.index = index;
            this.runnable = runnable;
        }
    }
}

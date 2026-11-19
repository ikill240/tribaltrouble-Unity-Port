// Translated from DeathTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger.campaign
{
    // Java: 7
    public class DeathTrigger : com.oddlabs.tt.trigger.IntervalTrigger
    {
        public com.oddlabs.tt.model.Selectable selectable;
        public java.lang.Runnable runnable;
        // Java: 17
        public override void check()
        {
            if (selectable.isDead())
            {
                triggered();
            }
        }
        // Java: 23
        public override void done()
        {
            runnable.run();
        }
        public DeathTrigger(com.oddlabs.tt.model.Selectable selectable, java.lang.Runnable runnable) : base(selectable.getOwner().getWorld(), 0.5f, 0.0f)
        {
            this.selectable = selectable;
            this.runnable = runnable;
        }
    }
}

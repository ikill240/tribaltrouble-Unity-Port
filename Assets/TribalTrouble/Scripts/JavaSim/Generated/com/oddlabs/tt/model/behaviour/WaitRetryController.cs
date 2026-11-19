// Translated from WaitRetryController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 12
    public class WaitRetryController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.behaviour.Behaviour wait_f;
        public float remaining;
        // Java: 41
        public static bool applies(com.oddlabs.tt.model.Unit unit)
        {
            return unit.getOwner().getWorld().isExtended();
        }
        // Java: 46
        public static float delay(com.oddlabs.tt.model.Unit unit)
        {
            return (float)(1.5f + (float)(((unit.getGridX() * 7 + unit.getGridY() * 3) & 7) * 0.25f));
        }
        // Java: 50
        public override void decide()
        {
            if (remaining <= 0)
            {
                unit.popController();
            }
            else
            {
                unit.setBehaviour(wait_f);
            }
        }
        public WaitRetryController(com.oddlabs.tt.model.Unit unit, float seconds) : base(0)
        {
            this.unit = unit;
            this.remaining = seconds;
            this.wait_f = new WaitRetryController.Anon1_Behaviour(this, unit);
        }
        // Java: anonymous class at line 21
        public sealed class Anon1_Behaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
        {
            public com.oddlabs.tt.model.behaviour.WaitRetryController this_0;
            public com.oddlabs.tt.model.Unit unit;
            public Anon1_Behaviour(com.oddlabs.tt.model.behaviour.WaitRetryController this_0_, com.oddlabs.tt.model.Unit unit_c)
            {
                this.this_0 = this_0_;
                this.unit = unit_c;
            }
            // Java: 22
            public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
            {
                unit.switchToIdleAnimation();
                this_0.remaining -= t;
                return (this_0.remaining > 0 ? com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE : com.oddlabs.tt.model.behaviour.Behaviour.State.DONE);
            }
            // Java: 29
            public bool isBlocking()
            {
                return true;
            }
            // Java: 34
            public void forceInterrupted()
            {
            }
        }
    }
}

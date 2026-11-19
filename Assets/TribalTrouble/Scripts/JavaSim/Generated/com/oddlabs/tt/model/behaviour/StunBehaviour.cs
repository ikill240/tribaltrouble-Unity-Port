// Translated from StunBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class StunBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public com.oddlabs.tt.model.behaviour.StunController controller;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 15
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            unit.switchToIdleAnimation();
            return (!controller.shouldSleep(t) ? com.oddlabs.tt.model.behaviour.Behaviour.State.DONE : com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE);
        }
        // Java: 21
        public bool isBlocking()
        {
            return true;
        }
        // Java: 26
        public void forceInterrupted()
        {
        }
        public StunBehaviour(com.oddlabs.tt.model.behaviour.StunController controller, com.oddlabs.tt.model.Unit unit) : base()
        {
            this.controller = controller;
            this.unit = unit;
        }
    }
}

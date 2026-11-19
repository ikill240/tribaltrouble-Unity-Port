// Translated from FollowWaitBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 13
    public class FollowWaitBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public com.oddlabs.tt.model.behaviour.FollowController controller;
        public com.oddlabs.tt.model.Unit unit;
        // Java: 22
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            unit.switchToIdleAnimation();
            return (controller.shouldWait(t) ? com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE : com.oddlabs.tt.model.behaviour.Behaviour.State.DONE);
        }
        // Java: 28
        public bool isBlocking()
        {
            return true;
        }
        // Java: 33
        public void forceInterrupted()
        {
        }
        public FollowWaitBehaviour(com.oddlabs.tt.model.behaviour.FollowController controller, com.oddlabs.tt.model.Unit unit) : base()
        {
            this.controller = controller;
            this.unit = unit;
        }
    }
}

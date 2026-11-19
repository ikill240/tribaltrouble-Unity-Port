// Translated from NullBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 5
    public class NullBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        // Java: 6
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
        }
        // Java: 11
        public bool isBlocking()
        {
            return true;
        }
        // Java: 16
        public void forceInterrupted()
        {
        }
        public NullBehaviour() : base()
        {
        }
    }
}

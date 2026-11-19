// Translated from StunController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class StunController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.behaviour.StunBehaviour stun_behaviour;
        public float time;
        // Java: 19
        public bool shouldSleep(float t)
        {
            time -= t;
            return time > 0;
        }
        // Java: 24
        public override void decide()
        {
            unit.setBehaviour(stun_behaviour);
            if (!shouldSleep(0.0f))
            {
                unit.popController();
            }
        }
        public StunController(com.oddlabs.tt.model.Unit unit, float time) : base(0)
        {
            this.unit = unit;
            this.time = time;
            stun_behaviour = new com.oddlabs.tt.model.behaviour.StunBehaviour(this, unit);
        }
    }
}

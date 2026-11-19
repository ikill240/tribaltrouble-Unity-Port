// Translated from SailController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class SailController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.util.Target target;
        public bool was_sliding;
        // Java: 17
        public override void decide()
        {
            if (ship.isDead())
            {
                return;
            }
            if (shouldGiveUp(0))
            {
                if (was_sliding)
                {
                    ship.setBehaviour(new com.oddlabs.tt.model.behaviour.SailBehaviour(ship, target));
                    was_sliding = false;
                    resetGiveUpCounter(0);
                }
                else
                {
                    ship.popController();
                }
            }
            else
            {
                if (!ship.slid())
                {
                    was_sliding = true;
                    ship.setBehaviour(new com.oddlabs.tt.model.behaviour.ShipSlideBehaviour(ship));
                }
                else
                {
                    was_sliding = false;
                    ship.setBehaviour(new com.oddlabs.tt.model.behaviour.SailBehaviour(ship, target));
                }
            }
        }
        private void init_SailController()
        {
            this.was_sliding = false;
        }
        public SailController(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.util.Target t) : base(1)
        {
            init_SailController();
            this.ship = ship;
            this.target = t;
        }
    }
}

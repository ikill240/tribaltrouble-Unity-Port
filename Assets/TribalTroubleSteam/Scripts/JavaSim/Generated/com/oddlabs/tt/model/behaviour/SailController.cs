// Translated from SailController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class SailController : com.oddlabs.tt.model.behaviour.Controller
    {
        public const int MAX_FAILED_UNSTUCK_ATTEMPTS = 3;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.util.Target target;
        public com.oddlabs.tt.model.behaviour.Behaviour current;
        public int failed_unstuck_attempts;
        // Java: 20
        public override void decide()
        {
            if (ship.isDead())
            {
                return;
            }
            switch ((current))
            {
                case null:
                {
                    current = (ship.slid() ? (com.oddlabs.tt.model.behaviour.Behaviour)(new com.oddlabs.tt.model.behaviour.SailBehaviour(ship, target)) : (com.oddlabs.tt.model.behaviour.Behaviour)(new com.oddlabs.tt.model.behaviour.ShipSlideBehaviour(ship)));
                    break;
                }
                case com.oddlabs.tt.model.behaviour.ShipSlideBehaviour slide:
                {
                    if (ship.slid())
                    {
                        current = new com.oddlabs.tt.model.behaviour.SailBehaviour(ship, target);
                    }
                    break;
                }
                case com.oddlabs.tt.model.behaviour.SailBehaviour sail:
                {
                    if (sail.isStuck())
                    {
                        current = new com.oddlabs.tt.model.behaviour.ShipUnstuckBehaviour(ship);
                    }
                    break;
                }
                case com.oddlabs.tt.model.behaviour.ShipUnstuckBehaviour unstuck:
                {
                    if (unstuck.isFinished())
                    {
                        failed_unstuck_attempts = 0;
                        current = new com.oddlabs.tt.model.behaviour.SailBehaviour(ship, target);
                    }
                    else if (unstuck.hasFailed())
                    {
                        failed_unstuck_attempts++;
                        if (failed_unstuck_attempts >= com.oddlabs.tt.model.behaviour.SailController.MAX_FAILED_UNSTUCK_ATTEMPTS)
                        {
                            ship.popController();
                            return;
                        }
                        current = new com.oddlabs.tt.model.behaviour.SailBehaviour(ship, target);
                    }
                    break;
                }
                default:
                {
                    break;
                }
            }
            ship.setBehaviour(current);
        }
        private void init_SailController()
        {
            this.current = null;
            this.failed_unstuck_attempts = 0;
        }
        public SailController(com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.util.Target t) : base(0)
        {
            init_SailController();
            this.ship = ship;
            this.target = t;
        }
    }
}

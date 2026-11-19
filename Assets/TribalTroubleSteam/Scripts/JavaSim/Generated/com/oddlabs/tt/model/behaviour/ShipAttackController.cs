// Translated from ShipAttackController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 10
    public class ShipAttackController : com.oddlabs.tt.model.behaviour.Controller
    {
        public const float MIN_SCAN_DELAY = 0.1f;
        public const float MAX_SCAN_DELAY = 0.2f;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.Ship ship;
        public com.oddlabs.tt.model.ShipAllocation allocation;
        public com.oddlabs.tt.model.AttackScanFilter scan_filter;
        public com.oddlabs.tt.model.behaviour.ShipAttackBehaviour ship_attack_behaviour;
        public float redecide_time;
        // Java: 31
        public bool shouldSleep(float t)
        {
            redecide_time -= t;
            return redecide_time > 0;
        }
        // Java: 36
        public override void decide()
        {
            unit.setBehaviour(ship_attack_behaviour);
            if (shouldSleep(0.0f))
            {
                return;
            }
            redecide_time = (float)(com.oddlabs.tt.model.behaviour.ShipAttackController.MIN_SCAN_DELAY + (float)(unit.getOwner().getWorld().getRandom().nextFloat() * ((float)(com.oddlabs.tt.model.behaviour.ShipAttackController.MAX_SCAN_DELAY - com.oddlabs.tt.model.behaviour.ShipAttackController.MIN_SCAN_DELAY))));
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                unit.scanVicinity(scan_filter);
            }
            com.oddlabs.tt.model.Selectable s = scan_filter.removeTarget();
            if (s != null)
            {
                unit.pushController(new com.oddlabs.tt.model.behaviour.AttackController(unit, s, allocation, ship));
            }
        }
        // Java: 47
        public override string getKey()
        {
            return java.lang.JStr.valueOf(base.getKey()) + java.lang.JStr.valueOf(unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.BUILD)) + java.lang.JStr.valueOf(unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.MAGIC));
        }
        public ShipAttackController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Ship ship, com.oddlabs.tt.model.AttackScanFilter filter, com.oddlabs.tt.model.ShipAllocation allocation) : base(0)
        {
            this.unit = unit;
            this.ship = ship;
            this.allocation = allocation;
            this.scan_filter = filter;
            this.ship_attack_behaviour = new com.oddlabs.tt.model.behaviour.ShipAttackBehaviour(this, unit, ship, allocation, false);
        }
    }
}

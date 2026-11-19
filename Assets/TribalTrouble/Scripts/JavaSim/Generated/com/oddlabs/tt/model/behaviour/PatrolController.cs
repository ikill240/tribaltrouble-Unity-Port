// Translated from PatrolController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 28
    public class PatrolController : com.oddlabs.tt.model.behaviour.Controller
    {
        public const float WAYPOINT_THRESHOLD = 3.0f;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.util.Target point_a;
        public com.oddlabs.tt.util.Target point_b;
        public com.oddlabs.tt.model.AttackScanFilter scan_filter;
        public bool heading_to_b;
        // Java: 55
        public override void decide()
        {
            if (unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                unit.scanVicinity(scan_filter);
                com.oddlabs.tt.model.Selectable enemy = scan_filter.removeTarget();
                if (enemy != null)
                {
                    unit.pushControllers(new com.oddlabs.tt.model.behaviour.PatrolController(unit, point_a, point_b, heading_to_b), new com.oddlabs.tt.model.behaviour.HuntController(unit, enemy));
                    return;
                }
            }
            com.oddlabs.tt.util.Target current_waypoint = (heading_to_b ? point_b : point_a);
            if (unit.isCloseEnough(com.oddlabs.tt.model.behaviour.PatrolController.WAYPOINT_THRESHOLD, current_waypoint))
            {
                heading_to_b = !heading_to_b;
                current_waypoint = (heading_to_b ? point_b : point_a);
            }
            unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, current_waypoint, 0.0f, true));
        }
        public PatrolController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target point_a, com.oddlabs.tt.util.Target point_b) : this(unit, point_a, point_b, true)
        {
        }
        public PatrolController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target point_a, com.oddlabs.tt.util.Target point_b, bool heading_to_b) : base(1)
        {
            this.unit = unit;
            this.point_a = point_a;
            this.point_b = point_b;
            this.scan_filter = new com.oddlabs.tt.model.AttackScanFilter(unit.getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE);
            this.heading_to_b = heading_to_b;
        }
    }
}

// Translated from GuardController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 52
    public class GuardController : com.oddlabs.tt.model.behaviour.Controller
    {
        public const float RETURN_THRESHOLD = 3.0f;
        public const float MAX_LEASH_DISTANCE = 10.0f;
        public const float WANDER_RADIUS = 6.0f;
        public const float MIN_WANDER_RADIUS = 2.0f;
        public const float WANDER_ARRIVE_THRESHOLD = 1.0f;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.util.Target guard_target;
        public com.oddlabs.tt.model.AttackScanFilter scan_filter;
        public com.oddlabs.tt.model.Selectable current_enemy;
        public com.oddlabs.tt.util.Target wander_point;
        public com.oddlabs.tt.model.behaviour.WalkBehaviour.HuntControllerFactory leashed_hunt_factory;
        // Java: 101
        public bool withinLeash(com.oddlabs.tt.util.Target t)
        {
            float dx = (float)(t.getPositionX() - guard_target.getPositionX());
            float dy = (float)(t.getPositionY() - guard_target.getPositionY());
            return (float)((float)(dx * dx) + (float)(dy * dy)) <= (float)(com.oddlabs.tt.model.behaviour.GuardController.MAX_LEASH_DISTANCE * com.oddlabs.tt.model.behaviour.GuardController.MAX_LEASH_DISTANCE);
        }
        // Java: 107
        public override void decide()
        {
            if ((guard_target is com.oddlabs.tt.model.Selectable guarded) && guarded.isDead())
            {
                unit.popController();
                return;
            }
            if (current_enemy != null && (current_enemy.isDead() || !withinLeash(current_enemy)))
            {
                current_enemy = null;
            }
            if (current_enemy == null && unit.getAbilities().hasAbilities(com.oddlabs.tt.model.Abilities.ATTACK))
            {
                unit.scanVicinity(scan_filter);
                com.oddlabs.tt.model.Selectable found = scan_filter.removeTarget();
                if (found != null && withinLeash(found))
                {
                    current_enemy = found;
                }
            }
            if (current_enemy != null)
            {
                if (unit.isCloseEnough(unit.getRange(current_enemy), current_enemy))
                {
                    unit.setBehaviour(new com.oddlabs.tt.model.behaviour.AttackBehaviour(unit, current_enemy));
                }
                else
                {
                    unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, current_enemy, unit.getRange(current_enemy), true, leashed_hunt_factory));
                }
                return;
            }
            if (!unit.isCloseEnough(com.oddlabs.tt.model.behaviour.GuardController.RETURN_THRESHOLD, guard_target))
            {
                wander_point = null;
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, guard_target, 0.0f, true, leashed_hunt_factory));
                return;
            }
            if (wander_point == null || unit.isCloseEnough(com.oddlabs.tt.model.behaviour.GuardController.WANDER_ARRIVE_THRESHOLD, wander_point))
            {
                java.util.Random random = unit.getOwner().getWorld().getRandom();
                float angle = (float)(random.nextFloat() * ((float)((java.lang.Math.PI * 2))));
                float radius = (float)(com.oddlabs.tt.model.behaviour.GuardController.MIN_WANDER_RADIUS + (float)(random.nextFloat() * ((float)(com.oddlabs.tt.model.behaviour.GuardController.WANDER_RADIUS - com.oddlabs.tt.model.behaviour.GuardController.MIN_WANDER_RADIUS))));
                int wx = guard_target.getGridX() + java.lang.Math.round((float)(((float)(java.lang.StrictMath.cos(angle))) * radius));
                int wy = guard_target.getGridY() + java.lang.Math.round((float)(((float)(java.lang.StrictMath.sin(angle))) * radius));
                if (wx == unit.getGridX() && wy == unit.getGridY())
                {
                    wx += 1;
                }
                wander_point = new com.oddlabs.tt.landscape.LandscapeTarget(wx, wy);
            }
            unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, wander_point, 0.0f, true, leashed_hunt_factory));
        }
        public GuardController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target guard_target) : base(1)
        {
            this.unit = unit;
            this.guard_target = guard_target;
            this.scan_filter = new com.oddlabs.tt.model.AttackScanFilter(unit.getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE);
            this.leashed_hunt_factory = new com.oddlabs.tt.model.behaviour.WalkBehaviour.HuntControllerFactory_L((com.oddlabs.tt.model.Unit u, com.oddlabs.tt.model.Selectable target) =>
            {
                return new com.oddlabs.tt.model.behaviour.LeashedHuntController(u, target, this.guard_target, com.oddlabs.tt.model.behaviour.GuardController.MAX_LEASH_DISTANCE);
            });
        }
    }
}

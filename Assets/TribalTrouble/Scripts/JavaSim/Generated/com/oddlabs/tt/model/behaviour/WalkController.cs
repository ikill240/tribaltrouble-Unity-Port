// Translated from WalkController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 8
    public class WalkController : com.oddlabs.tt.model.behaviour.Controller
    {
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.util.Target target;
        public bool scan_attack;
        public int nudges;
        public const int MAX_NUDGES = 4;
        public static int[][] SIDESTEPS;
        // Java: 24
        public override void decide()
        {
            if (shouldGiveUp(0))
            {
                unit.popController();
            }
            else
            {
                unit.setBehaviour(new com.oddlabs.tt.model.behaviour.WalkBehaviour(unit, target, 0.0f, scan_attack));
            }
        }
        // Java: 38
        public override bool onStuck()
        {
            com.oddlabs.tt.landscape.World world = unit.getOwner().getWorld();
            if (!world.isExtended() && !world.isFreeForm())
            {
                return false;
            }
            if (nudges >= com.oddlabs.tt.model.behaviour.WalkController.MAX_NUDGES)
            {
                return false;
            }
            int[] step = com.oddlabs.tt.model.behaviour.WalkController.SIDESTEPS[(nudges + unit.getGridX() + unit.getGridY()) & 7];
            nudges++;
            com.oddlabs.tt.pathfinder.UnitGrid grid = unit.getUnitGrid();
            int x = java.lang.Math.max(1, java.lang.Math.min(grid.getGridSize() - 2, unit.getGridX() + step[0]));
            int y = java.lang.Math.max(1, java.lang.Math.min(grid.getGridSize() - 2, unit.getGridY() + step[1]));
            com.oddlabs.tt.util.Target[] free = grid.findGridTargets(x, y, 1, true);
            if (free.Length == 0 || free[0] == null)
            {
                return false;
            }
            resetGiveUpCounters();
            com.oddlabs.tt.model.behaviour.WalkController aside = new com.oddlabs.tt.model.behaviour.WalkController(unit, free[0], false);
            aside.nudges = com.oddlabs.tt.model.behaviour.WalkController.MAX_NUDGES;
            unit.pushController(aside);
            return true;
        }
        // Java: 60
        public bool isAgressive()
        {
            return scan_attack;
        }
        // Java: 64
        public com.oddlabs.tt.util.Target getTarget()
        {
            return target;
        }
        static WalkController()
        {
            WalkController.SIDESTEPS = new int[][] { new int[] { 2, 0 }, new int[] { 0, 2 }, new int[] { (-2), 0 }, new int[] { 0, (-2) }, new int[] { 2, 2 }, new int[] { (-2), 2 }, new int[] { (-2), (-2) }, new int[] { 2, (-2) } };
        }
        public WalkController(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target t, bool scan_attack) : base(1)
        {
            this.unit = unit;
            this.target = t;
            this.scan_attack = scan_attack;
        }
    }
}

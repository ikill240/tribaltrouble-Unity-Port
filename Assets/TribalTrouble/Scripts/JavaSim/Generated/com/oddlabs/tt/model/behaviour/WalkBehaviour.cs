// Translated from WalkBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 16
    public class WalkBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const float WAIT_RETRY_DELAY = 0.5f;
        public const float MAX_WAIT_RETRY_DELAY = 5.0f;
        public const float EXT_WAIT_RETRY_DELAY = 0.25f;
        public const float EXT_MAX_WAIT_RETRY_DELAY = 1.5f;
        public static int[][] ASIDE;
        // Java: 32
        public interface HuntControllerFactory
        {
            // Java: 33
            com.oddlabs.tt.model.behaviour.Controller create(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target);
        }
        public sealed class HuntControllerFactory_L : java.lang.Object, HuntControllerFactory
        {
            readonly System.Func<com.oddlabs.tt.model.Unit, com.oddlabs.tt.model.Selectable, com.oddlabs.tt.model.behaviour.Controller> f;
            public HuntControllerFactory_L(System.Func<com.oddlabs.tt.model.Unit, com.oddlabs.tt.model.Selectable, com.oddlabs.tt.model.behaviour.Controller> f) { this.f = f; }
            public com.oddlabs.tt.model.behaviour.Controller create(com.oddlabs.tt.model.Unit p0, com.oddlabs.tt.model.Selectable p1) { return f(p0, p1); }
        }
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm;
        public com.oddlabs.tt.model.AttackScanFilter scan_filter;
        public bool scan_attack;
        public com.oddlabs.tt.model.behaviour.WalkBehaviour.HuntControllerFactory hunt_controller_factory;
        public com.oddlabs.tt.pathfinder.Movable blocking_movable;
        public int blocker_x;
        public int blocker_y;
        public float retry_delay_counter;
        public float retry_delay;
        public com.oddlabs.tt.pathfinder.PathTracker.State state;
        public float blocked_time;
        public int unstick_step;
        // Java: 80
        public bool isBlocking()
        {
            return ((object)state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED);
        }
        // Java: 85
        public void appendToolTip(com.oddlabs.tt.gui.ToolTipBox tool_tip_box)
        {
            tool_tip_box.append("WalkBehaviour: state=");
            tool_tip_box.append(java.lang.JO.toString(state));
            tool_tip_box.append(" | retry_delay=");
            tool_tip_box.append(java.lang.JC.f2l(retry_delay));
            tool_tip_box.append("(");
            tool_tip_box.append(java.lang.JC.f2l(retry_delay));
            tool_tip_box.append("s)");
            unit.getTracker().appendToolTip(tool_tip_box);
        }
        // Java: 96
        public void switchToMoving()
        {
            unit.switchAnimation(unit.getMetersPerSecond(), com.oddlabs.tt.model.Unit.Animation.MOVING);
        }
        // Java: 100
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (((object)state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED) || ((object)state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.SOFTBLOCKED))
            {
                blocked_time += t;
            }
            retry_delay_counter -= t;
            bool blocker_moved = blocking_movable != null && (blocking_movable.getGridX() != blocker_x || blocking_movable.getGridY() != blocker_y);
            if (retry_delay_counter > 0 && !blocker_moved)
            {
                return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
            }
            retry_delay_counter = 0;
            blocking_movable = null;
            com.oddlabs.tt.pathfinder.PathTracker tracker = unit.getTracker();
            state = tracker.animate((float)(unit.getMetersPerSecond() * t));
            {
                switch ((state).ordinal())
                {
                    case 0 /*OK*/:
                    {
                        blocked_time = 0;
                        unstick_step = 0;
                        switchToMoving();
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
                    }
                    case 1 /*OK_INTERRUPTIBLE*/:
                    {
                        blocked_time = 0;
                        unstick_step = 0;
                        retry_delay = (extendedRules() ? com.oddlabs.tt.model.behaviour.WalkBehaviour.EXT_WAIT_RETRY_DELAY : com.oddlabs.tt.model.behaviour.WalkBehaviour.WAIT_RETRY_DELAY);
                        switchToMoving();
                        scan();
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
                    }
                    case 2 /*DONE*/:
                    {
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
                    }
                    case 4 /*BLOCKED*/:
                    case 3 /*SOFTBLOCKED*/:
                    {
                        com.oddlabs.tt.pathfinder.Occupant blocker = tracker.getBlocker();
                        if ((blocker is com.oddlabs.tt.pathfinder.Movable movable))
                        {
                            blocking_movable = movable;
                            if (!blocking_movable.isDead())
                            {
                                blocking_movable.markBlocking();
                                blocker_x = blocking_movable.getGridX();
                                blocker_y = blocking_movable.getGridY();
                            }
                            else
                            {
                                blocking_movable = null;
                            }
                        }
                        if ((blocking_movable is com.oddlabs.tt.model.Unit friend) && extendedRules())
                        {
                            askToStepAside(friend, blocked_time > 1.5f);
                        }
                        if (com.oddlabs.tt.model.behaviour.WalkBehaviour.unstick_enabled && extendedRules())
                        {
                            unstick(tracker);
                        }
                        scan();
                        retry_delay = java.lang.Math.min((float)(2 * retry_delay), (extendedRules() ? com.oddlabs.tt.model.behaviour.WalkBehaviour.EXT_MAX_WAIT_RETRY_DELAY : com.oddlabs.tt.model.behaviour.WalkBehaviour.MAX_WAIT_RETRY_DELAY));
                        return doRetry();
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        public static bool unstick_enabled;
        // Java: 156
        public bool extendedRules()
        {
            com.oddlabs.tt.landscape.World world = unit.getOwner().getWorld();
            return world.isExtended() || world.isFreeForm();
        }
        // Java: 165
        public void askToStepAside(com.oddlabs.tt.model.Unit friend, bool insist)
        {
            if (friend.isDead() || friend.isMoving() || friend.getOwner().isEnemy(unit.getOwner()))
            {
                return;
            }
            bool ours = ((object)friend.getOwner() == (object)unit.getOwner());
            com.oddlabs.tt.model.behaviour.Controller c = friend.getCurrentController();
            bool idle = (c is com.oddlabs.tt.model.behaviour.IdleController);
            bool standing = (c is com.oddlabs.tt.model.behaviour.GuardController) || (c is com.oddlabs.tt.model.behaviour.DefendController) || (c is com.oddlabs.tt.model.behaviour.WaitRetryController) || (c is com.oddlabs.tt.model.behaviour.PatrolController);
            if (!(idle && (ours || insist)) && !(insist && standing))
            {
                return;
            }
            com.oddlabs.tt.pathfinder.UnitGrid grid = unit.getUnitGrid();
            int size = grid.getGridSize();
            float dx = unit.getDirectionX();
            float dy = unit.getDirectionY();
            int best = (-1);
            float best_score = java.lang.Float.MAX_VALUE;
            for (int i = 0; i < com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE.Length; i++)
            {
                int x = friend.getGridX() + com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE[i][0];
                int y = friend.getGridY() + com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE[i][1];
                if (x < 1 || y < 1 || x >= size - 1 || y >= size - 1 || grid.isGridOccupied(x, y) || grid.getRegion(x, y) == null)
                {
                    continue;
                }
                if (x == unit.getGridX() && y == unit.getGridY())
                {
                    continue;
                }
                float ahead = (float)((float)(com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE[i][0] * dx) + (float)(com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE[i][1] * dy));
                float score = (float)((float)(java.lang.Math.abs(ahead) + ((ahead > 0 ? 1.0f : 0.0f))) + ((i >= 4 ? 0.1f : 0.0f)));
                if (score < best_score)
                {
                    best_score = score;
                    best = i;
                }
            }
            if (best < 0)
            {
                return;
            }
            friend.pushController(new com.oddlabs.tt.model.behaviour.WalkController(friend, new com.oddlabs.tt.landscape.LandscapeTarget(friend.getGridX() + com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE[best][0], friend.getGridY() + com.oddlabs.tt.model.behaviour.WalkBehaviour.ASIDE[best][1]), false));
        }
        // Java: 206
        public void unstick(com.oddlabs.tt.pathfinder.PathTracker tracker)
        {
            if (unstick_step == 0 && blocked_time > 2.5f || unstick_step == 1 && blocked_time > 6.0f)
            {
                unstick_step++;
                tracker.setTarget(tracker_algorithm);
                retry_delay = com.oddlabs.tt.model.behaviour.WalkBehaviour.EXT_WAIT_RETRY_DELAY;
            }
            else if (unstick_step >= 2 && blocked_time > 10.0f)
            {
                unstick_step = 0;
                blocked_time = 0;
            }
        }
        // Java: 217
        public void scan()
        {
            if (scan_attack)
            {
                unit.scanVicinity(scan_filter);
                com.oddlabs.tt.model.Selectable s = scan_filter.removeTarget();
                if (s != null)
                {
                    unit.getCurrentController().resetGiveUpCounters();
                    unit.pushController(hunt_controller_factory.create(unit, s));
                }
            }
        }
        // Java: 232
        public com.oddlabs.tt.model.behaviour.Behaviour.State doRetry()
        {
            retry_delay_counter = retry_delay;
            unit.switchToIdleAnimation();
            return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
        }
        // Java: 238
        public void forceInterrupted()
        {
        }
        static WalkBehaviour()
        {
            WalkBehaviour.ASIDE = new int[][] { new int[] { 1, 0 }, new int[] { (-1), 0 }, new int[] { 0, 1 }, new int[] { 0, (-1) }, new int[] { 1, 1 }, new int[] { (-1), 1 }, new int[] { 1, (-1) }, new int[] { (-1), (-1) } };
            WalkBehaviour.unstick_enabled = true;
        }
        public WalkBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm, bool scan_attack) : this(unit, tracker_algorithm, scan_attack, new com.oddlabs.tt.model.behaviour.WalkBehaviour.HuntControllerFactory_L((com.oddlabs.tt.model.Unit a0, com.oddlabs.tt.model.Selectable a1) => { return new com.oddlabs.tt.model.behaviour.HuntController(a0, a1); }))
        {
        }
        public WalkBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm, bool scan_attack, com.oddlabs.tt.model.behaviour.WalkBehaviour.HuntControllerFactory hunt_controller_factory) : base()
        {
            this.unit = unit;
            this.tracker_algorithm = tracker_algorithm;
            this.scan_attack = scan_attack;
            this.hunt_controller_factory = hunt_controller_factory;
            scan_filter = new com.oddlabs.tt.model.AttackScanFilter(unit.getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE);
            retry_delay = (extendedRules() ? com.oddlabs.tt.model.behaviour.WalkBehaviour.EXT_WAIT_RETRY_DELAY : com.oddlabs.tt.model.behaviour.WalkBehaviour.WAIT_RETRY_DELAY);
            unit.getTracker().setTarget(tracker_algorithm);
        }
        public WalkBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target t, float range, bool scan_attack) : this(unit, new com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm(unit.getUnitGrid(), range, t), scan_attack)
        {
        }
        public WalkBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target t, float range, bool scan_attack, com.oddlabs.tt.model.behaviour.WalkBehaviour.HuntControllerFactory hunt_controller_factory) : this(unit, new com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm(unit.getUnitGrid(), range, t), scan_attack, hunt_controller_factory)
        {
        }
    }
}

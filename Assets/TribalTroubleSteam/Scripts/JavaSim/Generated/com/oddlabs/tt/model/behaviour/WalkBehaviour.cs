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
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm;
        public com.oddlabs.tt.model.AttackScanFilter scan_filter;
        public bool scan_attack;
        public com.oddlabs.tt.pathfinder.Movable blocking_movable;
        public int blocker_x;
        public int blocker_y;
        public float retry_delay_counter;
        public float retry_delay;
        public com.oddlabs.tt.pathfinder.PathTracker.State state;
        // Java: 46
        public bool isBlocking()
        {
            return ((object)state == (object)com.oddlabs.tt.pathfinder.PathTracker.State.BLOCKED);
        }
        // Java: 51
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
        // Java: 62
        public void switchToMoving()
        {
            unit.switchAnimation(unit.getMetersPerSecond(), com.oddlabs.tt.model.Unit.Animation.MOVING);
        }
        // Java: 66
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
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
                        switchToMoving();
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
                    }
                    case 1 /*OK_INTERRUPTIBLE*/:
                    {
                        retry_delay = com.oddlabs.tt.model.behaviour.WalkBehaviour.WAIT_RETRY_DELAY;
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
                        scan();
                        retry_delay = java.lang.Math.min((float)(2 * retry_delay), com.oddlabs.tt.model.behaviour.WalkBehaviour.MAX_WAIT_RETRY_DELAY);
                        return doRetry();
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 109
        public void scan()
        {
            if (scan_attack)
            {
                unit.scanVicinity(scan_filter);
                com.oddlabs.tt.model.Selectable s = scan_filter.removeTarget();
                if (s != null)
                {
                    unit.getCurrentController().resetGiveUpCounters();
                    unit.pushController(new com.oddlabs.tt.model.behaviour.HuntController(unit, s));
                }
            }
        }
        // Java: 124
        public com.oddlabs.tt.model.behaviour.Behaviour.State doRetry()
        {
            retry_delay_counter = retry_delay;
            unit.switchToIdleAnimation();
            return com.oddlabs.tt.model.behaviour.Behaviour.State.INTERRUPTIBLE;
        }
        // Java: 130
        public void forceInterrupted()
        {
        }
        public WalkBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.pathfinder.TrackerAlgorithm tracker_algorithm, bool scan_attack) : base()
        {
            this.unit = unit;
            this.tracker_algorithm = tracker_algorithm;
            this.scan_attack = scan_attack;
            scan_filter = new com.oddlabs.tt.model.AttackScanFilter(unit.getOwner(), com.oddlabs.tt.model.AttackScanFilter.UNIT_RANGE);
            retry_delay = com.oddlabs.tt.model.behaviour.WalkBehaviour.WAIT_RETRY_DELAY;
            unit.getTracker().setTarget(tracker_algorithm);
        }
        public WalkBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.util.Target t, float range, bool scan_attack) : this(unit, new com.oddlabs.tt.pathfinder.TargetTrackerAlgorithm(unit.getUnitGrid(), range, t), scan_attack)
        {
        }
    }
}

// Translated from DieBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 6
    public class DieBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const float SECONDS_PER_DEATH = 3.0f;
        public const float LYING_SECONDS = 1.0f;
        public const float MOVING_SECONDS = 60.0f;
        public const int MOVING_METERS = 3;
        // Java: 13
        public sealed class DieState : java.lang.Enum
        {
            public static DieState[] values() { return (DieState[])VALUES_.Clone(); }
            public static DieState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly DieState DYING = new DieState("DYING", 0);
            public static readonly DieState LYING = new DieState("LYING", 1);
            public static readonly DieState MOVING = new DieState("MOVING", 2);
            public static readonly DieState[] VALUES_ = new DieState[] { DYING, LYING, MOVING };
            public DieState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.model.Unit unit;
        public float anim_time;
        public com.oddlabs.tt.model.behaviour.DieBehaviour.DieState state;
        public float offset_z;
        public float dz;
        // Java: 31
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            anim_time -= t;
            offset_z -= (float)(dz * t);
            if (anim_time < 0)
            {
                switchState();
            }
            return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
        }
        // Java: 40
        public void switchState()
        {
            switch ((state).ordinal())
            {
                case 0 /*DYING*/:
                {
                    anim_time += com.oddlabs.tt.model.behaviour.DieBehaviour.LYING_SECONDS;
                    state = com.oddlabs.tt.model.behaviour.DieBehaviour.DieState.LYING;
                    break;
                }
                case 1 /*LYING*/:
                {
                    anim_time += com.oddlabs.tt.model.behaviour.DieBehaviour.MOVING_SECONDS;
                    state = com.oddlabs.tt.model.behaviour.DieBehaviour.DieState.MOVING;
                    dz = (float)(com.oddlabs.tt.model.behaviour.DieBehaviour.MOVING_METERS / com.oddlabs.tt.model.behaviour.DieBehaviour.MOVING_SECONDS);
                    break;
                }
                case 2 /*MOVING*/:
                {
                    unit.remove();
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
        // Java: 58
        public float getOffsetZ()
        {
            return offset_z;
        }
        // Java: 62
        public bool isBlocking()
        {
            throw new System.InvalidOperationException();
        }
        // Java: 67
        public void forceInterrupted()
        {
        }
        private void init_DieBehaviour()
        {
            this.anim_time = com.oddlabs.tt.model.behaviour.DieBehaviour.SECONDS_PER_DEATH;
            this.state = com.oddlabs.tt.model.behaviour.DieBehaviour.DieState.DYING;
            this.offset_z = 0;
            this.dz = 0;
        }
        public DieBehaviour(com.oddlabs.tt.model.Unit unit) : base()
        {
            init_DieBehaviour();
            this.unit = unit;
            unit.switchAnimation((float)(1.0f / anim_time), com.oddlabs.tt.model.Unit.Animation.DYING);
        }
    }
}

// Translated from AttackBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 11
    public class AttackBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const float SECONDS_PER_ATTACK = 2.0f;
        // Java: 14
        public sealed class AttackState : java.lang.Enum
        {
            public static AttackState[] values() { return (AttackState[])VALUES_.Clone(); }
            public static AttackState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly AttackState THROWING = new AttackState("THROWING", 0);
            public static readonly AttackState RELEASED = new AttackState("RELEASED", 1);
            public static readonly AttackState[] VALUES_ = new AttackState[] { THROWING, RELEASED };
            public AttackState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.model.Selectable target;
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.ShipAllocation allocation;
        public com.oddlabs.tt.model.Ship ship;
        public float anim_time;
        public com.oddlabs.tt.model.behaviour.AttackBehaviour.AttackState state;
        // Java: 52
        public bool isBlocking()
        {
            return true;
        }
        // Java: 57
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (ship != null && unit.isMounted())
            {
                allocation.updateFinal(unit, ship);
            }
            {
                switch ((state).ordinal())
                {
                    case 0 /*THROWING*/:
                    {
                        updateAttack(t);
                        if (anim_time <= 0)
                        {
                            if (unit.isMounted())
                            {
                                unit.getWeaponFactory().attack(unit, target, 3.0f);
                            }
                            else
                            {
                                unit.getWeaponFactory().attack(unit, target);
                            }
                            anim_time += (float)(com.oddlabs.tt.model.behaviour.AttackBehaviour.SECONDS_PER_ATTACK - unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / com.oddlabs.tt.model.behaviour.AttackBehaviour.SECONDS_PER_ATTACK)));
                            state = com.oddlabs.tt.model.behaviour.AttackBehaviour.AttackState.RELEASED;
                        }
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
                    }
                    case 1 /*RELEASED*/:
                    {
                        updateAttack(t);
                        return (anim_time > 0 ? com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE : com.oddlabs.tt.model.behaviour.Behaviour.State.DONE);
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 86
        public void updateAttack(float t)
        {
            anim_time -= t;
            unit.aimAtTarget(target);
        }
        // Java: 91
        public void forceInterrupted()
        {
        }
        private void init_AttackBehaviour()
        {
            this.state = com.oddlabs.tt.model.behaviour.AttackBehaviour.AttackState.THROWING;
        }
        public AttackBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target) : base()
        {
            init_AttackBehaviour();
            this.unit = unit;
            if ((target is com.oddlabs.tt.model.Building t))
            {
                this.target = t.getBase();
            }
            else
            {
                this.target = target;
            }
            this.allocation = null;
            this.ship = null;
            anim_time = unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / com.oddlabs.tt.model.behaviour.AttackBehaviour.SECONDS_PER_ATTACK));
            unit.switchAnimation((float)(1.0f / com.oddlabs.tt.model.behaviour.AttackBehaviour.SECONDS_PER_ATTACK), com.oddlabs.tt.model.Unit.Animation.THROWING);
        }
        public AttackBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.Selectable target, com.oddlabs.tt.model.ShipAllocation allocation, com.oddlabs.tt.model.Ship ship) : base()
        {
            init_AttackBehaviour();
            this.unit = unit;
            if ((target is com.oddlabs.tt.model.Building t))
            {
                this.target = t.getBase();
            }
            else
            {
                this.target = target;
            }
            this.ship = ship;
            this.allocation = allocation;
            anim_time = unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / com.oddlabs.tt.model.behaviour.AttackBehaviour.SECONDS_PER_ATTACK));
            unit.switchAnimation((float)(1.0f / com.oddlabs.tt.model.behaviour.AttackBehaviour.SECONDS_PER_ATTACK), com.oddlabs.tt.model.Unit.Animation.THROWING);
        }
    }
}

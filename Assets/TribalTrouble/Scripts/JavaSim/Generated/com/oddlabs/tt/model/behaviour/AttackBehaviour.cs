// Translated from AttackBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 12
    public class AttackBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        public const float BASE_SECONDS_PER_ATTACK = 2.0f;
        // Java: 15
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
        public float seconds_per_attack;
        public float anim_time;
        public com.oddlabs.tt.model.behaviour.AttackBehaviour.AttackState state;
        // Java: 56
        public bool isBlocking()
        {
            return true;
        }
        // Java: 61
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            if (ship != null && unit.isMounted())
            {
                float x = ship.getPositionX();
                float y = ship.getPositionY();
                float dx = ship.getDirectionX();
                float dy = ship.getDirectionY();
                float ox = allocation.getOffset().x_f;
                float oy = allocation.getOffset().y_f;
                float gx = (float)((float)(x + (float)(dx * ox)) - (float)(dy * oy));
                float gy = (float)((float)(y + (float)(dy * ox)) + (float)(dx * oy));
                unit.setPosition(gx, gy);
                int gridSize = ship.getUnitGrid().getGridSize();
                int gridX = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gx), 0, gridSize - 1);
                int gridY = java.lang.Math.clamp(com.oddlabs.tt.pathfinder.UnitGrid.toGridCoordinate(gy), 0, gridSize - 1);
                unit.setGridPosition(gridX, gridY);
                float rx = allocation.getRotation().x_f;
                float ry = allocation.getRotation().y_f;
                unit.setDirection((float)((float)(rx * dx) - (float)(ry * dy)), (float)((float)(ry * dx) + (float)(rx * dy)));
            }
            {
                switch ((state).ordinal())
                {
                    case 0 /*THROWING*/:
                    {
                        updateAttack(t);
                        if (anim_time <= 0)
                        {
                            float factor = (float)(((unit.isMounted() ? 3.0f : 1.0f)) * unit.getCourageCombatMultiplier());
                            unit.getWeaponFactory().attack(unit, target, factor);
                            anim_time += (float)(seconds_per_attack - unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / seconds_per_attack)));
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
        // Java: 103
        public void updateAttack(float t)
        {
            anim_time -= t;
            unit.aimAtTarget(target);
        }
        // Java: 109
        public com.oddlabs.tt.model.Selectable getTarget()
        {
            return target;
        }
        // Java: 113
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
            this.seconds_per_attack = (float)(com.oddlabs.tt.model.behaviour.AttackBehaviour.BASE_SECONDS_PER_ATTACK / unit.getCourageAttackSpeedMultiplier());
            anim_time = unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / seconds_per_attack));
            unit.switchAnimation((float)(1.0f / seconds_per_attack), com.oddlabs.tt.model.Unit.Animation.THROWING);
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
            this.seconds_per_attack = (float)(com.oddlabs.tt.model.behaviour.AttackBehaviour.BASE_SECONDS_PER_ATTACK / unit.getCourageAttackSpeedMultiplier());
            anim_time = unit.getWeaponFactory().getSecondsPerRelease((float)(1.0f / seconds_per_attack));
            unit.switchAnimation((float)(1.0f / seconds_per_attack), com.oddlabs.tt.model.Unit.Animation.THROWING);
        }
    }
}

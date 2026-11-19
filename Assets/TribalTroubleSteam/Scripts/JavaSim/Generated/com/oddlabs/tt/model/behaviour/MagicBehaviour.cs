// Translated from MagicBehaviour.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model.behaviour
{
    // Java: 9
    public class MagicBehaviour : java.lang.Object, com.oddlabs.tt.model.behaviour.Behaviour
    {
        // Java: 10
        public sealed class MagicState : java.lang.Enum
        {
            public static MagicState[] values() { return (MagicState[])VALUES_.Clone(); }
            public static MagicState valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
            public static readonly MagicState PREPARING = new MagicState("PREPARING", 0);
            public static readonly MagicState CASTING = new MagicState("CASTING", 1);
            public static readonly MagicState ENDING = new MagicState("ENDING", 2);
            public static readonly MagicState[] VALUES_ = new MagicState[] { PREPARING, CASTING, ENDING };
            public MagicState(string name_, int ordinal_) : base(name_, ordinal_)
            {
            }
        }
        public com.oddlabs.tt.model.Unit unit;
        public com.oddlabs.tt.model.weapon.MagicFactory magic_factory;
        public com.oddlabs.tt.model.behaviour.MagicController controller;
        public com.oddlabs.tt.model.weapon.Magic magic;
        public float anim_time;
        public com.oddlabs.tt.model.behaviour.MagicBehaviour.MagicState state;
        // Java: 33
        public bool isBlocking()
        {
            return true;
        }
        // Java: 38
        public com.oddlabs.tt.model.behaviour.Behaviour.State animate(float t)
        {
            anim_time -= t;
            {
                switch ((state).ordinal())
                {
                    case 0 /*PREPARING*/:
                    {
                        if (anim_time <= 0)
                        {
                            state = com.oddlabs.tt.model.behaviour.MagicBehaviour.MagicState.CASTING;
                            magic = magic_factory.execute(unit);
                            anim_time += (float)(magic_factory.getSecondsPerRelease() - magic_factory.getSecondsPerInit());
                        }
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
                    }
                    case 1 /*CASTING*/:
                    {
                        if (anim_time <= 0)
                        {
                            state = com.oddlabs.tt.model.behaviour.MagicBehaviour.MagicState.ENDING;
                            unit.getOwner().getWorld().getAnimationManagerGameTime().registerAnimation(magic);
                            anim_time += (float)(magic_factory.getSecondsPerAnim() - magic_factory.getSecondsPerRelease());
                        }
                        return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
                    }
                    case 2 /*ENDING*/:
                    {
                        if (anim_time > 0)
                        {
                            return com.oddlabs.tt.model.behaviour.Behaviour.State.UNINTERRUPTIBLE;
                        }
                        else
                        {
                            controller.popNextTime();
                            return com.oddlabs.tt.model.behaviour.Behaviour.State.DONE;
                        }
                    }
                }
                throw new System.InvalidOperationException("MatchException");
            }
        }
        // Java: 69
        public void forceInterrupted()
        {
            if (magic != null)
            {
                magic.interrupt();
            }
        }
        private void init_MagicBehaviour()
        {
            this.state = com.oddlabs.tt.model.behaviour.MagicBehaviour.MagicState.PREPARING;
        }
        public MagicBehaviour(com.oddlabs.tt.model.Unit unit, com.oddlabs.tt.model.weapon.MagicFactory magic_factory, com.oddlabs.tt.model.behaviour.MagicController controller) : base()
        {
            init_MagicBehaviour();
            this.unit = unit;
            this.magic_factory = magic_factory;
            this.controller = controller;
            anim_time = magic_factory.getSecondsPerInit();
            unit.switchAnimation((float)(1.0f / magic_factory.getSecondsPerAnim()), com.oddlabs.tt.model.Unit.Animation.MAGIC);
        }
    }
}

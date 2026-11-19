// Translated from IntervalTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.trigger
{
    // Java: 9
    public abstract class IntervalTrigger : java.lang.Object
    {
        public float after_done_time;
        public com.oddlabs.tt.animation.AnimationManager animation_manager;
        public com.oddlabs.tt.animation.TimerAnimation timer;
        // Java: 25
        public virtual void triggered()
        {
            this.timer.stop();
            this.timer = new com.oddlabs.tt.animation.TimerAnimation(animation_manager, new com.oddlabs.tt.animation.Updatable_L((object timer_l) =>
            {
                com.oddlabs.tt.animation.TimerAnimation timer = (com.oddlabs.tt.animation.TimerAnimation)timer_l;
                timer.stop();
                done();
            }), after_done_time);
            this.timer.start();
        }
        // Java: 34
        public void abort()
        {
            timer.stop();
            timer = null;
        }
        // Java: 39
        public abstract void check();
        // Java: 41
        public abstract void done();
        public IntervalTrigger(com.oddlabs.tt.landscape.World world, float check_interval, float after_done_time) : this(check_interval, after_done_time, world.getAnimationManagerRealTime())
        {
        }
        public IntervalTrigger(float check_interval, float after_done_time, com.oddlabs.tt.animation.AnimationManager animation_manager) : base()
        {
            this.after_done_time = after_done_time;
            this.animation_manager = animation_manager;
            this.timer = new com.oddlabs.tt.animation.TimerAnimation(animation_manager, new com.oddlabs.tt.animation.Updatable_L((object unused_l) =>
            {
                com.oddlabs.tt.animation.TimerAnimation unused = (com.oddlabs.tt.animation.TimerAnimation)unused_l;
                check();
            }), check_interval);
            timer.start();
        }
    }
}

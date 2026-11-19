// Translated from TimerAnimation.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.animation
{
    // Java: 8
    public class TimerAnimation : java.lang.Object, com.oddlabs.tt.animation.Animated
    {
        public com.oddlabs.tt.animation.AnimationManager manager;
        public float time;
        public float interval;
        public com.oddlabs.tt.animation.Updatable timer_owner;
        public bool running;
        // Java: 25
        public override string toString()
        {
            return "TimerAnimation{owner=" + java.lang.JStr.valueOf(timer_owner) + "}";
        }
        // Java: 30
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
            checksum.update(time);
        }
        // Java: 35
        public bool isRunning()
        {
            return running;
        }
        // Java: 39
        public void stop()
        {
            running = false;
            manager.removeAnimation(this);
        }
        // Java: 44
        public void start()
        {
            running = true;
            manager.registerAnimation(this);
        }
        // Java: 49
        public void setTimerOwner(com.oddlabs.tt.animation.Updatable obj)
        {
            this.timer_owner = obj;
        }
        // Java: 53
        public com.oddlabs.tt.animation.Updatable getTimerOwner()
        {
            return timer_owner;
        }
        // Java: 57
        public void setTimerInterval(float interval)
        {
            this.interval = interval;
        }
        // Java: 61
        public void resetTime()
        {
            time = 0;
        }
        // Java: 65
        public void animate(float t)
        {
            time += t;
            while (time > interval)
            {
                time -= java.lang.Math.max(t, interval);
                if (timer_owner != null)
                {
                    timer_owner.update(this);
                }
            }
        }
        private void init_TimerAnimation()
        {
            this.time = 0;
            this.running = false;
        }
        public TimerAnimation(com.oddlabs.tt.animation.Updatable owner, float interval) : this(com.oddlabs.tt.@event.LocalEventQueue.getQueue().getManager(), owner, interval)
        {
        }
        public TimerAnimation(com.oddlabs.tt.animation.AnimationManager manager, com.oddlabs.tt.animation.Updatable owner, float interval) : base()
        {
            init_TimerAnimation();
            this.manager = manager;
            this.interval = interval;
            this.timer_owner = owner;
        }
    }
}

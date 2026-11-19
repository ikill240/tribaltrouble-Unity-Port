// Translated from CounterLabel.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.gui
{
    // Java: 8
    public class CounterLabel : com.oddlabs.tt.gui.Label, com.oddlabs.tt.animation.Animated
    {
        public const string colon = ":";
        public float initial_seconds;
        public bool render_seconds;
        public com.oddlabs.tt.animation.AnimationManager manager;
        public float seconds;
        // Java: 22
        public void start(com.oddlabs.tt.animation.AnimationManager manager)
        {
            this.manager = manager;
            setTime(initial_seconds);
            manager.registerAnimation(this);
        }
        // Java: 28
        public void stop()
        {
            if (manager != null)
            {
                manager.removeAnimation(this);
            }
        }
        // Java: 33
        public void setTime(float seconds)
        {
            this.seconds = seconds;
            int rest_seconds = (java.lang.JC.f2i(seconds)) % 60;
            int minutes = ((java.lang.JC.f2i(seconds)) / 60) % 60;
            int hours = ((java.lang.JC.f2i(seconds)) / 60) / 60;
            if (!render_seconds && seconds > 0)
            {
                minutes++;
                if (minutes == 60)
                {
                    hours++;
                    minutes = 0;
                }
            }
            clear();
            append(hours);
            append(com.oddlabs.tt.gui.CounterLabel.colon);
            if (minutes < 10)
            {
                append(0);
            }
            append(minutes);
            if (render_seconds)
            {
                append(com.oddlabs.tt.gui.CounterLabel.colon);
                if (rest_seconds < 10)
                {
                    append(0);
                }
                append(rest_seconds);
            }
        }
        // Java: 59
        public void animate(float t)
        {
            setTime((float)(seconds - t));
        }
        public CounterLabel(float seconds, com.oddlabs.tt.font.Font font, bool render_seconds) : base("", font, 300)
        {
            this.render_seconds = render_seconds;
            initial_seconds = seconds;
            setTime(initial_seconds);
        }
        // mixin of default method Animated.updateChecksum
        public void updateChecksum(com.oddlabs.tt.util.StateChecksum checksum)
        {
        }
    }
}

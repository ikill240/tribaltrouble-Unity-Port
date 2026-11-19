// Translated from TutorialTrigger.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.tutorial
{
    // Java: 3
    public abstract class TutorialTrigger : java.lang.Object
    {
        public float check_interval;
        public float after_done_time;
        public string text_key;
        public object[] format_args;
        // Java: 20
        public float getCheckInterval()
        {
            return check_interval;
        }
        // Java: 24
        public float getAfterDoneTime()
        {
            return after_done_time;
        }
        // Java: 28
        public string getTextKey()
        {
            return text_key;
        }
        // Java: 32
        public object[] getFormatArgs()
        {
            return format_args;
        }
        // Java: 36
        public abstract void run(com.oddlabs.tt.tutorial.Tutorial tutorial);
        public TutorialTrigger(float check_interval, float after_done_time, string text_key) : this(check_interval, after_done_time, text_key, null)
        {
        }
        public TutorialTrigger(float check_interval, float after_done_time, string text_key, object[] format_args) : base()
        {
            this.check_interval = check_interval;
            this.after_done_time = after_done_time;
            this.text_key = text_key;
            this.format_args = format_args;
        }
    }
}

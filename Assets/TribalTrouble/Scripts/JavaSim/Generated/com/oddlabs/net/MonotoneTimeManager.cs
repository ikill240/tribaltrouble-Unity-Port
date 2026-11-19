// Translated from MonotoneTimeManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public class MonotoneTimeManager : java.lang.Object, com.oddlabs.net.TimeManager
    {
        public com.oddlabs.net.TimeManager source;
        public long last_time;
        // Java: 14
        public long getMillis()
        {
            long new_time = source.getMillis();
            this.last_time = java.lang.Math.max(last_time, new_time);
            return last_time;
        }
        public MonotoneTimeManager(com.oddlabs.net.TimeManager source) : base()
        {
            this.source = source;
            this.last_time = source.getMillis();
        }
    }
}

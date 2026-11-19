// Translated from TimeManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 3
    public interface TimeManager
    {
        // Java: 5
        long getMillis();
    }
    public sealed class TimeManager_L : java.lang.Object, TimeManager
    {
        readonly System.Func<long> f;
        public TimeManager_L(System.Func<long> f) { this.f = f; }
        public long getMillis() { return f(); }
    }
}

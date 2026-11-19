// Steam port: crash reports. The Java game uploads them to bug.tribaltrouble.org, where Oddlabs replays the crashed run
// with the Java build; a report from this port could not be replayed there, so none is sent (the caller shows the
// error as for a failed upload, and the crash is in the Unity log).
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.bugreport
{
    public class BugReporter : java.lang.Object
    {
        BugReporter() { }

        public static java.net.URI reportUri() => java.net.URI.create("https://bug." + com.oddlabs.tt.global.Settings.OFFICIAL_DOMAIN + com.oddlabs.bugreport.BugReportProtocol.REPORT_PATH);

        public static string submit(string stack_trace, java.nio.file.Path event_log)
            => throw new System.IO.IOException("Crash reports are only sent by the Java game");

        public static string submit(System.Exception t, java.nio.file.Path event_log)
            => submit(t == null ? "" : t.ToString(), event_log);
    }
}

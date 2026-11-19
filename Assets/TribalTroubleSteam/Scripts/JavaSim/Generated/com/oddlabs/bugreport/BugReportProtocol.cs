// Translated from BugReportProtocol.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.bugreport
{
    // Java: 10
    public class BugReportProtocol : java.lang.Object
    {
        public const string REPORT_PATH = "/report";
        public const string HEALTH_PATH = "/health";
        public const string CONTENT_TYPE = "application/json";
        public const int MAX_EVENT_LOG_BYTES = 33554432;
        public const int MAX_STACK_TRACE_CHARS = 262144;
        public const int MAX_REQUEST_BYTES = 50331648;
        public BugReportProtocol() : base()
        {
        }
    }
}

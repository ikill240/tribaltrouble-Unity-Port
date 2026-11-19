// Translated from BugReport.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.bugreport
{
    // Java: 17
    public class BugReport : java.lang.Object
    {
        public string version() { return version_f; }
        public string commit() { return commit_f; }
        public bool dirty() { return dirty_f; }
        public string os() { return os_f; }
        public string java_() { return java_f; }
        public string stack_trace() { return stack_trace_f; }
        public sbyte[] event_log() { return event_log_f; }
        public BugReport(string version, string commit, bool dirty, string os, string java_, string stack_trace, sbyte[] event_log)
        {
            this.version_f = version;
            this.commit_f = commit;
            this.dirty_f = dirty;
            this.os_f = os;
            this.java_f = java_;
            this.stack_trace_f = stack_trace;
            this.event_log_f = event_log;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is BugReport r)) return false; return true && java.lang.JO.equals(version_f, r.version_f) && java.lang.JO.equals(commit_f, r.commit_f) && java.lang.JO.equals(dirty_f, r.dirty_f) && java.lang.JO.equals(os_f, r.os_f) && java.lang.JO.equals(java_f, r.java_f) && java.lang.JO.equals(stack_trace_f, r.stack_trace_f) && java.lang.JO.equals(event_log_f, r.event_log_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(version_f); h = 31 * h + java.lang.JO.hashCode(commit_f); h = 31 * h + java.lang.JO.hashCode(dirty_f); h = 31 * h + java.lang.JO.hashCode(os_f); h = 31 * h + java.lang.JO.hashCode(java_f); h = 31 * h + java.lang.JO.hashCode(stack_trace_f); h = 31 * h + java.lang.JO.hashCode(event_log_f); } return h; }
        public override string toString() { return "BugReport[" + "version=" + java.lang.JO.toString(version_f) + ", commit=" + java.lang.JO.toString(commit_f) + ", dirty=" + java.lang.JO.toString(dirty_f) + ", os=" + java.lang.JO.toString(os_f) + ", java=" + java.lang.JO.toString(java_f) + ", stack_trace=" + java.lang.JO.toString(stack_trace_f) + ", event_log=" + java.lang.JO.toString(event_log_f) + "]"; }
        public string version_f;
        public string commit_f;
        public bool dirty_f;
        public string os_f;
        public string java_f;
        public string stack_trace_f;
        public sbyte[] event_log_f;
    }
}

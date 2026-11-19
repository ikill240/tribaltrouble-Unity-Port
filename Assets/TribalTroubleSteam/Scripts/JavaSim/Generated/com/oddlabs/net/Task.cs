// Translated from Task.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 3
    public interface Task
    {
        // Java: 4
        void cancel();
    }
    public sealed class Task_L : java.lang.Object, Task
    {
        readonly System.Action f;
        public Task_L(System.Action f) { this.f = f; }
        public void cancel() { f(); }
    }
}

// Translated from BlockingTask.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 5
    public class BlockingTask : java.lang.Object, com.oddlabs.net.Task
    {
        public int id;
        public bool cancelled;
        public com.oddlabs.net.TaskThread.TaskResult result;
        // Java: 14
        public void cancel()
        {
            cancelled = true;
        }
        public BlockingTask(int id) : base()
        {
            this.id = id;
        }
    }
}

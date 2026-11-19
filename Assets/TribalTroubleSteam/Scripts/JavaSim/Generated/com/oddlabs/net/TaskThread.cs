// Translated from TaskThread.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 13
    public class TaskThread : java.lang.Object
    {
        public java.util.Map id_to_callable;
        public java.util.List tasks;
        public java.util.List finished_tasks;
        public object @lock;
        public java.lang.Runnable notification_action;
        public int current_id;
        public java.lang.Thread thread;
        public bool finished;
        public com.oddlabs.@event.Deterministic deterministic;
        // Java: 30
        public interface TaskResult
        {
            // Java: 31
            void deliverResult(com.oddlabs.net.TaskExecutorLoopbackInterface callback);
        }
        public sealed class TaskResult_L : java.lang.Object, TaskResult
        {
            readonly System.Action<com.oddlabs.net.TaskExecutorLoopbackInterface> f;
            public TaskResult_L(System.Action<com.oddlabs.net.TaskExecutorLoopbackInterface> f) { this.f = f; }
            public void deliverResult(com.oddlabs.net.TaskExecutorLoopbackInterface p0) { f(p0); }
        }
        // Java: 34
        public class TaskFailed : java.lang.Object, com.oddlabs.net.TaskThread.TaskResult
        {
            public System.Exception result;
            // Java: 41
            public void deliverResult(com.oddlabs.net.TaskExecutorLoopbackInterface callback)
            {
                callback.taskFailed(result);
            }
            public TaskFailed(System.Exception e) : base()
            {
                this.result = e;
            }
        }
        // Java: 47
        public class TaskSucceeded : java.lang.Object, com.oddlabs.net.TaskThread.TaskResult
        {
            public object result;
            // Java: 54
            public void deliverResult(com.oddlabs.net.TaskExecutorLoopbackInterface callback)
            {
                callback.taskCompleted(result);
            }
            public TaskSucceeded(object result) : base()
            {
                this.result = result;
            }
        }
        // Java: 60
        public void processTasks()
        {
            while (!finished)
            {
                com.oddlabs.net.BlockingTask task = default;
                com.oddlabs.net.Callable callable = default;
                lock ((@lock))
                {
                    while (tasks.isEmpty())
                    {
                        try
                        {
                            @lock.wait();
                        }
                        catch (System.Exception unused)
                        {
                        }
                    }
                    task = ((com.oddlabs.net.BlockingTask)tasks.getFirst());
                    callable = lookupCallable(task);
                }
                com.oddlabs.net.TaskThread.TaskResult result = default;
                try
                {
                    object callable_result = callable.call();
                    result = new com.oddlabs.net.TaskThread.TaskSucceeded(callable_result);
                }
                catch (System.Exception e)
                {
                    result = new com.oddlabs.net.TaskThread.TaskFailed(e);
                }
                lock ((@lock))
                {
                    task.result = result;
                    tasks.removeFirst();
                    finished_tasks.add(task);
                }
            }
            if (notification_action != null)
            {
                notification_action.run();
            }
        }
        // Java: 92
        public com.oddlabs.@event.Deterministic getDeterministic()
        {
            return deterministic;
        }
        // Java: 96
        public com.oddlabs.net.Task addTask(com.oddlabs.net.Callable callable)
        {
            com.oddlabs.net.BlockingTask task = default;
            lock ((@lock))
            {
                int task_id = current_id++;
                id_to_callable.put(task_id, callable);
                task = new com.oddlabs.net.BlockingTask(task_id);
                tasks.add(task);
                @lock.notify();
            }
            if (!deterministic.isPlayback() && thread == null)
            {
                this.thread = new java.lang.Thread(new java.lang.Runnable_L(() => { this.processTasks(); }));
                this.thread.setName("Task executor thread");
                this.thread.setDaemon(true);
                this.thread.start();
            }
            return task;
        }
        // Java: 114
        public void poll()
        {
            while (true)
            {
                com.oddlabs.net.BlockingTask task = default;
                com.oddlabs.net.Callable callable = default;
                lock ((@lock))
                {
                    if (!deterministic.log(!finished_tasks.isEmpty()))
                    {
                        if (!tasks.isEmpty())
                        {
                            com.oddlabs.net.BlockingTask current_task = ((com.oddlabs.net.BlockingTask)tasks.getFirst());
                            if (current_task.cancelled && thread != null)
                            {
                                thread.interrupt();
                            }
                        }
                        return;
                    }
                    task = ((com.oddlabs.net.BlockingTask)deterministic.log((deterministic.isPlayback() ? (com.oddlabs.net.BlockingTask)(null) : ((com.oddlabs.net.BlockingTask)finished_tasks.removeFirst()))));
                    callable = lookupCallable(task);
                }
                if (!task.cancelled)
                {
                    task.result.deliverResult(callable);
                }
            }
        }
        // Java: 136
        public com.oddlabs.net.Callable lookupCallable(com.oddlabs.net.BlockingTask task)
        {
            return ((com.oddlabs.net.Callable)id_to_callable.get(task.id));
        }
        // Java: 140
        public void close()
        {
            finished = true;
            if (!deterministic.isPlayback())
            {
                thread.interrupt();
            }
        }
        private void init_TaskThread()
        {
            this.id_to_callable = new java.util.HashMap();
            this.tasks = new java.util.ArrayList();
            this.finished_tasks = new java.util.ArrayList();
            this.@lock = new object();
            this.current_id = 0;
        }
        public TaskThread(com.oddlabs.@event.Deterministic deterministic, java.lang.Runnable notification_action) : base()
        {
            init_TaskThread();
            this.deterministic = deterministic;
            this.notification_action = notification_action;
        }
    }
}

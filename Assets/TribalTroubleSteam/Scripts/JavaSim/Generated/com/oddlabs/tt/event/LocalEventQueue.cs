// Translated from LocalEventQueue.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.@event
{
    // Java: 15
    public class LocalEventQueue : java.lang.Object, java.lang.AutoCloseable
    {
        public static java.util.logging.Logger logger;
        public static com.oddlabs.tt.@event.LocalEventQueue queue_instance;
        public com.oddlabs.tt.util.StateChecksum checksum;
        public com.oddlabs.tt.animation.AnimationManager manager;
        public com.oddlabs.tt.animation.AnimationManager high_precision_manager;
        public com.oddlabs.@event.Deterministic deterministic;
        public java.nio.file.Path event_log_file;
        public bool closed;
        public float time;
        // Java: 27
        public float getTime()
        {
            return time;
        }
        // Java: 31
        public long getMillis()
        {
            return high_precision_manager.getTick() * com.oddlabs.tt.animation.AnimationManager.ANIMATION_MILLISECONDS_PER_PRECISION_TICK;
        }
        // Java: 35
        public static com.oddlabs.tt.@event.LocalEventQueue getQueue()
        {
            return com.oddlabs.tt.@event.LocalEventQueue.queue_instance;
        }
        // Java: 39
        public void setEventsLogged(java.nio.file.Path log_file)
        {
            this.deterministic = new com.oddlabs.@event.SaveDeterministic(log_file);
            this.event_log_file = log_file;
        }
        // Java: 46
        public java.nio.file.Path getEventLogFile()
        {
            return event_log_file;
        }
        // Java: 51
        public void close()
        {
            if (closed)
            {
                return;
            }
            closed = true;
            com.oddlabs.tt.@event.LocalEventQueue.logger.info("LocalEventQueue disposing...");
            com.oddlabs.tt.@event.LocalEventQueue.logger.info("Ending deterministic log...");
            deterministic.endLog();
            com.oddlabs.tt.@event.LocalEventQueue.logger.info("LocalEventQueue disposed.");
        }
        // Java: 62
        public void loadEvents(java.nio.file.Path log_file, bool zipped)
        {
            this.deterministic = new com.oddlabs.@event.LoadDeterministic(log_file, zipped);
        }
        // Java: 72
        public com.oddlabs.tt.animation.AnimationManager getHighPrecisionManager()
        {
            return high_precision_manager;
        }
        // Java: 76
        public com.oddlabs.tt.animation.AnimationManager getManager()
        {
            return manager;
        }
        // Java: 80
        public int computeChecksum()
        {
            checksum.update(getManager().getTick());
            checksum.update(getHighPrecisionManager().getTick());
            manager.updateChecksum(checksum);
            high_precision_manager.updateChecksum(checksum);
            return checksum.getValue();
        }
        // Java: 88
        public com.oddlabs.@event.Deterministic getDeterministic()
        {
            return deterministic;
        }
        // Java: 92
        public void tickHighPrecision(float t)
        {
            time += t;
            getHighPrecisionManager().runAnimations(t);
        }
        // Java: 97
        public void tickLowPrecision(float t)
        {
            getManager().runAnimations(t);
        }
        // Java: 101
        public void debugPrintAnimations()
        {
            getManager().debugPrintAnimations();
        }
        static LocalEventQueue()
        {
            LocalEventQueue.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.@event.LocalEventQueue).getName());
            LocalEventQueue.queue_instance = new com.oddlabs.tt.@event.LocalEventQueue();
        }
        private void init_LocalEventQueue()
        {
            this.checksum = new com.oddlabs.tt.util.StateChecksum();
            this.manager = new com.oddlabs.tt.animation.AnimationManager();
            this.high_precision_manager = new com.oddlabs.tt.animation.AnimationManager();
            this.deterministic = new com.oddlabs.@event.NotDeterministic();
            this.time = 0;
        }
        public LocalEventQueue() : base()
        {
            init_LocalEventQueue();
        }
    }
}

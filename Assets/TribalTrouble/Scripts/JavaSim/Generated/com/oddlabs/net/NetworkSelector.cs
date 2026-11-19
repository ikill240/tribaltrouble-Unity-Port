// Translated from NetworkSelector.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 16
    public class NetworkSelector : java.lang.Object
    {
        public const long PING_TIMEOUT = 600000L;
        public const long PING_DELAY = 300000L;
        public com.oddlabs.net.MonotoneTimeManager time_manager;
        public int current_handler_id;
        public java.util.Map handler_map;
        public com.oddlabs.net.TaskThread task_thread;
        public java.nio.channels.Selector selector;
        public java.util.List ping_connections;
        public java.util.List ping_timeouts;
        public com.oddlabs.@event.Deterministic deterministic;
        // Java: 49
        public com.oddlabs.@event.Deterministic getDeterministic()
        {
            return deterministic;
        }
        // Java: 53
        public void asyncConnect(string dns_name, int port, com.oddlabs.net.Connection conn)
        {
            try
            {
                initSelector();
            }
            catch (System.IO.IOException e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
            com.oddlabs.net.DNSTask task = new com.oddlabs.net.DNSTask(dns_name, port, conn);
            getTaskThread().addTask(task);
        }
        // Java: 63
        public com.oddlabs.net.TaskThread getTaskThread()
        {
            if (task_thread == null)
            {
                task_thread = new com.oddlabs.net.TaskThread(deterministic, new java.lang.Runnable_L(() => { selector.wakeup(); }));
            }
            return task_thread;
        }
        // Java: 70
        public void initSelector()
        {
            if (selector == null)
            {
                selector = java.nio.channels.Selector.open();
            }
        }
        // Java: 75
        public java.nio.channels.Selector getSelector()
        {
            try
            {
                initSelector();
            }
            catch (System.IO.IOException e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
            return selector;
        }
        // Java: 84
        public void unregisterForPinging(com.oddlabs.net.Connection conn)
        {
            com.oddlabs.net.TimedConnection unregister_key = new com.oddlabs.net.TimedConnection((-1), conn);
            ping_timeouts.remove(unregister_key);
            ping_connections.remove(unregister_key);
        }
        // Java: 90
        public void registerForPingTimeout(com.oddlabs.net.Connection conn)
        {
            long ping_timeout = time_manager.getMillis() + com.oddlabs.net.NetworkSelector.PING_TIMEOUT;
            ping_timeouts.add(new com.oddlabs.net.TimedConnection(ping_timeout, conn));
        }
        // Java: 95
        public void registerForPing(com.oddlabs.net.Connection conn)
        {
            long ping_time = time_manager.getMillis() + com.oddlabs.net.NetworkSelector.PING_DELAY;
            ping_connections.add(new com.oddlabs.net.TimedConnection(ping_time, conn));
        }
        // Java: 100
        public void processTasks()
        {
            if (task_thread != null)
            {
                task_thread.poll();
            }
        }
        // Java: 105
        public long processPings(long millis)
        {
            long next_select_timeout = com.oddlabs.net.NetworkSelector.PING_DELAY;
            while (!ping_timeouts.isEmpty())
            {
                com.oddlabs.net.TimedConnection first_conn = ((com.oddlabs.net.TimedConnection)ping_timeouts.getFirst());
                long first = first_conn.getTimeout();
                if (first <= millis)
                {
                    ping_timeouts.removeFirst();
                    first_conn.getConnection().timeout();
                }
                else
                {
                    next_select_timeout = first - millis;
                    break;
                }
            }
            while (!ping_connections.isEmpty())
            {
                com.oddlabs.net.TimedConnection first_conn_2 = ((com.oddlabs.net.TimedConnection)ping_connections.getFirst());
                long first_2 = first_conn_2.getTimeout();
                if (first_2 <= millis)
                {
                    ping_connections.removeFirst();
                    com.oddlabs.net.Connection conn = first_conn_2.getConnection();
                    if (conn.isConnected())
                    {
                        conn.doPing();
                        registerForPing(conn);
                    }
                }
                else
                {
                    next_select_timeout = java.lang.Math.min(first_2 - millis, next_select_timeout);
                    break;
                }
            }
            return next_select_timeout;
        }
        // Java: 136
        public void tickBlocking(long timeout)
        {
            processTasks();
            long millis = time_manager.getMillis();
            long next_timeout = default;
            long ping_timeout = processPings(millis);
            if (ping_timeout == 0)
            {
                next_timeout = timeout;
            }
            else if (timeout == 0)
            {
                next_timeout = ping_timeout;
            }
            else
            {
                next_timeout = java.lang.Math.min(ping_timeout, timeout);
            }
            if (deterministic.log(selector != null && selector.select(next_timeout) > 0))
            {
                doTick();
            }
        }
        // Java: 151
        public void tickBlocking()
        {
            tickBlocking(0);
        }
        // Java: 155
        public void tick()
        {
            try
            {
                processTasks();
                processPings(time_manager.getMillis());
                if (deterministic.log(selector != null && selector.selectNow() > 0))
                {
                    doTick();
                }
            }
            catch (System.IO.IOException e)
            {
                throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
            }
        }
        // Java: 166
        public com.oddlabs.net.MonotoneTimeManager getTimeManager()
        {
            return time_manager;
        }
        // Java: 170
        public void cancelKey(java.nio.channels.SelectionKey key, com.oddlabs.net.Handler handler)
        {
            object handler_key = null;
            if (!deterministic.isPlayback())
            {
                handler_key = key.attachment();
                key.cancel();
            }
            handler_key = deterministic.log(handler_key);
            handler_map.remove(handler_key);
        }
        // Java: 180
        public void attachToKey(java.nio.channels.SelectionKey key, com.oddlabs.net.Handler handler)
        {
            object handler_key = null;
            if (!deterministic.isPlayback())
            {
                handler_key = current_handler_id++;
                key.attach(handler_key);
            }
            handler_key = deterministic.log(handler_key);
            handler_map.put(handler_key, handler);
        }
        // Java: 190
        public void doTick()
        {
            java.util.Iterator selected_keys = null;
            if (!deterministic.isPlayback())
            {
                selected_keys = selector.selectedKeys().iterator();
            }
            while (deterministic.log(deterministic.isPlayback() || selected_keys.hasNext()))
            {
                java.nio.channels.SelectionKey key = default;
                if (!deterministic.isPlayback())
                {
                    key = ((java.nio.channels.SelectionKey)selected_keys.next());
                    selected_keys.remove();
                }
                else
                {
                    key = null;
                }
                if (deterministic.log(deterministic.isPlayback() || !key.isValid()))
                {
                    continue;
                }
                object handler_key = null;
                if (!deterministic.isPlayback())
                {
                    handler_key = key.attachment();
                }
                handler_key = deterministic.log(handler_key);
                com.oddlabs.net.Handler handler = ((com.oddlabs.net.Handler)handler_map.get(handler_key));
                try
                {
                    handler.handle();
                }
                catch (System.Exception e)
                {
                    handler.handleError(((e is System.IO.IOException ioe) ? ioe : new System.IO.IOException("Unexpected error", (System.Exception)e)));
                    cancelKey(key, handler);
                }
            }
        }
        private void init_NetworkSelector()
        {
            this.handler_map = new java.util.HashMap();
            this.ping_connections = new java.util.LinkedList();
            this.ping_timeouts = new java.util.LinkedList();
        }
        public NetworkSelector(com.oddlabs.@event.Deterministic deterministic) : this(deterministic, new com.oddlabs.net.TimeManager_L(() =>
        {
            return deterministic.log(java.util.concurrent.TimeUnit.NANOSECONDS.toMillis(java.lang.JSystem.nanoTime()));
        }))
        {
        }
        public NetworkSelector(com.oddlabs.@event.Deterministic deterministic, com.oddlabs.net.TimeManager time_manager) : base()
        {
            init_NetworkSelector();
            this.deterministic = deterministic;
            this.time_manager = new com.oddlabs.net.MonotoneTimeManager(time_manager);
        }
    }
}

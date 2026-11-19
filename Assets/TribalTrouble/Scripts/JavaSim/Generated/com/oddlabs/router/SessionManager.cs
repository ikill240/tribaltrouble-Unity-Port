// Translated from SessionManager.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 14
    public class SessionManager : java.lang.Object
    {
        public java.util.Map id_to_session;
        public java.util.SortedMap timeouts;
        public com.oddlabs.net.MonotoneTimeManager time_manager;
        public java.util.logging.Logger logger;
        public int internal_session_id;
        // Java: 27
        public com.oddlabs.router.Session getExistingSession(com.oddlabs.router.SessionID session_id)
        {
            com.oddlabs.router.Session session = ((com.oddlabs.router.Session)id_to_session.get(session_id));
            if (session == null || !session.isComplete())
            {
                logger.warning("Session not found or not started for spectator: " + java.lang.JStr.valueOf(session_id));
                return null;
            }
            return session;
        }
        // Java: 37
        public com.oddlabs.router.Session get(com.oddlabs.router.SessionID session_id, com.oddlabs.router.SessionInfo session_info, int client_id)
        {
            com.oddlabs.router.Session session = ((com.oddlabs.router.Session)id_to_session.get(session_id));
            if (session == null)
            {
                session = new com.oddlabs.router.Session(logger, session_id, session_info, this);
                id_to_session.put(session_id, session);
                logger.log(java.util.logging.Level.INFO, "Creating session: {0}", session);
            }
            else
            {
                if (session.isComplete() || !java.lang.JO.equals(session.info, session_info) || session.hasClient(client_id) || client_id >= session_info.num_participants)
                {
                    throw new System.Exception("SessionInfo mismatch " + java.lang.JStr.valueOf(session.info) + " != " + java.lang.JStr.valueOf(session_info) + " client_id = " + (client_id));
                }
            }
            return session;
        }
        // Java: 53
        public long getNextTimeout()
        {
            if (timeouts.isEmpty())
            {
                return 0;
            }
            com.oddlabs.router.SessionManager.Timeout timeout = ((com.oddlabs.router.SessionManager.Timeout)timeouts.firstKey());
            long timeout_value = timeout.next_timeout - time_manager.getMillis();
            return java.lang.Math.max(1, timeout_value);
        }
        // Java: 61
        public void process()
        {
            long millis = time_manager.getMillis();
            while (!timeouts.isEmpty())
            {
                com.oddlabs.router.SessionManager.Timeout timeout = ((com.oddlabs.router.SessionManager.Timeout)timeouts.firstKey());
                com.oddlabs.router.RouterClient client = ((com.oddlabs.router.RouterClient)timeouts.get(timeout));
                if (client.getSession().getNumPlayers() > 0)
                {
                    if (!heartbeat(timeout.next_timeout, client, millis))
                    {
                        return;
                    }
                }
                bool removed = unregister(timeout);
            }
        }
        // Java: 76
        public bool heartbeat(long next_timeout, com.oddlabs.router.RouterClient client, long millis)
        {
            if (millis < next_timeout)
            {
                return false;
            }
            int next_tick = computeNextTick(client, millis);
            client.heartbeat(next_tick);
            return true;
        }
        // Java: 85
        public int computeNextTick(com.oddlabs.router.RouterClient client, long millis)
        {
            return com.oddlabs.router.SessionManager.doComputeNextTick(client.getSession(), millis);
        }
        // Java: 90
        public com.oddlabs.router.SessionManager.Timeout createTimeout(com.oddlabs.router.RouterClient client, long millis)
        {
            return new com.oddlabs.router.SessionManager.Timeout(internal_session_id++, client, millis);
        }
        // Java: 94
        public long start(com.oddlabs.router.Session session)
        {
            long initial_time = time_manager.getMillis();
            session.visit(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                com.oddlabs.router.SessionManager.Timeout timeout = createTimeout(client, initial_time);
                client.setTimeout(timeout);
                timeouts.put(timeout, client);
            }));
            logger.log(java.util.logging.Level.INFO, "Started session: {0}", session);
            return initial_time;
        }
        // Java: 106
        public bool unregister(com.oddlabs.router.SessionManager.Timeout timeout)
        {
            return ((com.oddlabs.router.RouterClient)timeouts.remove(timeout)) != null;
        }
        // Java: 110
        public void remove(com.oddlabs.router.Session session)
        {
            id_to_session.remove(session.session_id);
            logger.log(java.util.logging.Level.INFO, "Removing session: {0}", session);
        }
        // Java: 115
        public int getNextTick(com.oddlabs.router.Session session)
        {
            long millis = time_manager.getMillis();
            session.visit(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                unregister(client.getTimeout());
            }));
            return com.oddlabs.router.SessionManager.doComputeNextTick(session, millis);
        }
        // Java: 121
        public void startTimeout(com.oddlabs.router.RouterClient client)
        {
            if (client.getTimeout() != null)
            {
                unregister(client.getTimeout());
            }
            long millis = time_manager.getMillis();
            com.oddlabs.router.SessionManager.Timeout timeout = createTimeout(client, millis);
            client.setTimeout(timeout);
            timeouts.put(timeout, client);
        }
        // Java: 129
        public static int doComputeNextTick(com.oddlabs.router.Session session, long millis)
        {
            return unchecked((int)((millis - session.getInitialTime())));
        }
        // Java: 133
        public class Timeout : java.lang.Object, java.lang.Comparable
        {
            public int id;
            public long next_timeout;
            // Java: 160
            public int compareTo(object other_p)
            {
                com.oddlabs.router.SessionManager.Timeout other = (com.oddlabs.router.SessionManager.Timeout)other_p;
                int diff = unchecked((int)((next_timeout - other.next_timeout)));
                if (diff != 0)
                {
                    return diff;
                }
                else
                {
                    return id - other.id;
                }
            }
            // Java: 169
            public override bool equals(object other)
            {
                return (other is com.oddlabs.router.SessionManager.Timeout other_session) && compareTo(other_session) == 0;
            }
            // Java: 174
            public override int hashCode()
            {
                return unchecked((int)((id + next_timeout)));
            }
            // Java: 179
            public override string toString()
            {
                return "Timout: id = " + (id) + " timeout " + (next_timeout);
            }
            public Timeout(int id, com.oddlabs.router.RouterClient client, long millis) : base()
            {
                this.id = id;
                this.next_timeout = millis + client.getSession().info.milliseconds_per_heartbeat;
            }
        }
        private void init_SessionManager()
        {
            this.id_to_session = new java.util.HashMap();
            this.timeouts = new java.util.TreeMap();
        }
        public SessionManager(com.oddlabs.net.MonotoneTimeManager time_manager, java.util.logging.Logger logger) : base()
        {
            init_SessionManager();
            this.logger = logger;
            this.time_manager = time_manager;
        }
    }
}

// Translated from RouterClient.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 16
    public class RouterClient : java.lang.Object, com.oddlabs.net.ConnectionInterface
    {
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.router.RouterClientInterface client_interface;
        public java.util.logging.Logger logger;
        public com.oddlabs.net.AbstractConnection connection;
        public com.oddlabs.router.Router router;
        public java.util.List checksums;
        public int client_id;
        public bool is_spectator;
        public com.oddlabs.router.SessionManager.Timeout timeout;
        public com.oddlabs.router.Session session;
        public com.oddlabs.router.RouterClient.Interface current_interface;
        // Java: 49
        public java.util.List getChecksums()
        {
            return checksums;
        }
        // Java: 54
        public com.oddlabs.router.Session getSession()
        {
            return session;
        }
        // Java: 58
        public void setTimeout(com.oddlabs.router.SessionManager.Timeout timeout)
        {
            this.timeout = timeout;
        }
        // Java: 62
        public com.oddlabs.router.SessionManager.Timeout getTimeout()
        {
            return timeout;
        }
        // Java: 66
        public void heartbeat(int millis)
        {
            client_interface.heartbeat(millis);
        }
        // Java: 70
        public com.oddlabs.router.RouterClientInterface getInterface()
        {
            return client_interface;
        }
        // Java: 75
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
            if (session != null && session.isComplete())
            {
                session.startTimeout(this);
            }
        }
        // Java: 81
        public int getClientID()
        {
            return client_id;
        }
        // Java: 85
        public void doLogin(com.oddlabs.router.Session session, com.oddlabs.router.SessionInfo session_info, int client_id)
        {
            this.session = session;
            this.client_id = client_id;
            this.current_interface = new com.oddlabs.router.RouterClient.Interface(typeof(com.oddlabs.router.GameInterface), new RouterClient.Anon1_GameInterface(this));
            session.addPlayer(this);
            logger.log(java.util.logging.Level.INFO, "Player logged in: session = {0} client_id = {1}", new object[] { session, client_id });
        }
        // Java: 113
        public void doLoginSpectator(com.oddlabs.router.Session session)
        {
            if (session == null)
            {
                doError(false, new System.IO.IOException("Session not available for spectating"));
                return;
            }
            this.session = session;
            this.is_spectator = true;
            this.current_interface = new com.oddlabs.router.RouterClient.Interface(typeof(com.oddlabs.router.GameInterface), new RouterClient.Anon2_GameInterface(this, session));
            session.addSpectator(this);
            logger.info("Spectator logged in: session = " + java.lang.JStr.valueOf(session));
        }
        // Java: 146
        public void doChecksum(int checksum)
        {
            checksums.add(checksum);
            session.checksum();
        }
        // Java: 154
        public void doRelayGameStateEvent(com.oddlabs.net.ARMIEvent @event)
        {
            int next_tick = session.getNextTick();
            session.visit(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                client.client_interface.receiveGameStateEvent(client_id, next_tick, @event);
            }));
        }
        // Java: 160
        public void doRelayEvent(com.oddlabs.net.ARMIEvent @event)
        {
            session.visit(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                client.client_interface.receiveEvent(client_id, @event);
            }));
        }
        // Java: 165
        public void doRelayEventTo(int receiver_client_id, com.oddlabs.net.ARMIEvent @event)
        {
            session.visitPlayers(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
            {
                if (client.client_id == receiver_client_id)
                {
                    client.client_interface.receiveEvent(client_id, @event);
                }
            }));
        }
        // Java: 172
        public void handle(object sender, com.oddlabs.net.ARMIEvent @event)
        {
            try
            {
                @event.execute(current_interface.methods_f, current_interface.instance_f);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                doError(false, e);
            }
        }
        // Java: 181
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 185
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            doError(false, e);
        }
        // Java: 190
        public void close(bool checksum_error)
        {
            connection.close();
            if (session != null)
            {
                logger.log(java.util.logging.Level.INFO, "Removing client: {0}", this);
                if (is_spectator)
                {
                    session.removeSpectator(this);
                }
                else
                {
                    session.removePlayer(this);
                    session.visit(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
                    {
                        client.client_interface.playerDisconnected(client_id, checksum_error);
                    }));
                }
            }
        }
        // Java: 204
        public void doError(bool checksum_error, System.Exception e)
        {
            close(checksum_error);
            router.removeClient(this);
            logger.log(java.util.logging.Level.INFO, "Client disconnected, reason: {0}", e);
        }
        // Java: 210
        public class Interface : java.lang.Object
        {
            public com.oddlabs.net.ARMIInterfaceMethods methods() { return methods_f; }
            public object instance() { return instance_f; }
            public Interface(com.oddlabs.net.ARMIInterfaceMethods methods, object instance)
            {
                this.methods_f = methods;
                this.instance_f = instance;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is Interface r)) return false; return true && java.lang.JO.equals(methods_f, r.methods_f) && java.lang.JO.equals(instance_f, r.instance_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(methods_f); h = 31 * h + java.lang.JO.hashCode(instance_f); } return h; }
            public override string toString() { return "Interface[" + "methods=" + java.lang.JO.toString(methods_f) + ", instance=" + java.lang.JO.toString(instance_f) + "]"; }
            public com.oddlabs.net.ARMIInterfaceMethods methods_f;
            public object instance_f;
            public Interface(System.Type methods_v, object instance_v) : this(new com.oddlabs.net.ARMIInterfaceMethods(methods_v), instance_v)
            {
            }
        }
        private void init_RouterClient()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.router.RouterInterface));
            this.checksums = new java.util.LinkedList();
        }
        public RouterClient(com.oddlabs.router.SessionManager session_manager, com.oddlabs.net.AbstractConnection conn, java.util.logging.Logger logger, com.oddlabs.router.Router router) : base()
        {
            init_RouterClient();
            this.router = router;
            this.connection = conn;
            this.logger = logger;
            this.client_interface = ((com.oddlabs.router.RouterClientInterface)com.oddlabs.net.ARMIEvent.createProxy(conn, typeof(com.oddlabs.router.RouterClientInterface)));
            this.current_interface = new com.oddlabs.router.RouterClient.Interface(typeof(com.oddlabs.router.RouterInterface), new RouterClient.Anon3_RouterInterface(this, session_manager));
        }
        // Java: anonymous class at line 88
        public sealed class Anon1_GameInterface : java.lang.Object, com.oddlabs.router.GameInterface
        {
            public com.oddlabs.router.RouterClient this_0;
            public Anon1_GameInterface(com.oddlabs.router.RouterClient this_0_)
            {
                this.this_0 = this_0_;
            }
            // Java: 89
            public void checksum(int checksum_v)
            {
                this_0.doChecksum(checksum_v);
            }
            // Java: 94
            public void relayEventTo(int client_id_2, com.oddlabs.net.ARMIEvent @event)
            {
                this_0.doRelayEventTo(client_id_2, @event);
            }
            // Java: 99
            public void relayGameStateEvent(com.oddlabs.net.ARMIEvent @event)
            {
                this_0.doRelayGameStateEvent(@event);
            }
            // Java: 104
            public void relayEvent(com.oddlabs.net.ARMIEvent @event)
            {
                this_0.doRelayEvent(@event);
            }
        }
        // Java: anonymous class at line 120
        public sealed class Anon2_GameInterface : java.lang.Object, com.oddlabs.router.GameInterface
        {
            public com.oddlabs.router.RouterClient this_0;
            public com.oddlabs.router.Session session;
            public Anon2_GameInterface(com.oddlabs.router.RouterClient this_0_, com.oddlabs.router.Session session_c)
            {
                this.this_0 = this_0_;
                this.session = session_c;
            }
            // Java: 121
            public void checksum(int checksum_v)
            {
            }
            // Java: 125
            public void relayEventTo(int client_id, com.oddlabs.net.ARMIEvent @event)
            {
            }
            // Java: 129
            public void relayGameStateEvent(com.oddlabs.net.ARMIEvent @event)
            {
            }
            // Java: 134
            public void relayEvent(com.oddlabs.net.ARMIEvent @event)
            {
                session.visitSpectators(new com.oddlabs.router.SessionVisitor_L((com.oddlabs.router.RouterClient client) =>
                {
                    if (((object)client != (object)this.this_0))
                    {
                        client.client_interface.receiveEvent(com.oddlabs.router.GameInterface.SPECTATOR_CLIENT_ID, @event);
                    }
                }));
            }
        }
        // Java: anonymous class at line 34
        public sealed class Anon3_RouterInterface : java.lang.Object, com.oddlabs.router.RouterInterface
        {
            public com.oddlabs.router.RouterClient this_0;
            public com.oddlabs.router.SessionManager session_manager;
            public Anon3_RouterInterface(com.oddlabs.router.RouterClient this_0_, com.oddlabs.router.SessionManager session_manager_c)
            {
                this.this_0 = this_0_;
                this.session_manager = session_manager_c;
            }
            // Java: 35
            public void login(com.oddlabs.router.SessionID session_id, com.oddlabs.router.SessionInfo session_info, int client_id)
            {
                com.oddlabs.router.Session session = session_manager.get(session_id, session_info, client_id);
                this_0.doLogin(session, session_info, client_id);
            }
            // Java: 41
            public void loginSpectator(com.oddlabs.router.SessionID session_id)
            {
                com.oddlabs.router.Session session = session_manager.getExistingSession(session_id);
                this_0.doLoginSpectator(session);
            }
        }
    }
}

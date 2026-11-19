// Translated from MatchmakingServer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 21
    public class MatchmakingServer : java.lang.Object, com.oddlabs.net.ConnectionListenerInterface
    {
        public static java.util.Map online_users;
        public static int current_id;
        public static java.util.logging.Logger logger;
        public java.util.logging.Logger chat_logger;
        public com.oddlabs.net.AbstractConnectionListener connection_listener;
        public java.security.spec.AlgorithmParameterSpec param_spec;
        public com.oddlabs.net.NetworkSelector network;
        public java.util.Map client_map;
        public int server_tick_timeout;
        // Java: 55
        public int getTickTimeout()
        {
            return server_tick_timeout;
        }
        // Java: 59
        public void close()
        {
            connection_listener.close();
            foreach (com.oddlabs.matchserver.Client c in java.lang.JIter.of(new java.util.ArrayList(com.oddlabs.matchserver.MatchmakingServer.online_users.values())))
            {
                c.close();
            }
            com.oddlabs.matchserver.MatchmakingServer.online_users.clear();
            client_map.clear();
        }
        // Java: 67
        public int getOnlineCount()
        {
            return com.oddlabs.matchserver.MatchmakingServer.online_users.size();
        }
        // Java: 71
        public static java.util.logging.Logger getLogger()
        {
            return com.oddlabs.matchserver.MatchmakingServer.logger;
        }
        // Java: 75
        public java.util.logging.Logger getChatLogger()
        {
            return chat_logger;
        }
        // Java: 79
        public java.security.spec.AlgorithmParameterSpec getSpec()
        {
            return param_spec;
        }
        // Java: 83
        public void incomingConnection(com.oddlabs.net.AbstractConnectionListener connection_listener, object remote_address)
        {
            int id = com.oddlabs.matchserver.MatchmakingServer.current_id++;
            com.oddlabs.net.AbstractConnection conn = connection_listener.acceptConnection(null);
            com.oddlabs.net.SecureConnection secure_conn = new com.oddlabs.net.SecureConnection(network.getDeterministic(), conn, param_spec);
            com.oddlabs.matchserver.Authenticator client = new com.oddlabs.matchserver.Authenticator(this, secure_conn, ((java.net.InetAddress)remote_address), id);
        }
        // Java: 94
        public void loginClient(java.net.InetAddress remote_address, java.net.InetAddress local_remote_address, string username, com.oddlabs.net.AbstractConnection conn, int revision, int sim_version, int host_id)
        {
            com.oddlabs.matchserver.Client old_logged_in = ((com.oddlabs.matchserver.Client)com.oddlabs.matchserver.MatchmakingServer.online_users.remove(username.toLowerCase()));
            if (old_logged_in != null)
            {
                old_logged_in.close();
                com.oddlabs.matchserver.MatchmakingServer.logger.info(java.lang.JStr.valueOf(username) + " overtaked old login");
            }
            com.oddlabs.matchserver.Client client = new com.oddlabs.matchserver.Client(this, conn, remote_address, local_remote_address, username, false, revision, sim_version, host_id);
            com.oddlabs.matchserver.MatchmakingServer.online_users.put(username.toLowerCase(), client);
            client_map.put(client.getHostID(), client);
            com.oddlabs.matchserver.MatchmakingServer.logger.info(java.lang.JStr.valueOf(username) + " logged in");
            if (com.oddlabs.matchserver.MatchmakingServer.online_users.size() == 1)
            {
                com.oddlabs.matchserver.MatchmakingServer.logger.info("A user is online, starting automatic server ticking.");
                server_tick_timeout = 100;
            }
        }
        // Java: 112
        public com.oddlabs.matchserver.Client getClientFromID(int host_id)
        {
            return ((com.oddlabs.matchserver.Client)client_map.get(host_id));
        }
        // Java: 116
        public void error(com.oddlabs.net.AbstractConnectionListener conn_id, System.IO.IOException e)
        {
            com.oddlabs.matchserver.MatchmakingServer.logger.severe("Server socket failed!");
            throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
        }
        // Java: 121
        public void logoutClient(com.oddlabs.matchserver.Client client)
        {
            com.oddlabs.matchserver.MatchmakingServer.online_users.remove(client.getUsername().toLowerCase());
            removeInstance(client.getHostID());
            if (com.oddlabs.matchserver.MatchmakingServer.online_users.isEmpty())
            {
                com.oddlabs.matchserver.MatchmakingServer.logger.info("No users online, pausing automatic server ticking.");
                server_tick_timeout = 0;
            }
        }
        // Java: 130
        public void removeInstance(int instance_id)
        {
            client_map.remove(instance_id);
        }
        static MatchmakingServer()
        {
            MatchmakingServer.online_users = new java.util.HashMap();
            MatchmakingServer.current_id = 1;
            MatchmakingServer.logger = java.util.logging.Logger.getLogger("com.oddlabs.matchserver");
        }
        private void init_MatchmakingServer()
        {
            this.chat_logger = java.util.logging.Logger.getLogger("chatlog");
            this.client_map = new java.util.HashMap();
            this.server_tick_timeout = 0;
        }
        public MatchmakingServer(com.oddlabs.net.NetworkSelector network) : base()
        {
            init_MatchmakingServer();
            this.network = network;
            com.oddlabs.matchserver.MatchmakingServer.online_users.clear();
            com.oddlabs.matchserver.MatchmakingServer.current_id = 1;
            com.oddlabs.matchserver.MatchmakingServer.logger.info("Generating encryption keys.");
            this.param_spec = com.oddlabs.util.KeyManager.generateParameterSpec();
            connection_listener = new com.oddlabs.net.ConnectionListener(network, null, com.oddlabs.matchmaking.MatchmakingServerInterface.MATCHMAKING_SERVER_PORT, this);
            com.oddlabs.matchserver.DBInterface.initDropGames();
            com.oddlabs.matchserver.DBInterface.clearOnlineProfiles();
            com.oddlabs.matchserver.MatchmakingServer.logger.log(java.util.logging.Level.INFO, "Matchmaking server started: {0}", com.oddlabs.util.BuildInfo.FULL_VERSION);
        }
    }
}

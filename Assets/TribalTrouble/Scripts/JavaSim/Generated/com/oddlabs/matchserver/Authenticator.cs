// Translated from Authenticator.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 19
    public class Authenticator : java.lang.Object, com.oddlabs.matchmaking.MatchmakingServerLoginInterface, com.oddlabs.net.ConnectionInterface
    {
        public static int guest_postfix;
        public com.oddlabs.net.SecureConnection conn;
        public com.oddlabs.matchmaking.MatchmakingClientInterface client_interface;
        public com.oddlabs.matchserver.MatchmakingServer server;
        public java.net.InetAddress remote_address;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public int host_id;
        public java.net.InetAddress local_remote_address;
        public int sim_version;
        // Java: 42
        public void handle(object sender, com.oddlabs.net.ARMIEvent @event)
        {
            try
            {
                @event.execute(interface_methods, this);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                error(e);
            }
        }
        // Java: 50
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 53
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            error(e);
        }
        // Java: 57
        public void error(System.Exception e)
        {
            close();
            com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Exception e = " + java.lang.JStr.valueOf(e));
        }
        // Java: 62
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 65
        public void setLocalRemoteAddress(java.net.InetAddress local_remote_address)
        {
            this.local_remote_address = local_remote_address;
        }
        // Java: 69
        public void setSimVersion(int sim_version)
        {
            this.sim_version = sim_version;
        }
        // Java: 73
        public static void checkUsername(string name)
        {
            int min_username_length = com.oddlabs.matchserver.DBInterface.getSettingsInt("min_username_length");
            if (name.length() < min_username_length)
            {
                throw new com.oddlabs.matchserver.InvalidUsernameException(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_TOO_SHORT);
            }
            int max_username_length = com.oddlabs.matchserver.DBInterface.getSettingsInt("max_username_length");
            if (name.length() > max_username_length)
            {
                throw new com.oddlabs.matchserver.InvalidUsernameException(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_TOO_LONG);
            }
            string allowed_chars = com.oddlabs.matchserver.DBInterface.getSetting("allowed_chars");
            for (int i = 0; i < name.length(); i++)
            {
                char c = name.charAt(i);
                if (allowed_chars.indexOf(c) == (-1))
                {
                    throw new com.oddlabs.matchserver.InvalidUsernameException(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_INVALID_CHARACTERS);
                }
            }
            if (!com.oddlabs.matchserver.BannedWordFilter.isAllowed(name))
            {
                throw new com.oddlabs.matchserver.InvalidUsernameException(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_INVALID_CHARACTERS);
            }
        }
        // Java: 95
        public void createUser(com.oddlabs.matchmaking.Login login_v, com.oddlabs.matchmaking.LoginDetails login_details, java.security.SignedObject reg_key, int revision)
        {
            if (login_v == null || !login_v.isValid())
            {
                close();
                return;
            }
            if (!revisionOK(revision))
            {
                return;
            }
            if (com.oddlabs.matchserver.ServerConfiguration.getInstance().isSteamOnlyAuth())
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_STEAM_REQUIRED);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Username/password authentication disabled - Steam-only mode enabled");
                return;
            }
            try
            {
                com.oddlabs.matchserver.Authenticator.checkUsername(login_v.getUsername());
            }
            catch (com.oddlabs.matchserver.InvalidUsernameException e)
            {
                client_interface.loginError(e.getErrorCode());
                return;
            }
            if (login_details == null || !login_details.isValid())
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_INVALID_EMAIL);
                return;
            }
            if (com.oddlabs.matchserver.DBInterface.usernameExists(login_v.getUsername()))
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USERNAME_ERROR_ALREADY_EXISTS);
                return;
            }
            com.oddlabs.matchserver.DBInterface.createUser(login_v, login_details, null);
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Created user " + java.lang.JStr.valueOf(login_v.getUsername()) + " with email address " + java.lang.JStr.valueOf(login_details.getEmail()));
            doLogin(login_v.getUsername(), revision);
        }
        // Java: 135
        public void login(com.oddlabs.matchmaking.Login login_v, java.security.SignedObject reg_key, int revision)
        {
            if (login_v == null || !login_v.isValid())
            {
                close();
                return;
            }
            if (!revisionOK(revision))
            {
                return;
            }
            if (com.oddlabs.matchserver.ServerConfiguration.getInstance().isSteamOnlyAuth())
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_STEAM_REQUIRED);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Username/password authentication disabled - Steam-only mode enabled");
                return;
            }
            string username = login_v.getUsername().trim();
            if (!com.oddlabs.matchserver.DBInterface.queryUser(username, login_v.getPasswordDigest()))
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_NO_SUCH_USER);
                return;
            }
            doLogin(username, revision);
        }
        // Java: 161
        public void loginAsGuest(int revision)
        {
            if (!revisionOK(revision))
            {
                return;
            }
            if (com.oddlabs.matchserver.ServerConfiguration.getInstance().isSteamOnlyAuth())
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_STEAM_REQUIRED);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Guest authentication disabled - Steam-only mode enabled");
                return;
            }
            string username = "Guest" + (com.oddlabs.matchserver.Authenticator.guest_postfix++);
            doLogin(username, revision);
        }
        // Java: 176
        public void loginWithSteam(long steamAccountId, string personaName, sbyte[] authTicket, int appId, int revision)
        {
            if (!revisionOK(revision))
            {
                return;
            }
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Steam login attempt: accountId=" + (steamAccountId) + " persona=" + java.lang.JStr.valueOf(personaName) + " appId=" + (appId) + " ticketLen=" + (((authTicket != null ? authTicket.Length : 0))));
            if (!com.oddlabs.matchserver.SteamAuthValidator.validateTicket(steamAccountId, authTicket, appId))
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_NO_SUCH_USER);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("Steam auth ticket validation failed for account ID: " + (steamAccountId));
                return;
            }
            string username = com.oddlabs.matchserver.DBInterface.getOrCreateSteamRegistration(steamAccountId);
            if (username == null)
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_NO_SUCH_USER);
                com.oddlabs.matchserver.MatchmakingServer.getLogger().warning("getOrCreateSteamRegistration returned null for account ID: " + (steamAccountId));
                return;
            }
            com.oddlabs.matchserver.MatchmakingServer.getLogger().info("Steam login success: " + java.lang.JStr.valueOf(username));
            doLogin(username, revision);
        }
        // Java: 201
        public bool revisionOK(int revision)
        {
            if (revision != com.oddlabs.util.Compatibility.API_VERSION)
            {
                client_interface.loginError(com.oddlabs.matchmaking.MatchmakingClientInterface.USER_ERROR_VERSION_TOO_OLD);
                return false;
            }
            else
            {
                return true;
            }
        }
        // Java: 209
        public void doLogin(string username, int revision)
        {
            if (local_remote_address != null)
            {
                client_interface.loginOK(username, new com.oddlabs.matchmaking.TunnelAddress(getHostID(), remote_address, local_remote_address));
                server.loginClient(remote_address, local_remote_address, username, conn.getWrappedConnectionAndShutdown(), revision, sim_version, host_id);
            }
            else
            {
                error(new System.InvalidOperationException("Client didnt set local_remote_address"));
            }
        }
        // Java: 219
        public int getHostID()
        {
            return host_id;
        }
        // Java: 223
        public void close()
        {
            conn.close();
        }
        static Authenticator()
        {
            Authenticator.guest_postfix = 1;
        }
        private void init_Authenticator()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.matchmaking.MatchmakingServerLoginInterface));
            this.sim_version = com.oddlabs.util.Compatibility.SIM_LEGACY;
        }
        public Authenticator(com.oddlabs.matchserver.MatchmakingServer server, com.oddlabs.net.SecureConnection conn, java.net.InetAddress remote_address, int host_id) : base()
        {
            init_Authenticator();
            this.conn = conn;
            this.server = server;
            this.remote_address = remote_address;
            this.client_interface = ((com.oddlabs.matchmaking.MatchmakingClientInterface)com.oddlabs.net.ARMIEvent.createProxy(conn, typeof(com.oddlabs.matchmaking.MatchmakingClientInterface)));
            this.host_id = host_id;
            conn.setConnectionInterface(this);
        }
    }
}

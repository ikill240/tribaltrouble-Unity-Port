// Translated from GameNetwork.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 8
    public class GameNetwork : java.lang.Object
    {
        public com.oddlabs.tt.net.Server server;
        public com.oddlabs.tt.net.Client client;
        public com.oddlabs.matchmaking.RosterTemplate initial_roster;
        // Java: 24
        public void setInitialRoster(com.oddlabs.matchmaking.RosterTemplate initial_roster)
        {
            this.initial_roster = initial_roster;
            if (server != null && initial_roster != null)
            {
                server.applyRosterJoinDefaults(initial_roster);
            }
        }
        // Java: 30
        public com.oddlabs.matchmaking.RosterTemplate getInitialRoster()
        {
            return initial_roster;
        }
        // Java: 34
        public void closeServer()
        {
            if (server != null)
            {
                server.close();
            }
        }
        // Java: 39
        public com.oddlabs.tt.net.Client getClient()
        {
            return client;
        }
        // Java: 44
        public com.oddlabs.tt.net.Server getServer()
        {
            return server;
        }
        // Java: 48
        public void close()
        {
            client.close();
            closeServer();
        }
        public GameNetwork(com.oddlabs.tt.net.Server server, com.oddlabs.tt.net.Client client) : base()
        {
            this.server = server;
            this.client = client;
        }
    }
}

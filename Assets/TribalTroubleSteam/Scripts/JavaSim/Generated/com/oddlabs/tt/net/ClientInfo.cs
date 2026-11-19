// Translated from ClientInfo.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 12
    public class ClientInfo : java.lang.Object, com.oddlabs.tt.net.GameServerInterface, com.oddlabs.net.ConnectionInterface
    {
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.tt.net.Server server;
        public com.oddlabs.tt.net.PlayerSlot player_slot;
        // Java: 22
        public void handle(object sender, com.oddlabs.net.ARMIEvent armi_event)
        {
            try
            {
                armi_event.execute(interface_methods, this);
            }
            catch (com.oddlabs.net.IllegalARMIEventException e)
            {
                server.handleError(((com.oddlabs.net.AbstractConnection)sender), e);
            }
        }
        // Java: 31
        public void writeBufferDrained(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 35
        public void error(com.oddlabs.net.AbstractConnection conn, System.IO.IOException e)
        {
            server.handleError(conn, e);
        }
        // Java: 40
        public void connected(com.oddlabs.net.AbstractConnection conn)
        {
        }
        // Java: 44
        public com.oddlabs.tt.net.PlayerSlot getPlayerSlot()
        {
            return player_slot;
        }
        // Java: 48
        public void resetSlotState(int slot, bool open)
        {
            server.resetSlotState(player_slot, slot, open);
        }
        // Java: 53
        public void setPlayerSlot(int slot, int type, int race, int team, bool ready, int ai_difficulty)
        {
            server.setPlayerSlot(player_slot, slot, type, race, team, ready, ai_difficulty);
        }
        // Java: 58
        public void startServer()
        {
            server.startServer(player_slot);
        }
        // Java: 63
        public void chat(string chat_v)
        {
            server.chat(player_slot, chat_v);
        }
        private void init_ClientInfo()
        {
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.tt.net.GameServerInterface));
        }
        public ClientInfo(com.oddlabs.tt.net.Server server, com.oddlabs.tt.net.PlayerSlot player_slot) : base()
        {
            init_ClientInfo();
            this.player_slot = player_slot;
            this.server = server;
        }
    }
}

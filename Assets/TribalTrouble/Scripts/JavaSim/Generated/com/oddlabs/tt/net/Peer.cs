// Translated from Peer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 17
    public class Peer : java.lang.Object, com.oddlabs.tt.net.PeerHubInterface
    {
        public static java.util.logging.Logger logger;
        public com.oddlabs.tt.net.GameArgumentReader argument_reader;
        public int peer_index;
        public com.oddlabs.tt.player.Player player;
        public com.oddlabs.tt.net.PeerHub peer_hub;
        public java.util.List event_queue;
        public com.oddlabs.net.ARMIInterfaceMethods interface_methods;
        public com.oddlabs.tt.net.PeerHubInterface peerhub_interface;
        // Java: 37
        public int getPeerIndex()
        {
            return peer_index;
        }
        // Java: 41
        public override string toString()
        {
            return "player: " + java.lang.JStr.valueOf(java.lang.JO.toString(player));
        }
        // Java: 46
        public void addEvent(int tick, com.oddlabs.net.ARMIEvent @event)
        {
            event_queue.add(new com.oddlabs.tt.net.Peer.GameEvent(tick, @event));
        }
        // Java: 50
        public void executeEvents(int tick)
        {
            while (!event_queue.isEmpty())
            {
                com.oddlabs.tt.net.Peer.GameEvent game_event = ((com.oddlabs.tt.net.Peer.GameEvent)event_queue.getFirst());
                if (game_event.tick_f != tick)
                {
                    return;
                }
                event_queue.removeFirst();
                try
                {
                    game_event.event_f.execute(interface_methods, argument_reader, player);
                }
                catch (com.oddlabs.net.ARMIHandlerException e)
                {
                    com.oddlabs.tt.net.Peer.logger.log(java.util.logging.Level.SEVERE, "Command from " + java.lang.JStr.valueOf(player.getPlayerInfo().getName()) + " failed at tick " + (tick) + "; ignored, player stays connected", e.getCause());
                }
            }
        }
        // Java: 77
        public void chat(string text, bool team)
        {
            peer_hub.receiveChat(player.getPlayerInfo().getName(), text, team);
        }
        // Java: 82
        public void beacon(float x, float y)
        {
            peer_hub.receiveBeacon(x, y, player.getPlayerInfo().getName());
        }
        // Java: 87
        public com.oddlabs.tt.net.PeerHubInterface getPeerHubInterface()
        {
            return peerhub_interface;
        }
        // Java: 91
        public com.oddlabs.tt.player.PlayerInfo getPlayerInfo()
        {
            return player.getPlayerInfo();
        }
        // Java: 95
        public com.oddlabs.tt.player.Player getPlayer()
        {
            return player;
        }
        // Java: 99
        public class GameEvent : java.lang.Object
        {
            public int tick() { return tick_f; }
            public com.oddlabs.net.ARMIEvent @event() { return event_f; }
            public GameEvent(int tick, com.oddlabs.net.ARMIEvent @event)
            {
                this.tick_f = tick;
                this.event_f = @event;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is GameEvent r)) return false; return true && java.lang.JO.equals(tick_f, r.tick_f) && java.lang.JO.equals(event_f, r.event_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(tick_f); h = 31 * h + java.lang.JO.hashCode(event_f); } return h; }
            public override string toString() { return "GameEvent[" + "tick=" + java.lang.JO.toString(tick_f) + ", event=" + java.lang.JO.toString(event_f) + "]"; }
            public int tick_f;
            public com.oddlabs.net.ARMIEvent event_f;
        }
        static Peer()
        {
            Peer.logger = java.util.logging.Logger.getLogger(typeof(com.oddlabs.tt.net.Peer).getName());
        }
        private void init_Peer()
        {
            this.event_queue = new java.util.LinkedList();
            this.interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(typeof(com.oddlabs.tt.player.PlayerInterface));
        }
        public Peer(com.oddlabs.tt.net.PeerHub peer_hub, int peer_index, com.oddlabs.tt.player.Player player, com.oddlabs.tt.net.GameArgumentReader argument_reader, com.oddlabs.tt.net.PeerHubInterface peerhub_interface) : base()
        {
            init_Peer();
            this.peerhub_interface = peerhub_interface;
            this.peer_index = peer_index;
            this.peer_hub = peer_hub;
            this.player = player;
            this.argument_reader = argument_reader;
        }
    }
}

// Translated from PeerHubSpectatorController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 17
    public class PeerHubSpectatorController : java.lang.Object
    {
        public const int CATCH_UP_TICKS_PER_FRAME = 15;
        public static com.oddlabs.tt.net.PeerHubSpectatorController instance;
        public com.oddlabs.tt.net.PeerHub hub;
        public bool catchingUp;
        public int catchUpTargetTick;
        public java.util.List heldEvents;
        public bool logApplied;
        // Java: 28
        public static com.oddlabs.tt.net.PeerHubSpectatorController getInstance()
        {
            return com.oddlabs.tt.net.PeerHubSpectatorController.instance;
        }
        // Java: 32
        public static void clearInstance(com.oddlabs.tt.net.PeerHubSpectatorController controller)
        {
            if (((object)com.oddlabs.tt.net.PeerHubSpectatorController.instance == (object)controller))
            {
                com.oddlabs.tt.net.PeerHubSpectatorController.instance = null;
            }
        }
        // Java: 41
        public void onStart()
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().requestSpectatorEventLog();
            }
        }
        // Java: 51
        public bool animateCatchUp(float t)
        {
            if (!catchingUp)
            {
                return false;
            }
            int ticks = 0;
            while (hub.getTick() < catchUpTargetTick && ticks < com.oddlabs.tt.net.PeerHubSpectatorController.CATCH_UP_TICKS_PER_FRAME)
            {
                hub.doTickInternal(t);
                ticks++;
            }
            if (hub.getTick() >= catchUpTargetTick)
            {
                catchingUp = false;
                hub.setSynchronized(true);
                java.lang.IO.println("Spectator catch-up complete at tick " + (hub.getTick()));
            }
            return true;
        }
        // Java: 67
        public bool holdLiveEvent(com.oddlabs.tt.net.Peer peer, int tick, com.oddlabs.net.ARMIEvent @event)
        {
            if (logApplied)
            {
                return false;
            }
            heldEvents.add(new com.oddlabs.tt.net.PeerHubSpectatorController.HeldEvent(peer, tick, @event));
            return true;
        }
        // Java: 73
        public void fastForward(sbyte[] eventLogData, int targetTick)
        {
            int eventCount = 0;
            int firstHeldTick = (heldEvents.isEmpty() ? java.lang.Integer.MAX_VALUE : ((com.oddlabs.tt.net.PeerHubSpectatorController.HeldEvent)heldEvents.getFirst()).tick());
            java.util.List loggedSinceHeld = new java.util.ArrayList();
            if (eventLogData != null && eventLogData.Length > 0)
            {
                java.io.DataInputStream dis = new java.io.DataInputStream(new java.io.ByteArrayInputStream(eventLogData));
                try
                {
                    while (dis.available() > 0)
                    {
                        int tick = dis.readInt();
                        int clientId = dis.readInt();
                        short eventSize = dis.readShort();
                        if (eventSize <= 0 || eventSize > dis.available())
                        {
                            break;
                        }
                        sbyte[] data = new sbyte[eventSize];
                        dis.readFully(data);
                        java.nio.ByteBuffer buf = java.nio.ByteBuffer.wrap(data);
                        com.oddlabs.net.ARMIEvent @event = com.oddlabs.net.ARMIEvent.read(buf, eventSize);
                        com.oddlabs.tt.net.Peer peer = hub.getPeerFromClientID(clientId);
                        if (peer != null)
                        {
                            peer.addEvent(tick, @event);
                            eventCount++;
                            if (tick >= firstHeldTick)
                            {
                                loggedSinceHeld.add(com.oddlabs.tt.net.PeerHubSpectatorController.key(tick, clientId, data));
                            }
                        }
                    }
                }
                catch (System.IO.IOException e)
                {
                    java.lang.IO.println("Error reading event log: " + java.lang.JStr.valueOf(e));
                }
            }
            foreach (com.oddlabs.tt.net.PeerHubSpectatorController.HeldEvent held in java.lang.JIter.of(heldEvents))
            {
                if (!loggedSinceHeld.remove(com.oddlabs.tt.net.PeerHubSpectatorController.key(held.tick(), held.peer().getPeerIndex(), com.oddlabs.tt.net.PeerHubSpectatorController.bytes(held.@event()))))
                {
                    held.peer().addEvent(held.tick(), held.@event());
                }
            }
            heldEvents.clear();
            logApplied = true;
            this.catchUpTargetTick = targetTick;
            this.catchingUp = true;
            java.lang.IO.println("Spectator catching up to tick " + (targetTick) + " (" + (eventCount) + " events queued)");
        }
        // Java: 116
        public static void sendCommandEvent(int tick, int clientId, com.oddlabs.net.ARMIEvent @event)
        {
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateCommandEvent(tick, clientId, @event.getEventSize(), com.oddlabs.tt.net.PeerHubSpectatorController.bytes(@event));
        }
        // Java: 121
        public static sbyte[] bytes(com.oddlabs.net.ARMIEvent @event)
        {
            java.nio.ByteBuffer buf = java.nio.ByteBuffer.allocate(@event.getEventSize());
            @event.write(buf);
            return buf.array();
        }
        // Java: 127
        public static string key(int tick, int clientId, sbyte[] data)
        {
            return (tick) + ":" + (clientId) + ":" + java.lang.JStr.valueOf(java.util.HexFormat.of().formatHex(data));
        }
        // Java: 131
        public class HeldEvent : java.lang.Object
        {
            public com.oddlabs.tt.net.Peer peer() { return peer_f; }
            public int tick() { return tick_f; }
            public com.oddlabs.net.ARMIEvent @event() { return event_f; }
            public HeldEvent(com.oddlabs.tt.net.Peer peer, int tick, com.oddlabs.net.ARMIEvent @event)
            {
                this.peer_f = peer;
                this.tick_f = tick;
                this.event_f = @event;
            }
            public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is HeldEvent r)) return false; return true && java.lang.JO.equals(peer_f, r.peer_f) && java.lang.JO.equals(tick_f, r.tick_f) && java.lang.JO.equals(event_f, r.event_f); }
            public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(peer_f); h = 31 * h + java.lang.JO.hashCode(tick_f); h = 31 * h + java.lang.JO.hashCode(event_f); } return h; }
            public override string toString() { return "HeldEvent[" + "peer=" + java.lang.JO.toString(peer_f) + ", tick=" + java.lang.JO.toString(tick_f) + ", event=" + java.lang.JO.toString(event_f) + "]"; }
            public com.oddlabs.tt.net.Peer peer_f;
            public int tick_f;
            public com.oddlabs.net.ARMIEvent event_f;
        }
        private void init_PeerHubSpectatorController()
        {
            this.heldEvents = new java.util.ArrayList();
        }
        public PeerHubSpectatorController(com.oddlabs.tt.net.PeerHub hub) : base()
        {
            init_PeerHubSpectatorController();
            this.hub = hub;
            com.oddlabs.tt.net.PeerHubSpectatorController.instance = this;
        }
    }
}

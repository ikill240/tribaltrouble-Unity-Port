// Translated from PeerHubSpectatorController.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 14
    public class PeerHubSpectatorController : java.lang.Object
    {
        public const int CATCH_UP_TICKS_PER_FRAME = 15;
        public static com.oddlabs.tt.net.PeerHubSpectatorController instance;
        public com.oddlabs.tt.net.PeerHub hub;
        public bool catchingUp;
        public int catchUpTargetTick;
        // Java: 23
        public static com.oddlabs.tt.net.PeerHubSpectatorController getInstance()
        {
            return com.oddlabs.tt.net.PeerHubSpectatorController.instance;
        }
        // Java: 27
        public static void clearInstance(com.oddlabs.tt.net.PeerHubSpectatorController controller)
        {
            if (((object)com.oddlabs.tt.net.PeerHubSpectatorController.instance == (object)controller))
            {
                com.oddlabs.tt.net.PeerHubSpectatorController.instance = null;
            }
        }
        // Java: 36
        public void onStart()
        {
            if (com.oddlabs.tt.net.Network.getMatchmakingClient().isConnected())
            {
                com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().requestSpectatorEventLog();
            }
        }
        // Java: 46
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
        // Java: 61
        public void fastForward(sbyte[] eventLogData, int targetTick)
        {
            int eventCount = 0;
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
                        }
                    }
                }
                catch (System.IO.IOException e)
                {
                    java.lang.IO.println("Error reading event log: " + java.lang.JStr.valueOf(e));
                }
            }
            this.catchUpTargetTick = targetTick;
            this.catchingUp = true;
            java.lang.IO.println("Spectator catching up to tick " + (targetTick) + " (" + (eventCount) + " events queued)");
        }
        // Java: 94
        public static void sendCommandEvent(int tick, int clientId, com.oddlabs.net.ARMIEvent @event)
        {
            short eventSize = @event.getEventSize();
            java.nio.ByteBuffer buf = java.nio.ByteBuffer.allocate(eventSize);
            @event.write(buf);
            com.oddlabs.tt.net.Network.getMatchmakingClient().getInterface().updateCommandEvent(tick, clientId, eventSize, buf.array());
        }
        public PeerHubSpectatorController(com.oddlabs.tt.net.PeerHub hub) : base()
        {
            this.hub = hub;
            com.oddlabs.tt.net.PeerHubSpectatorController.instance = this;
        }
    }
}

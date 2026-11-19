// Translated from EmbeddedServer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver
{
    // Java: 15
    public class EmbeddedServer : java.lang.Object
    {
        public com.oddlabs.net.NetworkSelector network;
        public com.oddlabs.router.Router router;
        public com.oddlabs.matchserver.MatchmakingServer matchmaking;
        // Java: 34
        public void tick(long maxWaitMillis)
        {
            long timeout = maxWaitMillis;
            long router_timeout = router.getNextTimeout();
            if (router_timeout > 0 && router_timeout < timeout)
            {
                timeout = router_timeout;
            }
            int mm_timeout = matchmaking.getTickTimeout();
            if (mm_timeout > 0 && mm_timeout < timeout)
            {
                timeout = mm_timeout;
            }
            network.tickBlocking(java.lang.Math.max(1, timeout));
            router.process();
        }
        // Java: 46
        public int getOnlineCount()
        {
            return matchmaking.getOnlineCount();
        }
        // Java: 50
        public void close()
        {
            try
            {
                matchmaking.close();
            }
            catch (System.Exception e)
            {
            }
            router.close();
        }
        public EmbeddedServer() : base()
        {
            this.network = new com.oddlabs.net.NetworkSelector(new com.oddlabs.@event.NotDeterministic());
            this.router = new com.oddlabs.router.Router(network, java.util.logging.Logger.getLogger("com.oddlabs.router.Router"));
            com.oddlabs.matchserver.MatchmakingServer mm = default;
            try
            {
                mm = new com.oddlabs.matchserver.MatchmakingServer(network);
            }
            catch (System.Exception e)
            {
                router.close();
                throw e;
            }
            this.matchmaking = mm;
        }
    }
}

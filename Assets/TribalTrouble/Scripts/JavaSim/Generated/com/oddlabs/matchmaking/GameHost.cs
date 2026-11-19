// Translated from GameHost.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 8
    public class GameHost : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public com.oddlabs.matchmaking.Game game;
        public int host_id;
        public int revision;
        // Java: 25
        public com.oddlabs.matchmaking.Game getGame()
        {
            return game;
        }
        // Java: 29
        public int getHostID()
        {
            return host_id;
        }
        // Java: 33
        public int getSimVersion()
        {
            return revision;
        }
        public GameHost(com.oddlabs.matchmaking.Game game, int host_id, int revision) : base()
        {
            this.host_id = host_id;
            this.game = game;
            this.revision = revision;
        }
    }
}

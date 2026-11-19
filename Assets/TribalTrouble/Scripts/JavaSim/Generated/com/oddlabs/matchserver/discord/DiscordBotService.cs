// Translated from DiscordBotService.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchserver.discord
{
    // Java: 6
    public class DiscordBotService : java.lang.Object
    {
        public static com.oddlabs.matchserver.discord.DiscordBotService instance;
        // Java: 9
        public static com.oddlabs.matchserver.discord.DiscordBotService getInstance()
        {
            return com.oddlabs.matchserver.discord.DiscordBotService.instance;
        }
        // Java: 11
        public virtual void initialize(string token, long serverId)
        {
            java.lang.JSystem.@out.println("[DiscordBotService stub] initialize ignored");
        }
        // Java: 15
        public virtual bool isInitialized()
        {
            return false;
        }
        // Java: 17
        public virtual void dispose()
        {
        }
        // Java: 19
        public virtual java.util.Optional getChatroomCoordinator()
        {
            return java.util.Optional.empty();
        }
        static DiscordBotService()
        {
            DiscordBotService.instance = new com.oddlabs.matchserver.discord.DiscordBotService();
        }
        public DiscordBotService() : base()
        {
        }
    }
}

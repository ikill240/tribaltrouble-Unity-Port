// Translated from MatchmakingServerLoginInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 6
    public interface MatchmakingServerLoginInterface
    {
        // Java: 7
        void setLocalRemoteAddress(java.net.InetAddress local_remote_address);
        // Java: 9
        void login(com.oddlabs.matchmaking.Login login_v, java.security.SignedObject reg_key, int revision);
        // Java: 11
        void loginAsGuest(int revision);
        // Java: 13
        void loginWithSteam(long steamAccountId, string personaName, sbyte[] authTicket, int appId, int revision);
        // Java: 15
        void createUser(com.oddlabs.matchmaking.Login login_v, com.oddlabs.matchmaking.LoginDetails login_details, java.security.SignedObject reg_key, int revision);
        // Java: 17
        void setSimVersion(int sim_version);
    }
}

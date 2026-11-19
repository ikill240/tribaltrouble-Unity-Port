// Translated from RouterInterface.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.router
{
    // Java: 3
    public interface RouterInterface
    {
        public const int PORT = 11221;
        // Java: 6
        void login(com.oddlabs.router.SessionID id, com.oddlabs.router.SessionInfo info, int client_id);
        // Java: 8
        void loginSpectator(com.oddlabs.router.SessionID session_id);
    }
}

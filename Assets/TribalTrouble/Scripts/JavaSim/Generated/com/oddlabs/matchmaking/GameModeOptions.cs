// Translated from GameModeOptions.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 23
    public interface GameModeOptions
    {
        // Java: 31
        com.oddlabs.matchmaking.GameMode getMode();
    }
    public sealed class GameModeOptions_L : java.lang.Object, GameModeOptions
    {
        readonly System.Func<com.oddlabs.matchmaking.GameMode> f;
        public GameModeOptions_L(System.Func<com.oddlabs.matchmaking.GameMode> f) { this.f = f; }
        public com.oddlabs.matchmaking.GameMode getMode() { return f(); }
    }
}

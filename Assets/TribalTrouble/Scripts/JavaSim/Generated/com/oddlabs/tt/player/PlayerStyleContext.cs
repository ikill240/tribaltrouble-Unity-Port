// Translated from PlayerStyleContext.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 11
    public sealed class PlayerStyleContext : java.lang.Enum
    {
        public static PlayerStyleContext[] values() { return (PlayerStyleContext[])VALUES_.Clone(); }
        public static PlayerStyleContext valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly PlayerStyleContext LOW_AGGRESSION = new PlayerStyleContext("LOW_AGGRESSION", 0);
        public static readonly PlayerStyleContext MEDIUM_AGGRESSION = new PlayerStyleContext("MEDIUM_AGGRESSION", 1);
        public static readonly PlayerStyleContext HIGH_AGGRESSION = new PlayerStyleContext("HIGH_AGGRESSION", 2);
        public const float LOW_CEILING = 0.4f;
        public const float HIGH_FLOOR = 0.6f;
        // Java: 19
        public static com.oddlabs.tt.player.PlayerStyleContext fromAggression(float aggression)
        {
            if (aggression < com.oddlabs.tt.player.PlayerStyleContext.LOW_CEILING)
            {
                return com.oddlabs.tt.player.PlayerStyleContext.LOW_AGGRESSION;
            }
            if (aggression > com.oddlabs.tt.player.PlayerStyleContext.HIGH_FLOOR)
            {
                return com.oddlabs.tt.player.PlayerStyleContext.HIGH_AGGRESSION;
            }
            return com.oddlabs.tt.player.PlayerStyleContext.MEDIUM_AGGRESSION;
        }
        public static readonly PlayerStyleContext[] VALUES_ = new PlayerStyleContext[] { LOW_AGGRESSION, MEDIUM_AGGRESSION, HIGH_AGGRESSION };
        public PlayerStyleContext(string name_, int ordinal_) : base(name_, ordinal_)
        {
        }
    }
}

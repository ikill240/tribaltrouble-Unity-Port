// Translated from AiBehaviorProfile.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.player
{
    // Java: 9
    public sealed class AiBehaviorProfile : java.lang.Enum
    {
        public static AiBehaviorProfile[] values() { return (AiBehaviorProfile[])VALUES_.Clone(); }
        public static AiBehaviorProfile valueOf(string name) { foreach (var v in VALUES_) if (v.name() == name) return v; throw new System.ArgumentException(name); }
        public static readonly AiBehaviorProfile BALANCED = new AiBehaviorProfile("BALANCED", 0, 1.0f, 1.0f);
        public static readonly AiBehaviorProfile AGGRESSIVE = new AiBehaviorProfile("AGGRESSIVE", 1, 0.6f, 0.6f);
        public static readonly AiBehaviorProfile ECONOMIC = new AiBehaviorProfile("ECONOMIC", 2, 1.5f, 1.5f);
        public static readonly AiBehaviorProfile HARASSER = new AiBehaviorProfile("HARASSER", 3, 0.75f, 0.4f);
        public float stall_multiplier;
        public float min_warriors_multiplier;
        // Java: 35
        public float getStallMultiplier()
        {
            return stall_multiplier;
        }
        // Java: 39
        public float getMinWarriorsMultiplier()
        {
            return min_warriors_multiplier;
        }
        public static readonly AiBehaviorProfile[] VALUES_ = new AiBehaviorProfile[] { BALANCED, AGGRESSIVE, ECONOMIC, HARASSER };
        public AiBehaviorProfile(string name_, int ordinal_, float stall_multiplier, float min_warriors_multiplier) : base(name_, ordinal_)
        {
            this.stall_multiplier = stall_multiplier;
            this.min_warriors_multiplier = min_warriors_multiplier;
        }
    }
}

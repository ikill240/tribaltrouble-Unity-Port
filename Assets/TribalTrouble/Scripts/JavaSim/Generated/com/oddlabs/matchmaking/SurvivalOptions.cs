// Translated from SurvivalOptions.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 15
    public class SurvivalOptions : java.lang.Object, com.oddlabs.matchmaking.GameModeOptions
    {
        public const long serialVersionUID = 1L;
        public float wave_interval_seconds;
        public float growth_per_wave;
        // Java: 28
        public static com.oddlabs.matchmaking.SurvivalOptions.Builder builder()
        {
            return new com.oddlabs.matchmaking.SurvivalOptions.Builder();
        }
        // Java: 32
        public static com.oddlabs.matchmaking.SurvivalOptions defaults()
        {
            return new com.oddlabs.matchmaking.SurvivalOptions.Builder().build();
        }
        // Java: 36
        public com.oddlabs.matchmaking.GameMode getMode()
        {
            return com.oddlabs.matchmaking.GameMode.SURVIVAL;
        }
        // Java: 41
        public float getWaveIntervalSeconds()
        {
            return wave_interval_seconds;
        }
        // Java: 45
        public float getGrowthPerWave()
        {
            return growth_per_wave;
        }
        // Java: 49
        public class Builder : java.lang.Object
        {
            public float wave_interval_seconds;
            public float growth_per_wave;
            // Java: 58
            public com.oddlabs.matchmaking.SurvivalOptions.Builder waveIntervalSeconds(float v)
            {
                this.wave_interval_seconds = v;
                return this;
            }
            // Java: 63
            public com.oddlabs.matchmaking.SurvivalOptions.Builder growthPerWave(float v)
            {
                this.growth_per_wave = v;
                return this;
            }
            // Java: 68
            public com.oddlabs.matchmaking.SurvivalOptions build()
            {
                return new com.oddlabs.matchmaking.SurvivalOptions(this);
            }
            private void init_Builder()
            {
                this.wave_interval_seconds = 45.0f;
                this.growth_per_wave = 2.0f;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public SurvivalOptions(com.oddlabs.matchmaking.SurvivalOptions.Builder b) : base()
        {
            this.wave_interval_seconds = b.wave_interval_seconds;
            this.growth_per_wave = b.growth_per_wave;
        }
    }
}

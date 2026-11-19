// Translated from KingOfTheIslandOptions.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 10
    public class KingOfTheIslandOptions : java.lang.Object, com.oddlabs.matchmaking.GameModeOptions
    {
        public const long serialVersionUID = 1L;
        public float capture_radius;
        public float hold_seconds_to_win;
        // Java: 23
        public static com.oddlabs.matchmaking.KingOfTheIslandOptions.Builder builder()
        {
            return new com.oddlabs.matchmaking.KingOfTheIslandOptions.Builder();
        }
        // Java: 27
        public static com.oddlabs.matchmaking.KingOfTheIslandOptions defaults()
        {
            return new com.oddlabs.matchmaking.KingOfTheIslandOptions.Builder().build();
        }
        // Java: 31
        public com.oddlabs.matchmaking.GameMode getMode()
        {
            return com.oddlabs.matchmaking.GameMode.KING_OF_THE_ISLAND;
        }
        // Java: 36
        public float getCaptureRadius()
        {
            return capture_radius;
        }
        // Java: 40
        public float getHoldSecondsToWin()
        {
            return hold_seconds_to_win;
        }
        // Java: 44
        public class Builder : java.lang.Object
        {
            public float capture_radius;
            public float hold_seconds_to_win;
            // Java: 53
            public com.oddlabs.matchmaking.KingOfTheIslandOptions.Builder captureRadius(float v)
            {
                this.capture_radius = v;
                return this;
            }
            // Java: 58
            public com.oddlabs.matchmaking.KingOfTheIslandOptions.Builder holdSecondsToWin(float v)
            {
                this.hold_seconds_to_win = v;
                return this;
            }
            // Java: 63
            public com.oddlabs.matchmaking.KingOfTheIslandOptions build()
            {
                return new com.oddlabs.matchmaking.KingOfTheIslandOptions(this);
            }
            private void init_Builder()
            {
                this.capture_radius = 15.0f;
                this.hold_seconds_to_win = 120.0f;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public KingOfTheIslandOptions(com.oddlabs.matchmaking.KingOfTheIslandOptions.Builder b) : base()
        {
            this.capture_radius = b.capture_radius;
            this.hold_seconds_to_win = b.hold_seconds_to_win;
        }
    }
}

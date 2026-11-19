// Translated from StandardOptions.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 9
    public class StandardOptions : java.lang.Object, com.oddlabs.matchmaking.GameModeOptions
    {
        public const long serialVersionUID = 1L;
        public bool rated;
        // Java: 20
        public static com.oddlabs.matchmaking.StandardOptions.Builder builder()
        {
            return new com.oddlabs.matchmaking.StandardOptions.Builder();
        }
        // Java: 24
        public static com.oddlabs.matchmaking.StandardOptions defaults()
        {
            return new com.oddlabs.matchmaking.StandardOptions.Builder().build();
        }
        // Java: 28
        public com.oddlabs.matchmaking.GameMode getMode()
        {
            return com.oddlabs.matchmaking.GameMode.STANDARD;
        }
        // Java: 33
        public bool isRated()
        {
            return rated;
        }
        // Java: 37
        public class Builder : java.lang.Object
        {
            public bool rated_f;
            // Java: 44
            public com.oddlabs.matchmaking.StandardOptions.Builder rated(bool rated_v)
            {
                this.rated_f = rated_v;
                return this;
            }
            // Java: 49
            public com.oddlabs.matchmaking.StandardOptions build()
            {
                return new com.oddlabs.matchmaking.StandardOptions(this);
            }
            private void init_Builder()
            {
                this.rated_f = false;
            }
            public Builder() : base()
            {
                init_Builder();
            }
        }
        public StandardOptions(com.oddlabs.matchmaking.StandardOptions.Builder b) : base()
        {
            this.rated = b.rated_f;
        }
    }
}

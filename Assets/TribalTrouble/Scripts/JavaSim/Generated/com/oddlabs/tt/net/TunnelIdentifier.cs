// Translated from TunnelIdentifier.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.net
{
    // Java: 7
    public class TunnelIdentifier : java.lang.Object
    {
        public com.oddlabs.matchmaking.Profile profile() { return profile_f; }
        public com.oddlabs.matchmaking.TunnelAddress address() { return address_f; }
        public TunnelIdentifier(com.oddlabs.matchmaking.Profile profile, com.oddlabs.matchmaking.TunnelAddress address)
        {
            this.profile_f = profile;
            this.address_f = address;
        }
        public override bool equals(object o) { if (ReferenceEquals(this, o)) return true; if (!(o is TunnelIdentifier r)) return false; return true && java.lang.JO.equals(profile_f, r.profile_f) && java.lang.JO.equals(address_f, r.address_f); }
        public override int hashCode() { int h = 0; unchecked { h = 31 * h + java.lang.JO.hashCode(profile_f); h = 31 * h + java.lang.JO.hashCode(address_f); } return h; }
        public com.oddlabs.matchmaking.Profile profile_f;
        public com.oddlabs.matchmaking.TunnelAddress address_f;
        // Java: 10
        public override string toString()
        {
            return "profile: " + java.lang.JStr.valueOf(profile_f) + " tunnel address: " + java.lang.JStr.valueOf(address_f);
        }
    }
}

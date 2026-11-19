// Translated from TunnelAddress.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.matchmaking
{
    // Java: 9
    public class TunnelAddress : java.lang.Object
    {
        public const long serialVersionUID = (-2854382209354714233L);
        public int host_id;
        public java.net.InetAddress address;
        public java.net.InetAddress local_address;
        // Java: 22
        public int getHostID()
        {
            return host_id;
        }
        // Java: 26
        public java.net.InetAddress getAddress()
        {
            return address;
        }
        // Java: 30
        public java.net.InetAddress getLocalAddress()
        {
            return local_address;
        }
        // Java: 34
        public override string toString()
        {
            return "host id = " + (host_id) + " address = " + java.lang.JStr.valueOf(address) + " local_address = " + java.lang.JStr.valueOf(local_address);
        }
        public TunnelAddress(int host_id, java.net.InetAddress address, java.net.InetAddress local_address) : base()
        {
            this.host_id = host_id;
            this.address = address;
            this.local_address = local_address;
        }
    }
}

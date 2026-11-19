// Translated from DNSTask.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 9
    public class DNSTask : java.lang.Object, com.oddlabs.net.Callable
    {
        public string dns_name;
        public int port;
        public com.oddlabs.net.Connection connection;
        // Java: 20
        public void taskCompleted(object result_p)
        {
            java.net.InetSocketAddress result = (java.net.InetSocketAddress)result_p;
            connection.connect(result);
        }
        // Java: 25
        public void taskFailed(System.Exception e)
        {
            connection.dnsError(((System.IO.IOException)e));
        }
        // Java: 31
        public object call()
        {
            java.net.InetAddress inet_address = java.net.InetAddress.getByName(dns_name);
            java.net.InetSocketAddress address = new java.net.InetSocketAddress(inet_address, port);
            return address;
        }
        public DNSTask(string dns_name, int port, com.oddlabs.net.Connection conn) : base()
        {
            this.dns_name = dns_name;
            this.port = port;
            this.connection = conn;
        }
    }
}

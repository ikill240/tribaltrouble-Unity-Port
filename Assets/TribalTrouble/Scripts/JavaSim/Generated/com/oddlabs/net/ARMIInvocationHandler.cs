// Translated from ARMIInvocationHandler.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 9
    public class ARMIInvocationHandler : java.lang.Object, java.lang.reflect.InvocationHandler
    {
        public com.oddlabs.net.ARMIInterfaceMethods armi_interface_methods;
        public com.oddlabs.net.ARMIEventWriter broker;
        public com.oddlabs.net.ARMIArgumentWriter writer;
        // Java: 21
        public object invoke(object proxy, java.lang.reflect.Method method, object[] args)
        {
            sbyte method_id = armi_interface_methods.getMethodIndex(method);
            broker.handle(new com.oddlabs.net.ARMIEvent(writer, method.getParameterTypes(), method_id, args));
            return null;
        }
        public ARMIInvocationHandler(com.oddlabs.net.ARMIEventWriter broker, com.oddlabs.net.ARMIArgumentWriter writer, com.oddlabs.net.ARMIInterfaceMethods armi_interface_methods) : base()
        {
            this.broker = broker;
            this.writer = writer;
            this.armi_interface_methods = armi_interface_methods;
        }
    }
}

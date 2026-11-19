// Translated from ARMIEvent.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 14
    public class ARMIEvent : java.lang.Object
    {
        public const long serialVersionUID = 1L;
        public static com.oddlabs.util.ByteBufferOutputStream static_byte_stream;
        public const short HEADER_SIZE = unchecked((short)1);
        /// <summary>(Steam fix) the biggest command stream the 16-bit wire size can carry.</summary>
        public const int MAX_COMMAND_STREAM_SIZE = 32766;
        public static com.oddlabs.net.ARMIArgumentWriter default_writer;
        public static com.oddlabs.net.ARMIArgumentReader default_reader;
        public sbyte method_id;
        public sbyte[] command_stream;
        // Java: 26
        public static object createProxy(com.oddlabs.net.ARMIEventWriter broker, System.Type armi_interface)
        {
            return com.oddlabs.net.ARMIEvent.createProxy(broker, com.oddlabs.net.ARMIEvent.default_writer, armi_interface);
        }
        // Java: 30
        public static object createProxy(com.oddlabs.net.ARMIEventWriter broker, com.oddlabs.net.ARMIArgumentWriter writer, System.Type armi_interface)
        {
            com.oddlabs.net.ARMIInterfaceMethods armi_interface_methods = new com.oddlabs.net.ARMIInterfaceMethods(armi_interface);
            com.oddlabs.net.ARMIInvocationHandler handler = new com.oddlabs.net.ARMIInvocationHandler(broker, writer, armi_interface_methods);
            return java.lang.reflect.Proxy.newProxyInstance(typeof(com.oddlabs.net.ARMIEvent).getClassLoader(), new System.Type[] { armi_interface }, handler);
        }
        // Java: 37
        public short getEventSize()
        {
            int command_stream_length = (command_stream != null ? command_stream.Length : 0);
            return unchecked((short)((com.oddlabs.net.ARMIEvent.HEADER_SIZE + command_stream_length)));
        }
        // Java: 42
        public void write(java.nio.ByteBuffer buffer)
        {
            buffer.put(method_id);
            if (command_stream != null)
            {
                buffer.put(command_stream);
            }
        }
        // Java: 48
        public static com.oddlabs.net.ARMIEvent read(java.nio.ByteBuffer buffer, short size)
        {
            sbyte method_id = buffer.get();
            int stream_length = size - com.oddlabs.net.ARMIEvent.HEADER_SIZE;
            sbyte[] command_stream = default;
            if (stream_length > 0)
            {
                command_stream = new sbyte[stream_length];
                buffer.get(command_stream);
            }
            else
            {
                command_stream = null;
            }
            return new com.oddlabs.net.ARMIEvent(method_id, command_stream);
        }
        // Java: 60
        public static sbyte[] createByteArrayFromCommand(com.oddlabs.net.ARMIArgumentWriter writer, System.Type[] method_parameter_types, object[] args)
        {
            if (args != null)
            {
                try
                {
                    com.oddlabs.net.ARMIEvent.static_byte_stream.reset();
                    for (int i = 0; i < args.Length; i++)
                    {
                        object arg = args[i];
                        System.Type type = method_parameter_types[i];
                        writer.writeArgument(type, arg, com.oddlabs.net.ARMIEvent.static_byte_stream);
                    }
                    return com.oddlabs.net.ARMIEvent.static_byte_stream.toByteArray();
                }
                catch (System.IO.IOException e)
                {
                    throw new System.Exception(java.lang.JStr.valueOf(e), (System.Exception)e);
                }
            }
            else
            {
                return null;
            }
        }
        // Java: 88
        public object[] parseArgs(com.oddlabs.net.ARMIArgumentReader reader, java.lang.reflect.Method method)
        {
            System.Type[] parameter_types = method.getParameterTypes();
            int num_params = parameter_types.Length;
            if (num_params == 0)
            {
                return null;
            }
            if (command_stream == null)
            {
                throw new System.IO.IOException("ARMI event for " + java.lang.JStr.valueOf(method) + " expects " + (num_params) + " argument(s) but has no argument data (command_stream is null)");
            }
            object[] args = default;
            args = new object[num_params];
            com.oddlabs.net.ByteBufferInputStream byte_stream = new com.oddlabs.net.ByteBufferInputStream(command_stream);
            for (int i = 0; i < args.Length; i++)
            {
                System.Type type = parameter_types[i];
                args[i] = reader.readArgument(type, byte_stream);
            }
            return args;
        }
        // Java: 119
        public void execute(com.oddlabs.net.ARMIInterfaceMethods interface_methods, object instance)
        {
            execute(interface_methods, com.oddlabs.net.ARMIEvent.default_reader, instance);
        }
        // Java: 124
        public void execute(com.oddlabs.net.ARMIInterfaceMethods interface_methods, com.oddlabs.net.ARMIArgumentReader reader, object instance)
        {
            java.lang.reflect.Method method = default;
            try
            {
                method = interface_methods.getMethod(method_id);
            }
            catch (System.IndexOutOfRangeException e)
            {
                throw new com.oddlabs.net.IllegalARMIEventException(e);
            }
            object[] args = default;
            try
            {
                args = parseArgs(reader, method);
            }
            catch (System.Exception e_2)
            {
                throw new com.oddlabs.net.IllegalARMIEventException(e_2);
            }
            interface_methods.invoke(instance, method, args);
        }
        static ARMIEvent()
        {
            ARMIEvent.static_byte_stream = new com.oddlabs.util.ByteBufferOutputStream(false);
            ARMIEvent.default_writer = new com.oddlabs.net.DefaultARMIArgumentWriter();
            ARMIEvent.default_reader = new com.oddlabs.net.DefaultARMIArgumentReader();
        }
        public ARMIEvent(com.oddlabs.net.ARMIArgumentWriter writer, System.Type[] method_parameter_types, sbyte method_id, object[] args) : this(method_id, com.oddlabs.net.ARMIEvent.createByteArrayFromCommand(writer, method_parameter_types, args))
        {
        }
        public ARMIEvent(sbyte method_id, sbyte[] command_stream) : base()
        {
            if (command_stream != null && command_stream.Length > com.oddlabs.net.ARMIEvent.MAX_COMMAND_STREAM_SIZE)
            {
                throw new System.ArgumentException("ARMI event too large: " + (command_stream.Length) + " bytes");
            }
            this.method_id = method_id;
            this.command_stream = command_stream;
        }
    }
}

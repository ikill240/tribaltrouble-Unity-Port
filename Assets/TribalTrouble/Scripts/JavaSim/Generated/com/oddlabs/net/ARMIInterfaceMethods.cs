// Translated from ARMIInterfaceMethods.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.net
{
    // Java: 9
    public class ARMIInterfaceMethods : java.lang.Object
    {
        public System.Type armi_interface;
        public java.lang.reflect.Method[] methods;
        // Java: 23
        public bool isLegal(java.lang.reflect.Method method)
        {
            return java.lang.JO.equals(method.getReturnType(), typeof(void)) && method.getExceptionTypes().Length == 0;
        }
        // Java: 27
        public bool isInstance(object instance)
        {
            return armi_interface.isInstance(instance);
        }
        // Java: 31
        public System.Type getInterfaceClass()
        {
            return armi_interface;
        }
        // Java: 36
        public void invoke(object instance, java.lang.reflect.Method method, object[] args)
        {
            try
            {
                method.invoke(instance, args);
            }
            catch (System.Exception e) when (e is System.Exception || e is System.ArgumentException)
            {
                throw new com.oddlabs.net.IllegalARMIEventException(e);
            }
            catch (System.Exception e_2)
            {
                throw new com.oddlabs.net.ARMIHandlerException((e_2.getCause() != null ? e_2.getCause() : e_2));
            }
        }
        // Java: 49
        public java.lang.reflect.Method getMethod(sbyte index)
        {
            return methods[index];
        }
        // Java: 53
        public sbyte getMethodIndex(java.lang.reflect.Method method)
        {
            for (sbyte i = unchecked((sbyte)(0)); i < methods.Length; i++)
            {
                if (java.lang.JO.equals(methods[i], method))
                {
                    return i;
                }
            }
            throw new System.Exception("Unknown method: " + java.lang.JStr.valueOf(method));
        }
        public ARMIInterfaceMethods(System.Type armi_interface) : base()
        {
            this.armi_interface = armi_interface;
            this.methods = armi_interface.getMethods();
            java.util.Arrays.sort(methods, new com.oddlabs.net.MethodComparator());
            foreach (java.lang.reflect.Method method in methods)
            {
            }
        }
    }
}

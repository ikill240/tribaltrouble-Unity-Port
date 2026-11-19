// Translated from Resources.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.resource
{
    // Java: 12
    public class Resources : java.lang.Object
    {
        public static java.util.concurrent.ConcurrentMap LOADED_RESOURCES;
        // Java: 16
        public static object findResource(java.util.function.Supplier resSupplier)
        {
            object resource = com.oddlabs.tt.resource.Resources.LOADED_RESOURCES.get(resSupplier);
            if (resource == null)
            {
                resource = resSupplier.get();
                object existing = com.oddlabs.tt.resource.Resources.LOADED_RESOURCES.putIfAbsent(resSupplier, resource);
                if (existing != null)
                {
                    resource = existing;
                }
            }
            return ((object)resource);
        }
        // Java: 34
        public static void clearResources()
        {
            foreach (object resource in java.lang.JIter.of(com.oddlabs.tt.resource.Resources.LOADED_RESOURCES.values()))
            {
                com.oddlabs.tt.resource.Resources.closeResource(resource);
            }
            com.oddlabs.tt.resource.Resources.LOADED_RESOURCES.clear();
        }
        // Java: 41
        public static void closeResource(object resource)
        {
            if ((resource is java.lang.AutoCloseable closeable))
            {
                try
                {
                    closeable.close();
                }
                catch (System.Exception e)
                {
                    e.printStackTrace();
                }
            }
            else if ((resource is object[] array))
            {
                foreach (object obj in array)
                {
                    com.oddlabs.tt.resource.Resources.closeResource(obj);
                }
            }
        }
        static Resources()
        {
            Resources.LOADED_RESOURCES = new java.util.concurrent.ConcurrentHashMap();
        }
        public Resources() : base()
        {
        }
    }
}

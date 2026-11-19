// Translated from SupplyContainerFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public abstract class SupplyContainerFactory : java.lang.Object
    {
        public int max_resource_count;
        // Java: 10
        public abstract com.oddlabs.tt.model.SupplyContainer createContainer(com.oddlabs.tt.model.Selectable selectable);
        // Java: 12
        public int getMaxResourceCount()
        {
            return max_resource_count;
        }
        public SupplyContainerFactory(int max_resource_count) : base()
        {
            this.max_resource_count = max_resource_count;
        }
    }
}

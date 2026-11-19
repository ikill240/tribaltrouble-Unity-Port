// Translated from UnitSupplyContainerFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 8
    public class UnitSupplyContainerFactory : com.oddlabs.tt.model.SupplyContainerFactory
    {
        public java.util.Map supply_sprite_lists;
        // Java: 17
        public override com.oddlabs.tt.model.SupplyContainer createContainer(com.oddlabs.tt.model.Selectable selectable)
        {
            return new com.oddlabs.tt.model.UnitSupplyContainer(getMaxResourceCount(), supply_sprite_lists);
        }
        public UnitSupplyContainerFactory(int max_resource_count, java.util.Map supply_sprite_lists) : base(max_resource_count)
        {
            this.supply_sprite_lists = supply_sprite_lists;
        }
    }
}

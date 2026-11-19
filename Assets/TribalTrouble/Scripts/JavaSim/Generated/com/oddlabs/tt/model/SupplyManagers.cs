// Translated from SupplyManagers.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 10
    public class SupplyManagers : java.lang.Object
    {
        public java.util.Map supply_managers;
        // Java: 13
        public void debugSpawn()
        {
            supply_managers.values().forEach(new java.util.function.Consumer_L((object a0) => { ((com.oddlabs.tt.model.SupplyManager)a0).debugSpawnSupply(); }));
        }
        // Java: 30
        public com.oddlabs.tt.model.SupplyManager getSupplyManager(System.Type type)
        {
            return ((com.oddlabs.tt.model.SupplyManager)supply_managers.get(type));
        }
        public SupplyManagers(com.oddlabs.tt.landscape.World world) : base()
        {
            java.util.Map managers = new java.util.HashMap(java.util.Map_.of(typeof(com.oddlabs.tt.landscape.TreeSupply), new com.oddlabs.tt.model.SupplyManager(world), typeof(com.oddlabs.tt.model.RockSupply), new com.oddlabs.tt.model.SupplyManager(world), typeof(com.oddlabs.tt.model.IronSupply), new com.oddlabs.tt.model.SupplyManager(world), typeof(com.oddlabs.tt.model.RubberSupply), new com.oddlabs.tt.model.RubberSupplyManager(world)));
            if (world.isExtended())
            {
                foreach (System.Type rare in com.oddlabs.tt.model.ExtendedResources.RARE)
                {
                    managers.put(rare, new com.oddlabs.tt.model.SupplyManager(world));
                }
            }
            supply_managers = managers;
        }
    }
}

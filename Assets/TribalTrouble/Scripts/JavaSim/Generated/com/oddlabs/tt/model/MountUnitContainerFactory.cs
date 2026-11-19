// Translated from MountUnitContainerFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 5
    public class MountUnitContainerFactory : java.lang.Object, com.oddlabs.tt.model.UnitContainerFactory
    {
        // Java: 6
        public com.oddlabs.tt.model.UnitContainer createContainer(com.oddlabs.tt.model.Building building)
        {
            return new com.oddlabs.tt.model.MountUnitContainer(building);
        }
        public MountUnitContainerFactory() : base()
        {
        }
    }
}

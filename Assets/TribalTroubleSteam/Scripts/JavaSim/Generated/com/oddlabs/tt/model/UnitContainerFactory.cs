// Translated from UnitContainerFactory.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 3
    public interface UnitContainerFactory
    {
        // Java: 4
        com.oddlabs.tt.model.UnitContainer createContainer(com.oddlabs.tt.model.Building building);
    }
    public sealed class UnitContainerFactory_L : java.lang.Object, UnitContainerFactory
    {
        readonly System.Func<com.oddlabs.tt.model.Building, com.oddlabs.tt.model.UnitContainer> f;
        public UnitContainerFactory_L(System.Func<com.oddlabs.tt.model.Building, com.oddlabs.tt.model.UnitContainer> f) { this.f = f; }
        public com.oddlabs.tt.model.UnitContainer createContainer(com.oddlabs.tt.model.Building p0) { return f(p0); }
    }
}

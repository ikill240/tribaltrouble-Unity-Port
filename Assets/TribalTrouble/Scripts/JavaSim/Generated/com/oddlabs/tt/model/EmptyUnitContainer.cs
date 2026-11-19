// Translated from EmptyUnitContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 7
    public class EmptyUnitContainer : com.oddlabs.tt.model.UnitContainer
    {
        // Java: 12
        public override void enter(com.oddlabs.tt.model.Unit unit)
        {
            throw new System.InvalidOperationException("nobody can enter this building");
        }
        // Java: 17
        public override bool canEnter(com.oddlabs.tt.model.Unit unit)
        {
            return false;
        }
        // Java: 22
        public override com.oddlabs.tt.model.Unit exit()
        {
            return null;
        }
        public EmptyUnitContainer() : base(0)
        {
        }
    }
}

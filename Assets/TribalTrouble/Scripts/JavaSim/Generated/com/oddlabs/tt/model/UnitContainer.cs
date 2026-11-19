// Translated from UnitContainer.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.model
{
    // Java: 6
    public abstract class UnitContainer : com.oddlabs.tt.model.SupplyContainer
    {
        // Java: 11
        public abstract void enter(com.oddlabs.tt.model.Unit unit);
        // Java: 13
        public abstract bool canEnter(com.oddlabs.tt.model.Unit unit);
        // Java: 15
        public abstract com.oddlabs.tt.model.Unit exit();
        // Java: 17
        public virtual void animate(float t)
        {
        }
        public UnitContainer(int capacity) : base(capacity)
        {
        }
    }
}
